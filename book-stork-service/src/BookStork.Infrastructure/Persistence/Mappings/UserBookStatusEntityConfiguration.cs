using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class UserBookStatusEntityConfiguration : IEntityTypeConfiguration<UserBookStatusEntity>
{
    public void Configure(EntityTypeBuilder<UserBookStatusEntity> b)
    {
        b.ToTable("UserBookStatuses");
        b.HasKey(s => s.Id);
        b.Property(s => s.Id).ValueGeneratedNever();
        b.Property(s => s.Status).IsRequired().HasMaxLength(20);
        b.HasIndex(s => new { s.UserId, s.BookId }).IsUnique();
        b.HasOne(s => s.User).WithMany(u => u.BookStatuses).HasForeignKey(s => s.UserId);
        b.HasOne(s => s.Book).WithMany().HasForeignKey(s => s.BookId);
    }
}