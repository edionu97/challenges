using System.Linq;
using System.Collections.Generic;
using GridVile.DataModeling.Data;
using GridVile.DataModeling.Grid;
using GridVile.DataModeling.Coordinates;
using GridVile.Navigator.ShortestPathComputer.Metadata;

namespace GridVile.Navigator.ShortestPathComputer.Impl
{
    public partial class LeeBasedShortestPathComputer : IShortestPathComputer
    {
        /// <summary>
        /// This method computes the shortest path and returns also the metadata
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <returns>The path metadata</returns>
        public ShortestPathMetadata ComputeShortestPath(NavigatorData navigatorData)
        {
            //create the matrix
            var matrix = Create2DModelMatrix(navigatorData);

            //create the queue
            var queue = new Queue<Point>();

            //put the starting point in queue
            queue.Enqueue(navigatorData.StartPoint);

            //create the visited dictionary
            var visited = new Dictionary<Point, Point>
            {
                [navigatorData.StartPoint] = null
            };

            //as long as there is data
            while (queue.Any())
            {
                //get the current point
                var parentPoint = queue.Dequeue();

                //stop when we solve it
                if (parentPoint == navigatorData.EndPoint)
                {
                    break;
                }

                //deconstruct the parent point
                var (x, y) = parentPoint;

                //try to move from current location to new location
                for (var index = 0; index < Dx.Length; ++index)
                {
                    //create new coordinates
                    var newPoint = 
                        new Point(x + Dx[index], y + Dy[index]);

                    //ignore invalid moves
                    if (!IsValidMove(matrix, newPoint, visited))
                    {
                        continue;
                    }

                    //increment distance to new point
                    matrix[newPoint] = matrix[parentPoint] + 1;

                    //add the visited point
                    visited[newPoint] = parentPoint;

                    //put the new point into queue
                    queue.Enqueue(newPoint);
                }
            }

            //return the shortest path metadata
            return new ShortestPathMetadata
            {
                DistanceMatrix = matrix,
                Path = RebuildTrip(navigatorData, visited)
            };
        }

        /// <summary>
        /// This function returns true if the move to <paramref name="newPoint"/> is valid
        /// </summary>
        /// <param name="matrix">The distance matrix</param>
        /// <param name="newPoint">The new point coordinates</param>
        /// <param name="visited">The visited information</param>
        /// <returns>True if the move is valid or false otherwise</returns>
        private static bool IsValidMove(
            Matrix<int> matrix, 
            Point newPoint, 
            IDictionary<Point, Point> visited)
                => matrix.RespectsMatrixBoundaries(newPoint) 
                   && !visited.ContainsKey(newPoint) 
                   && matrix[newPoint] != -1;
    }
}
