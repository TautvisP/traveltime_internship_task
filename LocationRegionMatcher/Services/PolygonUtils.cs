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
            double pointLon = point[0];
            double pointLat = point[1];
            int vertexCount = polygon.Count;
            int prev = vertexCount - 1;
            bool isInside = false;

            for (int curr = 0; curr < vertexCount; prev = curr++)
            {
                double[] vertexA = polygon[curr];
                double[] vertexB = polygon[prev];

                double lonA = vertexA[0], latA = vertexA[1];
                double lonB = vertexB[0], latB = vertexB[1];

                // Check if the point matches either vertex exactly
                if ((lonA == pointLon && latA == pointLat) ||
                    (lonB == pointLon && latB == pointLat))
                    return true;

                // Check if the ray from the point crosses the edge (vertexB to vertexA)
                bool crossesLatitude = (latA > pointLat) != (latB > pointLat);
                if (crossesLatitude)
                {
                    // Calculate intersection of edge with horizontal line at point's latitude
                    double intersectionLon =
                        (lonB - lonA) * (pointLat - latA) / (latB - latA) + lonA;

                    // Check if the point is exactly on the edge
                    if (pointLon == intersectionLon)
                        return true;

                    // Toggle isInside if intersection is to the right of the point
                    if (pointLon < intersectionLon)
                        isInside = !isInside;
                }
            }
            return isInside;
        }
    }
}