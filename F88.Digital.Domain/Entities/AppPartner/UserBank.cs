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
    public class UserBank : AuditableEntity
    {
        [Required]
        [StringLength(25)]
        public string AccNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string AccName{ get; set; }

        [StringLength(500)]
        [Column(Order = 3)]
        public string Branch { get; set; }

        public bool UserBankStatus { get; set; }

        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public int BankId { get; set; }

        public virtual Bank Bank { get; set; }

    }
}
