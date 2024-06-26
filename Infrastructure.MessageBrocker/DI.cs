using System.Net;
using App.Notifications;
using Confluent.Kafka;
using Infrastructure.Kafka.BrockerSender;
using Infrastructure.Kafka.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class DI
{
    public static IServiceCollection AddMessageBrocker(this IServiceCollection collection)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:29092",
            ClientId = Dns.GetHostName()
        };
        
        collection.AddTransient<INotificationSender, MessageSender>();

        var producer = new ProducerBuilder<Null, string>(config).Build();
        
        collection.AddSingleton(producer);
        
        return collection;
    }
}