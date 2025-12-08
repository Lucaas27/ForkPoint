namespace ForkPoint.Application.ExternalClients.Foursquare;

public interface IFoursquareClient
{
    Task<FoursquareClientResponse> SearchAsync(
        CancellationToken cancellationToken = default
    );
}
