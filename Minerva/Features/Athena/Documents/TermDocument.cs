using Minerva.Infrastructure.Database;

namespace Minerva.Features.Athena.Documents;

public class TermDocument : MongoDocument
{
    public int TermId { get; set; }
    
    public string Semester { get; set; }
    
    public long StartDate { get; set; }
    
    public long EndDate { get; set; }

    public DateTime ScrapedAt { get; set; }
    
    public List<string> SubjectIds { get; set; }
}