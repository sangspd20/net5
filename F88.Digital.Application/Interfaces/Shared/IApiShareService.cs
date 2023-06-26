using AspNetCoreHero.Results;
using F88.Digital.Application.DTOs.ShareService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Shared
{
    public interface IApiShareService
    {
        Task<Result<List<GetRegionLevelResponse>>> GetRegionLevel();

        Task<Result<List<GetDistrictByRegionResponse>>> GetDistrict();

        Task<Result<List<GetShopResponse>>> GetShop();

        Task<Result<CheckDupPhoneResponse>> CheckDupPhone(string phoneNumber);

        Task<Result<List<SelectListItemResponse>>> GetProvinceContainsShop();

        Task<Result<List<SelectListItemResponse>>> GetDistrictsContainsShop(string province);

        Task<Result<List<SelectListItemResponse>>> GetShopsByDisctrict(string regionID);
        List<SelectListItemResponse> GetAllShop();
        Task<GetShopAffiliateResponsePartner> GetShopByRegionId(string regionId);

        #region --Private function--
        string GetShopsById(int groupId);

        List<SelectListItemResponse> GetShopsByRegionId(string regionId);

        List<SelectListItemResponse> GetShopsByProvince(string province);
        #endregion

    }
}
