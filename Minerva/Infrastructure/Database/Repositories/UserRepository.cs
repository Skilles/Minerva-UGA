using Minerva.Config;
using Minerva.Features.Authentication.Documents;
using MongoDB.Driver;

namespace Minerva.Infrastructure.Database.Repositories;

public class UserRepository : MongoRepository<UserDocument>
{
    public UserRepository(IMongoClient client, MinervaConfig config) : base(client, config, "users") { }
}