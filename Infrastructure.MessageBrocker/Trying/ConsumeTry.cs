using Confluent.Kafka;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Reader.Trying
{
    public static class ConsumeTry
    {
        public static async Task<Result<ValueT>> TryGetAsync<KeyT, ValueT>(this IConsumer<KeyT, ValueT> consumer, int TrysCount, int TrysDeadline)
        {
            ArgumentNullException.ThrowIfNull(consumer, nameof(consumer));

            if (TrysCount <= 1)
            {
                throw new ArgumentOutOfRangeException("trys count must be more that one");
            }

            if (TrysDeadline <= 0)
            {
                throw new ArgumentOutOfRangeException("deadline must be positive and not zero");
            }



            try
            {
                for (int i = 0; i < TrysCount; i++)
                {
                    var tryResult = await consumer.TryGetAsync(TrysCount, TrysDeadline);

                    if (tryResult.IsSuccess)
                    {
                        return tryResult.Value;
                    }
                }

                return Result.Failure<ValueT>("all trys was unsucsesfull");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static Task<Result<ValueT>> TryGetAsync<KeyT, ValueT>(this IConsumer<KeyT, ValueT> consumer, int TrysDeadline)
        {
            ArgumentNullException.ThrowIfNull(consumer, nameof(consumer));

            if(TrysDeadline <= 0)
            {
                throw new ArgumentOutOfRangeException("deadline must be positive and not zero");
            }

            var completitionSource = new TaskCompletionSource<Result<ValueT>>();

            try
            {

                var consumeResult = consumer.Consume(TrysDeadline);

                if (consumeResult == null)
            {
                    completitionSource.SetResult(Result.Failure<ValueT>("deadline was entered"));
                }
                else
                {
                    completitionSource.SetResult(Result.Success(consumeResult.Message.Value));
                }

                return completitionSource.Task;
            }
            catch(Exception ex)
            {
                completitionSource.SetException(ex);
                return completitionSource.Task;
            }
        }
    }
}
