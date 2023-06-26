using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Queries
{
    public class NotiApplicationQuery : IRequest<Result<List<PushNotificationModel>>>
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public class NotiApplicationQueryHandler : IRequestHandler<NotiApplicationQuery, Result<List<PushNotificationModel>>>
        {
            private readonly IAppNotificationRepository _appNotificationRepository;
            private readonly IMapper _mapper;

            public NotiApplicationQueryHandler(IAppNotificationRepository appNotificationRepository, IMapper mapper)
            {
                _appNotificationRepository = appNotificationRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<PushNotificationModel>>> Handle(NotiApplicationQuery query, CancellationToken cancellationToken)
            {
                var lstPublicNotifications = await _appNotificationRepository.GetPublicNotificationsByDate(query.FromDate, query.ToDate);
                var mappedLstPublicNotifications = _mapper.Map<List<UserNotificationResponse>>(lstPublicNotifications);

                var result = new List<PushNotificationModel>();

                foreach(var notiItem in mappedLstPublicNotifications)
                {
                    var bodyMsg = new
                    {
                        type = notiItem.NotiTypeCode,
                        title = notiItem.NotiTitle,
                        details = notiItem.NotiDetail,
                        date = notiItem.Date,
                        time = notiItem.Time,
                        iconUrl = notiItem.NotiIconUrl
                    };

                    var resultPush = PushNotificationExtension.SendNotification("NotificationPublish", JsonConvert.SerializeObject(bodyMsg));

                    result.Add(new PushNotificationModel() {
                        Result = resultPush,
                        Data = notiItem
                    });
                }

                return Result<List<PushNotificationModel>>.Success(result);
            }
        }
    }
}
