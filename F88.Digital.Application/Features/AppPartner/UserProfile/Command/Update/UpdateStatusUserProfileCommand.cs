using AspNetCoreHero.Results;
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
    public class UpdateStatusUserProfileCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Source { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateStatusUserProfileCommandHandler : IRequestHandler<UpdateStatusUserProfileCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileRepository _userProfileRepository;
        public UpdateStatusUserProfileCommandHandler(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
        }
        public async Task<Result<int>> Handle(UpdateStatusUserProfileCommand command, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(command.UserPhone);
            if (userProfile == null)
            {
                return Result<int>.Fail($"User không tồn tại");
            }
            userProfile.Status = command.Status;
            userProfile.Source = command.Source ?? userProfile.Source;
            await _userProfileRepository.UpdateAsync(userProfile);
            var result = await _unitOfWork.Commit(cancellationToken, command.UserPhone);
            if (result > 0) return await Result<int>.SuccessAsync("Cập nhật thành công!");
            return await Result<int>.FailAsync("Cập nhật không thành công!");
        }
    }
}
