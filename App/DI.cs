using App.Services.UseCases;
using App.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class DI
{
    public static IServiceCollection AddApp(this IServiceCollection services)
    {
        services.AddTransient<SendNotificationUseCase>();
        services.AddTransient<ReportHandleUseCase>();
        services.AddTransient<NotificationValidator>();

        return services;
    }
}