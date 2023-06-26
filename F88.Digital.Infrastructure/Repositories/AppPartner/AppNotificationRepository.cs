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
    public class AppNotificationRepository : IAppNotificationRepository
    {
        private readonly IRepositoryAsync<Notification> _repository;
        private readonly IDistributedCache _distributedCache;

        public AppNotificationRepository(IDistributedCache distributedCache, IRepositoryAsync<Notification> repository)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }

        public IQueryable<Notification> AppNotifications => _repository.Entities;

        public async Task<Notification> GetByIdAsync(int Id)
        {
            var appNoti = await AppNotifications
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            return appNoti;
        }

        public async Task<int> InsertAsync(Notification appNotification)
        {
            await _repository.AddAsync(appNotification);
            return appNotification.Id;
        }

        Task<List<Notification>> IAppNotificationRepository.GetPublicNotificationsByDate(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}
