using F88.Digital.Application.Interfaces.Shared;
using F88.Digital.Application.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.Share
{
    [Route("api/share")]
    [ApiController]
    [AllowAnonymous]
    public class ShareController : ControllerBase
    {
        private readonly IApiShareService _apiShareService;
        public ShareController(IApiShareService apiPosShareService)
        {
            _apiShareService = apiPosShareService;
        }

        // GET api/<controller>
        [HttpGet("pos/get-region")]
        public async Task<IActionResult> GetRegion()
        {
            var result = await _apiShareService.GetRegionLevel();
            return Ok(result);
        }

        // GET api/<controller>
        [HttpGet("pos/get-district")]
        public async Task<IActionResult> GetDistrict()
        {
            var result = await _apiShareService.GetDistrict();
            return Ok(result);
        }

        // GET api/<controller>
        [HttpGet("pos/get-shop")]
        public async Task<IActionResult> GetShop()
        {
            var result = await _apiShareService.GetShop();
            return Ok(result);
        }

        // GET api/<controller>
        [HttpGet("digital/v1/get-province-contains-shop")]
        public async Task<IActionResult> GetProvinceContainsShop()
        {
            F88LogManage.F88PartnerLog.Info(string.Format("get-province-contains-shop: Request: {0}", string.Empty));

            var result = await _apiShareService.GetProvinceContainsShop();
            return Ok(result);
        }

        // GET api/<controller>
        [HttpGet("digital/v1/get-district-contains-shop/{province}")]
        [AllowAnonymous]
        public async Task<IActionResult> GeDistrictContainsShop(string province)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("get-district-contains-shop: Request: {0}", province));

            var result = await _apiShareService.GetDistrictsContainsShop(province);
            return Ok(result);
        }

        // GET api/<controller>
        [HttpGet("digital/v1/get-shop-by-district/{regionId}")]
        public async Task<IActionResult> GetShopsByDistrict(string regionId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("get-shop-by-district: Request: {0}", regionId));

            var result = await _apiShareService.GetShopByRegionId(regionId);
            return Ok(result);
        }
    }
}
