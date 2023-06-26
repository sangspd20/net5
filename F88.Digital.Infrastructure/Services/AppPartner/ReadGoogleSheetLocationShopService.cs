using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Services.AppPartner
{
    public class ReadGoogleSheetLocationShopService : IHostedService, IDisposable
    {
        private bool isRunning = false;
        private readonly ILogger<ReadGoogleSheetLocationShopService> _logger;
        private Timer _timer;
        public IServiceProvider Services { get; }
        public ReadGoogleSheetLocationShopService(ILogger<ReadGoogleSheetLocationShopService> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(ReadGoogleSheetLocationShop, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }
        private void ReadGoogleSheetLocationShop(object state)
        {
            if (!isRunning)
            {
                isRunning = true;
                using (var scope = Services.CreateScope())
                {
                    var googleSheetLocationShop =
                       scope.ServiceProvider
                           .GetRequiredService<ILocationShopRepository>();
                    googleSheetLocationShop.ReadGoogleSheetLocationShop();
                }
                isRunning = false;
            }

        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
