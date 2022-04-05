using BugTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTNotifcationService
    {
        Task AddNotificationAsync(Notification notification);
        Task<List<Notification>> GetReceivedNotificationsAsync(string userId);
        Task<List<Notification>> GetSentNotificationsAsync(string userId);
        Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role);
        Task SendMembersEmailNotificationsAsync(Notification notification, IEnumerable<BTUser> members);
        Task<bool> SendEmailNotificationsAsync(Notification notification, string emailSubject);
    }
}
