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
    public class UserAuthToken : AuditableEntity
    {
        [StringLength(100)]
        public string Password  { get; set; }

        [StringLength(1000)]
        public string LoginToken  { get; set; }

        public DateTime? LoginTokenCreatedDate  { get; set; }

        [ForeignKey("Id")]
        public virtual UserProfile UserProfile { get; set; }

        [StringLength(100)]
        public string OTPHash { get; set; }

        [StringLength(1000)]
        public string ResetPasswordToken { get; set; }

    }
}
