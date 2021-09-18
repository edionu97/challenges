using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ParkingProblem.Utility.ExtensionMethods
{
    public static class PrimitivesExtensions
    {
        public static string Stringify(this IEnumerable<char[]> matrix)
        {
            //create a new builder
            var builder = new StringBuilder();

            //iterate the matrix
            foreach (var line in matrix)
            {
                //append a new line
                builder.AppendLine(new string(line));
            }

            return builder.ToString();
        }

        public static StreamWriter OpenWrite (this string path)
        {
            //check the path
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("The path must not be empty");
            }

            //get the file info
            var fileInfo = new FileInfo(Path.GetFullPath(path));
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(fileInfo.Name);
            }

            //return the writer
            return new StreamWriter(fileInfo.OpenWrite());
        }
    }
}
