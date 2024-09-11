using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Persistense.EF.Notifications.Interfaces;

namespace Persistense.Stores;

public class BlueprintStore : IBlueprintStore
{
    protected readonly ApplicationDBContext _context;
    private readonly IBlueprintCacheStore _cache;

    public BlueprintStore(ApplicationDBContext context, IBlueprintCacheStore cache)
    {
        _context = context;
        _cache = cache;
    }
    
    public async Task<Result<Blueprint>> Get(Guid id)
    {
        var getBlueprintFromCacheResult = await _cache.GetBlueprint(id);

        if (getBlueprintFromCacheResult.IsSuccess)
        {
            return getBlueprintFromCacheResult.Value.ToDomain();
        }

        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure<Blueprint>("database unawaliable");
        }

        try
        {
            var blueprint = await _context
                .Blueprints
                .AsNoTracking()
                .FirstOrDefaultAsync(ex => ex.Id == id);

            if (blueprint == default)
            {
                return Result.Failure<Blueprint>($"dont contain blueprint with Id {id}");
            }

            var saveToCacheResult = await _cache.SaveBlueprintToCache(blueprint);

            return blueprint.ToDomain();
        }
        catch (Exception ex)
        {
            return Result.Failure<Blueprint>(ex.Message);
        }
    }
}