namespace EmailServiceProvider.Models;

public class AzureCommunicationSettings
{
    public string ConnectionString { get; set; } = default!;
    public string SenderAddress { get; set; } = default!;
}
