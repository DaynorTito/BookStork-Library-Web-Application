export interface AuthorDto {
  id: string
  name: string | null
  biography: string | null
}

export interface CategoryDto {
  id: string
  name: string | null
  description: string | null
}

export interface GenreDto {
  id: string
  name: string | null
}

export interface BookSummaryDto {
  id: string
  isbn: string | null
  title: string | null
  authorNames: string | null
  categoryName: string | null
  status: string | null
  coverImageUrl: string | null
  averageRating: number
  language: string | null
}

export interface BookListDto {
  id: string
  isbn: string | null
  title: string | null
  authorName: string | null
  categoryName: string | null
  genres: GenreDto[] | null
  language: string | null
  averageRating: number
  status: string | null
  availableCopies: number
  totalCopies: number
  publishedDate: string
  coverImageUrl: string | null
}

export interface BookDetailDto {
  id: string
  isbn: string | null
  title: string | null
  authors: AuthorDto[] | null
  category: CategoryDto | null
  genres: GenreDto[] | null
  publisher: string | null
  publishedDate: string
  description: string | null
  pageCount: number
  height: number
  weight: number
  thickness: number
  language: string | null
  averageRating: number
  status: string | null
  availableCopies: number
  totalCopies: number
  images: string[] | null
  createdAt: string
  updatedAt: string | null
}

export interface PagedResult<T> {
  items: T[] | null
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasNextPage: boolean
  hasPreviousPage: boolean
}

export interface UserDto {
  id: string
  email: string | null
  firstName: string | null
  lastName: string | null
  fullName: string | null
  status: string | null
  loanLimit: number
  activeLoansCount: number
  notificationPreference: string | null
  createdAt: string
  updatedAt: string | null
}

export interface AuthResponse {
  accessToken: string | null
  tokenType: string | null
  expiresAt: string
  user: UserDto
}

export interface LoanDto {
  id: string
  userId: string
  userFullName: string | null
  book: BookSummaryDto
  status: string | null
  loanedAt: string
  dueDate: string
  returnedAt: string | null
  isOverdue: boolean
  daysRemaining: number
}

export interface ReservationDto {
  id: string
  userId: string
  userFullName: string | null
  book: BookSummaryDto
  status: string | null
  reservedAt: string
  expiresAt: string
  fulfilledAt: string | null
}

export interface WishlistItemDto {
  id: string
  userId: string
  book: BookSummaryDto
  notifyOnAvailable: boolean
  addedAt: string
}

export interface UserBookStatusDto {
  userId: string
  bookId: string
  bookTitle: string | null
  status: string | null
  updatedAt: string
}

export type BookStatus = 'Available' | 'Loaned' | 'Reserved'
export type LoanStatus = 'Active' | 'Returned' | 'Overdue'
export type ReservationStatus = 'Active' | 'Fulfilled' | 'Cancelled' | 'Expired'
export type ReadingStatus = 'WantToRead' | 'Reading' | 'Read'