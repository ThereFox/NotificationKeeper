using App.InputObjects;
using App.Interfaces.Notifications;
using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Infrastructure.Reader.Trying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Kafka.BrockerSender
{
    public sealed class ReportListener : IReportReader
    {
        private const int TryDeadline = 200;
        private const int TrysCount = 3;

        private IConsumer<Null, SendingReport> _consumer;

        public ReportListener(IConsumer<Null, SendingReport> Consumer)
        {
            _consumer = Consumer;
        }

        public async Task<Result<SendingReport>> GetNewMessage()
        {
            var readResult = await _consumer.TryGetAsync(TrysCount, TryDeadline);

            if (readResult.IsFailure)
            {
                return Result.Failure<SendingReport>(readResult.Error);
            }

            if (readResult.IsSuccess)
            {
                _consumer.Commit();
            }

            return readResult;
        }
    }
}
