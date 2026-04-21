using System.Threading;
using System.Threading.Tasks;
using GiftedIQ.Application.DTOs;
using GiftedIQ.Domain.Repositories;
using GiftedIQ.Domain.ValueObjects;
using MediatR;

namespace GiftedIQ.Application.Queries.GetNotificationById;

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationByIdQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var notificationId = new NotificationId(request.Id);
        
        var notification = await _notificationRepository.GetByIdAsync(notificationId);

        if (notification == null)
        {
            return null;
        }

        return new NotificationDto(
            notification.Id.Value,
            notification.RecipientId,
            notification.ActorId,
            notification.Type.ToString(),
            notification.Message,
            notification.IsRead,
            notification.CreatedAt
        );
    }
}