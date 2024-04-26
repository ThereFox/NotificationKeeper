using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistense.Entitys;

namespace Persistense.Configurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder
            .HasKey(ex => ex.Id);

        builder
            .Property(ex => ex.UserName)
            .HasColumnType("varchar")
            .IsRequired();
        
        builder
            .Property(ex => ex.Role)
            .HasColumnType("varchar")
            .IsRequired();

        builder
            .Property(ex => ex.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .HasMany(ex => ex.AllDevices)
            .WithOne(ex => ex.Owner)
            .HasPrincipalKey(ex => ex.Id)
            .HasForeignKey(ex => ex.CustomerId)
            .IsRequired();

        builder
            .HasMany(ex => ex.ResivedNotifications)
            .WithOne(ex => ex.Customer)
            .HasPrincipalKey(ex => ex.Id)
            .HasForeignKey(ex => ex.CustomerId)
            .IsRequired();
        
    }
}