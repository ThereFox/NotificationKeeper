namespace Persistense.Entitys;

public class CustomerEntity
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public int Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public List<DeviceSettingsEntity> AllDevices { get; set; }
    public List<NotificationEntity> ResivedNotifications { get; set; }
}