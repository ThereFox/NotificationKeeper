using App.InputObjects;
using App.Stores;
using Common;
using Domain.Entitys;
using Octonica.ClickHouseClient;

namespace Infrastructure.Loging
{
    public class ClickHouseLogger : ILogger
    {
        private readonly ClickHouseConnection _connection;

        private const string TableName = "CommonLogs";

        public ClickHouseLogger(ClickHouseConnection factory)
        {
            _connection = factory;
        }

        public async Task LogAddToQueue(Notification message)
        {
            await _connection.OpenAsync();

            var command = _connection.CreateCommand();
            var logMessage = $"notification id:{message.Id} (templateId:{message.Blueprint.Id} for {message.Resiver.Id} created at {message.SendedAt}) was add to queue";

            command.CommandText = $"INSERT INTO {TableName} Values (@date, @logMessage)";

            command.Parameters.AddWithValue("@date", DateTime.Now, ClickHouseDbType.DateTime);
            command.Parameters.AddWithValue("@logMessage", logMessage, ClickHouseDbType.String);

            await command.ExecuteNonQueryAsync();

            await command.DisposeAsync();

            await _connection.CloseAsync();
        }

        public async Task LogError(Error exception)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();

            var logMessage = $"error was throw {exception.ErrorMessage}";

            command.CommandText = $"INSERT INTO {TableName} Values (@date, @logMessage)";

            command.Parameters.AddWithValue("@date", DateTime.Now, ClickHouseDbType.DateTime);
            command.Parameters.AddWithValue("@logMessage", logMessage, ClickHouseDbType.String);

            await command.ExecuteNonQueryAsync();
                
            await command.DisposeAsync();


            await _connection.CloseAsync();
        }

        public async Task LogRequest(Guid bluepringId, Guid customerId)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();

                var logMessage = $"request with params: blueprintId = {bluepringId} customerId = {customerId}";

                command.CommandText = $"INSERT INTO {TableName} Values (@date, @logMessage)";

                command.Parameters.AddWithValue("@date", DateTime.Now, ClickHouseDbType.DateTime);
                command.Parameters.AddWithValue("@logMessage", logMessage, ClickHouseDbType.String);

                await command.ExecuteNonQueryAsync();

                await command.DisposeAsync();


            await _connection.CloseAsync();
        }

        public async Task LogGetReport(SendingReport report)
        {
            await _connection.OpenAsync();
            var command = _connection.CreateCommand();

            var logMessage = $"report with params: notificationId = {report.NotificationId} sucsess = {report.isSucsessfull}";

            command.CommandText = $"INSERT INTO {TableName} Values (@date, @logMessage)";

            command.Parameters.AddWithValue("@date", DateTime.Now, ClickHouseDbType.DateTime);
            command.Parameters.AddWithValue("@logMessage", logMessage, ClickHouseDbType.String);

            await command.ExecuteNonQueryAsync();

            await command.DisposeAsync();


            await _connection.CloseAsync();
        }
    }
}
