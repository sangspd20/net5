using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class UserLoanReferral : AuditableEntity
    {
        [Required]
        public int UserProfileId { get; set; }

        [JsonIgnore]
        public virtual UserProfile UserProfile { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Province { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        public int RefTempGroupId { get; set; }

        public int RefRealGroupId { get; set; }

        public int RefFinalGroupId { get; set; }

        public int RefContractGroupId { get; set; }

        public string RefAsset { get; set; }

        public string TransactionId { get; set; }

        public bool IsF88Cus { get; set; }

        public decimal LoanAmount { get; set; }
        public string PolId { get; set; }
        public string PawnId { get; set; }

        // Chờ xử lý: tạo đơn xong, pending hoặc đang approved: 0
        // Thành công: Ra hợp đồng: 2
        // Thất bại: đơn bị cancel : 3
        // Đơn được xác nhận tính tiền: 4
        [Range(-5, 5)]
        public int LoanStatus { get; set; } = 0;

        public virtual Deposit Deposit { get; set; }
    }
}
