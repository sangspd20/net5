using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using UserLoanRefEntity = F88.Digital.Domain.Entities.AppPartner.UserLoanReferral;
using UserPaymentEntity = F88.Digital.Domain.Entities.AppPartner.Payment;
using PaymentUserLoanReferralEntity = F88.Digital.Domain.Entities.AppPartner.PaymentUserLoanReferral;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using F88.Digital.Application.Constants;
using Microsoft.EntityFrameworkCore;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Application.Extensions;

namespace F88.Digital.Application.Features.AppPartner.Payment.Command.Create
{
    public partial class CreatePaymentCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public string AccNumber { get; set; }

        public decimal? PaidValue { get; set; }

        public decimal? TaxValue { get; set; }

        public decimal? OtherAmount { get; set; }

        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// Thành công: 1
        /// Thất bại: 0
        /// </summary>
        public bool Status { get; set; }

        public string Notes { get; set; }

        public List<UserLoanReferralRequest> UserLoanReferrals { get; set; }
    }

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<int>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserBankRepository _userbankRepository;
        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IAppNotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreatePaymentCommandHandler(IUserBankRepository userbankRepository, 
            IPaymentRepository paymentRepository, 
            IUserLoanReferralRepository userLoanReferralRepository,
            IUserNotificationRepository userNotificationRepository,
            IAppNotificationRepository notificationRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _userbankRepository = userbankRepository;
            _userLoanReferralRepository = userLoanReferralRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var userBank = await _userbankRepository.GetByAccNumberAsync(request.AccNumber);

            if (userBank == null) return await Result<int>.FailAsync("Số tài khoản ngân hàng không tồn tại");

            List<UserLoanRefEntity> userLoanReferrals = new List<UserLoanRefEntity>();

            // push data to Payment Table
            var payment = _mapper.Map<UserPaymentEntity>(request);
            payment.UserBankId = userBank.Id;
            
            foreach (var item in request.UserLoanReferrals)
            {
                var userLoan = _userLoanReferralRepository.UserLoans
                                                          .Where(x => x.TransactionId == item.TransactionId)
                                                          .Include(x => x.Deposit)
                                                          .FirstOrDefault();

                payment.PaymentUserLoanReferrals.Add(new PaymentUserLoanReferralEntity
                {
                    UserLoanReferralId = userLoan.Id
                });

                // update đơn vay sau khi payment
                userLoan.Deposit.Status = false;
                await _userLoanReferralRepository.UpdateAsync(userLoan);
            }

            await _paymentRepository.InsertAsync(payment);
            var result = await _unitOfWork.Commit(cancellationToken, request.UserPhone);

            if(result > 0)
            {
                if (request.Status)
                {
                    #region set notification for user

                    var notification = _notificationRepository.AppNotifications
                           .Where(x => x.NotiTypeCode == ApiConstants.NotificationCode.PAYMENTSUCCESS && x.Status)
                           .SingleOrDefault();

                    var userNotiApp = new UserNotification()
                    {
                        UserProfileId = userBank.UserProfileId,
                        AppNotificationId = notification.Id
                    };

                    await _userNotificationRepository.InsertAsync(userNotiApp);
                    await _unitOfWork.Commit(cancellationToken, request.UserPhone);

                    //Push noti
                    PushNotificationExtension.SendNotification(notification.NotiTitle, notification.NotiSummary);

                    #endregion
                }

                return await Result<int>.SuccessAsync("Cập nhật thành công");
            }

            return await Result<int>.FailAsync("Cập nhật thất bại");
        }
    }
}
