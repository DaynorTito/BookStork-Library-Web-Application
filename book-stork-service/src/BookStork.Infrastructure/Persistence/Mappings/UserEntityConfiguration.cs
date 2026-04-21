using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStork.Infrastructure.Persistence.Mappings;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{

    public void Configure(EntityTypeBuilder<UserEntity> b)
    {
        b.ToTable("Users");
        b.HasKey(u => u.Id);
        b.Property(u => u.Id).ValueGeneratedNever();
        b.Property(u => u.Email).IsRequired().HasMaxLength(254);
        b.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        b.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        b.Property(u => u.PasswordHash).IsRequired();
        b.Property(u => u.Status).IsRequired().HasMaxLength(20);
        b.Property(u => u.LoanLimit).IsRequired();
        b.Property(u => u.NotificationPreference).IsRequired().HasMaxLength(30);
        b.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_Users_Email");    
    }
}