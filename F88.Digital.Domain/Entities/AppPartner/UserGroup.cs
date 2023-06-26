using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class UserGroup : AuditableEntity
    {
        public int UserProfileId { get; set; }
        public int GroupId { get; set; }
    }
}
