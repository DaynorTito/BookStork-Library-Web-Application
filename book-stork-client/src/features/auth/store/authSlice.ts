import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import type { AuthState } from '../types'
import type { LoginRequest, RegisterRequest } from '../types'
import { loginUser, logoutUser, registerUser, getStoredAuth } from '../services/authService'

const stored = getStoredAuth()

const initialState: AuthState = {
  user: stored.user,
  token: stored.token,
  isLoading: false,
  error: null,
  isAuthenticated: !!stored.token && !!stored.user,
}

export const loginThunk = createAsyncThunk('auth/login', async (req: LoginRequest, { rejectWithValue }) => {
  try {
    return await loginUser(req)
  } catch (err) {
    return rejectWithValue((err as Error).message)
  }
})

export const registerThunk = createAsyncThunk('auth/register', async (req: RegisterRequest, { rejectWithValue }) => {
  try {
    return await registerUser(req)
  } catch (err) {
    return rejectWithValue((err as Error).message)
  }
})

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logout(state) {
      logoutUser()
      state.user = null
      state.token = null
      state.isAuthenticated = false
      state.error = null
    },
    clearError(state) {
      state.error = null
    },
    updateUser(state, action) {
      if (state.user) {
        state.user = { ...state.user, ...action.payload }
      }
    },
  },
  extraReducers(builder) {
    builder
      .addCase(loginThunk.pending, (state) => {
        state.isLoading = true
        state.error = null
      })
      .addCase(loginThunk.fulfilled, (state, action) => {
        state.isLoading = false
        state.user = action.payload.user
        state.token = action.payload.accessToken
        state.isAuthenticated = true
      })
      .addCase(loginThunk.rejected, (state, action) => {
        state.isLoading = false
        state.error = action.payload as string
      })
      .addCase(registerThunk.pending, (state) => {
        state.isLoading = true
        state.error = null
      })
      .addCase(registerThunk.fulfilled, (state) => {
        state.isLoading = false
      })
      .addCase(registerThunk.rejected, (state, action) => {
        state.isLoading = false
        state.error = action.payload as string
      })
  },
})

export const { logout, clearError, updateUser } = authSlice.actions
export default authSlice.reducer
