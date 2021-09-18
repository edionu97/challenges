using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingProblem.DataReading.Data;
using ParkingProblem.Solver.Move;

namespace ParkingProblem.Solver.PathFinder
{
    public interface IShortestPathFinder
    {
        /// <summary>
        /// Get the shortest path 
        /// </summary>
        /// <param name="matrixData">the matrix data</param>
        /// <returns>a list that contains the locations of the free cell</returns>
        public IEnumerable<ConfigMove> GetPath(MatrixData matrixData);
    }
}
