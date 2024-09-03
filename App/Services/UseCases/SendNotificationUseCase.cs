using App.InputObjects;
using App.Notifications;
using App.Services.Validators;
using App.Stores;
using Common;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Entitys;
using Common;

namespace App.Services.UseCases;

public sealed class SendNotificationUseCase
{

    private INotificationStore _notificationStore;
    private ICustomerStore _customerStore;

    private NotificationValidator _validator;
    private INotificationSender _sender;
    private ILogger _logger;

    public SendNotificationUseCase(
        INotificationSender sender,
        ILogger logger,
        NotificationValidator validator,
        INotificationStore notificationStore,
        ICustomerStore customerStore
        )
    {
        _sender = sender;
        _logger = logger;
        _validator = validator;
        _notificationStore = notificationStore;
        _customerStore = customerStore;
    }

    public async Task<Result> Send(SendNotificationDTO input)
    {
        await _logger.LogRequest(input.BlueprintId, input.CustomerId);

        var validateResult = await _validator.ValidateNotification(input);

        if (validateResult.IsFailure)
        {
            await _logger.LogError(validateResult.AsError());

            return Result.Failure(validateResult.Error);
        }

        var message = validateResult.Value;

        var notificationCountCouldntOverheadCheckResult = await hasTooManyNotificationInDayForUser(message.Resiver);

        if (notificationCountCouldntOverheadCheckResult.IsFailure)
        {
            await _logger.LogError(notificationCountCouldntOverheadCheckResult.AsError());

            return notificationCountCouldntOverheadCheckResult;
        }

        var saveResult = await _notificationStore.SaveNew(message);

        if (saveResult.IsFailure)
        {
            await _logger.LogError(saveResult.AsError());

            return saveResult;
        }

        var sendResult = await _sender.SendNotification(message);

        await logSendMessageResult(sendResult, message);

        return sendResult;
    }

    private async Task logSendMessageResult(Result result, Notification message)
    {
        if (result.IsSuccess)
        {
            await _logger.LogAddToQueue(message);
        }
        else
        {
            await _logger.LogError(new Error(result.Error));
        }
    }

    private async Task<Result> hasTooManyNotificationInDayForUser(Customer customer)
    {
        var getCountOfNotificationResult = await _customerStore.GetCountOfNotificationByDayForCustomerById(customer.Id);

        if (getCountOfNotificationResult.IsFailure)
        {
            return Result.Failure(getCountOfNotificationResult.Error);
        }

        var countOfNotification = getCountOfNotificationResult.Value;

        return Result.FailureIf(countOfNotification + 1 >= Notification.MaxCountByDay, $"too many notification for user {customer.Id}");

    }

}