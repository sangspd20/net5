using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Contexts
{
    public interface IAppPartnerDbContext
    {
        IDbConnection Connection { get; }
        bool HasChanges { get; }

        EntityEntry Entry(object entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserAuthToken> UserAuthTokens { get; set; }

        public DbSet<UserLoanReferral> UserLoanReferrals { get; set; }

        public DbSet<UserDevice> UserDevices { get; set; }
    }
}