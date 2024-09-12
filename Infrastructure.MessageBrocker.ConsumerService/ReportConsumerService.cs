using App.InputObjects;
using App.Interfaces.Notifications;
using App.Services.UseCases;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
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
        private IServiceScopeFactory _scopeFactory;

        private CancellationTokenSource _cancelerTokenSource = new();

        public ReportConsumerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
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
                await consumeMessageInNewScope();
            }
        }

        private async Task consumeMessageInNewScope()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var readerService = scope.ServiceProvider.GetService<IReportReader>();
                var handlerService = scope.ServiceProvider.GetService<ReportHandleUseCase>();

                await consumeMessage(readerService, handlerService);
            }
        }

        private async Task consumeMessage(IReportReader reader, ReportHandleUseCase handler)
        {
            var getMessageResult = await reader.GetNewMessage();

            if (getMessageResult.IsSuccess)
            {
                await handler.Handle(getMessageResult.Value);
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
