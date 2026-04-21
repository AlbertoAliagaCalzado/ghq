using System;
using GiftedIQ.Application.DTOs;
using MediatR;

namespace GiftedIQ.Application.Queries.GetNotificationById;

public record GetNotificationByIdQuery(Guid Id) : IRequest<NotificationDto?>;