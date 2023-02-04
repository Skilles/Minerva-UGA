using FastEndpoints;
using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class CreatePlannerRequest
{
    [FromClaim]
    public ObjectId UserId { get; set; }
    
    public ObjectId TermId { get; set; }
}