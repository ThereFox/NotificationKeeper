namespace Persistense.Entitys;

public class NotificationBlueprintEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Subject { get; set; }
    public string Content { get; set; }
    
    public int Channel { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public List<NotificationEntity> UsedIn { get; set; }
}