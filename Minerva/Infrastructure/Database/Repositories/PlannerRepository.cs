using Minerva.Config;
using Minerva.Features.CoursePlanner.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class PlannerRepository : MongoRepository<PlannerDocument>
{
    public PlannerRepository(IMongoClient client, MinervaConfig config) : base(client, config, "planners") { }
}