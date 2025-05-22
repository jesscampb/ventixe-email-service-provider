using Azure.Messaging.ServiceBus;
using EmailServiceProvider.Models;
using EmailServiceProvider.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EmailServiceProvider.Functions;

public class HandleEmailRequest(ILogger<HandleEmailRequest> logger, IEmailService emailService)
{
    private readonly ILogger<HandleEmailRequest> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    [Function(nameof(HandleEmailRequest))]
    public async Task Run(
        [ServiceBusTrigger("verification-email-requested", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            var request = JsonSerializer.Deserialize<EmailRequestModel>(
                message.Body.ToString(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            ArgumentNullException.ThrowIfNull(request);

            var result = await _emailService.SendEmailAsync(request);

            if (result.Succeeded)
            {
                _logger.LogInformation("Email sent successfully: {Message}", result.Message);
                await messageActions.CompleteMessageAsync(message);
            }
            else
            {
                _logger.LogWarning("Email send failed: {Error}", result.Message);
                await messageActions.DeadLetterMessageAsync(
                    message,
                    deadLetterReason: "SendFailure",
                    deadLetterErrorDescription: result.Message
                    );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in email processing.");
            await messageActions.DeadLetterMessageAsync(
                message,
                deadLetterReason: "ProcessingError",
                deadLetterErrorDescription: ex.Message
                );
        }
    }
}