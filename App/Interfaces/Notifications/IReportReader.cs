using App.InputObjects;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Interfaces.Notifications
{
    public interface IReportReader
    {
        public Task<Result<ResivedReport>> GetNewMessage();
    }
}
