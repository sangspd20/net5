using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface ITransactionHistoryRepository
    {
        IQueryable<TransactionHistory> UserProfiles { get; }

        Task<int> InsertAsync(TransactionHistory userProfile);
    }
}
