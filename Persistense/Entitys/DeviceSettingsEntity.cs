namespace Persistense.Entitys;

public class DeviceSettingsEntity
{
    public Guid Id { get; private set; }
    
    public Guid CustomerId { get; private set; }
    
    public string DeviceToken { get; private set; }
    
    public int NotificationChannel { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public bool IsActive { get; set; }
    
    public CustomerEntity Owner { get; private set; }
}