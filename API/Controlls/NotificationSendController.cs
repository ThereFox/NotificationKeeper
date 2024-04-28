using App.Services;
using Microsoft.AspNetCore.Mvc;
using Notification.Request;

namespace Notification.Controlls;

[Controller]
public class NotificationSendController : Controller
{
    protected readonly NotificationService _service;

    public NotificationSendController(NotificationService notificationService)
    {
        _service = notificationService;
    }
    
    
    public async Task<IActionResult> Notify([FromBody] SendNotificationRequest request)
    {

        if (Guid.TryParse(request.BlueprintId, out var blueprintGuid))
        {
            return BadRequest("invalid input data");
        }
        if (Guid.TryParse(request.CustomerId, out var customerGuid))
        {
            return BadRequest("invalid input data");
        }

        var sendResult = await _service.Send(blueprintGuid, customerGuid);

        if (sendResult.IsFailure)
        {
            return BadRequest(sendResult.Error);
        }

        return Ok();
    }
}