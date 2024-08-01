using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace App.Notifications;

public interface INotificationSender
{
    public Task<Result> SendNotification(Notification notification);
}
