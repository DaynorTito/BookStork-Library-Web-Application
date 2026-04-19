import type { UserDto } from '@/shared/types'

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  firstName: string
  lastName: string
  password: string
  confirmPassword: string
}

export interface StoredUser extends UserDto {
  passwordHash: string
}

export interface AuthState {
  user: UserDto | null
  token: string | null
  isLoading: boolean
  error: string | null
  isAuthenticated: boolean
}
