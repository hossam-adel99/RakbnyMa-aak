using Microsoft.AspNetCore.SignalR;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.SignalR;
using static RakbnyMa_aak.Utilities.Enums;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task SendNotificationAsync(
        string recipientUserId,
        ApplicationUser sender,
        string message,
        NotificationType type = NotificationType.Custom,
        string? relatedEntityId = null)
    {
        var notification = new Notification
        {
            UserId = recipientUserId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            Type = type,
            RelatedEntityId = relatedEntityId
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        var payload = new NotificationDto
        {
            Message = message,
            SenderId = sender.Id,
            SenderFullName = sender.FullName,
            SenderPicture = sender.Picture,
            Type = type,
            CreatedAt = notification.CreatedAt.ToString("g")
        };

        await _hubContext.Clients.User(recipientUserId)
            .SendAsync("ReceiveNotification", payload);
    }
}
