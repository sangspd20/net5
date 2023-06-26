using F88.Digital.Application.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Reporting.Queries
{
    public class ReportingSaleResponse
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public Decimal TotalFormByMonth { get; set; }

        public Decimal TotalSaleByMonth { get; set; }

        public string F2SByMonth { get; set; }

        public Decimal TotalFormByDay { get; set; }

        public Decimal TotalSaleByDay { get; set; }

        public string F2SByDay { get; set; }

        public int TotalCount { get; set; }

        public decimal F2SByMonthOrderByDesc { get; set; }
    }
}
