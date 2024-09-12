using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.EF.Notifications.Interfaces
{
    public interface ICustomerCacheStore
    {
        public Task<Result<int>> GetCountOfNotificationForCustomerAtDay(Guid CustomerId);

        public Task<Result> SetCountOfNotificationForCustomerAtDay(Guid CustomerId, int Count);
        public Task<Result> IncrementCountOfNotificationForCustomerAtDay(Guid CustomerId);
    }
}
