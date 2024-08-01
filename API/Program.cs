using App;
using Infrastructure.Kafka;
using Infrastructure.Loging;
using Microsoft.EntityFrameworkCore;
using Persistense;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();

builder.Services
    .AddSpecialClickhouseLogger(builder.Configuration.GetConnectionString("ClickHouse"))
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