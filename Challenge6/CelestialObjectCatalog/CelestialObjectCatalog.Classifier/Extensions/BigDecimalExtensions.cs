// ReSharper disable once CheckNamespace
namespace Deveel.Math
{
    internal static class BigDecimalExtensions
    {
        /// <summary>
        /// This function compares two big decimals, and returns true if first is less than second
        /// </summary>
        /// <param name="a">The first big decimal</param>
        /// <param name="b">The second big decimal</param>
        /// <returns>True or false depending on inequality</returns>
        public static bool Lt(
            this BigDecimal a, BigDecimal b) => a?.CompareTo(b) < 0;

        /// <summary>
        /// This function compares two big decimals, and returns true if first is less than equal to second
        /// </summary>
        /// <param name="a">The first big decimal</param>
        /// <param name="b">The second big decimal</param>
        /// <returns>True or false depending on inequality</returns>
        public static bool Lte(
            this BigDecimal a, BigDecimal b) => a?.CompareTo(b) <= 0;

        /// <summary>
        /// This function compares two big decimals, and returns true if first is greater than second
        /// </summary>
        /// <param name="a">The first big decimal</param>
        /// <param name="b">The second big decimal</param>
        /// <returns>True or false depending on inequality</returns>
        public static bool Gt(
            this BigDecimal a, BigDecimal b) => a?.CompareTo(b) > 0;

        /// <summary>
        /// This function compares two big decimals, and returns true if first is greater than or equal to second
        /// </summary>
        /// <param name="a">The first big decimal</param>
        /// <param name="b">The second big decimal</param>
        /// <returns>True or false depending on inequality</returns>
        public static bool Gte(
            this BigDecimal a, BigDecimal b) => a?.CompareTo(b) >= 0;
    }
}
