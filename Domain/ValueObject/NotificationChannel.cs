using CSharpFunctionalExtensions;

namespace Domain.ValueObject;

public class NotificationChannel : CSharpFunctionalExtensions.ValueObject
{
    public static NotificationChannel Email => new NotificationChannel(1);
    public static NotificationChannel SMS => new NotificationChannel(2);
    public static NotificationChannel Android => new NotificationChannel(3);
    
    private static List<NotificationChannel> _all = [ Email, SMS, Android ];
    
    public int Value { get; init; }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    protected NotificationChannel(int value)
    {
        Value = value;
    }

    public static Result<NotificationChannel> Create(int value)
    {
        if (_all.Any(ex => ex.Value == value) == false)
        {
            return Result.Failure<NotificationChannel>("invalidValue");
        }
        
        return Result.Success<NotificationChannel>(new NotificationChannel(value));
    }
    
}