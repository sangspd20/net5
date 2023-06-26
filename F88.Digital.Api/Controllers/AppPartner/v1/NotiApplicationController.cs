using F88.Digital.API.Controllers.AppPartner;
using F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create;
using F88.Digital.Application.Features.AppPartner.NotiApplication.Queries;
using F88.Digital.Application.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    public class NotiApplicationController : BaseApiController<UserProfileController>
    {
        // POST api/<controller>
        [HttpPost("CreateNotification")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] CreateNotiAppCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("CreateNotification: Request: {0}", JsonConvert.SerializeObject(command)));

            return Ok(await _mediator.Send(command));
        }

        // POST api/<controller>
        [HttpPost("CreateUserNotification")]
        public async Task<IActionResult> CreateUserNotification(CreateUserNotificationCommand command)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("CreateUserNotification: Request: {0}", JsonConvert.SerializeObject(command)));

            return Ok(await _mediator.Send(command));
        }

        // GET api/<controller>
        [HttpGet("GetListNotificationByUser")]
        public async Task<IActionResult> GetListNotificationByUser(int userProfileId, int PageNumber)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetListNotificationByUser: Request: {0} - {1}", userProfileId, PageNumber));

            if (userProfileId == 0 || PageNumber == 0)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new GetListNotificationsByUserQuery { userProfileId = userProfileId, PageNumber = PageNumber }));
        }

        // GET api/<controller>
        [HttpGet("GetListPublishNotification")]
        public async Task<IActionResult> GetListPublishNotification(int PageNumber)
        {
            F88LogManage.F88PartnerLog.Info(string.Format("GetListPublishNotification: Request: {0}", PageNumber));

            if (PageNumber == 0)
            {
                return BadRequest();
            }
            return Ok(await _mediator.Send(new GetListPublishNotificationsQuery { PageNumber = PageNumber }));
        }
    }
}
