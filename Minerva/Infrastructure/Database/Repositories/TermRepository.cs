using Minerva.Config;
using Minerva.Features.Athena.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class TermRepository : MongoRepository<TermDocument>
{
    public TermRepository(IMongoClient client, MinervaConfig config) : base(client, config, "terms")
    {
        
    }
}