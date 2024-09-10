namespace Notification.ConfigsInputObjects
{
    public record KafkaConfig
    (
        ProducerConfig Producer,
        ConsumerConfig Consumer
    );
}
