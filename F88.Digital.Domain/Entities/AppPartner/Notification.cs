using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class Notification : AuditableEntity
    {
        /// <summary>
        /// MÃ thông báo:
        /// </summary>
        public string NotiTypeCode { get; set; }

        [StringLength(300)]
        public string NotiTitle { get; set; }

        [StringLength(1000)]
        public string NotiDetail { get; set; }

        /// <summary>
        /// Chu kỳ gửi (theo ngày) tính từ ngày bắt đầu
        /// </summary>
        public int NotiPeriod { get; set; }

        public string NotiIconUrl { get; set; }

        public bool Status { get; set; } = true;

        public DateTime NotiDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public int NotiType { get; set; }

        public string NotiSummary { get; set; }

        public int Frequency { get; set; }

        public bool IsCampaign { get; set; }
    }
}

