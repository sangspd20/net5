using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.EntityFrameworkCore.Auditing;
using F88.Digital.Application.Interfaces.Contexts;
using F88.Digital.Application.Interfaces.Shared;
using F88.Digital.Domain.Entities.Share;
using F88.Digital.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;

namespace F88.Digital.Infrastructure.DbContexts
{
    public class AffiliateDbContext : AuditableContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public AffiliateDbContext(DbContextOptions<AffiliateDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<LocationShop> LocationShop { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PawnOnlineAuditSystem> PawnOnlineAuditSystem { get; set; }
        public DbSet<GroupProvinceQuery> GroupProvinceQuery { get; set; }
        public DbSet<PawnOnlineSource> PawnOnlineSource { get; set; }

        public bool HasChanges => ChangeTracker.HasChanges();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                var entityType = entry.Entity.GetType().Name;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            builder.Entity<GroupProvinceQuery>().HasKey(p => p.GroupId);
        }
    }
}
