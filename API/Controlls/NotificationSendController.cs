using App.InputObjects;
using App.Services.UseCases;
using Microsoft.AspNetCore.Mvc;
using Notification.Request;

namespace Notification.Controlls;

[Controller]
public class NotificationSendController : Controller
{
    protected readonly SendNotificationUseCase _service;

    public NotificationSendController(SendNotificationUseCase notificationService)
    {
        _service = notificationService;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Notify([FromBody] SendNotificationRequest request)
    {

        if (Guid.TryParse(request.BlueprintId, out var blueprintGuid) == false)
        {
            return BadRequest("invalid input data");
        }
        if (Guid.TryParse(request.CustomerId, out var customerGuid) == false)
        {
            return BadRequest("invalid input data");
        }

        var InputObject = new SendNotificationDTO(Guid.Parse(request.CustomerId), Guid.Parse(request.BlueprintId));

        var sendResult = await _service.Send(InputObject);

        if (sendResult.IsFailure)
        {
            return BadRequest(sendResult.Error);
        }

        return Ok($"Notification with params: CustomerId {request.CustomerId} and blueprintId {request.BlueprintId} was sended");
    }
}