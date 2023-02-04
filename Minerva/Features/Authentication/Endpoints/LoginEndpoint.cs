using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Authentication.Records;
using Minerva.Features.Authentication.Services;

namespace Minerva.Features.Authentication.Endpoints;

[HttpPost("/auth/login")]
[AllowAnonymous]
public class LoginEndpoint : Endpoint<LoginUserRequest, UserResponseRecord>
{
    private readonly AuthenticationService AuthenticationService;
    
    public LoginEndpoint(AuthenticationService authenticationService)
    {
        AuthenticationService = authenticationService;
    }
    
    public override async Task<UserResponseRecord> ExecuteAsync(LoginUserRequest req, CancellationToken ct) => await AuthenticationService.LoginUserAsync(req.Email, req.Password, ct);
}