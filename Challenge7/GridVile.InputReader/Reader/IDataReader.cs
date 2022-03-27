using System.IO;
using System.Threading.Tasks;
using GridVile.DataModeling.Data;

namespace GridVile.DataModeling.Reader
{
    public interface IDataReader
    {
        /// <summary>
        /// Reads data and build the navigator data
        /// </summary>
        /// <param name="dataPath">The path to the file which contains data</param>
        /// <returns>A new instance of data</returns>
        /// <exception cref="FileNotFoundException">If file does not exist</exception>
        public Task<NavigatorData> ReadDataAsync(FileInfo dataPath);
    }
}
