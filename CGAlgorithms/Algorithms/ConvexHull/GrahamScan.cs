using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {

        public List<Point> get_true_points(List<Tuple<double, int>> points, List<Point> real_points)
        {
            List<Point> updated_points = new List<Point>();
            for (int i = 0; i < points.Count; i++) 
            {
                updated_points.Add(real_points[points.ElementAt(i).Item2]);
            }
            return updated_points;
        }
        public List<Point> sort(List<Point> points) 
        {
            var angles_with_points = new List<Tuple<double, int>>();
            List<Point> updated_points = new List<Point>();
            for (int i = 1; i < points.Count; i++) 
            {
                double deltaY = (points[i].Y - points[0].Y);
                double deltaX = (points[i].X - points[0].X);
                double angle = Math.Atan2(deltaY,deltaX);
                angle = angle * 180 / Math.PI;
                angles_with_points.Add(new Tuple<double, int>(angle,i));
            }
           angles_with_points.Sort(Comparer<Tuple<double, int>>.Default);
           updated_points =  get_true_points(angles_with_points, points);
           updated_points.Insert(0, points[0]);
           return updated_points;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1 || points.Count == 2 || points.Count == 3)
            {
                outPoints = points;
                return;
            }
            //n3ml intialization el2wl w ba3d keda nlf w neshof 2a2l point
            double minY = points[0].Y;
            int min = 0; //index llpoint
            for (int i = 1; i < points.Count; i++)
            {
                double y = points[i].Y;
                if (y < minY)
                {
                    minY = points[i].Y;
                    min = i;
                }
            }


            Point p1 = points[0];
            Point minPoint = points[min];
            points[0] = minPoint;
            points[min] = p1;
            Point First_point = points[0];//l7d hna keda gbna 2a2l no2ta ely hbntdy mn 3ndha
           
            Stack<Point> all_points = new Stack<Point>();
            List<Point>sorted_points =  sort(points);
            all_points.Push(sorted_points[0]);
            all_points.Push(sorted_points[1]);
            all_points.Push(sorted_points[2]);
            Line l = null;
            Enums.TurnType e = 0;
            for (int i = 3; i < sorted_points.Count; ) 
            {
                Point top = all_points.Peek();
                all_points.Pop();
                Point second_top = all_points.Peek();
                all_points.Push(top);
                l = new Line(second_top, top);
                e = HelperMethods.CheckTurn(l, sorted_points[i]);
                if (e == Enums.TurnType.Left)
                {
                    all_points.Push(sorted_points[i]);
                    i++;
               //     outLines.Add(l);
                }
                else 
                {
                    all_points.Pop();
                }
            }
            outPoints = all_points.ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
