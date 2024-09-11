using CSharpFunctionalExtensions;
using Persistense.EF.Notifications.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Cache.Notifications.CacheStore
{
    public class CustomerCacheStore : ICustomerCacheStore
    {
        private IDatabase _database;

        public CustomerCacheStore(IDatabase database)
        {
            _database = database;
        }

        public async Task<Result<int>> GetCountOfNotificationForCustomerAtDay(Guid CustomerId)
        {
            try
            {
                var getResult = await _database.StringGetAsync(CustomerId.ToString());

                if(getResult.HasValue == false)
                {
                    return Result.Failure<int>($"dont contain value by id {CustomerId}");
                }

                if (getResult.IsInteger == false)
                {
                    return Result.Failure<int>($"version conflict");
                }

                if(getResult.TryParse(out int result))
                {
                    return Result.Failure<int>($"parse error");
                }

                return Result.Success<int>(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(ex.Message);
            }
        }

        public async Task<Result> IncrementCountOfNotificationForCustomerAtDay(Guid CustomerId)
        {
            try
            {
                await _database.StringIncrementAsync(CustomerId.ToString());

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
