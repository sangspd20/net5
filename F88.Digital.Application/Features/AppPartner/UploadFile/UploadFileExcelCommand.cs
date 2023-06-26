using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Entity = F88.Digital.Domain.Entities.AppPartner.UserBank;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using F88.Digital.Domain.Entities.AppPartner;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using F88.Digital.Application.Helpers;
using Newtonsoft.Json;
using F88.Digital.Application.Extensions;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create
{
    public partial class UploadFileExcelCommand
    {
        public string PhoneNumber { get; set; }
        public IFormFile Files { get; set; }


    }
}
