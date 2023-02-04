using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Athena.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.Search.Endpoints;

[HttpGet("/search/courses")]
[Authorize]
public class CoursesEndpoint : Endpoint<IdRequest<string>, IEnumerable<CourseDocument>>
{
    private readonly PlannerFetchService PlannerFetchService;
    
    public CoursesEndpoint(PlannerFetchService plannerFetchService)
    {
        PlannerFetchService = plannerFetchService;
    }
    
    public override Task<IEnumerable<CourseDocument>> ExecuteAsync(IdRequest<string> req, CancellationToken ct) => PlannerFetchService.GetCoursesBySubjectIdAsync(req.Id, ct);
}