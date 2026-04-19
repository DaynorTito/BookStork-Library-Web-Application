/**
 * ETL - Google Books API → bookstork_dev MySQL
 *
 * Usage:
 *   node etl.js
 *
 * Deps:  npm install mysql2 axios uuid
 */

const mysql = require('mysql2/promise');
const axios = require('axios');
const { v4: uuidv4 } = require('uuid');

// ─── CONFIG ──────────────────────────────────────────────────────────────────

const API_KEY  = 'AIzaSyAVpR8HQgHIDjLE4qmisexPZGQJNxDXJg0';
const BASE_URL = 'https://www.googleapis.com/books/v1/volumes';
const TARGET   = 500;   // books to insert

const DB = {
  host    : 'roundhouse.proxy.rlwy.net',
  port    : 35873,
  database: 'railway',
  user    : 'root',
  password: 'qoZbsldXGaNBNJSbLWwEccNYVKedgRIE',
};

/**
 * Diverse queries → variety of genres / authors / languages
 * Each fetches up to 40 results (Google's per-request max).
 */
const SEARCH_QUERIES = [
  // Programming / Tech
  'python machine learning',
  'clean code software engineering',
  'database design sql',
  'linux unix systems',
  // Science
  'quantum physics',
  'evolutionary biology darwin',
  'astronomy cosmos',
  'chemistry organic',
  'mathematics proofs',
  // History & Politics
  'world war ii history',
  'ancient rome civilization',
  'american revolution history',
  'cold war espionage',
  'latin america history',
  // Philosophy
  'stoicism marcus aurelius',
  'existentialism philosophy',
  'ethics morality philosophy',
  'buddhism mindfulness',
  'political philosophy democracy',
  // Fiction Genres
  'epic fantasy tolkien',
  'science fiction asimov',
  'mystery detective thriller',
  'romance literary fiction',
  'horror stephen king',
  'historical fiction medieval',
  'dystopian fiction novel',
  'classic literature shakespeare',
  'magical realism garcia marquez',
  // Self Help / Psychology
  'psychology behavior cognitive',
  'self help productivity habits',
  'leadership management business',
  'economics finance investing',
  'entrepreneurship startup',
  // Art & Culture
  'art history painting',
  'music theory composition',
  'cinema film studies',
  'architecture design urbanism',
  'photography visual arts',
  // Health & Lifestyle
  'nutrition medicine health',
  'fitness exercise sports',
  'cooking recipes culinary',
  'travel adventure memoir',
  'parenting children education',
  // Other
  'law justice society',
  'environment climate change',
  'biography autobiography',
  'poetry anthology',
  'graphic novel comics',
];

// ─── HELPERS ─────────────────────────────────────────────────────────────────

const sleep = ms => new Promise(r => setTimeout(r, ms));

/** Parse a Google date string like "2010", "2010-05", "2010-05-21" → "YYYY-MM-DD" */
function parseDate(raw) {
  if (!raw) return null;
  const m = raw.match(/^(\d{4})(?:-(\d{2}))?(?:-(\d{2}))?/);
  if (!m) return null;
  return `${m[1]}-${m[2] || '01'}-${m[3] || '01'}`;
}

/** Pick first ISBN_13, fallback ISBN_10 */
function extractISBN(identifiers = []) {
  return (
    identifiers.find(i => i.type === 'ISBN_13')?.identifier ||
    identifiers.find(i => i.type === 'ISBN_10')?.identifier ||
    null
  );
}

/** Normalize a Google Books category string to a clean name */
function normalizeCategory(raw) {
  if (!raw) return 'General';
  return raw.split('/')[0].trim().substring(0, 100) || 'General';
}

/**
 * Extract up to 4 genre names from volumeInfo.categories.
 * Each category string can be "Fiction / Science Fiction" → ["Fiction", "Science Fiction"]
 */
function extractGenres(categories = []) {
  const seen = new Set();
  for (const cat of categories) {
    for (const part of cat.split('/')) {
      const g = part.trim().substring(0, 100);
      if (g) seen.add(g);
    }
  }
  return [...seen].slice(0, 4);
}

/** MySQL datetime(6) format */
function nowSql() {
  return new Date().toISOString().replace('T', ' ').replace('Z', '').padEnd(26, '0');
}

// ─── GOOGLE BOOKS FETCH ───────────────────────────────────────────────────────

