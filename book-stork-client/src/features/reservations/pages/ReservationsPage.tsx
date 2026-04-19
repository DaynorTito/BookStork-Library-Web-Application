import React, { useCallback } from 'react'
import { Link } from 'react-router-dom'
import { Bookmark } from 'lucide-react'
import { useReservations } from '@/features/reservations/hooks/useReservations'
import { useToastContext } from '@/shared/context/ToastContext'
import Badge, { getStatusVariant } from '@/shared/components/ui/Badge'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'
import { formatDate, formatDateTime } from '@/shared/utils/formatDate'

export default function ReservationsPage() {
  const { list, isLoading, cancel, reload } = useReservations()
  const { addToast } = useToastContext()

  const handleCancel = useCallback(
    async (reservationId: string, title: string | null) => {
      try {
        const result = await cancel(reservationId)
        if ((result as { meta: { requestStatus: string } }).meta.requestStatus === 'fulfilled') {
          addToast(`Reservation for "${title}" cancelled`, 'info')
          reload()
        } else {
          addToast(((result as { payload: string }).payload) ?? 'Could not cancel reservation', 'error')
        }
      } catch {
        addToast('Could not cancel reservation', 'error')
      }
    },
    [cancel, addToast, reload],
  )

  if (isLoading) return <PageLoader />

  const reservations = list?.items ?? []

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-900 dark:text-gray-100 mb-6">
        My Reservations
      </h1>

      {reservations.length === 0 ? (
        <div className="text-center py-20 text-gray-500 dark:text-gray-400">
          <Bookmark size={48} className="mx-auto mb-4 text-gray-300 dark:text-gray-600" />
          <p className="text-lg font-medium">No reservations yet</p>
          <p className="text-sm mt-1 mb-6">
            Reserve a book that is currently checked out and you'll be notified when it's available.
          </p>
          <Link to="/catalog" className="btn-primary">
            Browse Catalog
          </Link>
        </div>
      ) : (
        <ul className="space-y-3">
          {reservations.map((res) => {
            const isActive = res.status === 'Active'
            return (
              <li
                key={res.id}
                className="flex items-start gap-4 p-4 rounded-lg border border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card"
              >
                {/* Cover */}
                <div className="w-12 h-16 rounded-lg overflow-hidden shrink-0 bg-gray-100 dark:bg-dark-border flex items-center justify-center">
                  {res.book?.coverImageUrl ? (
                    <img
                      src={res.book.coverImageUrl}
                      alt={res.book.title ?? ''}
                      className="w-full h-full object-cover"
                    />
                  ) : (
                    <span className="text-xl font-bold text-gray-300 dark:text-gray-600">
                      {res.book?.title?.charAt(0) ?? '?'}
                    </span>
                  )}
                </div>

                {/* Info */}
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1 flex-wrap">
                    <Link
                      to={`/catalog/${res.book?.id}`}
                      className="font-semibold text-gray-900 dark:text-gray-100 hover:text-primary-700 dark:hover:text-primary-400 line-clamp-1"
                    >
                      {res.book?.title ?? 'Unknown book'}
                    </Link>
                    <Badge variant={getStatusVariant(res.status)}>{res.status}</Badge>
                  </div>
                  <p className="text-xs text-gray-500 dark:text-gray-400">{res.book?.authorNames}</p>
                  <div className="mt-2 text-xs text-gray-400 dark:text-gray-500 space-y-0.5">
                    <p>Reserved: {formatDateTime(res.reservedAt)}</p>
                    <p>Expires: {formatDate(res.expiresAt)}</p>
                    {res.fulfilledAt && <p>Fulfilled: {formatDateTime(res.fulfilledAt)}</p>}
                  </div>
                </div>

                {/* Action */}
                {isActive && (
                  <button
                    onClick={() => handleCancel(res.id, res.book.title)}
                    className="btn-ghost text-sm py-1.5 px-4 shrink-0 text-red-500 hover:bg-red-50 dark:hover:bg-red-900/20"
                  >
                    Cancel
                  </button>
                )}
              </li>
            )
          })}
        </ul>
      )}
    </div>
  )
}
