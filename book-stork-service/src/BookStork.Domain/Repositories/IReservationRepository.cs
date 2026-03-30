using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Reservation;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(ReservationId id, CancellationToken ct = default);
    Task<Reservation?> GetPendingByUserAndBookAsync(UserId userId, BookId bookId, CancellationToken ct = default);
    Task<IReadOnlyList<Reservation>> GetPendingByBookAsync(BookId bookId, CancellationToken ct = default);
    Task<(IReadOnlyList<Reservation> Reservations, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Reservation reservation, CancellationToken ct = default);
    void Update(Reservation reservation);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
