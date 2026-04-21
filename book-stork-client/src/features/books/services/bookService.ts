import { api } from '@/core/api/apiClient'
import type { BookListDto, BookDetailDto, PagedResult } from '@/shared/types'

export interface BookQueryParams {
  Title?: string
  AuthorId?: string
  GenreId?: string
  CategoryId?: string
  Keyword?: string
  Language?: string
  SortBy?: string
  Ascending?: boolean
  Page?: number
  PageSize?: number
}

export async function fetchBooks(params: BookQueryParams = {}): Promise<PagedResult<BookDetailDto>> {
  const search = new URLSearchParams()
  for (const [key, value] of Object.entries(params)) {
    if (value !== undefined && value !== null && value !== '') {
      search.set(key, String(value))
    }
  }
  const query = search.toString()
  return api.get<PagedResult<BookDetailDto>>(`/api/v1/books${query ? `?${query}` : ''}`)
}

export async function fetchBookById(id: string): Promise<BookDetailDto | null> {
  try {
    return await api.get<BookDetailDto>(`/api/v1/books/${id}`)
  } catch {
    return null
  }
}
