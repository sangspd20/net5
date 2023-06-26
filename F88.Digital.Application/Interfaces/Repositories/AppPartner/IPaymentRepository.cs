using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries;
using F88.Digital.Domain.Entities.Share;
using RestSharp;
using F88.Digital.Application.DTOs.POL.Response;
using F88.Digital.Application.DTOs.POL.Request;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IPaymentRepository
    {
        IQueryable<Payment> Payments { get; }

        Task<int> InsertAsync(Payment userProfile);

        Task<Payment> InsertAsyncPayment(Payment payment);

        Task<Payment> GetByIdAsync(int Id);

        Task<List<Payment>> GetListTransactionPaymentByCurrentMonthsync(int userProfileId);

        Task<List<Payment>> GetListTransactionPaymentByDateAsync(int userProfileId, DateTime fromDate, DateTime toDate);

        Task<List<Payment>> Report_ListTransactionPaymentByMonthAsync(int userProfileId, int month, int year);
        List<Payment> GetPaymentByPhoneTrans(string phoneNumber,DateTime transDate, int status);

        List<GetPaymentInform> GetPaymentInform(string createdOn);
        Task<List<Payment>> GetPaymentsOfUserByMonthAsync(string userPhone, int month, int year);

        List<PaymentModel> GetPaymentHistory(string userphone);
        List<GetPaymentDetail> GetPaymentDetail(string createdOn);
        List<GetPaymentPartnership> GetPaymentPartnership(string fromDate, string toDate);
        int GetGroupIdByProvince(string province, string county);
        PawnOnlineSource GetPawnOnlineSourceByUrlSource(string urlSource);
        bool ReadGoogleSheet(string spreadSheetId);
        decimal GetTotalPaidForUser(string userphone);
        IRestResponse SendPOL(DataGoogleSheetQuery model);
        Payment GetPaymentById(int userLoanReferralId);
        PawnOnlineSource GetPawnOnlineSourceBySubSource(string subSource);
        bool JobReSendPolCancel();
    }
}
