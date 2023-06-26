using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create;
using F88.Digital.Application.Features.AppPartner.NotiApplication.Queries;
using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Application.Features.AppPartner.UploadFile;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Application.Logging;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserBankRepository _userBankRepository;
        private readonly PhoneAccessSetting _phoneAccessSetting;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPaymentUserLoanReferralRepository _paymentUserLoanReferralRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private string folderName = $"UploadFiles/Excels/{DateTime.Now:dd-MM-yyyy}";
        public UploadController(ILogger<UploadController> logger, IHostingEnvironment env,
            IPaymentRepository paymentRepository, IUserBankRepository userBankRepository, IUserProfileRepository userProfileRepository,
            IPaymentUserLoanReferralRepository paymentUserLoanReferralRepository, UserManager<ApplicationUser> userManager, IOptions<PhoneAccessSetting> phoneAccessSetting)
        {
            _env = env;
            _logger = logger;
            _paymentRepository = paymentRepository;
            _userBankRepository = userBankRepository;
            _userProfileRepository = userProfileRepository;
            _paymentUserLoanReferralRepository = paymentUserLoanReferralRepository;
            _userManager = userManager;
            _phoneAccessSetting = phoneAccessSetting.Value;
        }

        [HttpPost("api/app-partner/v{version:apiVersion}/Upload/UploadExcel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadExcel([FromForm] UploadFileExcelCommand command)
        {
            string newPath = Path.Combine(_env.ContentRootPath, folderName);
            try
            {
                
               

                var checkAccessPhone = _phoneAccessSetting.Phone.Split(',').Contains(command.PhoneNumber);
                if(checkAccessPhone == false) return Ok(new
                {

                    result = false,
                    message = "Không có quyền truy cập.",

                });
                var data = ReadFileExcelCustomerPay(command.Files);
                foreach (var payInform in data.PayInform)
                {
                    var formatTransferDate = StringUtils.FormatDateTime(payInform.PayDate.Replace("-","/"), "dd/MMM/yyyy");
                    var statuspayInform = StringUtils.GetEnumValueFromDescription<StatusEnum>(payInform.Status.ToString()).GetHashCode();                    
                    if (!_paymentRepository.GetPaymentByPhoneTrans(payInform.PhoneNumber, formatTransferDate, statuspayInform).Any())
                    {
                        var payment = new Payment
                        {

                            PaidValue = !string.IsNullOrEmpty(payInform.MoneyPaid) ? decimal.Parse(payInform.MoneyPaid) : 0,
                            Notes = payInform.Note,
                            TransferDate = formatTransferDate,
                            UserBankId = _userBankRepository.GetByAccNumberAsync("18006388").Result.Id,
                            CreatedBy = command.PhoneNumber,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = command.PhoneNumber,
                            LastModifiedOn = DateTime.Now,
                            UserPhone = payInform.PhoneNumber,
                            OtherAmount = !string.IsNullOrEmpty(payInform.OtherAmount) ? decimal.Parse(payInform.OtherAmount) : 0,
                            TaxValue = !string.IsNullOrEmpty(payInform.Tax.Replace("%","")) ? decimal.Parse(payInform.Tax.Replace("%", "")) : 0,
                            Status = statuspayInform,
                            PaidMonth = payInform.PaidMonth,
                            PaidYear = payInform.PaidYear,
                        };

                        if(!string.IsNullOrEmpty(payInform.AccountNumber))
                        {
                            if(_userBankRepository.GetByAccNumberAsync(payInform.AccountNumber).Result != null)
                            {
                                payment.UserBankId = _userBankRepository.GetByAccNumberAsync(payInform.AccountNumber).Result.Id;
                            }                                
                        }    
                       
                        await _paymentRepository.InsertAsyncPayment(payment);
                    }                    
                }
                foreach (var paydetail in data.PayDetail)
                {
                    var statusPayDetail = StringUtils.GetEnumValueFromDescription<StatusEnum>(paydetail.Status.ToString()).GetHashCode();                    
                    if (_paymentUserLoanReferralRepository.PaymentUserLoanReferrals.FirstOrDefault(x=>x.UserLoanReferralId == Int32.Parse(paydetail.UserLoanReferralId) && x.Status == statusPayDetail) == null)
                    {
                        var payment = _paymentRepository.Payments.FirstOrDefault(x => x.UserPhone.Equals(paydetail.PhoneByUser) && x.Status == statusPayDetail && x.PaidMonth == paydetail.PaidMonth && x.PaidYear == paydetail.PaidYear);
                        if (payment != null)
                        {
                            var paymentUserLoanReferral = new PaymentUserLoanReferral
                            {
                                PaymentId = payment.Id,
                                TransactionId = paydetail.TransactionId,
                                UserLoanReferralId = Int32.Parse(paydetail.UserLoanReferralId),
                                CreatedBy = command.PhoneNumber,
                                CreatedOn = DateTime.Now,
                                LastModifiedBy = command.PhoneNumber,
                                LastModifiedOn = DateTime.Now,
                                Status = statusPayDetail
                            };
                            await _paymentUserLoanReferralRepository.InsertAsync(paymentUserLoanReferral);
                        }
                    }                        
                }
                return Ok(new
                {

                    succeeded = true,
                    message = "Upload File Thành Công.",

                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    succeeded = false,
                    message = "Thất bại"
                });
            }
        }
        public InformAppPartner ReadFileExcelCustomerPay(IFormFile file)
        {
            try
            {
                var mDataSale = new InformAppPartner();
                mDataSale.PayInform = ListPayInform(file);
                mDataSale.PayDetail = ListPayDetail(file);
                return mDataSale;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;

            }
        }
        public List<PayInform> ListPayInform(IFormFile file)
        {
            try
            {
                var listPayInform = new List<PayInform>();
                return UploadFileExcel.ReadFileExcel(file, 0, sheet =>
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;

                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        try
                        {
                            var item = new PayInform()
                            {
                                PhoneNumber = row.GetCell(1).ToString().Trim(),
                                IdCard = row.GetCell(2).ToString().Trim(),
                                CollaboratorName = row.GetCell(3).ToString().Trim(),
                                BankCode = row.GetCell(4).ToString().Trim(),
                                BankName = row.GetCell(5).ToString().Trim(),
                                BankBranch = row.GetCell(6).ToString().Trim(),
                                AccountNumber = row.GetCell(7).ToString().Trim(),
                                NetMoney = row.GetCell(8).ToString().Trim(),
                                CurrentMoney = row.GetCell(9).ToString().Trim(),
                                Tax = row.GetCell(10).ToString().Trim(),
                                MoneyPaid = row.GetCell(11).ToString().Trim(),
                                Content = row.GetCell(12).ToString().Trim(),
                                Note = row.GetCell(13).ToString().Trim(),
                                PaidMonth = row.GetCell(14).ToString().Trim(),
                                PaidYear = row.GetCell(15).ToString().Trim(),
                                PayDate = row.GetCell(16).ToString().Trim(),
                                OtherAmount = row.GetCell(17).ToString().Trim(),
                                AcceptPrivacy = row.GetCell(18).ToString().Trim(),
                                Status = row.GetCell(19).ToString().Trim()                              
                            };
                            listPayInform.Add(item);

                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.ToString());
                        }
                    }
                    return listPayInform.ToList();
                }, _env).Result as List<PayInform>;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;

            }
        }
        public List<PayDetail> ListPayDetail(IFormFile file)
        {
            try
            {
                var listPayDetail = new List<PayDetail>();
                return UploadFileExcel.ReadFileExcel(file, 1, sheet =>
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;

                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        try
                        {
                            var item = new PayDetail()
                            {
                                TransactionId = row.GetCell(0).ToString().Trim(),
                                UserLoanReferralId = row.GetCell(1).ToString().Trim(),
                                ReferralPhone = row.GetCell(2).ToString().Trim(),
                                IsF88Cus = row.GetCell(3).ToString().Trim(),
                                PhoneByUser = row.GetCell(4).ToString().Trim(),
                                TSContract = row.GetCell(5).ToString().Trim(),
                                LoanApplication = row.GetCell(6).ToString().Trim(),
                                FocusTransaction = row.GetCell(7).ToString().Trim(),
                                ContractCode = row.GetCell(8).ToString().Trim(),
                                SubSrouce = row.GetCell(9).ToString().Trim(),
                                TransferAppDate = row.GetCell(10).ToString().Trim(),
                                CreateContractDate = row.GetCell(11).ToString().Trim(),
                                CreateOnlineDate = row.GetCell(12).ToString().Trim(),
                                TransactionLastReceive = row.GetCell(13).ToString().Trim(),
                                TransactionCreate = row.GetCell(14).ToString().Trim(),
                                TransactionReceive = row.GetCell(15).ToString().Trim(),
                                PawnOnlineWid = row.GetCell(16).ToString().Trim(),
                                LoanMoney = row.GetCell(17).ToString().Trim(),
                                Status = row.GetCell(18).ToString().Trim(),
                                PaidMonth = row.GetCell(19).ToString().Trim(),
                                PaidYear = row.GetCell(20).ToString().Trim(),
                            };

                            listPayDetail.Add(item);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.ToString());
                        }
                    }
                    return listPayDetail.ToList();
                }, _env).Result as List<PayDetail>;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;

            }
        }

        [HttpGet("api/app-partner/v{version:apiVersion}/Upload/GetFolderExcel")]
        public IActionResult GetFolderExcel()
        {
            try
            {
                var webRootPath = _env.ContentRootPath + "\\UploadFiles\\Excels";
                var file = Directory.Exists(webRootPath);
                if(file == true)
                {
                    var folders = Directory.GetDirectories(Path.Combine(webRootPath));

                    var listData = new List<GetListFolderExcelPayment>();
                    foreach (var folder in folders)
                    {
                        var fileNames = Directory.GetFiles(folder);
                        foreach (var item in fileNames)
                        {
                            var model = new GetListFolderExcelPayment
                            {
                                FolderName = folder,
                                FileName = Path.GetFileName(item),
                                CreateDate = Path.GetFileName(folder),
                                FileSize = Path.GetFileName(item).Length
                            };
                            listData.Add(model);
                        };
                    };

                    return Ok(new
                    {
                        succeeded = true,
                        message = "Lấy danh sách file excel thành công.",
                        data = listData
                    });
                }
                return Ok(new
                {
                    succeeded = true,
                    message = "Lấy danh sách file excel thành công.",
                    data = ""
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;

            }
        }
    }
}
