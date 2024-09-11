using Common.Customs;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistense.EF.Notifications.Interfaces;
using Persistense.Entitys;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Persistense.Cache.Notifications.CacheProviders
{
    public class BlueprintCacheStore : IBlueprintCacheStore
    {
        private readonly IDatabase _database;

        public BlueprintCacheStore(IDatabase database)
        {
            _database = database;
        }

        public async Task<Result> SaveBlueprintToCache(NotificationBlueprintEntity blueprint)
        {
            try
            {
                var SavedContent = getContentForSave(blueprint);

                var options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                var saveResult = await _database.StringSetAsync(blueprint.Id.ToString(), SavedContent, TimeSpan.FromMinutes(5));

                return Result.SuccessIf(saveResult, "inner error");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        private string getContentForSave(NotificationBlueprintEntity blueprint)
        {
            if (blueprint.UsedIn == null)
            {
                return JsonConvert.SerializeObject(blueprint);
            }

            var copyForNulling = new NotificationBlueprintEntity()
            {
                Channel = blueprint.Channel,
                CreatedAt = blueprint.CreatedAt,
                Id = blueprint.Id
            };

            return JsonConvert.SerializeObject(copyForNulling);
        }

        public async Task<Result<NotificationBlueprintEntity>> TryGetBlueprint(Guid Id)
        {
            try
            {
                var dataById = await _database.StringGetAsync(Id.ToString());

                if(dataById.HasValue == false)
                {
                    return Result.Failure<NotificationBlueprintEntity>($"Dont contain data with id {Id}");
                }

                var deserializeResult = ResultJsonDeserializer.DeserializeObject<NotificationBlueprintEntity>(dataById);

                return deserializeResult;

            }
            catch (Exception ex)
            {
                return Result.Failure<NotificationBlueprintEntity>(ex.Message);
            }
        }
    }
}
