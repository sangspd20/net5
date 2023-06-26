using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class UserOTP : AuditableEntity
    {
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string UserPhone { get; set; }

        [Required]
        public string OTPHash { get; set; }

        [Required]
        public string DeviceId { get; set; }
    }
}
