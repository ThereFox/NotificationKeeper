using CSharpFunctionalExtensions;
using Persistense.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.EF.Notifications.Interfaces
{
    public interface INotificationCacheStore
    {
        public Task<Result<NotificationEntity>> GetNotificationWhatWaitReport(Guid Id);
        public Task<Result> SaveNotificationWhatWaitReport(NotificationEntity notification);
        public Task<Result> DeleteNotificationWhatWaitReport(Guid Id);

    }
}
