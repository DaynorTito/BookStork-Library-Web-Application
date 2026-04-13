import { screen } from "@testing-library/react";
import { describe, it, expect } from "vitest";
import type { Book } from "../components/BookCard";
import { renderWithProviders } from "./renderWithProviders";
import BookCard from "../components/BookCard";

const bookCompleto: Book = {
  key: "/works/OL82563W",
  title: "Dune",
  author_name: ["Frank Herbert"],
  first_publish_year: 1965,
  cover_i: 12345,
};

const bookSinOpcionales: Book = {
  key: "/works/OL999W",
  title: "Libro sin datos opcionales",
};

describe("BookCard", () => {
  it("muestra el titulo del libro", () => {
    renderWithProviders(<BookCard book={bookCompleto} />);

    expect(screen.getByText("Dune")).toBeInTheDocument();
  });

  it("muestra el nombre del primer autor cuando esta disponible", () => {
    renderWithProviders(<BookCard book={bookCompleto} />);

    expect(screen.getByText("Frank Herbert")).toBeInTheDocument();
  });

  it("muestra el anio de primera publicacion cuando esta disponible", () => {
    renderWithProviders(<BookCard book={bookCompleto} />);

    expect(screen.getByText("1965")).toBeInTheDocument();
  });

  it("no muestra autor ni anio cuando el libro no los tiene", () => {
    renderWithProviders(<BookCard book={bookSinOpcionales} />);

    expect(screen.getByText("Libro sin datos opcionales")).toBeInTheDocument();

    expect(screen.queryByText("Frank Herbert")).not.toBeInTheDocument();
    expect(screen.queryByText("1965")).not.toBeInTheDocument();
  });

  it("genera el enlace correcto hacia la pagina de detalle", () => {
    renderWithProviders(<BookCard book={bookCompleto} />);

    const link = screen.getByRole("link");
    expect(link).toHaveAttribute("href", "/books/OL82563W");
  });

  it("muestra la imagen de portada con el alt text correcto cuando hay cover_i", () => {
    renderWithProviders(<BookCard book={bookCompleto} />);

    const img = screen.getByRole("img");
    expect(img).toHaveAttribute("alt", "Cover of Dune");
    expect(img).toHaveAttribute(
      "src",
      "https://covers.openlibrary.org/b/id/12345-M.jpg"
    );
  });

  it("no muestra imagen cuando el libro no tiene cover_i", () => {
    renderWithProviders(<BookCard book={bookSinOpcionales} />);

    expect(screen.queryByRole("img")).not.toBeInTheDocument();
  });
});
