using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForkPoint.Application.ExternalClients.Foursquare;

public class FoursquareClientResponse
{
    [JsonPropertyName("results")]
    public List<Place>? Results { get; set; }
}

public class Place
{
    [JsonPropertyName("fsq_place_id")]
    public string? FsqPlaceId { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    [JsonPropertyName("categories")]
    public List<Category>? Categories { get; set; }

    [JsonPropertyName("distance")]
    public int? Distance { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("placemaker_url")]
    public string? PlacemakerUrl { get; set; }

    [JsonPropertyName("related_places")]
    public JsonElement? RelatedPlaces { get; set; }

    [JsonPropertyName("social_media")]
    public JsonElement? SocialMedia { get; set; }

    [JsonPropertyName("tel")]
    public string? Tel { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }
}

public class Category
{
    [JsonPropertyName("fsq_category_id")]
    public string? FsqCategoryId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Location
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("locality")]
    public string? Locality { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }
}
