namespace LocationRegionMatcher
{
    public static class RegionMatcher
    {
        /// <summary>
        /// Matches locations to regions.
        /// For each region, finds all locations that are inside any of its polygons.
        /// </summary>
        /// <param name="locations">List of Location objects.</param>
        /// <param name="regions">List of Region objects.</param>
        /// <returns>List of anonymous objects with region name and matched location names.</returns>
        public static List<object> MatchLocationsToRegions(List<Location> locations, List<Region> regions)
        {
            return regions.Select(region => new
            {
                region = region.Name,
                matched_locations = locations
                    .Where(loc => region.Coordinates.Any(poly => PolygonUtils.IsPointInPolygon(loc.Coordinates, poly)))
                    .Select(loc => loc.Name)
                    .ToList()
            }).ToList<object>();
        }
    }
}