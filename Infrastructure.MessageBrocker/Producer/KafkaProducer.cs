using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Producer
{
    public sealed class KafkaProducer
    {
        private readonly IProducer<Null, string> _messageProduser;


        public KafkaProducer(IProducer<Null, string> messageProduser)
        {
            _messageProduser = messageProduser;
        }


        public async Task<Result> SendDataToTopic(string data, string TopicName)
        {
            try
            {
                var message = new Message<Null, string>()
                {
                    Value = data,
                };

                var deliveryResult = await _messageProduser.ProduceAsync(TopicName, message);

                if (deliveryResult.Status != PersistenceStatus.Persisted)
                {
                    return Result.Failure("error");
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
