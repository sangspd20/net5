
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
using static F88.Digital.Application.Constants.ApiConstants;

namespace F88.Digital.WebApi.Controllers.AppPartner.v1
{
    [ApiController]
    [Route("api/app-partner/v{version:apiVersion}/[controller]")]
    public class ExportExcelController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        private readonly IPaymentRepository _paymentRepository;
        public ExportExcelController(ILogger<UploadController> logger, IHostingEnvironment env,
            IPaymentRepository paymentRepository)
        {
            _env = env;
            _logger = logger;
            _paymentRepository = paymentRepository;          
        }
        [HttpGet("ExportPayment")]
        public async Task<IActionResult> ExportPayment(string createdOn)
        {
            try
            {
                if(string.IsNullOrEmpty(createdOn)) return Ok(new
                {

                    result = false,
                    message = "Vui lòng nhập thời gian.",

                });

                string sWebRootFolder = $"{_env.ContentRootPath}/Export/Excels/";
                string sFileName = $@"ThongTinThanhToan{DateTime.Now.ToString("ddMMyyyymmss")}.xlsx";
                var fullPath = Path.Combine(sWebRootFolder, sFileName);
                FileInfo file = new FileInfo(fullPath);
                var filePath = Path.Combine(
                               Directory.GetCurrentDirectory(), "Upload",
                               sWebRootFolder);

                bool folderExists = Directory.Exists(filePath);
                if (!folderExists)
                    Directory.CreateDirectory(filePath);
                var memory = new MemoryStream();
                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet sheetInform = workbook.CreateSheet("Thông tin thanh toán");
                    IRow rowHeaderInform = sheetInform.CreateRow(0);
                    rowHeaderInform.CreateCell(0).SetCellValue("STT");
                    rowHeaderInform.CreateCell(1).SetCellValue("Số điện thoại");
                    rowHeaderInform.CreateCell(2).SetCellValue("CMND");
                    rowHeaderInform.CreateCell(3).SetCellValue("Tên đơn vị thụ hưởng");
                    rowHeaderInform.CreateCell(4).SetCellValue("Mã ngân hàng");
                    rowHeaderInform.CreateCell(5).SetCellValue("Tên ngân hàng");
                    rowHeaderInform.CreateCell(6).SetCellValue("Chi nhánh ngân hàng");                    
                    rowHeaderInform.CreateCell(7).SetCellValue("Số tài khoản nhận");
                    rowHeaderInform.CreateCell(8).SetCellValue("Net Money");
                    rowHeaderInform.CreateCell(9).SetCellValue("Current Money");
                    rowHeaderInform.CreateCell(10).SetCellValue("Tax");
                    rowHeaderInform.CreateCell(11).SetCellValue("Money Paid");
                    rowHeaderInform.CreateCell(12).SetCellValue("Nội dung");
                    rowHeaderInform.CreateCell(13).SetCellValue("Ghi chú");
                    rowHeaderInform.CreateCell(14).SetCellValue("PaidMonth");
                    rowHeaderInform.CreateCell(15).SetCellValue("PaidYear");
                    rowHeaderInform.CreateCell(16).SetCellValue("Ngày thanh toán");
                    rowHeaderInform.CreateCell(17).SetCellValue("Phụ thu khác");
                    rowHeaderInform.CreateCell(18).SetCellValue("Đồng ý điều khoản HĐ");
                    rowHeaderInform.CreateCell(19).SetCellValue("Trạng thái thanh toán");
                    rowHeaderInform.CreateCell(20).SetCellValue("Nguồn");
                    var paymentInform = _paymentRepository.GetPaymentInform(createdOn);
                    for (int i = 0; i < paymentInform.Count; i++)
                    {
                        IRow rowInform = sheetInform.CreateRow(i + 1);
                        rowInform.CreateCell(0).SetCellValue(i + 1);
                        rowInform.CreateCell(1).SetCellValue(paymentInform[i].PhoneNumber);
                        rowInform.CreateCell(2).SetCellValue(paymentInform[i].Passport);
                        rowInform.CreateCell(3).SetCellValue(paymentInform[i].AccountName);
                        rowInform.CreateCell(4).SetCellValue(paymentInform[i].BankCode);
                        rowInform.CreateCell(5).SetCellValue(paymentInform[i].BankName);
                        rowInform.CreateCell(6).SetCellValue(paymentInform[i].BankBranch);
                        rowInform.CreateCell(7).SetCellValue(paymentInform[i].AccountNumber);
                        rowInform.CreateCell(8).SetCellValue($"{(decimal.ToDouble((decimal)paymentInform[i].NetMoney) != 0 ? decimal.ToDouble((decimal)paymentInform[i].NetMoney) : "")}");
                        rowInform.CreateCell(9).SetCellValue($"{(decimal.ToDouble((decimal)paymentInform[i].CurrentMoney) != 0 ? decimal.ToDouble((decimal)paymentInform[i].CurrentMoney) : "")}");
                        rowInform.CreateCell(10).SetCellValue($"{paymentInform[i].Tax}%");
                        rowInform.CreateCell(11).SetCellValue(paymentInform[i].MoneyPaid);
                        rowInform.CreateCell(12).SetCellValue(paymentInform[i].Content);
                        rowInform.CreateCell(13).SetCellValue(paymentInform[i].Note);
                        rowInform.CreateCell(14).SetCellValue(paymentInform[i].PaidMonth);
                        rowInform.CreateCell(15).SetCellValue(paymentInform[i].PaidYear);
                        if (paymentInform[i].TransferDate != null)
                        {
                            rowInform.CreateCell(16).SetCellValue(paymentInform[i].TransferDate.Value.ToString("dd/MM/yyyy HH:mm"));
                        }
                        else
                        {
                            rowInform.CreateCell(16).SetCellValue("");
                        }                        
                        rowInform.CreateCell(17).SetCellValue(paymentInform[i].OtherAmount);
                        rowInform.CreateCell(18).SetCellValue(paymentInform[i].IsAgreementConfirmed == true ? "1" : "0");
                        if(paymentInform[i].Status == 0)
                        {
                            rowInform.CreateCell(19).SetCellValue("Pending");
                        }  
                        else if (paymentInform[i].Status == 1)
                        {
                            rowInform.CreateCell(19).SetCellValue("Success");
                        }
                        else
                        {
                            rowInform.CreateCell(19).SetCellValue("Failed");
                        }

                        rowInform.CreateCell(20).SetCellValue(paymentInform[i].Source);

                    }

                    ISheet sheetDetail = workbook.CreateSheet("Chi tiết thanh toán");
                    IRow rowHeaderDetail = sheetDetail.CreateRow(0);
                    rowHeaderDetail.CreateCell(0).SetCellValue("TransactionId");
                    rowHeaderDetail.CreateCell(1).SetCellValue("UserLoanRefId");
                    rowHeaderDetail.CreateCell(2).SetCellValue("ReferralPhone");
                    rowHeaderDetail.CreateCell(3).SetCellValue("IsF88Cus");
                    rowHeaderDetail.CreateCell(4).SetCellValue("UserPartner");
                    rowHeaderDetail.CreateCell(5).SetCellValue("Loại TS Hợp đồng");
                    rowHeaderDetail.CreateCell(6).SetCellValue("Loại TS đơn vay");
                    rowHeaderDetail.CreateCell(7).SetCellValue("Loại đơn chuyển thẳng PGD");
                    rowHeaderDetail.CreateCell(8).SetCellValue("Mã Hđ");
                    rowHeaderDetail.CreateCell(9).SetCellValue("Nguồn phụ");
                    rowHeaderDetail.CreateCell(10).SetCellValue("Ngày chuyển đơn về PGD");
                    rowHeaderDetail.CreateCell(11).SetCellValue("Ngày giờ tạo HĐ");
                    rowHeaderDetail.CreateCell(12).SetCellValue("Ngày tạo đơn online");
                    rowHeaderDetail.CreateCell(13).SetCellValue("PGD nhận đơn sau cùng");
                    rowHeaderDetail.CreateCell(14).SetCellValue("PGD tạo HĐ");
                    rowHeaderDetail.CreateCell(15).SetCellValue("PGD nhận đơn");
                    rowHeaderDetail.CreateCell(16).SetCellValue("Pawn Online Id");
                    rowHeaderDetail.CreateCell(17).SetCellValue("Số tiền vay");
                    rowHeaderDetail.CreateCell(18).SetCellValue("Final Status Loan");
                    rowHeaderDetail.CreateCell(19).SetCellValue("PaidMonth");
                    rowHeaderDetail.CreateCell(20).SetCellValue("PaidYear");
                    var paymentDetail =  _paymentRepository.GetPaymentDetail(createdOn);
                    for (int i = 0; i < paymentDetail.Count; i++)
                    {
                        IRow rowDetail = sheetDetail.CreateRow(i + 1);
                        rowDetail.CreateCell(0).SetCellValue(paymentDetail[i].TransactionId);
                        rowDetail.CreateCell(1).SetCellValue(paymentDetail[i].UserLoanReferralId);
                        rowDetail.CreateCell(2).SetCellValue(paymentDetail[i].ReferralPhone);
                        rowDetail.CreateCell(3).SetCellValue(paymentDetail[i].IsF88Cus == true ? "1" : "0");
                        rowDetail.CreateCell(4).SetCellValue(paymentDetail[i].PhoneByUser);                        
                        rowDetail.CreateCell(5).SetCellValue(paymentDetail[i].AssetSale);
                        rowDetail.CreateCell(6).SetCellValue(paymentDetail[i].Asset);
                        rowDetail.CreateCell(7).SetCellValue(paymentDetail[i].FocusTransaction);
                        rowDetail.CreateCell(8).SetCellValue(paymentDetail[i].ContractId);
                        rowDetail.CreateCell(9).SetCellValue(paymentDetail[i].SubSrouce);
                        rowDetail.CreateCell(10).SetCellValue(paymentDetail[i].TransferAppDate.ToString("dd/MM/yyyy HH:mm"));
                        rowDetail.CreateCell(11).SetCellValue(paymentDetail[i].CreateContractDate == null ? "" :  paymentDetail[i].CreateContractDate.Value.ToString("dd/MM/yyyy HH:mm"));
                        rowDetail.CreateCell(12).SetCellValue(paymentDetail[i].CreateOnlineDate.ToString("dd/MM/yyyy HH:mm"));
                        rowDetail.CreateCell(13).SetCellValue(paymentDetail[i].RefFinalGroupId);
                        rowDetail.CreateCell(14).SetCellValue(paymentDetail[i].RefContractGroupId);
                        rowDetail.CreateCell(15).SetCellValue(paymentDetail[i].RefRealGroupId);
                        rowDetail.CreateCell(16).SetCellValue(paymentDetail[i].PolId);
                        rowDetail.CreateCell(17).SetCellValue(paymentDetail[i].LoanMoney == null ? 0 : (double)paymentDetail[i].LoanMoney);
                        if (paymentDetail[i].Status == 0)
                        {
                            rowDetail.CreateCell(18).SetCellValue("Pending");
                        }
                        else if (paymentDetail[i].Status == 1)
                        {
                            rowDetail.CreateCell(18).SetCellValue("Success");
                        }
                        else
                        {
                            rowDetail.CreateCell(18).SetCellValue("Failed");
                        }
                        rowDetail.CreateCell(19).SetCellValue(paymentDetail[i].PaidMonth);
                        rowDetail.CreateCell(20).SetCellValue(paymentDetail[i].PaidYear);
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                System.IO.File.Delete(fullPath);
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
        [HttpGet("ExportPaymentPartner")]
        public async Task<IActionResult> ExportPaymentPartner(string fromDate,string toDate)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate)) return Ok(new
                {

                    result = false,
                    message = "Vui lòng nhập thời gian.",

                });

                string sWebRootFolder = $"{_env.ContentRootPath}/Export/Excels/";
                string sFileName = $@"ThongTinThanhToanPartner{DateTime.Now.ToString("ddMMyyyymmss")}.xlsx";
                var fullPath = Path.Combine(sWebRootFolder, sFileName);
                FileInfo file = new FileInfo(fullPath);
                var filePath = Path.Combine(
                               Directory.GetCurrentDirectory(), "Upload",
                               sWebRootFolder);

                bool folderExists = Directory.Exists(filePath);
                if (!folderExists)
                    Directory.CreateDirectory(filePath);
                var memory = new MemoryStream();
                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet sheetInform = workbook.CreateSheet("Thông tin thanh toán");
                    IRow rowHeaderInform = sheetInform.CreateRow(0);
                    rowHeaderInform.CreateCell(0).SetCellValue("STT");
                    rowHeaderInform.CreateCell(1).SetCellValue("Nguồn CTV");
                    rowHeaderInform.CreateCell(2).SetCellValue("Tên CTV");
                    rowHeaderInform.CreateCell(3).SetCellValue("Số điện thoại CTV");
                    rowHeaderInform.CreateCell(4).SetCellValue("Số điện thoại được giới thiệu");
                    rowHeaderInform.CreateCell(5).SetCellValue("Họ tên");
                    rowHeaderInform.CreateCell(6).SetCellValue("Trạng thái cuối");
                    rowHeaderInform.CreateCell(7).SetCellValue("PolId");
                    rowHeaderInform.CreateCell(8).SetCellValue("Ngày tạo");
                    var paymentDetail = _paymentRepository.GetPaymentPartnership(fromDate,toDate);
                    for (int i = 0; i < paymentDetail.Count; i++)
                    {
                        IRow rowInform = sheetInform.CreateRow(i + 1);
                        rowInform.CreateCell(0).SetCellValue(i + 1);
                        rowInform.CreateCell(1).SetCellValue(paymentDetail[i].Source);
                        rowInform.CreateCell(2).SetCellValue(paymentDetail[i].PartnerFullName);
                        rowInform.CreateCell(3).SetCellValue(paymentDetail[i].PartnerPhoneNumber);
                        rowInform.CreateCell(4).SetCellValue(paymentDetail[i].PhoneNumber);
                        rowInform.CreateCell(5).SetCellValue(paymentDetail[i].FullName);
                        if (paymentDetail[i].Status == LoanStatus.APPROVED)
                        {
                        rowInform.CreateCell(6).SetCellValue("Nhận cầm cố");
                        }
                        else if(paymentDetail[i].Status == LoanStatus.CANCEL)
                        {
                            rowInform.CreateCell(6).SetCellValue("Huỷ đăng ký");
                        }
                        else
                        {
                            rowInform.CreateCell(6).SetCellValue("Đang chăm sóc");
                        }
                        rowInform.CreateCell(7).SetCellValue(paymentDetail[i].PolId);
                        rowInform.CreateCell(8).SetCellValue(paymentDetail[i].CreateDate.ToString("dd/MM/yyyy"));
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                System.IO.File.Delete(fullPath);
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
