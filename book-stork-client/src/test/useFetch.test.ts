import { renderHook, waitFor, act } from "@testing-library/react";
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import useFetch from "../hooks/useFetch";

const fetchMock = vi.fn();

beforeEach(() => {
  vi.stubGlobal("fetch", fetchMock);
  fetchMock.mockClear();
});

afterEach(() => {
  vi.restoreAllMocks();
});

function okResponse(data: unknown) {
  return Promise.resolve({
    ok: true,
    status: 200,
    json: () => Promise.resolve(data),
  });
}

function errorResponse(status: number, statusText: string) {
  return Promise.resolve({
    ok: false,
    status,
    statusText,
    json: () => Promise.resolve({}),
  });
}


describe("useFetch", () => {

  it("inicia con loading en true cuando se proporciona una URL", () => {
    fetchMock.mockReturnValue(new Promise(() => {}));

    const { result } = renderHook(() =>
      useFetch("https://api.ejemplo.com/libros")
    );

    expect(result.current.loading).toBe(true);
    expect(result.current.data).toBeNull();
    expect(result.current.error).toBeNull();
  });


  it("establece data y loading en false cuando la peticion es exitosa", async () => {
    const dataMock = { docs: [{ key: "/works/OL1W", title: "Dune" }] };
    fetchMock.mockReturnValue(okResponse(dataMock));

    const { result } = renderHook(() =>
      useFetch("https://api.ejemplo.com/libros")
    );

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.data).toEqual(dataMock);
    expect(result.current.error).toBeNull();
  });


  it("establece error cuando el servidor responde con codigo de error HTTP", async () => {
    fetchMock.mockReturnValue(errorResponse(404, "Not Found"));

    const { result } = renderHook(() =>
      useFetch("https://api.ejemplo.com/libros")
    );

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toMatch(/404/);
    expect(result.current.data).toBeNull();
  });


  it("vuelve a realizar la peticion cuando se llama a refetch", async () => {
    const dataMock = { docs: [] };
    fetchMock.mockReturnValue(okResponse(dataMock));

    const { result } = renderHook(() =>
      useFetch("https://api.ejemplo.com/libros")
    );

    await waitFor(() => expect(result.current.loading).toBe(false));

    const llamadasAntes = fetchMock.mock.calls.length;

    act(() => {
      result.current.refetch();
    });

    await waitFor(() => expect(result.current.loading).toBe(false));

    expect(fetchMock.mock.calls.length).toBeGreaterThan(llamadasAntes);
  });


  it("establece error cuando la red falla (fetch rechaza la promesa)", async () => {
    fetchMock.mockRejectedValue(new Error("Network error"));

    const { result } = renderHook(() =>
      useFetch("https://api.ejemplo.com/libros")
    );

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toBe("Network error");
    expect(result.current.data).toBeNull();
  });
});
