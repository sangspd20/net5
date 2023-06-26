using AspNetCoreHero.Abstractions.Domain;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class UserNotification : AuditableEntity
    {
        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public int AppNotificationId { get; set; }

        public virtual Notification AppNotification { get; set; }

        public int? PaymentId { get; set; }

        public virtual Payment Payment { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
