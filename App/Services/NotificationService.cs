using App.Notifications;
using App.Stores;
using Common;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Entitys;
using Domain.ValueObject;

namespace App.Services;

public sealed class NotificationService
{
    private ICustomerStore _customerStore;
    private IBlueprintStore _blueprintStore;
    private INotificationStore _mainStore;

    private INotificationSender _sender;

    private ILogger _logger;
    
    public NotificationService(
        ICustomerStore customers,
        IBlueprintStore blueprints,
        INotificationStore notifications,
        INotificationSender sender,
        ILogger logger)
    {
        _customerStore = customers;
        _blueprintStore = blueprints;
        _mainStore = notifications;
        _sender = sender;
        _logger = logger;
    }
    
    public async Task<Result> Send(Guid BlueprintId, Guid CustomerId)
    {
        await _logger.LogRequest(BlueprintId, CustomerId);

        var getCustomerResult = await _customerStore.Get(CustomerId);
        
        if (getCustomerResult.IsFailure)
        {
            var error = new Error($"dont contain customer with Id {CustomerId}, info {getCustomerResult.Error}");

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }

        var getBlueprintResult = await _blueprintStore.Get(BlueprintId);
        
        if (getBlueprintResult.IsFailure)
        {
            var error = new Error($"dont contain blueprint with Id {BlueprintId}");

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }

        var customer = getCustomerResult.Value;
        var blueprint = getBlueprintResult.Value;

        if (canSendNotificationByChannel(customer, blueprint) == false)
        {
            var error = new Error($"notification channel by customer {customer.Id} unawaliable for bluepring {blueprint.Id}");

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }

        var checkNotificationLimitOverflowResult = await hasTooManyNotificationInDayForUser(CustomerId);

        if (checkNotificationLimitOverflowResult.IsFailure)
        {
            var error = new Error(checkNotificationLimitOverflowResult.Error);

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }

        if (checkNotificationLimitOverflowResult.Value == true)
        {
            var error = new Error($"too many notification in day for user {customer.Id}");

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }

        var id = Guid.NewGuid();

        var device = customer.Devices.First(ex => ex.NotificationChannel == blueprint.Channel && ex.IsActive == true);
        
        var createMessageResult = Notification.Create(id, device, blueprint, NotificationStatus.Created, DateTime.Now, null );

        if (createMessageResult.IsFailure)
        {
            var error = new Error(createMessageResult.Error);

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }

        var message = createMessageResult.Value;
        
        var saveResult = await _mainStore.Create(message);

        if (saveResult.IsFailure)
        {
            var error = new Error(saveResult.Error);

            await _logger.LogError(error);

            return Result.Failure(error.ErrorMessage);
        }


        var sendResult = await _sender.SendNotification(message);

        if (sendResult.IsSuccess) 
        {
            await _logger.LogAddToQueue(message);
        }
        else
        {
            await _logger.LogError(new Error(sendResult.Error));
        }

        return sendResult;
    }

    private async Task<Result<bool>> hasTooManyNotificationInDayForUser(Guid userId)
    {
        var getCountOfNotificationResult = await _customerStore.GetCountOfNotificationByDay(userId);

        if (getCountOfNotificationResult.IsFailure)
        {
            var error = new Error(getCountOfNotificationResult.Error);

            await _logger.LogError(error);

            return Result.Failure<bool>(error.ErrorMessage);
        }

        var countOfNotification = getCountOfNotificationResult.Value;
        
        return Result.Success(countOfNotification + 1 >= Notification.MaxCountByDay);

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