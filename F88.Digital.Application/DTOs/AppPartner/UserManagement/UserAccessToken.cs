using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner
{
    public class UserAccessToken
    {
        public string AccessToken { get; set; }

        public int ExpiredToken { get; set; }
    }
}
