namespace Infrastructure.Kafka.Requests;

public class SendNotificationRequest
{
    public Guid Id { get; set; }
    public Guid BlueprintId { get; set; }
    public string DeviceToken { get; set; }
}