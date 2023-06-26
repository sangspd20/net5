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
    public class GetAllUserProfileQuery : IRequest<Result<PaginationSet<UserProfileDetailResponse>>>
    {
        public string Search { get; set; }
        public string FullName { get; set; }
        public int? GroupId { get; set; }
        public string Source { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageNumber { get; set; }
        public int? PageSize { get; set; }
        public class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfileQuery, Result<PaginationSet<UserProfileDetailResponse>>>
        {
            private readonly IUserProfileRepository _userProfileRepository;
            private readonly IGroupMenuRepository _groupMenuRepository;
            private readonly IMapper _mapper;
            private readonly IAWSS3Repository _aWSS3Repository;
            public GetAllUserProfileQueryHandler(IUserProfileRepository userProfileRepository, IGroupMenuRepository groupMenuRepository, IMapper mapper, IAWSS3Repository aWSS3Repository)
            {
                _userProfileRepository = userProfileRepository;
                _groupMenuRepository = groupMenuRepository;
                _mapper = mapper;
                _aWSS3Repository = aWSS3Repository;
            }

            public async Task<Result<PaginationSet<UserProfileDetailResponse>>> Handle(GetAllUserProfileQuery query, CancellationToken cancellationToken)
            {
                var pageSize = query.PageSize ?? 20;
                var userProfiles = _userProfileRepository.UserProfiles.AsEnumerable();
                if (!string.IsNullOrEmpty(query.Search))
                {
                    userProfiles = userProfiles.Where(x => x.Id.ToString().Trim() == query.Search.Trim() || x.UserPhone.Trim().Contains(query.Search.Trim()));
                };
                if (!string.IsNullOrEmpty(query.FullName))
                {
                    userProfiles = userProfiles.Where(x => StringUtils.RemoveSign4VietnameseString((x.LastName + x.FirstName).Replace(" ","").ToLower()).Contains(StringUtils.RemoveSign4VietnameseString(query.FullName.Replace(" ", "").ToLower())));
                };
                if (!string.IsNullOrEmpty(query.Source))
                {
                    userProfiles = userProfiles.Where(x => (!string.IsNullOrEmpty(x.Source)  && x.Source.Equals(query.Source)));
                };
                if (!string.IsNullOrEmpty(query.FromDate) && !string.IsNullOrEmpty(query.ToDate))
                {
                    var formatFromDate = StringUtils.FormatDateTime(query.FromDate, "dd-MM-yyyy");
                    var formatToDate = StringUtils.FormatDateTime(query.ToDate, "dd-MM-yyyy");
                    userProfiles = userProfiles.Where(x => x.CreatedOn.Date >= formatFromDate.Date && x.CreatedOn.Date <= formatToDate.Date);
                };
                var mapper = userProfiles.Select(x => new UserProfileDetailResponse
                {
                    Id = x.Id,
                    UserPhone = x.UserPhone,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Passport = x.Passport,
                    PassportDate = x.PassportDate,
                    PassportPlace = x.PassportPlace,
                    AvatarURL = !string.IsNullOrEmpty(x.AvatarURL) ? (x.AvatarURL.Contains("/") ? _aWSS3Repository.PresignURL(x.AvatarURL) : x.AvatarURL) : x.AvatarURL,
                    Status = x.Status,
                    PassportFrontURL = !string.IsNullOrEmpty(x.PassportFrontURL) ? (x.PassportFrontURL.Contains("/") ? _aWSS3Repository.PresignURL(x.PassportFrontURL) : x.PassportFrontURL) : x.PassportFrontURL,
                    PassportBackURL = !string.IsNullOrEmpty(x.PassportBackURL) ? (x.PassportBackURL.Contains("/") ? _aWSS3Repository.PresignURL(x.PassportBackURL) : x.PassportBackURL) : x.PassportBackURL,
                    IsAgreementConfirmed = x.IsAgreementConfirmed,
                    IsActiveUpdate = x.IsActiveUpdate,
                    UserBanks = x.UserBanks,
                    Source = x.Source,
                    CreatedOn= x.CreatedOn,
                }).ToList();
                if (query.GroupId != null)
                {
                    mapper = mapper.Where(x => x.Groups.FirstOrDefault()?.Id == query.GroupId).ToList();
                };
                var items = mapper.Skip((query.PageNumber - 1) * pageSize).Take(pageSize);

                foreach (var item in items)
                {
                    item.Groups = _groupMenuRepository.GetListGroupMenuByUserId(item.Id).Groups;
                }
                var pagination = CommonHelper.SetPagination(items, query.PageNumber, pageSize, mapper.Count());
                return await Result<PaginationSet<UserProfileDetailResponse>>.SuccessAsync(pagination);
            }
        }
    }
}
