using App.Notifications;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Infrastructure.Kafka.Requests;

namespace Infrastructure.Kafka.BrockerSender;

public class MessageSender : INotificationSender
{
    protected readonly IProducer<Null, string> _messageProduser;

    private readonly Dictionary<NotificationChannel , string> TopicNames = new Dictionary<NotificationChannel, string>()
    {
        { NotificationChannel.SMS, "SmsMessages" },
        { NotificationChannel.Android, "AndroidMessages" },
        { NotificationChannel.Email, "EmailMessages" }
    };
    
    public async Task<Result> SendNotification(Notification notification)
    {
        try
        {
            var notificationChannel = notification.Resiver.NotificationChannel;

            var request = new SendNotificationRequest(notification.Id, notification.Blueprint.Id, notification.Resiver.Token);

            var message = new Message<Null, string>()
            {
                Value = request.ToString(),
            };
            
            var deliveryResult = await _messageProduser.ProduceAsync(TopicNames[notificationChannel], message);

            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
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