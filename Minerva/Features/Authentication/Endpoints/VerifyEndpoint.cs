using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Authentication.Records;
using Minerva.Features.Authentication.Services;

namespace Minerva.Features.Authentication.Endpoints;

[HttpGet("/auth/verify")]
[AllowAnonymous]
public class VerifyEndpoint : Endpoint<VerifyEndpointRequest>
{
    private readonly AuthenticationService AuthenticationService;
    
    public VerifyEndpoint(AuthenticationService authenticationService)
    {
        AuthenticationService = authenticationService;
    }
    
    public override async Task HandleAsync(VerifyEndpointRequest request, CancellationToken ct)
    {
        var verified = await AuthenticationService.VerifyUserAsync(request.Token, ct);
        
        if (verified)
        {
            await SendOkAsync(cancellation: ct);
        }
        else
        {
            await SendErrorsAsync(cancellation: ct);
        }
    }
}