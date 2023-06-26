using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserBank.Queries.GetListBanks
{
    public class GetListBanksQuery : IRequest<Result<List<BankResponse>>>
    {
        public class GetListBanksQueryHandler : IRequestHandler<GetListBanksQuery, Result<List<BankResponse>>>
        {
            private readonly IUserBankRepository _userBankRepository;
            private readonly IMapper _mapper;

            public GetListBanksQueryHandler(IUserBankRepository userBankRepository, IMapper mapper)
            {
                _userBankRepository = userBankRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<BankResponse>>> Handle(GetListBanksQuery request, CancellationToken cancellationToken)
            {
                var lstBanks = await _userBankRepository.Banks
                    .Where(s => s.Status)
                    .ToListAsync();

                var lstBankResponse = _mapper.Map<List<BankResponse>>(lstBanks);
                return await Result<List<BankResponse>>.SuccessAsync(lstBankResponse);
            }
        }
    }
}
