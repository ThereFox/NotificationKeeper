using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Infrastructure.Reader.Trying;

namespace Infrastructure.Reader
{
    public class KafkaConsumer
    {
        private const int TryDeadline = 2000;
        private const int TrysCount = 3;

        private readonly IConsumer<Null, string> _consumer;

        public KafkaConsumer(IConsumer<Null, string> consumer)
        {
            _consumer = consumer;
        }

        public async Task<Result<string>> GetNewMessageFromTopic(string topic)
        {
            var readResult = await _consumer.TryGetAsync(TrysCount, TryDeadline);

            if (readResult.IsFailure)
            {
                return Result.Failure<string>(readResult.Error);
            }

            return readResult;
        }
    }
}
