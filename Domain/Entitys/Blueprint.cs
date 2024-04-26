using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Blueprint : Entity<Guid>
{
    public Guid Id { get; private set; }
    
    public NotificationChannel Channel { get; private set; }
}