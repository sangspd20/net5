using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Entity = F88.Digital.Domain.Entities.AppPartner.UserLoanReferral;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class FilterUserLoanRefByDateQuery : IRequest<Result<SummaryUserLoanRefResponse>>
    {
        public int UserProfileId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ToDate { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; } = ApiConstants.PagingInfo.PAGE_SIZE;
    }

    public class FilterUserLoanRefByDateQueryHandler : IRequestHandler<FilterUserLoanRefByDateQuery, Result<SummaryUserLoanRefResponse>>
    {
        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly IMapper _mapper;

        public FilterUserLoanRefByDateQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
        {
            _userLoanReferralRepository = userLoanReferralRepository;
            _mapper = mapper;
        }

        public async Task<Result<SummaryUserLoanRefResponse>> Handle(FilterUserLoanRefByDateQuery request, CancellationToken cancellationToken)
        {
            var paginatedUserLoans = await _userLoanReferralRepository.UserLoans
                .Where(x => x.CreatedOn.Date >= request.FromDate.Date 
                            && x.CreatedOn.Date <= request.ToDate.Date 
                            && x.UserProfileId == request.UserProfileId)
                .OrderByDescending(x => x.CreatedOn)
                .Include(x => x.Deposit)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            var lstUserLoanGroupByDate = paginatedUserLoans.Data
                .GroupBy(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
                .Select(s => new UserLoanRefGroupByDateResponse
                 {
                     CreatedOn = s.Key.Substring(0,5),
                     UserLoanRefResponses = _mapper.Map<List<UserLoanRefResponse>>(s.ToList())
                 })
                .ToList();


            var numberOfPending = paginatedUserLoans.Data.Count(x => x.LoanStatus == ApiConstants.LoanStatus.PENDING || x.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM);
            var numberOfApproved = paginatedUserLoans.Data.Count(x => x.LoanStatus == ApiConstants.LoanStatus.APPROVED);
            var numberOfCancel = paginatedUserLoans.Data.Count(x => x.LoanStatus == ApiConstants.LoanStatus.CANCEL);

            var summaryUserLoanResponse = new SummaryUserLoanRefResponse
            {
                NumberOfPending = numberOfPending,
                NumberOfApproved = numberOfApproved,
                NumberOfCancel = numberOfCancel,
                ListUserLoanRefResponse = lstUserLoanGroupByDate,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return await Result<SummaryUserLoanRefResponse>.SuccessAsync(summaryUserLoanResponse);
        }
    }
}
