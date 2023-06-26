using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserBank.Queries.GetListBanks
{
    public class BankResponse
    {
        public int BankId { get; set; }

        public string Code { get; set; }

        public string BankName { get; set; }

        public string IconName { get; set; }
    }
}
