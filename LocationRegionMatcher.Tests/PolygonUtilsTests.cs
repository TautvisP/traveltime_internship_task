using LocationRegionMatcher;

public class PolygonUtilsTests
{
    /// <summary>
    /// Ensures that a point strictly inside the polygon returns true.
    /// </summary>
    [Fact]
    public void PointInsidePolygon_ReturnsTrue()
    {
        var polygon = new List<double[]>
        {
            new[] {0.0, 0.0},
            new[] {0.0, 1.0},
            new[] {1.0, 1.0},
            new[] {1.0, 0.0},
            new[] {0.0, 0.0}
        };
        var point = new[] {0.5, 0.5};
        Assert.True(PolygonUtils.IsPointInPolygon(point, polygon));
    }

    /// <summary>
    /// Ensures that a point outside the polygon returns false.
    /// </summary>
    [Fact]
    public void PointOutsidePolygon_ReturnsFalse()
    {
        var polygon = new List<double[]>
        {
            new[] {0.0, 0.0},
            new[] {0.0, 1.0},
            new[] {1.0, 1.0},
            new[] {1.0, 0.0},
            new[] {0.0, 0.0}
        };
        var point = new[] {1.5, 0.5};
        Assert.False(PolygonUtils.IsPointInPolygon(point, polygon));
    }

    /// <summary>
    /// Ensures that a point exactly on a polygon vertex returns true.
    /// </summary>
    [Fact]
    public void PointOnVertex_ReturnsTrue()
    {
        var polygon = new List<double[]>
        {
            new[] {0.0, 0.0},
            new[] {0.0, 1.0},
            new[] {1.0, 1.0},
            new[] {1.0, 0.0},
            new[] {0.0, 0.0}
        };
        var point = new[] {0.0, 0.0};
        Assert.True(PolygonUtils.IsPointInPolygon(point, polygon));
    }

    /// <summary>
    /// Ensures that a point exactly on a polygon edge returns true.
    /// </summary>
    [Fact]
    public void PointOnEdge_ReturnsTrue()
    {
        var polygon = new List<double[]>
        {
            new[] {0.0, 0.0},
            new[] {0.0, 1.0},
            new[] {1.0, 1.0},
            new[] {1.0, 0.0},
            new[] {0.0, 0.0}
        };
        var point = new[] {0.5, 0.0};
        Assert.True(PolygonUtils.IsPointInPolygon(point, polygon));
    }
}