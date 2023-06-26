using F88.Digital.API.Controllers;
using F88.Digital.API.Controllers.AppPartner;
using F88.Digital.Application.Features.AppPartner.Commands.Delete;
using F88.Digital.Application.Features.AppPartner.Contract.Query;
using F88.Digital.Application.Features.AppPartner.UserBank.Command.Create;
using F88.Digital.Application.Features.AppPartner.UserBank.Command.Update;
using F88.Digital.Application.Features.AppPartner.UserBank.Queries;
using F88.Digital.Application.Features.AppPartner.UserBank.Queries.GetListBanks;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries;
using F88.Digital.Application.Features.AppPartner.UserProfile.Command.Create;
using F88.Digital.Application.Features.AppPartner.UserProfile.Command.Update;
using F88.Digital.Application.Features.AppPartner.UserProfile.Commands.Create;
using F88.Digital.Application.Features.AppPartner.UserProfile.Commands.Update;
using F88.Digital.Application.Features.AppPartner.UserProfile.Queries;
using F88.Digital.Application.Logging;
using FluentValidation.Resources;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    public class UserProfileController : BaseApiController<UserProfileController>
    {
        // POST api/<controller>
        [HttpPost("SignUp")]
        public async Task<IActionResult> Post(SignUpRequestModel sigupRequest)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("SignUp: Request: {0}", JsonConvert.SerializeObject(sigupRequest)));

            CreateUserProfileCommand command = new CreateUserProfileCommand
            {
                UserPhone = sigupRequest.UserPhone,
                UserAuthToken = new UserAuthTokenRequestModel
                {
                    Password = sigupRequest.Password
                }
            }; 

            return Ok(await _mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("UpdateProfile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Put([FromForm] UpdateUserProfileCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("UpdateProfile: Request: {0}", JsonConvert.SerializeObject(command)));

            if (command.UserPhone == null)
            {
                return BadRequest();
            }

            return Ok(await this._mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> Put(ResetPasswordCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("ResetPassword: Request: {0}", JsonConvert.SerializeObject(command)));

            if (command.UserPhone == null)
            {
                return BadRequest();
            }

            return Ok(await this._mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> Put(ChangePasswordCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("ChangePassword: Request: {0}", JsonConvert.SerializeObject(command)));

            if (command.UserPhone == null)
            {
                return BadRequest();
            }

            return Ok(await this._mediator.Send(command));
        }

        // POST api/<controller>
        [HttpPost("RegisterBank")]
        public async Task<IActionResult> Post(CreateUserBankCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("RegisterBank: Request: {0}", JsonConvert.SerializeObject(command)));

            return Ok(await _mediator.Send(command));
        }

        // GET api/<controller>
        [HttpGet("GetListBanksByUser/{userProfileId}")]
        public async Task<IActionResult> Get(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetListBanksByUser: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetListUserBankQuery { userProfileId = userProfileId }));
        }

        // PUT api/<controller>/5
        [HttpPut("UpdateUserBankInfo")]
        public async Task<IActionResult> Put(UpdateUserBankCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("UpdateUserBankInfo: Request: {0}", command));

            if (command.UserPhone == null || command.UserBankId == 0)
            {
                return BadRequest();
            }

            return Ok(await this._mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("ActiveUserBankAccount")]
        public async Task<IActionResult> Put(ActiveUserBankCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("ActiveUserBankAccount: Request: {0}", command));

            if (command.UserPhone == null || command.UserBankId == 0)
            {
                return BadRequest();
            }

            return Ok(await this._mediator.Send(command));
        }
        
        //DELETE api/<controller>/5
        [HttpDelete("removeUserBank")]
        public async Task<IActionResult> Delete(DeleteUserBankCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("removeUserBank: Request: {0}", command));

            if (command.Id == 0 || command.UserPhone == null)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(command));
        }

        // GET api/<controller>
        [HttpGet("GetBalanceByUser/{userProfileId}")]
        public async Task<IActionResult> GetBalance(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetBalanceByUser: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetBalanceByUserQuery { userProfileId = userProfileId }));
        }


        // GET api/<controller>
        [HttpGet("GetBalanceByUserInCurrentMonth/{userProfileId}")]
        public async Task<IActionResult> GetBalanceByUserInCurrentMonth(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetBalanceByUserInCurrentMonthQuery: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetBalanceByUserInCurrentMonthQuery { userProfileId = userProfileId }));
        }

        // GET api/<controller>
        [HttpGet("GetWaitBalanceByUser/{userProfileId}")]
        public async Task<IActionResult> GetWaitBalanceByUser(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetWaitBalanceByUser: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetWaitBalanceByUserQuery { userProfileId = userProfileId }));
        }

        // GET api/<controller>
        [HttpGet("GetFinalBalanceByUser/{userProfileId}")]
        public async Task<IActionResult> GetFinalBalanceByUser(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetFinalBalanceByUser: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetFinalBalanceByUserQuery { userProfileId = userProfileId }));
        }

        // GET api/<controller>
        [AllowAnonymous]
        [HttpGet("CheckDuplicateUserPhone/{userPhone}")]
        public async Task<IActionResult> CheckDuplicatePhone(string userPhone)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("CheckDuplicateUserPhone: Request: {0}", userPhone));

            if (string.IsNullOrEmpty(userPhone))
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new CheckUserPhoneQuery { UserPhone = userPhone }));
        }

        // GET api/<controller>
        [HttpGet("GetUserProfileDetail/{userProfileId}")]
        public async Task<IActionResult> GetUserProfile(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetUserProfileDetail: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetUserProfileDetailQuery { UserProfileId = userProfileId }));
        }

        // GET api/<controller>
        [HttpGet("GetListBank")]
        public async Task<IActionResult> GetListBank()
        {
            return Ok(await _mediator.Send(new GetListBanksQuery { }));
        }
        [HttpGet("GetAllUserProfile")]
        public async Task<IActionResult> GetAllUserProfile(string search, string fullName, int? groupId, string source, string fromDate, string toDate, int pageNumber, int pageSize)
        {
            return Ok(await _mediator.Send(new GetAllUserProfileQuery { Search = search, FullName = fullName, GroupId = groupId, Source = source, FromDate = fromDate, ToDate = toDate, PageNumber = pageNumber, PageSize = pageSize }));
        }
        [HttpGet("GetAllUserProfileByName")]
        public async Task<IActionResult> GetAllUserProfileByName(string fullName)
        {
            return Ok(await _mediator.Send(new GetAllUserProfileByNameQuery { FullName = fullName}));
        }
        [HttpPost("UpdateStatusUserProfile")]
        public async Task<IActionResult> UpdateStatusUserProfile(UpdateStatusUserProfileCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("UpdateStatusUserProfileCommand: Request: {0}", JsonConvert.SerializeObject(command)));

            return Ok(await _mediator.Send(command));
        }
    }
}
