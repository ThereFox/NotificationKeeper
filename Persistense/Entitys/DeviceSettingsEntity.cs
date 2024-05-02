namespace Persistense.Entitys;

public class DeviceSettingsEntity
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public string DeviceToken { get; set; }
    
    public int NotificationChannel { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public bool IsActive { get; set; }
    
    public CustomerEntity Owner { get; set; }
}