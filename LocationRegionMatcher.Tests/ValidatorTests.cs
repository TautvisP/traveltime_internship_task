using LocationRegionMatcher;

public class ValidatorTests
{
    /// <summary>
    /// Ensures that duplicate location names throw an InvalidDataException.
    /// </summary>
    [Fact]
    public void DuplicateLocationName_Throws()
    {
        var nameSet = new HashSet<string> { "A" };
        var loc = new Location("A", new[] { 0.0, 0.0 });
        Assert.Throws<InvalidDataException>(() => Validator.ValidateLocation(loc, nameSet));
    }

    /// <summary>
    /// Ensures that locations with invalid coordinates throw an InvalidDataException.
    /// </summary>
    [Fact]
    public void InvalidCoordinates_Throws()
    {
        var nameSet = new HashSet<string>();
        var loc = new Location("B", new[] { 0.0 });
        Assert.Throws<InvalidDataException>(() => Validator.ValidateLocation(loc, nameSet));
    }

    /// <summary>
    /// Ensures that a region with no polygons throws an InvalidDataException.
    /// </summary>
    [Fact]
    public void RegionWithNoPolygons_Throws()
    {
        var nameSet = new HashSet<string>();
        var region = new Region("R", new List<List<double[]>>());
        Assert.Throws<InvalidDataException>(() => Validator.ValidateRegion(region, nameSet));
    }

    /// <summary>
    /// Ensures that a polygon with an invalid coordinate (null) throws an InvalidDataException.
    /// </summary>
    [Fact]
    public void PolygonWithInvalidCoordinate_Throws()
    {
        var nameSet = new HashSet<string>();
        var region = new Region("R", new List<List<double[]>>
        {
            new List<double[]> { new[] { 0.0, 0.0 }, null, new[] { 1.0, 1.0 } }
        });
        Assert.Throws<InvalidDataException>(() => Validator.ValidateRegion(region, nameSet));
    }
}