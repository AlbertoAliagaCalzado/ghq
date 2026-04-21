using System;
using GiftedIQ.Domain.Enums;
using MediatR;

namespace GiftedIQ.Application.Commands.CreateNotification;

public record CreateNotificationCommand(
    Guid RecipientId,
    Guid? ActorId,
    NotificationType Type,
    string Message
) : IRequest<Guid>;