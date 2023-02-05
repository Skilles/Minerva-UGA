using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerSectionRequest : IdRequest<ObjectId>
{
    public int SectionId { get; set; }
}