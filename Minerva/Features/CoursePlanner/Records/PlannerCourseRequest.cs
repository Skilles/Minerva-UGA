using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Records;

public class PlannerCourseRequest : PlannerDataRequest
{
    public string CourseId { get; set; }
}