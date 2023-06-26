using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.Features.AppPartner.UserProfile.Queries.GetUserProfile;
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
    public class GetUserProfileDetailQuery : IRequest<Result<UserProfileDetailResponse>>
    {
        public int UserProfileId { get; set; }

        public class GetUserProfileDetailQueryHandler : IRequestHandler<GetUserProfileDetailQuery, Result<UserProfileDetailResponse>>
        {
            private readonly IUserProfileRepository _userProfileRepository;
            private readonly IMapper _mapper;
            private readonly IAWSS3Repository _aWSS3Repository;
            public GetUserProfileDetailQueryHandler(IUserProfileRepository userProfileRepository, IMapper mapper, IAWSS3Repository aWSS3Repository)
            {
                _userProfileRepository = userProfileRepository;
                _mapper = mapper;
                _aWSS3Repository = aWSS3Repository;
            }

            public async Task<Result<UserProfileDetailResponse>> Handle(GetUserProfileDetailQuery query, CancellationToken cancellationToken)
            {
                #region Get info
                var userProfile = await _userProfileRepository.GetByUserIdAsync(query.UserProfileId);
                if (userProfile == null) return await Result<UserProfileDetailResponse>.FailAsync("Tài khoản không tồn tại hoặc đã bị khóa");
                #endregion
                var userProfileResponse = _mapper.Map<UserProfileDetailResponse>(userProfile);
                userProfileResponse.UserBanks = userProfile.UserBanks.Where(x=>x.UserBankStatus).ToList();
                userProfileResponse.IsVerify = false;
                userProfileResponse.AvatarURL = !string.IsNullOrEmpty(userProfile.AvatarURL) ? (userProfile.AvatarURL.Contains("/") ? _aWSS3Repository.PresignURL(userProfile.AvatarURL) : userProfile.AvatarURL) : userProfile.AvatarURL;
                if (!string.IsNullOrEmpty(userProfile.PassportFrontURL)
                    && !string.IsNullOrEmpty(userProfile.PassportBackURL)
                    && !string.IsNullOrEmpty(userProfile.Passport)
                    && userProfile.UserBanks.Any())
                {
                    userProfileResponse.PassportFrontURL = !string.IsNullOrEmpty(userProfile.PassportFrontURL) ? (userProfile.PassportFrontURL.Contains("/") ? _aWSS3Repository.PresignURL(userProfile.PassportFrontURL) : userProfile.PassportFrontURL) : userProfile.PassportFrontURL;
                    userProfileResponse.PassportBackURL = !string.IsNullOrEmpty(userProfile.PassportBackURL) ? (userProfile.PassportBackURL.Contains("/") ? _aWSS3Repository.PresignURL(userProfile.PassportBackURL) : userProfile.PassportBackURL) : userProfile.PassportBackURL;
                    userProfileResponse.IsVerify = true;
                }
                return Result<UserProfileDetailResponse>.Success(userProfileResponse);
            }
        }
    }
}
