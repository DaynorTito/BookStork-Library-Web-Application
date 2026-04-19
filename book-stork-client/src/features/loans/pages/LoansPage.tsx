import React, { useCallback } from 'react'
import { Link } from 'react-router-dom'
import { BookOpen } from 'lucide-react'
import { useLoans } from '@/features/loans/hooks/useLoans'
import { useToastContext } from '@/shared/context/ToastContext'
import Badge, { getStatusVariant } from '@/shared/components/ui/Badge'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'
import { formatDate, formatDateTime } from '@/shared/utils/formatDate'

export default function LoansPage() {
  const { list, isLoading, returnBook, reload } = useLoans()
  const { addToast } = useToastContext()

  const handleReturn = useCallback(
    async (loanId: string, title: string | null) => {
      try {
        const result = await returnBook(loanId)
        if ((result as { meta: { requestStatus: string } }).meta.requestStatus === 'fulfilled') {
          addToast(`"${title}" returned successfully`, 'success')
          reload()
        } else {
          addToast(((result as { payload: string }).payload) ?? 'Could not return book', 'error')
        }
      } catch {
        addToast('Could not return book', 'error')
      }
    },
    [returnBook, addToast, reload],
  )

  if (isLoading) return <PageLoader />

  const loans = list?.items ?? []

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-900 dark:text-gray-100 mb-6">
        My Loans
      </h1>

      {loans.length === 0 ? (
        <div className="text-center py-20 text-gray-500 dark:text-gray-400">
          <BookOpen size={48} className="mx-auto mb-4 text-gray-300 dark:text-gray-600" />
          <p className="text-lg font-medium">No loans yet</p>
          <p className="text-sm mt-1 mb-6">Browse the catalog and borrow a book.</p>
          <Link to="/catalog" className="btn-primary">
            Browse Catalog
          </Link>
        </div>
      ) : (
        <ul className="space-y-3">
          {loans.map((loan) => {
            const isActive = loan.status === 'ACTIVE' || loan.status === 'OVERDUE';

            return (
              <li
                key={loan.id}
                className="flex items-start gap-4 p-4 rounded-lg border border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card"
              >
                {/* Cover */}
                <div className="w-12 h-16 rounded-lg overflow-hidden shrink-0 bg-gray-100 dark:bg-dark-border flex items-center justify-center">
                  {loan.book?.coverImageUrl ? (
                    <img
                      src={loan.book.coverImageUrl}
                      alt={loan.book.title ?? ''}
                      className="w-full h-full object-cover"
                    />
                  ) : (
                    <span className="text-xl font-bold text-gray-300 dark:text-gray-600">
                      {loan.book?.title?.charAt(0) ?? '?'}
                    </span>
                  )}
                </div>

                {/* Info */}
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1">
                    <Link
                      to={`/catalog/${loan.book?.id}`}
                      className="font-semibold text-gray-900 dark:text-gray-100 hover:text-primary-700 dark:hover:text-primary-400 line-clamp-1"
                    >
                      {loan.book?.title ?? 'Unknown book'}
                    </Link>
                    <Badge variant={getStatusVariant(loan.status)}>{loan.status}</Badge>
                  </div>
                  <p className="text-xs text-gray-500 dark:text-gray-400">{loan.book?.authorNames}</p>
                  <div className="mt-2 text-xs text-gray-400 dark:text-gray-500 space-y-0.5">
                    <p>Borrowed: {formatDateTime(loan.loanedAt)}</p>
                    <p>
                      Due: {formatDate(loan.dueDate)}
                      {isActive && loan.daysRemaining >= 0 && (
                        <span className="ml-1 text-gray-500">
                          ({loan.daysRemaining} day{loan.daysRemaining !== 1 ? 's' : ''} left)
                        </span>
                      )}
                      {loan.isOverdue && (
                        <span className="ml-1 text-red-500 font-medium">Overdue!</span>
                      )}
                    </p>
                    {loan.returnedAt && <p>Returned: {formatDateTime(loan.returnedAt)}</p>}
                  </div>
                </div>

                {/* Action */}
                {isActive && (
                  <button
                    onClick={() => handleReturn(loan.id, loan.book?.title ?? null)}
                    className="btn-secondary text-sm py-1.5 px-4 shrink-0"
                  >
                    Return
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
