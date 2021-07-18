using System;
using System.IO;
using Challenge3.Helpers;
using System.Collections.Generic;
using Challenge3.Helpers.Items;

namespace Challenge3.Provider.Impl
{
    public class TwoDToThreeDMazeProvider : IMazeProvider
    {
        public (IEnumerable<(Point3d, int CellValue)>, int) Get3DMaze(FileInfo mazeFileInfoFile)
        {
            //get the reader
            using var reader = new StreamReader(mazeFileInfoFile.OpenRead());

            //read the first line and get the number of rows / cols
            int.TryParse(reader.ReadLine(), out var n);

            //get the 3d maze
            return (Get3DMaze(mazeFileInfoFile, n), n);
        }

        private static IEnumerable<(Point3d, int CellValue)> 
            Get3DMaze(FileInfo twoDimensionalTemplate, int n)
        {
            //get the reader
            using var reader = new StreamReader(twoDimensionalTemplate.OpenRead());

            //ignore the first line
            reader.ReadLine();

            //create the 3d matrix
            string line;
            var xIndex = 0;
            while ((line = reader.ReadLine()) != null)
            {
                //split the items
                var rowElements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                //process the first ro
                for (var yIndex = 0; yIndex < n; ++yIndex)
                {
                    //get the value of the row index
                    int.TryParse(rowElements[yIndex], out var rowElement);

                    //describe a 3d cell value
                    for (var zIndex = 0; zIndex < n; ++zIndex)
                    {
                        yield return (new Point3d(xIndex, yIndex, zIndex), -rowElement);
                    }
                }

                //move to next line
                ++xIndex;
            }
        }
    }
}
