using F88.Digital.API.Controllers.AppPartner;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command.Update;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries.FilterByLoanStatus;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries.SearchByPhone;
using F88.Digital.Application.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.Results;
using MediatR;
using Newtonsoft.Json;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    public class UserLoanReferralController : BaseApiController<UserLoanReferralController>
    {
        // POST api/<controller>
        [HttpPost("CreateReferral")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateUserLoanRefRequest createUserLoanRefRequest)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("CreateReferral: Request: {0}", createUserLoanRefRequest));

            CreateUserLoanRefCommand command = new CreateUserLoanRefCommand
            {
                UserPhone = createUserLoanRefRequest.UserPhone,
                UserProfileId = createUserLoanRefRequest.UserProfileId,
                PhoneNumber = createUserLoanRefRequest.PhoneNumber,
                FullName = createUserLoanRefRequest.FullName,
                Province = createUserLoanRefRequest.Province,
                District = createUserLoanRefRequest.District,
                RefTempGroupId = createUserLoanRefRequest.RefTempGroupId,
                RefRealGroupId = createUserLoanRefRequest.RefRealGroupId,
                RefContractGroupId = createUserLoanRefRequest.RefContractGroupId,
                RefAsset = createUserLoanRefRequest.RefAsset,

                Deposit = new CreateDepositRequest
                {
                    UserProfileId = createUserLoanRefRequest.UserProfileId,
                    Status = true
                }
            };

            return Ok(await _mediator.Send(command));
        }

        //PUT api/<controller>
        [HttpPut("UpdateUserBalance")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(UpdateUserLoanStatusRequest request)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("UpdateUserBalance: Request: {0}", request));

            if (request.TransactionId == null)
            {
                return BadRequest();
            }

            UpdateUserLoanRefCommand command = new UpdateUserLoanRefCommand()
            {
                TransactionId = request.TransactionId,
                LoanStatus = request.LoanStatus,
                RefContractGroupId = request.LoanStatus == 2 ? request.RefContractGroupId : 0,
                RefFinalGroupId = request.RefContractGroupId,
                LoanAmount = request.LoanAmount,
                AssetType = request.AssetType,
                PawnId = request.PawnId,
                UpdateBalance = new UpdateDepositRequest()
                {
                    Notes = request.Notes
                }
            };

            return Ok(await this._mediator.Send(command));
        }

        // GET api/<controller>
        [HttpGet("GetListUserLoanByUser")]
        public async Task<IActionResult> GetByCurrentMonth(int userProfileId, int pageNumber)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetListUserLoanByUser: Request: {0} - {1}", userProfileId, pageNumber));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetListUserLoanRefByCurrentMonthQuery { userProfileId = userProfileId, PageNumber = pageNumber }));
        }

        // GET api/<controller>
        [HttpGet("FilterUserLoanByDate")]
        public async Task<IActionResult> FilterByDate(int userProfileId, DateTime fromDate, DateTime toDate, int pageNumber)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("FilterUserLoanByDate: Request: {0} - {1} - {2} - {3}", userProfileId, fromDate, toDate, pageNumber));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new FilterUserLoanRefByDateQuery { UserProfileId = userProfileId, FromDate = fromDate, ToDate = toDate, PageNumber = pageNumber }));
        }

        // GET api/<controller>
        [HttpGet("FilterUserLoanByLoanStatusAndDate")]
        public async Task<IActionResult> FilterUserLoanByLoanStatusAndDate(int userProfileId, int loanStatus, DateTime fromDate, DateTime toDate, string phoneNumber)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("FilterUserLoanByLoanStatusAndDate: Request: {0} - {1} - {2} - {3} - {4}", userProfileId, loanStatus, fromDate, toDate, phoneNumber));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new FilterByLoanStatusDateQuery { UserProfileId = userProfileId, LoanStatus = loanStatus, FromDate = fromDate, ToDate = toDate, PhoneNumber = phoneNumber }));
        }

        [AllowAnonymous]
        [HttpPost("ReferralCreatePawnonline")]
        public async Task<IActionResult> ReferralCreatePawnonline(CreateUserLoanRefRequest createUserLoanRefRequest)
        {
            F88LogManage.F88PartnerLog.Info((object)string.Format("ReferralCreatePawnonline: Request: {0}", (object)JsonConvert.SerializeObject((object)createUserLoanRefRequest)));
            CreateUserLoanRefCommand userLoanRefCommand = new CreateUserLoanRefCommand();
            userLoanRefCommand.UserPhone = createUserLoanRefRequest.UserPhone;
            userLoanRefCommand.UserProfileId = createUserLoanRefRequest.UserProfileId;
            userLoanRefCommand.PhoneNumber = createUserLoanRefRequest.PhoneNumber;
            userLoanRefCommand.FullName = createUserLoanRefRequest.FullName;
            userLoanRefCommand.Province = createUserLoanRefRequest.Province;
            userLoanRefCommand.District = createUserLoanRefRequest.District;
            userLoanRefCommand.RefTempGroupId = createUserLoanRefRequest.RefTempGroupId;
            userLoanRefCommand.RefRealGroupId = createUserLoanRefRequest.RefRealGroupId;
            userLoanRefCommand.RefContractGroupId = createUserLoanRefRequest.RefContractGroupId;
            userLoanRefCommand.RefAsset = createUserLoanRefRequest.RefAsset;
            userLoanRefCommand.RegionID = createUserLoanRefRequest.RegionID;
            userLoanRefCommand.Deposit = new CreateDepositRequest()
            {
                UserProfileId = createUserLoanRefRequest.UserProfileId,
                Status = true
            };
            CreateUserLoanRefCommand command = userLoanRefCommand;
            return Ok(await this._mediator.Send<Result<int>>((IRequest<Result<int>>)command));
            // old code
            //Result<int> result = await this._mediator.Send<Result<int>>((IRequest<Result<int>>)command);
            //IActionResult actionResult = (IActionResult)this.Ok((object)result);
            //command = (CreateUserLoanRefCommand)null;
            //return actionResult;
        }
    }
}
