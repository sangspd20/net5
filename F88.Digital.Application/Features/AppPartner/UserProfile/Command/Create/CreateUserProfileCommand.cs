using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using UserEntity = F88.Digital.Domain.Entities.AppPartner.UserProfile;
using F88.Digital.Application.Features.AppPartner.UserProfile.Command.Create;
using F88.Digital.Application.Helpers;
using System.Linq;
using F88.Digital.Domain.Entities.AppPartner;
using System.Collections.Generic;

namespace F88.Digital.Application.Features.AppPartner.UserProfile.Commands.Create
{
    public partial class CreateUserProfileCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int WarningCount { get; set; } = 0;

        public bool Status { get; set; } = true;

        public UserAuthTokenRequestModel UserAuthToken { get; set; }
    }

    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, Result<int>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IAppNotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateUserProfileCommandHandler(IUserProfileRepository userProfileRepository,
            IUserNotificationRepository userNotificationRepository,
            IAppNotificationRepository notificationRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = EncryptHelper.HashPassword(request.UserAuthToken.Password);
            request.UserAuthToken.Password = passwordHash;

            var userProfile = _mapper.Map<UserEntity>(request);
            await _userProfileRepository.InsertAsync(userProfile);
            var result = await _unitOfWork.Commit(cancellationToken, request.UserPhone);

            if (result > -1)
            {
                #region Set public notification for user               
                //Get list public notifications
                var lstPublicNoti = _notificationRepository.AppNotifications
                                                           .Where(x => x.NotiType == -1 && x.Status)
                                                           .ToList();

                var lstUserNoti = new List<UserNotification>(); 

                foreach (var pubNoti in lstPublicNoti)
                {
                    var notiApp = new UserNotification()
                    {
                        UserProfileId = userProfile.Id,
                        AppNotificationId = pubNoti.Id
                    };

                    lstUserNoti.Add(notiApp);
                }

                await _userNotificationRepository.InsertRangeAsync(lstUserNoti);
                await _unitOfWork.Commit(cancellationToken, request.UserPhone);

                return await Result<int>.SuccessAsync("Đăng ký thành công!");
                #endregion
            }

            return await Result<int>.FailAsync("Đăng ký thất bại!");
        }
    }
}