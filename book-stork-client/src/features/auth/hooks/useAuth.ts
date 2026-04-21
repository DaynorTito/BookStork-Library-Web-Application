import { useCallback } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import type { RootState, AppDispatch } from '@/core/store/store'
import { loginThunk, registerThunk, logout, clearError } from '../store/authSlice'
import { clearWishlist } from '@/features/wishlist/store/wishlistSlice'
import type { LoginRequest, RegisterRequest } from '../types'

export function useAuth() {
  const dispatch = useDispatch<AppDispatch>()
  const { user, token, isLoading, error, isAuthenticated } = useSelector(
    (state: RootState) => state.auth,
  )

  const login = useCallback(
    (req: LoginRequest) => dispatch(loginThunk(req)),
    [dispatch],
  )

  const register = useCallback(
    (req: RegisterRequest) => dispatch(registerThunk(req)),
    [dispatch],
  )

  const logoutUser = useCallback(() => {
    dispatch(logout())
    dispatch(clearWishlist())
  }, [dispatch])

  const resetError = useCallback(() => dispatch(clearError()), [dispatch])

  return { user, token, isLoading, error, isAuthenticated, login, register, logout: logoutUser, resetError }
}
