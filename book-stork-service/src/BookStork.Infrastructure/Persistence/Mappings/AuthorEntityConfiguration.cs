using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class AuthorEntityConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> b)
    {
        b.ToTable("Authors");
        b.HasKey(a => a.Id);
        b.Property(a => a.Id).ValueGeneratedNever();
        b.Property(a => a.Name).IsRequired().HasMaxLength(200);
        b.Property(a => a.Biography).HasMaxLength(2000);
    }
}