using App.Services.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class DI
{
    public static IServiceCollection AddApp(this IServiceCollection services)
    {
        services.AddTransient<SendNotificationUseCase>();

        return services;
    }
}