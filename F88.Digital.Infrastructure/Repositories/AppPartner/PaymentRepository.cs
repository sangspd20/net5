using AutoMapper;
using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Application.Features.AppPartner.UploadFile;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using F88.Digital.Domain.Entities.Share;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PawnOnlineAuditSystem = F88.Digital.Application.Features.AppPartner.Payment.Queries.PawnOnlineAuditSystem;
using RestSharp;
using F88.Digital.Application.DTOs.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Google.Apis.Sheets.v4.Data;
using F88.Digital.Application.DTOs.POL.Request;
using F88.Digital.Application.DTOs.POL.Response;
using F88.Digital.Application.Interfaces.Shared;
using static F88.Digital.Application.Constants.Permissions;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IRepositoryAsync<Payment> _repository;
        private readonly IDistributedCache _distributedCache;
        private readonly AppPartnerDbContext _context;
        private readonly IApiPolService _apiPolService;
        private readonly AffiliateDbContext _contextAffiliate;
        private readonly ILogger<PaymentRepository> _logger;
        private readonly IMapper _mapper;
        private readonly string[] scopes = { SheetsService.Scope.Spreadsheets };
        //private readonly string spreadSheetId = "1DSxaits83r7p-n6KgE3_MZww2OmwkrJn4hU3W84VWxA";
        private readonly SpreadSheetIdSetting _spreadSheetIdSetting;
        private readonly UrlPOLSetting _urlPOLSetting;
        private readonly string applicationName = "Scanning GG sheet";
        private string sheetName = "Data";
        //private readonly string urlPOL = "http://192.168.10.33:1994/LadipageReturnID/";
        public PaymentRepository(IRepositoryAsync<Payment> repository,
            IDistributedCache distributedCache,
            AppPartnerDbContext context, IMapper mapper, AffiliateDbContext contextAffiliate, IOptions<SpreadSheetIdSetting> spreadSheetIdSetting, IOptions<UrlPOLSetting> urlPOLSetting,
            ILogger<PaymentRepository> logger, IApiPolService apiPolService)
        {
            _repository = repository;
            _distributedCache = distributedCache;
            _context = context;
            _mapper = mapper;
            _spreadSheetIdSetting = spreadSheetIdSetting.Value;
            _urlPOLSetting = urlPOLSetting.Value;
            _contextAffiliate = contextAffiliate;
            _apiPolService = apiPolService;
            _logger = logger;
        }

        public IQueryable<Payment> Payments => _repository.Entities;

        public IQueryable<Payment> UserProfiles => throw new NotImplementedException();

        public async Task<Payment> GetByIdAsync(int Id)
        {
            var payment = await Payments.Where(x => x.Id == Id)
                                        .Include(x => x.PaymentUserLoanReferrals)
                                        .FirstOrDefaultAsync();

            return payment;
        }

        public async Task<List<Payment>> GetListTransactionPaymentByCurrentMonthsync(int userProfileId)
        {
            var lstPayment = await Payments.Include(x => x.UserBank)
                                           .ThenInclude(x => x.Bank)
                                           .Where(x => x.UserBank.UserProfileId == userProfileId
                                                        && x.CreatedOn.Month == DateTime.Now.Month
                                                        && x.CreatedOn.Year == DateTime.Now.Year)
                                           .Include(x => x.PaymentUserLoanReferrals)
                                           .ThenInclude(x => x.UserLoanReferral)
                                           .ToListAsync();
            return lstPayment;
        }

        public async Task<List<Payment>> GetListTransactionPaymentByDateAsync(int userProfileId, DateTime fromDate, DateTime toDate)
        {
            var lstPayment = await Payments.Include(x => x.UserBank)
                                           .ThenInclude(x => x.Bank)
                                           .Include(x => x.PaymentUserLoanReferrals)
                                           .ThenInclude(x => x.UserLoanReferral)
                                           .Where(x => x.UserBank.UserProfileId == userProfileId
                                                                && x.CreatedOn.Date >= fromDate.Date
                                                                && x.CreatedOn.Date <= toDate.Date)
                                           .ToListAsync();
            return lstPayment;
        }

        public async Task<int> InsertAsync(Payment payment)
        {
            await _repository.AddAsync(payment);
            return payment.Id;
        }

        public async Task<Payment> InsertAsyncPayment(Payment payment)
        {
            var item = await _context.Payments.AddAsync(payment);
            _context.SaveChanges();
            return item.Entity;

        }

        public Task<List<Payment>> Report_ListTransactionPaymentByMonthAsync(int userProfileId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public List<Payment> GetPaymentByPhoneTrans(string phoneNumber, DateTime transDate, int status)
        {
            return _context.Payments.Where(x => x.UserPhone.Trim() == phoneNumber.Trim() && x.TransferDate.Value.Month == transDate.Month && x.TransferDate.Value.Year == transDate.Year && x.Status == status).ToList();
        }
        public Payment GetPaymentById(int userLoanReferralId)
        {
            var paymentUserLoanReferral = _context.PaymentUserLoanReferral.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.UserLoanReferralId == userLoanReferralId);
            if (paymentUserLoanReferral != null)
            {
                var payment = _context.Payments.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.Id == paymentUserLoanReferral.PaymentId);
                return payment;
            }
            return null;
        }

        public List<GetPaymentInform> GetPaymentInform(string createdOn)
        {
            var query = (from userprofile in _context.UserProfiles.Include(x => x.UserBanks).ThenInclude(x => x.Bank)
                         join deposits in _context.Deposits on userprofile.Id equals deposits.UserProfileId
                         where deposits.Status == true
                         let userBankStatus = userprofile.UserBanks.FirstOrDefault(x => x.UserBankStatus)
                         select new
                         {
                             UserLoanReferralId = deposits.Id,
                             PhoneNumber = userprofile.UserPhone,
                             Passport = userprofile.Passport,
                             //AccountName = userprofile.UserBanks.FirstOrDefault(x => x.UserBankStatus).AccName,
                             AccountName = userBankStatus.AccName,
                             BankCode = userBankStatus.Bank.Code,
                             BankName = userBankStatus.Bank.Name ?? "",
                             BankBranch = userBankStatus.Branch ?? "",
                             AccountNumber = userprofile.UserBanks.FirstOrDefault().AccNumber,
                             CurrentMoney = deposits.BalanceValue,
                             IsAgreementConfirmed = userprofile.IsAgreementConfirmed,
                             Source = userprofile.Source,
                             PayDate = deposits.CreatedOn,
                             TransferDate = new DateTime(),
                             PaymentCreatedOn = "",
                             PaidMonth = deposits.CreatedOn.ToString("MM"),
                             PaidYear = deposits.CreatedOn.ToString("yyyy"),
                             MoneyPaid = "",
                             Note = "",
                             Status = 0,
                         }).ToList();

            if (!string.IsNullOrEmpty(createdOn))
            {
                var formatDate = StringUtils.FormatDateTime(createdOn, "dd-MM-yyyy");
                query = query.Where(x => x.PayDate.Month == formatDate.Month && x.PayDate.Year == formatDate.Year).ToList();
            }

            var rs = (from item in query
                     where item.CurrentMoney != 0
                     let payment = GetPaymentById(item.UserLoanReferralId)
                     group item by new
                     {
                         item.PayDate.Month,
                         item.PayDate.Year,
                         item.PhoneNumber,
                         Payment = payment
                     } into groupTemp
                     select new GetPaymentInform()
                     {
                         PhoneNumber = groupTemp.FirstOrDefault().PhoneNumber,
                         Passport = groupTemp.FirstOrDefault().Passport,
                         AccountName = groupTemp.FirstOrDefault().AccountName,
                         BankCode = groupTemp.FirstOrDefault().BankCode,
                         BankName = groupTemp.FirstOrDefault().BankName,
                         BankBranch = groupTemp.FirstOrDefault().BankBranch,
                         AccountNumber = groupTemp.FirstOrDefault().AccountNumber,
                         CurrentMoney = groupTemp.Sum(s => s.CurrentMoney),
                         NetMoney = groupTemp.Sum(s => s.CurrentMoney) >= 2000000 ? (groupTemp.Sum(s => s.CurrentMoney) * 90) / 100 : groupTemp.Sum(s => s.CurrentMoney),
                         Tax = groupTemp.Sum(s => s.CurrentMoney) >= 2000000 ? 10 : 0,
                         Content = $"TT F88 Partner Tháng {groupTemp.FirstOrDefault().PaidMonth}-{groupTemp.FirstOrDefault().PaidYear}",
                         MoneyPaid = groupTemp.Key.Payment?.PaidValue.ToString(),
                         Note = groupTemp.Key.Payment?.Notes.ToString(),
                         Status = groupTemp.Key.Payment?.Status ?? 0,
                         TransferDate = groupTemp.Key.Payment?.TransferDate,
                         PayDate = groupTemp.FirstOrDefault().PayDate,
                         OtherAmount = "",
                         IsAgreementConfirmed = groupTemp.FirstOrDefault().IsAgreementConfirmed,
                         PaidMonth = groupTemp.FirstOrDefault().PaidMonth,
                         PaidYear = groupTemp.FirstOrDefault().PaidYear,
                         Source = groupTemp.FirstOrDefault().Source
                     });

            //var rs = query.Where(w => w.CurrentMoney != 0).GroupBy(x => new { x.PayDate.Month, x.PayDate.Year, x.PhoneNumber});        
            //var rs = query.Where(w => w.CurrentMoney != 0).GroupBy(x => new { x.PayDate.Month, x.PayDate.Year, x.PhoneNumber }).Select(x => new GetPaymentInform
            //{
            //    PhoneNumber = x.FirstOrDefault().PhoneNumber,
            //    Passport = x.FirstOrDefault().Passport,
            //    AccountName = x.FirstOrDefault().AccountName,
            //    BankCode = x.FirstOrDefault().BankCode,
            //    BankName = x.FirstOrDefault().BankName,
            //    BankBranch = x.FirstOrDefault().BankBranch,
            //    AccountNumber = x.FirstOrDefault().AccountNumber,
            //    CurrentMoney = x.Sum(s => s.CurrentMoney),
            //    NetMoney = x.Sum(s => s.CurrentMoney) >= 2000000 ? (x.Sum(s => s.CurrentMoney) * 90) / 100 : x.Sum(s => s.CurrentMoney),
            //    Tax = x.Sum(s => s.CurrentMoney) >= 2000000 ? 10 : 0,
            //    Content = $"TT F88 Partner Tháng {x.FirstOrDefault().PaidMonth}-{x.FirstOrDefault().PaidYear}",
            //    MoneyPaid = GetPaymentById(x.FirstOrDefault().UserLoanReferralId)?.PaidValue.ToString(),
            //    Note = GetPaymentById(x.FirstOrDefault().UserLoanReferralId)?.Notes.ToString(),
            //    Status = GetPaymentById(x.FirstOrDefault().UserLoanReferralId)?.Status ?? 0,
            //    TransferDate = GetPaymentById(x.FirstOrDefault().UserLoanReferralId)?.TransferDate,
            //    PayDate = x.FirstOrDefault().PayDate,
            //    OtherAmount = "",
            //    IsAgreementConfirmed = x.FirstOrDefault().IsAgreementConfirmed,
            //    PaidMonth = x.FirstOrDefault().PaidMonth,
            //    PaidYear = x.FirstOrDefault().PaidYear,
            //    Source = x.FirstOrDefault().Source
            //});


            return rs.ToList();
        }

        public List<PawnOnlineAuditSystem> GetPawnOnlineAuditSystem(DateTime date)
        {
            var data = new List<PawnOnlineAuditSystem>();
            var query = _contextAffiliate.PawnOnlineAuditSystem.Where(x => x.SentPartnerStatus == "2" && x.SentPartnerDate.Value.Month == date.Month && x.SentPartnerDate.Value.Year == date.Year).Select(x => new PawnOnlineAuditSystem
            {
                TransactionId = x.TransactionId,
                Asset = x.Asset,
                AssetSale = x.AssetSale,
                LoanValue = x.LoanValue,
                PawnOnlineId = x.PawnOnlineId,
            });
            return query.ToList();
        }
        public List<GetPaymentDetail> GetPaymentDetail(string createdOn)
        {
            //var abc = _context.PaymentUserLoanReferral.Where(x => x.UserLoanReferralId == 1).OrderByDescending(x => x.CreatedOn).FirstOrDefault()?.Status ?? 0;
            var formatDate = StringUtils.FormatDateTime(createdOn, "dd-MM-yyyy");           
            var query = _context.UserLoanReferrals.Include(x => x.UserProfile).Where(x => x.CreatedOn.Month == formatDate.Month && x.CreatedOn.Year == formatDate.Year && x.LoanStatus == 2).ToList();

            var data = (from item in query
                        select new GetPaymentDetail
                        {
                            TransactionId = item.TransactionId,
                            UserLoanReferralId = item.Id,
                            ReferralPhone = item.PhoneNumber,
                            IsF88Cus = item.IsF88Cus,
                            PhoneByUser = item.UserProfile.UserPhone,
                            AssetSale = item.RefAsset,
                            Asset = item.RefAsset,
                            FocusTransaction = "Đơn chuyển thẳng PGD",
                            ContractId = item.PawnId,
                            SubSrouce = "App partner",
                            TransferAppDate = item.CreatedOn,
                            CreateContractDate = item.LastModifiedOn,
                            CreateOnlineDate = item.CreatedOn,
                            RefFinalGroupId = item.RefFinalGroupId,
                            RefContractGroupId = item.RefContractGroupId,
                            RefRealGroupId = item.RefRealGroupId,
                            PolId = item.PolId,
                            LoanMoney = item.LoanAmount,
                            Status = _context.PaymentUserLoanReferral.Where(w => w.UserLoanReferralId == item.Id).OrderByDescending(w => w.CreatedOn).FirstOrDefault()?.Status ?? 0,
                            PaidMonth = formatDate.ToString("MM"),
                            PaidYear = formatDate.ToString("yyyy"),
                        }).ToList();

            //var data = query.Select(x => new GetPaymentDetail
            //{
            //    TransactionId = x.TransactionId,
            //    UserLoanReferralId = x.Id,
            //    ReferralPhone = x.PhoneNumber,
            //    IsF88Cus = x.IsF88Cus,
            //    PhoneByUser = x.UserProfile.UserPhone,
            //    AssetSale = pawnOnlineAudit.FirstOrDefault(f => f.TransactionId == x.TransactionId)?.AssetSale,
            //    Asset = pawnOnlineAudit.FirstOrDefault(f => f.TransactionId == x.TransactionId)?.Asset,
            //    FocusTransaction = "Đơn chuyển thẳng PGD",
            //    ContractId = "",
            //    SubSrouce = "App partner",
            //    TransferAppDate = x.CreatedOn,
            //    CreateContractDate = x.LastModifiedOn,
            //    CreateOnlineDate = x.CreatedOn,
            //    RefFinalGroupId = x.RefFinalGroupId,
            //    RefContractGroupId = x.RefContractGroupId,
            //    RefRealGroupId = x.RefRealGroupId,
            //    PawnOnlineWid = pawnOnlineAudit.FirstOrDefault(f => f.TransactionId == x.TransactionId)?.PawnOnlineId,
            //    LoanMoney = pawnOnlineAudit.FirstOrDefault(f => f.TransactionId == x.TransactionId)?.LoanValue,
            //    Status = _context.PaymentUserLoanReferral.Where(w => w.UserLoanReferralId == x.Id).OrderByDescending(w => w.CreatedOn).FirstOrDefault()?.Status ?? 0,
            //    PaidMonth = formatDate.ToString("MM"),
            //    PaidYear = formatDate.ToString("yyyy"),
            //}).ToList();
            return data;
        }
        public List<GetPaymentPartnership> GetPaymentPartnership(string fromDate, string toDate)
        {
            var formatFromDate = StringUtils.FormatDateTime(fromDate, "dd-MM-yyyy");
            var formatToDate = StringUtils.FormatDateTime(toDate, "dd-MM-yyyy");
            var userLoanReferals = _context.UserLoanReferrals.Where(x => x.CreatedOn.Date >= formatFromDate.Date && x.CreatedOn.Date <= formatToDate.Date);
            var userProfiles = _context.UserProfiles.Where(x => x.Source == "CTV doanh nghiệp");
            return (from userLoanReferal in userLoanReferals
                    join userProfile in userProfiles on userLoanReferal.UserProfileId equals userProfile.Id
                    select new GetPaymentPartnership
                    {
                        Source = userProfile.Source,
                        PartnerPhoneNumber = userProfile.UserPhone,
                        PartnerFullName = $"{userProfile.LastName} {userProfile.FirstName}",
                        PhoneNumber = userLoanReferal.PhoneNumber,
                        FullName = userLoanReferal.FullName,
                        PolId = userLoanReferal.PolId,
                        Status = userLoanReferal.LoanStatus,
                        CreateDate = userLoanReferal.CreatedOn
                    }).ToList();           
        }

        public async Task<List<Payment>> GetPaymentsOfUserByMonthAsync(string userPhone, int month, int year)
        {
            return await Payments.Where(r => r.UserPhone == userPhone && r.CreatedOn.Month == month && r.CreatedOn.Year == year).ToListAsync();
        }

        public List<PaymentModel> GetPaymentHistory(string userphone)
        {
            var query = _context.Payments.Include(x => x.UserBank).ThenInclude(x => x.Bank).Where(x => x.UserPhone == userphone).AsEnumerable();

            var rs = query.Select(x => new PaymentModel
            {
                AccountNumber = x.UserBank.AccNumber,
                AccountName = x.UserBank.Bank.Name,
                CreatedBy = x.CreatedBy,
                LastModifiedBy = x.LastModifiedBy,
                CreatedOn = x.CreatedOn,
                LastModifiedOn = x.LastModifiedOn,
                Notes = x.Notes,
                OtherAmount = x.OtherAmount,
                PaidValue = x.PaidValue,
                Status = x.Status,
                TaxValue = x.TaxValue,
                TransferDate = x.TransferDate,
                UserBankId = x.UserBankId,
                UserPhone = x.UserPhone,
                Id = x.Id,
                PaidMonth = x.PaidMonth,
                PaidYear = x.PaidYear
            });
            return rs.ToList();
        }

        public decimal GetTotalPaidForUser(string userphone)
        {
            return GetPaymentHistory(userphone).Where(r => r.Status != 0).Sum(r => r.PaidValue).Value;
        }

        public int GetGroupIdByProvince(string province, string county)
        {
            var rs = _contextAffiliate.Set<GroupProvinceQuery>().FromSqlRaw("dbo.GetGroupIdByProvince @Province={0},@County={1}", province, county).AsEnumerable();
            if (rs.Any())
            {
                return rs.FirstOrDefault().GroupId;
            }
            return 0;

        }
        public int? GetCancelRandomGroupId(string regionID)
        {
            var rs = _contextAffiliate.LocationShop.Where(x => x.RegionID == regionID && x.Status == true);
            if (rs.Any())
            {
                if (rs.Count() > 1)
                {
                    var rnd = new Random();
                    var random = rnd.Next(rs.Count());
                    return rs.ToArray()[random].GroupID;
                }
                return rs.FirstOrDefault()?.GroupID;
            }
            return 110;
        }

        public PawnOnlineSource GetPawnOnlineSourceByUrlSource(string urlSource)
        {
            return _contextAffiliate.PawnOnlineSource.FirstOrDefault(x => x.URLSource.ToLower().Equals(urlSource.ToLower()));
        }
        public PawnOnlineSource GetPawnOnlineSourceBySubSource(string subSource)
        {
            return _contextAffiliate.PawnOnlineSource.FirstOrDefault(x => x.SubSource.ToLower().Equals(subSource.ToLower()));
        }
        public bool ReadGoogleSheet(string spreadSheetId)
        {
            try
            {
                var service = GoogleSheetHelper.Authentication(scopes, applicationName);
                var range = $"{sheetName}!A:N";
                SpreadsheetsResource.ValuesResource.GetRequest requestSheet =
                    service.Spreadsheets.Values.Get(spreadSheetId, range);
                var responseSheet = requestSheet.Execute();
                var listData = responseSheet.Values.Skip(1).Where(x => x[8].ToString() != "1");
                var listNull = responseSheet.Values.Skip(1).Where(x => x[8].ToString() == "1" && x[9].ToString() == "-1" && x[10].ToString() == "null" && (string.IsNullOrEmpty(x[11].ToString()) || int.Parse(x[11].ToString()) < 2));
                if (listNull.Any())
                {
                    foreach (var item in listNull)
                    {
                        var googleSheet = new DataGoogleSheetQuery
                        {
                            Position = item[0].ToString(),
                            FullName = item[1].ToString(),
                            PhoneNumber = StringUtils.FormatPhoneNumber(item[2].ToString()),
                            Url = item[3].ToString(),
                            Description = item[4].ToString(),
                            GroupId = item[5].ToString(),
                            Province = item[6].ToString(),
                            District = item[7].ToString(),
                            IsRead = item[8].ToString(),
                            Status = item[9].ToString(),
                            Note = item[10].ToString(),
                            Records = string.IsNullOrEmpty(item[11].ToString()) ? "0" : item[11].ToString(),
                            Asset = item[12].ToString(),
                        };
                        var position = int.Parse(googleSheet.Position) + 1;
                        if (!string.IsNullOrEmpty(googleSheet.Province))
                        {
                            if (string.IsNullOrEmpty(googleSheet.GroupId))
                            {
                                googleSheet.GroupId = GetGroupIdByProvince(googleSheet.Province, googleSheet.District).ToString(); // random groupId;                            
                            }
                            if (googleSheet.GroupId != "0")
                            {
                                var responsePOL = SendPOL(googleSheet); //call api insert Data POL
                                var responseJson = JsonConvert.DeserializeObject<JsonReponseQuery>(responsePOL.Content);
                                var status = responseJson?.success == true ? GoogleSheetStatus.Success.GetHashCode() : GoogleSheetStatus.Failed.GetHashCode();
                                if (string.IsNullOrEmpty(responseJson.message) || responseJson?.message.Equals("Kết nối bị gián đoạn, tạo đơn không thành công. Vui lòng thử lại sau!") == true || responseJson.message.Equals("Đã có lead có cùng số điện thoại trong vòng 90 ngày"))
                                {
                                    responseJson.message = "";
                                }
                                if (string.IsNullOrEmpty(googleSheet.District))
                                {
                                    var district = _contextAffiliate.LocationShop.FirstOrDefault(x => x.GroupID.ToString() == googleSheet.GroupId);
                                    if (district != null)
                                    {
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, googleSheet.GroupId, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.H, position, district.County, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                                        if (googleSheet.Url.ToLower().Equals("gogroup.excel"))
                                        {
                                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.O, position, status == 1 ? $"POL-{responseJson.data.LastOrDefault()}" : "null", spreadSheetId, sheetName);
                                        }
                                    }
                                    else
                                    {
                                        responseJson.message = "Không tìm thấy Quận/Huyện Hoặc Tinh/Thành Phố không tồn tại";
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                                    }
                                }
                                else
                                {
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, googleSheet.GroupId, spreadSheetId, sheetName);
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                                    if (googleSheet.Url.ToLower().Equals("gogroup.excel"))
                                    {
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.O, position, status == 1 ? $"POL-{responseJson.data.LastOrDefault()}" : "null", spreadSheetId, sheetName);
                                    }
                                }
                            }
                            else
                            {
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, "-1", spreadSheetId, sheetName);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, "Không tìm thấy Quận/Huyện Hoặc Tinh/Thành Phố không tồn tại", spreadSheetId, sheetName);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                            }
                        }
                        else
                        {
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, "-1", spreadSheetId, sheetName);
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, "Không có Tỉnh/Thành phố", spreadSheetId, sheetName);
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                        }
                    }
                }
                if (listData.Any())
                {
                    foreach (var item in listData)
                    {
                        var googleSheet = new DataGoogleSheetQuery
                        {
                            Position = item[0].ToString(),
                            FullName = item[1].ToString(),
                            PhoneNumber = StringUtils.FormatPhoneNumber(item[2].ToString()),
                            Url = item[3].ToString(),
                            Description = item[4].ToString(),
                            GroupId = item[5].ToString(),
                            Province = item[6].ToString(),
                            District = item[7].ToString(),
                            IsRead = item[8].ToString(),
                            Status = item[9].ToString(),
                            Note = item[10].ToString(),
                            Records = string.IsNullOrEmpty(item[11].ToString()) ? "0" : item[11].ToString(),
                            Asset = item[12].ToString(),
                        };
                        var position = int.Parse(googleSheet.Position) + 1;
                        if (!string.IsNullOrEmpty(googleSheet.Province))
                        {
                            if (string.IsNullOrEmpty(googleSheet.GroupId))
                            {
                                googleSheet.GroupId = GetGroupIdByProvince(googleSheet.Province, googleSheet.District).ToString(); // random groupId;                            
                            }
                            if (googleSheet.GroupId != "0")
                            {
                                var responsePOL = SendPOL(googleSheet); //call api insert Data POL
                                var responseJson = JsonConvert.DeserializeObject<JsonReponseQuery>(responsePOL.Content);
                                var status = responseJson?.success == true ? GoogleSheetStatus.Success.GetHashCode() : GoogleSheetStatus.Failed.GetHashCode();
                                if (string.IsNullOrEmpty(responseJson.message) || responseJson?.message.Equals("Kết nối bị gián đoạn, tạo đơn không thành công. Vui lòng thử lại sau!") == true || responseJson.message.Equals("Đã có lead có cùng số điện thoại trong vòng 90 ngày"))
                                {
                                    responseJson.message = "";
                                }
                                if (string.IsNullOrEmpty(googleSheet.District))
                                {
                                    var district = _contextAffiliate.LocationShop.FirstOrDefault(x => x.GroupID.ToString() == googleSheet.GroupId);
                                    if (district != null)
                                    {
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, googleSheet.GroupId, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.H, position, district.County, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                                        if (googleSheet.Url.ToLower().Equals("gogroup.excel"))
                                        {
                                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.O, position, status == 1 ? $"POL-{responseJson.data.LastOrDefault()}" : "null", spreadSheetId, sheetName);
                                        }
                                    }
                                    else
                                    {
                                        responseJson.message = "Không tìm thấy Quận/Huyện Hoặc Tinh/Thành Phố không tồn tại";
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                                    }
                                }
                                else
                                {
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.F, position, googleSheet.GroupId, spreadSheetId, sheetName);
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                   GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, status.ToString(), spreadSheetId, sheetName); //update cột trạng thái nếu insert POL thành công = 1, thất bại = -1
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, string.IsNullOrEmpty(responseJson?.message) ? "null" : responseJson.message, spreadSheetId, sheetName);
                                    GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                                    if (googleSheet.Url.ToLower().Equals("gogroup.excel"))
                                    {
                                        GoogleSheetHelper.UpdateData(ColumnGoogleSheet.O, position, status == 1 ? $"POL-{responseJson.data.LastOrDefault()}" : "null", spreadSheetId, sheetName);
                                    }
                                }
                            }
                            else
                            {
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, "-1", spreadSheetId, sheetName);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, "Không tìm thấy Quận/Huyện Hoặc Tinh/Thành Phố không tồn tại", spreadSheetId, sheetName);
                                GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                            }
                        }
                        else
                        {
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.I, position, GoogleSheetReadStatus.IsRead.GetHashCode().ToString(), spreadSheetId, sheetName); //update cột Đã quét
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.J, position, "-1", spreadSheetId, sheetName);
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.K, position, "Không có Tỉnh/Thành phố", spreadSheetId, sheetName);
                            GoogleSheetHelper.UpdateData(ColumnGoogleSheet.L, position, $"{int.Parse(googleSheet.Records) + 1}", spreadSheetId, sheetName);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public bool JobReSendPolCancel()
        {
            try
            {
                var responsePolCancel = _apiPolService.ReSendPolCancel();
                if (responsePolCancel.ok == true)
                {
                    foreach (var recordset in responsePolCancel.data.recordset)
                    {
                        var requestSendPolQuery = new RequestSendPolQuery
                        {
                            PawnID = recordset.PawnID,
                            FullName = recordset.FullName,
                            PhoneNumber = recordset.PhoneNumber.Split(";").FirstOrDefault(),
                            Asset = recordset.Asset,
                            District = recordset.District,
                            Province = string.IsNullOrEmpty(recordset.Province) ? "Bình Dương" : recordset.Province,
                            Url = recordset.Url,
                            Description = "",
                            SubSource = recordset.SubSource,
                            IsCancel = true,
                            RegionID = recordset.DistrictId,
                        };
                        var responsePOL = SendCancelPOL(requestSendPolQuery);
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public IRestResponse SendCancelPOL(RequestSendPolQuery model)
        {
            var urlPOL = _urlPOLSetting.UrlPOL;
            var client = new RestClient(urlPOL);
            var req = new RestRequest("?", Method.POST);
            var source =!string.IsNullOrEmpty(model.SubSource) ? GetPawnOnlineSourceBySubSource(model.SubSource) : new PawnOnlineSource();
            var groupId = GetCancelRandomGroupId(model.RegionID).ToString();
            req.AddParameter("PawnID", model.PawnID);
            req.AddParameter("name", model.FullName);
            req.AddParameter("phone", model.PhoneNumber);
            req.AddParameter("select1", model.Asset);
            req.AddParameter("select2", "");
            req.AddParameter("District", model.District);
            req.AddParameter("Province", model.Province);
            req.AddParameter("link", model.Url);
            req.AddParameter("Description", model.Description);
            req.AddParameter("ReferenceType", source?.PartnerCode ?? "48");
            req.AddParameter("partnerCode", source?.PartnerCode ?? "48");
            req.AddParameter("CurrentGroupID", groupId);
            req.AddParameter("Source", source?.SubSource ?? "Facebook");
            req.AddParameter("TransactionID", model.PawnID);
            req.AddParameter("Passport", "");
            req.AddParameter("IsCancel", model.IsCancel);
            req.AddParameter("RegionID", model.RegionID);
            var rs = client.Execute(req);
            return rs;
        }
        public IRestResponse SendPOL(DataGoogleSheetQuery model)
        {
            var urlPOL = _urlPOLSetting.UrlPOL;
            var campaign = GetCampaignFromUrl(model.Url);
            var source = GetPawnOnlineSourceByUrlSource(campaign);
            var client = new RestClient(urlPOL);
            var req = new RestRequest("?", Method.POST);
            req.AddParameter("name", model.FullName);
            req.AddParameter("phone", model.PhoneNumber);
            req.AddParameter("select1", model.Asset);
            req.AddParameter("select2", "");
            req.AddParameter("District", model.District);
            req.AddParameter("Province", model.Province);
            req.AddParameter("link", model.Url);
            req.AddParameter("Description", model.Description);
            req.AddParameter("ReferenceType", source?.PartnerCode); 
            req.AddParameter("Passport", "");
            req.AddParameter("partnerCode", "F88");
            req.AddParameter("CurrentGroupID", model.GroupId);
            req.AddParameter("Source", source?.SubSource);
            req.AddParameter("TransactionID", "");
            if(campaign.Equals("KhaiThacThemGDN"))
            {
                req.AddParameter("Campaign", "22282");
            }    
            var rs = client.Execute(req);
            return rs;
        }
        public string GetCampaignFromUrl(string url)
        {
            if(url.ToLower().Contains("utm_campaign=google"))
            {
                return "Google.paid";
            } 
            else if (url.ToLower().Contains("utm_campaign=facebook"))
            {
                return "Facebook";
            }
            else if (url.ToLower().Contains("khaithacthem.com-google"))
            {
                return "khaithacthem.com";
            }
            else if (url.ToLower().Contains("gogroup"))
            {
                return "GoGroup.excel";
            }
            return "";
        }
    }
}
