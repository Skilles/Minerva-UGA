using MongoDB.Bson;

namespace Minerva.Features.Authentication.Records;

public class UserData
{
    public Dictionary<int, ObjectId> Planners { get; set; } = new();
}