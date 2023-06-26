using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace F88.Digital.Application.Enums
{
    public enum StatusEnum
    {
        [Description("Pending")]
        Pending = 0,

        [Description("Success")]
        Success = 1,

        [Description("Failed")]
        Failed = 2
    }

    public enum GoogleSheetStatus
    {
        [Description("Thành công")]
        Success = 1,
        [Description("Thất bại")]
        Failed = -1,
    }
    public enum GoogleSheetReadStatus
    {
        [Description("Đã quét")]
        IsRead = 1,
        [Description("Chưa quét")]
        UnRead = 0,
    }

    public static class TypeSendOtp
    {
        public static string Register = "Register";
        public static string ForgetPassword = "ForgetPassword";
    }
}
