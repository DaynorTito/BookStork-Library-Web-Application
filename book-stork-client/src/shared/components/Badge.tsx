import React, { memo } from 'react'
import clsx from 'clsx'

export type BadgeVariant = 'success' | 'warning' | 'error' | 'info' | 'default'

interface BadgeProps {
  variant?: BadgeVariant
  children: React.ReactNode
  className?: string
}

const variantClasses: Record<BadgeVariant, string> = {
  success: 'bg-emerald-100 text-emerald-800 dark:bg-emerald-900/30 dark:text-emerald-300',
  warning: 'bg-amber-100 text-amber-800 dark:bg-amber-900/30 dark:text-amber-300',
  error: 'bg-red-100 text-red-800 dark:bg-red-900/30 dark:text-red-300',
  info: 'bg-primary-100 text-primary-800 dark:bg-primary-900/30 dark:text-primary-300',
  default: 'bg-gray-100 text-gray-700 dark:bg-dark-border dark:text-gray-400',
}

export function getStatusVariant(status: string | null | undefined): BadgeVariant {
  switch (status) {
    case 'Available': return 'success'
    case 'Loaned': return 'error'
    case 'Reserved': return 'warning'
    case 'Active': return 'info'
    case 'Returned': return 'success'
    case 'Overdue': return 'error'
    case 'Fulfilled': return 'success'
    case 'Cancelled': return 'default'
    case 'Expired': return 'default'
    default: return 'default'
  }
}

const Badge = memo(function Badge({ variant = 'default', children, className }: BadgeProps) {
  return (
    <span className={clsx('badge', variantClasses[variant], className)}>
      {children}
    </span>
  )
})

export default Badge
