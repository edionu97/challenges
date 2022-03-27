using System.Collections.Generic;
using GridVile.DataModeling.Coordinates;
using GridVile.DataModeling.Data;

namespace GridVile.Navigator.Solver
{
    public interface IGridVileNavigator
    {
        public IEnumerable<Point> GetShortestTrip(NavigatorData navigatorData, out bool areDropPoints);

    }
}
