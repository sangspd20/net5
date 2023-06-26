using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create.CreatePublicNotiUser
{
    public class CreatePublicUserNotificationCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int UserProfileId { get; set; }
    }

    public class CreatePublicUserNotificationCommandHandler : IRequestHandler<CreatePublicUserNotificationCommand, Result<int>>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IAppNotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreatePublicUserNotificationCommandHandler(IUserNotificationRepository userNotificationRepository, 
            IAppNotificationRepository notificationRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _userNotificationRepository = userNotificationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<Result<int>> Handle(CreatePublicUserNotificationCommand request, CancellationToken cancellationToken)
        {
            var notiApp = _mapper.Map<UserNotification>(request);

            //Get list public notifications
            var lstPublicNoti = _notificationRepository.AppNotifications
                                                       .Where(x => x.NotiType == -1 && x.Status)
                                                       .ToList();
            foreach(var pubNoti in lstPublicNoti)
            {
                notiApp.AppNotificationId = pubNoti.Id;
                await _userNotificationRepository.InsertAsync(notiApp);
            }
            
            await _unitOfWork.Commit(cancellationToken, request.UserPhone);
            return await Result<int>.SuccessAsync("Thành công!");
        }
    }
}
