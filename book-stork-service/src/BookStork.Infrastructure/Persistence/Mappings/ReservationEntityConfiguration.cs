using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class ReservationEntityConfiguration : IEntityTypeConfiguration<ReservationEntity>
{
    public void Configure(EntityTypeBuilder<ReservationEntity> b)
    {
        b.ToTable("Reservations");
        b.HasKey(r => r.Id);
        b.Property(r => r.Id).ValueGeneratedNever();
        b.Property(r => r.Status).IsRequired().HasMaxLength(20);
        b.HasOne(r => r.User).WithMany(u => u.Reservations).HasForeignKey(r => r.UserId);
        b.HasOne(r => r.Book).WithMany(bk => bk.Reservations).HasForeignKey(r => r.BookId);
        b.HasIndex(r => new { r.UserId, r.BookId, r.Status });
    }
}
