using CSharpFunctionalExtensions;

namespace Domain.ValueObject;

public class NotificationStatus : CSharpFunctionalExtensions.ValueObject
{
    public static NotificationStatus Created => new NotificationStatus(0);
    public static NotificationStatus Sended => new NotificationStatus(1);
    public static NotificationStatus Rejected => new NotificationStatus(2);

    protected static List<NotificationStatus> _all => [Created, Sended, Rejected];
    
    public int Value { get; init; }

    protected NotificationStatus(int value)
    {
        Value = value;
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<NotificationStatus> Create(int value)
    {
        if (_all.Any(ex => ex.Value == value) == false)
        {
            return Result.Failure<NotificationStatus>("invalid status");
        }

        return Result.Success<NotificationStatus>(new NotificationStatus(value));
    }
    
}