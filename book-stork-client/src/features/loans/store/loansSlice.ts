import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import type { LoanDto, PagedResult } from '@/shared/types'
import { fetchMyLoans, createLoan, returnLoan } from '../services/loanService'

interface LoansState {
  list: PagedResult<LoanDto> | null
  isLoading: boolean
  error: string | null
}

const initialState: LoansState = { list: null, isLoading: false, error: null }

export const fetchLoansThunk = createAsyncThunk(
  'loans/fetch',
  async ({ page, pageSize }: { page?: number; pageSize?: number }, { rejectWithValue }) => {
    try {
      return await fetchMyLoans(page, pageSize)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const createLoanThunk = createAsyncThunk(
  'loans/create',
  async (
    { userId, bookId, days }: { userId: string; bookId: string; days: number },
    { rejectWithValue },
  ) => {
    try {
      return await createLoan(userId, bookId, days)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const returnLoanThunk = createAsyncThunk(
  'loans/return',
  async (loanId: string, { rejectWithValue }) => {
    try {
      return await returnLoan(loanId)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

const loansSlice = createSlice({
  name: 'loans',
  initialState,
  reducers: {},
  extraReducers(builder) {
    builder
      .addCase(fetchLoansThunk.pending, (state) => { state.isLoading = true; state.error = null })
      .addCase(fetchLoansThunk.fulfilled, (state, a) => { state.isLoading = false; state.list = a.payload })
      .addCase(fetchLoansThunk.rejected, (state, a) => { state.isLoading = false; state.error = a.payload as string })
      .addCase(createLoanThunk.fulfilled, (state, a) => {
        if (state.list) state.list.items = [a.payload, ...(state.list.items ?? [])]
      })
      .addCase(returnLoanThunk.fulfilled, (state, a) => {
        if (state.list?.items) {
          const idx = state.list.items.findIndex((l) => l.id === a.payload.id)
          if (idx !== -1) state.list.items[idx] = a.payload
        }
      })
  },
})

export default loansSlice.reducer
