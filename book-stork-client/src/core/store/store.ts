import { configureStore } from '@reduxjs/toolkit'
import authReducer from '@/features/auth/store/authSlice'
import booksReducer from '@/features/books/store/booksSlice'
import wishlistReducer from '@/features/wishlist/store/wishlistSlice'
import loansReducer from '@/features/loans/store/loansSlice'
import reservationsReducer from '@/features/reservations/store/reservationsSlice'

export const store = configureStore({
  reducer: {
    auth: authReducer,
    books: booksReducer,
    wishlist: wishlistReducer,
    loans: loansReducer,
    reservations: reservationsReducer,
  },
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
