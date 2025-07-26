using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocationRegionMatcher
{
    /// <summary>
    /// Custom JSON converter for Polygon.
    /// Handles deserialization from a list of coordinates and serialization to a list of coordinates.
    /// </summary>
    public class PolygonJsonConverter : JsonConverter<Polygon>
    {
        /// <summary>
        /// Reads a Polygon from JSON.
        /// Expects a JSON array of coordinates (each coordinate is [longitude, latitude]).
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type being converted (Polygon).</param>
        /// <param name="options">Serializer options.</param>
        /// <returns>A Polygon instance.</returns>
        public override Polygon Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var coords = JsonSerializer.Deserialize<List<Coordinate>>(ref reader, options);
            return new Polygon(coords ?? new List<Coordinate>());
        }

        /// <summary>
        /// Writes a Polygon to JSON as a list of coordinates.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The Polygon to serialize.</param>
        /// <param name="options">Serializer options.</param>
        public override void Write(Utf8JsonWriter writer, Polygon value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (List<Coordinate>)value, options);
        }
    }
}