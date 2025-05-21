using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskForge.Domain.Entities;

namespace TaskForge.Application.Interfaces.Services;
public interface INotificationService
{
	Task NotifyUserAsync(string userId, Notification notification);
	Task NotifyUsersAsync(IEnumerable<string> userIds, Notification notification);

	Task AddAsync(Notification notification);
	Task<IEnumerable<Notification>> GetAllNotificationsAsync(int userProfileId);
	Task MarkAsReadAsync(int notificationId);
}
