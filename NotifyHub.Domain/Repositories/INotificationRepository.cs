using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotifyHub.Domain.Entities;
using NotifyHub.Domain.ValueObjects;

namespace NotifyHub.Domain.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<Notification?> GetByIdAsync(NotificationId id);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId);
    Task UpdateAsync(Notification notification);
}