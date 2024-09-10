using App;
using Asp.Versioning;
using Infrastructure.Kafka;
using Infrastructure.Logging.InfluxDB;
using Infrastructure.MessageBrocker.ConsumerService;
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
    .AddPersistense(
        servicesConfig.Database.ConnectionString
    )
    .AddProducer(
        servicesConfig.MessageBrocker.Producer.BrockerURL,
        servicesConfig.MessageBrocker.Producer.Topics.ToDictionary()
    )
    .AddReportReader(
        servicesConfig.MessageBrocker.Consumer.BrockerURL,
        servicesConfig.MessageBrocker.Consumer.ReportTopic,
        servicesConfig.MessageBrocker.Consumer.GroupId
    )
    .AddConsumerService()
    .AddApp()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "info");
});

app.Run();