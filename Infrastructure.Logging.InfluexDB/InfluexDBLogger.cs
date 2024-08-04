using App.Stores;
using Common;
using Domain.Entitys;
using Infrastructure.Logging.InfluxDB;
using System;
using System.Collections.Generic;
using System.Linq;
using InfluxDB3.Client;
using System.Text;

namespace Infrastructure.Logging.InfluxDB
{
    public class InfluexDBLogger : ILogger
    {
        private readonly IInfluxDBClient _client;

        public InfluexDBLogger(IInfluxDBClient client)
        {
            _client = client;
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

            await _client.WritePointAsync(point, "TestBucket");
        }

        public async Task LogError(Error exception)
        {
            var point = InfluxDB3.Client.Write.PointData.Measurement("Logs")
                .SetStringField("type", "Error")
                .SetTimestamp(DateTime.Now)
                .SetStringField("Message", exception.ErrorMessage);

            await _client.WritePointAsync(point, "TestBucket");
        }

        public async Task LogRequest(Guid bluepringId, Guid customerId)
        {

            var point = InfluxDB3.Client.Write.PointData.Measurement("Action")
                .SetStringField("type", "Request")
                .SetTimestamp(DateTime.Now)
                .SetStringField("BlueprintId", bluepringId.ToString())
                .SetStringField("CustomerId", customerId.ToString());

            await _client.WritePointAsync(point, "TestBucket");
        }
    }
}
