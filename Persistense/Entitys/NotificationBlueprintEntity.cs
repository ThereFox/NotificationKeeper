using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace Persistense.Entitys;

public class NotificationBlueprintEntity
{
    public Guid Id { get; set; }

    public int Channel { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<NotificationEntity> UsedIn { get; set; }

    public static NotificationBlueprintEntity FromDomain(Blueprint blueprint)
    {
        return new NotificationBlueprintEntity()
        {
            Id = blueprint.Id,
            Channel = blueprint.Channel.Value
        };
    }

    public Result<Blueprint> ToDomain()
    {
        var validateChannel = NotificationChannel.Create(Channel);

        if (validateChannel.IsFailure)
        {
            return Result.Failure<Blueprint>($"invalid channel error: {validateChannel.Error}");
        }

        return Blueprint.Create(Id, validateChannel.Value);
    }
}