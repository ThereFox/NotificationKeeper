using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace Persistense.Entitys;

public class CustomerEntity
{
    public Guid Id { get; set; }
    
    public int Role { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public List<DeviceSettingsEntity> AllDevices { get; set; }
    public List<NotificationEntity> ResivedNotifications { get; set; }

    public static CustomerEntity FromDomain(Customer customer)
    {
        return new CustomerEntity()
        {
            Id = customer.Id,
            Role = customer.Role.Value,
            CreatedAt = customer.CreatedAt,
        };
    }
    public Result<Customer> ToDomain()
    {
        var validateRole = CustomerRole.Create(Role);

        if (validateRole.IsFailure)
        {
            return Result.Failure<Customer>(validateRole.Error);
        }

        var allValidedDevice = AllDevices.Select(ex => ex.ToDomain()).Where(ex => ex.IsSuccess).Select(ex => ex.Value).ToList();

        return Customer.Create(Id, validateRole.Value, CreatedAt, allValidedDevice);
    }
}