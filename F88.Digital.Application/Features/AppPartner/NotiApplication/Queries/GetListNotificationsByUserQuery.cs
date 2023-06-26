using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Queries
{
    public class GetListNotificationsByUserQuery : IRequest<Result<List<UserNotificationResponse>>>
    {
        public int userProfileId { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; } = ApiConstants.PagingInfo.PAGE_SIZE;

        public class GetListNotificationsByUserQueryHandler : IRequestHandler<GetListNotificationsByUserQuery, Result<List<UserNotificationResponse>>>
        {
            private readonly IUserNotificationRepository _appNotificationRepository;
            private readonly IMapper _mapper;


            public GetListNotificationsByUserQueryHandler(IUserNotificationRepository appNotificationRepository, IMapper mapper)
            {
                _appNotificationRepository = appNotificationRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<UserNotificationResponse>>> Handle(GetListNotificationsByUserQuery request, CancellationToken cancellationToken)
            {
                var paginatedUserNotis = await _appNotificationRepository.UserNotifications
                .Include(i => i.AppNotification)
                .Where(x => x.UserProfileId == request.userProfileId && x.AppNotification.Status)
                .OrderByDescending(x => x.CreatedOn)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                var lstNotification = new List<UserNotificationResponse>();

                foreach(var item in paginatedUserNotis.Data)
                {
                    var notification = new UserNotificationResponse()
                    {
                        UserProfileId = item.UserProfileId,
                        NotiTitle = item.AppNotification.NotiTitle,
                        NotiDetail = item.AppNotification.NotiDetail,
                        NotiIconUrl = item.AppNotification.NotiIconUrl,
                        PaymentId = item.PaymentId.HasValue ? item.PaymentId.Value : 0,
                        NotiType = item.AppNotification.NotiType,
                        NotiStartDate = item.AppNotification.NotiDate,     
                        NotiTypeCode = item.AppNotification.NotiTypeCode
                    };

                    lstNotification.Add(notification);
                }
                
                return await Result<List<UserNotificationResponse>>.SuccessAsync(lstNotification);
            }
        }
    }
}
