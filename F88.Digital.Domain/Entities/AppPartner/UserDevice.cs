using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class UserDevice : AuditableEntity
    {
        [Required]
        public string DeviceId { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        [StringLength(100)]
        public string DeviceName { get; set; }

        public string DeviceOS { get; set; }

        public string DeviceBrowserType { get; set; }

        public string DeviceLocation { get; set; }            
    }
}