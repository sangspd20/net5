using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Commands.Delete
{
    public class DeleteUserBankCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteUserBankCommandHandler : IRequestHandler<DeleteUserBankCommand, Result<int>>
        {
            private readonly IUserBankRepository _userBankRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteUserBankCommandHandler(IUserBankRepository userBankRepository, IUnitOfWork unitOfWork)
            {
                _userBankRepository = userBankRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteUserBankCommand command, CancellationToken cancellationToken)
            {
                var userBank = await _userBankRepository.GetByIdAsync(command.Id);
                await _userBankRepository.DeleteAsync(userBank);
                await _unitOfWork.Commit(cancellationToken, command.UserPhone);
                return Result<int>.Success("Thành công");
            }
        }
    }
}