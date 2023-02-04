namespace Minerva.Utility;

public static class HttpClientExtensions
{
    public static async Task<IEnumerable<TOutput>> BulkFetchFromJsonAsync<TResponse, TOutput>(this HttpClient client, string url, Func<TResponse, TOutput> mappingFunction, bool retry = true, int maxDepth = 2, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(url);

        try
        {
            var response = await client.GetFromJsonAsync<IEnumerable<TResponse>>(uri);
            return response == null ? Enumerable.Empty<TOutput>() : response.Select(mappingFunction);
        }
        catch (HttpRequestException e)
        {
            if (!retry || maxDepth == 0)
            {
                await Console.Error.WriteLineAsync($"Error in fetching from URL {url}: {e.Message}");
                return Enumerable.Empty<TOutput>();
            }

            return await BulkFetchFromJsonAsync(client, url, mappingFunction, true, maxDepth - 1);
        }
    }
    
    public static async Task<TOutput?> FetchFromJsonAsync<TResponse, TOutput>(this HttpClient client, string url, Func<TResponse, TOutput> mappingFunction, bool retry = true, int maxDepth = 2, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(url);

        try
        {
            var response = await client.GetFromJsonAsync<TResponse>(uri);
            return response == null ? default : mappingFunction(response);
        }
        catch (HttpRequestException e)
        {
            if (!retry || maxDepth == 0)
            {
                await Console.Error.WriteLineAsync($"Error in fetching from URL {url}: {e.Message}");
                return default;
            }

            return await FetchFromJsonAsync(client, url, mappingFunction, true, maxDepth - 1);
        }
    }
}