using App.InputObjects;
using App.Services.Validators;
using App.Stores;
using CSharpFunctionalExtensions;
using Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.UseCases
{
    public class ReportHandleUseCase
    {
        private INotificationStore _notificationStore;
        private ILogger _logger;

        public ReportHandleUseCase(INotificationStore Store, ILogger Logger)
        {
            _notificationStore = Store;
            _logger = Logger;
        }

        public async Task<Result> Handle(ResivedReport report)
        { 
            await _logger.LogGetReport(report);

            var getNotificationResult = await _notificationStore.Get(report.NotificationId);

            if (getNotificationResult.IsFailure)
            {
                await _logger.LogError(new Common.Error(getNotificationResult.Error));
                return getNotificationResult;
            }

            var newStatus = report.isSucsessfull ? NotificationStatus.Sended : NotificationStatus.Rejected;


            var changeStatusResult = getNotificationResult.Value.ChangeStatus(newStatus);

            if (changeStatusResult.IsFailure)
            {
                await _logger.LogError(new Common.Error(changeStatusResult.Error));
                return changeStatusResult;
            }

            return await _notificationStore.UpdateSendInfo(getNotificationResult.Value);
        }
    }
}
