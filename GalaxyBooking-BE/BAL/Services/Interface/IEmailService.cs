using BAL.DTOs.Authentication;

namespace BAL.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailDto mail, bool isHtml = false);       
    }
}
