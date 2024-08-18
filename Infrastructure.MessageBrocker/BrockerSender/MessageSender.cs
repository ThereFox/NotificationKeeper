using App.Notifications;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Infrastructure.Kafka.Requests;

namespace Infrastructure.Kafka.BrockerSender;

public sealed class MessageSender : INotificationSender
{
    private readonly IProducer<Null, string> _messageProduser;


    private readonly Dictionary<NotificationChannel, string> _channelTopicNames = new()
    {
        { NotificationChannel.SMS, "SmsMessages" },
        { NotificationChannel.Android, "AndroidMessages" },
        { NotificationChannel.Email, "EmailMessages" }
    };

    public MessageSender(IProducer<Null, string> messageProduser)
    {
        _messageProduser = messageProduser;
    }

    
    public async Task<Result> SendNotification(Notification notification)
    {
        try
        {
            var notificationChannel = notification.Blueprint.Channel;

            var request = new SendNotificationMessage(notification.Id, notification.Blueprint.Id, notification.Resiver.Id);

            var message = new Message<Null, string>()
            {
                Value = request.ToString(),
            };

            var deliveryResult = await _messageProduser.ProduceAsync(_channelTopicNames[notificationChannel], message);

            if (deliveryResult.Status != PersistenceStatus.Persisted)
            {
                return Result.Failure("error");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}