using Domain.ValueObject;

namespace Notification.ConfigsInputObjects
{
    public record ChannelTopics
    (
        string Email    
    );

    public static class ChannelTopicsToDictionary
    {
        public static Dictionary<NotificationChannel, string> ToDictionary(this ChannelTopics topics)
        {
            var dictionary = new Dictionary<NotificationChannel, string>();

            dictionary.Add(NotificationChannel.Email, topics.Email);

            return dictionary;
        }
    }

}
