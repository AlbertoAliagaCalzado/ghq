namespace NotifyHub.Domain.ValueObjects;

public record NotificationId(Guid Value)
{
    public static NotificationId New() => new(Guid.NewGuid());
}