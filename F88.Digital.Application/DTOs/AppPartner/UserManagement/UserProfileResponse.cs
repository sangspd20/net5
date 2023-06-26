using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner
{
   public class UserProfileResponse
    {
        public int Id { get; set; }

        public string UserPhone { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Passport { get; set; }

        public DateTime? PassportDate { get; set; }

        public string PassportPlace { get; set; }

        public string AvatarURL { get; set; }

        public string PassportFrontURL { get; set; }

        public string PassportBackURL { get; set; }
        public bool IsAgreementConfirmed { get; set; }

        public string AccessToken { get; set; }
        public string Source { set; get; }
        public bool isVerify { get; set; }
        public List<Group> Groups { get; set; }
        public List<Menu> Menus { get; set; }
        public List<UserBank> UserBanks { get; set; }
    }
}
