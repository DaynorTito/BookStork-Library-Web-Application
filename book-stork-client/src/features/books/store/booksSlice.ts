import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import type { BookDetailDto, PagedResult } from '@/shared/types'
import { fetchBooks, fetchBookById, type BookQueryParams } from '../services/bookService'

interface BooksState {
  list: PagedResult<BookDetailDto> | null
  detail: BookDetailDto | null
  isLoading: boolean
  detailLoading: boolean
  error: string | null
}

const initialState: BooksState = {
  list: null,
  detail: null,
  isLoading: false,
  detailLoading: false,
  error: null,
}

export const fetchBooksThunk = createAsyncThunk(
  'books/fetchList',
  async (params: BookQueryParams, { rejectWithValue }) => {
    try {
      return await fetchBooks(params)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const fetchBookDetailThunk = createAsyncThunk(
  'books/fetchDetail',
  async (id: string, { rejectWithValue }) => {
    try {
      const book = await fetchBookById(id)
      if (!book) throw new Error('Book not found')
      return book
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

const booksSlice = createSlice({
  name: 'books',
  initialState,
  reducers: {
    clearDetail(state) {
      state.detail = null
      state.error = null
    },
  },
  extraReducers(builder) {
    builder
      .addCase(fetchBooksThunk.pending, (state) => { state.isLoading = true; state.error = null })
      .addCase(fetchBooksThunk.fulfilled, (state, a) => { state.isLoading = false; state.list = a.payload })
      .addCase(fetchBooksThunk.rejected, (state, a) => { state.isLoading = false; state.error = a.payload as string })
      .addCase(fetchBookDetailThunk.pending, (state) => { state.detailLoading = true; state.error = null })
      .addCase(fetchBookDetailThunk.fulfilled, (state, a) => { state.detailLoading = false; state.detail = a.payload })
      .addCase(fetchBookDetailThunk.rejected, (state, a) => { state.detailLoading = false; state.error = a.payload as string })
  },
})

export const { clearDetail } = booksSlice.actions
export default booksSlice.reducer
