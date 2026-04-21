import React, { memo, useState, useCallback, useEffect } from 'react'
import { Link, NavLink, useNavigate, useLocation } from 'react-router-dom'
import { Menu, X } from 'lucide-react'
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

const mobileNavLinkClass = ({ isActive }: { isActive: boolean }) =>
  clsx(
    'block px-4 py-3 text-sm font-medium rounded-lg transition-colors',
    isActive
      ? 'text-primary-700 dark:text-primary-400 bg-primary-50 dark:bg-primary-900/20'
      : 'text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-dark-border',
  )

const Navbar = memo(function Navbar() {
  const { isAuthenticated, user, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [menuOpen, setMenuOpen] = useState(false)

  // Close the mobile menu on route change
  useEffect(() => {
    setMenuOpen(false)
  }, [location.pathname])

  const handleLogout = useCallback(() => {
    logout()
    navigate('/')
  }, [logout, navigate])

  const toggleMenu = useCallback(() => setMenuOpen((v) => !v), [])

  return (
    <header className="sticky top-0 z-40 border-b border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card">
      <div className="page-container flex items-center h-14 gap-6">
        {/* Logo */}
        <Link
          to="/"
          className="font-display font-bold text-xl text-primary-700 dark:text-primary-400 shrink-0"
        >
          BookStork
        </Link>

        {/* Desktop nav */}
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

        {/* Right-side controls */}
        <div className="flex items-center gap-3 ml-auto">
          <ThemeToggle />

          {/* Desktop auth */}
          <div className="hidden md:flex items-center gap-3">
            {isAuthenticated ? (
              <>
                <span className="text-sm text-gray-600 dark:text-gray-400">{user?.firstName}</span>
                <button onClick={handleLogout} className="btn-ghost text-sm py-1.5">
                  Sign out
                </button>
              </>
            ) : (
              <>
                <Link to="/login" className="btn-ghost text-sm py-1.5">
                  Sign in
                </Link>
                <Link to="/register" className="btn-primary text-sm py-1.5">
                  Register
                </Link>
              </>
            )}
          </div>

          {/* Hamburger button — mobile only */}
          <button
            onClick={toggleMenu}
            aria-label={menuOpen ? 'Close menu' : 'Open menu'}
            aria-expanded={menuOpen}
            className="md:hidden p-1.5 rounded-lg text-gray-600 dark:text-gray-400 hover:bg-gray-100 dark:hover:bg-dark-border transition-colors"
          >
            {menuOpen ? <X size={22} /> : <Menu size={22} />}
          </button>
        </div>
      </div>

      {/* Mobile drawer */}
      {menuOpen && (
        <div className="md:hidden border-t border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card px-4 py-3 flex flex-col gap-1">
          <NavLink to="/" end className={mobileNavLinkClass}>
            Home
          </NavLink>
          <NavLink to="/catalog" className={mobileNavLinkClass}>
            Catalog
          </NavLink>

          {isAuthenticated && (
            <>
              <NavLink to="/wishlist" className={mobileNavLinkClass}>
                Wishlist
              </NavLink>
              <NavLink to="/loans" className={mobileNavLinkClass}>
                Loans
              </NavLink>
              <NavLink to="/reservations" className={mobileNavLinkClass}>
                Reservations
              </NavLink>
            </>
          )}

          {/* Mobile auth */}
          <div className="mt-2 pt-3 border-t border-gray-100 dark:border-dark-border flex items-center justify-between">
            {isAuthenticated ? (
              <>
                <span className="text-sm text-gray-600 dark:text-gray-400">{user?.firstName}</span>
                <button onClick={handleLogout} className="btn-ghost text-sm py-1.5">
                  Sign out
                </button>
              </>
            ) : (
              <div className="flex items-center gap-2 w-full">
                <Link to="/login" className="btn-ghost text-sm py-1.5 flex-1 text-center">
                  Sign in
                </Link>
                <Link to="/register" className="btn-primary text-sm py-1.5 flex-1 text-center">
                  Register
                </Link>
              </div>
            )}
          </div>
        </div>
      )}
    </header>
  )
})

export default Navbar
