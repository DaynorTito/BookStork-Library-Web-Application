import { api } from '@/core/api/apiClient'
import type { ReservationDto, PagedResult } from '@/shared/types'

export async function fetchMyReservations(
  page = 1,
  pageSize = 10,
): Promise<PagedResult<ReservationDto>> {
  return api.get<PagedResult<ReservationDto>>(
    `/api/v1/reservations/me?page=${page}&pageSize=${pageSize}`,
  )
}

export async function createReservation(
  userId: string,
  bookId: string,
): Promise<ReservationDto> {
  return api.post<ReservationDto>('/api/v1/reservations', { userId, bookId })
}

export async function cancelReservation(reservationId: string): Promise<ReservationDto> {
  return api.post<ReservationDto>(`/api/v1/reservations/${reservationId}/cancel`)
}
