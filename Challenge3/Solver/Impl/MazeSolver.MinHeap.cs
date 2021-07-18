using System.Collections.Generic;
using Challenge3.Helpers;
using Challenge3.Helpers.Items;

namespace Challenge3.Solver.Impl
{
    public partial class MazeSolver
    {
        private readonly C5.IntervalHeap<Point3d> _heap;

        private readonly IDictionary<Point3d, C5.IPriorityQueueHandle<Point3d>> _pointHandlers =
            new Dictionary<Point3d, C5.IPriorityQueueHandle<Point3d>>();

        private readonly ISet<Point3d> _points = new HashSet<Point3d>();
 
        /// <summary>
        /// This function it is used for adding a value into the heap
        /// </summary>
        /// <param name="point"></param>
        private void AddOrUpdateElementHeap(Point3d point)
        {
            //if the heap already contains the point (use the set for O(1)) 
            if (!_points.Contains(point))
            {
                //create the point handle
                C5.IPriorityQueueHandle<Point3d> pointHandle = null;
                _heap.Add(ref pointHandle, point);

                //memorize the point handle
                _pointHandlers[point] = pointHandle;

                //also use this set for O(1) checks
                _points.Add(point);
                return;
            }

            //replace the priority
            _heap.Replace(_pointHandlers[point], point);
        }
    }
}
