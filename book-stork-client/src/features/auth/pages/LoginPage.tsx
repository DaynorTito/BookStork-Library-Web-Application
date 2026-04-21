import React, { useState, useCallback } from 'react'
import { Link, useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { useToastContext } from '@/shared/context/ToastContext'

export default function LoginPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const { login, isLoading, error, resetError } = useAuth()
  const { addToast } = useToastContext()

  const from = (location.state as { from?: { pathname: string } })?.from?.pathname ?? '/catalog'

  const [form, setForm] = useState({ email: '', password: '' })

  const handleChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    resetError()
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }))
  }, [resetError])

  const handleSubmit = useCallback(async (e: React.FormEvent) => {
    e.preventDefault()
    const result = await login({ email: form.email, password: form.password })
    if (result.meta.requestStatus === 'fulfilled') {
      addToast('Welcome back!', 'success')
      navigate(from, { replace: true })
    }
  }, [form, login, navigate, from, addToast])

  return (
    <div className="min-h-[80vh] flex items-center justify-center animate-slide-up">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-display font-bold text-gray-900 dark:text-gray-100 mb-1">Welcome back</h1>
          <p className="text-gray-500 dark:text-gray-400">Sign in to access your library</p>
        </div>

        <div className="bg-white dark:bg-dark-card rounded-2xl shadow-sm border border-gray-100 dark:border-dark-border p-8">
          <form onSubmit={handleSubmit} className="space-y-5" noValidate>
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5" htmlFor="email">
                Email address
              </label>
              <input
                id="email"
                name="email"
                type="email"
                autoComplete="email"
                required
                value={form.email}
                onChange={handleChange}
                placeholder="you@example.com"
                className="input-base"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5" htmlFor="password">
                Password
              </label>
              <input
                id="password"
                name="password"
                type="password"
                autoComplete="current-password"
                required
                value={form.password}
                onChange={handleChange}
                placeholder="••••••••"
                className="input-base"
              />
            </div>

            {error && (
              <div className="rounded-lg bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 px-4 py-3 text-sm text-red-700 dark:text-red-400">
                {error}
              </div>
            )}

            <button type="submit" disabled={isLoading} className="btn-primary w-full py-2.5">
              {isLoading ? 'Signing in...' : 'Sign in'}
            </button>
          </form>

          <p className="text-center text-sm text-gray-500 dark:text-gray-400 mt-6">
            No account yet?{' '}
            <Link to="/register" className="text-primary-700 dark:text-primary-400 font-medium hover:underline">
              Create one
            </Link>
          </p>
        </div>

        <p className="text-center text-xs text-gray-400 dark:text-gray-500 mt-4">
          Demo: register a new account to get started
        </p>
      </div>
    </div>
  )
}
