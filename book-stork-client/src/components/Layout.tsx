import { Link, Outlet, useNavigate } from "react-router-dom";
import { useSearch } from "../context/useSearch";
import { useState } from "react";
import "./Layout.css";

export default function Layout() {
  const { setQuery } = useSearch();
  const navigate = useNavigate();
  const [input, setInput] = useState("");

  function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    const trimmed = input.trim();
    if (!trimmed) return;
    setQuery(trimmed);
    navigate("/books");
  }

  return (
    <div className="layout">
      <header className="navbar">
        <Link to="/" className="navbar_brand">
          BookStork
        </Link>

        <form className="navbar_search" onSubmit={handleSearch}>
          <input
            className="navbar_input"
            type="text"
            placeholder="Search for a book…"
            value={input}
            onChange={(e) => setInput(e.target.value)}
          />
          <button className="navbar_btn" type="submit">Search</button>
        </form>

        <nav className="navbar_links">
          <Link to="/">Home</Link>
          <Link to="/books">Browse</Link>
        </nav>
      </header>

      <main className="main-content">
        <Outlet />
      </main>

      <footer className="footer">
        <p>Data from <a href="https://openlibrary.org" target="_blank" rel="noreferrer">Open Library</a></p>
      </footer>
    </div>
  );
}
