using App.Stores;
using Common;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Persistense.EF.Notifications.Interfaces;
using Persistense.Entitys;

namespace Persistense.Stores;

public class NotificationStore : INotificationStore
{
    protected readonly ApplicationDBContext _context;
    private readonly ICustomerCacheStore _customerCache;
    private readonly INotificationCacheStore _notificationCache;
    private readonly ILogger _logger;

    public NotificationStore(
        ApplicationDBContext context,
        ICustomerCacheStore customerCache,
        INotificationCacheStore notificationCache,
        ILogger logger)
    {
        _context = context;
        _customerCache = customerCache;
        _notificationCache = notificationCache;
        _logger = logger;
    }

    public async Task<Result<Notification>> Get(Guid id)
    {
        var getValueFromCacheResult = await _notificationCache.GetNotificationWhatWaitReport(id);

        if (getValueFromCacheResult.IsSuccess)
        {
            return getValueFromCacheResult.Value.ToDomain();
        }
        else
        {
            await _logger.LogError(getValueFromCacheResult.AsError());
        }

        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure<Notification>("Database is unawaliable");
        }

       var message = await _context
            .Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(ex => ex.Id == id);

        if(message == default)
        {
            return Result.Failure<Notification>($"Dont contain notification with Id {id}");
        }

        return message.ToDomain();

    }

    public async Task<Result> UpdateSendInfo(Notification notification)
    {
        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure<Notification>("Database is unawaliable");
        }

        try
        {
            using(var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                var oldInfo = await _context
                    .Notifications
                    .FirstOrDefaultAsync(ex => ex.Id == notification.Id);

                if (oldInfo == default)
                {
                    return Result.Failure("dont contain init data");
                }

                oldInfo.Status = notification.Status.Value;
                oldInfo.SendAt = notification.SendedAt;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var deleteInfoResult =  await _notificationCache.DeleteNotificationWhatWaitReport(oldInfo.Id);

                if (deleteInfoResult.IsFailure)
                {
                    await _logger.LogError(deleteInfoResult.AsError());
                }

                return Result.Success();
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }

    }

    public async Task<Result> SaveNew(Notification notification)
    {
        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure("Database is unawaliable");
        }

        try
        {
            var saveEntity = NotificationEntity.FromDomain(notification);

            _context.Add(saveEntity);

            await _context.SaveChangesAsync();

            var incrementResult = await _customerCache.IncrementCountOfNotificationForCustomerAtDay(saveEntity.CustomerId);

            if (incrementResult.IsFailure)
            {
                await _logger.LogError(incrementResult.AsError());
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}