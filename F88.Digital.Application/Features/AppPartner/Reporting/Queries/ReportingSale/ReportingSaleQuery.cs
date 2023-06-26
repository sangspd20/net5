using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Extensions;
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

namespace F88.Digital.Application.Features.AppPartner.Reporting.Queries
{
    public class ReportingSaleQuery : IRequest<Result<List<ReportingSaleResponse>>>
    {
        public string RegionId { get; set; }
        public string Province { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class ReportingSaleQueryHandler : IRequestHandler<ReportingSaleQuery, Result<List<ReportingSaleResponse>>>
    {
        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly IMapper _mapper;
        private readonly IApiShareService _apiShareService;
        public ReportingSaleQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IApiShareService apiShareService, IMapper mapper)
        {
            _userLoanReferralRepository = userLoanReferralRepository;
            _apiShareService = apiShareService;
            _mapper = mapper;
        }

        public async Task<Result<List<ReportingSaleResponse>>> Handle(ReportingSaleQuery request, CancellationToken cancellationToken)
        {
            DateTime fromDate = new DateTime(request.Year, request.Month, 01);
            DateTime toDate = new DateTime(request.Year, request.Month, DateTime.DaysInMonth(request.Year, request.Month));

            // Lấy tổng đơn trong tháng
            var lstF2SByGroupId = await _userLoanReferralRepository.UserLoans
            .Where(x => x.CreatedOn.Date >= fromDate.Date
                        && x.CreatedOn.Date <= toDate.Date)
            .GroupBy(g => g.RefFinalGroupId)
            .Select(x => new ReportingSaleResponse
            {
                GroupId = x.Key,
                TotalFormByMonth = x.Count(),
                TotalSaleByMonth = x.Count(c => c.LoanStatus == ApiConstants.LoanStatus.APPROVED),
                TotalFormByDay = x.Count(c => c.CreatedOn.Day == request.Day),
                TotalSaleByDay = x.Count(c => c.CreatedOn.Day == request.Day && c.LoanStatus == ApiConstants.LoanStatus.APPROVED)
            }).ToListAsync();

            // init
            if (String.IsNullOrEmpty(request.RegionId)  && String.IsNullOrEmpty(request.Province))
            {
                var lstAllShop = _apiShareService.GetAllShop();
                lstF2SByGroupId = lstF2SByGroupId.Where(x => lstAllShop
                .Select(s => Convert.ToInt32(s.Value))
                .Contains(x.GroupId))
                .ToList();
            }


            if (String.IsNullOrEmpty(request.RegionId) && !String.IsNullOrEmpty(request.Province))
            {
                var lstShopFromProvince = _apiShareService.GetShopsByProvince(request.Province);
                lstF2SByGroupId = lstF2SByGroupId.Where(x => lstShopFromProvince
                .Select(s => Convert.ToInt32(s.Value))
                .Contains(x.GroupId))
                .ToList();
            }

             if (!String.IsNullOrEmpty(request.RegionId) && !String.IsNullOrEmpty(request.Province))
            {
                var lstShopFromDistrict = _apiShareService.GetShopsByRegionId(request.RegionId);
                lstF2SByGroupId = lstF2SByGroupId.Where(x => lstShopFromDistrict
                .Select(s => Convert.ToInt32(s.Value))
                .Contains(x.GroupId))
                .ToList();
            }

                //Tỉnh tỉ lệ
                lstF2SByGroupId.ForEach(s =>
                {
                    s.F2SByMonthOrderByDesc = s.TotalFormByMonth != 0 ? s.TotalSaleByMonth / s.TotalFormByMonth : 0;
                    s.F2SByMonth = (s.TotalFormByMonth != 0 && s.TotalSaleByMonth != 0) ? String.Format("{0:P2}.", (s.TotalSaleByMonth / s.TotalFormByMonth)) : "0%";
                    s.F2SByDay = (s.TotalFormByDay != 0 && s.TotalSaleByDay != 0) ? String.Format("{0:P2}.", (s.TotalSaleByDay / s.TotalFormByDay)) : "0%";
                    s.TotalCount = lstF2SByGroupId.Count;
                    s.TotalFormByDay = Math.Round(s.TotalFormByDay);
                    s.TotalFormByMonth = Math.Round(s.TotalFormByMonth);
                    s.TotalSaleByDay = Math.Round(s.TotalSaleByDay);
                    s.TotalSaleByMonth = Math.Round(s.TotalSaleByMonth);
                });

                //Lấy tên PGD
                lstF2SByGroupId.ForEach(s => s.GroupName = _apiShareService.GetShopsById(s.GroupId));

                // Order by F2S
                return await Result<List<ReportingSaleResponse>>.SuccessAsync(lstF2SByGroupId.OrderByDescending(s => s.F2SByMonthOrderByDesc).ToList());
            }
        }
}
