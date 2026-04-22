using System;
using NotifyHub.Application.DTOs;
using MediatR;

namespace NotifyHub.Application.Queries.GetNotificationById;

public record GetNotificationByIdQuery(Guid Id) : IRequest<NotificationDto?>;