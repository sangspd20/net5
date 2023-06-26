using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetPaymentPartnership
    {
        public string PartnerPhoneNumber { get; set; }
        public string PartnerFullName { get; set; }
        public string Source { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string PolId { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
