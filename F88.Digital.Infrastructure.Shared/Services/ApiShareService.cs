using AspNetCoreHero.Results;
using F88.Digital.Application.CacheKeys.ShareServiceKey;
using F88.Digital.Application.Constants;
using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.DTOs.ShareService;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Interfaces.Shared;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.Extensions.Caching;
using System.Threading.Tasks;
using Newtonsoft.Json;
using F88.Digital.Domain.Entities.Share;
using F88.Digital.Application.Interfaces.Repositories.Share;
using AutoMapper;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using RestSharp;

namespace F88.Digital.Infrastructure.Shared.Services
{
    public class ApiShareService : IApiShareService
    {
        private ApiShareServiceSettings _apiShareServiceSettings { get; }
        private readonly IDistributedCache _distributedCache;
        private readonly IShareRepositoryAsync<LocationShop> _locationRepository;
        private readonly IShareRepositoryAsync<Region> _regionRepository;
        private readonly IMapper _mapper;
        private readonly AffiliateSettings _affiliateSettings;
        private Task<List<LocationShop>> _lstLocationShop { get; }
        private Task<List<Region>> _lstRegion { get; }
        public ApiShareService(IOptions<ApiShareServiceSettings> apiShareServiceSettings,
            IDistributedCache distributedCache,
            IShareRepositoryAsync<LocationShop> locationRepository,
            IShareRepositoryAsync<Region> regionRepository,
            IMapper mapper, IOptions<AffiliateSettings> affiliateSettings)
        {
            _apiShareServiceSettings = apiShareServiceSettings.Value;
            _distributedCache = distributedCache;
            _locationRepository = locationRepository;
            _affiliateSettings = affiliateSettings.Value;
            _mapper = mapper;
            _lstLocationShop = locationRepository.GetAllAffiliateAsync();
            _regionRepository = regionRepository;
            _lstRegion = regionRepository.GetAllAffiliateAsync();
        }

        /// <summary>
        /// POS API
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<GetRegionLevelResponse>>> GetRegionLevel()
        {
            string cacheKey = ShareServiceCacheKeys.GetShareServiceLocationKey(CacheKeysContants.GET_REGION_FIRST_LEVEL);
            var lstCurrentRegions = await _distributedCache.GetAsync<List<GetRegionLevelResponse>>(cacheKey);

            if (lstCurrentRegions == null)
            {
                string accessToken = GetAccessTokenShareService();
                string getRegionApiUrl = _apiShareServiceSettings.GetRegionLevelApiUrl;

                lstCurrentRegions = RestApiPerform.RestApiGetWithBearerToken<List<GetRegionLevelResponse>>(getRegionApiUrl, accessToken);

                await _distributedCache.SetAsync(cacheKey, lstCurrentRegions, cacheExpirationInMinutes: 60);
            }

            return await Result<List<GetRegionLevelResponse>>.SuccessAsync(lstCurrentRegions);
        }

        /// <summary>
        /// POS API
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<GetDistrictByRegionResponse>>> GetDistrict()
        {
            string cacheKey = ShareServiceCacheKeys.GetShareServiceLocationKey(CacheKeysContants.GET_DISTRICT_BY_REGION);
            var lstCurrentDistrict = await _distributedCache.GetAsync<List<GetDistrictByRegionResponse>>(cacheKey);

            if (lstCurrentDistrict == null)
            {
                string accessToken = GetAccessTokenShareService();
                string getDistrictApiUrl = _apiShareServiceSettings.GetDistrictApiUrl;
                lstCurrentDistrict = RestApiPerform.RestApiGetWithBearerToken<List<GetDistrictByRegionResponse>>(getDistrictApiUrl, accessToken);

                await _distributedCache.SetAsync(cacheKey, lstCurrentDistrict, cacheExpirationInMinutes: 60);
            }

            return await Result<List<GetDistrictByRegionResponse>>.SuccessAsync(lstCurrentDistrict);
        }

