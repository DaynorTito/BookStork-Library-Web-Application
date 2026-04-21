import React, { useState, useCallback } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { useToastContext } from '@/shared/context/ToastContext'

export default function RegisterPage() {
  const navigate = useNavigate()
  const { register, isLoading, error, resetError } = useAuth()
  const { addToast } = useToastContext()

  const [form, setForm] = useState({
    firstName: '', lastName: '', email: '', password: '', confirmPassword: '',
  })
  const [localError, setLocalError] = useState('')

  const handleChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    resetError()
    setLocalError('')
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }))
  }, [resetError])

  const handleSubmit = useCallback(async (e: React.FormEvent) => {
    e.preventDefault()
    if (form.password !== form.confirmPassword) {
      setLocalError('Passwords do not match')
      return
    }
    if (form.password.length < 6) {
      setLocalError('Password must be at least 6 characters')
      return
    }
    const result = await register(form)
    if (result.meta.requestStatus === 'fulfilled') {
      addToast('Account created! Please sign in.', 'success')
      navigate('/login')
    }
  }, [form, register, navigate, addToast])

  const displayError = localError || error

  return (
    <div className="min-h-[80vh] flex items-center justify-center animate-slide-up py-8">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-display font-bold text-gray-900 dark:text-gray-100 mb-1">Join BookStork</h1>
          <p className="text-gray-500 dark:text-gray-400">Create your virtual library account</p>
        </div>

        <div className="bg-white dark:bg-dark-card rounded-2xl shadow-sm border border-gray-100 dark:border-dark-border p-8">
          <form onSubmit={handleSubmit} className="space-y-4" noValidate>
            <div className="grid grid-cols-2 gap-3">
              {(['firstName', 'lastName'] as const).map((field) => (
                <div key={field}>
                  <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5" htmlFor={field}>
                    {field === 'firstName' ? 'First name' : 'Last name'}
                  </label>
                  <input
                    id={field}
                    name={field}
                    type="text"
                    required
                    value={form[field]}
                    onChange={handleChange}
                    className="input-base"
                  />
                </div>
              ))}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5" htmlFor="email">
                Email address
              </label>
              <input
                id="email" name="email" type="email" autoComplete="email" required
                value={form.email} onChange={handleChange} className="input-base"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5" htmlFor="password">
                Password
              </label>
              <input
                id="password" name="password" type="password" required
                value={form.password} onChange={handleChange} className="input-base"
                placeholder="Min. 6 characters"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1.5" htmlFor="confirmPassword">
                Confirm password
              </label>
              <input
                id="confirmPassword" name="confirmPassword" type="password" required
                value={form.confirmPassword} onChange={handleChange} className="input-base"
              />
            </div>

            {displayError && (
              <div className="rounded-lg bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 px-4 py-3 text-sm text-red-700 dark:text-red-400">
                {displayError}
              </div>
            )}

            <button type="submit" disabled={isLoading} className="btn-primary w-full py-2.5 mt-2">
              {isLoading ? 'Creating account...' : 'Create account'}
            </button>
          </form>

          <p className="text-center text-sm text-gray-500 dark:text-gray-400 mt-6">
            Already a member?{' '}
            <Link to="/login" className="text-primary-700 dark:text-primary-400 font-medium hover:underline">
              Sign in
            </Link>
          </p>
        </div>
      </div>
    </div>
  )
}
