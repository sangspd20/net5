using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IAWSS3Repository
    {
        Task<string> UploadFile(IFormFile file, string path);
        string PresignURL(string fileName);
    }
}
