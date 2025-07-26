using System.Text.Json.Serialization;

namespace LocationRegionMatcher
{
    /// <summary>
    /// Represents a location with a name and coordinates.
    /// </summary>
    public record Location(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("coordinates")] Coordinate Coordinates
    );
}