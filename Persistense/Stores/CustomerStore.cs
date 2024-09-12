using App.Stores;
using Common;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Persistense.EF.Notifications.Interfaces;

namespace Persistense.Stores;

public class CustomerStore : ICustomerStore
{
    protected readonly ApplicationDBContext _context;
    private readonly ICustomerCacheStore _cache;
    private readonly ILogger _logger;


    public CustomerStore(
        ApplicationDBContext context,
        ICustomerCacheStore cache,
        ILogger logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<Result<Customer>> Get(Guid Id)
    {
        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure<Customer>("database unawaliable");
        }

        try
        {

            var customer = await _context
                .CustomerEntities
                .Include(ex => ex.AllDevices)
                .AsNoTracking()
                .FirstOrDefaultAsync(ex => ex.Id == Id);

            if (customer == default)
            {
                return Result.Failure<Customer>($"dont contain customer with Id {Id}");
            }

            return customer.ToDomain();
        }
        catch (Exception ex)
        {
            return Result.Failure<Customer>(ex.Message);
        }
    }

    public async Task<Result<int>> GetCountOfNotificationByDayForCustomerById(Guid Id)
    {
        var getValueFromCacheResult = await _cache.GetCountOfNotificationForCustomerAtDay(Id);

        if (getValueFromCacheResult.IsSuccess)
        {
            return getValueFromCacheResult;
        }
        else
        {
            await _logger.LogError(getValueFromCacheResult.AsError());
        }

        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure<int>("database unawaliable");
        }

        try
        {
            var currentDay = DateTime.Today;

            var count = await _context
                .CustomerEntities
                .AsNoTracking()
                .Where(ex => ex.Id == Id)
                .Select(
                ex => ex.ResivedNotifications.Count(not => not.CreatedAt.Date == currentDay.Date)
            ).SingleAsync();

            var saveResult = await _cache.SetCountOfNotificationForCustomerAtDay(Id, count);

            if (saveResult.IsFailure)
            {
                await _logger.LogError(saveResult.AsError());
            }

            return Result.Success(count);
        }
        catch (Exception ex)
        {
            return Result.Failure<int>(ex.Message);
        }
    }
}