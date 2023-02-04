using Microsoft.AspNetCore.Identity;
using Minerva.Features.Authentication.Documents;
using Minerva.Features.Authentication.Enums;
using Minerva.Features.Authentication.Records;
using Minerva.Infrastructure.Database;
using Minerva.Infrastructure.Email.Services;
using Minerva.Utility;
using Minerva.Validation;
using MongoDB.Driver;

namespace Minerva.Features.Authentication.Services;

public class AuthenticationService
{
    private readonly IRepository<UserDocument> UserRepository;
    
    private readonly JWTService JWTService;
    
    private readonly EmailService EmailService;

    private readonly PasswordHasher<UserDocument> PasswordHasher;

    public AuthenticationService(JWTService jwtService, EmailService emailService, IRepository<UserDocument> userRepository)
    {
        JWTService = jwtService;
        EmailService = emailService;
        UserRepository = userRepository;
        PasswordHasher = new();
    }

    public async Task RegisterUserAsync(string firstName, string lastName, string password, string email, CancellationToken ct)
    {
        var mailAddress = email.ToEmail();

        if (mailAddress == null)
        {
            throw new MinervaValidationException("Email address is not valid");
        }
        
        if (!mailAddress.Host.EndsWith(".edu"))
        {
            throw new MinervaValidationException("Email address must be a .edu address");
        }

        var existingEmail = await UserRepository.Collection.FindAsync(u => u.Email == mailAddress, cancellationToken: ct);
        
        if (await existingEmail.AnyAsync(cancellationToken: ct))
        {
            throw new MinervaValidationException("Email address is already in use");
        }
        
        var token = Guid.NewGuid();

        var user = new UserDocument
        {
            FirstName = firstName,
            LastName = lastName,
            Email = mailAddress,
            Password = HashPassword(password),
            Role = Role.User,
            Verified = false,
            UniqueToken = token
        };

        await UserRepository.UpsertAsync(user, u => u.Email, ct);
        
        // Index UserRepository
        var emailIndex = Builders<UserDocument>.IndexKeys.Ascending(u => u.Email);
        var emailIndexOptions = new CreateIndexOptions { Unique = true };

        await UserRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserDocument>(emailIndex, emailIndexOptions), cancellationToken: ct);
        
        await EmailService.SendVerificationEmailAsync(mailAddress.Address, token.ToString(), ct);
    }

    public async Task<UserResponseRecord> LoginUserAsync(string email, string password, CancellationToken ct)
    {
        var mailAddress = email.ToEmail();

        if (mailAddress == null)
        {
            throw new MinervaValidationException("Email address is not valid");
        }

        var cursor = await UserRepository.Collection.FindAsync(u => u.Email == mailAddress, cancellationToken: ct);

        var user = await cursor.SingleOrDefaultAsync(ct);
        if (user == null)
        {
            throw new MinervaValidationException("Invalid email address");
        }

        if (!IsPasswordValid(password, user.Password))
        {
            throw new MinervaValidationException("Invalid password");
        }

        if (!user.Verified)
        {
            throw new MinervaValidationException("User is not verified");
        }

        return new
        (
            user.FirstName,
            user.LastName,
            user.Role,
            JWTService.GenerateToken(user),
            user.Verified
        );
    }

    public async Task<bool> VerifyUserAsync(string token, CancellationToken ct)
    {
        var guid = Guid.Parse(token);
        var cursor = await UserRepository.Collection.FindAsync(u => u.UniqueToken == guid, cancellationToken: ct);
        
        var user = await cursor.SingleOrDefaultAsync(ct);
        if (user == null)
        {
            throw new("Invalid token");
        }
        
        user.UniqueToken = null;
        user.Verified = true;
        
        var result = await UserRepository.Collection.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: ct);

        if (result.ModifiedCount != 1)
        {
            throw new("Error verifying user");
        }
        
        return true;
    }

    private string HashPassword(string password) => PasswordHasher.HashPassword(null!, password);
    
    private bool IsPasswordValid(string password, string hash) => PasswordHasher.VerifyHashedPassword(null!, hash, password) == PasswordVerificationResult.Success;
}