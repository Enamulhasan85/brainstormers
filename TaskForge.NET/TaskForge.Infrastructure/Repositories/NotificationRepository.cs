using TaskForge.Application.Interfaces.Repositories;
using TaskForge.Application.Interfaces.Services;
using TaskForge.Domain.Entities;
using TaskForge.Infrastructure.Data;
using TaskForge.Infrastructure.Repositories.Common;

namespace TaskForge.Infrastructure.Repositories;

 public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    // Pass the context and IUserContextService to the base Repository class
    public NotificationRepository(ApplicationDbContext context, IUserContextService userContextService) 
        : base(context, userContextService)
    {

    }
}
