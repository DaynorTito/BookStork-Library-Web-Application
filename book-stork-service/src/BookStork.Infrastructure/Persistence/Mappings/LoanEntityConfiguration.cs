using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class LoanEntityConfiguration : IEntityTypeConfiguration<LoanEntity>
{
    public void Configure(EntityTypeBuilder<LoanEntity> b)
    {
        b.ToTable("Loans");
        b.HasKey(l => l.Id);
        b.Property(l => l.Id).ValueGeneratedNever();
        b.Property(l => l.Status).IsRequired().HasMaxLength(20);
        b.HasOne(l => l.User).WithMany(u => u.Loans).HasForeignKey(l => l.UserId);
        b.HasOne(l => l.Book).WithMany(bk => bk.Loans).HasForeignKey(l => l.BookId);
        b.HasIndex(l => new { l.UserId, l.Status });
        b.HasIndex(l => new { l.BookId, l.Status });
    }
}