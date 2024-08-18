using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Notification
{
    public const int MaxCountByDay = 5;
    
    public Guid Id { get; init; }
    public Customer Resiver { get; init; }
    public Blueprint Blueprint { get; init; }
    
    public NotificationStatus Status { get; private set; }
   
    public DateTime CreatedAt { get; init; }
    public DateTime? SendedAt { get; private set; }

    public Notification(Guid id, Customer resiver, Blueprint blueprint, NotificationStatus status, DateTime createdAt, DateTime? sendAt)
    {
        Id = id;
        Resiver = resiver;
        Blueprint = blueprint;
        Status = status;
        CreatedAt = createdAt;
        SendedAt = sendAt;
    }

    public static Result<Notification> Create(Guid id, Customer resiver, Blueprint blueprint, NotificationStatus status,
        DateTime createdAt, DateTime? sendAt)
    {
        if (status != NotificationStatus.Sended && sendAt != null)
        {
            return Result.Failure<Notification>("send time in unsended message");
        }

        if (sendAt != null && createdAt > sendAt)
        {
            return Result.Failure<Notification>("invalid time");
        }

        if (createdAt >= DateTime.Now || (sendAt != null && sendAt >= DateTime.Now))
        {
            return Result.Failure<Notification>("invalid time");
        }

        return Result.Success<Notification>(new Notification(id, resiver, blueprint, status, createdAt, sendAt));

    }
    
    public Result ChangeStatus(NotificationStatus newStatus)
    {
        if (newStatus == Status)
        {
            return Result.Failure("nothing to change");
        }

        if (newStatus == NotificationStatus.Sended)
        {
            SendedAt = DateTime.Now;
        }
        
        Status = newStatus;
        
        return Result.Success();
    }
    
}