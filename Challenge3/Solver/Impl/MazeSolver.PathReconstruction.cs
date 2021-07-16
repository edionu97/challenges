﻿using System.Collections.Generic;
using System.Linq;
using Challenge3.Helpers;

namespace Challenge3.Solver.Impl
{
    public partial class MazeSolver
    {
        private readonly IDictionary<Point3d, Point3d> _pathReconstruction = new Dictionary<Point3d, Point3d>();

        private IEnumerable<Point3d> ReconstructPath(Point3d endPoint)
        {
            var stack = new Stack<Point3d>();

            //if the endpoint is not in path do nothing
            if (!_pathReconstruction.ContainsKey(endPoint))
            {
                yield break;
            }

            //put the items into the stack
            for (var point = endPoint; point != null; point = _pathReconstruction[point])
            {
                stack.Push(point);
            }

            //return the elements
            while (stack.Any())
            {
                yield return stack.Pop();
            }
        }
    }
}
