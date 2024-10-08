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
        string connectionString
        )
    {
        services.AddDbContext<ApplicationDBContext>(
            (ex) => ex.UseNpgsql(connectionString)
            , ServiceLifetime.Scoped
            );

        services.AddScoped<IBlueprintStore, BlueprintStore>();
        services.AddScoped<ICustomerStore, CustomerStore>();
        services.AddScoped<INotificationStore, NotificationStore>();
        
        return services;
    }
}