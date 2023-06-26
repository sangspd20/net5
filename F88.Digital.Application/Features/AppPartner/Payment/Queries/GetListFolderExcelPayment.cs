using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetListFolderExcelPayment
    {
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string CreateDate { get; set; }
        public int FileSize { get; set; }
    }
}
