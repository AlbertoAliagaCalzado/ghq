using System;
using System.Threading.Tasks;
using NotifyHub.Application.DTOs;
using NotifyHub.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace NotifyHub.Infrastructure.RealTime;

public class SignalRNotificationDispatcher : INotificationDispatcher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationDispatcher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task DispatchToUserAsync(Guid userId, NotificationDto notification)
    {
        await _hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", notification);
    }
}