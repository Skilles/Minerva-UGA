using Minerva.Config;
using Minerva.Features.Athena.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class SubjectRepository : MongoRepository<SubjectDocument>
{
    public SubjectRepository(IMongoClient client, MinervaConfig config) : base(client, config, "subjects")
    {
        
    }
}