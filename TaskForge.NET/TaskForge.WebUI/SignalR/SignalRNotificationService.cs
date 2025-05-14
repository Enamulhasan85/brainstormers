using Microsoft.AspNetCore.SignalR;
using TaskForge.Application.Interfaces.Services;
using TaskForge.WebUI.Hubs;

namespace TaskForge.WebUI.SignalR
{
	public class SignalRNotificationService : INotificationService
	{
		private readonly IHubContext<NotificationHub> _hubContext;

		public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task NotifyUserAsync(string userId, string message)
		{
			await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
		}

		public async Task NotifyUsersAsync(IEnumerable<string> userIds, string message)
		{
			foreach (var userId in userIds)
			{
				await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
			}
		}
	}

}
