using System;
using NotifyHub.Domain.Enums;
using MediatR;

namespace NotifyHub.Application.Commands.CreateNotification;

public record CreateNotificationCommand(
    Guid RecipientId,
    Guid? ActorId,
    NotificationType Type,
    string Message
) : IRequest<Guid>;