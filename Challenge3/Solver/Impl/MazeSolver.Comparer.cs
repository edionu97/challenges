using Challenge3.Helpers;
using System.Collections.Generic;

namespace Challenge3.Solver.Impl
{
    public partial class MazeSolver : IComparer<Point3d>
    {
        /// <summary>
        /// This method it is used for the comparison of two points in min heap
        /// </summary>
        /// <param name="x">the first point</param>
        /// <param name="y">the second point</param>
        public int Compare(Point3d x, Point3d y)
        {
            //get the first point's heuristics
            var heuristicsOfPointX = _heuristicsCostToLocationFromStartingPoint[x];

            //get the last point heuristics
            var heuristicsOfPointY = _heuristicsCostToLocationFromStartingPoint[y];

            //return the comparison
            return heuristicsOfPointX.CompareTo(heuristicsOfPointY);
        }
    }
}
