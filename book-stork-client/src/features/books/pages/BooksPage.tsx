import React, { useState, useCallback, useEffect } from 'react'
import { BookX } from 'lucide-react'
import { useBooks } from '@/features/books/hooks/useBooks'
import type { BookQueryParams } from '@/features/books/services/bookService'
import BookCard from './BookCard'
import BookSearch from './BookSearch'
import BookFilters from './BookFilters'
import Pagination from '@/shared/components/Pagination'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'

const DEFAULT_PARAMS: BookQueryParams = {
  PageSize: 12,
  Page: 1,
  Ascending: true,
}

export default function BooksPage() {
  const { list, isLoading, fetch } = useBooks()
  const [params, setParams] = useState<BookQueryParams>(DEFAULT_PARAMS)

  useEffect(() => {
    fetch(params)
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [params])

  const handleSearch = useCallback((keyword: string) => {
    setParams((p) => ({ ...p, Keyword: keyword || undefined, Page: 1 }))
  }, [])

  const handleFilter = useCallback((changes: Partial<BookQueryParams>) => {
    setParams((p) => ({ ...p, ...changes, Page: 1 }))
  }, [])

  const handlePage = useCallback((page: number) => {
    setParams((p) => ({ ...p, Page: page }))
  }, [])

  return (
    <div>
      <div className="flex flex-col sm:flex-row gap-3 mb-6 items-start sm:items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-gray-100">
          Book Catalog
        </h1>
        <BookSearch onSearch={handleSearch} />
      </div>

      <div className="flex flex-col sm:flex-row gap-6">
        <div className="w-full sm:w-52 shrink-0">
          <BookFilters params={params} onChange={handleFilter} />
        </div>

        <div className="flex-1 min-w-0">
          {isLoading ? (
            <PageLoader />
          ) : list?.items && list.items.length > 0 ? (
            <>
              <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">
                {list.totalCount} book{list.totalCount !== 1 ? 's' : ''} found
              </p>
              <div className="grid grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-4">
                {list.items.map((book) => (
                  <BookCard key={book.id} book={book} />
                ))}
              </div>
              <Pagination
                page={list.page}
                totalPages={list.totalPages}
                onPageChange={handlePage}
              />
            </>
          ) : (
            <div className="text-center py-20 text-gray-500 dark:text-gray-400">
              <BookX size={48} className="mx-auto mb-4 text-gray-300 dark:text-gray-600" />
              <p className="text-lg font-medium">No books found</p>
              <p className="text-sm mt-1">Try adjusting your search or filters</p>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}
