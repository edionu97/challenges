using System.Collections.Generic;
using GridVile.DataModeling.Coordinates;

namespace GridVile.DataModeling.Data
{
    public class NavigatorData
    {
        public int RowNo { get; set; }

        public int ColNo { get; set; }

        public Point StartPoint { get; set; }

        public Point EndPoint { get; set; }

        public List<Point> Jams { get; init; } = new();
    }
}
