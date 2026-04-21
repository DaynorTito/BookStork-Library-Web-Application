using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class BookEntityConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> b)
    {
        b.ToTable("Books");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.ISBN).IsRequired().HasMaxLength(20);
        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.Property(x => x.Publisher).IsRequired().HasMaxLength(200);
        b.Property(x => x.Description).IsRequired();
        b.Property(x => x.Language).IsRequired().HasMaxLength(10);
        b.Property(x => x.Status).IsRequired().HasMaxLength(20);
        b.Property(x => x.AverageRating).HasPrecision(3, 2);
        b.Property(x => x.Height).HasPrecision(10, 2);
        b.Property(x => x.Weight).HasPrecision(10, 2);
        b.Property(x => x.Thickness).HasPrecision(10, 2);
        b.HasIndex(x => x.ISBN).IsUnique().HasDatabaseName("IX_Books_ISBN");
        b.HasOne(x => x.Category).WithMany(c => c.Books).HasForeignKey(x => x.CategoryId);
        b.HasMany(x => x.Images).WithOne(i => i.Book).HasForeignKey(i => i.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        b.HasMany(x => x.BookGenres).WithOne(bg => bg.Book).HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        b.HasMany(x => x.BookAuthors).WithOne(bg => bg.Book).HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
