using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Features.AppPartner.UserProfile.Command.Create;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserProfile.Command.Update
{
    public class ResetPasswordCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<int>>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public ResetPasswordCommandHandler(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(request.UserPhone);
            if (userProfile == null) return await Result<int>.FailAsync("Số điện thoại không tồn tại trong hệ thống");

            var passwordHash = EncryptHelper.HashPassword(request.NewPassword);
            userProfile.UserAuthToken.Password = passwordHash;

            await _userProfileRepository.UpdateAsync(userProfile);
            await _unitOfWork.Commit(cancellationToken, request.UserPhone);
            return await Result<int>.SuccessAsync("Thay đổi mật khẩu thành công!");
        }
    }
}
