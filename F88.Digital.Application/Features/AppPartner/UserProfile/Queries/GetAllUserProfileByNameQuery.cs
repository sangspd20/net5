using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Features.AppPartner.UserProfile.Queries.GetUserProfile;
using F88.Digital.Application.Generics;
using F88.Digital.Application.Helpers;
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
    public class GetAllUserProfileByNameQuery : IRequest<Result<List<UserProfileDetailResponse>>>
    {      
        public string FullName { get; set; }

        public class GetAllUserProfileByNameQueryHandler : IRequestHandler<GetAllUserProfileByNameQuery, Result<List<UserProfileDetailResponse>>>
        {
            private readonly IUserProfileRepository _userProfileRepository;
            private readonly IMapper _mapper;
            public GetAllUserProfileByNameQueryHandler(IUserProfileRepository userProfileRepository, IMapper mapper)
            {
                _userProfileRepository = userProfileRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<UserProfileDetailResponse>>> Handle(GetAllUserProfileByNameQuery query, CancellationToken cancellationToken)
            {
                var userProfiles = _userProfileRepository.UserProfiles.AsEnumerable();
                if (!string.IsNullOrEmpty(query.FullName))
                {
                    userProfiles = userProfiles.Where(x => StringUtils.RemoveSign4VietnameseString((x.LastName + x.FirstName).Replace(" ","").ToLower()).Contains(StringUtils.RemoveSign4VietnameseString(query.FullName.Replace(" ", "").ToLower())));
                };
                var mapper = _mapper.Map<List<UserProfileDetailResponse>>(userProfiles);
                return await Result<List<UserProfileDetailResponse>>.SuccessAsync(mapper);
            }
        }
    }
}
