using System;
using System.Threading;
using System.Threading.Tasks;
using GiftedIQ.Application.Interfaces;
using GiftedIQ.Domain.Entities;
using GiftedIQ.Domain.Repositories;
using MediatR;

namespace GiftedIQ.Application.Commands.CreateNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
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
        return notification.Id.Value;
    }
}