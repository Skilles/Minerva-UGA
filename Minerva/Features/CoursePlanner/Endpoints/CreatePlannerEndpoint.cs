using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;
using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Endpoints;

[HttpPost("/planner")]
[Authorize]
public class CreatePlannerEndpoint : Endpoint<CreatePlannerRequest, string>
{
    private readonly PlannerService PlannerService;
    
    public CreatePlannerEndpoint(PlannerService plannerService)
    {
        PlannerService = plannerService;
    }
    
    public override async Task<string> ExecuteAsync(CreatePlannerRequest req, CancellationToken ct) => await PlannerService.CreatePlannerAsync(req.Email, req.TermId, ct);
}