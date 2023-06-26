using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner
{
    public class UserOtpResult
    {
        public class BrandNameOtpResult
        {
            /// <summary>
            /// Id của tin brandname gửi đi.
            /// </summary>
            public int MessageId { get; set; }

            /// <summary>
            /// Số điện thoại nhận tin nhắn.
            /// </summary>
            public string Phone { get; set; }

            /// <summary>
            /// Tên Brandname.
            /// </summary>
            public string BrandName { get; set; }

            /// <summary>
            /// Nội dung tin nhắn.
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Nhà mạng.
            /// </summary>
            public string Telco { get; set; }

            /// <summary>
            /// Mã đối tác.
            /// </summary>
            public string PartnerId { get; set; }

            /// <summary>
            /// Trạng thái gửi tin nhắn thành công hay không
            /// </summary>
            public bool IsSent
            {
                get
                {
                    if (Error > 0) return false;
                    return true;
                }
            }

            /// <summary>
            /// Trạng thái gửi tin nhắn thành công hay không
            /// </summary>
            public int? Error { get; set; }

            /// <summary>
            /// Trạng thái gửi tin nhắn thành công hay không
            /// </summary>
            public string Error_Description { get; set; }
        }
    }
}
