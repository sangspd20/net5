using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IUserBankRepository
    {
        IQueryable<Bank> Banks { get; }
        Task DeleteAsync(UserBank userBank);
        Task<int> InsertAsync(UserBank userBank);
        Task UpdateAsync(UserBank userBank);
        Task<List<UserBank>> GetListBanksByUserAsync(int userProfileId);
        Task<UserBank> GetByIdAsync(int id);
        Task ActiveUserBankAsync(int userBankId, int userProfileId, UserBank userBank);

        Task<bool> IsExistAccNumberAsync(string accNumber);

        Task<UserBank> GetByAccNumberAsync(string accNumber);
    }
}
