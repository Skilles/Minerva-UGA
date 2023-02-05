using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Minerva.External.RateMyProfessor.Queries;
using Minerva.External.RateMyProfessor.Records;
using Newtonsoft.Json;


namespace Minerva.Features.Rating.Services;

public class RmpClient
{
    private const string ProfessorBaseUrl = "https://www.ratemyprofessors.com/professor?tid=";

    private GraphQLHttpClient GraphQlClient;
    private readonly ILogger<RmpClient> Logger;

    public RmpClient(ILogger<RmpClient> logger)
    {
        Logger = logger;
        var authHeader = new KeyValuePair<string, string>("Authorization", "Basic dGVzdDp0ZXN0");
        GraphQlClient = new("https://www.ratemyprofessors.com/graphql", new SystemTextJsonSerializer());
        GraphQlClient.HttpClient.DefaultRequestHeaders.Add(authHeader.Key, authHeader.Value);
    }

    public async Task<ProfessorInfo?> GetProfessorInfoAsync(string professorName, CancellationToken cancellationToken)
    {
        const string schoolId = "U2Nob29sLTExMDE=";
        // remove middle initial
        var professorWords = professorName.Split(' ');
        professorName = $"{professorWords[0]} {professorWords[^1]}";
        var request = new GraphQLRequest()
        {
            Query = SearchProfessorQuery.Query,
            Variables = new
            {
                Query = new
                {
                    Fallback = true,
                    Text = professorName
                },
                SchoolId = schoolId
            }
        };
        var content = await GraphQlClient.SendQueryAsync<RmpResponse>(request, cancellationToken: cancellationToken);

        var data = content.Data.Search.Teachers.Edges.FirstOrDefault()?.Node;

        Logger.LogInformation("RMP Data Retrieved: {Data}", JsonConvert.SerializeObject(data));

        return data == null
            ? null
            : new ProfessorInfo(data.Id, data.AvgRating, data.AvgDifficulty, ProfessorBaseUrl + data.LegacyId);
    }
}