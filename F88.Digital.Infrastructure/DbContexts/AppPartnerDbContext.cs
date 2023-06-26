using AspNetCoreHero.Abstractions.Domain;
using F88.Digital.Application.Interfaces.Contexts;
using F88.Digital.Application.Interfaces.Shared;
using F88.Digital.Domain.Entities.Catalog;
using AspNetCoreHero.EntityFrameworkCore.Auditing;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using F88.Digital.Domain.Entities.AppPartner;
using System;

namespace F88.Digital.Infrastructure.DbContexts
{
    public class AppPartnerDbContext : AuditableContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public AppPartnerDbContext(DbContextOptions<AppPartnerDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserAuthToken> UserAuthTokens { get; set; }

        public DbSet<UserLoanReferral> UserLoanReferrals { get; set; }

        public DbSet<UserDevice> UserDevices { get; set; }

        public DbSet<UserBank> UserBanks { get; set; }

        public DbSet<TransactionHistory> TransactionHistorys { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

        public DbSet<UserOTP> UserOTP { get; set; }

        public DbSet<PaymentUserLoanReferral> PaymentUserLoanReferral { get; set; }

        public DbSet<UserNotification> UserNotification { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<GroupMenu> GroupMenus { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();

        public bool HasChanges => ChangeTracker.HasChanges();

        public override async Task<int> SaveChangesAsync(string userId = null)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                var entityType = entry.Entity.GetType().Name;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.Now;
                        entry.Entity.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.Now;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                }
            }
            return await base.SaveChangesAsync(userId);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // IDX UserProfile
            builder.Entity<UserProfile>().HasIndex(p => p.Passport).IsUnique().IsClustered(false);
            builder.Entity<UserProfile>().HasIndex(p => p.UserPhone).IsUnique().IsClustered(false);
            builder.Entity<UserProfile>().HasIndex(p => p.FirstName).IsClustered(false);
            builder.Entity<UserProfile>().HasIndex(p => p.LastName).IsClustered(false);

            //IDX UserLoanReferral
            builder.Entity<UserLoanReferral>().HasIndex(p => p.UserProfileId).IsClustered(false);
            builder.Entity<UserLoanReferral>().HasIndex(p => p.PhoneNumber).IsClustered(false);
            builder.Entity<UserLoanReferral>().HasIndex(p => p.FullName).IsClustered(false);
            builder.Entity<UserLoanReferral>().HasIndex(p => p.PhoneNumber).IsClustered(false);
            //IDX UserDevice
            builder.Entity<UserDevice>().HasIndex(p => p.DeviceId).IsClustered(false);
            builder.Entity<UserDevice>().HasIndex(p => p.DeviceName).IsClustered(false);

            //IDX UserBank
            builder.Entity<UserBank>().HasIndex(p => p.AccNumber).IsUnique().IsClustered(false);
            builder.Entity<UserBank>().HasIndex(p => p.AccName).IsClustered(false);

            //IDX TransactionHistory
            builder.Entity<TransactionHistory>().HasIndex(p => p.TransactionId).IsUnique().IsClustered(false);
            builder.Entity<TransactionHistory>().HasIndex(p => p.TransactionDate).IsClustered(false);
            builder.Entity<TransactionHistory>().HasIndex(p => p.TransFinalStatus).IsClustered(false);
            builder.Entity<TransactionHistory>().HasIndex(p => p.TransactionType).IsClustered(false);

            //IDX Payment
            builder.Entity<Payment>().HasIndex(p => p.TransferDate).IsClustered(false);

            //IDX AppNoti
            builder.Entity<Notification>().HasIndex(p => p.NotiTypeCode).IsClustered(false);

            //Delete Cascade
            builder.Entity<UserProfile>().HasMany(p => p.TransactionHistorys).WithOne(p => p.UserProfile).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserProfile>().HasMany(p => p.UserLoanReferrals).WithOne(p => p.UserProfile).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserProfile>().HasMany(p => p.UserBanks).WithOne(p => p.UserProfile).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLoanReferral>().HasOne(p => p.UserProfile).WithMany(p => p.UserLoanReferrals).HasForeignKey(p => p.UserProfileId).OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Group>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<UserGroup>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Menu>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<GroupMenu>().Property(p => p.Id).ValueGeneratedOnAdd();


            base.OnModelCreating(builder);
        }
    }
}