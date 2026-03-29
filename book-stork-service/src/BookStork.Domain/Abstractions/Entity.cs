using BookStork.Domain.Abstractions;

namespace BookStork.Domain.Entities;

public abstract class Entity<TId> : AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
 
    protected Entity(TId id) => Id = id;
    protected Entity() { }

    public TId Id { get; protected set; } = default!;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
    public static bool operator ==(Entity<TId>? a, Entity<TId>? b) => a?.Equals(b) ?? b is null;
    public static bool operator !=(Entity<TId>? a, Entity<TId>? b) => !(a == b);
}
