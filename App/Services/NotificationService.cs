using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace App.Services;

public class NotificationService
{
    protected ICustomerStore _customerStore;
    protected IBlueprintStore _blueprintStore;
    protected INotificationStore _mainStore;

    public NotificationService(ICustomerStore customers, IBlueprintStore blueprints, INotificationStore notifications)
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
            return Result.Failure($"dont contain customer with Id {CustomerId}");
        }

        var getBlueprintResult = await _blueprintStore.Get(BlueprintId);
        
        if (getBlueprintResult.IsFailure)
        {
            return Result.Failure($"dont contain blueprint with Id {BlueprintId}");
        }

        var customer = getCustomerResult.Value;
        var blueprint = getBlueprintResult.Value;
        
        if (await HasTooManyNotificationInDayForUser(CustomerId))
        {
            
            return Result.Failure("too many notification in day");
        }

        var id = Guid.NewGuid();
        
        var createMessageResult = Notification.Create(id, customer, blueprint, NotificationStatus.Created, DateTime.Now, null );

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

        return Result.Success();
    }

    private async Task<bool> HasTooManyNotificationInDayForUser(Guid userId)
    {
        var getCountOfNotificationResult = await _customerStore.GetCountOfNotificationByDay(userId);

        if (getCountOfNotificationResult.IsFailure)
        {
            throw new InvalidCastException();
        }

        var countOfNotification = getCountOfNotificationResult.Value;
        
        return countOfNotification + 1 >= Notification.MaxCountByDay;

    }
}