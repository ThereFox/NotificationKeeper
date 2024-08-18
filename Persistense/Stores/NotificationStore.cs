using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Persistense.Entitys;

namespace Persistense.Stores;

public class NotificationStore : INotificationStore
{
    protected readonly ApplicationDBContext _context;

    public NotificationStore(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Result<Notification>> Get(Guid id)
    {
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

    public async Task<Result> SaveChanges(Notification notification)
    {
        if (await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure<Notification>("Database is unawaliable");
        }

        try
        {
            var newInfo = NotificationEntity.FromDomain(notification);

            using(var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                var oldInfo = await _context
                    .Notifications
                    .FirstOrDefaultAsync(ex => ex.Id == notification.Id);

                if (oldInfo == default)
                {
                    return Result.Failure("dont contain init data");
                }

                oldInfo.Status = newInfo.Status;
                oldInfo.SendAt = newInfo.SendAt;

                await _context.SaveChangesAsync();
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
            return Result.Failure<Notification>("Database is unawaliable");
        }

        try
        {
            var saveEntity = NotificationEntity.FromDomain(notification);

            _context.Notifications.Append(saveEntity);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}