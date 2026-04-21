import React, { memo, useCallback, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Heart } from 'lucide-react'
import type { BookDetailDto } from '@/shared/types'
import StarRating from '@/shared/components/ui/StarRating'
import Badge, { getStatusVariant } from '@/shared/components/ui/Badge'
import { useAuth } from '@/features/auth/hooks/useAuth'
import { useWishlist } from '@/features/wishlist/hooks/useWishlist'
import { useToastContext } from '@/shared/context/ToastContext'
import clsx from 'clsx'

interface BookCardProps {
  book: BookDetailDto
}

/** Returns cover URL candidates in priority order */
function getCoverUrls(book: BookDetailDto): string[] {
  const urls: string[] = []
  // The catalog API returns images[] (BookDetailDto), not coverImageUrl
  if (book.images && book.images.length > 0) urls.push(book.images[0])
  // ?default=false makes Open Library return 404 instead of a placeholder
  if (book.isbn) urls.push(`https://covers.openlibrary.org/b/isbn/${book.isbn}-M.jpg?default=false`)
  return urls
}

const BookCard = memo(function BookCard({ book }: BookCardProps) {
  const navigate = useNavigate()
  const { isAuthenticated } = useAuth()
  const { isInWishlist, add, remove, fetchAll } = useWishlist()
  const { addToast } = useToastContext()
  const [srcIndex, setSrcIndex] = useState(0)
  const [wishlistLoading, setWishlistLoading] = useState(false)

  const coverUrls = getCoverUrls(book)
  const currentSrc = coverUrls[srcIndex]

  const inWishlist = isInWishlist(book.id)

  const handleCardClick = useCallback(() => {
    navigate(`/catalog/${book.id}`)
  }, [navigate, book.id])

  const handleWishlist = useCallback(
    async (e: React.MouseEvent) => {
      e.stopPropagation()
      if (!isAuthenticated) {
        addToast('Sign in to add books to your wishlist', 'info')
        return
      }
      setWishlistLoading(true)
      try {
        if (inWishlist) {
          await remove(book.id)
          addToast(`Removed "${book.title}" from wishlist`, 'info')
        } else {
          await add(book.id, true)
          await fetchAll()
          addToast(`Added "${book.title}" to wishlist`, 'success')
        }
      } catch {
        addToast('Something went wrong', 'error')
      } finally {
        setWishlistLoading(false)
      }
    },
    [isAuthenticated, inWishlist, book, add, remove, fetchAll, addToast],
  )

  const coverBg = `hsl(${book.id.charCodeAt(1) * 20}, 35%, 45%)`

  return (
    <article
      onClick={handleCardClick}
      className={clsx(
        'flex flex-col rounded-lg overflow-hidden cursor-pointer',
        'bg-white dark:bg-dark-card border border-gray-200 dark:border-dark-border',
        'shadow-sm hover:shadow-md transition-shadow',
      )}
      aria-label={`${book.title} by ${book.authors?.map((a) => a.name).join(', ')}`}
    >
      {/* Cover image */}
      <div className="relative overflow-hidden" style={{ aspectRatio: '2/3' }}>
        {currentSrc ? (
          <img
            src={currentSrc}
            alt={`Cover of ${book.title}`}
            className="w-full h-full object-cover"
            onError={() => setSrcIndex((i) => i + 1)}
            loading="lazy"
          />
        ) : (
          <div
            className="w-full h-full flex flex-col items-center justify-center p-4 text-white"
            style={{
              background: `linear-gradient(135deg, ${coverBg}, hsl(${book.id.charCodeAt(2) * 15}, 40%, 30%))`,
            }}
          >
            <span className="text-4xl font-bold opacity-40 mb-2">
              {book.title?.charAt(0) ?? '?'}
            </span>
            <p className="text-xs text-center opacity-70 leading-tight line-clamp-3">
              {book.title}
            </p>
          </div>
        )}

        <div className="absolute top-2 left-2">
          <Badge variant={getStatusVariant(book.status)}>{book.status}</Badge>
        </div>

        <button
          onClick={handleWishlist}
          disabled={wishlistLoading}
          aria-label={inWishlist ? 'Remove from wishlist' : 'Add to wishlist'}
          className={clsx(
            'absolute top-2 right-2 w-8 h-8 rounded-full flex items-center justify-center transition-colors shadow',
            inWishlist
              ? 'bg-primary-700 text-white'
              : 'bg-white dark:bg-dark-card text-gray-400 hover:text-primary-700',
          )}
        >
          <Heart size={14} fill={inWishlist ? 'currentColor' : 'none'} />
        </button>
      </div>

      <div className="flex flex-col flex-1 p-3 gap-1">
        <h3 className="font-semibold text-gray-900 dark:text-gray-100 text-sm leading-snug line-clamp-2">
          {book.title}
        </h3>
        <p className="text-xs text-gray-500 dark:text-gray-400 line-clamp-1">
          {book.authors?.map((a) => a.name).join(', ')}
        </p>

        <div className="mt-auto pt-2 flex items-center justify-between">
          <StarRating rating={book.averageRating} size="sm" showValue />
          <span className="text-xs text-gray-400 dark:text-gray-500">
            {book.availableCopies}/{book.totalCopies}
          </span>
        </div>

        {book.genres && book.genres.length > 0 && (
          <div className="flex flex-wrap gap-1 mt-1">
            {book.genres.slice(0, 2).map((g) => (
              <span
                key={g.id}
                className="text-[10px] px-1.5 py-0.5 rounded bg-gray-100 dark:bg-dark-border text-gray-500 dark:text-gray-400"
              >
                {g.name}
              </span>
            ))}
          </div>
        )}
      </div>
    </article>
  )
})

export default BookCard
