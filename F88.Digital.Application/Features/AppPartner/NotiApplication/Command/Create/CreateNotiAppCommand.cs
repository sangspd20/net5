using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Entity = F88.Digital.Domain.Entities.AppPartner.UserBank;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using F88.Digital.Domain.Entities.AppPartner;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using F88.Digital.Application.Helpers;
using Newtonsoft.Json;
using F88.Digital.Application.Extensions;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create
{
    public partial class CreateNotiAppCommand : BaseRequestModel, IRequest<Result<int>>
    {
        /// <summary>
        /// Loại thông báo:
        /// -1 - chung
        ///  1 - riêng
        /// </summary>
        public int NotiType { get; set; }

        public string NotiTitle { get; set; }

        public string NotiDetail { get; set; }

        /// <summary>
        /// Chu kỳ gửi (theo ngày) tính từ ngày bắt đầu
        /// </summary>
        public int NotiPeriod { get; set; }

        public IFormFile NotiIconImage { get; set; }

        public string NotiIconUrl { get; set; }

        public string NotiTypeCode { get; set; }

        public string NotiSummary { get; set; }

        public int Frequency { get; set; }

        public bool IsCampaign { get; set; } = false;

        public DateTime NotiDate { get; set; }
     
    }

    public class CreateNotiAppCommandHandler : IRequestHandler<CreateNotiAppCommand, Result<int>>
    {
        private readonly IAppNotificationRepository _appNotificationRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IMapper _mapper;
        private readonly IAWSS3Repository _aWSS3Repository;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateNotiAppCommandHandler(IAppNotificationRepository appNotificationRepository,
            IUserNotificationRepository userNotificationRepository,
            IUserProfileRepository userProfileRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IAWSS3Repository aWSS3Repository)
        {
            _appNotificationRepository = appNotificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
            _aWSS3Repository = aWSS3Repository;
        }

        public async Task<Result<int>> Handle(CreateNotiAppCommand request, CancellationToken cancellationToken)
        {
            var notiApp = _mapper.Map<Notification>(request);

            #region Save NotiIcon Image
            string notiIconName = string.Empty;


            if (request.NotiIconImage == null && !string.IsNullOrEmpty(request.NotiIconUrl))
                return await Result<int>.FailAsync("Cập nhật không thành công!");


            if (request.NotiIconImage != null && !string.IsNullOrEmpty(request.NotiIconUrl))
            {
                //CommonHelper.UploadImageAsync(request.NotiIconImage, "Upload\\NotiIcon", out notiIconName);
                notiIconName = await _aWSS3Repository.UploadFile(request.NotiIconImage, "NotiIcon");
                notiApp.NotiIconUrl = notiIconName;
            }
            #endregion

            await _appNotificationRepository.InsertAsync(notiApp);
            var result = await _unitOfWork.Commit(cancellationToken, request.UserPhone);
          
            if (result > 0)
            {
                var bodyMsg = new
                {
                    type = notiApp.NotiTypeCode,
                    title = notiApp.NotiTitle,
                    details = notiApp.NotiDetail,
                    //date = notiApp.Date,
                    //time = notiApp.Time,
                    iconUrl = notiApp.NotiIconUrl
                };

                var resultPush = PushNotificationExtension.SendNotification(notiApp.NotiTitle, notiApp.NotiSummary);

                if (request.NotiType == -1)
                {
                    #region Set noti to List User
                    var lstUser = _userProfileRepository.UserProfiles
                                                        .Where(x => x.Status)
                                                        .ToList();

                    var lstUserNoti = new List<UserNotification>();

                    foreach (var user in lstUser)
                    {
                        var userNotiApp = new UserNotification()
                        {
                            UserProfileId = user.Id,
                            AppNotificationId = notiApp.Id
                        };

                        lstUserNoti.Add(userNotiApp);
                    }

                    await _userNotificationRepository.InsertRangeAsync(lstUserNoti);
                    await _unitOfWork.Commit(cancellationToken, request.UserPhone);

                    // Push noti
                    PushNotificationExtension.SendNotification(notiApp.NotiTitle, notiApp.NotiSummary);

                    #endregion
                }

                return await Result<int>.SuccessAsync("Thành công!");
            }

            return await Result<int>.FailAsync("Thất bại!");
        }
    }
}
