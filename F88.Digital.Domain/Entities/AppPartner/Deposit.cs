using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class Deposit : AuditableEntity
    {

        [ForeignKey("Id")]
        public virtual UserLoanReferral UserLoanReferral { get; set; }

        public int UserProfileId { get; set; }

        public decimal? BalanceValue { get; set; } = 0;

        public bool Status { get; set; } = false;

        public string Notes { get; set; }
    }
}

