using F88.Digital.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Reporting.Queries
{
    public class ReportingPaymentResponse
    {
        public int PaymentId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Passport { get; set; }

        public decimal RewardMoney { get; set; }

        public List<ReportingPaymentDetailsResponse> PaymentDetails { get; set; }

        public int PageNumber { get; set; }

        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int StartNum { get; set; }
        public int EndNum { get; set; }
        public int UserProfileId { get; set; }
    }

    public class ReportingPaymentDetailsResponse
    {
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string FormGroup { get; set; }

        public string RandomGroup { get; set; }

        public string SalesGroup { get; set; }

        public string FinalGroup { get; set; }

        public DateTime Created { get; set; }

        public DateTime? SaleDate { get; set; }

        public string TimeToSale
        {
            get
            {
                if (String.IsNullOrEmpty(SalesGroup)) return string.Empty;
                
                TimeSpan span = (TimeSpan)(SaleDate - Created);
                if(span.Days > 0) return $"{span.Days} ngày {span.Hours} giờ {span.Minutes} phút";

                return $"{span.Hours} giờ {span.Minutes} phút";

            }
        }

        public string Warning
        {
            get
            {
                if (String.IsNullOrEmpty(SalesGroup)) return string.Empty;

                TimeSpan span = (TimeSpan)(SaleDate - Created);

                if (span.Days > 0) return string.Empty;

                return (span.Hours * 60 + span.Minutes) < 45 ? ApiConstants.Warning.WARNING_SALE : string.Empty;
            }
           
        }

        public class GroupUserProfile
        {
            public string FullName { get; set; }

            public string Phone { get; set; }

            public string Passport { get; set; }

            public int UserProfileId { get; set; }

            public decimal? RewardMoney { get; set; }

            public List<ReportingPaymentDetailsResponse> PaymentDetails { get; set; }
        }
    }
}
