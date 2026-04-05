using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class BookImageEntityConfiguration : IEntityTypeConfiguration<BookImageEntity>
{
    public void Configure(EntityTypeBuilder<BookImageEntity> b)
    {
        b.ToTable("BookImages");
        b.HasKey(i => i.Id);
        b.Property(i => i.Id).ValueGeneratedNever();
        b.Property(i => i.Url).IsRequired().HasMaxLength(500);
    }
}

