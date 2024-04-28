using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace App.Stores;

public interface INotificationStore
{
    public Task<Result> Create(Notification notification);
}