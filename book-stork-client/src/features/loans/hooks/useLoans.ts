import { useEffect, useCallback } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '@/core/store/store'
import { fetchLoansThunk, createLoanThunk, returnLoanThunk } from '../store/loansSlice'

export function useLoans() {
  const dispatch = useDispatch<AppDispatch>()
  const { list, isLoading, error } = useSelector((state: RootState) => state.loans)
  const user = useSelector((state: RootState) => state.auth.user)

  useEffect(() => {
    dispatch(fetchLoansThunk({}))
  }, [dispatch])

  const borrow = useCallback(
    (bookId: string, days: number) => {
      if (!user) return Promise.reject(new Error('Not authenticated'))
      return dispatch(createLoanThunk({ userId: user.id, bookId, days }))
    },
    [dispatch, user],
  )

  const returnBook = useCallback(
    (loanId: string) => dispatch(returnLoanThunk(loanId)),
    [dispatch],
  )

  const reload = useCallback(
    () => dispatch(fetchLoansThunk({})),
    [dispatch],
  )

  return { list, isLoading, error, borrow, returnBook, reload }
}
