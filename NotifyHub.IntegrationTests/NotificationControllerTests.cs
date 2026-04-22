using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NotifyHub.Application.DTOs;
using Moq;
using Xunit;

namespace NotifyHub.IntegrationTests;

public class NotificationsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public NotificationsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(); 
    }

    [Fact]
    public async Task Post_Notification_ReturnsCreated_And_TriggersSignalR()
    {
        var recipientId = Guid.NewGuid();
        var payload = new
        {
            recipientId = recipientId,
            actorId = Guid.NewGuid(),
            type = "Like",
            message = "Test de integración E2E"
        };

        _factory.MockDispatcher.Invocations.Clear(); 

        var response = await _client.PostAsJsonAsync("/api/Notification", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        _factory.MockDispatcher.Verify(d => d.DispatchToUserAsync(
            It.Is<Guid>(id => id == recipientId),
            It.Is<NotificationDto>(dto => dto.Message == payload.message)
        ), Times.Once);
    }
}