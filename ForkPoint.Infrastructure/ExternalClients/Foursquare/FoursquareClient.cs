using ForkPoint.Application.ExternalClients.Foursquare;
using ForkPoint.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Infrastructure.ExternalClients.Foursquare;

public class FoursquareClient(
    IHttpClientFactory httpClientFactory,
    ILogger<FoursquareClient> logger
) : IFoursquareClient
{
    private const string DiningAndDrinking = "63be6904847c3692a84b9bb5";
    private const string Limit = "50";
    private const string Fields = "fsq_place_id,name,location,categories";
    private const string Near = "Manchester, UK";
    private const string Sort = "POPULARITY";

    public async Task<FoursquareClientResponse> SearchAsync(
        CancellationToken cancellationToken = default
    )
    {
        var httpClient = httpClientFactory.CreateClient("FoursquareClient");

        var queryParams = new Dictionary<string, string>
        {
            ["fsq_category_ids"] = DiningAndDrinking,
            ["limit"] = Limit,
            ["fields"] = Fields,
            ["near"] = Near,
            ["sort"] = Sort
        };

        // Concatenate query parameters with & and append to the URL
        var queryString = string.Join("&",
            queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}")
        );

        // Build the request URL with query parameters
        var url = "search?" + queryString;

        logger.LogInformation("Sending request to Foursquare API: {Url}",
            httpClient.BaseAddress + url);

        var httpResponse = await httpClient.GetAsync(url, cancellationToken);

        logger.LogInformation("Received response from Foursquare API: {StatusCode}",
            httpResponse.StatusCode);

        var jsonRes =
            await httpResponse
                .DeserializeResponseAsync<FoursquareClientResponse>(cancellationToken);

        return jsonRes ??
               throw new InvalidOperationException(
                   "Failed to deserialize Foursquare API response.");
    }
}
