using System.Linq;
using Challenge3.Helpers.Items;
using System.Collections.Generic;

namespace Challenge3.Solver.Impl
{
    public  partial class MazeSolver
    {
        /// <summary>
        /// Decide if the value is valid or not
        /// </summary>
        /// <param name="matrix">the maze matrix</param>
        /// <param name="to">the two</param>
        /// <param name="hashSet">the visited collection</param>
        /// <returns>true or false</returns>
        private static bool IsMoveValid(IDictionary<Point3d, int> matrix, Point3d to, ICollection<Point3d> hashSet)
        {
            //if the new point it is not located into the matrix the move (from => to) is invalid
            if (!matrix.ContainsKey(to))
            {
                return false;
            }

            //if the node is already visited the move is invalid
            if (hashSet.Contains(to))
            {
                return false;
            }

            //the move is valid either we are moving in an empty cell, either we've arrived at destination
            return matrix[to] == 0 || matrix[to] == -2;
        }

        /// <summary>
        /// Compute the euclidean distance between two points in 3d
        /// </summary>
        /// <param name="pointA">the first point</param>
        /// <param name="pointB"> the second point</param>
        /// <returns>a double representing the distance</returns>
        private static double ComputeSquaredEuclideanDistance(Point3d pointA, Point3d pointB)
        {
            //deconstruct the first point
            var (x1, y1, z1) = pointA;

            //deconstruct the second point
            var (x2, y2, z2) = pointB;

            //compute the squared euclidean distance
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2);
        }

        /// <summary>
        /// Initialize the matrix
        /// </summary>
        /// <param name="mazeInfo">the maze information</param>
        /// <returns>a pair (the dictionary representing the matrix and the cell value)</returns>
        private static (IDictionary<Point3d, int>, Point3d)
            GetThe3dMatrixAndEndingPoint(IEnumerable<(Point3d Point, int CellValue)> mazeInfo)
        {
            //create the matrix (represent it internally as dictionary)
            var coordinatesToCellValue = mazeInfo
                .ToDictionary(
                    keySelector => keySelector.Point,
                    valueSelector => valueSelector.CellValue);//get the exit cell coordinates

            //get the ending point
            var endingPoint = coordinatesToCellValue
                .First(x => x.Value == -2).Key;//get the exit cell coordinates

            //the required information
            return (coordinatesToCellValue, endingPoint);
        }

    }
}
