using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Consumer.Messages
{
    public record NotificationSendReport
    (
        Guid Id,
        bool IsSucsessfull
    );
}
