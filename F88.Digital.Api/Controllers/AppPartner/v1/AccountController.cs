using AspNetCoreHero.Results;
using F88.Digital.Application.Constants;
using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using F88.Digital.Application.DTOs.Identity;
using F88.Digital.Application.Interfaces;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Application.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    [Route("api/app-partner/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityService _identityService;

        public AccountController(IAccountService accountService, IIdentityService identityService)
        {
            _accountService = accountService;
            _identityService = identityService;
        }

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTPAsync(UserOTPRequest userOTPRequest)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("SendOTP: Request: {0}", userOTPRequest));

            //var otp = await _accountService.SendOTPAsync(userOTPRequest);
            var otp = await _accountService.SendOtpShareService(userOTPRequest);
            return Ok(otp);
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTPAsync(UserOTPRequest userOTPRequest)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("VerifyOTP: Request: {0}", userOTPRequest));

            //var otp = await _accountService.VerifyOTPAsync(userOTPRequest.UserPhone, userOTPRequest.OTP, userOTPRequest.DeviceId);
            var otp = await _accountService.CheckOtpShareService(userOTPRequest.UserPhone, userOTPRequest.OTP, userOTPRequest.DeviceId);
            if (otp.Data == null) return Ok(otp);
            //Get Access Token
            if (otp.Data.Ok)
            {
                var accRequest = TokenRequest();

                var token = await _identityService.GetTokenAsync(accRequest, GenerateIPAddress());

                otp.Data.AccessToken = token.Data.JWToken;
            }

            return Ok(otp);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginRequest userLoginRequest)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("Login: Request: {0}", userLoginRequest));

            var result = await _accountService.LoginAsync(userLoginRequest);

            if (result.Data != null)
            {
                var accRequest = TokenRequest();

                var token = await _identityService.GetTokenAsync(accRequest, GenerateIPAddress());

                result.Data.AccessToken = token.Data.JWToken;
                result.Message = "Đăng nhập thành công";
            }

            return Ok(result);
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private TokenRequest TokenRequest()
        {
            var accRequest = new TokenRequest
            {
                Email = Permissions.AppPartner.EMAIL_REQUEST_TOKEN,
                Password = Permissions.AppPartner.PASSWORD_REQUEST_TOKEN
            };

            return accRequest;
        }

        // GET api/<controller>
        [HttpGet("GetUserContract")]
        public async Task<ContentResult> GetUserContract(string userPhone)
        {
            var res = await _accountService.GetContractInfo(userPhone);

            if (res.Data != null)
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = res.Data.ContractDetail
                };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                Content = string.Empty
            };
        }

        [HttpPost("ReferralVerifyOTP")]
        public async Task<IActionResult> ReferralVerifyOTP(
                            ReferralUserOtp userOTPRequest)
        {
            F88LogManage.F88PartnerLog.Info((object)string.Format("VerifyOTP: Request: {0}", (object)userOTPRequest));
            var otp = await _accountService.CheckOtpShareService(userOTPRequest.UserPhone, userOTPRequest.OTP, userOTPRequest.DeviceId);            
            if (otp.Data == null)
                return (IActionResult)this.Ok((object)otp);
            if (otp.Data.Ok)
            {
                await this._accountService.VerifyOTPReferralAsync(userOTPRequest.UserPhone, userOTPRequest.OTP, userOTPRequest.DeviceId, userOTPRequest.LastName, userOTPRequest.FirstName);
                F88.Digital.Application.DTOs.Identity.TokenRequest accRequest = this.TokenRequest();
                Result<TokenResponse> token = await this._identityService.GetTokenAsync(accRequest, this.GenerateIPAddress());
                otp.Data.AccessToken = token.Data.JWToken;
                accRequest = (F88.Digital.Application.DTOs.Identity.TokenRequest)null;
                token = (Result<TokenResponse>)null;
            }
            return (IActionResult)this.Ok((object)otp);
        }
    }
}
