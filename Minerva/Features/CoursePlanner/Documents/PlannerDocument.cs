using Minerva.Infrastructure.Database;
using MongoDB.Bson;

namespace Minerva.Features.CoursePlanner.Documents;

public class PlannerDocument : MongoDocument
{
    public string Name { get; set; }
    
    public string UserId { get; set; }
    
    public int TermId { get; set; }
    
    public List<string> CourseIds { get; set; }
    
    public List<int> SectionIds { get; set; }
}