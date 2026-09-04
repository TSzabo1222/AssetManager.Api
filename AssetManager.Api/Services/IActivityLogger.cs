using AssetManager.Api.Data;
using AssetManager.Api.Models;

namespace AssetManager.Api.Services
{
    public interface IActivityLogger
    {
        Task LogAsync(string userName, string action, string entityType, string entityName);
    }

    public class ActivityLogger : IActivityLogger
    {
        private readonly AppDbContext _context;

        public ActivityLogger(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string userName, string action, string entityType, string entityName)
        {
            _context.ActivityLogs.Add(new ActivityLog
            {
                UserName = userName,
                Action = action,
                EntityType = entityType,
                EntityName = entityName,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
    }
}