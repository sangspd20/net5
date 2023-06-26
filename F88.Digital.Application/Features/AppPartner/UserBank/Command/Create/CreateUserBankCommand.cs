using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Entity = F88.Digital.Domain.Entities.AppPartner.UserBank;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserBank.Command.Create
{
    public partial class CreateUserBankCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public string AccNumber { get; set; }

        public string AccName { get; set; }

        public string Branch { get; set; }

        public bool UserBankStatus { get; set; }

        public int UserProfileId { get; set; }

        public int BankId { get; set; }
    }

    public class CreateUserBankCommandHandler : IRequestHandler<CreateUserBankCommand, Result<int>>
    {
        private readonly IUserBankRepository _userBankRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateUserBankCommandHandler(IUserBankRepository userBankRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userBankRepository = userBankRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateUserBankCommand request, CancellationToken cancellationToken)
        {
            var isExistAccNumber = await _userBankRepository.IsExistAccNumberAsync(request.AccNumber);
            if(isExistAccNumber) return await Result<int>.FailAsync("Tài khoản ngân hàng đã tồn tại!");

            var userBank = _mapper.Map<Entity>(request);
            await _userBankRepository.InsertAsync(userBank);
            await _unitOfWork.Commit(cancellationToken, request.UserPhone);
            return await Result<int>.SuccessAsync("Thành công!");
        }
    }

}
