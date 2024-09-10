namespace Notification.ConfigsInputObjects
{
    public record ConsumerConfig
    (
        string BrockerURL,
        string ReportTopic,
        string GroupId
    );
}
