
import { lazy, Suspense } from "react";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Layout from "../components/Layout";
import LoadingSpinner from "../components/LoadingSpinner";

const HomePage   = lazy(() => import("../pages/HomePage"));
const BooksPage  = lazy(() => import("../pages/BooksPage"));
const DetailPage = lazy(() => import("../pages/DetailPage"));
const NotFound   = lazy(() => import("../pages/NotFound"));

const Lazy = ({ element }: { element: React.ReactNode }) => (
  <Suspense fallback={<LoadingSpinner />}>{element}</Suspense>
);

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [
      { index: true, element: <Lazy element={<HomePage />} /> },

      { path: "books", element: <Lazy element={<BooksPage />} /> },

      { path: "books/:workId", element: <Lazy element={<DetailPage />} /> },

      { path: "*", element: <Lazy element={<NotFound />} /> },
    ],
  },
]);

export default function AppRouter() {
  return <RouterProvider router={router} />;
}
