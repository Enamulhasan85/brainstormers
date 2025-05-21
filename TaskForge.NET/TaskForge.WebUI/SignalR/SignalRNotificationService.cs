using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskForge.Application.Interfaces.Repositories;
using TaskForge.Application.Interfaces.Repositories.Common;
using TaskForge.Application.Interfaces.Services;
using TaskForge.Domain.Entities;
using TaskForge.WebUI.Hubs;

namespace TaskForge.WebUI.SignalR;

public class SignalRNotificationService : INotificationService
{
	private readonly IHubContext<NotificationHub> _hubContext;
	private readonly INotificationRepository _notificationRepository;
	private readonly IUnitOfWork _unitOfWork;

	public SignalRNotificationService(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepository, IUnitOfWork unitOfWork )
	{
		_hubContext = hubContext;
		_notificationRepository = notificationRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task NotifyUserAsync(string userId, Notification notification)
	{
		await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
		{
			id = notification.Id,
			title = notification.Title,
			message = notification.Message,
			link = notification.TargetUrl,
			createdAt = notification.CreatedAt,
			type = notification.Type.ToString()
		});
	}

	public async Task NotifyUsersAsync(IEnumerable<string> userIds, Notification notification)
	{
		foreach (var userId in userIds)
		{
			await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
			{
				id = notification.Id,
				title = notification.Title,
				message = notification.Message,
				link = notification.TargetUrl,
				createdAt = notification.CreatedAt,
				type = notification.Type.ToString()
			});
		}
	}

	public async Task AddAsync(Notification notification)
	{
		await _notificationRepository.AddAsync(notification);
	}

	public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(int userProfileId)
	{
		return await _notificationRepository.FindByExpressionAsync(
			predicate: n => n.UserProfileId == userProfileId,
			orderBy: q => q.OrderByDescending(n => n.CreatedDate)
		);
	}

	[HttpPost]
	[Route("Notification/MarkAsRead/{id}")]
	public async Task MarkAsReadAsync(int notificationId)
	{
		var notification = await _notificationRepository.GetByIdAsync(notificationId);
		if (notification == null)
		{
			throw new ArgumentException("Notification not found.");
		}

		if (!notification.IsRead)
		{
			notification.IsRead = true;
			await _unitOfWork.SaveChangesAsync();
		}
	}


}