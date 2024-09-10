using Infrastructure.Kafka.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MessageBrocker.ConsumerService
{
    public static class ReportReaderServiceRegister
    {
        public static IServiceCollection AddConsumerService(this IServiceCollection services)
        {
            services.AddHostedService<ReportConsumerService>();

            return services;
        }
    }
}
