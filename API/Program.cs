using App;
using Asp.Versioning;
using Infrastructure.Kafka;
using Infrastructure.Logging.InfluxDB;
using Infrastructure.MessageBrocker.ConsumerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notification.ConfigsInputObjects;
using Persistense;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var servicesConfig = builder.Configuration.GetSection("ServicesConfigs").Get<ConnectionsForServices>();

if(servicesConfig == null)
{
    throw new InvalidProgramException("invalid service configuration");
}
builder.Services.AddApiVersioning(ex => {

    ex.DefaultApiVersion = new ApiVersion(1, 0);
    ex.AssumeDefaultVersionWhenUnspecified = true;
    ex.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services
    .AddInfluexDBLogging(new InfluxConfig(
        servicesConfig.Logger.Host,
        servicesConfig.Logger.Token,
        servicesConfig.Logger.Organisation,
        servicesConfig.Logger.Bucket
        ))
    .AddPersistense(servicesConfig.Database.ConnectionString)
    .AddMessageBrocker(servicesConfig.MessageBrocker.Url)
    .AddApp()
    //.AddConsumerService()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    ;

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "info");
});

app.Run();