using NotifyHub.Application.Commands.CreateNotification;
using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Repositories;
using NotifyHub.Infrastructure.Persistence;
using NotifyHub.Infrastructure.RealTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateNotificationCommand).Assembly));

builder.Services.AddDbContext<NotifyHubDbContext>(options =>
    options.UseInMemoryDatabase("NotifyHubDb"));

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<IUnitOfWork>(provider => 
    provider.GetRequiredService<NotifyHubDbContext>());

builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationDispatcher, SignalRNotificationDispatcher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseMiddleware<NotifyHub.Presentation.Middlewares.ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();

public partial class Program { }