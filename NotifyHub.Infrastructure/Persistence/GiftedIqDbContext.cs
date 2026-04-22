using NotifyHub.Application.Interfaces;
using NotifyHub.Domain.Entities;
using NotifyHub.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Persistence;

public class NotifyHubDbContext : DbContext, IUnitOfWork
{
    public NotifyHubDbContext(DbContextOptions<NotifyHubDbContext> options) : base(options) { }

    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>(builder =>
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id)
                   .HasConversion(
                       id => id.Value,
                       value => new NotificationId(value));
            builder.Property(n => n.Message).IsRequired().HasMaxLength(500);
        });
    }
}