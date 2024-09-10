using App.Notifications;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Infrastructure.Brocker.Kafka.Producer;
using Infrastructure.Kafka.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Kafka
{
    public class NotificationCommandSender : INotificationSender
    {
        private readonly Dictionary<NotificationChannel, string> _topics;
        private readonly KafkaProducer _producer;

        public NotificationCommandSender(KafkaProducer producer, Dictionary<NotificationChannel, string> topic)
        {
            _producer = producer;
            _topics = topic;
        }

        public async Task<Result> SendNotification(Notification notification)
        {
            if(_topics.TryGetValue(notification.Blueprint.Channel, out var topic) == false)
            {
                return Result.Failure("Unsupported notification channel");
            }

            var message = new SendNotificationMessage(notification.Id, notification.Blueprint.Id, notification.Resiver.Id);

            var selialisedMessage = JsonSerializer.Serialize(message);

            var sendResult = await _producer.SendDataToTopic(selialisedMessage, topic);

            return sendResult;
        }
    }
}
