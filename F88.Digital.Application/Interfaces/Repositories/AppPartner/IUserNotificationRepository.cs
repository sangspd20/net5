using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IUserNotificationRepository
    {
        Task<int> InsertAsync(UserNotification userNoti);

        IQueryable<UserNotification> UserNotifications { get; }

        Task UpdateAsync(UserNotification userProfile);

        Task<List<UserNotification>> GetByUserAsync(int userProfileId);

        Task InsertRangeAsync(List<UserNotification> lstuserNoti);
    }
}
