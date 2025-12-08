using System.Net.Http.Json;
using System.Text.Json;

namespace ForkPoint.Infrastructure.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> DeserializeResponseAsync<T>(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default
    )
    {
        response.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions
        {
            // Ignore case when matching property names during deserialization
            PropertyNameCaseInsensitive = true
        };

        var obj = await response.Content.ReadFromJsonAsync<T>(options, cancellationToken);
        return obj;
    }
}
