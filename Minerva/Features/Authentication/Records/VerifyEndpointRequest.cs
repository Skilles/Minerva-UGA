using FastEndpoints;

namespace Minerva.Features.Authentication.Records;

public class VerifyEndpointRequest
{
    [QueryParam]
    public string Token { get; set; }
}