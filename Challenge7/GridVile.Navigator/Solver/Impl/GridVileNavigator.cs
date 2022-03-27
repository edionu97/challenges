using System.Linq;
using System.Collections.Generic;
using GridVile.DataModeling.Data;
using GridVile.DataModeling.Coordinates;
using GridVile.DataModeling.Grid;
using GridVile.Navigator.ShortestPathComputer;

namespace GridVile.Navigator.Solver.Impl
{
    public class GridVileNavigator : IGridVileNavigator
    {
        private readonly IShortestPathComputer _shortestPathComputer;

        public GridVileNavigator(IShortestPathComputer shortestPathComputer)
        {
            _shortestPathComputer = shortestPathComputer;
        }

        public IEnumerable<Point> GetShortestTrip(NavigatorData navigatorData, out bool areDropPoints)
        {
            //compute the start to end shortest path
            var startToEndPathMetadata =
                    _shortestPathComputer
                        .ComputeShortestPath(navigatorData);

            //check if the result will be drop points
            areDropPoints = !startToEndPathMetadata.Path.Any();

            //return path
            if (!areDropPoints)
            {
                return startToEndPathMetadata.Path;
            }

            //compute the end to start shortest path
            var endToStartMetadata =
                    _shortestPathComputer
                        .ComputeShortestPath(new NavigatorData
                        {
                            ColNo = navigatorData.ColNo,
                            RowNo = navigatorData.RowNo,
                            StartPoint = navigatorData.EndPoint,
                            EndPoint = navigatorData.StartPoint,
                            Jams = navigatorData.Jams
                        });

            //get the border points 
            //since there is no path between starting point and end point there must be a border between those two regions
            //convert them to set since is faster in finding
            var borderPoints = navigatorData.Jams.ToHashSet();

            //sort all the points by dropping cost
            //choose first one with the lower cost
            var droppingPointsByCost =
                GetAllPossibleDroppingPoints(
                        borderPoints,
                        startToEndPathMetadata.DistanceMatrix,
                        endToStartMetadata.DistanceMatrix)
                    .GroupBy(x => x.Cost)
                    .ToDictionary(
                        x => x.Key, 
                        x => x.Select(v => v.Point));

            //return dropping points
            return droppingPointsByCost[droppingPointsByCost.Min(x => x.Key)];
        }

        /// <summary>
        /// This function computes a list with all the possible dropping points<br/>
        /// </summary>
        /// <param name="borderPoints">The border points</param>
        /// <param name="startToEndMatrix">The distance matrix generated </param>
        /// <param name="endToStartMatrix"></param>
        /// <returns>A tuple (the dropping point and the total cost of choosing that point)</returns>
        private static IEnumerable<(Point Point, int Cost)> GetAllPossibleDroppingPoints(
            ICollection<Point> borderPoints, Matrix<int> startToEndMatrix, Matrix<int> endToStartMatrix)
        {
            //get pair of directions
            var dx = new[] { 1, 0 };
            var dy = new[] { 0, 1 };

            //iterate the points
            foreach (var (x, y) in borderPoints)
            {
                //get the neighbor points
                var neighborPoints = new List<IReadOnlyCollection<Point>>();

                //try to get two points that are belonging two those two separated regions
                //region from start to end and from end to start
                // ReSharper disable once LoopCanBeConvertedToQuery
                for (var index = 0; index < dx.Length; index++)
                {
                    //compute points
                    var points = new[]
                    {
                        new Point(x - dx[index], y - dy[index]),
                        new Point(x + dx[index], y + dy[index])
                    };

                    //all points should be in the matrix limits
                    if (!points.All(startToEndMatrix.RespectsMatrixBoundaries))
                    {
                        continue;
                    }

                    //all points should not be part of border
                    if (points.Any(borderPoints.Contains))
                    {
                        continue;
                    }

                    //get the points
                    var first = points.First();
                    var second = points.Last();

                    //check if is a valid move
                    var isValidMove = IsOk(
                        first,
                        second,
                        startToEndMatrix,
                        endToStartMatrix);

                    //one point should be in one region and another in the other region
                    if (!isValidMove)
                    {
                        continue;
                    }

                    //add the neighbor points
                    neighborPoints.Add(points);
                }

                //get the points
                foreach (var points in neighborPoints)
                {
                    //get points from each region
                    var endRegionPoint = points.First(endToStartMatrix.HasPoint);
                    var startRegionPoint = points.First(startToEndMatrix.HasPoint);

                    //compute the travel distance (dropping point cost)
                    var droppingPointCost =
                        endToStartMatrix[endRegionPoint] + startToEndMatrix[startRegionPoint];

                    //compute the total trip distance if we choose as drop-point start region point
                    yield return (startRegionPoint, droppingPointCost);
                }
            }
        }

        /// <summary>
        /// Ensure points are in different matrices
        /// </summary>
        private static bool IsOk(Point a, Point b, Matrix<int> firstMatrix, Matrix<int> secondMatrix)
        {
            if (firstMatrix.HasPoint(a) && secondMatrix.HasPoint(b))
            {
                return firstMatrix[a] >= 0 && secondMatrix[b] >= 0;
            }

            if (firstMatrix.HasPoint(b) && secondMatrix.HasPoint(a))
            {
                return firstMatrix[b] >= 0 && secondMatrix[a] >= 0;
            }

            return false;
        }
    }

}
