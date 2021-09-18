#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ParkingProblem.DataReading.Data;
using ParkingProblem.DataReading.Matrix.Impl;
using ParkingProblem.Solver.Move;
using ParkingProblem.Solver.PathFinder.Impl;
using ParkingProblem.Utility.ExtensionMethods;

namespace ParkingProblem
{
    public static class Program
    {
        private static IEnumerable<string> ConvertMovesIntoParkingConfigs(
            ICollection<ConfigMove> configMoves, MatrixData matrixData, TextWriter writer)
        {
            //if we have no data then write the no result message
            if (!configMoves.Any())
            {
                yield break;
            }

            //get the car location
            var (x, y) = matrixData.CarLocation;

            //get the parking config
            var parkingConfig = matrixData.Data;

            //get the parking config
            parkingConfig[x][y] = parkingConfig[0][0] = 'c';

            //iterate the moves
            ConfigMove? previousMove = null;
            foreach (var configMove in configMoves)
            {
                //reset the matrix
                if (previousMove is not null)
                {
                    //deconstruct the matrix
                    var (fX, fY, cX, cY) = previousMove;

                    //reset the values
                    parkingConfig[fX][fY] = parkingConfig[cX][cY] = 'c';
                }

                //deconstruct the move
                var (freeX, freeY, carX, carY) = previousMove = configMove;

                //set the values
                parkingConfig[freeX][freeY] = '.';
                parkingConfig[carX][carY] = 'X';

                //get the parking config
                yield return parkingConfig.Stringify();
            }
        }

        /// <summary>
        /// This function should be used for opening the result file
        /// </summary>
        /// <param name="path">the path of the result file</param>
        /// <param name="configs"></param>
        /// <param name="writer"></param>
        private static void WriteTheResultAndOpenTheResultFile(
            string path, ICollection<string> configs, TextWriter writer)
        {
            //if there are not configurations it means that there is no result
            writer.WriteLine(!configs.Any()
                ? "IMPOSSIBLE"
                : $"Gigel could solve the problem, using {configs.Count - 1} moves\n");


            //iterate the configs
            foreach (var config in configs)
            {
                writer.WriteLine(config);
            }

            //try to open with default viewer the result file
            try
            {
                //try to open the result file
                new Process
                {
                    //create the start info
                    StartInfo = new ProcessStartInfo(path)
                    {
                        UseShellExecute = true
                    }

                }.Start();
            }
            catch
            {
                Console.WriteLine(
                    $"Automatically open result file failed. You should manually open the file located into {path} for seeing the result");
            }
        }

        public static async Task Main(string[] args)
        {
            //declare and start the stopwatch
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            //initialise variables
            var reader = new FileMatrixReader();
            var shortestPathFinder = new LeeShortestPathFinder();

            //read data
            await using var matrixDataStream = new FileInfo(@"Data\data.txt").OpenRead();
            var matrixData = await reader.ReadMatrixAsync(matrixDataStream);

            //get the full path of the result file
            var path = Path.GetFullPath(@"Data\out.txt");

            //clear the file
            await using (var stream = File.OpenWrite(path))
            {
                stream.SetLength(0);
            }

            //prepare the writer
            await using var resultStreamWriter = path.OpenWrite();

            //solve the problem
            var moves = shortestPathFinder.GetPath(matrixData).ToList();

            //write the result
            await resultStreamWriter.WriteLineAsync($"Elapsed: {stopWatch.Elapsed.TotalSeconds} seconds");

            //get the parking configs
            var parkingConfigs =
                ConvertMovesIntoParkingConfigs(moves, matrixData, resultStreamWriter).ToList();

            //open the result file
            WriteTheResultAndOpenTheResultFile(path, parkingConfigs, resultStreamWriter);
        }
    }
}
