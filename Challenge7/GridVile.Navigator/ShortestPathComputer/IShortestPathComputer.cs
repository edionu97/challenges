using GridVile.DataModeling.Data;
using GridVile.Navigator.ShortestPathComputer.Metadata;

namespace GridVile.Navigator.ShortestPathComputer
{
    public interface IShortestPathComputer
    {
        /// <summary>
        /// Computes the shortest path, if this exists between two points
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <returns>
        /// A new instance of shortest path metadata which contains info about path, <br/>
        /// point distances and path reconstruction</returns>
        public ShortestPathMetadata ComputeShortestPath(NavigatorData navigatorData);
    }
}
