import { screen, fireEvent } from "@testing-library/react";
import { describe, it, expect } from "vitest";
import HomePage from "../pages/HomePage";
import { renderWithProviders } from "./renderWithProviders";

describe("HomePage", () => {
  it("renderiza el titulo principal", () => {
    renderWithProviders(<HomePage />);

    expect(
      screen.getByRole("heading", { name: /discover your next read/i })
    ).toBeInTheDocument();
  });

  it("renderiza el campo de texto de busqueda y el boton", () => {
    renderWithProviders(<HomePage />);

    expect(
      screen.getByPlaceholderText(/title, author, or keyword/i)
    ).toBeInTheDocument();

    expect(
      screen.getByRole("button", { name: /search/i })
    ).toBeInTheDocument();
  });

  it("renderiza todos los chips de sugerencias", () => {
    renderWithProviders(<HomePage />);

    const sugerencias = ["Dune", "1984", "The Hobbit", "Sapiens", "Harry Potter"];
    sugerencias.forEach((s) => {
      expect(screen.getByRole("button", { name: s })).toBeInTheDocument();
    });
  });

  it("actualiza el valor del input al escribir", () => {
    renderWithProviders(<HomePage />);

    const input = screen.getByPlaceholderText(/title, author, or keyword/i);
    fireEvent.change(input, { target: { value: "Orwell" } });

    expect(input).toHaveValue("Orwell");
  });

  it("no navega si el usuario envia el formulario con el campo vacio", () => {
    renderWithProviders(<HomePage />);

    const boton = screen.getByRole("button", { name: /search/i });
    fireEvent.click(boton);

    expect(
      screen.getByRole("heading", { name: /discover your next read/i })
    ).toBeInTheDocument();
  });

  it("no navega si el usuario envia solo espacios en blanco", () => {
    renderWithProviders(<HomePage />);

    const input = screen.getByPlaceholderText(/title, author, or keyword/i);
    fireEvent.change(input, { target: { value: "   " } });
    fireEvent.submit(input.closest("form")!);

    expect(
      screen.getByRole("heading", { name: /discover your next read/i })
    ).toBeInTheDocument();
  });
});
