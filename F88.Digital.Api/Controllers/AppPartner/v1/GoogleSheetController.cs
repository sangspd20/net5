
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using F88.Digital.Application.DTOs.Settings;
using Microsoft.Extensions.Options;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    [AllowAnonymous]
    [Route("api/app-partner/v{version:apiVersion}/[controller]")]
    public class GoogleSheetController : Controller
    {
        private readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string applicationName = "Scanning GG sheet";
        private readonly string sheetName = "Data";
        private readonly SpreadSheetIdSetting _spreadSheetIdSetting;
        private readonly string urlPOL = "http://192.168.10.33:1994/LadipageReturnID/";
        //private SheetsService service;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<GoogleSheetController> _logger;
        public GoogleSheetController(ILogger<GoogleSheetController> logger, IPaymentRepository paymentRepository, IOptions<SpreadSheetIdSetting> spreadSheetIdSetting)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _spreadSheetIdSetting = spreadSheetIdSetting.Value;
        }
        [HttpGet("GoogleSheet")]
        public IActionResult ReadGoogleSheet(string sheetId)
        {
            try
            {
                var spreadSheetId = sheetId;
                var service = GoogleSheetHelper.Authentication(scopes, applicationName);
                var range = $"{sheetName}!A:K";
                SpreadsheetsResource.ValuesResource.GetRequest requestSheet =
                    service.Spreadsheets.Values.Get(spreadSheetId, range);
                var responseSheet = requestSheet.Execute();

                var listData = responseSheet.Values.Skip(1).Where(x => x[8].ToString() != "1");
                var listNull = responseSheet.Values.Skip(1).Where(x => x[8].ToString() == "1" && x[9].ToString() == "-1" && x[10].ToString() == "null");
                if (listNull.Any())
                {
                    foreach (var item in listNull)
                    {
                        var googleSheet = new DataGoogleSheetQuery
                        {
                            Position = item[0].ToString(),
                            FullName = item[1].ToString(),
                            PhoneNumber = StringUtils.FormatPhoneNumber(item[2].ToString()),
                            Url = item[3].ToString(),
                            Description = item[4].ToString(),
                            GroupId = item[5].ToString(),
                            Province = item[6].ToString(),
                            District = item[7].ToString(),
                            IsRead = item[8].ToString(),
                            Status = item[9].ToString(),
                            Note = item[10].ToString(),
                        };
                        var position = int.Parse(googleSheet.Position) + 1;

                        if (string.IsNullOrEmpty(googleSheet.GroupId))
                        {
                            googleSheet.GroupId = _paymentRepository.GetGroupIdByProvince(googleSheet.Province, googleSheet.District).ToString(); // random groupId;
                        }

                        var responsePOL = _paymentRepository.SendPOL(googleSheet); //call api insert Data POL
                        var responseJson = JsonConvert.DeserializeObject<JsonReponseQuery>(responsePOL.Content);
                        var status = responseJson?.success == true ? GoogleSheetStatus.Success.GetHashCode() : GoogleSheetStatus.Failed.GetHashCode();
                        if (responseJson?.message.Equals("Kết nối bị gián đoạn, tạo đơn không thành công. Vui lòng thử lại sau!") == true)
                        {
                            responseJson.message = "";
                        }
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, googleSheet.GroupId, spreadSheetId, sheetName);
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                        //GoogleSheetHelper.UpdateData(ColumnGoogleSheet.M, position, string.IsNullOrEmpty(responsePOL.ErrorMessage.ToString()) ? "null" : responsePOL.ErrorMessage.ToString(), spreadSheetId, sheetName);
                    }
                }
                if (listData.Any())
                {
                    foreach (var item in listData)
                    {
                        var googleSheet = new DataGoogleSheetQuery
                        {
                            Position = item[0].ToString(),
                            FullName = item[1].ToString(),
                            PhoneNumber = StringUtils.FormatPhoneNumber(item[2].ToString()),
                            Url = item[3].ToString(),
                            Description = item[4].ToString(),
                            GroupId = item[5].ToString(),
                            Province = item[6].ToString(),
                            District = item[7].ToString(),
                            IsRead = item[8].ToString(),
                            Status = item[9].ToString(),
                            Note = item[10].ToString(),
                        };
                        var position = int.Parse(googleSheet.Position) + 1;

                        if (string.IsNullOrEmpty(googleSheet.GroupId))
                        {
                            googleSheet.GroupId = _paymentRepository.GetGroupIdByProvince(googleSheet.Province, googleSheet.District).ToString(); // random groupId;
                        }

                        var responsePOL = _paymentRepository.SendPOL(googleSheet); //call api insert Data POL
                        var responseJson = JsonConvert.DeserializeObject<JsonReponseQuery>(responsePOL.Content);
                        var status = responseJson?.success == true ? GoogleSheetStatus.Success.GetHashCode() : GoogleSheetStatus.Failed.GetHashCode();
                        if (responseJson?.message.Equals("Kết nối bị gián đoạn, tạo đơn không thành công. Vui lòng thử lại sau!") == true)
                        {
                            responseJson.message = "";
                        }
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, googleSheet.GroupId, spreadSheetId, sheetName);
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                        //GoogleSheetHelper.UpdateData(ColumnGoogleSheet.M, position, string.IsNullOrEmpty(responsePOL.ErrorMessage.ToString()) ? "null" : responsePOL.ErrorMessage.ToString(), spreadSheetId, sheetName);
                    }
                }
                return Ok(new
                {
                    succeed = true,
                    message = "Thành công."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new
                {
                    succeed = false,
                    message = ex.ToString()
                });
            }
        }

    }
}
