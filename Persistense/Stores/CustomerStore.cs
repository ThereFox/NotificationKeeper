using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace Persistense.Stores;

public class CustomerStore : ICustomerStore
{
    protected readonly ApplicationDBContext _context;

    public CustomerStore(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<Result<Customer>> Get(Guid Id)
    {
        try
        {

            var customer = await _context
                .CustomerEntities
                .Include(ex => ex.AllDevices)
                .AsNoTracking()
                .FirstAsync(ex => ex.Id == Id);

            var validateCustomerRole = CustomerRole.Create(customer.Role);

            if (validateCustomerRole.IsFailure)
            {
                return Result.Failure<Customer>(validateCustomerRole.Error);
            }

            var customerRole = validateCustomerRole.Value;

            var devices = customer.AllDevices.Select(ex =>
            {
                var validateNotificationChannel = NotificationChannel.Create(ex.NotificationChannel);

                if (validateNotificationChannel.IsFailure)
                {
                    return null;
                }

                var channel = validateNotificationChannel.Value;

                var validateDeviceResult =
                    Device.Create(ex.Id, ex.DeviceToken, channel, ex.CreatedAt, ex.UpdatedAt, ex.IsActive);

                if (validateDeviceResult.IsFailure)
                {
                    return null;
                }

                var device = validateDeviceResult.Value;

                return device;
            }).ToList();

            var validateCustomer = Customer.Create(Id, customerRole, customer.CreatedAt, devices);



            return validateCustomer;
        }
        catch (Exception ex)
        {
            return Result.Failure<Customer>(ex.Message);
        }
    }

    public async Task<Result<int>> GetCountOfNotificationByDay(Guid Id)
    {
        try
        {
            var currentDay = DateTime.Today;

            var count = await _context.CustomerEntities.AsNoTracking().Where(ex => ex.Id == Id).Select(
                ex => ex.ResivedNotifications.Count(not => not.CreatedAt.Date == currentDay.Date)
            ).SingleAsync();

            return Result.Success(count);
        }
        catch (Exception ex)
        {
            return Result.Failure<int>(ex.Message);
        }
    }
}