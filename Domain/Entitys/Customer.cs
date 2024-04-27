using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Customer
{
    public Guid Id { get; init; }
    
    public CustomerRole Role { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public static Result<Customer> Create(Guid id, CustomerRole role, DateTime createdAt)
    {
        if (createdAt >= DateTime.Now)
        {
            return Result.Failure<Customer>("invalid creation time");
        }

        return Result.Success<Customer>(new Customer(id, role, createdAt));

    }
    
    protected Customer(Guid id, CustomerRole role, DateTime createdAt)
    {
        Id = id;
        Role = role;
        CreatedAt = createdAt;
    }
}