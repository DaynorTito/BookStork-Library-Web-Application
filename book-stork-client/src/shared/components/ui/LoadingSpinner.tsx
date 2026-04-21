import React, { memo } from 'react'
import clsx from 'clsx'

interface LoadingSpinnerProps {
  size?: 'sm' | 'md' | 'lg'
  className?: string
}

const LoadingSpinner = memo(function LoadingSpinner({ size = 'md', className }: LoadingSpinnerProps) {
  const sizeClass = { sm: 'w-5 h-5', md: 'w-8 h-8', lg: 'w-12 h-12' }[size]
  return (
    <div
      className={clsx(
        'rounded-full border-2 border-gray-200 dark:border-dark-border border-t-primary-700 animate-spin',
        sizeClass,
        className,
      )}
      role="status"
      aria-label="Loading"
    />
  )
})

export function PageLoader() {
  return (
    <div className="flex flex-col items-center gap-3 py-16">
      <LoadingSpinner size="lg" />
      <p className="text-sm text-gray-500 dark:text-gray-400">Loading…</p>
    </div>
  )
}

export default LoadingSpinner
