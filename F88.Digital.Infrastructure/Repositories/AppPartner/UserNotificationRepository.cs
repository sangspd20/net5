using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly IRepositoryAsync<UserNotification> _repository;
        private readonly IDistributedCache _distributedCache;

        public UserNotificationRepository(IDistributedCache distributedCache, IRepositoryAsync<UserNotification> repository)
        {
            this._distributedCache = distributedCache;
            this._repository = repository;
        }

        public IQueryable<UserNotification> UserNotifications => _repository.Entities;

        public async Task<List<UserNotification>> GetByUserAsync(int userProfileId)
        {
            var userNoti = await _repository.Entities
                .Where(x => x.UserProfileId == userProfileId)
                .ToListAsync();

            return userNoti;
        }

        public async Task<int> InsertAsync(UserNotification userNotification)
        {
            await _repository.AddAsync(userNotification);
            return userNotification.Id;
        }

        public async Task InsertRangeAsync(List<UserNotification> lstuserNoti)
        {
            await _repository.AddRangeAsync(lstuserNoti);
        }

        public Task UpdateAsync(UserNotification userNotification)
        {
            throw new NotImplementedException();
        }
    }
}
