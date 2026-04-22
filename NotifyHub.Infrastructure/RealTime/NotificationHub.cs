using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NotifyHub.Infrastructure.RealTime;

public class NotificationHub : Hub
{
    public async Task IdentifyUser(Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
    }
}