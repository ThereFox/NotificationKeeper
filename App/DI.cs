using App.Services.UseCases;
using App.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class DI
{
    public static IServiceCollection AddApp(this IServiceCollection services)
    {
        services.AddScoped<SendNotificationUseCase>();
        services.AddScoped<ReportHandleUseCase>();
        services.AddScoped<NotificationValidator>();

        return services;
    }
}