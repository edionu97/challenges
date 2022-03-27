using System.Collections.Generic;
using System.Linq;
using GridVile.DataModeling.Coordinates;
using GridVile.DataModeling.Data;
using GridVile.DataModeling.Grid;

namespace GridVile.Navigator.ShortestPathComputer.Impl
{
    public partial class LeeBasedShortestPathComputer
    {
        //define directions (left, right, bottom, top)
        private static readonly int[] Dx = { -1, 1, 0, 0 };
        private static readonly int[] Dy = { 0, 0, -1, 1 };

        /// <summary>
        /// Used for rebuilding the trip
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <param name="visited">The visited list</param>
        /// <returns>A list of points that represents the trip coordinates</returns>
        private static IEnumerable<Point> RebuildTrip(NavigatorData navigatorData, IDictionary<Point, Point> visited)
        {
            //if we did not reach the destination do nothing
            if (!visited.ContainsKey(navigatorData.EndPoint))
            {
                yield break;
            }

            //rebuild the steps
            var stack = new Stack<Point>();
            for (var point = navigatorData.EndPoint; point != null; point = visited[point])
            {
                stack.Push(point);
            }

            //return the steps in the correct order (from start to end)
            while (stack.Any())
            {
                yield return stack.Pop();
            }
        }

        /// <summary>
        /// Helper method for creating the 2d model matrix
        /// </summary>
        /// <param name="navigatorData">Build the navigator data</param>
        /// <returns>An instance of matrix</returns>
        private static Matrix<int> Create2DModelMatrix(NavigatorData navigatorData)
        {
            //build the matrix
            var matrix = new Matrix<int>(
                navigatorData.RowNo, navigatorData.ColNo)
            {
                [navigatorData.StartPoint] = 0
            };


            //set the jam points
            foreach (var jamPoint in navigatorData.Jams)
            {
                matrix[jamPoint] = -1;
            }

            return matrix;
        }
    }
}
