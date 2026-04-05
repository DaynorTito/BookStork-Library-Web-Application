using BookStork.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.Persistence;


public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<BookEntity> Books => Set<BookEntity>();
    public DbSet<AuthorEntity> Authors => Set<AuthorEntity>();
    public DbSet<GenreEntity> Genres => Set<GenreEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<BookImageEntity> BookImages => Set<BookImageEntity>();
    public DbSet<BookGenreEntity> BookGenres => Set<BookGenreEntity>();

    public DbSet<BookAuthorEntity> BookAuthors => Set<BookAuthorEntity>();

    public DbSet<LoanEntity> Loans => Set<LoanEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
    public DbSet<UserBookStatusEntity> UserBookStatuses => Set<UserBookStatusEntity>();

    public DbSet<WishlistItemEntity> WishlistItems => Set<WishlistItemEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

  //  private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
   // {

        //
        // var domainEntities = ChangeTracker.Entries<Entity<object>>()
        //     .Where(e => e.Entity.DomainEvents.Any())
        //     .Select(e => e.Entity)
        //     .ToList();
        //
        // foreach (var entity in domainEntities)
        // {
        //     var events = entity.DomainEvents.ToList();
        //     entity.ClearDomainEvents();
        //     foreach (var evt in events)
        //         await _mediator.Publish(evt, cancellationToken);
        // }
   // }
}
