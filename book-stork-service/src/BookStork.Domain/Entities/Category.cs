namespace BookStork.Domain.Entities;

public class Category : Entity<Guid>
{
    public Category(Guid id, string name, string description) : base(id)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