        /// <summary>
        /// POS API
        /// </summary>
        /// <returns></returns>
        private string GetAccessTokenShareService()
        {
            #region Get Token
            object objLoginParam = new
            {
                UserName = _apiShareServiceSettings.UserName,
                PassWord = _apiShareServiceSettings.Password
            };

            string apiLoginUrl = _apiShareServiceSettings.LoginApiUrl;

            var loginResult = RestApiPerform.RestApiPost(apiLoginUrl, objLoginParam);
            var loginResultModel = loginResult != null ? JsonConvert.DeserializeObject<ShareServiceTokenResponse>(JsonConvert.SerializeObject(loginResult)) : new ShareServiceTokenResponse();

            return loginResultModel.Data;
            #endregion
        }

        /// <summary>
        /// POS API
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<GetShopResponse>>> GetShop()
        {
            string cacheKey = ShareServiceCacheKeys.GetShareServiceLocationKey(CacheKeysContants.GET_SHOP_ALL);
            var lstCurrentShop = await _distributedCache.GetAsync<List<GetShopResponse>>(cacheKey);

            if (lstCurrentShop == null)
            {
                string accessToken = GetAccessTokenShareService();
                string getShopApiUrl = _apiShareServiceSettings.GetAllShopApiUrl;
                lstCurrentShop = RestApiPerform.RestApiGetWithBearerToken<List<GetShopResponse>>(getShopApiUrl, accessToken);

                await _distributedCache.SetAsync(cacheKey, lstCurrentShop, cacheExpirationInMinutes: 60);
            }

            return await Result<List<GetShopResponse>>.SuccessAsync(lstCurrentShop);
        }

        public async Task<Result<CheckDupPhoneResponse>> CheckDupPhone(string phoneNumber)
        {
            #region Check phone number
            object objParam = new
            {
                phone_number = phoneNumber,
                day = _apiShareServiceSettings.POLExistDay
            };

            string POLCheckPhoneApiUrl = _apiShareServiceSettings.POLCheckDupPhoneApiUrl;

            var loginResult = RestApiPerform.RestApiPost(POLCheckPhoneApiUrl, objParam);
            var loginResultModel = loginResult != null ? JsonConvert.DeserializeObject<CheckDupPhoneResponse>(JsonConvert.SerializeObject(loginResult)) : new CheckDupPhoneResponse();

            return await Result<CheckDupPhoneResponse>.SuccessAsync(loginResultModel);
            #endregion
        }

        public async Task<Result<List<SelectListItemResponse>>> GetProvinceContainsShop()
        {
            var lstLocationShop = _lstLocationShop.GetAwaiter().GetResult();
            var regions = _lstRegion.GetAwaiter().GetResult();

            List<int> regionIDs = lstLocationShop.Where(x => !string.IsNullOrEmpty(x.RegionID)).Select(x => x.RegionID.Trim()).ToList().ConvertAll(int.Parse);

            List<int> parentIDRegions = regions
                .Where(x => x.ParentId.HasValue && x.RegionLevel > 1 && regionIDs.Any(s => x.RegionId == s))
                .GroupBy(x => x.ParentId.Value)
                .Select(grp => grp.Key)
                .ToList();

            var provinceContainShops = regions.Where(x => x.Status == 1 && x.RegionLevel == 1 && parentIDRegions.Any(pr => pr == x.RegionId)
             ).Select(x => new SelectListItemResponse
             {
                 Value = x.RegionId.ToString(),
                 Text = x.Name
             }).ToList();

            return await Result<List<SelectListItemResponse>>.SuccessAsync(provinceContainShops);
        }


        public async Task<Result<List<SelectListItemResponse>>> GetDistrictsContainsShop(string province)
        {
            var lstLocationShop = _lstLocationShop.GetAwaiter().GetResult();
            var regions = _lstRegion.GetAwaiter().GetResult();

            List<int> regionIDs = lstLocationShop.Where(x => !string.IsNullOrEmpty(x.RegionID)).Select(x => x.RegionID.Trim()).ToList().ConvertAll(int.Parse);

            List<int> parentIDRegions = regions
                .Where(x => x.ParentId.HasValue && x.RegionLevel > 1 && regionIDs.Any(s => x.RegionId == s))
                .GroupBy(x => x.ParentId.Value)
                .Select(grp => grp.Key)
                .ToList();

            var provinceContainShops = regions.Where(x => !string.IsNullOrEmpty(x.ParentName) && x.ParentName.Trim().ToLower() == province.Trim().ToLower() && x.Status == 1
            && ((x.RegionLevel > 1 && regionIDs.Any(s => x.RegionId == s)) || (x.RegionLevel == 1 && parentIDRegions.Any(pr => pr == x.RegionId)))
             ).Select(x => new SelectListItemResponse
             {
                 Value = x.RegionId.ToString(),
                 Text = x.Name
             }).ToList();

            return await Result<List<SelectListItemResponse>>.SuccessAsync(provinceContainShops);
        }

