import { api } from '@/core/api/apiClient'
import type { WishlistItemDto } from '@/shared/types'

export async function fetchWishlist(): Promise<WishlistItemDto[]> {
  return api.get<WishlistItemDto[]>('/api/v1/wishlist')
}

export async function addToWishlist(
  bookId: string,
  notifyOnAvailable: boolean,
): Promise<WishlistItemDto> {
  return api.post<WishlistItemDto>('/api/v1/wishlist', { bookId, notifyOnAvailable })
}

export async function removeFromWishlist(bookId: string): Promise<void> {
  return api.delete<void>(`/api/v1/wishlist/${bookId}`)
}

export async function toggleWishlistNotification(
  bookId: string,
  notifyOnAvailable: boolean,
): Promise<WishlistItemDto> {
  return api.patch<WishlistItemDto>(`/api/v1/wishlist/${bookId}/notify`, { notifyOnAvailable })
}
