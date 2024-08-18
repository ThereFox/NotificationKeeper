using App.InputObjects;
using App.Stores;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain.Entitys;
using Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Validators
{
    internal class NotificationValidator
    {
        private ICustomerStore _customerStore;
        private IBlueprintStore _blueprintStore;
        private INotificationStore _mainStore;

        internal async Task<Result<Notification>> ValidateNotification(SendNotificationDTO input)
        {
            var getCustomerResult = await _customerStore.Get(input.CustomerId);

            if (getCustomerResult.IsFailure)
            {
                return Result.Failure<Notification>(getCustomerResult.Error);
            }

            var getBlueprintResult = await _blueprintStore.Get(input.BlueprintId);

            if (getBlueprintResult.IsFailure)
            {
                return Result.Failure<Notification>(getBlueprintResult.Error);
            }

            var customer = getCustomerResult.Value;
            var blueprint = getBlueprintResult.Value;

            if (hasDeviceForNotification(customer, blueprint.Channel) == false)
            {
                return Result.Failure<Notification>($"notification channel by customer {customer.Id} unawaliable for bluepring {blueprint.Id}");
            }

            var id = Guid.NewGuid();

            var createMessageResult = Notification
                .Create(
                id,
                customer,
                blueprint,
                NotificationStatus.Created,
                DateTime.Now,
                null);

            return createMessageResult;
        }
        private bool hasDeviceForNotification(Customer customer, NotificationChannel channel)
        {
            return customer.Devices.Any(ex => ex.IsActive && ex.NotificationChannel == channel);
        }
    }
}
