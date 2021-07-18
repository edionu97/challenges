using Challenge3.Helpers.Items;
using System.Collections.Generic;

namespace Challenge3.Solver
{
    public interface IMazeSolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mazeInfo"></param>
        /// <param name="startingPoint"></param>
        /// <param name="n"></param>
        public (IEnumerable<Point3d> Path, int PathLength) SolveMaze(
            IEnumerable<(Point3d Point, int CellValue)> mazeInfo, Point3d startingPoint,  int n);
    }
}
