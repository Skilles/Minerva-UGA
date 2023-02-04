using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Minerva.Infrastructure.Database;

public abstract class MongoDocument
{
    [BsonIgnoreIfDefault]
    public ObjectId Id { get; set; } = ObjectId.Empty;

    /// <summary>The created on (UTC).</summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}