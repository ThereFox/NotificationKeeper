namespace Notification.Request;

public record SendNotificationRequest
(
    string BlueprintId,
    string CustomerId
);