using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Athena.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.Search.Endpoints;

[HttpGet("/search/subjects")]
[Authorize]
public class SubjectsEndpoint : Endpoint<IdRequest<int>, IEnumerable<SubjectDocument>>
{
    private readonly PlannerFetchService PlannerFetchService;
    
    public SubjectsEndpoint(PlannerFetchService plannerFetchService)
    {
        PlannerFetchService = plannerFetchService;
    }

    public override Task<IEnumerable<SubjectDocument>> ExecuteAsync(IdRequest<int> req, CancellationToken ct) => PlannerFetchService.GetAllSubjectsAsync(req.Id, ct);
}