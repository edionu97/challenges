using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using GridVile.DataModeling.Data;

namespace GridVile.DataModeling.Reader.Impl
{
    public partial class KeywordDataReader : IDataReader
    {
        private readonly Dictionary<string, Action<NavigatorData, string>> _keywordActions = new();

        public KeywordDataReader()
        {
            //define the grid dimension action
            _keywordActions["Grid dimensions"] = SetMatrixDimensions;
            _keywordActions["Start point"] = SetStartPoint;
            _keywordActions["End point"] = SetEndPoint;
            _keywordActions["Traffic jams"] = SetTrafficJams;
        }

        public async Task<NavigatorData> ReadDataAsync(FileInfo dataPath)
        {
            //null check
            if (dataPath is null)
            {
                throw new ArgumentNullException(nameof(dataPath));
            }

            //ensure file exists
            if (!dataPath.Exists)
            {
                throw new FileNotFoundException(dataPath.FullName);
            }

            //declare the reader
            using var reader = new StreamReader(dataPath.OpenRead());

            //get navigator data
            var navigatorData = new NavigatorData();

            //start reading the line
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                //get the match
                var match = DataSetterRegex.Match(line);
                if (!match.Success)
                {
                    continue;
                }

                //get command name
                var commandName = match
                    .Groups["commandName"]
                    .Value
                    .Trim();

                //get command value
                var commandValue = match
                    .Groups["commandValue"]
                    .Value
                    .Trim();

                //ignore un-existent command
                if (!_keywordActions.ContainsKey(commandName))
                {
                    continue;
                }

                //execute the proper set action
                _keywordActions[commandName]
                    .Invoke(navigatorData, commandValue);
            }

            //return navigator data instance
            return navigatorData;
        }
    }
}
