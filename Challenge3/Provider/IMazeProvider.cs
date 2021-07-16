using System.Collections.Generic;
using System.IO;
using Challenge3.Helpers;

namespace Challenge3.Provider
{
    public interface IMazeProvider
    {
        /// <summary>
        /// This method it is used for getting the 
        /// </summary>
        /// <param name="mazeFileInfo"></param>
        /// <returns></returns>
        public (IEnumerable<(Point3d, int CellValue)>, int) Get3DMaze(FileInfo mazeFileInfo);
    }
}
