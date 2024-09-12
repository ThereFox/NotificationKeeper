using Common.Customs;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistense.EF.Notifications.Interfaces;
using Persistense.Entitys;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Cache.Notifications.CacheStore
{
    public class NotificationCacheStore : INotificationCacheStore
    {
        private readonly IDatabase _database;

        public NotificationCacheStore(IDatabase database)
        {
            _database = database;
        }

        public async Task<Result> DeleteNotificationWhatWaitReport(Guid Id)
        {
            try
            {
                var result =  await _database.KeyDeleteAsync(Id.ToString());

                return Result.SuccessIf(result, "inner error");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result<NotificationEntity>> GetNotificationWhatWaitReport(Guid Id)
        {
            try
            {
                var dataById = await _database.StringGetAsync(Id.ToString());

                if (dataById.HasValue == false)
                {
                    return Result.Failure<NotificationEntity>($"Dont contain data with id {Id}");
                }

                var deserializeResult = ResultJsonDeserializer.DeserializeObject<NotificationEntity>(dataById);

                return deserializeResult;

            }
            catch (Exception ex)
            {
                return Result.Failure<NotificationEntity>(ex.Message);
            }
        }

        public async Task<Result> SaveNotificationWhatWaitReport(NotificationEntity notification)
        {
            try
            {
                var SavedContent = getContentForSave(notification);

                var options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                var saveResult = await _database.StringSetAsync(notification.Id.ToString(), SavedContent, TimeSpan.FromMinutes(5));

                return Result.SuccessIf(saveResult, "inner error");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        private string getContentForSave(NotificationEntity notification)
        {
            if (notification.Customer == null && notification.Blueprint == null)
            {
                return JsonConvert.SerializeObject(notification);
            }

            var copyForNulling = new NotificationEntity()
            {
                Id = notification.Id,
                BlueprintId = notification.BlueprintId,
                CreatedAt = notification.CreatedAt,
                CustomerId = notification.CustomerId,
                SendAt = notification.SendAt,
                Status = notification.Status
            };

            return JsonConvert.SerializeObject(copyForNulling);
        }
    }
}
