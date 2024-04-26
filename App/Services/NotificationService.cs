using App.Stores;
using CSharpFunctionalExtensions;

namespace App.Services;

public class NotificationService
{
    protected ICustomerStore _customerStore;
    protected IBlueprintStore _blueprintStore;
    
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
        
        
        
        
        
        
    }

    private async Task<bool> HasTooManyNotificationInDayForUser(Guid userId)
    {
        
    }
}