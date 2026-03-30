import { createContext, useState, useCallback, type ReactNode } from "react";

interface SearchContextValue {
  query: string;
  setQuery: (q: string) => void;
  clearQuery: () => void;
}

const SearchContext = createContext<SearchContextValue | undefined>(undefined);


export function SearchProvider({ children }: { children: ReactNode }) {
  const [query, setQueryState] = useState<string>("");

  const setQuery = useCallback((q: string) => {
    setQueryState(q);
  }, []);

  const clearQuery = useCallback(() => {
    setQueryState("");
  }, []);

  return (
    <SearchContext.Provider value={{ query, setQuery, clearQuery }}>
      {children}
    </SearchContext.Provider>
  );
}

export { SearchContext };
export type { SearchContextValue };