async function fetchVolumes(query, startIndex = 0) {
  try {
    const { data } = await axios.get(BASE_URL, {
      params: {
        q         : query,
        maxResults: 40,
        startIndex,
        printType : 'books',
        key       : API_KEY,
      },
      timeout: 12000,
    });
    return data.items || [];
  } catch (err) {
    console.warn(`  ⚠ Fetch failed for "${query}" (start=${startIndex}): ${err.message}`);
    return [];
  }
}

// ─── DB UPSERT HELPERS ───────────────────────────────────────────────────────

async function getOrCreateCategory(conn, cache, name) {
  if (cache.has(name)) return cache.get(name);
  const id = uuidv4();
  await conn.execute(
    'INSERT IGNORE INTO Categories (Id, Name, Description) VALUES (?, ?, NULL)',
    [id, name],
  );
  // Re-read in case IGNORE swallowed a duplicate from a parallel run
  const [[row]] = await conn.execute(
    'SELECT Id FROM Categories WHERE Name = ?', [name],
  );
  cache.set(name, row.Id);
  return row.Id;
}

async function getOrCreateGenre(conn, cache, name) {
  if (cache.has(name)) return cache.get(name);
  const id = uuidv4();
  await conn.execute(
    'INSERT IGNORE INTO Genres (Id, Name) VALUES (?, ?)',
    [id, name],
  );
  const [[row]] = await conn.execute(
    'SELECT Id FROM Genres WHERE Name = ?', [name],
  );
  cache.set(name, row.Id);
  return row.Id;
}

async function getOrCreateAuthor(conn, cache, name) {
  if (cache.has(name)) return cache.get(name);
  const id = uuidv4();
  await conn.execute(
    'INSERT IGNORE INTO Authors (Id, Name, Biography) VALUES (?, ?, NULL)',
    [id, name],
  );
  const [[row]] = await conn.execute(
    'SELECT Id FROM Authors WHERE Name = ?', [name],
  );
  cache.set(name, row.Id);
  return row.Id;
}

// ─── PROCESS A SINGLE VOLUME ─────────────────────────────────────────────────

