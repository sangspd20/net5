using AutoMapper;
using F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create;
using F88.Digital.Application.Features.AppPartner.NotiApplication.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Mappings.AppPartner
{
    public class NotiApplicationProfile : Profile
    {
        public NotiApplicationProfile()
        {
            CreateMap<Notification, AppNotificationResponse>();
            CreateMap<Notification, CreateNotiAppCommand>().ReverseMap();
            CreateMap<UserNotification, CreateUserNotificationCommand>().ReverseMap();
        }
    }
}
