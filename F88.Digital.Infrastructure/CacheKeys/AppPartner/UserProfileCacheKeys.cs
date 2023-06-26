using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.CacheKeys.AppPartner
{
    public static class UserProfileCacheKeys
    {
        public static string ListKey => "UserProfileList";

        public static string SelectListKey => "UserProfileSelectList";

        public static string GetKey(int userProfileId) => $"UserProfile-{userProfileId}";

        public static string GetDetailsKey(int userProfileId) => $"userProfileDetails-{userProfileId}";

        public static string GetKeyUserPhone(string userPhone) => $"userProfilePhone-{userPhone}";

        public static string GetKeyUserBank(int userProfileId) => $"userBank-{userProfileId}";

        public static string GetKeyUserBankId(int userBankId) => $"userBankId-{userBankId}";

        public static string GetKeyUserBankAccNumber(string accNumber) => $"userBankAccnumber-{accNumber}";
    }
}
