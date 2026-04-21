import { api } from '@/core/api/apiClient'
import type { LoanDto, PagedResult } from '@/shared/types'

export async function fetchMyLoans(page = 1, pageSize = 10): Promise<PagedResult<LoanDto>> {
  return api.get<PagedResult<LoanDto>>(`/api/v1/loans/me?page=${page}&pageSize=${pageSize}`)
}

export async function createLoan(
  userId: string,
  bookId: string,
  days: number,
): Promise<LoanDto> {
  return api.post<LoanDto>('/api/v1/loans', { userId, bookId, days })
}

export async function returnLoan(loanId: string): Promise<LoanDto> {
  return api.post<LoanDto>(`/api/v1/loans/${loanId}/return`)
}
