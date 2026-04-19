import React, { memo, useMemo } from 'react'
import clsx from 'clsx'

interface StarRatingProps {
  rating: number
  maxStars?: number
  size?: 'sm' | 'md' | 'lg'
  showValue?: boolean
  interactive?: boolean
  onRate?: (value: number) => void
}

const StarRating = memo(function StarRating({
  rating,
  maxStars = 5,
  size = 'sm',
  showValue = true,
  interactive = false,
  onRate,
}: StarRatingProps) {
  const sizeClass = useMemo(() => {
    return { sm: 'text-sm', md: 'text-base', lg: 'text-xl' }[size]
  }, [size])

  const stars = useMemo(() => {
    return Array.from({ length: maxStars }, (_, i) => {
      const filled = i < Math.floor(rating)
      const partial = !filled && i < rating
      return { filled, partial, idx: i }
    })
  }, [rating, maxStars])

  return (
    <div className="flex items-center gap-1">
      <div className={clsx('flex items-center', sizeClass)}>
        {stars.map(({ filled, partial, idx }) => (
          <button
            key={idx}
            type="button"
            disabled={!interactive}
            onClick={() => interactive && onRate?.(idx + 1)}
            className={clsx(
              'leading-none',
              interactive ? 'cursor-pointer hover:scale-110 transition-transform' : 'cursor-default',
            )}
            aria-label={`${idx + 1} star${idx !== 0 ? 's' : ''}`}
          >
            {filled ? (
              <span className="text-accent-500">&#9733;</span>
            ) : partial ? (
              <span className="relative inline-block">
                <span className="text-gray-300 dark:text-gray-600">&#9733;</span>
                <span
                  className="absolute inset-0 overflow-hidden text-accent-500"
                  style={{ width: `${(rating % 1) * 100}%` }}
                >
                  &#9733;
                </span>
              </span>
            ) : (
              <span className="text-gray-300 dark:text-gray-600">&#9733;</span>
            )}
          </button>
        ))}
      </div>
      {showValue && (
        <span className={clsx('font-medium text-gray-500 dark:text-gray-400', sizeClass)}>
          {rating.toFixed(1)}
        </span>
      )}
    </div>
  )
})

export default StarRating
