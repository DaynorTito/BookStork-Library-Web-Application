import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import type { ReservationDto, PagedResult } from '@/shared/types'
import { fetchMyReservations, createReservation, cancelReservation } from '../services/reservationService'

interface ReservationsState {
  list: PagedResult<ReservationDto> | null
  isLoading: boolean
  error: string | null
}

const initialState: ReservationsState = { list: null, isLoading: false, error: null }

export const fetchReservationsThunk = createAsyncThunk(
  'reservations/fetch',
  async ({ page, pageSize }: { page?: number; pageSize?: number }, { rejectWithValue }) => {
    try {
      return await fetchMyReservations(page, pageSize)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const createReservationThunk = createAsyncThunk(
  'reservations/create',
  async ({ userId, bookId }: { userId: string; bookId: string }, { rejectWithValue }) => {
    try {
      return await createReservation(userId, bookId)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const cancelReservationThunk = createAsyncThunk(
  'reservations/cancel',
  async (id: string, { rejectWithValue }) => {
    try {
      return await cancelReservation(id)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

const reservationsSlice = createSlice({
  name: 'reservations',
  initialState,
  reducers: {},
  extraReducers(builder) {
    builder
      .addCase(fetchReservationsThunk.pending, (state) => { state.isLoading = true; state.error = null })
      .addCase(fetchReservationsThunk.fulfilled, (state, a) => { state.isLoading = false; state.list = a.payload })
      .addCase(fetchReservationsThunk.rejected, (state, a) => { state.isLoading = false; state.error = a.payload as string })
      .addCase(createReservationThunk.fulfilled, (state, a) => {
        if (state.list) state.list.items = [a.payload, ...(state.list.items ?? [])]
      })
      .addCase(cancelReservationThunk.fulfilled, (state, a) => {
        if (state.list?.items) {
          const idx = state.list.items.findIndex((r) => r.id === a.payload.id)
          if (idx !== -1) state.list.items[idx] = a.payload
        }
      })
  },
})

export default reservationsSlice.reducer