        public async Task<Result<List<SelectListItemResponse>>> GetShopsByDisctrict(string regionID)
        {
            var lstLocationShop = await _lstLocationShop;

            var lstProvinceContainsShop = lstLocationShop.Where(x => x.RegionID?.Trim() == regionID?.Trim() && x.Status.Value)
                                                         .OrderBy(p => p.Shop)
                                                         .Select(x => new SelectListItemResponse
                                                         {
                                                             Value = x.GroupID.ToString(),
                                                             Text = x.Shop
                                                         })
                                                         .ToList();

            return await Result<List<SelectListItemResponse>>.SuccessAsync(lstProvinceContainsShop);
        }

        public string GetShopsById(int groupId)
        {
            var locationShopRepo = _lstLocationShop.GetAwaiter().GetResult();

            var groupInfo = locationShopRepo.Where(x => x.GroupID == groupId && x.Status.Value)
                                            .FirstOrDefault();

            if (groupInfo == null)
            {
                groupInfo = locationShopRepo.Where(x => x.GroupIdOld == groupId && x.Status.Value)
                                            .FirstOrDefault();
                if (groupInfo == null) return string.Format("{0}", groupId);
            }
            return groupInfo.Shop;
        }

        public List<SelectListItemResponse> GetShopsByRegionId(string regionId)
        {
            var lstLocationShop = _lstLocationShop.GetAwaiter().GetResult();

            var lstProvinceContainsShop = lstLocationShop.Where(x => x.RegionID?.Trim() == regionId?.Trim() && x.Status.Value)
                                                         .OrderBy(p => p.Shop)
                                                         .Select(x => new SelectListItemResponse
                                                         {
                                                             Value = x.GroupID.ToString(),
                                                             Text = x.Shop
                                                         })
                                                         .ToList();

            return lstProvinceContainsShop;
        }

        public List<SelectListItemResponse> GetShopsByProvince(string province)
        {
            var lstLocationShop = _lstLocationShop.GetAwaiter().GetResult();

            var lstProvinceContainsShop = lstLocationShop.Where(x => x.Province?.Trim() == province?.Trim() && x.Status.Value)
                                                         .OrderBy(p => p.Shop)
                                                         .Select(x => new SelectListItemResponse
                                                         {
                                                             Value = x.GroupID.ToString(),
                                                             Text = x.Shop
                                                         })
                                                         .ToList();

            return lstProvinceContainsShop;
        }

        public List<SelectListItemResponse> GetAllShop()
        {

            var lstLocationShop = _lstLocationShop.GetAwaiter().GetResult();

            var lstAllShop = lstLocationShop.Where(x => x.Status.Value)
                                                         .OrderBy(p => p.Shop)
                                                         .Select(x => new SelectListItemResponse
                                                         {
                                                             Value = x.GroupID.ToString(),
                                                             Text = x.Shop
                                                         })
                                                         .ToList();

            return lstAllShop;
        }

        public async Task<GetShopAffiliateResponsePartner> GetShopByRegionId(string regionId)
        {
            var client = new RestClient(_affiliateSettings.ShopApiUrl);
            var req = new RestRequest("", Method.GET);
            req.AddParameter("regionId", regionId);
            var rs = client.Execute(req);
            var jsonData = JsonConvert.DeserializeObject<GetShopAffiliateResponse>(rs.Content);
            GetShopAffiliateResponsePartner result = new GetShopAffiliateResponsePartner();
            result.Data = _mapper.Map<List<DataShopAffiliatePartner>>(jsonData.Data);
            return result;
        }
    }
}