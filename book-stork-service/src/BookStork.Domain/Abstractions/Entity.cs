namespace BookStork.Domain.Entities;

public abstract class Entity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];
 
    protected Entity(TId id)
    {
        Id = id;
    }
 
    public TId Id { get; protected set; }
 
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
 
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
 
    public void ClearDomainEvents()
        => _domainEvents.Clear();
 
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }
 
    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);
 
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => left?.Equals(right) ?? right is null;
 
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !(left == right);
}
