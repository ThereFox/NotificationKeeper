﻿using App.InputObjects;
using App.Interfaces.Notifications;
using CSharpFunctionalExtensions;
using Infrastructure.Brocker.Kafka.Consumer.Messages;
using Infrastructure.Reader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Consumer
{
    public class ReportListener : IReportReader
    {
        private readonly string _topicName;
        private KafkaConsumer _consumer;

        public ReportListener(KafkaConsumer consumer, string topicName)
        {
            _consumer = consumer;
            _topicName = topicName;
        }

        public async Task<Result<ResivedReport>> GetNewMessage()
        {
            var getMessageResult = await _consumer.GetNewMessageFromTopic(_topicName);

            if (getMessageResult.IsFailure)
            {
                return getMessageResult.ConvertFailure<ResivedReport>();
            }

            var jsonInputMessage = JsonConvert.DeserializeObject<NotificationSendReport>(getMessageResult.Value);

            if(jsonInputMessage == default)
            {
                return Result.Failure<ResivedReport>("invalid value readed");
            }

            var convertedMessage = new ResivedReport(jsonInputMessage.Id, jsonInputMessage.IsSucsessfull);

            return Result.Success(convertedMessage);

        }
    }
}