using AutoMapper;
using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Application.Features.AppPartner.UploadFile;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using F88.Digital.Domain.Entities.Share;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PawnOnlineAuditSystem = F88.Digital.Application.Features.AppPartner.Payment.Queries.PawnOnlineAuditSystem;
using RestSharp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using F88.Digital.Application.DTOs.Settings;
using Microsoft.Extensions.Options;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class ReadGoogleSheetOwnedRepository : IHostedService, IDisposable
    {
        private bool isRunning = false;
        private readonly ILogger<ReadGoogleSheetOwnedRepository> _logger;
        private Timer _timer;
        private readonly SpreadSheetIdSetting _spreadSheetIdSetting;
        public IServiceProvider Services { get; }
        public ReadGoogleSheetOwnedRepository(ILogger<ReadGoogleSheetOwnedRepository> logger, IServiceProvider services, IOptions<SpreadSheetIdSetting> spreadSheetIdSetting)
        {
            _logger = logger;
            Services = services;
            _spreadSheetIdSetting = spreadSheetIdSetting.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(GoogleSheetOwned, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }
        private void GoogleSheetOwned(object state)
        {
            if (!isRunning)
            {
                isRunning = true;
                using (var scope = Services.CreateScope())
                {
                    var readGoogleSheet =
                        scope.ServiceProvider
                            .GetRequiredService<IPaymentRepository>();
                    readGoogleSheet.ReadGoogleSheet(_spreadSheetIdSetting.GoogleOnwedSpreadSheetId);
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
