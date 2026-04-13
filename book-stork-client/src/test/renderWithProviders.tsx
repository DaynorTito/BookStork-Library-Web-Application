import { render, type RenderOptions } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { SearchProvider } from "../context/SearchContext";
import type { ReactNode } from "react";


function renderWithProviders(
  ui: ReactNode,
  options?: RenderOptions & { initialEntries?: string[] }
) {
  const { initialEntries = ["/"], ...rest } = options ?? {};

  return render(
    <MemoryRouter initialEntries={initialEntries}>
      <SearchProvider>{ui}</SearchProvider>
    </MemoryRouter>,
    rest
  );
}

export { renderWithProviders };
