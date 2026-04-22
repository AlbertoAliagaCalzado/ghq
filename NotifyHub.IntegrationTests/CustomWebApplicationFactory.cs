using System.Linq;
using NotifyHub.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace NotifyHub.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<INotificationDispatcher> MockDispatcher { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(INotificationDispatcher));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<INotificationDispatcher>(_ => MockDispatcher.Object);
        });
    }
}