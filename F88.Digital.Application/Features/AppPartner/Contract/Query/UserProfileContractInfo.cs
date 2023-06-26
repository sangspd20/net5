using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Contract.Query
{
    public class UserProfileContractInfo
    {
        public string FullName { get; set; }

        public string UserPhone { get; set; }

        public string Passport { get; set; }

        public string PassportDate { get; set; }

        public string PassportPlace { get; set; }

        public string AccNumber { get; set; }

        public string BankName { get; set; }

        public string BankBranch { get; set; }
    }
}
