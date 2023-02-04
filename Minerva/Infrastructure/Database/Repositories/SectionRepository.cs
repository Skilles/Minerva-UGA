using Minerva.Config;
using Minerva.Features.Athena.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class SectionRepository : MongoRepository<SectionDocument>
{
    public SectionRepository(IMongoClient client, MinervaConfig config) : base(client, config, "sections")
    {
        
    }
}