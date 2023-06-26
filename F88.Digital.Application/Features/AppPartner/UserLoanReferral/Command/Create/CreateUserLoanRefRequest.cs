using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command
{
    public class CreateUserLoanRefRequest : BaseRequestModel
    {
        public int UserProfileId { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public int RefTempGroupId { get; set; }

        public int RefRealGroupId { get; set; }

        public int RefContractGroupId { get; set; }

        public string RefAsset { get; set; }
        public string RegionID { get; set; }
    }
}
