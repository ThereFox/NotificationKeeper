using System.Net;
using App.Interfaces.Notifications;
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
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = brockerUrl,
            AllowAutoCreateTopics = true,
            Acks = Acks.All,
            EnableIdempotence = true
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = brockerUrl,
            GroupId = "default",
            EnableAutoCommit = false
        };

        collection.AddSingleton(new ProducerBuilder<Null, string>(producerConfig).Build());
        collection.AddSingleton(new ConsumerBuilder<Null, string>(consumerConfig).Build());


        collection.AddScoped<INotificationSender, MessageSender>();
        collection.AddScoped<IReportReader, ReportListener>();

        return collection;
    }
}