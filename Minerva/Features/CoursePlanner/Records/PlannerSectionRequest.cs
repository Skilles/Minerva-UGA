using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerSectionRequest : PlannerDataRequest
{
    public int SectionId { get; set; }
}