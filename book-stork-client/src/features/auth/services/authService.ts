import { api, getToken, setToken, clearToken, getStoredUser, setStoredUser, clearStoredUser } from '@/core/api/apiClient'
import type { AuthResponse, UserDto } from '@/shared/types'
import type { LoginRequest, RegisterRequest } from '../types'

export async function registerUser(req: RegisterRequest): Promise<UserDto> {
  return api.post<UserDto>('/api/v1/auth/register', {
    email: req.email,
    firstName: req.firstName,
    lastName: req.lastName,
    password: req.password,
    loanLimit: 5,
    notificationPreference: 'Email',
  })
}

export async function loginUser(req: LoginRequest): Promise<AuthResponse> {
  const response = await api.post<AuthResponse>('/api/v1/auth/login', {
    email: req.email,
    password: req.password,
  })
  if (response.accessToken) {
    setToken(response.accessToken)
    setStoredUser(response.user)
  }
  return response
}

export function logoutUser(): void {
  clearToken()
  clearStoredUser()
}

export function getStoredAuth(): { user: UserDto | null; token: string | null } {
  return {
    user: getStoredUser<UserDto>(),
    token: getToken(),
  }
}
