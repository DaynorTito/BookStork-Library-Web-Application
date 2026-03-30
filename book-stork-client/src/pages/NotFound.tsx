import { Link } from "react-router-dom";
import "./NotFound.css";

export default function NotFound() {
  return (
    <section className="not-found">
      <div className="not-found_icon">📭</div>
      <h2 className="not-found_heading">404 — Page Not Found</h2>
      <p className="not-found_sub">
        The page you are looking for does not exist.
      </p>
      <Link to="/" className="not-found_link">Go Home</Link>
    </section>
  );
}