async function processVolume(conn, caches, item) {
  const { categoryCache, genreCache, authorCache, isbnSet } = caches;
  const v = item?.volumeInfo;
  if (!v) return false;

  // ── Mandatory fields ──────────────────────────────────────────────────────
  const description = v.description?.trim();
  if (!description || description.length < 30) return false;   // must have description

  const isbn = extractISBN(v.industryIdentifiers);
  if (!isbn) return false;
  if (isbnSet.has(isbn)) return false;   // already processed (or in DB)

  const title = v.title?.trim().substring(0, 200);
  if (!title) return false;

  const publishedDate = parseDate(v.publishedDate);
  if (!publishedDate) return false;

  // ── Optional / defaulted fields ───────────────────────────────────────────
  const publisher    = (v.publisher || 'Unknown').substring(0, 200);
  const pageCount    = v.pageCount  || 100;
  const language     = (v.language  || 'en').substring(0, 10);
  const avgRating    = Math.min(parseFloat(v.averageRating) || 0, 5.00);
  const categories   = v.categories || [];
  const authorNames  = (v.authors   || ['Unknown']).map(a => a.trim().substring(0, 200));
  const imageLinks   = v.imageLinks || {};

  // ── Category ──────────────────────────────────────────────────────────────
  const catName  = normalizeCategory(categories[0]);
  const catId    = await getOrCreateCategory(conn, categoryCache, catName);

  // ── Genres ────────────────────────────────────────────────────────────────
  const genreNames = extractGenres(categories);
  const genreIds   = await Promise.all(
    genreNames.map(g => getOrCreateGenre(conn, genreCache, g)),
  );

  // ── Authors ───────────────────────────────────────────────────────────────
  const authorIds = await Promise.all(
    authorNames.map(a => getOrCreateAuthor(conn, authorCache, a)),
  );

  // ── Book insert ───────────────────────────────────────────────────────────
  const bookId = uuidv4();
  const now    = nowSql();

  try {
    await conn.execute(
      `INSERT INTO Books
         (Id, ISBN, Title, CategoryId, Publisher, PublishedDate, Description,
          PageCount, Height, Weight, Thickness, Language,
          AverageRating, Status, TotalCopies, AvailableCopies, CreatedAt, UpdatedAt)
       VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
      [
        bookId,
        isbn.substring(0, 20),
        title,
        catId,
        publisher,
        publishedDate,
        description,
        pageCount,
        21.00,   // height cm – not provided by Google Books
        0.40,    // weight kg
        1.20,    // thickness cm
        language,
        avgRating,
        'Available',
        5,       // TotalCopies
        5,       // AvailableCopies
        now,
        null,
      ],
    );
  } catch (err) {
    // Duplicate ISBN or other constraint – skip silently
    if (err.code === 'ER_DUP_ENTRY') return false;
    throw err;
  }

  // ── Relations ─────────────────────────────────────────────────────────────
  for (const aId of authorIds) {
    await conn.execute(
      'INSERT IGNORE INTO BookAuthors (BookId, AuthorId) VALUES (?, ?)',
      [bookId, aId],
    );
  }
  for (const gId of genreIds) {
    await conn.execute(
      'INSERT IGNORE INTO BookGenres (BookId, GenreId) VALUES (?, ?)',
      [bookId, gId],
    );
  }

  // ── Book image ────────────────────────────────────────────────────────────
  const imageUrl = imageLinks.thumbnail || imageLinks.smallThumbnail;
  if (imageUrl) {
    await conn.execute(
      'INSERT INTO BookImages (Id, BookId, Url, IsPrimary) VALUES (?, ?, ?, 1)',
      [uuidv4(), bookId, imageUrl.substring(0, 500)],
    );
  }

  isbnSet.add(isbn);
  return true;
}

// ─── MAIN ─────────────────────────────────────────────────────────────────────

async function main() {
  console.log('╔══════════════════════════════════════════╗');
  console.log('║     ETL: Google Books → bookstork_dev    ║');
  console.log('╚══════════════════════════════════════════╝\n');

  const conn = await mysql.createConnection(DB);
  console.log('✔ Connected to MySQL\n');

  // ── Prime caches from existing DB data ────────────────────────────────────
  const categoryCache = new Map();
  const genreCache    = new Map();
  const authorCache   = new Map();
  const isbnSet       = new Set();

  for (const [Id, Name] of await conn.execute('SELECT Id, Name FROM Categories').then(([r]) => r.map(x => [x.Id, x.Name])))
    categoryCache.set(Name, Id);

  for (const [Id, Name] of await conn.execute('SELECT Id, Name FROM Genres').then(([r]) => r.map(x => [x.Id, x.Name])))
    genreCache.set(Name, Id);

  for (const [Id, Name] of await conn.execute('SELECT Id, Name FROM Authors').then(([r]) => r.map(x => [x.Id, x.Name])))
    authorCache.set(Name, Id);

  for (const { ISBN } of await conn.execute('SELECT ISBN FROM Books').then(([r]) => r))
    isbnSet.add(ISBN);

  console.log(`  Pre-loaded: ${categoryCache.size} categories, ${genreCache.size} genres, ${authorCache.size} authors, ${isbnSet.size} ISBNs\n`);

  const caches = { categoryCache, genreCache, authorCache, isbnSet };

  let inserted = 0;
  let attempted = 0;

  // ── Main query loop ───────────────────────────────────────────────────────
  outer: for (const query of SEARCH_QUERIES) {
    if (inserted >= TARGET) break;

    console.log(`\n🔍  Searching: "${query}"`);

    // Google allows startIndex up to 80 (2 pages of 40)
    for (const startIndex of [0, 40]) {
      if (inserted >= TARGET) break outer;

      const items = await fetchVolumes(query, startIndex);
      console.log(`    → ${items.length} results (offset ${startIndex})`);

      for (const item of items) {
        if (inserted >= TARGET) break outer;
        attempted++;
        const ok = await processVolume(conn, caches, item);
        if (ok) {
          inserted++;
          const title = item.volumeInfo?.title || '?';
          console.log(`    [${inserted}/${TARGET}] ✔ ${title}`);
        }
      }

      await sleep(600); // stay well under Google's quota
    }
  }

  // ── Summary ───────────────────────────────────────────────────────────────
  console.log('\n╔══════════════════════════════════════════╗');
  console.log(`║  Done!  Inserted ${String(inserted).padStart(3)} / ${TARGET} books`);
  console.log(`║  Volumes evaluated : ${attempted}`);
  console.log(`║  Categories        : ${categoryCache.size}`);
  console.log(`║  Genres            : ${genreCache.size}`);
  console.log(`║  Authors           : ${authorCache.size}`);
  console.log('╚══════════════════════════════════════════╝');

  await conn.end();
}

main().catch(err => {
  console.error('\n❌ Fatal error:', err.message);
  process.exit(1);
});