using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.External.RateMyProfessor.Records;
using Minerva.Features.Rating.Records;
using Minerva.Features.Rating.Services;

namespace Minerva.Features.Rating.Endpoints;

[HttpGet("rmp")]
[AllowAnonymous]
public class GetDataEndpoint : Endpoint<RmpRequest, ProfessorInfo>
{
    private readonly RmpService RmpService;

    public GetDataEndpoint(RmpService rmpService)
    {
        RmpService = rmpService;
    }

    public override async Task<ProfessorInfo> ExecuteAsync(RmpRequest request, CancellationToken cancellationToken)
        => await RmpService.GetProfessorInfoAsync(request.Name, cancellationToken);
}