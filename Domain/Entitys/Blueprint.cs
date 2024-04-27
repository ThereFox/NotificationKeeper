using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Blueprint : Entity<Guid>
{
    public Guid Id { get; private set; }
    
    public NotificationChannel Channel { get; private set; }

    public static Result<Blueprint> Create(Guid id, NotificationChannel channel)
    {
        return Result.Success<Blueprint>(new Blueprint(id, channel));
    }
    
    protected Blueprint(Guid id, NotificationChannel channel)
    {
        Id = id;
        Channel = channel;
    }
}