using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Device
{
    public Guid Id { get; private set; }
    
    public NotificationChannel NotificationChannel { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public bool IsActive { get; private set; }

    public static Result<Device> Create(Guid id, NotificationChannel notificationChannel, DateTime createdAt,
        DateTime updatedAt, bool isActive)
    {
        if (
            createdAt >= updatedAt
            ||
            createdAt >= DateTime.Now
            ||
            updatedAt >= DateTime.Now
            )
        {
            return Result.Failure<Device>("invalid time");
        }

        return Result.Success<Device>(new Device(id, notificationChannel, createdAt, updatedAt, isActive));

    }
    
    protected Device(Guid id, NotificationChannel notificationChannel, DateTime createdAt, DateTime updatedAt, bool isActive)
    {
        Id = id;
        NotificationChannel = notificationChannel;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        IsActive = isActive;
    }

    public Result ChangeActive(bool newIsActive)
    {
        if (IsActive == newIsActive)
        {
            return Result.Failure("nothing to change");
        }
        
        IsActive = newIsActive;
        UpdatedAt = DateTime.Now;

        return Result.Success();
    }
    
}