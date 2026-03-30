import { useNavigate } from "react-router-dom";
import { useSearch } from "../context/useSearch";
import { useState } from "react";
import "./HomePage.css";

const SUGGESTIONS = ["Dune", "1984", "The Hobbit", "Sapiens", "Harry Potter"];

export default function HomePage() {
  const { setQuery } = useSearch();
  const navigate = useNavigate();
  const [input, setInput] = useState("");

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    const q = input.trim();
    if (!q) return;
    setQuery(q);
    navigate("/books");
  }

  function handleSuggestion(s: string) {
    setQuery(s);
    navigate("/books");
  }

  return (
    <section className="home">
      <h1 className="home_heading">Discover Your Next Read</h1>
      <p className="home_sub">
        Search millions of books powered by the Open Library API.
      </p>

      <form className="home_form" onSubmit={handleSubmit}>
        <input
          className="home_input"
          type="text"
          placeholder="Title, author, or keyword…"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          autoFocus
        />
        <button className="home_btn" type="submit">Search</button>
      </form>

      <div className="home_suggestions">
        <span className="home_suggestions-label">Try:</span>
        {SUGGESTIONS.map((s) => (
          <button
            key={s}
            className="home_chip"
            onClick={() => handleSuggestion(s)}
          >
            {s}
          </button>
        ))}
      </div>
    </section>
  );
}