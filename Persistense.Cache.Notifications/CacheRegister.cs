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
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string Host, int Port, string UserName, string UserPassword)
        {
            services.AddSingleton<CacheConnectionGetter>(new CacheConnectionGetter(Host, Port, UserName, UserPassword));
            services.AddSingleton<IConnectionMultiplexer>(
                ex =>
                {
                    var factory = ex.GetService<CacheConnectionGetter>();
                    return factory.GetConnection();
                }
                );
            services.AddSingleton<IDatabase>(ex =>
            {
                var factory = ex.GetService<IConnectionMultiplexer>();
                return factory.GetDatabase();
            });
            services.AddSingleton<IBlueprintCacheStore, BlueprintCacheStore>();
            services.AddSingleton<ICustomerCacheStore, CustomerCacheStore>();
            services.AddSingleton<INotificationCacheStore, NotificationCacheStore>();

            return services;
        }
    }
}
