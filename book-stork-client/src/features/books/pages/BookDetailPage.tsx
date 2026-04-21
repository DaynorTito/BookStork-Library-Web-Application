import React, { useEffect, useState, useCallback } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft, Heart, BookX } from 'lucide-react'
import { useBookDetail } from '@/features/books/hooks/useBooks'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { useWishlist } from '@/features/wishlist/hooks/useWishlist'
import { useLoans } from '@/features/loans/hooks/useLoans'
import { useReservations } from '@/features/reservations/hooks/useReservations'
import { useToastContext } from '@/shared/context/ToastContext'
import StarRating from '@/shared/components/ui/StarRating'
import Badge, { getStatusVariant } from '@/shared/components/ui/Badge'
import Modal from '@/shared/components/Modal'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'
import { formatDate } from '@/shared/utils/formatDate'
import clsx from 'clsx'

const LOAN_DAYS_OPTIONS = [7, 14, 21, 30]

export default function BookDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const { book, isLoading, fetch, clear } = useBookDetail()
  const { isAuthenticated } = useAuth()
  const { isInWishlist, add, remove, fetchAll } = useWishlist()
  const { borrow } = useLoans()
  const { reserve } = useReservations()
  const { addToast } = useToastContext()

  const [borrowModalOpen, setBorrowModalOpen] = useState(false)
  const [loanDays, setLoanDays] = useState(14)
  const [actionLoading, setActionLoading] = useState(false)
  const [imgSrcIndex, setImgSrcIndex] = useState(0)

  useEffect(() => {
    if (id) fetch(id)
    return () => { clear() }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id])

  const inWishlist = book ? isInWishlist(book.id) : false

  const handleWishlist = useCallback(async () => {
    if (!book) return
    if (!isAuthenticated) {
      addToast('Sign in to manage your wishlist', 'info')
      return
    }
    try {
      if (inWishlist) {
        await remove(book.id)
        addToast('Removed from wishlist', 'info')
      } else {
        await add(book.id, true)
        await fetchAll()
        addToast('Added to wishlist', 'success')
      }
    } catch {
      addToast('Something went wrong', 'error')
    }
  }, [book, isAuthenticated, inWishlist, add, remove, fetchAll, addToast])

  const handleBorrow = useCallback(async () => {
    if (!book) return
    setActionLoading(true)
    try {
      const result = await borrow(book.id, loanDays)
      if ((result as { meta: { requestStatus: string } }).meta.requestStatus === 'fulfilled') {
        addToast('Book borrowed successfully!', 'success')
        setBorrowModalOpen(false)
        fetch(book.id)
      } else {
        addToast(((result as { payload: string }).payload) ?? 'Could not borrow book', 'error')
      }
    } catch (e) {
      addToast((e as Error).message ?? 'Could not borrow book', 'error')
    } finally {
      setActionLoading(false)
    }
  }, [book, loanDays, borrow, addToast, fetch])

  const handleReserve = useCallback(async () => {
    if (!book) return
    setActionLoading(true)
    try {
      const result = await reserve(book.id)
      if ((result as { meta: { requestStatus: string } }).meta.requestStatus === 'fulfilled') {
        addToast('Reservation created!', 'success')
        fetch(book.id)
      } else {
        addToast(((result as { payload: string }).payload) ?? 'Could not reserve book', 'error')
      }
    } catch (e) {
      addToast((e as Error).message ?? 'Could not reserve book', 'error')
    } finally {
      setActionLoading(false)
    }
  }, [book, reserve, addToast, fetch])

  if (isLoading) return <PageLoader />

  if (!book) {
    return (
      <div className="text-center py-20">
        <BookX size={48} className="mx-auto mb-4 text-gray-300 dark:text-gray-600" />
        <p className="text-lg text-gray-500">Book not found</p>
        <button onClick={() => navigate('/catalog')} className="btn-primary mt-4">
          Back to catalog
        </button>
      </div>
    )
  }

  const isAvailable = (book.availableCopies ?? 0) > 0
  const coverBg = `hsl(${book.id.charCodeAt(1) * 20}, 35%, 45%)`

  const coverSrcs = [
    ...(book.images ?? []),
    book.isbn ? `https://covers.openlibrary.org/b/isbn/${book.isbn}-L.jpg?default=false` : null,
  ].filter(Boolean) as string[]
  const currentCoverSrc = coverSrcs[imgSrcIndex]

  return (
    <div className="max-w-5xl mx-auto">
      <button
        onClick={() => navigate(-1)}
        className="text-sm text-gray-500 dark:text-gray-400 hover:text-primary-700 dark:hover:text-primary-400 mb-6 flex items-center gap-1"
      >
        <ArrowLeft size={16} /> Back
      </button>

      <div className="flex flex-col md:flex-row gap-8">
        {/* Cover */}
        <div className="w-full md:w-56 shrink-0">
          <div
            className="rounded-lg overflow-hidden shadow"
            style={{ aspectRatio: '2/3' }}
          >
            {currentCoverSrc ? (
              <img
                src={currentCoverSrc}
                alt={`Cover of ${book.title}`}
                className="w-full h-full object-cover"
                onError={() => setImgSrcIndex((i) => i + 1)}
              />
            ) : (
              <div
                className="w-full h-full flex flex-col items-center justify-center p-6 text-white"
                style={{
                  background: `linear-gradient(135deg, ${coverBg}, hsl(${book.id.charCodeAt(2) * 15}, 40%, 30%))`,
                }}
              >
                <span className="text-6xl font-bold opacity-40 mb-2">
                  {book.title?.charAt(0) ?? '?'}
                </span>
                <p className="text-sm text-center opacity-70 leading-tight">
                  {book.title}
                </p>
              </div>
            )}
          </div>

          {/* Wishlist button */}
          <button
            onClick={handleWishlist}
            className={clsx(
              'w-full mt-3 py-2 rounded-lg text-sm font-medium border transition-colors flex items-center justify-center gap-1.5',
              inWishlist
                ? 'bg-primary-700 text-white border-primary-700'
                : 'border-primary-700 text-primary-700 dark:text-primary-300 dark:border-primary-500 hover:bg-primary-50 dark:hover:bg-primary-950',
            )}
          >
            <Heart size={14} fill={inWishlist ? 'currentColor' : 'none'} />
            {inWishlist ? 'In Wishlist' : 'Add to Wishlist'}
          </button>
        </div>

        {/* Details */}
        <div className="flex-1 space-y-5">
          <div>
            <div className="flex items-center gap-2 mb-2">
              <Badge variant={getStatusVariant(book.status)}>{book.status}</Badge>
              {book.language && (
                <span className="text-xs text-gray-400 dark:text-gray-500">{book.language}</span>
              )}
            </div>
            <h1 className="text-3xl font-bold text-gray-900 dark:text-gray-100 leading-tight">
              {book.title}
            </h1>
            {book.authors && book.authors.length > 0 && (
              <p className="text-gray-500 dark:text-gray-400 mt-1">
                by {book.authors.map((a) => a.name).join(', ')}
              </p>
            )}
          </div>

          <StarRating rating={book.averageRating} size="md" showValue />

          {book.genres && book.genres.length > 0 && (
            <div className="flex flex-wrap gap-1.5">
              {book.genres.map((g) => (
                <span
                  key={g.id}
                  className="px-2.5 py-1 rounded-full text-xs bg-gray-100 dark:bg-dark-border text-gray-600 dark:text-gray-400"
                >
                  {g.name}
                </span>
              ))}
            </div>
          )}

          {book.description && (
            <p className="text-gray-700 dark:text-gray-300 text-sm leading-relaxed">
              {book.description}
            </p>
          )}

          <div className="grid grid-cols-2 gap-3 text-sm">
            {[
              { label: 'Publisher', value: book.publisher },
              { label: 'Published', value: formatDate(book.publishedDate) },
              { label: 'Category', value: book.category?.name },
              { label: 'Pages', value: book.pageCount ? String(book.pageCount) : null },
              { label: 'Available', value: `${book.availableCopies} of ${book.totalCopies}` },
              { label: 'ISBN', value: book.isbn },
            ]
              .filter((r) => r.value)
              .map((row) => (
                <div key={row.label} className="flex flex-col">
                  <span className="text-xs text-gray-400 dark:text-gray-500 uppercase tracking-wider">
                    {row.label}
                  </span>
                  <span className="text-gray-800 dark:text-gray-200 font-medium">{row.value}</span>
                </div>
              ))}
          </div>

          {/* Actions */}
          {isAuthenticated ? (
            <div className="flex gap-3 flex-wrap pt-2">
              <button
                onClick={() => setBorrowModalOpen(true)}
                disabled={actionLoading || !isAvailable}
                className="btn-primary px-6"
                title={!isAvailable ? 'No copies available' : undefined}
              >
                Borrow book
              </button>
              <button
                onClick={handleReserve}
                disabled={actionLoading}
                className="btn-secondary px-6"
              >
                {actionLoading ? 'Reserving…' : 'Reserve book'}
              </button>
            </div>
          ) : (
            <p className="text-sm text-gray-500 dark:text-gray-400 pt-2">
              <a href="/login" className="text-primary-700 dark:text-primary-400 hover:underline">
                Sign in
              </a>{' '}
              to borrow or reserve this book.
            </p>
          )}
        </div>
      </div>

      {/* Borrow modal */}
      <Modal
        isOpen={borrowModalOpen}
        onClose={() => setBorrowModalOpen(false)}
        title="Borrow book"
        size="sm"
      >
        <div className="space-y-5">
          <p className="text-sm text-gray-600 dark:text-gray-400">
            Select how many days you'd like to borrow{' '}
            <span className="font-semibold text-gray-900 dark:text-gray-100">"{book.title}"</span>.
          </p>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              Loan duration
            </label>
            <div className="grid grid-cols-4 gap-2">
              {LOAN_DAYS_OPTIONS.map((days) => (
                <button
                  key={days}
                  onClick={() => setLoanDays(days)}
                  className={clsx(
                    'py-2 rounded-lg text-sm font-medium border transition-all',
                    loanDays === days
                      ? 'bg-primary-700 text-white border-primary-700'
                      : 'border-gray-200 dark:border-dark-border text-gray-600 dark:text-gray-400 hover:border-primary-400',
                  )}
                >
                  {days}d
                </button>
              ))}
            </div>
          </div>

          <div className="flex gap-3">
            <button
              onClick={handleBorrow}
              disabled={actionLoading}
              className="btn-primary flex-1"
            >
              {actionLoading ? 'Borrowing…' : `Confirm (${loanDays} days)`}
            </button>
            <button
              onClick={() => setBorrowModalOpen(false)}
              className="btn-ghost flex-1"
            >
              Cancel
            </button>
          </div>
        </div>
      </Modal>
    </div>
  )
}
