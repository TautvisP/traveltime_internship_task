using System.Text.Json.Serialization;

namespace LocationRegionMatcher
{
    /// <summary>
    /// Represents a region with a name and a list of polygons.
    /// </summary>
    public record Region(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("coordinates")] List<Polygon> Polygons
    );
}