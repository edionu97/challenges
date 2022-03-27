using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GridVile.DataModeling.Reader.Impl;
using GridVile.Navigator.ShortestPathComputer.Impl;
using GridVile.Navigator.Solver.Impl;

namespace GridVile.App
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            //create the reader
            var reader = new KeywordDataReader();

            //get navigator data
            var navigatorData = await reader
                .ReadDataAsync(new FileInfo(@"Data\data.txt"));

            var trip = new GridVileNavigator(
                new LeeBasedShortestPathComputer());

            //get trip elements
            var tripElements = trip
                .GetShortestTrip(navigatorData, out var areDropPoints)
                .ToList();

            //treat the default scenario
            if (!areDropPoints)
            {
                Console.WriteLine($"Shortest route length: {tripElements.Count - 1}");
                Console.WriteLine($"Shortest route: {string.Join(',', tripElements)}");
                return;
            }

            Console.WriteLine("No solution for direct trip...");
            Console.WriteLine($"Possible drop points: {string.Join(',', tripElements)}");
        }
    }
}
