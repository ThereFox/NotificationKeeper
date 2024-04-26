using Domain.ValueObject;

namespace Domain.Entitys;

public class Customer
{
    public Guid Id { get; init; }
    
    public CustomerRole Role { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
}