using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace Persistense.Entitys;

public class DeviceSettingsEntity
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public int NotificationChannel { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public bool IsActive { get; set; }
    
    public CustomerEntity Owner { get; set; }

    public static DeviceSettingsEntity FromDomain(Device device)
    {
        return new DeviceSettingsEntity()
        {
            Id = device.Id,
            CreatedAt = device.CreatedAt,
            IsActive = device.IsActive,
            NotificationChannel = device.NotificationChannel.Value,
            UpdatedAt = device.UpdatedAt
        };
    }
    public Result<Device> ToDomain()
    {
        var validateChannel = Domain.ValueObject.NotificationChannel.Create(NotificationChannel);

        if (validateChannel.IsFailure)
        {
            return Result.Failure<Device>(validateChannel.Error);
        }

        return Device.Create(Id, validateChannel.Value, CreatedAt, UpdatedAt, IsActive);
    }
}