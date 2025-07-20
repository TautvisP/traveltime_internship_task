namespace LocationRegionMatcher
{
    public static class PolygonUtils
    {
        /// <summary>
        /// Determines if a point is inside a polygon using the ray casting algorithm.
        /// Returns true if the point is inside or exactly on the edge of the polygon.
        /// </summary>
        /// <param name="point">Array of two doubles representing [longitude, latitude].</param>
        /// <param name="polygon">List of coordinates representing the polygon.</param>
        /// <returns>True if the point is inside or on the edge, false otherwise.</returns>
        public static bool IsPointInPolygon(double[] point, List<double[]> polygon)
        {
            int vertexCount = polygon.Count;
            int previousVertex = vertexCount - 1;
            bool isInside = false;

            for (int currentVertex = 0; currentVertex < vertexCount; previousVertex = currentVertex++)
            {
                double[] vertexA = polygon[currentVertex];
                double[] vertexB = polygon[previousVertex];

                // Check if the point matches either vertex exactly
                if ((vertexA[0] == point[0] && vertexA[1] == point[1]) ||
                    (vertexB[0] == point[0] && vertexB[1] == point[1]))
                    return true;

                // Check if the ray from the point crosses the edge (vertexB to vertexA)
                bool crossesLatitude = (vertexA[1] > point[1]) != (vertexB[1] > point[1]);
                if (crossesLatitude)
                {
                    // Calculate intersection of edge with horizontal line at point's latitude
                    double intersectionLongitude =
                        (vertexB[0] - vertexA[0]) * (point[1] - vertexA[1]) / (vertexB[1] - vertexA[1]) + vertexA[0];

                    // Check if the point is exactly on the edge
                    if (point[0] == intersectionLongitude)
                        return true;

                    // Toggle isInside if intersection is to the right of the point
                    if (point[0] < intersectionLongitude)
                        isInside = !isInside;
                }
            }
            return isInside;
        }
    }
}