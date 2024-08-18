using Domain.Entitys;
using Domain.ValueObject;
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

        var baseCustomer = new CustomerEntity()
        {
            Id = Guid.Parse("a7f1cf1f-5f4f-4159-99cc-80a4e9f7c5cb"),
            CreatedAt = DateTime.MinValue,
            Role = CustomerRole.Base.Value,
        };
        var baseCustomerDevice = new DeviceSettingsEntity()
        {
            IsActive = true,
            CreatedAt = DateTime.MinValue,
            NotificationChannel = NotificationChannel.Email.Value,
            Id = Guid.NewGuid(),//.Parse("a7f1cf1f-5f4f-4159-99cc-80a4e9f7c5cb"),
            UpdatedAt = DateTime.MinValue,
            CustomerId = baseCustomer.Id
        };

        //baseCustomer.AllDevices = [baseCustomerDevice];
        //baseCustomerDevice.Owner = baseCustomer;

        var hellowBlueprint = new NotificationBlueprintEntity()
        {
            Id = Guid.Parse("b7f1cf1f-5f4f-4159-99cc-80a4e9f7c5cb"),
            Channel = NotificationChannel.Email.Value,
            CreatedAt = DateTime.MinValue
        };
        
        
        modelBuilder.Entity<CustomerEntity>().HasData(baseCustomer);
        modelBuilder.Entity<DeviceSettingsEntity>().HasData(baseCustomerDevice);
        modelBuilder.Entity<NotificationBlueprintEntity>().HasData(hellowBlueprint);
    }
    
}