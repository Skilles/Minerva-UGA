using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Athena.Documents;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.Search.Endpoints;

[HttpGet("/search/terms")]
[Authorize]
public class TermsEndpoint : EndpointWithoutRequest<IEnumerable<TermDocument>>
{
    private readonly PlannerFetchService PlannerFetchService;
    
    public TermsEndpoint(PlannerFetchService plannerFetchService)
    {
        PlannerFetchService = plannerFetchService;
    }
    
    public override Task<IEnumerable<TermDocument>> ExecuteAsync(CancellationToken ct) => PlannerFetchService.GetAllTermsAsync(ct);
}