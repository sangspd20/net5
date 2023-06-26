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

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Command.Create
{
    public class CreateUserNotificationCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int UserProfileId { get; set; }

        public int AppNotificationId { get; set; }

        public int? PaymentId { get; set; }
    }

    public class CreateUserNotificationCommandHandler : IRequestHandler<CreateUserNotificationCommand, Result<int>>
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateUserNotificationCommandHandler(IUserNotificationRepository userNotificationRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _userNotificationRepository = userNotificationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateUserNotificationCommand request, CancellationToken cancellationToken)
        {
            var notiApp = _mapper.Map<UserNotification>(request);

            await _userNotificationRepository.InsertAsync(notiApp);
            await _unitOfWork.Commit(cancellationToken, request.UserPhone);
            return await Result<int>.SuccessAsync("Thành công!");
        }
    }
}
