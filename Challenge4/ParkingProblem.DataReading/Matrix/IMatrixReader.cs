using System.IO;
using System.Threading.Tasks;
using ParkingProblem.DataReading.Data;

namespace ParkingProblem.DataReading.Matrix
{
    public interface IMatrixReader
    {
        /// <summary>
        /// Reads the matrix and returns its rows
        /// </summary>
        /// <param name="sourceStream">the source stream containing the matrix data</param>
        /// <returns>an instance of matrix data representing the data that is read from the sourceStream</returns>
        public Task<MatrixData> ReadMatrixAsync(Stream sourceStream);
    }
}
