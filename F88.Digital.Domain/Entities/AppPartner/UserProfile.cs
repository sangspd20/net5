using AspNetCoreHero.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class UserProfile : AuditableEntity
    {
        [Required]
        [StringLength(10, MinimumLength = 3)]       
        public string UserPhone { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public int WarningCount  { get; set; }


        [StringLength(12)]       
        public string Passport  { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? PassportDate { get; set; }

        [StringLength(255)]
        public string PassportPlace { get; set; }

        [StringLength(200)]
        public string AvatarURL  { get; set; }

        [StringLength(200)]
        public string PassportFrontURL  { get; set; }

        [StringLength(200)]  
        public string PassportBackURL  { get; set; }

        public bool Status { get; set; }

        public string Notes { get; set; }

        public bool IsAgreementConfirmed { get; set; }

        public bool IsActiveUpdate { get; set; }

        public virtual UserAuthToken UserAuthToken { get; set; }
        public string Source { set; get; }
        #region --Extension--
        public List<UserLoanReferral> UserLoanReferrals { get; set; }

        public List<TransactionHistory> TransactionHistorys { get; set; }

        public List<UserBank> UserBanks { get; set; }

        public List<Notification> AppNotifications { get; set; }
        #endregion

    }
}
