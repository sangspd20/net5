using AutoMapper;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command.Update;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries;
using F88.Digital.Domain.Entities.AppPartner;

namespace F88.Digital.Application.Mappings.AppPartner
{
    public class UserLoanReferralProfile : Profile
    {
        public UserLoanReferralProfile()
        {
            CreateMap<CreateUserLoanRefCommand, UserLoanReferral>().ReverseMap();
            CreateMap<CreateDepositRequest, Deposit>().ReverseMap();
            CreateMap<UpdateUserLoanRefCommand, UserLoanReferral>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.TransactionId));
            CreateMap<UpdateDepositRequest, Deposit>();

            CreateMap<UserLoanReferral, UserLoanRefResponse>()
                .AfterMap((s, d) => {
                    d.RewardMoney = s.Deposit.BalanceValue;
            });
        }
    }
}
