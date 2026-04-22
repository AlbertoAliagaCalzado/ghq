using GiftedIQ.Application.Commands.CreateNotification;
using GiftedIQ.Application.Interfaces;
using GiftedIQ.Domain.Repositories;
using GiftedIQ.Infrastructure.Persistence;
using GiftedIQ.Infrastructure.RealTime;
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

builder.Services.AddDbContext<GiftedIqDbContext>(options =>
    options.UseInMemoryDatabase("GiftedIqDb"));

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<IUnitOfWork>(provider => 
    provider.GetRequiredService<GiftedIqDbContext>());

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

app.UseMiddleware<GiftedIQ.Presentation.Middlewares.ExceptionMiddleware>();

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