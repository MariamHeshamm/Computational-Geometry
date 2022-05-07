using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public class point_info
        {
            double X_point;
            double Y_point;
            int Point_index;

            public point_info(double x_point,double y_point,int index)
            {
                this.X_point = x_point;
                this.Y_point = y_point;
                this.Point_index = index;
            }
        }

        public double farthest_distance(Line line, Point point)
        {
            double A = (line.End.X - line.Start.X) * (point.Y - line.Start.Y);
            double B = (line.End.Y - line.Start.Y) * (point.X - line.Start.X);
            double AB_norm = Math.Sqrt(Math.Pow((line.End.X - line.Start.X), 2)+Math.Pow((line.End.Y - line.Start.Y),2));
            double distance = Math.Abs((A - B) / AB_norm);

            return distance;

        }
        public double farthest_distance2(Line line, Point point)
        {
            Point AC =new Point((point.X - line.Start.X) , (point.Y - line.Start.Y));
            Point AB =new Point((line.End.X - line.Start.X) , (line.End.Y - line.Start.Y));
            double cross=Math.Abs(HelperMethods.CrossProduct(AC,AB));
            double AB_norm = Math.Sqrt(Math.Pow((line.End.X - line.Start.X), 2) + Math.Pow((line.End.Y - line.Start.Y), 2));
            double distance =cross/ AB_norm;

            return distance;

        }
        public List<Point> QuickhullRec(List<Point> p, Line l)
        {
            List<Point> final_points=new List<Point>();
            if (p.Count() == 0)
                return final_points;
            //double height = 0;
            //Point p1;
            //Point p2;
            //p1=new Point(l.End.X-l.Start.X , l.End.Y-l.Start.Y);
            Point mx_point=p[0];
            double init_height=0;

            for(int i=0;i<p.Count();i++)
            {
                double distance = farthest_distance2(l, p[i]);
                double distance2 = farthest_distance(l, p[i]);
                //p2 = new Point(p[i].X - l.Start.X, p[i].Y - l.Start.Y);
                //var distance = Math.Sqrt((Math.Pow(l.End.X - l.Start.X, 2) + Math.Pow(l.End.Y - l.Start.Y, 2)));
                //double cross=HelperMethods.CrossProduct(p1, p2);
                //double cross_dist=Math.Abs()
                //height = Math.Abs(cross/distance);
                if(distance>init_height)
                {
                    init_height=distance;
                    mx_point=p[i];
                }

            }
            
            Line l1 = new Line(l.Start, mx_point);
            List<Point> upper_points_list = new List<Point>();
            List<Point> upper_points_list_for_line2 = new List<Point>();

      

            Enums.TurnType e = 0;

            for (int i = 0; i < p.Count(); i++)
            {
                e = HelperMethods.CheckTurn(l1, p[i]);
                if (e == Enums.TurnType.Left)
                    upper_points_list.Add(p[i]);
            
            }
            List<Point> R1 = QuickhullRec(upper_points_list, l1);

            Line l2 = new Line(mx_point,l.End);
            for (int i = 0; i < p.Count(); i++)
            {
                e = HelperMethods.CheckTurn(l2, p[i]);
                if (e == Enums.TurnType.Left)
                    upper_points_list_for_line2.Add(p[i]);
                
            }
            List<Point> R2 = QuickhullRec(upper_points_list_for_line2, l2);

            List<Point> output = new List<Point>();
            R1.Add(mx_point);
            output = R1.Concat(R2).ToList();



            
            return output;

        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1 || points.Count == 2 || points.Count == 3)
            {
                outPoints = points;
                return;
            }
            
            int num_of_points=points.Count();
            //point_info min_x_point=new point_info(points[0].X,points[0].Y,0);
            //point_info max_x_point=new point_info(points[0].X,points[0].Y,0);

            double max_x=points[0].X;
            double min_x=points[0].X;
            int min_index=0;
            int max_index=0;

            
            for(int i=1;i<num_of_points;i++)
            {
                if (points[i].X<min_x)
                {
                    min_x = points[i].X;
                    min_index = i;
                }
                if (points[i].X > max_x)
                {
                    max_x = points[i].X;
                    max_index = i;
                }
            }
            Point min_x_point=points[min_index];
            Point max_x_point=points[max_index];

            outPoints.Add(min_x_point);
            outPoints.Add(max_x_point);

            List<Point> upper_points_list=new List<Point>();
            List<Point> lower_points_list = new List<Point>();

            Line init_line = new Line(min_x_point,max_x_point);
            Line init_line2 = new Line(max_x_point,min_x_point);


            Enums.TurnType e = 0;

            for(int i=0;i<num_of_points;i++)
            {
                e = HelperMethods.CheckTurn(init_line, points[i]);
                if (e == Enums.TurnType.Left)
                {
                    upper_points_list.Add(points[i]);
                }
                if (e == Enums.TurnType.Right)
                {
                    lower_points_list.Add(points[i]);
                    
                }
                if (e == Enums.TurnType.Colinear && points[i] != max_x_point && points[i] != min_x_point)
                {
                    points.Remove(points[i]);
                    num_of_points -= 1;
                    i -= 1;
                }
            }

            List<Point> list1 = new List<Point>();
            List<Point> list2 = new List<Point>();

            list1=QuickhullRec(upper_points_list, init_line);
            list2=QuickhullRec(lower_points_list, init_line2);

            outPoints.AddRange(list1);
            outPoints.AddRange(list2);



        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
