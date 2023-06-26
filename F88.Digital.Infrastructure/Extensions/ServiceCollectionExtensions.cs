using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Infrastructure.Repositories.AppPartner;
using F88.Digital.Application.Interfaces.CacheRepositories.AppPartner;
using F88.Digital.Infrastructure.CacheRepositories.AppPartner;
using F88.Digital.Application.Interfaces.Repositories.Share;
using F88.Digital.Infrastructure.Repositories.Share;

namespace F88.Digital.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddScoped<IAppPartnerDbContext, AppPartnerDbContext>();
            //services.AddScoped<IAffiliateDbContext, AffiliateDbContext>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            #region Repositories App Partner
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductCacheRepository, ProductCacheRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IBrandCacheRepository, BrandCacheRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
           
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient(typeof(IShareRepositoryAsync<>), typeof(ShareRepositoryAsync<>));
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IUserBankRepository, UserBankRepository>();
            services.AddTransient<IUserLoanReferralRepository, UserLoanReferralRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IAppNotificationRepository, AppNotificationRepository>();
            services.AddTransient<IUserNotificationRepository, UserNotificationRepository>();
            services.AddTransient<IPaymentUserLoanReferralRepository, PaymentUserLoanReferralRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ILocationShopRepository, LocationShopRepository>();
            services.AddTransient<IGroupMenuRepository, GroupMenuRepository>();
            services.AddTransient<IAWSS3Repository, AWSS3Repository>();
            #endregion Repositories
        }
    }
}