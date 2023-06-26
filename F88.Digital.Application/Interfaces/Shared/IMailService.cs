using F88.Digital.Application.DTOs.Mail;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Shared
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}