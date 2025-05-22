using EmailServiceProvider.Dtos;
using EmailServiceProvider.Models;

namespace EmailServiceProvider.Services
{
    public interface IEmailService
    {
        Task<EmailServiceResult> SendEmailAsync(EmailRequestModel? emailRequest = null);
    }
}