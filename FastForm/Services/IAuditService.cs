using FastForm.Models;

namespace FastForm.Services
{
    public interface IAuditService
    {
        Task LogActionAsync(string action, string entityType, int? entityId, int? userId, string? oldValue = null, string? newValue = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? entityType = null, int? entityId = null, int? userId = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<AuditLog>> GetUserActivityAsync(int userId, int top = 50);
        Task<IEnumerable<AuditLog>> GetEntityHistoryAsync(string entityType, int entityId);
    }
}
