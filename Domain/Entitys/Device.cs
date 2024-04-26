using Domain.ValueObject;

namespace Domain.Entitys;

public class Device
{
    public Guid Id { get; private set; }
    
    public NotificationChannel NotificationChannel { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public bool IsActive { get; set; }

}