using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserBank.Command.Update
{
    public class ActiveUserBankCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int UserBankId { get; set; }

        public int UserProfileId { get; set; }
    }

    public class ActiveUserBankCommandHandler : IRequestHandler<ActiveUserBankCommand, Result<int>>
    {
        private readonly IUserBankRepository _userBankRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public ActiveUserBankCommandHandler(IUserBankRepository userBankRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userBankRepository = userBankRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(ActiveUserBankCommand request, CancellationToken cancellationToken)
        {
            var userBank = await _userBankRepository.GetByIdAsync(request.UserBankId);
            if (userBank == null) return await Result<int>.FailAsync("Tài khoản ngân hàng người dùng không tồn tại");

            userBank.UserBankStatus = true;

            await _userBankRepository.ActiveUserBankAsync(request.UserBankId, request.UserProfileId, userBank);
            var result = _unitOfWork.Commit(cancellationToken, request.UserPhone);

            if(result.GetAwaiter().GetResult() > 0)
            return await Result<int>.SuccessAsync("Cập nhật thành công!");
            
            return await Result<int>.FailAsync("Cập nhật khôngthành công!");

        }
    }
}
