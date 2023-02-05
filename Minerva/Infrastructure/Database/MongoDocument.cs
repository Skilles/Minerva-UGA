using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Minerva.Infrastructure.Database;

public abstract class MongoDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string Id { get; set; } = ObjectId.Empty.ToString();

    /// <summary>The created on (UTC).</summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}