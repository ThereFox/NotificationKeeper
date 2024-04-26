namespace Persistense.Entitys;

public class NotificationEntity
{
    public Guid Id { get; private set; }
    
    public Guid CustomerId { get; private set; }
    public Guid BlueprintId { get; private set; }
    
    public string Status { get; set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? SendAt { get; set; }
    
    public CustomerEntity Customer { get; private set; }
    public NotificationBlueprintEntity Blueprint { get; private set; }
    
}