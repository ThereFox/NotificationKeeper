using App.Notifications;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace Infrastructure.Kafka.BrockerSender;

public class MessageSender : INotificationSender
{
    public async Task<Result> SendNotification(Notification notification)
    {
        return Result.Failure("not realised");
    }
}