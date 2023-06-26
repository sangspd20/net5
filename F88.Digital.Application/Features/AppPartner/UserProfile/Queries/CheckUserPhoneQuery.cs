using AspNetCoreHero.Results;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserProfile.Queries
{
    public class CheckUserPhoneQuery : IRequest<Result<int>>
    {
        public string UserPhone { get; set; }
        public class CheckUserPhoneQueryHandler : IRequestHandler<CheckUserPhoneQuery, Result<int>>
        {
            private readonly IUserProfileRepository _userProfileRepository;

            public CheckUserPhoneQueryHandler(IUserProfileRepository userProfileRepository)
            {
                _userProfileRepository = userProfileRepository;
            }

            public async Task<Result<int>> Handle(CheckUserPhoneQuery query, CancellationToken cancellationToken)
            {            
                #region Check duplicate Phone
                var userProfile = await _userProfileRepository.GetByIdAsync(query.UserPhone);
                if (userProfile != null) return await Result<int>.FailAsync("Số điện thoại đã được đăng ký trước đó");
                #endregion

                return Result<int>.Success(1);
            }
        }
    }
}
