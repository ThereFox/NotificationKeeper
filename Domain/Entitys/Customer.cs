using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Customer
{
    private List<Device> _devices;
    
    public Guid Id { get; init; }
    
    public CustomerRole Role { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Device> Devices => _devices;
    

    public static Result<Customer> Create(Guid id, CustomerRole role, DateTime createdAt, List<Device> devices)
    {
        if (createdAt >= DateTime.Now)
        {
            return Result.Failure<Customer>("invalid creation time");
        }

        return Result.Success<Customer>(new Customer(id, role, createdAt, devices));

    }
    
    protected Customer(Guid id, CustomerRole role, DateTime createdAt, List<Device> devices)
    {
        Id = id;
        Role = role;
        CreatedAt = createdAt;
        _devices = devices;
    }
}