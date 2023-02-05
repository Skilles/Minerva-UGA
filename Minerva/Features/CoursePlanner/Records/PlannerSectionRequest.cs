using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerSectionRequest : IdRequest<string>
{
    public int SectionId { get; set; }
}