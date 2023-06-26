using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class GroupMenu : AuditableEntity
    {
        public int MenuId { get; set; }
        public int GroupId { get; set; }
    }
}
