namespace Infrastructure.Kafka.Requests;

public record SendNotificationMessage
(
    Guid Id,
    Guid BlueprintId,
    Guid ClientId
);
