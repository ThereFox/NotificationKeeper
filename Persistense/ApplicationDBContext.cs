using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Persistense.Entitys;

namespace Persistense;

public class ApplicationDBContext : DbContext
{
    public DbSet<CustomerEntity> CustomerEntities { get; private set; }
    public DbSet<NotificationEntity> Notifications { get; private set; }
    public DbSet<NotificationBlueprintEntity> Blueprints { get; private set; }
    public DbSet<DeviceSettingsEntity> Devices { get; private set; }
    
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> DBconfiguration) : base(DBconfiguration)
    {
        this.Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);

        modelBuilder.Entity<CustomerEntity>().HasData(new CustomerEntity() {  });
    }
    
}