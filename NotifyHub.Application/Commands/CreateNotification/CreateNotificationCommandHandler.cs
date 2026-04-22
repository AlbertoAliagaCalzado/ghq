using System;
using System.Threading;
using System.Threading.Tasks;
using NotifyHub.Application.DTOs;
using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Entities;
using NotifyHub.Domain.Repositories;
using MediatR;

namespace NotifyHub.Application.Commands.CreateNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationDispatcher _dispatcher;

    public CreateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        INotificationDispatcher dispatcher)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _dispatcher = dispatcher;
    }

    public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(
            request.RecipientId,
            request.ActorId,
            request.Type,
            request.Message
        );
        
        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var notificationDto = new NotificationDto(
            notification.Id.Value,
            notification.RecipientId,
            notification.ActorId,
            notification.Type.ToString(),
            notification.Message,
            notification.IsRead,
            notification.CreatedAt
        );

        await _dispatcher.DispatchToUserAsync(notification.RecipientId, notificationDto);

        return notification.Id.Value;
    }
}