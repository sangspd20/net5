using F88.Digital.Domain.Entities.AppPartner;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserProfile.Queries.GetUserProfile
{
    public class UserProfileDetailResponse
    {
        public int Id { get; set; }
        public string UserPhone { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Passport { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? PassportDate { get; set; }

        public string PassportPlace { get; set; }

        public IFormFile AvatarImage { get; set; }

        public string AvatarURL { get; set; }
        public bool Status { get; set; }
        public string PassportFrontURL { get; set; }

        public string PassportBackURL { get; set; }
        public bool IsAgreementConfirmed { get; set; }

        public bool IsActiveUpdate { get; set; } = true;
        public bool IsVerify { get; set; }
        public List<Domain.Entities.AppPartner.UserBank> UserBanks { get; set; }
        public List<Group> Groups { get; set; }
        public List<Menu> Menus { get; set; }
        public string Source { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
