using System.Collections.Generic;
using GridVile.DataModeling.Grid;
using GridVile.DataModeling.Coordinates;

namespace GridVile.Navigator.ShortestPathComputer.Metadata
{
    public record ShortestPathMetadata
    {
        public IEnumerable<Point> Path { get; init; }

        public Matrix<int> DistanceMatrix { get; init; }

        public void Deconstruct(
            out IEnumerable<Point> path, 
            out Matrix<int> distanceMatrix)
        {
            path = Path;
            distanceMatrix = DistanceMatrix;
        }
    }
}
