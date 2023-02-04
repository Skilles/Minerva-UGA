using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerCourseRequest : IdRequest<ObjectId>
{
    public string CourseId { get; set; }
}