// dotnet run --project LocationRegionMatcher.csproj input/regions.json input/locations.json output/results.json
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a location with a name and coordinates.
/// </summary>
public record Location(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("coordinates")] double[] Coordinates
);

/// <summary>
/// Represents a region with a name and a list of polygons.
/// </summary>
public record Region(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("coordinates")] List<List<double[]>> Coordinates
);

class Program
{
    /// <summary>
    /// Parses a list of locations from a JSON file.
    /// Validates that each location has a unique name and valid coordinates.
    /// Throws exceptions for missing files, malformed JSON, or invalid data.
    /// </summary>
    /// <param name="path">Path to the locations JSON file.</param>
    /// <returns>List of valid Location objects.</returns>
    private static List<Location> ParseLocations(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Locations file not found: {path}");
        var json = File.ReadAllText(path);
        var locations = JsonSerializer.Deserialize<List<Location>>(json);
        if (locations == null)
            throw new InvalidDataException("Locations JSON is invalid.");

        var nameSet = new HashSet<string>();
        foreach (var loc in locations)
        {
            if (loc.Name == null)
                throw new InvalidDataException("Location name missing.");
            if (loc.Coordinates == null || loc.Coordinates.Length != 2)
                throw new InvalidDataException($"Location '{loc.Name}' has invalid coordinates.");
            if (!nameSet.Add(loc.Name))
                throw new InvalidDataException($"Duplicate location name: {loc.Name}");
        }
        return locations;
    }

    /// <summary>
    /// Validates a polygon for a region.
    /// Ensures the polygon has at least 3 points, all coordinates are valid,
    /// and warns if the polygon is not closed (first and last point differ).
    /// Throws exceptions for invalid polygons.
    /// </summary>
    /// <param name="regionName">Name of the region containing the polygon.</param>
    /// <param name="poly">List of coordinates representing the polygon.</param>
    private static void ValidatePolygon(string regionName, List<double[]> poly)
    {
        if (poly == null || poly.Count < 3)
            throw new InvalidDataException($"Region '{regionName}' has degenerate polygon (less than 3 points).");
        foreach (var coord in poly)
        {
            if (coord == null || coord.Length != 2)
                throw new InvalidDataException($"Region '{regionName}' has invalid coordinate in polygon.");
        }
        if (!poly.First().SequenceEqual(poly.Last()))
            Console.Error.WriteLine($"Warning: Polygon in region '{regionName}' is not closed (first and last point differ).");
    }

    /// <summary>
    /// Validates a region object.
    /// Checks for a valid name, uniqueness, existence of polygons, and validates each polygon.
    /// Throws exceptions for invalid regions.
    /// </summary>
    /// <param name="region">Region object to validate.</param>
    /// <param name="nameSet">Set of region names for uniqueness check.</param>
    private static void ValidateRegion(Region region, HashSet<string> nameSet)
    {
        if (region.Name == null)
            throw new InvalidDataException("Region name missing.");
        if (!nameSet.Add(region.Name))
            throw new InvalidDataException($"Duplicate region name: {region.Name}");
        if (region.Coordinates == null || region.Coordinates.Count == 0)
            throw new InvalidDataException($"Region '{region.Name}' has no polygons.");

        foreach (var poly in region.Coordinates)
            ValidatePolygon(region.Name, poly);
    }

    /// <summary>
    /// Parses a list of regions from a JSON file.
    /// Validates each region and its polygons.
    /// Throws exceptions for missing files, malformed JSON, or invalid data.
    /// </summary>
    /// <param name="path">Path to the regions JSON file.</param>
    /// <returns>List of valid Region objects.</returns>
    private static List<Region> ParseRegions(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Regions file not found: {path}");
        var json = File.ReadAllText(path);
        var regions = JsonSerializer.Deserialize<List<Region>>(json);
        if (regions == null)
            throw new InvalidDataException("Regions JSON is invalid.");

        var nameSet = new HashSet<string>();
        foreach (var region in regions)
        {
            ValidateRegion(region, nameSet);
        }
        return regions;
    }

    /// <summary>
    /// Determines if a point is inside a polygon using the ray casting algorithm.
    /// Returns true if the point is inside or exactly on the edge of the polygon.
    /// </summary>
    /// <param name="point">Array of two doubles representing [longitude, latitude].</param>
    /// <param name="polygon">List of coordinates representing the polygon.</param>
    /// <returns>True if the point is inside or on the edge, false otherwise.</returns>
    private static bool IsPointInPolygon(double[] point, List<double[]> polygon)
    {
        int n = polygon.Count, j = n - 1;
        bool inside = false;
        for (int i = 0; i < n; j = i++)
        {
            if ((polygon[i][0] == point[0] && polygon[i][1] == point[1]) ||
                (polygon[j][0] == point[0] && polygon[j][1] == point[1]))
                return true;
            if ((polygon[i][1] > point[1]) != (polygon[j][1] > point[1]))
            {
                double intersect = (polygon[j][0] - polygon[i][0]) * (point[1] - polygon[i][1]) / (polygon[j][1] - polygon[i][1]) + polygon[i][0];
                if (point[0] == intersect)
                    return true;
                if (point[0] < intersect)
                    inside = !inside;
            }
        }
        return inside;
    }

    /// <summary>
    /// Matches locations to regions.
    /// For each region, finds all locations that are inside any of its polygons.
    /// </summary>
    /// <param name="locations">List of Location objects.</param>
    /// <param name="regions">List of Region objects.</param>
    /// <returns>List of anonymous objects with region name and matched location names.</returns>
    private static List<object> MatchLocationsToRegions(List<Location> locations, List<Region> regions)
    {
        return regions.Select(region => new
        {
            region = region.Name,
            matched_locations = locations
                .Where(loc => region.Coordinates.Any(poly => IsPointInPolygon(loc.Coordinates, poly)))
                .Select(loc => loc.Name)
                .ToList()
        }).ToList<object>();
    }

    /// <summary>
    /// Main entry point for the application.
    /// Expects three arguments: regions file, locations file, and output file.
    /// Parses input files, matches locations to regions, and writes results to output.
    /// Handles and reports errors.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: dotnet run -- <regions.json> <locations.json> <results.json>");
            return;
        }

        try
        {
            var regions = ParseRegions(args[0]);
            var locations = ParseLocations(args[1]);
            var results = MatchLocationsToRegions(locations, regions);

            File.WriteAllText(args[2], JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("Matching complete. Results written to " + args[2]);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}