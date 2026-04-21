export const APP_NAME = 'BookStork'

export const DEFAULT_PAGE_SIZE = 8

export const LOAN_DEFAULT_DAYS = 14

export const RESERVATION_EXPIRY_DAYS = 7

export const MAX_LOANS_PER_USER = 5

export const BOOK_STATUS = {
  AVAILABLE: 'Available',
  LOANED: 'Loaned',
  RESERVED: 'Reserved',
} as const

export const LOAN_STATUS = {
  ACTIVE: 'Active',
  RETURNED: 'Returned',
  OVERDUE: 'Overdue',
} as const

export const RESERVATION_STATUS = {
  ACTIVE: 'Active',
  FULFILLED: 'Fulfilled',
  CANCELLED: 'Cancelled',
  EXPIRED: 'Expired',
} as const

export const READING_STATUS = {
  WANT_TO_READ: 'WantToRead',
  READING: 'Reading',
  READ: 'Read',
} as const
