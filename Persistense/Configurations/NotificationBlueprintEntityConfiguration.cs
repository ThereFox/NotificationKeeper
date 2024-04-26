using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistense.Entitys;

namespace Persistense.Configurations;

public class NotificationBlueprintEntityConfiguration : IEntityTypeConfiguration<NotificationBlueprintEntity>
{
    public void Configure(EntityTypeBuilder<NotificationBlueprintEntity> builder)
    {
        builder
            .HasKey(ex => ex.Id);

        builder
            .Property(ex => ex.Name)
            .HasColumnType("varchar")
            .IsRequired();
        
        builder
            .Property(ex => ex.Subject)
            .HasColumnType("varchar")
            .IsRequired();
        
        builder
            .Property(ex => ex.Content)
            .HasColumnType("text")
            .IsRequired();

        builder
            .Property(ex => ex.Channel)
            .IsRequired();

        builder
            .Property(ex => ex.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .HasMany(ex => ex.UsedIn)
            .WithOne(ex => ex.Blueprint)
            .HasPrincipalKey(ex => ex.Id)
            .HasForeignKey(ex => ex.BlueprintId)
            .IsRequired();

    }
}