using Minerva.External.RateMyProfessor.Records;
using Minerva.Infrastructure.Database;

namespace Minerva.Features.Athena.Documents;

public class ProfessorDocument : MongoDocument
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string FullName { get; set; }
    
    public ProfessorInfo? RmpInfo { get; set; }
}