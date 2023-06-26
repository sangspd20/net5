using F88.Digital.Infrastructure.Identity.Models;
using AspNetCoreHero.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using System.IO;
using F88.Digital.Infrastructure.Repositories.AppPartner;
using F88.Digital.Infrastructure.Services.AppPartner;

namespace F88.Digital.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.GlobalContext.Properties["LogPath"] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp");
            log4net.Config.XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("app");
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                    await Infrastructure.Identity.Seeds.DefaultSuperAdminUser.SeedAsync(userManager, roleManager);
                    await Infrastructure.Identity.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);
                    logger.LogInformation("Finished Seeding Default Data");
                    logger.LogInformation("Application Starting");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "An error occurred seeding the DB");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((webHostBuilderContext, loggingBuilder) =>
            {
                loggingBuilder.AddLog4Net();
            })
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<ReadGoogleSheetFacebookRepository>();
            //})
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<ReadGoogleSheetGoogleRepository>();
            //})
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<ReadGoogleSheetFacebookMessRepository>();
            //})
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<ReadGoogleSheetGoogle2ndRepository>();
            //})
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<ReadGoogleSheetOwnedRepository>();
            //})
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<SendPolCancelRepository>();
            //})
            //.ConfigureServices(services =>
            //{
            //    services.AddHostedService<ReadGoogleSheetPartnershipRepository>();
            //})
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}