import React, { useEffect, useCallback } from 'react'
import { Link } from 'react-router-dom'
import { Star, Bell, X } from 'lucide-react'
import { useWishlist } from '@/features/wishlist/hooks/useWishlist'
import { useToastContext } from '@/shared/context/ToastContext'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'
import { formatDate } from '@/shared/utils/formatDate'
import clsx from 'clsx'

export default function WishlistPage() {
  const { items, isLoading, fetchAll, remove, toggleNotify } = useWishlist()
  const { addToast } = useToastContext()

  useEffect(() => {
    fetchAll()
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const handleRemove = useCallback(
    async (bookId: string, title: string | null) => {
      try {
        await remove(bookId)
        addToast(`Removed "${title}" from wishlist`, 'info')
      } catch {
        addToast('Could not remove from wishlist', 'error')
      }
    },
    [remove, addToast],
  )

  const handleToggleNotify = useCallback(
    async (bookId: string, current: boolean) => {
      try {
        await toggleNotify(bookId, !current)
        addToast(current ? 'Notifications disabled' : 'You will be notified when available', 'success')
      } catch {
        addToast('Could not update notification preference', 'error')
      }
    },
    [toggleNotify, addToast],
  )

  if (isLoading) return <PageLoader />

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-900 dark:text-gray-100 mb-6">
        My Wishlist
      </h1>

      {items.length === 0 ? (
        <div className="text-center py-20 text-gray-500 dark:text-gray-400">
          <Star size={48} className="mx-auto mb-4 text-gray-300 dark:text-gray-600" />
          <p className="text-lg font-medium">Your wishlist is empty</p>
          <p className="text-sm mt-1 mb-6">Browse the catalog and add books you'd like to read.</p>
          <Link to="/catalog" className="btn-primary">
            Browse Catalog
          </Link>
        </div>
      ) : (
        <ul className="space-y-3">
          {items.map((item) => (
            <li
              key={item.id}
              className="flex items-center gap-4 p-4 rounded-lg border border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card"
            >
              {/* Tiny cover */}
              <div className="w-12 h-16 rounded-lg overflow-hidden shrink-0 bg-gray-100 dark:bg-dark-border flex items-center justify-center">
                {item.book.coverImageUrl ? (
                  <img
                    src={item.book.coverImageUrl}
                    alt={item.book.title ?? ''}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <span className="text-xl font-display font-bold text-gray-300 dark:text-gray-600">
                    {item.book.title?.charAt(0) ?? '?'}
                  </span>
                )}
              </div>

              {/* Info */}
              <div className="flex-1 min-w-0">
                <Link
                  to={`/catalog/${item.book.id}`}
                  className="font-semibold text-gray-900 dark:text-gray-100 hover:text-primary-700 dark:hover:text-primary-400 line-clamp-1"
                >
                  {item.book.title}
                </Link>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5 line-clamp-1">
                  {item.book.authorNames}
                </p>
                <p className="text-xs text-gray-400 dark:text-gray-500 mt-0.5">
                  Added {formatDate(item.addedAt)}
                </p>
              </div>

              {/* Actions */}
              <div className="flex items-center gap-2 shrink-0">
                <button
                  onClick={() => handleToggleNotify(item.book.id, item.notifyOnAvailable)}
                  aria-label={item.notifyOnAvailable ? 'Disable notification' : 'Enable notification'}
                  title={item.notifyOnAvailable ? 'Notifications on' : 'Notifications off'}
                  className={clsx(
                    'w-8 h-8 rounded-full flex items-center justify-center transition-colors border',
                    item.notifyOnAvailable
                      ? 'bg-primary-700 border-primary-700 text-white'
                      : 'border-gray-200 dark:border-dark-border text-gray-400 hover:border-primary-400',
                  )}
                >
                  <Bell size={14} />
                </button>

                <button
                  onClick={() => handleRemove(item.book.id, item.book.title)}
                  aria-label="Remove from wishlist"
                  className="w-8 h-8 rounded-full flex items-center justify-center text-gray-400 hover:text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                >
                  <X size={14} />
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
