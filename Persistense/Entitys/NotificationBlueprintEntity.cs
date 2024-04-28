namespace Persistense.Entitys;

public class NotificationBlueprintEntity
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Subject { get; private set; }
    public string Content { get; private set; }
    
    public int Channel { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public List<NotificationEntity> UsedIn { get; private set; }
}