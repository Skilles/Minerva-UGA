using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Minerva.Config;

namespace Minerva.Infrastructure.Email.Services;

public class EmailService
{

    private const string MailgunApiUrl = "api.mailgun.net/v3";
    
    private readonly HttpClient HttpClient;

    private readonly MailgunConfig Config;

    private readonly string AppUrl;

    private ILogger<EmailService> Logger;

    public EmailService(ILogger<EmailService> logger, HttpClient httpClient, MinervaConfig config)
    {
        Logger = logger;
        HttpClient = httpClient;
        Config = config.Mailgun;
        AppUrl = config.AppUrl;
        HttpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{Config.ApiKey}")));
    }

    public async Task SendVerificationEmailAsync(string email, string token, CancellationToken ct)
    {
        var verificationLink = $"https://{AppUrl}/verify?token={token}";
        var emailBody = $"Please verify your email by clicking on the link below: {verificationLink}";
        await SendEmailAsync(new(email), "minerva", "Verify your email", emailBody, ct);
    }

    public async Task SendEmailAsync(MailAddress email, string from, string subject, string body, CancellationToken ct)
    {
        var uri = new Uri($"https://{MailgunApiUrl}/{Config.Domain}/messages");
        
        var formContent = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "from", $"Minerva <{from}@{Config.Domain}>" },
            { "to", email.Address },
            { "subject", subject },
            { "text", body }
        });
        
        var response = await HttpClient.PostAsync(uri, formContent, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            Logger.LogError("Email error: {Error}", await response.Content.ReadAsStringAsync(ct));
            throw new HttpRequestException("Failed to send email");
        }
    }
}