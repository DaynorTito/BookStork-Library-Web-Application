import { useParams, Link } from "react-router-dom";
import useFetch from "../hooks/useFetch";
import LoadingSpinner from "../components/LoadingSpinner";
import "./DetailPage.css";

interface WorkDetail {
  title: string;
  description?: string | { value: string };
  covers?: number[];
  subjects?: string[];
  authors?: { author: { key: string }; type: { key: string } }[];
}

function resolveDescription(desc?: string | { value: string }): string {
  if (!desc) return "No description available.";
  if (typeof desc === "string") return desc;
  return desc.value;
}

export default function DetailPage() {

  const { workId } = useParams<{ workId: string }>();

  const url = workId
    ? `https://openlibrary.org/works/${workId}.json`
    : "";

  const { data, loading, error } = useFetch<WorkDetail>(url);

  const cover = data?.covers?.[0]
    ? `https://covers.openlibrary.org/b/id/${data.covers[0]}-L.jpg`
    : null;

  return (
    <section className="detail">
      <Link to="/books" className="detail_back">← Back to results</Link>

      {loading && <LoadingSpinner />}

      {error && (
        <div className="detail_error">
          <p>Could not load book: {error}</p>
        </div>
      )}

      {!loading && !error && data && (
        <div className="detail_body">
          <div className="detail_cover-wrap">
            {cover ? (
              <img className="detail_cover" src={cover} alt={`Cover of ${data.title}`} />
            ) : (
                <div className="detail_cover-placeholder">No Cover</div>
            )}
          </div>

          <div className="detail_info">
            <h1 className="detail_title">{data.title}</h1>

            <p className="detail_desc">{resolveDescription(data.description)}</p>

            {data.subjects && data.subjects.length > 0 && (
              <div className="detail_subjects">
                <h3>Subjects</h3>
                <div className="detail_tags">
                  {data.subjects.slice(0, 10).map((s) => (
                    <span key={s} className="detail_tag">{s}</span>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </section>
  );
}