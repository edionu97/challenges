using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ParkingProblem.DataReading.Data;

namespace ParkingProblem.DataReading.Matrix.Impl
{
    public class FileMatrixReader : IMatrixReader
    {
        private static readonly Regex RowColumnIdentifier = new(@"(?<rows>[1-9][0-9]*)\s+(?<columns>[1-9][0-9]*)");

        public async Task<MatrixData> ReadMatrixAsync(Stream sourceStream)
        {
            //get the stream
            var streamReader = new StreamReader(sourceStream);

            //read the first line from the file
            var line = await streamReader.ReadLineAsync() ?? string.Empty;

            //declare the variables
            var rowCount = 0;
            var columnCount = 0;

            //if there is a match
            if (RowColumnIdentifier.IsMatch(line))
            {
                int.TryParse(RowColumnIdentifier.Match(line).Groups["rows"].Value, out rowCount);
                int.TryParse(RowColumnIdentifier.Match(line).Groups["columns"].Value, out columnCount);
            }

            //get the matrix data
            var matrixData = await ReadMatrixAsync(streamReader);

            //get the car location
            var location = 
                GetCarLocation(matrixData) ?? throw new Exception("Data is incorrect");

            //initialize the matrix data
            return new MatrixData
            {
                Data = matrixData,
                RowCount = rowCount,
                CarLocation = location,
                ColumnCount = columnCount
            };
        }

        /// <summary>
        /// This is a helper method used for reading all the lines
        /// </summary>
        /// <param name="reader">the reader</param>
        /// <returns>the list of rows which represents the matrix</returns>
        private static async Task<char[][]> ReadMatrixAsync(TextReader reader)
        {
            //define the matrix lines
            var matrixLines = new List<char[]>();

            //read the stream line by line
            string line;
            while ((line = await reader.ReadLineAsync()) is not null)
            {
                //add the line
                matrixLines.Add(line.ToCharArray());
            }

            //return  the array
            return matrixLines.ToArray();
        }

        /// <summary>
        /// Get the location of the car
        /// </summary>
        /// <param name="data">this represents the input matrix</param>
        /// <returns>the coordination of the car or null if there is no cell with value X</returns>
        private static (int X, int Y)? GetCarLocation(IReadOnlyList<char[]> data)
        {
            //get the car coordination
            for (var x = 0; x < data.Count; ++x)
            {
                for (var y = 0; y < data[x].Length; ++y)
                {
                    //if in the cell is no car continue
                    if ((data[x][y] + "").ToLower() != "x")
                    {
                        continue;
                    }

                    return (x, y);
                }
            }

            //return null if data is incorrect
            return null;
        }
    }
}
