﻿using App.Stores;
using Common;
using Domain.Entitys;
using Infrastructure.Logging.InfluxDB;
using System;
using System.Collections.Generic;
using System.Linq;
using InfluxDB3.Client;
using System.Text;
using App.InputObjects;

namespace Infrastructure.Logging.InfluxDB
{
    public class InfluexDBLogger : ILogger
    {
        private readonly IInfluxDBClient _client;
        private readonly string _bucketName;

        public InfluexDBLogger(IInfluxDBClient client, string bucketName)
        {
            _client = client;
            _bucketName = bucketName;
        }

        public async Task LogAddToQueue(Notification message)
        {
            var point = InfluxDB3.Client.Write.PointData.Measurement("Action")
                .SetStringField("type", "AddToQueue")
                .SetTimestamp(DateTime.Now)
                .SetStringField("NotificationId", message.Id.ToString())
                .SetStringField("TemplateId", message.Blueprint.Id.ToString())
                .SetStringField("ResiverId", message.Resiver.Id.ToString())
                .SetStringField("CreatedAt", message.CreatedAt.ToString());

            await _client.WritePointAsync(point, _bucketName);
        }

        public async Task LogError(Error exception)
        {
            var point = InfluxDB3.Client.Write.PointData.Measurement("Logs")
                .SetStringField("type", "Error")
                .SetTimestamp(DateTime.Now)
                .SetStringField("Message", exception.ErrorMessage);

            await _client.WritePointAsync(point, _bucketName);
        }

        public async Task LogGetReport(ResivedReport report)
        {
            var point = InfluxDB3.Client.Write.PointData.Measurement("Action")
                .SetStringField("type", "Report")
                .SetTimestamp(DateTime.Now)
                .SetStringField("NotificationId", report.NotificationId.ToString())
                .SetBooleanField("IsSucsessfull", report.isSucsessfull);

            await _client.WritePointAsync(point, _bucketName);
        }

        public async Task LogRequest(Guid bluepringId, Guid customerId)
        {

            var point = InfluxDB3.Client.Write.PointData.Measurement("Action")
                .SetStringField("type", "Request")
                .SetTimestamp(DateTime.Now)
                .SetStringField("BlueprintId", bluepringId.ToString())
                .SetStringField("CustomerId", customerId.ToString());

            await _client.WritePointAsync(point, _bucketName);
        }
    }
}
