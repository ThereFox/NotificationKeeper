using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace App.Stores;

public interface ICustomerStore
{
    public Task<Result<Customer>> Get(Guid Id);
    public Task<Result<List<Device>>> GetActiveDevicesByCustomer(Guid id);

    public Task<Result<int>> GetCountOfNotificationByDay(Guid Id);

}