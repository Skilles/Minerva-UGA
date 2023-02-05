using System.Net.Mail;
using Minerva.Features.Authentication.Enums;
using Minerva.Features.Authentication.Records;
using Minerva.Infrastructure.Database;

namespace Minerva.Features.Authentication.Documents;

public class UserDocument : MongoDocument
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Password { get; set; }
    
    public MailAddress Email { get; set; }
    
    public Role Role { get; set; }
    
    public Guid? UniqueToken { get; set; }
    
    public bool Verified { get; set; }
    
    public UserData Data { get; set; }
}