import React, { memo, useCallback } from 'react'
import { ChevronLeft, ChevronRight } from 'lucide-react'
import clsx from 'clsx'

interface PaginationProps {
  page: number
  totalPages: number
  onPageChange: (page: number) => void
}

const Pagination = memo(function Pagination({ page, totalPages, onPageChange }: PaginationProps) {
  const handlePrev = useCallback(() => onPageChange(page - 1), [page, onPageChange])
  const handleNext = useCallback(() => onPageChange(page + 1), [page, onPageChange])

  if (totalPages <= 1) return null

  const pages = Array.from({ length: totalPages }, (_, i) => i + 1).filter(
    (p) => p === 1 || p === totalPages || Math.abs(p - page) <= 1,
  )

  const withEllipsis: (number | '...')[] = []
  pages.forEach((p, i) => {
    if (i > 0 && p - (pages[i - 1] as number) > 1) withEllipsis.push('...')
    withEllipsis.push(p)
  })

  const btnBase = 'flex items-center justify-center h-9 rounded-lg text-sm font-medium transition-colors border'
  const btnActive = 'bg-primary-700 text-white border-primary-700'
  const btnNormal = 'border-gray-200 dark:border-dark-border text-gray-600 dark:text-gray-400 hover:bg-gray-50 dark:hover:bg-dark-border'
  const btnDisabled = 'border-transparent text-gray-300 dark:text-gray-600 cursor-not-allowed'

  return (
    <nav className="flex items-center justify-center gap-1 mt-8" aria-label="Pagination">
      <button
        onClick={handlePrev}
        disabled={page === 1}
        className={clsx(btnBase, 'px-3 gap-1', page === 1 ? btnDisabled : btnNormal)}
        aria-label="Previous page"
      >
        <ChevronLeft size={16} />
        <span>Prev</span>
      </button>

      {withEllipsis.map((p, i) =>
        p === '...' ? (
          <span key={`ellipsis-${i}`} className="w-9 h-9 flex items-center justify-center text-gray-400 text-sm">
            …
          </span>
        ) : (
          <button
            key={p}
            onClick={() => onPageChange(p as number)}
            className={clsx(btnBase, 'w-9', p === page ? btnActive : btnNormal)}
            aria-current={p === page ? 'page' : undefined}
          >
            {p}
          </button>
        ),
      )}

      <button
        onClick={handleNext}
        disabled={page === totalPages}
        className={clsx(btnBase, 'px-3 gap-1', page === totalPages ? btnDisabled : btnNormal)}
        aria-label="Next page"
      >
        <span>Next</span>
        <ChevronRight size={16} />
      </button>
    </nav>
  )
})

export default Pagination