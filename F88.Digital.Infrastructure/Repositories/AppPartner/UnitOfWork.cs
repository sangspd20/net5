using AspNetCoreHero.Abstractions.Domain;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Application.Interfaces.Shared;
using F88.Digital.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        private readonly AppPartnerDbContext _dbContext;

        private bool disposed;

        public UnitOfWork(AppPartnerDbContext dbContext, IAuthenticatedUserService authenticatedUserService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<int> Commit(CancellationToken cancellationToken, string userId = null)
        {          
            //Auditable Commits
            if (userId == null)
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await _dbContext.SaveChangesAsync(userId);
            }
        }

        public Task Rollback()
        {
            //todo
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
    }
}