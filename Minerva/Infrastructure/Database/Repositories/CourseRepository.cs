using Minerva.Config;
using Minerva.Features.Athena.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class CourseRepository : MongoRepository<CourseDocument>
{
    public CourseRepository(IMongoClient client, MinervaConfig config) : base(client, config, "courses")
    {
        
    }
}