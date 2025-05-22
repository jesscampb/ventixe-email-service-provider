using Azure.Communication.Email;
using EmailServiceProvider.Models;
using EmailServiceProvider.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.Configure<AzureCommunicationSettings>(builder.Configuration.GetSection("AzureCommunication"));
builder.Services.AddSingleton<EmailClient>(provider =>
{
    var settings = provider.GetRequiredService<IOptions<AzureCommunicationSettings>>().Value;
    return new EmailClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IEmailService, EmailService>();

builder.ConfigureFunctionsWebApplication();
builder.Build().Run();
