// add readme comments with the new project structure
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocationRegionMatcher
{
    /// <summary>
    /// Custom JSON converter for Coordinate.
    /// Handles deserialization from [longitude, latitude] arrays and serialization to arrays.
    /// </summary>
    public class CoordinateJsonConverter : JsonConverter<Coordinate>
    {
        /// <summary>
        /// Reads a Coordinate from JSON.
        /// Expects a JSON array: [longitude, latitude].
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type being converted (Coordinate).</param>
        /// <param name="options">Serializer options.</param>
        /// <returns>A Coordinate instance.</returns>
        public override Coordinate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            reader.Read();
            double lon = reader.GetDouble();
            reader.Read();
            double lat = reader.GetDouble();
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndArray)
                throw new JsonException();

            return new Coordinate(lon, lat);
        }

        /// <summary>
        /// Writes a Coordinate to JSON as [longitude, latitude].
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The Coordinate to serialize.</param>
        /// <param name="options">Serializer options.</param>
        public override void Write(Utf8JsonWriter writer, Coordinate value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Longitude);
            writer.WriteNumberValue(value.Latitude);
            writer.WriteEndArray();
        }
    }
}