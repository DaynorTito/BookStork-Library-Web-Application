using BookStork.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.Persistence;


public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    private readonly IMediator _mediator;
    
    public DbSet<BookEntity> Books => Set<BookEntity>();
    public DbSet<BookImageEntity> BookImages => Set<BookImageEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

  //  private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
   // {
        // Recopila todas las entidades de dominio trackeadas por EF que tienen eventos
        // NOTA: necesitamos que el ChangeTracker conozca las entidades de dominio.
        // Como usamos entidades separadas (UserEntity vs User), este enfoque
        // requiere guardar los agregados en el contexto de otra forma (ver Opción C).
        //
        // Si usas EF Core directamente sobre las entidades de dominio (sin separación),
        // puedes hacer:
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
