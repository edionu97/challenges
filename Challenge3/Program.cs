using System;
using System.IO;
using System.Linq;
using Challenge3.Helpers;
using Challenge3.Solver.Impl;
using Challenge3.Provider.Impl;
using System.Collections.Generic;
using Challenge3.Helpers.Items;

namespace Challenge3
{
    public class Program
    {
        private static (IEnumerable<Point3d>, int)
            GetMazeExitPath(IEnumerable<(Point3d, int CellValue)> mazeInfo, Point3d enterPoint, int mazeSize)
        {
            //create the maze solver
            var mazeSolver = new MazeSolver();

            //try to solve the maze
            var (path, pathLength) = mazeSolver
                .SolveMaze(mazeInfo, enterPoint, mazeSize);

            //avoid multiple enumerations, by converting to a list
            var actualPath = path.ToList();

            //if there is no available path
            if (!actualPath.Any())
            {
                throw new Exception($"No path can be found from {enterPoint} to destination");
            }

            //get the actual path
            return (actualPath, pathLength);
        }

        public static void Main(string[] args)
        {
            //get the maze provider
            var mazeProvider = new ThreeDFileMazeProvider();

            //get the maze info
            var (mazeInfo, n) = mazeProvider
                .Get3DMaze(new FileInfo(@"Data\maze3d.txt"));

            //define the starting point
            var startPoint = new Point3d(0, 0, 0);

            try
            {
                //iterate the exit path
                var (pathCells, pathLength) = GetMazeExitPath(mazeInfo, startPoint, n);

                //display the path length message
                Console.WriteLine($"The minimum path from {startPoint} to destination is {pathLength}. Path:");

                //Display the path
                Console.WriteLine($"{string.Join('\n', pathCells)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
