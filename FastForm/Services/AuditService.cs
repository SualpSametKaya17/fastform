using FastForm.Data;
using FastForm.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FastForm.Services
{
    public class AuditService : IAuditService
    {
        private readonly FastFormDbContext _context;

        public AuditService(FastFormDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string action, string entityType, int? entityId, int? userId, string? oldValue = null, string? newValue = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    UserId = userId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    CreatedDate = DateTime.Now
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();

                Log.Debug("Audit log created: {Action} on {EntityType}:{EntityId}", action, entityType, entityId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating audit log");
                // Don't throw - audit logging failure shouldn't break the main operation
            }
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? entityType = null, int? entityId = null, int? userId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var query = _context.AuditLogs
                    .Include(a => a.User)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(entityType))
                    query = query.Where(a => a.EntityType == entityType);

                if (entityId.HasValue)
                    query = query.Where(a => a.EntityId == entityId);

                if (userId.HasValue)
                    query = query.Where(a => a.UserId == userId);

                if (fromDate.HasValue)
                    query = query.Where(a => a.CreatedDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(a => a.CreatedDate <= toDate.Value);

                return await query.OrderByDescending(a => a.CreatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting audit logs");
                throw;
            }
        }

        public async Task<IEnumerable<AuditLog>> GetUserActivityAsync(int userId, int top = 50)
        {
            try
            {
                return await _context.AuditLogs
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.CreatedDate)
                    .Take(top)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting user activity for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<AuditLog>> GetEntityHistoryAsync(string entityType, int entityId)
        {
            try
            {
                return await _context.AuditLogs
                    .Include(a => a.User)
                    .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                    .OrderByDescending(a => a.CreatedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting entity history for {EntityType}:{EntityId}", entityType, entityId);
                throw;
            }
        }
    }
}
