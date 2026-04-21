using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class BookGenreEntityConfiguration : IEntityTypeConfiguration<BookGenreEntity>
{
    public void Configure(EntityTypeBuilder<BookGenreEntity> b)
    {
        b.ToTable("BookGenres");
        b.HasKey(bg => new { bg.BookId, bg.GenreId });
        b.HasOne(bg => bg.Book).WithMany(bk => bk.BookGenres).HasForeignKey(bg => bg.BookId);
        b.HasOne(bg => bg.Genre).WithMany(g => g.BookGenres).HasForeignKey(bg => bg.GenreId);
    }
}