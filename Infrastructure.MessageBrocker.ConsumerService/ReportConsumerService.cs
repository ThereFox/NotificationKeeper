using App.InputObjects;
using App.Interfaces.Notifications;
using App.Services.UseCases;
using CSharpFunctionalExtensions;
using Infrastructure.Kafka.BrockerSender;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Kafka.Service
{
    public class ReportConsumerService : IHostedService
    {
        private readonly IReportReader _listener;
        private readonly ReportHandleUseCase _service;

        private CancellationTokenSource _cancelerTokenSource = new();

        public ReportConsumerService(IReportReader reportSource, ReportHandleUseCase reportHandler)
        {
            _listener = reportSource;
            _service = reportHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(consumeLoop, _cancelerTokenSource.Token);
            return Task.CompletedTask;
        }

        private async Task consumeLoop()
        {
            while (_cancelerTokenSource.IsCancellationRequested == false)
            {
                await consumeMessage();
            }
        }

        private async Task consumeMessage()
        {
            var getMessageResult = await _listener.GetNewMessage();

            if (getMessageResult.IsSuccess)
            {
                await _service.Handle(getMessageResult.Value);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(_cancelerTokenSource.IsCancellationRequested)
            {
                return;
            }

            await _cancelerTokenSource.CancelAsync();
            _cancelerTokenSource = new();
        }
    }
}
