using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Minerva.Features.Athena.Documents;
using Minerva.Features.CoursePlanner.Records;
using Minerva.Features.CoursePlanner.Services;

namespace Minerva.Features.Search.Endpoints;

[HttpPost("/search/bulkSections")]
[Authorize]
public class BulkSectionsEndpoint : Endpoint<BulkSectionRequest, IEnumerable<SectionDocument>>
{
    private readonly PlannerFetchService PlannerFetchService;
    
    public BulkSectionsEndpoint(PlannerFetchService plannerFetchService)
    {
        PlannerFetchService = plannerFetchService;
    }
    
    public override Task<IEnumerable<SectionDocument>> ExecuteAsync(BulkSectionRequest req, CancellationToken ct) => PlannerFetchService.GetSectionsByIdsAsync(req.Crns, ct);
}