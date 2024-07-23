using App.Stores;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistense.Stores;

namespace Persistense;

public static class DI
{
    public static IServiceCollection AddPersistense(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddDbContext<ApplicationDBContext>(
            (ex) => ex.UseNpgsql(
            configuration.GetConnectionString("Database")
            ?? throw new InvalidCastException("Connection string missed")
        )
            , ServiceLifetime.Scoped);

        services.AddTransient<IBlueprintStore, BlueprintStore>();
        services.AddTransient<ICustomerStore, CustomerStore>();
        services.AddTransient<INotificationStore, NotificationStore>();
        
        return services;
    }
}