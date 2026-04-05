using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class WishlistItemEntityConfiguration : IEntityTypeConfiguration<WishlistItemEntity>
{
    public void Configure(EntityTypeBuilder<WishlistItemEntity> b)
    {
        b.ToTable("WishlistItems");
        b.HasKey(w => w.Id);
        b.Property(w => w.Id).ValueGeneratedNever();
        b.Property(w => w.NotifyOnAvailable).IsRequired();
        b.Property(w => w.AddedAt).IsRequired();

        b.HasIndex(w => new { w.UserId, w.BookId }).IsUnique();

        b.HasOne(w => w.User).WithMany().HasForeignKey(w => w.UserId);
        b.HasOne(w => w.Book).WithMany().HasForeignKey(w => w.BookId);
    }
}
