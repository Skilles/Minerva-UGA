using Minerva.Infrastructure.Database;

namespace Minerva.Features.Athena.Documents;

public class SubjectDocument : MongoDocument
{
    public string SubjectId { get; set; }
    
    public string Name { get; set; }

    public List<string> CourseIds { get; set; }
}