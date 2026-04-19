import { useCallback } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '@/core/store/store'
import { fetchBooksThunk, fetchBookDetailThunk, clearDetail } from '../store/booksSlice'
import type { BookQueryParams } from '../services/bookService'

export function useBooks() {
  const dispatch = useDispatch<AppDispatch>()
  const { list, isLoading, error } = useSelector((state: RootState) => state.books)

  const fetch = useCallback(
    (params: BookQueryParams) => dispatch(fetchBooksThunk(params)),
    [dispatch],
  )

  return { list, isLoading, error, fetch }
}

export function useBookDetail() {
  const dispatch = useDispatch<AppDispatch>()
  const { detail, detailLoading, error } = useSelector((state: RootState) => state.books)

  const fetch = useCallback(
    (id: string) => dispatch(fetchBookDetailThunk(id)),
    [dispatch],
  )

  const clear = useCallback(() => dispatch(clearDetail()), [dispatch])

  return { book: detail, isLoading: detailLoading, error, fetch, clear }
}
