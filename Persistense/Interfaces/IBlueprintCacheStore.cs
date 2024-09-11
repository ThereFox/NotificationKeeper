using CSharpFunctionalExtensions;
using Persistense.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.EF.Notifications.Interfaces
{
    public interface IBlueprintCacheStore
    {
        public Task<Result<NotificationBlueprintEntity>> TryGetBlueprint(Guid Id);
        public Task<Result> SaveBlueprintToCache(NotificationBlueprintEntity blueprint);

    }
}
