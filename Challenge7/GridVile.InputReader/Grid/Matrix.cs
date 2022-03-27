using System;
using System.Collections.Generic;
using GridVile.DataModeling.Coordinates;

namespace GridVile.DataModeling.Grid
{
    public class Matrix<TValue>
    {
        private readonly Dictionary<Point, TValue> _matrixValues = new();

        private readonly int _rowNumber;
        private readonly int _columnNumber;

        public Matrix(int n, int m)
        {
            _rowNumber = n;
            _columnNumber = m;
        }

        /// <summary>
        /// Gets the value for a given point
        /// </summary>
        /// <param name="point">The point value</param>
        /// <returns>The value of the point or default value otherwise</returns>
        public TValue this[Point point]
        {
            get
            {
                //perform constraint check
                if (!RespectsMatrixBoundaries(point))
                {
                    //throw the exception
                    throw new IndexOutOfRangeException();
                }

                //return matrix value
                return _matrixValues.ContainsKey(point)
                    ? _matrixValues[point]
                    : default;
            }
            set
            {
                //perform constraint check
                if (!RespectsMatrixBoundaries(point))
                {
                    //throw the exception
                    throw new IndexOutOfRangeException();
                }

                //set the value
                _matrixValues[point] = value;
            }
        }

        /// <summary>
        /// Checks if a point is inside matrix
        /// </summary>
        /// <param name="point">The point we are checking</param>
        /// <returns>True if the point is in matrix or false otherwise</returns>
        public bool RespectsMatrixBoundaries(Point point)
        {
            //deconstruct the point
            var (x, y) = point;

            //ensure we are in the matrix
            return x >= 0 && x < _rowNumber && y >= 0 && y < _columnNumber;
        }

        public bool HasPoint(Point p) => _matrixValues.ContainsKey(p);
    }
}
