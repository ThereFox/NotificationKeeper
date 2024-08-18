using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace App.Stores;

public interface INotificationStore
{
    public Task<Result<Notification>> Get(Guid id);
    public Task<Result> SaveNew(Notification notification);
    public Task<Result> SaveChanges(Notification notification);
}