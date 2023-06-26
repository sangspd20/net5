using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserBank.Queries
{
    public class GetListUserBankResponse
    {
        public int UserBankId { get; set; }

        public string AccNumber { get; set; }

        public string AccName { get; set; }

        public string Branch { get; set; }

        public bool UserBankStatus { get; set; }

        public int BankId { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public string BankIcon { get; set; }
    }
}
