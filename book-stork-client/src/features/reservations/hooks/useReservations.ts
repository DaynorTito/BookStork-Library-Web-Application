import { useEffect, useCallback } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '@/core/store/store'
import {
  fetchReservationsThunk,
  createReservationThunk,
  cancelReservationThunk,
} from '../store/reservationsSlice'

export function useReservations() {
  const dispatch = useDispatch<AppDispatch>()
  const { list, isLoading, error } = useSelector((state: RootState) => state.reservations)
  const user = useSelector((state: RootState) => state.auth.user)

  useEffect(() => {
    dispatch(fetchReservationsThunk({}))
  }, [dispatch])

  const reserve = useCallback(
    (bookId: string) => {
      if (!user) return Promise.reject(new Error('Not authenticated'))
      return dispatch(createReservationThunk({ userId: user.id, bookId }))
    },
    [dispatch, user],
  )

  const cancel = useCallback(
    (reservationId: string) => dispatch(cancelReservationThunk(reservationId)),
    [dispatch],
  )

  const reload = useCallback(
    () => dispatch(fetchReservationsThunk({})),
    [dispatch],
  )

  return { list, isLoading, error, reserve, cancel, reload }
}
