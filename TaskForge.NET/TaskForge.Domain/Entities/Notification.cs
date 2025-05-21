using TaskForge.Domain.Entities.Common;
using TaskForge.Domain.Enums;

namespace TaskForge.Domain.Entities;

public class Notification : BaseEntity
{
	public int UserProfileId { get; set; }

	public string Title { get; set; } = default!;
	public string Message { get; set; } = default!;
	public bool IsRead { get; set; } = false;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public NotificationType Type { get; set; }
	public int? RelatedEntityId { get; set; }
	public string? TargetUrl { get; set; }


	public UserProfile? UserProfile { get; set; }
}


