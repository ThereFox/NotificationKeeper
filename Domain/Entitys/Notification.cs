using CSharpFunctionalExtensions;
using Domain.ValueObject;

namespace Domain.Entitys;

public class Notification : Entity<Guid>
{
    public Customer Resiver { get; init; }
    public Blueprint Blueprint { get; init; }
    
    public NotificationStatus Status { get; private set; }
   
    public DateTime CreatedAt { get; init; }
    public DateTime? SendAt { get; private set; }
    
    public Result ChangeStatus(NotificationStatus newStatus)
    {
        if (newStatus == Status)
        {
            return Result.Failure("nothing to change");
        }

        if (newStatus == NotificationStatus.Sended)
        {
            SendAt = DateTime.Now;
        }
        
        Status = newStatus;
        
        return Result.Success();
    }
    
}