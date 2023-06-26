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
    public class UpdateUserBankCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int UserBankId { get; set; }

        public string AccNumber { get; set; }

        public string AccName { get; set; }

        public string Branch { get; set; }

        public int BankId { get; set; }
    }

    public class UpdateUserBankCommandHandler : IRequestHandler<UpdateUserBankCommand, Result<int>>
    {
        private readonly IUserBankRepository _userBankRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public UpdateUserBankCommandHandler(IUserBankRepository userBankRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userBankRepository = userBankRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateUserBankCommand request, CancellationToken cancellationToken)
        {
            var userBank = await _userBankRepository.GetByIdAsync(request.UserBankId);
            if (userBank == null) return await Result<int>.FailAsync("Tài khoản ngân hàng người dùng không tồn tại");

           userBank.AccName = request.AccName ?? userBank.AccName;
           userBank.AccNumber = request.AccNumber ?? userBank.AccNumber;
           userBank.BankId = request.BankId == 0 ? userBank.BankId : request.BankId;
           userBank.Branch = request.Branch ?? userBank.Branch;

            await _userBankRepository.UpdateAsync(userBank);
            await _unitOfWork.Commit(cancellationToken, request.UserPhone);
            return await Result<int>.SuccessAsync("Thay đổi thông tin ngân hàng thành công!");
        }
    }
}
