using System.Linq;
using Challenge3.Helpers;
using System.Collections.Generic;
using Challenge3.Helpers.Items;

namespace Challenge3.Solver.Impl
{
    public partial class MazeSolver : IMazeSolver
    {
        //define the 3d directions (left, right, bottom, top, near, far) coordinates modification
        private static readonly int[] Dx = { -1, 1, 0, 0, 0, 0 };
        private static readonly int[] Dy = { 0, 0, -1, 1, 0, 0 };
        private static readonly int[] Dz = { 0, 0, 0, 0, -1, 1 };

        private readonly IDictionary<Point3d, double> _heuristicsCostToLocationFromStartingPoint 
            = new Dictionary<Point3d, double>();

        private readonly IDictionary<Point3d, int> _distanceToLocationFromStartingPoint 
            = new Dictionary<Point3d, int>();

        public MazeSolver()
        {
            _heap = new C5.IntervalHeap<Point3d>(this);
        }

        public (IEnumerable<Point3d> Path, int PathLength) SolveMaze (IEnumerable<(Point3d Point, int CellValue)> mazeInfo, Point3d startingPoint, int n)
        {
            _heuristicsCostToLocationFromStartingPoint.Clear();
            _distanceToLocationFromStartingPoint.Clear();
            _pathReconstruction.Clear();

            //get the matrix and the ending point
            var (matrix, endPoint) = GetThe3dMatrixAndEndingPoint(mazeInfo);

            //initialize the distance vector
            InitializeDistanceAndHeuristics(matrix, startingPoint, endPoint);

            //add element in heap
            AddOrUpdateElementHeap(startingPoint);

            //start the a* algorithm
            var visitedPoints = new HashSet<Point3d> {startingPoint};

            //the stating point does not have a parent
            _pathReconstruction[startingPoint] = null;

            //until we have elements into heap
            while (!_heap.IsEmpty)
            {
                //get the current point
                var currentPoint = _heap.DeleteMin();

                //mark the point as visited
                visitedPoints.Add(currentPoint);

                //we've exited from the maze
                if (matrix[currentPoint] == -2)
                {
                    break;
                }

                //get the item with the min heuristic cost from heap
                var (x, y, z) = currentPoint;

                //make a move 
                for (var index = 0; index < Dx.Length; ++index)
                {
                    //compute the new coordinates
                    var newX = x + Dx[index];
                    var newY = y + Dy[index];
                    var newZ = z + Dz[index];

                    //create a new point
                    var newPoint = new Point3d(newX, newY, newZ);

                    //if the move is not valid do nothing
                    if (!IsMoveValid(matrix, newPoint, visitedPoints))
                    {
                        continue;
                    }

                    //if the distance to the new point it's higher than the distance from this point to it
                    if (_distanceToLocationFromStartingPoint[newPoint] <=
                        _distanceToLocationFromStartingPoint[currentPoint] + 1)
                    {
                        continue;
                    }

                    //get the new point distance
                    _distanceToLocationFromStartingPoint[newPoint] =
                        _distanceToLocationFromStartingPoint[currentPoint] + 1;

                    //update the heuristics
                    _heuristicsCostToLocationFromStartingPoint[newPoint] =
                        _distanceToLocationFromStartingPoint[newPoint] + ComputeSquaredEuclideanDistance(newPoint, endPoint);

                    //keep the reconstruction path
                    _pathReconstruction[newPoint] = currentPoint;

                    //add new element into heap
                    AddOrUpdateElementHeap(newPoint);
                }
            }

            //return the path and the minimum path value
            return (ReconstructPath(endPoint), _distanceToLocationFromStartingPoint[endPoint]);
        }

        /// <summary>
        /// This method it is used for initializing the distance matrices
        /// </summary>
        /// <param name="matrix">the maze matrix</param>
        /// <param name="startingPoint">the starting point</param>
        /// <param name="endPoint">the ending point</param>
        private void InitializeDistanceAndHeuristics(
            IDictionary<Point3d, int> matrix, Point3d startingPoint, Point3d endPoint)
        {
            //initialize the distance vector
            foreach (var key in matrix.Keys)
            {
                //initialize the heuristic cost of oo
                _heuristicsCostToLocationFromStartingPoint[key] = !key.Equals(startingPoint)
                    ? double.MaxValue
                    : ComputeSquaredEuclideanDistance(startingPoint, endPoint);

                //initialize the distance vector
                _distanceToLocationFromStartingPoint[key] = !key.Equals(startingPoint) ? int.MaxValue : 0;
            }
        }
    }
}
