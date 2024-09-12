using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Cache.Notifications.DI
{
    public class CacheConnectionGetter
    {
        private readonly ConfigurationOptions _configuration;

        public CacheConnectionGetter(string Host, int Port, string UserName, string UserPassword)
        {
            var endpoints = new EndPointCollection();
            endpoints.Add(Host, Port);

            var configuration = new ConfigurationOptions()
            {
                AsyncTimeout = 200,
                //ChannelPrefix = RedisChannel.Literal("testApp_"),
                //ClientName = "NotificationKeeper",
                ConnectRetry = 3,
                EndPoints = endpoints,
                AbortOnConnectFail = false
            };

            _configuration = configuration;
        }

        public ConnectionMultiplexer GetConnection()
        {
            var connect = ConnectionMultiplexer.Connect(_configuration);
            return connect;
        }
    }
}
