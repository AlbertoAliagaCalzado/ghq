// Ruta: NotifyHub.UnitTests/Domain/Entities/NotificationTests.cs
using System;
using NotifyHub.Domain.Entities;
using NotifyHub.Domain.Enums;
using Xunit;

namespace NotifyHub.UnitTests.Domain.Entities;

public class NotificationTests
{
    [Fact]
    public void Create_Should_Initialize_Notification_Correctly()
    {
        var recipientId = Guid.NewGuid();
        var actorId = Guid.NewGuid();
        var type = NotificationType.Like;
        var message = "A alguien le gustó tu publicación";

        var notification = Notification.Create(recipientId, actorId, type, message);

        Assert.NotNull(notification);
        Assert.True(notification.Id.Value != Guid.Empty);
        Assert.Equal(recipientId, notification.RecipientId);
        Assert.Equal(actorId, notification.ActorId);
        Assert.Equal(type, notification.Type);
        Assert.Equal(message, notification.Message);
        Assert.False(notification.IsRead); // Por defecto debe ser false
        Assert.True(notification.CreatedAt <= DateTime.UtcNow);
    }
}