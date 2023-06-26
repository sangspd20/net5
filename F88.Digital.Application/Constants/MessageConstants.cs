using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Constants
{
    public static class MessageConstants
    {
        public const string Success_Response = "This operation has been completed successfully";
        public const string Error_Response = "This operation could not be completed";
        public const string Not_Found_Error_Response = "This item could not be found";
        public const string Internal_Error = "Server error. Please contact admin to support";
        public const string Forbidden_Error = "You don't have permission to perform this action";
        public const string LockAccount_Error = "Chúng tôi phát hiện bất thường trong các hoạt động của tài khoản. Tài khoản đã bị khóa tạm thời. Vui lòng liên hệ Phòng Giao Dịch tạo hợp đồng hoặc bộ phận CSKH của F88 để được hỗ trợ.";

        public const string CONTRACT_RULE = @"";
    }
}
