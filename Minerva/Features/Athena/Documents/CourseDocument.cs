using Minerva.Infrastructure.Database;

namespace Minerva.Features.Athena.Documents;

public class CourseDocument : MongoDocument
{
    public string CourseId { get; set; }

    public string Name { get; set; }
    
    public List<int> SectionIds { get; set; }
}