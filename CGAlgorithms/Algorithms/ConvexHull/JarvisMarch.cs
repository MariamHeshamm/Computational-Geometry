using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public double calculate_angle_Between_point_and_baseline(Point p1, Point p2, Point p3, Point p4)
        {
            //reference1 https://math.stackexchange.com/questions/873366/calculate-angle-between-two-lines dah kan tmam
            //reference2 https://gamedev.stackexchange.com/questions/109991/most-efficient-way-to-calculate-angle-between-two-line-segments-that-do-not-nece
            double theta1 = Math.Atan2(p2.Y - p1.Y, p1.X - p2.X);
            double theta2 = Math.Atan2(p4.Y - p3.Y, p3.X - p4.X);//bygeb alangle bymshy anti clock wise
            // double diff = Math.Abs(theta1-theta2);
            double angle = (theta1 - theta2) * (180 / Math.PI);
            return angle;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1 || points.Count == 2 )
            {
                outPoints = points;
                return;
            }

            double minY = points[0].Y;
            int min = 0; //index llpoint
            for (int i = 1; i < points.Count; i++)
            {
                double y = points[i].Y;
                if (y < minY || minY == y && points[i].X < points[min].X)//lama zwdt elor condition et7lt el7 
                {
                    minY = points[i].Y;
                    min = i;
                }
            }

            List<int> all_points = new List<int>();
            all_points.Add(min);
            Point forvector = new Point(points[min].X + 200, points[min].Y);
            double min_angle = 1000000000000000000;
            int min_index = -1;
            var largest_distance = 0.0;
            while (true)
            {
                min_angle = 1000000000000000000;
                for (int k = 0; k < points.Count; k++)
                {
                    if (k == all_points[all_points.Count - 1])
                    {
                        continue;
                    }
                    double angle = calculate_angle_Between_point_and_baseline(points[all_points[all_points.Count - 1]], forvector, points[k], points[all_points[all_points.Count - 1]]);
                    //                    var distance = Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
                    var distance = Math.Sqrt((Math.Pow(points[k].X - points[all_points[all_points.Count - 1]].X, 2) + Math.Pow(points[k].Y - points[all_points[all_points.Count - 1]].Y, 2)));
                    if (angle < 0)
                    {
                        angle += 360;
                    }
                    if (angle < min_angle)
                    {
                        min_angle = angle;
                        min_index = k;
                        largest_distance = distance;
                    }
                    if (angle == min_angle && distance > largest_distance)
                    {
                        min_angle = angle;
                        min_index = k;
                        largest_distance = distance;
                    }
                }

                if (all_points.Contains(min_index))
                {
                    break;
                }

                forvector = points[all_points[all_points.Count - 1]];
                all_points.Add(min_index);

            }
            for (int i = 0; i < all_points.Count; i++)
            {
                outPoints.Add(points[all_points[i]]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}