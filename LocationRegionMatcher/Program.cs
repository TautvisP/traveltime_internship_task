using System.Text.Json;
using CommandLine;

namespace LocationRegionMatcher
{
    class Program
    {
        // Register custom converters for Coordinate and Polygon
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new CoordinateJsonConverter(),
                new PolygonJsonConverter()
            }
        };

        public class Options
        {
            [Value(0, MetaName = "regions", Required = true, HelpText = "Path to the regions JSON file.")]
            public string Regions { get; set; } = "";

            [Value(1, MetaName = "locations", Required = true, HelpText = "Path to the locations JSON file.")]
            public string Locations { get; set; } = "";

            [Value(2, MetaName = "output", Required = true, HelpText = "Path to the output JSON file.")]
            public string Output { get; set; } = "";
        }

        /// <summary>
        /// Parses a list of locations from a JSON file using streaming.
        /// Validates each location as it is read.
        /// Throws exceptions for missing files, malformed JSON, or invalid data.
        /// </summary>
        /// <param name="path">Path to the locations JSON file.</param>
        /// <returns>List of valid Location objects.</returns>
        private static async Task<List<Location>> ParseLocationsAsync(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Locations file not found: {path}");

            var nameSet = new HashSet<string>();
            var locations = new List<Location>();

            await using var stream = File.OpenRead(path);
            await foreach (var loc in JsonSerializer.DeserializeAsyncEnumerable<Location>(stream, options))
            {
                if (loc == null)
                    throw new InvalidDataException("Locations JSON is invalid.");
                Validator.ValidateLocation(loc, nameSet);
                locations.Add(loc);
            }
            return locations;
        }


        /// <summary>
        /// Parses a list of regions from a JSON file using streaming.
        /// Validates each region and its polygons as they are read.
        /// Throws exceptions for missing files, malformed JSON, or invalid data.
        /// </summary>
        /// <param name="path">Path to the regions JSON file.</param>
        /// <returns>List of valid Region objects.</returns>
        private static async Task<List<Region>> ParseRegionsAsync(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Regions file not found: {path}");

            var nameSet = new HashSet<string>();
            var regions = new List<Region>();

            await using var stream = File.OpenRead(path);
            await foreach (var region in JsonSerializer.DeserializeAsyncEnumerable<Region>(stream, options))
            {
                if (region == null)
                    throw new InvalidDataException("Regions JSON is invalid.");
                Validator.ValidateRegion(region, nameSet);
                regions.Add(region);
            }
            return regions;
        }


        /// <summary>
        /// Main entry point for the application.
        /// Expects three arguments: regions file, locations file, and output file.
        /// Parses input files, matches locations to regions, and writes results to output.
        /// Handles and reports errors.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        static async Task<int> Main(string[] args)
        {
            int exitCode = 0;
            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async opts =>
                {
                    try
                    {
                        var regionsList = await ParseRegionsAsync(opts.Regions);
                        var locationsList = await ParseLocationsAsync(opts.Locations);
                        List<RegionMatchResult> results = RegionMatcher.MatchLocationsToRegions(locationsList, regionsList);

                        File.WriteAllText(opts.Output, JsonSerializer.Serialize(results, options));
                        Console.WriteLine("Matching complete. Results written to " + opts.Output);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: {ex.Message}");
                        exitCode = 1;
                    }
                });
            return exitCode;
        }
    }
}