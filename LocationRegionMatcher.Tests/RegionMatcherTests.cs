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
            new Location("A", new[] { 0.5, 0.5 }),
            new Location("B", new[] { 2.0, 2.0 })
        };
        var regions = new List<Region>
        {
            new Region("R", new List<List<double[]>>
            {
                new List<double[]>
                {
                    new[] { 0.0, 0.0 },
                    new[] { 0.0, 1.0 },
                    new[] { 1.0, 1.0 },
                    new[] { 1.0, 0.0 },
                    new[] { 0.0, 0.0 }
                }
            })
        };
        var result = RegionMatcher.MatchLocationsToRegions(locations, regions);
        var matched = result.First();
        Assert.Contains("A", matched.matched_locations);
        Assert.DoesNotContain("B", matched.matched_locations);
    }
}