//using AspNetCoreHero.Results;
//using AutoMapper;
//using F88.Digital.Application.Constants;
//using F88.Digital.Application.Extensions;
//using F88.Digital.Application.Interfaces.Repositories.AppPartner;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Queries
//{
//   public class GetListPublishNotificationsQuery : IRequest<Result<List<AppNotificationResponse>>>
//   {
//        public int PageNumber { get; set; }

//        public int PageSize { get; set; } = ApiConstants.PagingInfo.PAGE_SIZE;

//        public class GetListPublishNotificationsQueryHandler : IRequestHandler<GetListPublishNotificationsQuery, Result<List<AppNotificationResponse>>>
//        {
//            private readonly IAppNotificationRepository _appNotificationRepository;
//            private readonly IMapper _mapper;

//            public GetListPublishNotificationsQueryHandler(IAppNotificationRepository appNotificationRepository, IMapper mapper)
//            {
//                _appNotificationRepository = appNotificationRepository;
//                _mapper = mapper;
//            }

//            public async Task<Result<List<AppNotificationResponse>>> Handle(GetListPublishNotificationsQuery request, CancellationToken cancellationToken)
//            {
//                var paginatedUserNotis = await _appNotificationRepository.AppNotifications
//                .Where(x => x.NotiType == ApiConstants.NotificationType.PUBLISH && x.Status)
//                .OrderByDescending(x => x.CreatedOn)
//                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

//                var lstNotification = new List<AppNotificationResponse>();
//                lstNotification = _mapper.Map<List<AppNotificationResponse>>(paginatedUserNotis.Data.ToList());

//                return await Result<List<AppNotificationResponse>>.SuccessAsync(lstNotification);
//            }
//        }
//    }
//}
