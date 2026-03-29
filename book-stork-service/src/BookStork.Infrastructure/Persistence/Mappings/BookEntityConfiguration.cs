using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public class BookEntityConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("ISBN");

        builder.HasIndex(b => b.ISBN)
            .IsUnique()
            .HasDatabaseName("IX_Books_ISBN");

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(254)
            .HasColumnName("Author");

        builder.Property(b => b.Publisher)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Publisher");

        builder.Property(b => b.PublishedDate)
            .IsRequired()
            .HasColumnName("PublishedDate");

        builder.Property(b => b.Description)
            .IsRequired()
            .HasColumnName("Description");

        builder.Property(b => b.PageCount)
            .IsRequired()
            .HasColumnName("PageCount");

        builder.Property(b => b.Height)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasColumnName("Height");

        builder.Property(b => b.Weight)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasColumnName("Weight");

        builder.Property(b => b.Thickness)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasColumnName("Thickness");

        builder.Property(b => b.AverageRating)
            .IsRequired()
            .HasPrecision(3, 2)
            .HasColumnName("AverageRating");

        builder.Property(b => b.Language)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("Language");

        builder.Property(b => b.CategoryId)
            .IsRequired()
            .HasColumnName("CategoryId");

        builder.HasMany(b => b.Images)
            .WithOne(i => i.Book)
            .HasForeignKey(i => i.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
