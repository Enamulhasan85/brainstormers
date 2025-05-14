using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskForge.Application.Interfaces.Services;
public interface INotificationService
{
	Task NotifyUserAsync(string userId, string message);
	Task NotifyUsersAsync(IEnumerable<string> userIds, string message);
}
