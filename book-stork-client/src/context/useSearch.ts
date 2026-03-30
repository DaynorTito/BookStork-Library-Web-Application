import { useContext } from "react";
import { SearchContext, type SearchContextValue } from "./SearchContext";

export function useSearch(): SearchContextValue {
  const context = useContext(SearchContext);
  if (context === undefined) {
    throw new Error("useSearch must be used inside a <SearchProvider>.");
  }
  return context;
}
 
