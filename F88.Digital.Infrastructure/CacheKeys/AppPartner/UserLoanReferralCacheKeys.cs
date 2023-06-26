using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.CacheKeys.AppPartner
{
    public class UserLoanReferralCacheKeys
    {
        public static string GetKey(int userLoanRef) => $"UserLoanReferral-{userLoanRef}";

        public static string GetListUserLoanKey(int userLoanRef) => $"ListUserLoanKey-{userLoanRef}";
    }
}
