using F88.Digital.API.Controllers.Affiliate;
using F88.Digital.Application.Features.AppPartner.Payment.Command.Create;
using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Application.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    public class PaymentController : BaseApiController<PaymentController>
    {
        // POST api/<controller>
        [HttpPost("CreatePayment")]
        public async Task<IActionResult> Post(CreatePaymentCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("CreatePayment: Request: {0}", command));

            return Ok(await _mediator.Send(command));
        }

        [HttpGet("GetPaymentHistoryByCurrentMonth")]
        public async Task<IActionResult> GetByCurrentMonth(int userProfileId)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetPaymentHistoryByCurrentMonth: Request: {0}", userProfileId));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetPaymentDetailsCurrentMonthQuery { UserProfileId = userProfileId }));
        }

        [HttpGet("GetPaymentHistoryByDate")]
        public async Task<IActionResult> GetPaymentHistoryByDate(int userProfileId, DateTime fromDate, DateTime toDate)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetPaymentHistoryByDate: Request: {0} - {1} - {2}", userProfileId, fromDate, toDate));

            if (userProfileId == 0)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(new GetPaymentDetailsByDateQuery { userProfileId = userProfileId, FromDate = fromDate, ToDate = toDate }));
        }
    }
}
