import React, { Suspense, lazy } from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Layout from '@/shared/components/layout/Layout'
import ProtectedRoute from './ProtectedRoute'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'

const HomePage = lazy(() => import('@/features/books/pages/HomePage'))
const BooksPage = lazy(() => import('@/features/books/pages/BooksPage'))
const BookDetailPage = lazy(() => import('@/features/books/pages/BookDetailPage'))
const LoginPage = lazy(() => import('@/features/auth/pages/LoginPage'))
const RegisterPage = lazy(() => import('@/features/auth/pages/RegisterPage'))
const WishlistPage = lazy(() => import('@/features/wishlist/pages/WishlistPage'))
const LoansPage = lazy(() => import('@/features/loans/pages/LoansPage'))
const ReservationsPage = lazy(() => import('@/features/reservations/pages/ReservationsPage'))

export default function AppRoutes() {
  return (
    <Suspense fallback={<div className="min-h-screen flex items-center justify-center"><PageLoader /></div>}>
      <Routes>
        <Route element={<Layout />}>
          <Route index element={<HomePage />} />
          <Route path="catalog" element={<BooksPage />} />
          <Route path="catalog/:id" element={<BookDetailPage />} />
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterPage />} />
          <Route
            path="wishlist"
            element={<ProtectedRoute><WishlistPage /></ProtectedRoute>}
          />
          <Route
            path="loans"
            element={<ProtectedRoute><LoansPage /></ProtectedRoute>}
          />
          <Route
            path="reservations"
            element={<ProtectedRoute><ReservationsPage /></ProtectedRoute>}
          />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </Suspense>
  )
}
