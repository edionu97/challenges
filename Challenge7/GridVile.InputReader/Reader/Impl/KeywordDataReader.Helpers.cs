using System;
using System.Text.RegularExpressions;
using GridVile.DataModeling.Coordinates;
using GridVile.DataModeling.Data;

namespace GridVile.DataModeling.Reader.Impl
{
    public partial class KeywordDataReader
    {
        //define the constant values
        private static Regex DimRegex => new("(?<rowNo>[0-9]+)[^0-9]+(?<colNo>[0-9]+)");

        private static Regex PointRegex => new(@"\((?<xVal>[0-9]+)\s*,\s*(?<yVal>[0-9]+)\)");

        private static Regex DataSetterRegex => new(@"(?<commandName>[^\:]+)\s*:\s*(?<commandValue>.+)");

        /// <summary>
        /// Helper for setting the matrix dimensions
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <param name="stringValue">The line value</param>
        private static void SetMatrixDimensions(
            NavigatorData navigatorData, string stringValue)
        {
            //get the match and ensure success
            var match = DimRegex.Match(stringValue);
            if (!match.Success)
            {
                throw new ArgumentException("Bad format for: 'Grid dimensions:'");
            }

            //parse the values
            _ = int.TryParse(match.Groups["rowNo"].Value, out var rowNo);
            _ = int.TryParse(match.Groups["colNo"].Value, out var colNo);

            //set the proper values
            navigatorData.RowNo = rowNo + 1;
            navigatorData.ColNo = colNo + 1;
        }

        /// <summary>
        /// Helper for setting the matrix dimensions
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <param name="stringValue">The line values</param>
        private static void SetStartPoint(
            NavigatorData navigatorData, string stringValue)
                => navigatorData.StartPoint = GetPointValue(stringValue);

        /// <summary>
        /// Helper for setting the matrix dimensions
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <param name="stringValue">The line values</param>
        private static void SetEndPoint(
            NavigatorData navigatorData, string stringValue)
                => navigatorData.EndPoint = GetPointValue(stringValue);

        /// <summary>
        /// Set traffic jams
        /// </summary>
        /// <param name="navigatorData">The navigator data</param>
        /// <param name="stringValue">The string value</param>
        public static void SetTrafficJams(
            NavigatorData navigatorData, string stringValue)
        {
            //define the match
            for (var match = PointRegex.Match(stringValue);
                 match.Success;
                 match = match.NextMatch())
            {
                //get the match value
                var value = match.Value;

                //add jam into jams
                navigatorData
                    .Jams
                    .Add(GetPointValue(value));
            }
        }

        /// <summary>
        /// Get the value of a point 
        /// </summary>
        /// <param name="line">The line containing the point data</param>
        /// <returns>A new point location</returns>
        private static Point GetPointValue(string line)
        {
            //get the match and ensure success
            var match = PointRegex.Match(line);
            if (!match.Success)
            {
                throw new ArgumentException("Bad format for: 'Point:'");
            }

            //parse the values
            _ = int.TryParse(match.Groups["xVal"].Value, out var xVal);
            _ = int.TryParse(match.Groups["yVal"].Value, out var yVal);

            //return the xVal, yVal
            return new Point(xVal, yVal);
        }
    }
}
