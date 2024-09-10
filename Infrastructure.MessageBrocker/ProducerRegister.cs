using System.Net;
using App.InputObjects;
using App.Interfaces.Notifications;
using App.Notifications;
using Confluent.Kafka;
using Domain.ValueObject;
using Infrastructure.Brocker.Kafka.Producer;
using Infrastructure.Kafka.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class ProducerRegister
{
    public static IServiceCollection AddProducer(this IServiceCollection collection, string brockerUrl, Dictionary<NotificationChannel, string> topics)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = brockerUrl,
            AllowAutoCreateTopics = true,
            Acks = Acks.All,
            EnableIdempotence = true
        };

        collection.AddSingleton(new ProducerBuilder<Null, string>(producerConfig).Build());

        collection.AddScoped<KafkaProducer>();
        collection.AddScoped<INotificationSender, NotificationCommandSender>(
            ex =>
            {
                var producer = ex.GetService<KafkaProducer>();
                return new NotificationCommandSender(producer, topics);
            }
            );

        return collection;
    }
}