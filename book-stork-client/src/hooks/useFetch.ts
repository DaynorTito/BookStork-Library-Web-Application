import { useEffect, useCallback, useReducer } from "react";

interface FetchState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
}

interface UseFetchOptions {
  enabled?: boolean;
}

interface UseFetchReturn<T> extends FetchState<T> {
  refetch: () => void;
}

type Action<T> =
  | { type: "FETCH_SUCCESS"; payload: T }
  | { type: "FETCH_ERROR"; payload: string }
  | { type: "FETCH_RESET" };
function fetchReducer<T>(state: FetchState<T>, action: Action<T>): FetchState<T> {
  switch (action.type) {
    case "FETCH_RESET":
      return { data: null, loading: true, error: null };
    case "FETCH_SUCCESS":
      return { data: action.payload, loading: false, error: null };
    case "FETCH_ERROR":
      return { data: null, loading: false, error: action.payload };
    default:
      return state;
  }
}

function useFetch<T>(url: string, options: UseFetchOptions = {}): UseFetchReturn<T> {
  const { enabled = true } = options;

  const [state, dispatch] = useReducer(fetchReducer<T>, {
    data: null,
    loading: Boolean(url && enabled),
    error: null,
  });

  const [fetchTrigger, dispatchTrigger] = useReducer((n: number) => n + 1, 0);

  useEffect(() => {
    if (!enabled || !url) return;

    const controller = new AbortController();

    dispatch({ type: "FETCH_RESET" });

    fetch(url, { signal: controller.signal })
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
        return res.json() as Promise<T>;
      })
      .then((data) => dispatch({ type: "FETCH_SUCCESS", payload: data }))
      .catch((err: Error) => {
        if (err.name === "AbortError") return;
        dispatch({ type: "FETCH_ERROR", payload: err.message });
      });

    return () => controller.abort();
  }, [url, enabled, fetchTrigger]);

  const refetch = useCallback(() => {
    dispatchTrigger();
  }, []);

  return { ...state, refetch };
}

export default useFetch;