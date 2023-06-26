using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Generics;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Application.Interfaces.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static F88.Digital.Application.Features.AppPartner.Reporting.Queries.ReportingPaymentDetailsResponse;

namespace F88.Digital.Application.Features.AppPartner.Reporting.Queries
{
    public class ReportingPaymentQuery : IRequest<Result<PaginationSet<GroupUserProfile>>>
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; } = ApiConstants.PagingInfo.PAGE_SIZE;
    }

    public class ReportingPaymentQueryHandler : IRequestHandler<ReportingPaymentQuery, Result<PaginationSet<GroupUserProfile>>>
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        private readonly IMapper _mapper;
        private readonly IApiShareService _apiShareService;
        public ReportingPaymentQueryHandler(IPaymentRepository paymentRepository, IApiShareService apiShareService, IMapper mapper, IUserLoanReferralRepository userLoanReferralRepository, IUserProfileRepository userProfileRepository)
        {
            _paymentRepository = paymentRepository;
            _apiShareService = apiShareService;
            _mapper = mapper;
            _userLoanReferralRepository = userLoanReferralRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<Result<PaginationSet<GroupUserProfile>>> Handle(ReportingPaymentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime fromDate = new DateTime(request.Year, request.Month, 01);
                DateTime toDate = new DateTime(request.Year, request.Month, DateTime.DaysInMonth(request.Year, request.Month));

                var user = await _userLoanReferralRepository.UserLoans
                     .Include(x => x.Deposit)
                     .Include(x => x.UserProfile)
                     .Where(x => x.CreatedOn.Date >= fromDate.Date
                            && x.CreatedOn.Date <= toDate.Date && x.Deposit.Status)
                     .OrderByDescending(x => x.CreatedOn)
                     .ToListAsync();

                var groupUser = user.GroupBy(x => x.UserProfileId)
                                    .Select(s => new GroupUserProfile
                                    {
                                        UserProfileId = s.Key,
                                        RewardMoney = s.Sum(s => s.Deposit.BalanceValue)
                                    }).ToList();

                var items = groupUser.OrderByDescending(x => x.RewardMoney).Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

                var pagination = CommonHelper.SetPagination(items, request.PageNumber, request.PageSize, groupUser.Count());
                
                if (pagination.Items.Count() > 0)
                {
                    foreach (var item in pagination.Items)
                    {
                        var userObj = await _userProfileRepository.GetByUserIdAsync(item.UserProfileId);
                        item.FullName = $"{userObj.LastName} {userObj.FirstName}";
                        item.Phone = userObj.UserPhone;
                        item.Passport = userObj.Passport;

                        var userLoans = await _userLoanReferralRepository.UserLoans
                            .Include(x => x.Deposit)
                            .Include(x => x.UserProfile)
                            .Where(x => x.CreatedOn.Date >= fromDate.Date
                                   && x.CreatedOn.Date <= toDate.Date && x.Deposit.Status && x.UserProfileId == item.UserProfileId)
                            .OrderByDescending(x => x.CreatedOn)
                            .ToListAsync();

                        var userLoanNew = userLoans.Select(x => new ReportingPaymentDetailsResponse
                        {
                            FullName = x.FullName,
                            Phone = x.PhoneNumber,
                            FormGroup = _apiShareService.GetShopsById(x.RefTempGroupId),
                            RandomGroup = _apiShareService.GetShopsById(x.RefRealGroupId),
                            SalesGroup = x.RefContractGroupId != 0 ? _apiShareService.GetShopsById(x.RefContractGroupId) : "",
                            FinalGroup = _apiShareService.GetShopsById(x.RefFinalGroupId),
                            Created = x.CreatedOn,
                            SaleDate = x.RefContractGroupId != 0 ? (x.LastModifiedOn.HasValue ? x.LastModifiedOn.Value : null) : null
                        }).ToList();

                        item.PaymentDetails = userLoanNew;
                    }
                }
                return await Result<PaginationSet<GroupUserProfile>>.SuccessAsync(pagination);
            }
            catch(Exception error)
            {
                return null;
            }    
        }
    }
}
