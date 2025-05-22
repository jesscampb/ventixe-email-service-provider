namespace EmailServiceProvider.Models;

public class EmailRequestModel
{
    public List<string> Recipients { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string PlainText { get; set; } = null!;
    public string Html { get; set; } = null!;
}
