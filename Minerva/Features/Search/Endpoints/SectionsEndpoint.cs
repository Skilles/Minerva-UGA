using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Athena.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.Search.Endpoints;

[HttpGet("/search/sections")]
[Authorize]
public class SectionsEndpoint : Endpoint<IdRequest<string>, IEnumerable<SectionDocument>>
{
    private readonly PlannerFetchService PlannerFetchService;
    
    public SectionsEndpoint(PlannerFetchService plannerFetchService)
    {
        PlannerFetchService = plannerFetchService;
    }
    
    public override Task<IEnumerable<SectionDocument>> ExecuteAsync(IdRequest<string> req, CancellationToken ct) => PlannerFetchService.GetSectionsByCourseIdAsync(req.Id, ct);
}