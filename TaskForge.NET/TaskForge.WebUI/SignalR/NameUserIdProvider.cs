
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TaskForge.WebUI.SignalR
{
	public class NameUserIdProvider : IUserIdProvider
	{
		public string? GetUserId(HubConnectionContext connection)
		{
			var userId = connection.User?.FindFirst("sub")?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				Console.WriteLine("[DEBUG] 'sub' claim not found. Dumping all claims:");
				foreach (var claim in connection.User?.Claims ?? Enumerable.Empty<Claim>())
				{
					Console.WriteLine($"[DEBUG] Claim Type: {claim.Type}, Value: {claim.Value}");
				}
			}

			Console.WriteLine($"[DEBUG] Extracted UserId: {userId}");
			return userId;
		}
	}
}


