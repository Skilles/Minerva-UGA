using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerCourseRequest : IdRequest<string>
{
    public string CourseId { get; set; }
}