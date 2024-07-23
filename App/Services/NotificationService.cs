using App.Notifications;
using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace App.Services;

public sealed class NotificationService
{
    private ICustomerStore _customerStore;
    private IBlueprintStore _blueprintStore;
    private INotificationStore _mainStore;

    private INotificationSender _sender;
    
    public NotificationService(
        ICustomerStore customers,
        IBlueprintStore blueprints,
        INotificationStore notifications,
        INotificationSender sender)
    {
        _customerStore = customers;
        _blueprintStore = blueprints;
        _mainStore = notifications;
    }
    
    public async Task<Result> Send(Guid BlueprintId, Guid CustomerId)
    {
        var getCustomerResult = await _customerStore.Get(CustomerId);
        
        if (getCustomerResult.IsFailure)
        {
            return Result.Failure($"dont contain customer with Id {CustomerId}, info {getCustomerResult.Error}");
        }

        var getBlueprintResult = await _blueprintStore.Get(BlueprintId);
        
        if (getBlueprintResult.IsFailure)
        {
            return Result.Failure($"dont contain blueprint with Id {BlueprintId}");
        }

        var customer = getCustomerResult.Value;
        var blueprint = getBlueprintResult.Value;

        if (canSendNotificationByChannel(customer, blueprint) == false)
        {
            return Result.Failure("notification channel unawaliable");
        }
        
        if (await hasTooManyNotificationInDayForUser(CustomerId))
        {
            
            return Result.Failure("too many notification in day");
        }

        var id = Guid.NewGuid();

        var device = customer.Devices.First(ex => ex.NotificationChannel == blueprint.Channel && ex.IsActive == true);
        
        var createMessageResult = Notification.Create(id, device, blueprint, NotificationStatus.Created, DateTime.Now, null );

        if (createMessageResult.IsFailure)
        {
            return Result.Failure(createMessageResult.Error);
        }

        var message = createMessageResult.Value;
        
        var saveResult = await _mainStore.Create(message);

        if (saveResult.IsFailure)
        {
            return Result.Failure(saveResult.Error);
        }


        var sendResult = await _sender.SendNotification(message);
        
        return sendResult;
    }

    private async Task<bool> hasTooManyNotificationInDayForUser(Guid userId)
    {
        var getCountOfNotificationResult = await _customerStore.GetCountOfNotificationByDay(userId);

        if (getCountOfNotificationResult.IsFailure)
        {
            throw new InvalidCastException();
        }

        var countOfNotification = getCountOfNotificationResult.Value;
        
        return countOfNotification + 1 >= Notification.MaxCountByDay;

    }

    private bool canSendNotificationByChannel(Customer recipient, Blueprint blueprint)
    {
        if(recipient.Devices.Count == 0)
        {
            return false;
        }

        return recipient.Devices.Any(ex => ex != null && ex.NotificationChannel == blueprint.Channel);
    }


}