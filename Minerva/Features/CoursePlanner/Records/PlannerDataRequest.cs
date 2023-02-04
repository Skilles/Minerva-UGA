using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerDataRequest
{
    public ObjectId PlannerId { get; set; }
}