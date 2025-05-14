using Microsoft.AspNetCore.SignalR;

namespace TaskForge.WebUI.Hubs;

public class NotificationHub : Hub
{
	public async Task SendNotification(string userId, string message)
	{
		await Clients.User(userId).SendAsync("ReceiveNotification", message);
	}
	public override Task OnConnectedAsync()
	{
		Console.WriteLine($"Client connected: {Context.UserIdentifier}");
		return base.OnConnectedAsync();
	}
}
