namespace Notification.ConfigsInputObjects
{
    public record ProducerConfig
    (
        string BrockerURL,
        ChannelTopics Topics
    );
}
