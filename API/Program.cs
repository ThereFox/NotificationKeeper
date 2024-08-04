using App;
using Infrastructure.Kafka;
using Infrastructure.Logging.InfluxDB;
using Microsoft.EntityFrameworkCore;
using Persistense;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services
    .AddInfluexDBLogging(new InfluxConfig(
        "http://localhost:8051",
        @"kRhAVG6AcWwAJ9dac1xgKlTvyDQZunzsA6-WkHh3b0KNE7BQ4uaeqtFHtGye7xqHSPI_9IK9-KeHtbUbB3DvZA==",
        "ThereFoxOrganisation",
        "TestBucket"
        ))
    .AddPersistense(builder.Configuration)
    .AddMessageBrocker()
    .AddApp();

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapControllerRoute(
    "default",
    "/api/v1/{controller}/{action}"
    );

app.Map("/test", () => "test");

app.Run();