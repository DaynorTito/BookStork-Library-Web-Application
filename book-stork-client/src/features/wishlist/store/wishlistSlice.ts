import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import type { WishlistItemDto } from '@/shared/types'
import {
  fetchWishlist,
  addToWishlist,
  removeFromWishlist,
  toggleWishlistNotification,
} from '../services/wishlistService'

interface WishlistState {
  items: WishlistItemDto[]
  isLoading: boolean
  error: string | null
}

const initialState: WishlistState = { items: [], isLoading: false, error: null }

export const fetchWishlistThunk = createAsyncThunk(
  'wishlist/fetch',
  async (_, { rejectWithValue }) => {
    try {
      return await fetchWishlist()
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const addToWishlistThunk = createAsyncThunk(
  'wishlist/add',
  async ({ bookId, notify }: { bookId: string; notify: boolean }, { rejectWithValue }) => {
    try {
      return await addToWishlist(bookId, notify)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const removeFromWishlistThunk = createAsyncThunk(
  'wishlist/remove',
  async (bookId: string, { rejectWithValue }) => {
    try {
      await removeFromWishlist(bookId)
      return bookId
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

export const toggleNotifyThunk = createAsyncThunk(
  'wishlist/toggleNotify',
  async ({ bookId, notify }: { bookId: string; notify: boolean }, { rejectWithValue }) => {
    try {
      return await toggleWishlistNotification(bookId, notify)
    } catch (e) {
      return rejectWithValue((e as Error).message)
    }
  },
)

const wishlistSlice = createSlice({
  name: 'wishlist',
  initialState,
  reducers: {
    clearWishlist(state) {
      state.items = []
    },
  },
  extraReducers(builder) {
    builder
      .addCase(fetchWishlistThunk.pending, (state) => { state.isLoading = true })
      .addCase(fetchWishlistThunk.fulfilled, (state, a) => { state.isLoading = false; state.items = a.payload })
      .addCase(fetchWishlistThunk.rejected, (state, a) => { state.isLoading = false; state.error = a.payload as string })
      .addCase(addToWishlistThunk.fulfilled, (state, a) => {
        // Only push if the item has a valid book (some API responses omit it)
        if (a.payload?.book) {
          state.items.push(a.payload)
        }
      })
      .addCase(removeFromWishlistThunk.fulfilled, (state, a) => {
        state.items = state.items.filter((i) => i.book?.id !== a.payload)
      })
      .addCase(toggleNotifyThunk.fulfilled, (state, a) => {
        const idx = state.items.findIndex((i) => i.id === a.payload.id)
        if (idx !== -1) {
          // API may return the item without the nested book — preserve the existing one
          state.items[idx] = {
            ...state.items[idx],
            ...a.payload,
            book: a.payload.book ?? state.items[idx].book,
          }
        }
      })
  },
})

export const { clearWishlist } = wishlistSlice.actions
export default wishlistSlice.reducer
