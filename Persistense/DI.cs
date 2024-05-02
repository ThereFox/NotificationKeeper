using App.Stores;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistense.Stores;

namespace Persistense;

public static class DI
{
    public static IServiceCollection AddPersistense(this IServiceCollection services, Action<DbContextOptionsBuilder> dbConfiguration)
    {
        services.AddDbContext<ApplicationDBContext>(dbConfiguration);

        services.AddTransient<IBlueprintStore, BlueprintStore>();
        services.AddTransient<ICustomerStore, CustomerStore>();
        services.AddTransient<INotificationStore, NotificationStore>();
        
        return services;
    }
}