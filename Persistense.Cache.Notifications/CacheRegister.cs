using Microsoft.Extensions.DependencyInjection;
using Persistense.Cache.Notifications.CacheProviders;
using Persistense.Cache.Notifications.CacheStore;
using Persistense.Cache.Notifications.DI;
using Persistense.EF.Notifications.Interfaces;
using StackExchange.Redis;

namespace Persistense.Cache.Notifications
{
    public static class CacheRegister
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string Host, int Port)
        {
            services.AddSingleton<CacheConnectionGetter>(new CacheConnectionGetter(Host, Port));
            services.AddScoped<IConnectionMultiplexer>(
                ex =>
                {
                    var factory = ex.GetService<CacheConnectionGetter>();
                    return factory.GetConnection();
                }
                );
            services.AddScoped<IDatabase>(ex =>
            {
                var factory = ex.GetService<IConnectionMultiplexer>();
                return factory.GetDatabase();
            });
            services.AddScoped<IBlueprintCacheStore, BlueprintCacheStore>();
            services.AddScoped<ICustomerCacheStore, CustomerCacheStore>();
            services.AddScoped<INotificationCacheStore, NotificationCacheStore>();

            return services;
        }
    }
}
