import { useCallback } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '@/core/store/store'
import {
  fetchWishlistThunk,
  addToWishlistThunk,
  removeFromWishlistThunk,
  toggleNotifyThunk,
} from '../store/wishlistSlice'

export function useWishlist() {
  const dispatch = useDispatch<AppDispatch>()
  const { items, isLoading, error } = useSelector((state: RootState) => state.wishlist)

  const isInWishlist = useCallback(
    (bookId: string) => items.some((item) => item.book?.id === bookId),
    [items],
  )

  const fetchAll = useCallback(() => dispatch(fetchWishlistThunk()), [dispatch])

  const add = useCallback(
    (bookId: string, notify: boolean) => dispatch(addToWishlistThunk({ bookId, notify })),
    [dispatch],
  )

  const remove = useCallback(
    (bookId: string) => dispatch(removeFromWishlistThunk(bookId)),
    [dispatch],
  )

  const toggleNotify = useCallback(
    (bookId: string, notify: boolean) => dispatch(toggleNotifyThunk({ bookId, notify })),
    [dispatch],
  )

  return { items, isLoading, error, isInWishlist, add, remove, toggleNotify, fetchAll }
}
