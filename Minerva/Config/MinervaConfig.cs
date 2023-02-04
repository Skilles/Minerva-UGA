namespace Minerva.Config;

public class MinervaConfig
{
    public string AppUrl { get; set; } = string.Empty;
    
    public MongoConfig Mongo { get; set; } = new();
    
    
    public JwtConfig Jwt { get; set; } = new();
    
    public MailgunConfig Mailgun { get; set; } = new();
}