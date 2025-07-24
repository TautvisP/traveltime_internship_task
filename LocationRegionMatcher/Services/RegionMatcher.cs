namespace LocationRegionMatcher
{
    /// <summary>
    /// Represents the result of matching locations to a region.
    /// Contains the region name and a list of matched location names.
    /// </summary>
    public class RegionMatchResult
    {
        public string region { get; set; } = string.Empty;
        public List<string> matchedLocations { get; set; } = new();
    }

    public static class RegionMatcher
    {
        /// <summary>
        /// Matches locations to regions.
        /// For each region, finds all locations that are inside any of its polygons.
        /// </summary>
        /// <param name="locations">List of locations to match.</param>
        /// <param name="regions">List of regions to match against.</param>
        /// <returns>List of RegionMatchResult containing region names and their matched locations.</returns>
        public static List<RegionMatchResult> MatchLocationsToRegions(List<Location> locations, List<Region> regions)
        {
            return regions.Select(region => new RegionMatchResult
            {
                region = region.Name,
                matchedLocations = locations
                    .Where(loc => region.Coordinates.Any(poly => PolygonUtils.IsPointInPolygon(loc.Coordinates, poly)))
                    .Select(loc => loc.Name)
                    .ToList()
            }).ToList();
        }
    }
}