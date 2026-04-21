using System;

namespace GiftedIQ.Application.DTOs;

public record NotificationDto(
    Guid Id,
    Guid RecipientId,
    Guid? ActorId,
    string Type,
    string Message,
    bool IsRead,
    DateTime CreatedAt
);