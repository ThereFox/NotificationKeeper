namespace Persistense.Entitys;

public class CustomerEntity
{
    public Guid Id { get; private set; }
    
    public string UserName { get; set; }
    public int Role { get; set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public List<DeviceSettingsEntity> AllDevices { get; private set; }
    public List<NotificationEntity> ResivedNotifications { get; private set; }
}