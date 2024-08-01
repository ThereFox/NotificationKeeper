using App.Stores;
using Microsoft.Extensions.DependencyInjection;
using Octonica.ClickHouseClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Loging
{
    public static class DI
    {
        public static IServiceCollection AddSpecialClickhouseLogger(this IServiceCollection collection, string connectionString)
        {
            collection.AddScoped<ClickHouseConnection>(
                service =>
                {
                    var factory = new ClickHouseConnection(connectionString);
                    return factory;
                }
            );

            collection.AddScoped<ILogger, ClickHouseLogger>();

            return collection;
        }
    }
}
