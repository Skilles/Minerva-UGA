using FastEndpoints;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;
using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Endpoints;

[HttpGet("/planner")]
public class PlannerDataEndpoint  : Endpoint<IdRequest<string>, PlannerDataRecord>
{
    private readonly PlannerService PlannerService;
    
    public PlannerDataEndpoint(PlannerService plannerFetchService)
    {
        PlannerService = plannerFetchService;
    }
    
    public override Task<PlannerDataRecord> ExecuteAsync(IdRequest<string> req, CancellationToken ct) => PlannerService.GetPlannerDataAsync(req.Id, ct);
}