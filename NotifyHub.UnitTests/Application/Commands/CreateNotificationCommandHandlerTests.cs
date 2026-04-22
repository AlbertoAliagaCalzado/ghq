using System;
using System.Threading;
using System.Threading.Tasks;
using NotifyHub.Application.Commands.CreateNotification;
using NotifyHub.Application.DTOs;
using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Entities;
using NotifyHub.Domain.Enums;
using NotifyHub.Domain.Repositories;
using Moq;
using Xunit;

namespace NotifyHub.UnitTests.Application.Commands;

public class CreateNotificationCommandHandlerTests
{
    private readonly Mock<INotificationRepository> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<INotificationDispatcher> _mockDispatcher;
    private readonly CreateNotificationCommandHandler _handler;

    public CreateNotificationCommandHandlerTests()
    {
        _mockRepository = new Mock<INotificationRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDispatcher = new Mock<INotificationDispatcher>();

        _handler = new CreateNotificationCommandHandler(
            _mockRepository.Object,
            _mockUnitOfWork.Object,
            _mockDispatcher.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Save_Notification_And_Dispatch_To_User()
    {
        var command = new CreateNotificationCommand(
            RecipientId: Guid.NewGuid(),
            ActorId: Guid.NewGuid(),
            Type: NotificationType.Mention,
            Message: "Te han mencionado."
        );

        var resultId = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, resultId);
        
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Notification>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockDispatcher.Verify(disp => disp.DispatchToUserAsync(
            command.RecipientId, 
            It.Is<NotificationDto>(dto => dto.Message == command.Message)
        ), Times.Once);
    }
}