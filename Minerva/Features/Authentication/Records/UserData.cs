using MongoDB.Bson;

namespace Minerva.Features.Authentication.Records;

public class UserData
{
    // TermId => PlannerId
    public IDictionary<string, string> Planners { get; set; } = new Dictionary<string, string>();
}