#nullable enable
using System.Collections.Generic;
using System.Linq;
using ParkingProblem.DataReading.Data;
using ParkingProblem.Solver.Move;

namespace ParkingProblem.Solver.PathFinder.Impl
{

    public class LeeShortestPathFinder : IShortestPathFinder
    {
        //define the directions LEFT, RIGHT, DOWN, UP
        private static readonly int[] Dx = {-1, 1,  0, 0};
        private static readonly int[] Dy = { 0, 0, -1, 1};

        //create the queue
        private readonly ISet<ConfigMove> _addedIntoSet;
        private readonly Queue<ConfigMove> _configQueue = new();
        
        private readonly IDictionary<ConfigMove, ConfigMove?> _parent;

        public LeeShortestPathFinder()
        {
             _addedIntoSet = new HashSet<ConfigMove>();
             _parent = new Dictionary<ConfigMove, ConfigMove?>();
        }

        public IEnumerable<ConfigMove> GetPath(MatrixData matrixData)
        {
            //clear the queue
            _configQueue.Clear();
            _addedIntoSet.Clear();

            //deconstruct the object
            var (data, n, m, (x, y)) = matrixData;

            //imagine that the matrix contains only the 'c' and '#' 
            data[0][0] = data[x][y] = 'c';

            //create the initial config
            var initialConfig = new ConfigMove(0, 0, x, y);

            //enqueue the data
            _configQueue.Enqueue(initialConfig);

            //set the parent of the config
            _parent[initialConfig] = null;

            //start the lee algorithm
            _addedIntoSet.Add(initialConfig);
            while (_configQueue.Any())
            {
                //get the front of the queue
                var currentConfig = _configQueue.Dequeue();

                //extract an item from queue
                var (freeX, freeY, carX,carY) = currentConfig;

                //if we've existed from the parking we stop
                if (carX == 0 && carY == 0)
                {
                    break;
                }

                //make those four  of the free point moves
                for (var directionIndex = 0; directionIndex < Dx.Length; ++directionIndex)
                {
                    //move the free point to a new location
                    var newX = freeX + Dx[directionIndex];
                    var newY = freeY + Dy[directionIndex];

                    //check if the move is valid
                    if (!IsMoveValid(newX, newY, carX, carY, n, m, data))
                    {
                        continue;
                    }

                    //get the new config
                    var nextConfig = new ConfigMove(newX, newY, carX, carY);

                    //if we've moved the '.' over the 'X' we need to swap
                    if (newX == carX && newY == carY)
                    {
                        //the '.' is not on the (newX, newY) and the car in on the (freeX, freeY)
                        nextConfig = new ConfigMove(newX, newY, freeX, freeY);
                    }

                    //if the point is already visited do nothing
                    if (_addedIntoSet.Contains(nextConfig))
                    {
                        continue;
                    }

                    //otherwise we are only moving the position of the free point
                    _configQueue.Enqueue(nextConfig);

                    //mark the parent of the config
                    _parent[nextConfig] = currentConfig;

                    //mark the point as visited
                    _addedIntoSet.Add(nextConfig);
                }
            }

            //if there is no solution do nothing
            return _parent.Any(key => key.Key.CarX == 0 && key.Key.CarY == 0) 
                ? ReconstructPath() 
                : new List<ConfigMove>();
        }

        /// <summary>
        /// Check if the move is valid
        /// </summary>
        /// <param name="newX">the new position of X</param>
        /// <param name="newY">the new position of Y</param>
        /// <param name="carX"></param>
        /// <param name="carY"></param>
        /// <param name="rowNumber">the number of rows</param>
        /// <param name="columnNumber">the number of columns</param>
        /// <param name="data">the matrix data</param>
        /// <returns>true if the move is valid or false otherwise</returns>
        private bool IsMoveValid(
            int newX, 
            int newY, 
            int carX,
            int carY,
            int rowNumber, 
            int columnNumber, 
            IReadOnlyList<char[]> data)
        {
            //ensure the move is inside the matrix
            if (newX < 0 || newX >= rowNumber)
            {
                return false;
            }
            if (newY < 0 || newY >= columnNumber)
            {
                return false;
            }

            //ensure that we are not moving back
            if (_addedIntoSet.Contains(new ConfigMove(newX, newY, carX, carY)))
            {
                return false;
            }
            
            //we are allowed to move the free '.' only on 'c'
            return data[newX][newY] == 'c';
        }

        /// <summary>
        /// This method can be used for reconstructing the path
        /// </summary>
        /// <returns>a list of moves</returns>
        private IEnumerable<ConfigMove> ReconstructPath()
        {
            //get the final config
            var finalConfig = _parent
                .First(x => x.Key.CarX == 0 && x.Key.CarY == 0)
                .Key;


            //create the path stack
            var configMoves = new Stack<ConfigMove>();

            //put the configs in a stack
            //since we are interested to see the moves from the begging to the end not the reversed ones
            for (var parentConfig = finalConfig; 
                 parentConfig != null;
                 parentConfig = _parent[parentConfig])
            {
                configMoves.Push(parentConfig);
            }

            //return the moves in the order (start->end)
            while (configMoves.Any())
            {
                yield return configMoves.Pop();
            }
        }
    }
}
