namespace Persistense.Entitys;

public class NotificationEntity
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }
    public Guid BlueprintId { get; set; }
    
    public int Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? SendAt { get; set; }
    
    public CustomerEntity Customer { get; set; }
    public NotificationBlueprintEntity Blueprint { get; set; }
    
}