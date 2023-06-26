using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class Menu : AuditableEntity
    {
        public string Name { get; set; }
        public string Route { get; set; }
    }
}
