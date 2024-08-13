namespace Notification.ConfigsInputObjects
{
    public record InfluexDBConfig
    (
        string Host,
        string Token,
        string Organisation,
        string Bucket
    );
}
