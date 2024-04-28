using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace Persistense.Stores;

public class BlueprintStore : IBlueprintStore
{
    protected readonly ApplicationDBContext _context;

    public BlueprintStore(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<Result<Blueprint>> Get(Guid id)
    {
        try
        {
            var blueprint = await _context
                .Blueprints
                .AsNoTracking()
                .SingleAsync(ex => ex.Id == id);

            var validateNotificationChannelResult = NotificationChannel.Create(blueprint.Channel);

            if (validateNotificationChannelResult.IsFailure)
            {
                return Result.Failure<Blueprint>(validateNotificationChannelResult.Error);
            }

            var notificationChannel = validateNotificationChannelResult.Value;
            var validateBlueprintResult = Blueprint.Create(id, notificationChannel);

            if (validateBlueprintResult.IsFailure)
            {
                return Result.Failure<Blueprint>(validateBlueprintResult.Error);
            }
            
            var convertedBlueprint = validateBlueprintResult.Value;
            
            return Result.Success<Blueprint>(convertedBlueprint);
        }
        catch (Exception ex)
        {
            return Result.Failure<Blueprint>(ex.Message);
        }
    }
}