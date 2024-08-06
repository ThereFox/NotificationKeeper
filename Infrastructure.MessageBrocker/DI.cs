using System.Net;
using App.Notifications;
using Confluent.Kafka;
using Infrastructure.Kafka.BrockerSender;
using Infrastructure.Kafka.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class DI
{
    public static IServiceCollection AddMessageBrocker(this IServiceCollection collection, string brockerUrl)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = brockerUrl,
            AllowAutoCreateTopics = true
        };
        
        var producer = new ProducerBuilder<Null, string>(config)
            .Build();

        
        collection.AddSingleton(producer);

        collection.AddSingleton<INotificationSender, MessageSender>();

        return collection;
    }
}