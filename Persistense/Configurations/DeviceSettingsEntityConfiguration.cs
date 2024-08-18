using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistense.Entitys;

namespace Persistense.Configurations;

public class DeviceSettingsEntityConfiguration : IEntityTypeConfiguration<DeviceSettingsEntity>
{
    public void Configure(EntityTypeBuilder<DeviceSettingsEntity> builder)
    {
        builder
            .HasKey(ex => ex.Id);

        builder
            .Property(ex => ex.NotificationChannel)
            .IsRequired();

        builder
            .Property(ex => ex.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();


        builder
            .Property(ex => ex.UpdatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(ex => ex.IsActive)
            .IsRequired();

        builder
            .HasOne(ex => ex.Owner)
            .WithMany(ex => ex.AllDevices)
            .HasForeignKey(ex => ex.CustomerId)
            .HasPrincipalKey(ex => ex.Id)
            .IsRequired();

    }
}