using AutoMapper;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using F88.Digital.Application.DTOs.Settings;
using Microsoft.Extensions.Options;
using F88.Digital.Application.Features.AppPartner.LocationShop.Queries;
using F88.Digital.Domain.Entities.Share;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class LocationShopRepository : ILocationShopRepository
    {
        private readonly IRepositoryAsync<Payment> _repository;
        private readonly AppPartnerDbContext _context;
        private readonly AffiliateDbContext _contextAffiliate;
        private readonly IMapper _mapper;
        private readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
        private readonly SpreadSheetIdSetting _spreadSheetIdSetting;
        private readonly UrlShareServiceSetting _urlShareServiceSetting;
        private readonly string applicationName = "Scanning LocationShop";
        private readonly string sheetName = "Data";
        public LocationShopRepository(IRepositoryAsync<Payment> repository,
            AppPartnerDbContext context, IMapper mapper, AffiliateDbContext contextAffiliate,
            IOptions<SpreadSheetIdSetting> spreadSheetIdSetting, IOptions<UrlShareServiceSetting> urlShareServiceSetting)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
            _spreadSheetIdSetting = spreadSheetIdSetting.Value;
            _urlShareServiceSetting = urlShareServiceSetting.Value;
            _contextAffiliate = contextAffiliate;
        }
       
        public bool ReadGoogleSheetLocationShop()
       {
            try
            {
                var spreadSheetId = "";
                var service = GoogleSheetHelper.Authentication(scopes, applicationName);
                var range = $"{sheetName}!A:H";
                SpreadsheetsResource.ValuesResource.GetRequest requestSheet =
                    service.Spreadsheets.Values.Get(spreadSheetId, range);
                var responseSheet = requestSheet.Execute();

                var dataShareService = JsonConvert.DeserializeObject<JsonShareService>(GetDataShareService().Content).data;
                var status = "";
                var listData = responseSheet.Values.Skip(1).Where(x =>!string.IsNullOrEmpty(x[0].ToString()) && x[6].ToString() != "1" && x[4].ToString() == "Active").ToList();
                var listDataClosed = responseSheet.Values.Skip(1).Where(x => !string.IsNullOrEmpty(x[0].ToString()) && x[6].ToString() == "1" && x[4].ToString() == "Closed").ToList();
                var listDataComming = responseSheet.Values.Skip(1).Where(x => !string.IsNullOrEmpty(x[0].ToString()) && x[6].ToString() != "1" && DateTime.Now.AddDays(2).ToString("dd-MMM-yyyy") == x[5].ToString() && x[4].ToString().Equals("Coming soon")).ToList();
                if (listDataComming.Any())
                {
                    foreach (var item in listDataComming)
                    {
                        status = "";
                        var dataLocationShopQuery = new DataLocationShopQuery
                        {
                            Position = item[0].ToString(),
                            ShopName = item[1].ToString(),
                            County = GoogleSheetHelper.UpdateProvince(item[2].ToString()),
                            Province = GoogleSheetHelper.UpdateProvince(item[3].ToString()),
                            Status = item[4].ToString(),
                            OpenDate = item[5].ToString(),
                            IsRead = item[6].ToString(),
                        };
                        var position = int.Parse(dataLocationShopQuery.Position) + 1;                       
                        var existShareService = dataShareService.FirstOrDefault(x => x.Name.Equals(dataLocationShopQuery.ShopName));
                        if(existShareService == null)
                        {
                            status = "không tìm thấy cửa hàng";
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, status, spreadSheetId, sheetName);
                        }    
                        if (existShareService != null)
                        {                            
                            var regionId = _contextAffiliate.Regions.FirstOrDefault(x => dataLocationShopQuery.Province.Equals(x.ParentName) && dataLocationShopQuery.County.Equals(x.Name))?.RegionId;
                            if (regionId == null)
                            {
                                status = "không tìm thấy quận huyện";
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, status, spreadSheetId, sheetName);
                            }
                            if (regionId != null)
                            {
                                var locationShop = new LocationShop
                                {
                                    Province = dataLocationShopQuery.Province,
                                    Shop = dataLocationShopQuery.ShopName,
                                    Status = true,
                                    County = dataLocationShopQuery.County,
                                    RegionID = regionId.ToString(),
                                    GroupID = int.Parse(existShareService.GroupID),
                                };
                                AddLocationShop(locationShop, spreadSheetId, sheetName, position);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.H, position, DateTime.Now.ToString("dd-MMM-yyyy"), spreadSheetId, sheetName);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.G, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName);
                               
                            }
                        }                       
                    }
                }
                if (listDataClosed.Any())
                {
                    foreach (var item in listDataClosed)
                    {
                        var locationShop = _contextAffiliate.LocationShop.FirstOrDefault(x => x.Shop.Equals(item[1].ToString()));
                        if (locationShop != null)
                        {
                            locationShop.Status = false;
                            _contextAffiliate.LocationShop.Update(locationShop);
                            _contextAffiliate.SaveChanges();
                        }
                        var position = int.Parse(item[0].ToString()) + 1;
                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, "", spreadSheetId, sheetName); //update cột Đã quét                       
                    }
                }    
               if (listData.Any())
                {
                    foreach (var item in listData)
                    {
                        status = "";
                        var dataLocationShopQuery = new DataLocationShopQuery
                        {
                            Position = item[0].ToString(),
                            ShopName = item[1].ToString(),
                            County = GoogleSheetHelper.UpdateProvince(item[2].ToString()),
                            Province = GoogleSheetHelper.UpdateProvince(item[3].ToString()),
                            Status = item[4].ToString(),
                            OpenDate = item[5].ToString(),
                            IsRead = item[6].ToString(),                           
                        };
                        var position = int.Parse(dataLocationShopQuery.Position) + 1;
                        var existShareService = dataShareService.FirstOrDefault(x => x.Name.Equals(dataLocationShopQuery.ShopName));
                        if (existShareService == null)
                        {
                            status = "không tìm thấy cửa hàng";
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, status, spreadSheetId, sheetName);
                        }
                        if (existShareService != null)
                        {
                            var regionId = _contextAffiliate.Regions.FirstOrDefault(x => dataLocationShopQuery.Province.Equals(x.ParentName) && dataLocationShopQuery.County.Equals(x.Name))?.RegionId;
                            if (regionId == null)
                            {
                                status = "không tìm thấy quận huyện";
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, status, spreadSheetId, sheetName);
                            }
                            if (regionId != null)
                            {
                                var locationShop = new LocationShop
                                {
                                    Province = dataLocationShopQuery.Province,
                                    Shop = dataLocationShopQuery.ShopName,
                                    Status = true,
                                    County = dataLocationShopQuery.County,
                                    RegionID = regionId.ToString(),
                                    GroupID = int.Parse(existShareService.GroupID),
                                };
                                AddLocationShop(locationShop, spreadSheetId, sheetName, position);

                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.H, position, DateTime.Now.ToString("dd-MMM-yyyy"), spreadSheetId, sheetName);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.G, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét                                
                            }
                        }
                       
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }       
        public IRestResponse GetDataShareService()
        {
            var urlShareService = _urlShareServiceSetting.UrlShareService;
            var client = new RestClient(urlShareService);
            var req = new RestRequest("",Method.GET);
            var rs = client.Execute(req);
            return rs;
        }

        public void AddLocationShop(LocationShop locationShop,string spreadSheetId, string sheetName,int position)
        {
            var dataExist = _contextAffiliate.LocationShop.FirstOrDefault(x => x.Shop.Trim().Equals(locationShop.Shop.Trim()) && x.RegionID.Equals(locationShop.RegionID));
            if(dataExist == null)
            {
                _contextAffiliate.LocationShop.Add(locationShop);
                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, "Thành công", spreadSheetId, sheetName);
            }
            else
            {
                dataExist.Status = locationShop.Status;
                _contextAffiliate.LocationShop.Update(dataExist);
                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, "Đã tồn tại và cập nhật mới", spreadSheetId, sheetName);
            }
            _contextAffiliate.SaveChanges();
        }
    }
}
