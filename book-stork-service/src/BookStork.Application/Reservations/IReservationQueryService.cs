using BookStork.Application.DTOs;

namespace BookStork.Application.Reservations;

public interface IReservationQueryService
{
    Task<ReservationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<ReservationDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<(IReadOnlyList<ReservationDto> Items, int TotalCount)> GetByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
}