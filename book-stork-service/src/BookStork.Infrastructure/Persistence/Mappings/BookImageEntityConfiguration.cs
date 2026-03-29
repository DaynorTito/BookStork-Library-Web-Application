using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public class BookImageEntityConfiguration : IEntityTypeConfiguration<BookImageEntity>
{
    public void Configure(EntityTypeBuilder<BookImageEntity> builder)
    {
        builder.ToTable("BookImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("Url");

        builder.Property(i => i.IsPrimary)
            .IsRequired()
            .HasColumnName("IsPrimary");

        builder.Property(i => i.BookId)
            .IsRequired()
            .HasColumnName("BookId");
    }
}
