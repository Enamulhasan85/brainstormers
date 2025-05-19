using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskForge.Application.Interfaces.Services;

namespace TaskForge.WebUI.Controllers;

[Authorize(Roles = "Admin, User, Operator")]
[Route("Notification")]
public class NotificationController : Controller
{
	private readonly IUserProfileService _userProfileService;
	private readonly INotificationService _notificationService;
	private readonly UserManager<IdentityUser> _userManager;

	public NotificationController(
		IUserProfileService userProfileService,
		INotificationService notificationService,
		UserManager<IdentityUser> userManager)
	{
		_userProfileService = userProfileService;
		_notificationService = notificationService;
		_userManager = userManager;
	}

	[HttpGet("GetUserNotifications")]
	public async Task<IActionResult> GetUserNotifications()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return Unauthorized();
		}
		var userProfileId = await _userProfileService.GetUserProfileIdByUserIdAsync(user.Id);
		if (userProfileId == null)
		{
			return Unauthorized();
		}
		var notifications = await _notificationService.GetAllNotificationsAsync(userProfileId.Value);
		return Json(notifications);
	}

	[HttpPost("MarkAsRead/{notificationId}")]
	public async Task<IActionResult> MarkAsRead(int notificationId)
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return Unauthorized();
		}

		await _notificationService.MarkAsReadAsync(notificationId);

		return Ok();
	}

}