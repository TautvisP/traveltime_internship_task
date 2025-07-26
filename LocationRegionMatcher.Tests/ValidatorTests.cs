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
        var loc = new Location("A", new Coordinate(0.0, 0.0));
        Assert.Throws<InvalidDataException>(() => Validator.ValidateLocation(loc, nameSet));
    }

    /// <summary>
    /// Ensures that locations with invalid coordinates throw an InvalidDataException.
    /// </summary>
    [Fact]
    public void InvalidCoordinates_Throws()
    {
        var nameSet = new HashSet<string>();
        // Invalid: null coordinates
        var loc = new Location("B", null!);
        Assert.Throws<InvalidDataException>(() => Validator.ValidateLocation(loc, nameSet));
    }

    /// <summary>
    /// Ensures that a region with no polygons throws an InvalidDataException.
    /// </summary>
    [Fact]
    public void RegionWithNoPolygons_Throws()
    {
        var nameSet = new HashSet<string>();
        var region = new Region("R", new List<Polygon>());
        Assert.Throws<InvalidDataException>(() => Validator.ValidateRegion(region, nameSet));
    }

    /// <summary>
    /// Ensures that a polygon with an invalid coordinate (null) throws an InvalidDataException.
    /// </summary>
    [Fact]
    public void PolygonWithInvalidCoordinate_Throws()
    {
        var nameSet = new HashSet<string>();
        var poly = new Polygon
        {
            new Coordinate(0.0, 0.0),
            null!,
            new Coordinate(1.0, 1.0)
        };
        var region = new Region("R", new List<Polygon> { poly });
        Assert.Throws<InvalidDataException>(() => Validator.ValidateRegion(region, nameSet));
    }

    /// <summary>
    /// Ensures that a polygon that is not closed (first and last point differ) throws an InvalidDataException.
    /// </summary>
    [Fact]
    public void PolygonNotClosed_Throws()
    {
        var nameSet = new HashSet<string>();
        var poly = new Polygon
        {
            new Coordinate(0.0, 0.0),
            new Coordinate(0.0, 1.0),
            new Coordinate(1.0, 1.0),
            new Coordinate(1.0, 0.0)
            // Missing closing point (should be new Coordinate(0.0, 0.0))
        };
        var region = new Region("R", new List<Polygon> { poly });
        Assert.Throws<InvalidDataException>(() => Validator.ValidateRegion(region, nameSet));
    }
}