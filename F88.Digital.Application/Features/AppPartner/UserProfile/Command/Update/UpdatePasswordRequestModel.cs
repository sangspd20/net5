using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserProfile.Command.Update
{
    public class UpdatePasswordRequestModel 
    {
        public string UserPhone { get; set; }

        public string CurrentPassword { get; set; }

        public string Password { get; set; }
    }
}
