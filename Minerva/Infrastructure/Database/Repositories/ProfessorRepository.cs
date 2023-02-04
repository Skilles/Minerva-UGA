using Minerva.Config;
using Minerva.Features.Athena.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class ProfessorRepository : MongoRepository<ProfessorDocument>
{
    public ProfessorRepository(IMongoClient client, MinervaConfig config) : base(client, config, "faculty")
    {
        
    }
}