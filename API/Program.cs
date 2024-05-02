using App;
using Infrastructure.Kafka;
using Microsoft.EntityFrameworkCore;
using Persistense;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddPersistense(ex => ex.UseNpgsql(builder.Configuration.GetConnectionString("Notification")))
    .AddMessageBrocker()
    .AddApp();

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapControllerRoute(
    "default",
    "/{controller}/{action}"
    );

app.Map("/test", () => "test");

app.Run();