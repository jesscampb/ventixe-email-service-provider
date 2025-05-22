using Azure;
using Azure.Communication.Email;
using EmailServiceProvider.Dtos;
using EmailServiceProvider.Models;
using Microsoft.Extensions.Options;

namespace EmailServiceProvider.Services;

public interface IEmailService
{
    Task<EmailServiceResult> SendEmailAsync(EmailRequestModel message);
}

public class EmailService(EmailClient client, IOptions<AzureCommunicationSettings> options) : IEmailService
{
    private readonly EmailClient _client = client;
    private readonly AzureCommunicationSettings _settings = options.Value;

    public async Task<EmailServiceResult> SendEmailAsync(EmailRequestModel? emailRequest = null)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(emailRequest);

            if (emailRequest.Recipients == null || emailRequest.Recipients.Count == 0)
                throw new ArgumentException("At least one recipient is required", nameof(emailRequest));

            var recipients = emailRequest.Recipients.Select(r => new EmailAddress(r)).ToList();

            var emailMessage = new EmailMessage(
                senderAddress: _settings.SenderAddress,
                content: new EmailContent(emailRequest.Subject)
                {
                    PlainText = emailRequest.PlainText,
                    Html = emailRequest.Html
                },
                recipients: new EmailRecipients(recipients));

            var result = await _client.SendAsync(WaitUntil.Completed, emailMessage);

            return new EmailServiceResult { Succeeded = result.HasCompleted, Message = "Email was successfully sent." };
        }
        catch (Exception ex)
        {
            return new EmailServiceResult { Succeeded = false, Message = $"Failed to send email: {ex.Message}" };
        }
    }
}
