using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using AspNetCoreHero.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using F88.Digital.Application.Features.AppPartner;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using F88.Digital.Application.Helpers;

namespace F88.Digital.Application.Features.AppPartner.UserProfile.Commands.Update
{
    public class UpdateUserProfileCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Passport { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? PassportDate { get; set; }

        public string PassportPlace { get; set; }

        public IFormFile AvatarImage { get; set; }

        public string AvatarURL { get; set; }

        public string PassportFrontURL { get; set; }

        public IFormFile PassportFrontImage { get; set; }

        public string PassportBackURL { get; set; }

        public IFormFile PassportBackImage { get; set; }

        public bool? IsAgreementConfirmed { get; set; }
        public string Source { get; set; }
    }

    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IAWSS3Repository _aWSS3Repository;

        public UpdateUserProfileCommandHandler(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork, IAWSS3Repository aWSS3Repository)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
            _aWSS3Repository = aWSS3Repository;
        }

        public async Task<Result<int>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var userProfile = await _userProfileRepository.GetByIdAsync(command.UserPhone);

                if (userProfile == null)
                {
                    return Result<int>.Fail($"User không tồn tại");
                }
                else
                {

                    if (!string.IsNullOrEmpty(command.Passport))
                    {
                        var isPassportDuplicate = await _userProfileRepository.IsPassportExist(command.Passport, command.UserPhone);
                        if (isPassportDuplicate) return await Result<int>.FailAsync("Số CMND/CCCD đã tồn tại, vui lòng kiểm tra lại thông tin của bạn!");
                    }

                    #region Save Image
                    string avatarName = string.Empty;
                    string passportFrontImageName = string.Empty;
                    string passportBackImageName = string.Empty;

                    //if(command.AvatarImage == null && !string.IsNullOrEmpty(command.AvatarURL))
                    //    return await Result<int>.FailAsync("Cập nhật không thành công!");

                    //if (command.PassportFrontImage == null && !string.IsNullOrEmpty(command.PassportFrontURL))
                    //    return await Result<int>.FailAsync("Cập nhật không thành công!");

                    //if (command.PassportBackImage == null && !string.IsNullOrEmpty(command.PassportBackURL))
                    //    return await Result<int>.FailAsync("Cập nhật không thành công!");

                    userProfile.FirstName = command.FirstName ?? userProfile.FirstName;
                    userProfile.LastName = command.LastName ?? userProfile.LastName;
                    userProfile.Passport = command.Passport ?? userProfile.Passport;
                    userProfile.PassportDate = command.PassportDate ?? userProfile.PassportDate;
                    userProfile.PassportPlace = command.PassportPlace ?? userProfile.PassportPlace;
                    userProfile.IsAgreementConfirmed = command.IsAgreementConfirmed ?? true;
                    userProfile.IsActiveUpdate = false;
                    userProfile.Source = command.Source;

                    if (command.AvatarImage != null)
                    {
                        //CommonHelper.UploadAvatarAsync(command.AvatarImage, out avatarName);
                        avatarName = await _aWSS3Repository.UploadFile(command.AvatarImage, "Avatar");
                        userProfile.AvatarURL = avatarName;
                    }

                    if (command.PassportFrontImage != null)
                    {
                        //CommonHelper.UploadPassportAsync(command.PassportFrontImage, out passportFrontImageName);
                        passportFrontImageName = await _aWSS3Repository.UploadFile(command.PassportFrontImage, "Passport");
                        userProfile.PassportFrontURL = passportFrontImageName;
                    }

                    if (command.PassportBackImage != null)
                    {
                        //CommonHelper.UploadPassportAsync(command.PassportBackImage, out passportBackImageName);
                        passportBackImageName = await _aWSS3Repository.UploadFile(command.PassportBackImage, "Passport");
                        userProfile.PassportBackURL = passportBackImageName;
                    }


                    #endregion
                }

                var result =  _userProfileRepository.UpdateUserProfile(userProfile);
                //var result = await _unitOfWork.Commit(cancellationToken, command.UserPhone);

                if (result > 0) return await Result<int>.SuccessAsync("Cập nhật thành công!");

                return await Result<int>.FailAsync("Cập nhật không thành công!");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
            
    }
}
