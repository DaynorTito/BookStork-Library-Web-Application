using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Reservation;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _ctx;
    public ReservationRepository(AppDbContext ctx) => _ctx = ctx;
 
    public async Task<Reservation?> GetByIdAsync(ReservationId id, CancellationToken ct = default)
    {
        var e = await _ctx.Reservations
            .Include(l => l.Book)
            .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id.Value, ct);
        return e is null ? null : ReservationMapper.ToDomain(e);
    }
 
    public async Task<Reservation?> GetPendingByUserAndBookAsync(UserId userId, BookId bookId, CancellationToken ct = default)
    {
        var e = await _ctx.Reservations
            .Include(l => l.Book)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.UserId == userId.Value && r.BookId == bookId.Value && r.Status == "PENDING", ct);
        return e is null ? null : ReservationMapper.ToDomain(e);
    }
 
    public async Task<IReadOnlyList<Reservation>> GetPendingByBookAsync(BookId bookId, CancellationToken ct = default)
    {
        var list = await _ctx.Reservations.AsNoTracking()
            .Include(l => l.Book)
            .Where(r => r.BookId == bookId.Value && r.Status == "PENDING")
            .OrderBy(r => r.ReservedAt).ToListAsync(ct);
        return list.Select(ReservationMapper.ToDomain).ToList().AsReadOnly();
    }

    public async Task<(IReadOnlyList<Reservation> Reservations, int TotalCount)> GetAllByUserAsync(UserId userId, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Reservations.AsNoTracking()
            .Include(l => l.Book)
            .Where(r => r.UserId == userId.Value)   
            .OrderByDescending(r => r.ReservedAt);
        var total = await q.CountAsync(ct);
        var list = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (list.Select(ReservationMapper.ToDomain).ToList().AsReadOnly(), total);
    }

    public async Task<(IReadOnlyList<Reservation> Reservations, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Reservations
            .Include(l => l.Book)
            .AsNoTracking().OrderByDescending(r => r.ReservedAt);
        var total = await q.CountAsync(ct);
        var list = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (list.Select(ReservationMapper.ToDomain).ToList().AsReadOnly(), total);
    }
 
    public async Task AddAsync(Reservation r, CancellationToken ct = default)
        => await _ctx.Reservations.AddAsync(ReservationMapper.ToEntity(r), ct);
 
    public void Update(Reservation r)
    {
        var e = _ctx.Reservations.Local.FirstOrDefault(x => x.Id == r.Id.Value);
        if (e is null) { e = ReservationMapper.ToEntity(r); _ctx.Reservations.Attach(e); }
        else ReservationMapper.Update(e, r);
        _ctx.Entry(e).State = EntityState.Modified;
    }
 
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}