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

    public async Task<Result> Create(Notification notification)
    {
        try
        {
            var owner = await _context.Devices.FirstAsync(ex => ex.Id == notification.Resiver.Id);
            
            var notificationEntity = new NotificationEntity()
            {
                Id  = notification.Id,
                Status = notification.Status.Value,
                CreatedAt = notification.CreatedAt,
                BlueprintId = notification.Blueprint.Id,
                SendAt = notification.SendAt,
                CustomerId = owner.CustomerId
            } ;

            _context.Notifications.Append(notificationEntity);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}