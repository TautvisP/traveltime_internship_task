using System.Text.Json.Serialization;

namespace LocationRegionMatcher
{
    /// <summary>
    /// Represents a geographic coordinate (longitude, latitude).
    /// </summary>
    public record Coordinate(
        [property: JsonPropertyOrder(0)] double Longitude,
        [property: JsonPropertyOrder(1)] double Latitude
    );
}