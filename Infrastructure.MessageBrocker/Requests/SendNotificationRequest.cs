namespace Infrastructure.Kafka.Requests;

public record SendNotificationRequest
(
    Guid Id,
    Guid BlueprintId,
    string DeviceToken
);