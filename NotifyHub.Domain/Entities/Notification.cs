using System;
using NotifyHub.Domain.Enums;
using NotifyHub.Domain.ValueObjects;

namespace NotifyHub.Domain.Entities;

public class Notification
{
    public NotificationId Id { get; private set; }
    public Guid RecipientId { get; private set; }
    public Guid? ActorId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification() 
    { 
        Id = null!;
        Message = null!;
    }

    public static Notification Create(Guid recipientId, Guid? actorId, NotificationType type, string message)
    {
        return new Notification
        {
            Id = NotificationId.New(),
            RecipientId = recipientId,
            ActorId = actorId,
            Type = type,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsRead()
    {
        if (!IsRead)
        {
            IsRead = true;
        }
    }
}