using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Notification> GetUserNotifications(string userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedAt);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notif = await _context.Notifications.FindAsync(notificationId);
            if (notif != null)
            {
                notif.IsRead = true;
                _context.Update(notif);
                await _context.SaveChangesAsync();
            }
        }
    }
}
