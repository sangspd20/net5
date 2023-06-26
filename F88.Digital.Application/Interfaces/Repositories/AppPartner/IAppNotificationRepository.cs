using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IAppNotificationRepository
    {
        IQueryable<Notification> AppNotifications { get; }

        Task<int> InsertAsync(Notification userProfile);

        Task<Notification> GetByIdAsync(int Id);

        Task<List<Notification>> GetPublicNotificationsByDate(DateTime fromDate, DateTime toDate);
    }
}
