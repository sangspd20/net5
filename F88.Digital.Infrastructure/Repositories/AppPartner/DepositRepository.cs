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
    public class DepositRepository : IDepositRepository
    {
        private readonly IRepositoryAsync<Deposit> _repository;
        private readonly IDistributedCache _distributedCache;

        public DepositRepository(IRepositoryAsync<Deposit> repository, IDistributedCache distributedCache)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }

        public IQueryable<Deposit> Deposits => _repository.Entities;
    }
}
