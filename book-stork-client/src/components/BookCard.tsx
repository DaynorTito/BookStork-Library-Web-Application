import { Link } from "react-router-dom";
import "./BookCard.css";

export interface Book {
  key: string;
  title: string;
  author_name?: string[];
  first_publish_year?: number;
  cover_i?: number;
}

interface BookCardProps {
  book: Book;
}

export default function BookCard({ book }: BookCardProps) {

    const workId = book.key.replace("/works/", "");
  const coverUrl = book.cover_i
    ? `https://covers.openlibrary.org/b/id/${book.cover_i}-M.jpg`
    : null;

  return (
    <Link to={`/books/${workId}`} className="book-card" title={book.title}>
      <div className="book-card_cover">
        {coverUrl ? (
          <img src={coverUrl} alt={`Cover of ${book.title}`} loading="lazy" />
        ) : (
          <div className="book-card_placeholder">No Cover</div>)}
      </div>
      <div className="book-card_body">
        <h3 className="book-card_title">{book.title}</h3>
        {book.author_name && (
          <p className="book-card_author">{book.author_name[0]}</p>
        )}
        {book.first_publish_year && (
          <p className="book-card_year">{book.first_publish_year}</p>
        )}
      </div>
    </Link>
  );
}
