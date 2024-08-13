namespace Notification.ConfigsInputObjects
{
    public record ConnectionsForServices
    (
        DatabaseConfig Database,
        InfluexDBConfig Logger,
        KafkaConfig MessageBrocker
    );
}
