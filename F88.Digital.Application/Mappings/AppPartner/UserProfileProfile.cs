using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F88.Digital.Application.Features.AppPartner.UserProfile.Commands.Create;
using F88.Digital.Application.Features.AppPartner.UserProfile.Command.Create;
using F88.Digital.Application.Features.AppPartner.UserProfile.Commands.Update;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.Features.AppPartner.UserBank.Command.Create;
using F88.Digital.Application.Features.AppPartner.UserBank.Queries;
using F88.Digital.Application.Features.AppPartner.UserProfile.Queries.GetUserProfile;
using F88.Digital.Application.Features.AppPartner.UserBank.Queries.GetListBanks;

namespace F88.Digital.Application.Mappings.AppPartner
{
    internal class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<CreateUserProfileCommand, UserProfile>().ReverseMap();
            CreateMap<UserAuthTokenRequestModel, UserAuthToken>().ReverseMap();
            CreateMap<UpdateUserProfileCommand, UserAuthToken>().ReverseMap();
            CreateMap<UserOTPRequest, UserOTP>().ForMember(dest => dest.OTPHash, act => act.MapFrom(src => src.OTP));
            CreateMap<UserProfileResponse, UserProfile>().ReverseMap();
            CreateMap<UserProfile, UserProfileDetailResponse>();

            //User Bank
            CreateMap<CreateUserBankCommand, UserBank>().ReverseMap();
            CreateMap<UserBank, GetListUserBankResponse>()
                .ForMember(dest => dest.BankName, act => act.MapFrom(src => src.Bank.Name))
                .ForMember(dest => dest.BankCode, act => act.MapFrom(src => src.Bank.Code))
                .ForMember(dest => dest.UserBankId, act => act.MapFrom(src => src.Id))
                .AfterMap((s, d) => {
                   d.BankIcon = s.Bank.Icon;
               });

            CreateMap<Bank, BankResponse>()
                .ForMember(dest => dest.BankName, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.IconName, act => act.MapFrom(src => src.Icon))
                .ForMember(dest => dest.BankId, act => act.MapFrom(src => src.Id));
        }
    }
}
