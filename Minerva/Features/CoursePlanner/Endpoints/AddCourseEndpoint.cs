using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.CoursePlanner.Endpoints;

[HttpPost("/planner/courses")]
[Authorize]
public class AddCourseEndpoint : Endpoint<PlannerCourseRequest>
{
    private readonly PlannerService PlannerService;
    
    public AddCourseEndpoint(PlannerService plannerService)
    {
        PlannerService = plannerService;
    }
    
    public override async Task HandleAsync(PlannerCourseRequest req, CancellationToken ct)
    {
        await PlannerService.AddCourseToPlannerAsync(req.Id, req.CourseId, ct);
    }
}