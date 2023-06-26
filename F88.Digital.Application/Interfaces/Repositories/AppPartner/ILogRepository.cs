using F88.Digital.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface ILogRepository
    {
        Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId);

        Task AddLogAsync(string action, string userId);
    }
}