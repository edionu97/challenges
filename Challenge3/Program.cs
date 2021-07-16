using System;
using System.IO;
using System.Linq;
using Challenge3.Helpers;
using Challenge3.Provider.Impl;
using Challenge3.Solver.Impl;

namespace Challenge3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //get the maze provider
            var mazeProvider = new ThreeDFileMazeProvider();

            //get the maze info
            var (mazeInfo, n) = mazeProvider
                .Get3DMaze(new FileInfo(@"Data\maze3d.txt"));

            //create the maze solver
            var mazeSolver = new MazeSolver();

            var startPoint = new Point3d(0, 0, 0);

            var (path, pathLength) = 
                mazeSolver.SolveMaze(mazeInfo, new Point3d(0, 0, 0), n);

            var actualPath = path.ToList();

            if (!actualPath.Any())
            {
                Console.WriteLine($"No path can be found from {startPoint} to destination");
                return;
            }
            
            Console.WriteLine(pathLength);
            foreach (var point3d in actualPath)
            {
                Console.WriteLine(point3d);
            }
        }
    }
}
