using System.Net;
using App.Interfaces.Notifications;
using Confluent.Kafka;
using Infrastructure.Brocker.Kafka.Consumer;
using Infrastructure.Reader;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class ConsumerRegister
{
    public static IServiceCollection AddReportReader(this IServiceCollection collection, string brockerUrl, string topicName, string groupId)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = brockerUrl,
            GroupId = groupId
        };
        
        var consumer = new ConsumerBuilder<Null, string>(config)
            .Build();

        consumer.Subscribe(topicName);
        
        collection.AddSingleton(consumer);
        collection.AddSingleton<KafkaConsumer>();
        collection.AddSingleton<IReportReader, ReportListener>(ex =>
        {
            var consumer = ex.GetService<KafkaConsumer>();
            return new ReportListener(consumer, topicName);
        });

        return collection;
    }
}