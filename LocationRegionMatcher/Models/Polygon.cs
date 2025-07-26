namespace LocationRegionMatcher
{
    /// <summary>
    /// Represents a polygon as a list of coordinates.
    /// </summary>
    public class Polygon : List<Coordinate>
    {
        public Polygon() : base() { }
        public Polygon(IEnumerable<Coordinate> coords) : base(coords) { }
    }
}