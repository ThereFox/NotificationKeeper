using App.InputObjects;
using Common;
using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Stores
{
    public interface ILogger
    {
        public Task LogRequest(Guid bluepringId, Guid customerId);
        public Task LogAddToQueue(Notification message);
        public Task LogError(Error exception);
        public Task LogGetReport(ResivedReport report);
    }
}
