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

namespace F88.Digital.Application.Features.AppPartner.UserBank.Queries
{
    public class GetListUserBankQuery : IRequest<Result<List<GetListUserBankResponse>>>
    {
        public int userProfileId { get; set; }

        public class GetListUserBankQueryHandler : IRequestHandler<GetListUserBankQuery, Result<List<GetListUserBankResponse>>>
        {
            private readonly IUserBankRepository _userBankRepository;
            private readonly IMapper _mapper;

            public GetListUserBankQueryHandler(IUserBankRepository userBankRepository, IMapper mapper)
            {
                _userBankRepository = userBankRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<GetListUserBankResponse>>> Handle(GetListUserBankQuery query, CancellationToken cancellationToken)
            {
                var userBankLst = await _userBankRepository.GetListBanksByUserAsync(query.userProfileId);
                var mappedUserBanks = _mapper.Map<List<GetListUserBankResponse>>(userBankLst);

                return Result<List<GetListUserBankResponse>>.Success(mappedUserBanks);
            }
        }
    }
}
