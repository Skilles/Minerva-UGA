using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.CoursePlanner.Endpoints;

[HttpPost("/planner/sections")]
[Authorize]
public class AddSectionEndpoint : Endpoint<PlannerSectionRequest>
{
    private readonly PlannerService PlannerService;
    
    public AddSectionEndpoint(PlannerService plannerService)
    {
        PlannerService = plannerService;
    }
    
    public override async Task HandleAsync(PlannerSectionRequest req, CancellationToken ct)
    {
        await PlannerService.AddSectionToPlannerAsync(req.Id, req.SectionId, ct);
    }
}