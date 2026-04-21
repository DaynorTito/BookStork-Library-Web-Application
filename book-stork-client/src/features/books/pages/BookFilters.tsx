import React, { memo } from 'react'
import type { BookQueryParams } from '@/features/books/services/bookService'

interface BookFiltersProps {
  params: BookQueryParams
  onChange: (changes: Partial<BookQueryParams>) => void
}

const LANGUAGES = ['English', 'Spanish', 'French', 'German', 'Portuguese', 'Italian']
const SORT_OPTIONS = [
  { value: 'title', label: 'Title' },
  { value: 'rating', label: 'Rating' },
  { value: 'date', label: 'Date' },
]

const BookFilters = memo(function BookFilters({ params, onChange }: BookFiltersProps) {
  return (
    <aside className="space-y-5 p-4 rounded-xl border border-gray-100 dark:border-dark-border bg-white dark:bg-dark-card">
      <div>
        <h3 className="text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wider mb-2">
          Language
        </h3>
        <select
          value={params.Language ?? ''}
          onChange={(e) => onChange({ Language: e.target.value || undefined })}
          className="input-base text-sm"
        >
          <option value="">All languages</option>
          {LANGUAGES.map((lang) => (
            <option key={lang} value={lang}>
              {lang}
            </option>
          ))}
        </select>
      </div>

      <div>
        <h3 className="text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wider mb-2">
          Sort by
        </h3>
        <select
          value={params.SortBy ?? ''}
          onChange={(e) => onChange({ SortBy: e.target.value || undefined })}
          className="input-base text-sm"
        >
          <option value="">Default</option>
          {SORT_OPTIONS.map((opt) => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
      </div>

      {params.SortBy && (
        <div>
          <h3 className="text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wider mb-2">
            Order
          </h3>
          <div className="flex gap-2">
            {[
              { value: true, label: 'Asc' },
              { value: false, label: 'Desc' },
            ].map((opt) => (
              <button
                key={String(opt.value)}
                onClick={() => onChange({ Ascending: opt.value })}
                className={`flex-1 text-sm py-1.5 rounded-lg border font-medium transition-all ${
                  params.Ascending === opt.value
                    ? 'border-primary-700 bg-primary-50 dark:bg-primary-950 text-primary-700 dark:text-primary-300'
                    : 'border-gray-200 dark:border-dark-border text-gray-600 dark:text-gray-400 hover:border-primary-400'
                }`}
              >
                {opt.label}
              </button>
            ))}
          </div>
        </div>
      )}

      <button
        onClick={() => onChange({ Language: undefined, SortBy: undefined, Ascending: true })}
        className="w-full text-xs text-gray-400 hover:text-primary-700 dark:hover:text-primary-400 transition-colors py-1"
      >
        Clear filters
      </button>
    </aside>
  )
})

export default BookFilters
