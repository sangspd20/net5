using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Contract.Query
{
    public class ContractInfoQuery : IRequest<Result<ContractInfoResponse>>
    {
        public string UserPhone { get; set; }

        public class ContractInfoQueryHandler : IRequestHandler<ContractInfoQuery, Result<ContractInfoResponse>>
        {
            private readonly IUserProfileRepository _userProfileRepository;
            private readonly IMapper _mapper;
            public ContractInfoQueryHandler(IUserProfileRepository userProfileRepository, IMapper mapper)
            {
                _userProfileRepository = userProfileRepository;
                _mapper = mapper;
            }

            public async Task<Result<ContractInfoResponse>> Handle(ContractInfoQuery query, CancellationToken cancellationToken)
            {
                #region Get info
                var userProfile = await _userProfileRepository.GetUserInfoIncludeUserBankByUserPhoneAsync(query.UserPhone);
                if (userProfile == null) return await Result<ContractInfoResponse>.FailAsync("Tài khoản không tồn tại hoặc đã bị khóa");
                #endregion

                #region get user info property for contract
                var userProfileInfoContact = new UserProfileContractInfo()
                {
                    FullName = $"{userProfile.LastName} {userProfile.FirstName}",
                    UserPhone = userProfile.UserPhone,
                    PassportDate = userProfile.PassportDate.HasValue ? userProfile.PassportDate.Value.ToString("dd/MM/yyyy") : "Chưa cập nhật",
                    PassportPlace = userProfile.PassportPlace,
                    AccNumber = userProfile.UserBanks.Count > 0 ? userProfile.UserBanks.FirstOrDefault().AccNumber : "Chưa cập nhật",
                    BankName = userProfile.UserBanks.Count > 0 ? userProfile.UserBanks.FirstOrDefault().Bank.Name : "Chưa cập nhật",
                    BankBranch = userProfile.UserBanks.Count > 0 ? userProfile.UserBanks.FirstOrDefault().Branch : "Chưa cập nhật",
                    Passport = userProfile.Passport
                };
                #endregion

                var contractInfoResponse = new ContractInfoResponse()
                {
                    UserPhone = userProfile.UserPhone,
                    ContractDetail = MessageConstants.CONTRACT_RULE
                                                        .Replace("{Day}", DateTime.Now.Day.ToString())
                                                        .Replace("{Month}", DateTime.Now.Month.ToString())
                                                        .Replace("{Year}", DateTime.Now.Year.ToString())
                                                        .Replace("{Month}", DateTime.Now.Month.ToString())
                                                        .Replace("{Fullname}", userProfileInfoContact.FullName.ToUpper())
                                                        .Replace("{Userphone}", userProfileInfoContact.UserPhone)
                                                        .Replace("{Passport}", userProfileInfoContact.Passport)
                                                        .Replace("{Passportdate}", userProfileInfoContact.PassportDate)
                                                        .Replace("{Passportplace}", userProfileInfoContact.PassportPlace)
                                                        .Replace("{Accnumber}", userProfileInfoContact.AccNumber)
                                                        .Replace("{Bankname}", userProfileInfoContact.BankName)
                                                        .Replace("{Bankbranch}", userProfileInfoContact.BankBranch)
                };

                return Result<ContractInfoResponse>.Success(contractInfoResponse);
            }
        }
    }
}
