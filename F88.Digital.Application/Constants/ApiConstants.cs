using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Constants
{
    public class ApiConstants
    {
        public class Code
        {
            public const int Success = 200;

            public const int Fail = 400;

            public const int NotFound = 404;

            public const int LoginFailed = 204;

            public const int RegisterFailed = 205;

            public const int Default = 222;

            public const int Exist = 300;

            public const int Exception = -1;

            public const int InvalidRequest = 995;

            public const int InvalidSignature = 996;

            public const int DuplicateRequest = 997;

            public const int ValidationError = 205;

            public const int EntityExist = 300;

            public const int EntityNotExist = 205;

            public const int EntityNotActive = 205;
        }

        public class AmountValue
        {
            public const Decimal REWARD_AMOUNT = 200000;
            public const Decimal REWARD_OTO_AMOUNT = 800000;
        }
        public static class ConstantAsset
        {
            public static string[] Assets = new[] { "ô tô", "đăng ký ô tô" };
        }

        public class LoanStatus
        {
            public const int APPROVED = 2;

            public const int CANCEL = 3;

            public const int PENDING = 0;
            public const int APPROVED_QFORM = 1;
        }

        public class PagingInfo
        {
            public const int PAGE_SIZE = 10;

            public const int PAGE_NUMBER = 20;
        }

        public class PartnerCode
        {
            public const int APP_PARTNER_CODE = 32;

            public const string APP_PARTNER_LINK = "XOCN App Mobile";
            public const string APP_PARTNER_COMPANY = "partnership.f88partnerb2b";
        }

        public class TransactionType
        {
            public const int DEPOSIT = 1;

            public const int WITHDRAW = 2;
        }

        public class NotificationType
        {
            public const int PUBLISH = -1;

            public const int PRIVATE = 1;
        }


        public class Warning
        {
            public const string WARNING_SALE = "Thời gian ra sale ít hơn 45 phút";
        }

        public class NotificationCode
        {
            public const string REFERRALSUCCESS = "NT008";

            public const string PAYMENTSUCCESS = "NT005";
        }
    }
}
