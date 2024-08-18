using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.InputObjects
{
    public record SendingReport
    (
        Guid NotificationId,
        bool isSucsessfull
    );
}
