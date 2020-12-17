using System;
using System.Collections.Generic;

namespace Challenge1
{
    public static class ProblemSolver
    {
        /// <summary>
        /// This function it is used for getting the missing items from list
        /// Complexity O(N) 
        /// </summary>
        /// <param name="numbers">the shuffled list</param>
        /// <param name="missingCount">the number of items that are missing</param>
        /// <returns>all the missing elements</returns>
        public static IEnumerable<double> Solve(IList<double> numbers, int missingCount)
        {
            if (missingCount == 0 || numbers.Count <= 2)
            {
                yield break;
            }

            //get sum ratio and missing seq partial sum
            var (sum, ratio, partialSum) = GetSumRatioAndPartialSum(numbers, missingCount);

            //compute the first missing item
            var missingSum = sum - partialSum;
            var distanceValue = (missingCount * (missingCount - 1) * ratio) / 2;
            var firstMissingNumber = (missingSum - distanceValue) / missingCount;

            //get all the numbers (add the ratio to the first number)
            yield return firstMissingNumber;
            for (var idx = 1; idx < missingCount; ++idx)
            {
                yield return firstMissingNumber + ratio * idx;
            }
        }

        /// <summary>
        /// Helper function
        /// </summary>
        /// <param name="numbers">the shuffled numbers</param>
        /// <param name="missingCount">missing numbers count</param>
        /// <returns>a tuple of values (the total sum, the ratio and the partial sum)</returns>
        private static Tuple<double, double, double> GetSumRatioAndPartialSum(ICollection<double> numbers, int missingCount)
        {
            //declare the min and max values
            var minValue = double.MaxValue;
            var maxValue = double.MinValue;
            var partialSum = .0;

            //compute min and max values
            foreach (var number in numbers)
            {
                partialSum += number;
                minValue = Math.Min(minValue, number);
                maxValue = Math.Max(maxValue, number);
            }

            //get the total number of elements
            var numberOfElements = numbers.Count + missingCount;

            //get the sum
            var sum = ((minValue + maxValue) * numberOfElements) / 2;

            //get the ratio
            var ratio = (2 * sum - 2 * numberOfElements * minValue) / (numberOfElements * --numberOfElements);

            //compute the sum and the first number of the seq
            return Tuple.Create(sum, ratio, partialSum);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var shuffledList = new List<double> { 77, 66, 55, 11, 44};
            const int missingItemsCount = 2;

            foreach (var missingNumber in ProblemSolver.Solve(shuffledList, missingItemsCount))
            {
                Console.Write(missingNumber + " ");
            }
        }
    }
}
