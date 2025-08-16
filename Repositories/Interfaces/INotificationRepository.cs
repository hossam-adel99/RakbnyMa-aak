using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        IQueryable<Notification> GetUserNotifications(string userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
