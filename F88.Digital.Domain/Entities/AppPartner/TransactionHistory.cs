using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class TransactionHistory : AuditableEntity
    {
        /// <summary>
        /// Mã đối soát gửi tiền từ POL và rút tiền từ kế toán
        /// </summary>
        [Required]
        public string TransactionId { get; set; } = DateTime.Now.ToString("ddMMyyyyHHmmss");

        /// <summary>
        /// Số tiền chi tiết trên từng giao dịch.
        /// </summary>
        public decimal? TransSubTotal { get; set; }

        // Chờ xử lý: tạo đơn xong, pending hoặc đang approved: 0
        // Thành công: Ra hợp đồng: 2
        // Thất bại: đơn bị cancel : 3
        // Đơn được xác nhận tính tiền: 4
        // Hủy thanh toán tiền : 5
        [Range(-5, 5)]
        public int TransFinalStatus { get; set; }

        /// <summary>
        /// 1 : gửi tiền - Deposit
        /// 2 : rút tiền - Withdraw
        /// </summary>
        public int TransactionType { get; set; }

        /// <summary>
        /// Ngày phát sinh giao dịch
        /// </summary>
        public DateTime TransactionDate { get; set; }

        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
