using LocationRegionMatcher;

public class RegionMatcherTests
{
    /// <summary>
    /// Ensures that locations are correctly matched to regions.
    /// Location "A" is inside the region polygon and should be matched.
    /// Location "B" is outside and should not be matched.
    /// </summary>
    [Fact]
    public void LocationMatchedToRegion()
    {
        var locations = new List<Location>
        {
            new Location("A", new Coordinate(0.5, 0.5)),
            new Location("B", new Coordinate(2.0, 2.0))
        };
        var regions = new List<Region>
        {
            new Region("R", new List<Polygon>
            {
                new Polygon
                {
                    new Coordinate(0.0, 0.0),
                    new Coordinate(0.0, 1.0),
                    new Coordinate(1.0, 1.0),
                    new Coordinate(1.0, 0.0),
                    new Coordinate(0.0, 0.0)
                }
            })
        };
        var result = RegionMatcher.MatchLocationsToRegions(locations, regions);
        var matched = result.First();
        Assert.Contains("A", matched.matchedLocations);
        Assert.DoesNotContain("B", matched.matchedLocations);
    }
}