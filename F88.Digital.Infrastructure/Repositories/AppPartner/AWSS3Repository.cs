using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class AWSS3Repository : IAWSS3Repository
    {
        private S3Settings _s3Settings;
        private readonly ILogger<AWSS3Repository> _logger;
        public AWSS3Repository(IOptions<S3Settings> s3Settings, ILogger<AWSS3Repository> logger)
        {
            _s3Settings = s3Settings.Value;
            _logger = logger;
        }
        public async Task<string> UploadFile(IFormFile file, string path)
        {
            try
            {
                using var client = new AmazonS3Client(_s3Settings.AccessKey, _s3Settings.SecretKey, RegionEndpoint.APSoutheast1);
                await using var memStream = new MemoryStream();
                await file.CopyToAsync(memStream);
                var newPath = $"{DateTime.Now.ToString("yyyyMMdd")}/Digital/{(string.IsNullOrEmpty(path) ? string.Empty :  path + "/")}{Guid.NewGuid()}-{StringUtils.RemoveSign4VietnameseString(file.FileName.Replace(" ", "_"))}";
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memStream,
                    Key = newPath,
                    BucketName = _s3Settings.BucketName,
                    StorageClass = S3StorageClass.Standard
                };
                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);
                return newPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex);
                return null;
            }
        }

        public string PresignURL(string fileName)
        {
            string urlString = string.Empty;
            try
            {
                using (var client = new AmazonS3Client(_s3Settings.AccessKey, _s3Settings.SecretKey, RegionEndpoint.APSoutheast1))
                {
                    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                    {
                        BucketName = _s3Settings.BucketName,
                        Key = fileName,
                        Verb = HttpVerb.GET,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };
                    urlString = client.GetPreSignedURL(request1);
                }
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return urlString;
        }
    }
}
