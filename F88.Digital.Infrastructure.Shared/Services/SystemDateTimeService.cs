using F88.Digital.Application.Interfaces.Shared;
using System;

namespace F88.Digital.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}