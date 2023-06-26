using F88.Digital.Application.DTOs.POL.Request;
using F88.Digital.Application.DTOs.POL.Response;
using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.Interfaces.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Shared.Services
{
    public class ApiPolService : IApiPolService
    {
        private readonly UrlPOLSetting _urlPOLSetting;
        private readonly ILogger<ApiPolService> _logger;
        public ApiPolService(IOptions<UrlPOLSetting> urlPOLSetting, ILogger<ApiPolService> logger)
        {
            _urlPOLSetting = urlPOLSetting.Value;
            _logger = logger;
        }
        public ReponseSendPolCancelModel ReSendPolCancel()
        {
            try
            {
                var urlPOLCancel = _urlPOLSetting.UrlPOLCancel;
                var client = new RestClient(urlPOLCancel);
                var req = new RestRequest("", Method.POST);
                req.AddJsonBody(JsonConvert.SerializeObject(new RequestSendPolCancelModel{ Pagesize = int.Parse(_urlPOLSetting.PageSize) }));
                var rs = client.Execute(req);
                var jsonData = JsonConvert.DeserializeObject<ReponseSendPolCancelModel>(rs.Content);
                if (jsonData == null)
                {
                    jsonData = new ReponseSendPolCancelModel
                    {
                        ok = false,
                        Code = (int)rs.StatusCode
                    };
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ReponseSendPolCancelModel
                {
                    ok = false,
                    Code = 400
                };
            }
        }
    }
}
