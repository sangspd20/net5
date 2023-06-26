using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    [Authorize]
    [ApiController]
    [Route("api/app-partner/v{version:apiVersion}/[controller]")]
    public class PaymentTransferController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPaymentUserLoanReferralRepository _paymentUserLoanReferralRepository;
        public PaymentTransferController(ILogger<UploadController> logger, IHostingEnvironment env,
            IPaymentRepository paymentRepository, IUserProfileRepository userProfileRepository,
            IPaymentUserLoanReferralRepository paymentUserLoanReferralRepository)
        {
            _env = env;
            _logger = logger;
            _paymentRepository = paymentRepository;
            _userProfileRepository = userProfileRepository;
            _paymentUserLoanReferralRepository = paymentUserLoanReferralRepository;
        }
        [HttpGet("PaymentHistory")]
        public async Task<IActionResult> PaymentHistory(int userProfileId)
        {
            try
            {

                var user = await _userProfileRepository.GetByUserIdAsync(userProfileId);
                if(user != null)
                {
                   var rs = _paymentRepository.GetPaymentHistory(user.UserPhone);
                    return Ok(new
                    {
                        succeeded = true,
                        data = rs,
                        message = "Thành công.",

                    });
                }
                return Ok(new
                {

                    succeeded = false,
                    message = "Không tìm thấy user.",

                });

            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
        [HttpGet("PaymentHistoryDetail")]
        public async Task<IActionResult> PaymentHistoryDetail(int userProfileId, int paymentId)
        {
            try
            {

                var user = await _userProfileRepository.GetByUserIdAsync(userProfileId);
                if (user != null)
                {
                    if(paymentId == 0) return Ok(new
                    {
                        result = false,
                        message = "Mã thanh toán không hợp lệ.",
                    });
                    var rs = _paymentUserLoanReferralRepository.GetPaymentHistoryDetail(user.UserPhone, paymentId);
                    return Ok(new
                    {

                        succeeded = true,
                        data = rs,
                        message = "Thành công.",

                    });
                }
                return Ok(new
                {

                    succeeded = false,
                    message = "Không tìm thấy user.",

                });

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
