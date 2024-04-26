using Microsoft.EntityFrameworkCore;
using Persistense.Entitys;

namespace Persistense;

public class ApplicationDBContext : DbContext
{
    public DbSet<CustomerEntity> CustomerEntities { get; set; }
    public DbSet<NotificationEntity> Notifications { get; set; }
    
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> DBconfiguration) : base(DBconfiguration)
    {
        this.Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
    }
    
}