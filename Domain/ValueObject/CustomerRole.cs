using CSharpFunctionalExtensions;

namespace Domain.ValueObject;

public class CustomerRole : CSharpFunctionalExtensions.ValueObject
{
    public static CustomerRole Base => new CustomerRole(0);
    public static CustomerRole Special => new CustomerRole(1);

    public static List<CustomerRole> _all = [Base, Special];
    
    public int Value { get; init; }

    protected CustomerRole(int value)
    {
        Value = value;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<CustomerRole> Create(int value)
    {
        if (_all.Any(ex => ex.Value == value) == false)
        {
            return Result.Failure<CustomerRole>("invalid role");
        }

        return Result.Success<CustomerRole>(new CustomerRole(value));
    }
}