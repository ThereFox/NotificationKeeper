using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace App.Stores;

public interface ICustomerStore
{
    public Task<Result<Customer>> Get(Guid Id);

    public Task<Result<int>> GetCountOfNotificationByDayForCustomerById(Guid Id);

}