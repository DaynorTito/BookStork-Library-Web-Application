using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class GenreEntityConfiguration : IEntityTypeConfiguration<GenreEntity>
{
    public void Configure(EntityTypeBuilder<GenreEntity> b)
    {
        b.ToTable("Genres");
        b.HasKey(g => g.Id);
        b.Property(g => g.Id).ValueGeneratedNever();
        b.Property(g => g.Name).IsRequired().HasMaxLength(100);
        b.HasIndex(g => g.Name).IsUnique();
    }
}