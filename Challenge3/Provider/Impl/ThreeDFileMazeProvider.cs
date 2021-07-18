using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Challenge3.Helpers;
using Challenge3.Helpers.Items;

namespace Challenge3.Provider.Impl
{
    public class ThreeDFileMazeProvider : IMazeProvider
    {
        public (IEnumerable<(Point3d, int CellValue)>, int) Get3DMaze(FileInfo mazeFileInfoFile)
        {
            //get the reader
            using var reader = new StreamReader(mazeFileInfoFile.OpenRead());

            //ignore the first line
            int.TryParse(reader.ReadLine(), out var n);

            var list = new List<(Point3d, int CellValue)>();

            //create the 3d matrix
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                //split the items
                var rowElements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                //parse the lines
                int.TryParse(rowElements[0], out var x);
                int.TryParse(rowElements[1], out var y);
                int.TryParse(rowElements[2], out var z);
                int.TryParse(rowElements[3], out var value);

                //return the matrix
                list.Add((new Point3d(x, y,z), value));
            }

            //return the matrix
            return (list, n);
        }
    }
}
