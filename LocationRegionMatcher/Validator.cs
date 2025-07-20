namespace LocationRegionMatcher
{
    public static class Validator
    {
        /// <summary>
        /// Validates a location object.
        /// Checks for a valid name, uniqueness, and valid coordinates.
        /// Throws exceptions for invalid locations.
        /// </summary>
        /// <param name="loc">Location object to validate.</param>
        /// <param name="nameSet">Set of location names for uniqueness check.</param>
        public static void ValidateLocation(Location loc, HashSet<string> nameSet)
        {
            if (loc.Name == null)
                throw new InvalidDataException("Location name missing.");
            if (loc.Coordinates == null || loc.Coordinates.Length != 2)
                throw new InvalidDataException($"Location '{loc.Name}' has invalid coordinates.");
            if (!nameSet.Add(loc.Name))
                throw new InvalidDataException($"Duplicate location name: {loc.Name}");
        }

        /// <summary>
        /// Validates a region object.
        /// Checks for a valid name, uniqueness, existence of polygons, and validates each polygon.
        /// Throws exceptions for invalid regions.
        /// </summary>
        /// <param name="region">Region object to validate.</param>
        /// <param name="nameSet">Set of region names for uniqueness check.</param>
        public static void ValidateRegion(Region region, HashSet<string> nameSet)
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
        /// Validates a polygon for a region.
        /// Ensures the polygon has at least 3 points, all coordinates are valid,
        /// and warns if the polygon is not closed (first and last point differ).
        /// Throws exceptions for invalid polygons.
        /// </summary>
        /// <param name="regionName">Name of the region containing the polygon.</param>
        /// <param name="poly">List of coordinates representing the polygon.</param>
        public static void ValidatePolygon(string regionName, List<double[]> poly)
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
    }
}