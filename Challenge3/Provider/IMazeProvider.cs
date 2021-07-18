using System.IO;
using Challenge3.Helpers.Items;
using System.Collections.Generic;

namespace Challenge3.Provider
{
    public interface IMazeProvider
    {
        /// <summary>
        /// This method it is used for getting the items that are describing the 3d maxe
        /// The cells (coordinates) and their value (0, -1, -2)
        /// </summary>
        /// <param name="mazeFileInfoFile">the file</param>
        /// <returns></returns>
        public (IEnumerable<(Point3d, int CellValue)>, int) Get3DMaze(FileInfo mazeFileInfoFile);
    }
}
