using F88.Digital.API.Controllers.AppPartner;
using F88.Digital.Application.Features.AppPartner.Reporting.Queries;
using F88.Digital.Application.Features.AppPartner.Reporting.Queries.ReportingSummary;
using F88.Digital.Application.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    public class ReportController : BaseApiController<ReportController>
    {
        // GET api/<controller>
        [HttpGet("ReportingPaymentByMonth")]
        public async Task<IActionResult> ReportingPaymentByMonth(int month, int year, int PageNumber)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("ReportingPaymentByMonth: Request: {0} - {1} - {2}", month, year, PageNumber));

            if (PageNumber == 0)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new ReportingPaymentQuery { Month = month, Year = year, PageNumber = PageNumber }));
        }

        // GET api/<controller>
        [HttpGet("ReportingFormToSale")]
        public async Task<IActionResult> ReportingFormToSale(string province, string regionId, int day, int month, int year)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("ReportingFormToSale: Request: {0} - {1} - {2} - {3} - {4}", province, regionId, day, month, year));

            if (month == 0 || year == 0)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new ReportingSaleQuery { Province = province, RegionId = regionId, Day = day, Month = month, Year = year}));
        }

        // GET api/<controller>
        [HttpGet("ReportingSummary")]
        public async Task<IActionResult> ReportingSummary(int year, int month)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("ReportingSummary: Request: {0} - {1}", year, month));

            if (month == 0 || year == 0)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new ReportingSummaryQuery { Month = month, Year = year }));
        }
    }
}
