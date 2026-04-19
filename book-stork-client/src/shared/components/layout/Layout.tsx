import React, { useEffect } from 'react'
import { Outlet } from 'react-router-dom'
import { useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '@/core/store/store'
import { fetchWishlistThunk } from '@/features/wishlist/store/wishlistSlice'
import Navbar from '@/shared/components/Navbar'

export default function Layout() {
  const dispatch = useDispatch<AppDispatch>()
  const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated)

  useEffect(() => {
    if (isAuthenticated) {
      dispatch(fetchWishlistThunk())
    }
  }, [dispatch, isAuthenticated])

  return (
    <div className="min-h-screen bg-[var(--bg-primary)]">
      <Navbar />
      <main className="page-container py-6">
        <Outlet />
      </main>
    </div>
  )
}
