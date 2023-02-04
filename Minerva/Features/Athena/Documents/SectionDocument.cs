using Minerva.Features.Athena.Records;
using Minerva.Infrastructure.Database;

namespace Minerva.Features.Athena.Documents;

public class SectionDocument : MongoDocument
{
    public string? ProfessorName { get; set; }

    public IEnumerable<MeetingRecord> Meetings { get; set; }
    
    public int CourseReferenceNumber { get; set; }
    
    public float CreditHours { get; set; }
}