import React, { memo } from 'react'
import { Link, NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '@/features/auth/hooks/useAuth'
import ThemeToggle from '@/shared/components/ThemeToggle'
import clsx from 'clsx'

const navLinkClass = ({ isActive }: { isActive: boolean }) =>
  clsx(
    'text-sm font-medium transition-colors',
    isActive
      ? 'text-primary-700 dark:text-primary-400'
      : 'text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-gray-100',
  )

const Navbar = memo(function Navbar() {
  const { isAuthenticated, user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/')
  }

  return (
    <header className="sticky top-0 z-40 border-b border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card">
      <div className="page-container flex items-center h-14 gap-6">
        <Link
          to="/"
          className="font-display font-bold text-xl text-primary-700 dark:text-primary-400 shrink-0"
        >
          BookStork
        </Link>

        <nav className="hidden md:flex items-center gap-5 flex-1">
          <NavLink to="/" end className={navLinkClass}>
            Home
          </NavLink>
          <NavLink to="/catalog" className={navLinkClass}>
            Catalog
          </NavLink>
          {isAuthenticated && (
            <>
              <NavLink to="/wishlist" className={navLinkClass}>
                Wishlist
              </NavLink>
              <NavLink to="/loans" className={navLinkClass}>
                Loans
              </NavLink>
              <NavLink to="/reservations" className={navLinkClass}>
                Reservations
              </NavLink>
            </>
          )}
        </nav>

        <div className="flex items-center gap-3 ml-auto">
          <ThemeToggle />
          {isAuthenticated ? (
            <div className="flex items-center gap-3">
              <span className="hidden sm:block text-sm text-gray-600 dark:text-gray-400">
                {user?.firstName}
              </span>
              <button onClick={handleLogout} className="btn-ghost text-sm py-1.5">
                Sign out
              </button>
            </div>
          ) : (
            <div className="flex items-center gap-2">
              <Link to="/login" className="btn-ghost text-sm py-1.5">
                Sign in
              </Link>
              <Link to="/register" className="btn-primary text-sm py-1.5">
                Register
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  )
})

export default Navbar
