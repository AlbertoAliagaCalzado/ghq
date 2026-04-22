using System;
using System.Threading.Tasks;
using NotifyHub.Application.DTOs;

namespace NotifyHub.Application.Interfaces;

public interface INotificationDispatcher
{
    Task DispatchToUserAsync(Guid userId, NotificationDto notification);
}