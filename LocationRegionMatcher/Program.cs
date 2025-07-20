using System.Text.Json;

namespace LocationRegionMatcher
{
    class Program
    {
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
            await foreach (var loc in JsonSerializer.DeserializeAsyncEnumerable<Location>(stream))
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
            await foreach (var region in JsonSerializer.DeserializeAsyncEnumerable<Region>(stream))
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
        static async Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: dotnet run -- <regions.json> <locations.json> <results.json>");
                return;
            }

            try
            {
                var regions = await ParseRegionsAsync(args[0]);
                var locations = await ParseLocationsAsync(args[1]);
                List<RegionMatchResult> results = RegionMatcher.MatchLocationsToRegions(locations, regions);

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
}