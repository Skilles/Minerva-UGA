using Minerva.Infrastructure.Database;

namespace Minerva.Features.Athena.Documents;

public class BuildingDocument : MongoDocument
{
    // TODO: Create/use this when doing interop with Athena locations
    public int BuildingId { get; set; }
    
    public string Name { get; set; }
}