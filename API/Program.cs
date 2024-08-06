using App;
using Infrastructure.Kafka;
using Infrastructure.Logging.InfluxDB;
using Microsoft.EntityFrameworkCore;
using Notification.ConfigsInputObjects;
using Persistense;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

var servicesConfig = builder.Configuration.GetSection("ServicesConfigs").Get<ConnectionsForServices>();

if(servicesConfig == null)
{
    throw new InvalidProgramException("invalid service configuration");
}

builder.Services
    .AddInfluexDBLogging(new InfluxConfig(
        servicesConfig.Logger.Host,
        servicesConfig.Logger.Token,
        servicesConfig.Logger.Organisation,
        servicesConfig.Logger.Bucket
        ))
    .AddPersistense(servicesConfig.Database.ConnectionString)
    .AddMessageBrocker(servicesConfig.MessageBrocker.Url)
    .AddApp();

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapControllerRoute(
    "default",
    "/api/v1/{controller}/{action}"
    );

app.Run();