using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Application.Extensions;
using static F88.Digital.Application.Constants.ApiConstants;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command.Update
{
    public class UpdateUserLoanRefCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public string TransactionId { get; set; }

        public int LoanStatus { get; set; }

        public int RefContractGroupId { get; set; }

        public int RefFinalGroupId { get; set; }
        public string PawnId { get; set; }
        public string AssetType { get; set; }

        public UpdateDepositRequest UpdateBalance { get; set; }

        public decimal LoanAmount { get; set; }

    }

    public class UpdateUserLoanRefCommandHandler : IRequestHandler<UpdateUserLoanRefCommand, Result<int>>
    {
        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IAppNotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public UpdateUserLoanRefCommandHandler(IUserLoanReferralRepository userLoanReferralRepository,
            IUserNotificationRepository userNotificationRepository,
            IAppNotificationRepository notificationRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _userLoanReferralRepository = userLoanReferralRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(UpdateUserLoanRefCommand request, CancellationToken cancellationToken)
        {
            var userLoan = await _userLoanReferralRepository.GetByTransactionIdAsync(request.TransactionId);

            if(userLoan == null) return await Result<int>.FailAsync($"Đơn vay không tồn tại");

            decimal reward = ApiConstants.AmountValue.REWARD_AMOUNT;
            if (!string.IsNullOrEmpty(userLoan.RefAsset) && ConstantAsset.Assets.Any(x => x == userLoan.RefAsset.Trim().ToLower())) reward = ApiConstants.AmountValue.REWARD_OTO_AMOUNT;

            userLoan.Deposit.Notes = request.UpdateBalance.Notes;
            userLoan.LoanStatus = request.LoanStatus;
            userLoan.RefContractGroupId = request.RefContractGroupId;
            userLoan.LoanAmount = request.LoanAmount;
            userLoan.PawnId = request.PawnId;
            userLoan.RefAsset = request.AssetType;
            if (request.LoanStatus == ApiConstants.LoanStatus.APPROVED)
            {
                userLoan.Deposit.BalanceValue = reward;
                userLoan.RefFinalGroupId = userLoan.RefFinalGroupId == 0 ? request.RefContractGroupId : userLoan.RefFinalGroupId;               
            }
            else
            {
                userLoan.Deposit.BalanceValue = 0;
                userLoan.RefFinalGroupId = request.RefFinalGroupId;
            }

            await _userLoanReferralRepository.UpdateAsync(userLoan);
            var result = await _unitOfWork.Commit(cancellationToken, request.UserPhone ?? userLoan.CreatedBy);

            //if(result > 0)
            //{
            //    if (request.LoanStatus == ApiConstants.LoanStatus.APPROVED)
            //    {
            //        #region Set notification for user

            //        var notification = _notificationRepository.AppNotifications
            //            .Where(x => x.NotiTypeCode == ApiConstants.NotificationCode.REFERRALSUCCESS && x.Status)
            //            .SingleOrDefault();

            //        //var userNotiApp = new UserNotification()
            //        //{
            //        //    UserProfileId = userLoan.UserProfileId,
            //        //    AppNotificationId = notification.Id
            //        //};

            //        //await _userNotificationRepository.InsertAsync(userNotiApp);
            //        await _unitOfWork.Commit(cancellationToken, request.UserPhone);

            //        // Push noti
            //        //PushNotificationExtension.SendNotification(userNotiApp.AppNotification.NotiTitle, userNotiApp.AppNotification.NotiSummary);

            //        #endregion
            //    }

            //    return await Result<int>.SuccessAsync("Cập nhật thành công");
            //}

            //return await Result<int>.FailAsync("Cập nhật thất bại");
            return await Result<int>.SuccessAsync("Cập nhật thành công");
        }
    }
}
