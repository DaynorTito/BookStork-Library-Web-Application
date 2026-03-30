import { useSearch } from "../context/useSearch";
import useFetch from "../hooks/useFetch";
import BookCard, { type Book } from "../components/BookCard";
import LoadingSpinner from "../components/LoadingSpinner";
import "./BooksPage.css";

interface SearchResponse {
  numFound: number;
  docs: Book[];
}

export default function BooksPage() {
    
  const { query } = useSearch();

  const url = query
    ? `https://openlibrary.org/search.json?q=${encodeURIComponent(query)}&limit=20`
    : `https://openlibrary.org/search.json?q=trending&limit=20`;

  const { data, loading, error, refetch } = useFetch<SearchResponse>(url);

  return (
    <section className="books-page">
      <div className="books-page__header">
        <h2 className="books-page__title">
          {query ? `Results for "${query}"` : "Trending Books"}
        </h2>
        {data && !loading && (
          <p className="books-page__count">
            {data.numFound.toLocaleString()} books found
          </p>
        )}
      </div>

      {loading && <LoadingSpinner />}

      {error && (
        <div className="books-page__error">
          <p>Something went wrong: {error}</p>
          <button onClick={refetch}>Retry</button>
        </div>
      )}

      {!loading && !error && data && (
        <div className="books-page__grid">
          {data.docs.map((book) => (
            <BookCard key={book.key} book={book} />
          ))}
        </div>
      )}

      {!loading && !error && data?.docs.length === 0 && (
        <p className="books-page__empty">No books found. Try a different search.</p>
      )}
    </section>
  );
}