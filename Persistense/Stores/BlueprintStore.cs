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

            return blueprint.ToDomain();
        }
        catch (Exception ex)
        {
            return Result.Failure<Blueprint>(ex.Message);
        }
    }
}