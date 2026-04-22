using System;
using System.Threading.Tasks;
using NotifyHub.Application.Commands.CreateNotification;
using NotifyHub.Application.Queries.GetNotificationById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NotifyHub.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationCommand command)
    {
        var notificationId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = notificationId }, new { Id = notificationId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetNotificationByIdQuery(id);
        var notification = await _mediator.Send(query);

        if (notification == null)
        {
            return NotFound(new { Message = $"No se encontró la notificación con el ID especificado." });
        }

        return Ok(notification);
    }
}