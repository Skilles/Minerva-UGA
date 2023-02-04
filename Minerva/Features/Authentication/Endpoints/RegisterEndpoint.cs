using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Authentication.Records;
using Minerva.Features.Authentication.Services;

namespace Minerva.Features.Authentication.Endpoints;

[HttpPost("register")]
[AllowAnonymous]
public class RegisterEndpoint : Endpoint<RegisterUserRequest>
{
    private readonly AuthenticationService AuthenticationService;
    
    public RegisterEndpoint(AuthenticationService authenticationService)
    {
        AuthenticationService = authenticationService;
    }
    
    public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
    {
        await AuthenticationService.RegisterUserAsync(req.FirstName, req.LastName, req.Password, req.Email, ct);
    }
}