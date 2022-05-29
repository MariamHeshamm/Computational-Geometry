using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class MonotoneTriangulation : Algorithm
    {
        public bool checkCCW(System.Collections.Generic.List<CGUtilities.Point> points, System.Collections.Generic.List<CGUtilities.Line> lines)
        {
            //List<CGUtilities.Line> temp = lines.OrderBy(p => p.Start.Y).ThenBy(p => p.Start.X).ToList();

            List<CGUtilities.Point> temp = points.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
            int c = points.IndexOf(temp[0]);
            int prevPointIndx = c - 1;
            int nextPointIndx = (c + 1) % points.Count();

            if (c == 0)
            {
                prevPointIndx += points.Count();
            }
            CGUtilities.Point prevPoint = points[prevPointIndx];
            CGUtilities.Point nextPoint = points[nextPointIndx];
            CGUtilities.Line l = new CGUtilities.Line(prevPoint, points[c]);
            CGUtilities.Enums.TurnType e = 0;
            e = CGUtilities.HelperMethods.CheckTurn(l, nextPoint);
            if (e == CGUtilities.Enums.TurnType.Right)
                return false;

            return true;
        }
        public bool checkMonotone(System.Collections.Generic.List<CGUtilities.Point> points, System.Collections.Generic.List<CGUtilities.Line> lines)
        {
            //List<CGUtilities.Line> temp = lines.OrderBy(p => p.Start.Y).ThenBy(p => p.Start.X).ToList();

            List<CGUtilities.Point> temp = points.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
            int end_point = points.IndexOf(temp[0]);
            int start_point = points.IndexOf(temp[points.Count() - 1]);
            int count_start = 0;
            int count_end = 0;

             for (int i = 0; i <points.Count; i++)
             {
                //int currentPointIndex = points.IndexOf(points[i]);
                int nextPointIndx = (i + 1) % points.Count();
                int prevPointIndex = i-1;
                if (i == 0)
                {
                    prevPointIndex += points.Count();
                }
                if(points[i].Y<points[nextPointIndx].Y&& points[i].Y < points[prevPointIndex].Y)
                {
                    count_end++;
                }
                else if (points[i].Y > points[nextPointIndx].Y && points[i].Y > points[prevPointIndex].Y)
                {
                    count_start++;
                }
                //if()

            }
             if(count_start!=1||count_end!=1)
            {
                return false;
            }


            /*CGUtilities.Point prevPoint = points[prevPointIndx];
            CGUtilities.Point nextPoint = points[nextPointIndx];
            CGUtilities.Line l = new CGUtilities.Line(prevPoint, points[c]);
            CGUtilities.Enums.TurnType e = 0;
            e = CGUtilities.HelperMethods.CheckTurn(l, nextPoint);
            if (e == CGUtilities.Enums.TurnType.Right)
                return false;
            */
            return true;
        }

        public System.Collections.Generic.List<CGUtilities.Point> revPolygon(System.Collections.Generic.List<CGUtilities.Point> points)
        {
            System.Collections.Generic.List<CGUtilities.Point> reversed = new List<CGUtilities.Point>();
            for (int i = points.Count() - 1; i >= 0; i--)
            {
                reversed.Add(points[i]);
            }
            return reversed;
        }
        public bool isConvex(System.Collections.Generic.List<CGUtilities.Point> points, CGUtilities.Point current_point)
        {
            int c = points.IndexOf(current_point);
            int prevPointIndx = c - 1;
            int nextPointIndx = (c + 1) % points.Count();

            if (c == 0)
            {
                prevPointIndx += points.Count();
            }
            CGUtilities.Line l = new CGUtilities.Line(points[prevPointIndx], current_point);

            CGUtilities.Enums.TurnType e2 = 0;
            e2 = CGUtilities.HelperMethods.CheckTurn(l, points[nextPointIndx]);
            if (e2 == CGUtilities.Enums.TurnType.Left)
                return true;

                return false;
        }
        public override void Run(System.Collections.Generic.List<CGUtilities.Point> points, System.Collections.Generic.List<CGUtilities.Line> lines, System.Collections.Generic.List<CGUtilities.Polygon> polygons, ref System.Collections.Generic.List<CGUtilities.Point> outPoints, ref System.Collections.Generic.List<CGUtilities.Line> outLines, ref System.Collections.Generic.List<CGUtilities.Polygon> outPolygons)
        {
            //List<CGUtilities.Line> temp = lines.OrderBy(p => p.Start.Y).ThenBy(p => p.Start.X).ToList();
            //int minYIndx = lines.IndexOf(temp[0]);
            bool checkTurn = checkCCW(points, lines);
            //int mxYIndx = lines.IndexOf(temp[0]);

            if (!checkTurn)
            {
                points = revPolygon(points);
            }
            bool check_Monotone = checkMonotone(points, lines);
            if(check_Monotone)
            {
                Stack<CGUtilities.Point> points_stack=new Stack<CGUtilities.Point>();
                System.Collections.Generic.List<CGUtilities.Point> sorted_points= points.OrderByDescending(p => p.Y).ThenBy(p => p.X).ToList();

                points_stack.Push(sorted_points[0]);
                points_stack.Push(sorted_points[1]);

                int i = 2;

                while(i!=points.Count-1)
                {
                    CGUtilities.Point current_point = sorted_points[i];
                    CGUtilities.Point top_point = points_stack.Peek();
                    CGUtilities.Line l = new CGUtilities.Line(sorted_points[0], sorted_points[points.Count - 1]);

                    CGUtilities.Enums.TurnType e1 = 0;
                    e1 = CGUtilities.HelperMethods.CheckTurn(l, current_point);
                    CGUtilities.Enums.TurnType e2 = 0;
                    e2 = CGUtilities.HelperMethods.CheckTurn(l, top_point);
                    CGUtilities.Point top_point2;
                    if ((e1 == CGUtilities.Enums.TurnType.Right&& e2 == CGUtilities.Enums.TurnType.Right)|| (e1 == CGUtilities.Enums.TurnType.Left && e2 == CGUtilities.Enums.TurnType.Left))
                    {
                        points_stack.Pop();
                        top_point2 = points_stack.Peek();
                        if(isConvex(points,top_point2))
                        {
                            CGUtilities.Line l1 = new CGUtilities.Line(current_point, top_point2);
                            outLines.Add(l1);
                            if (points_stack.Count == 1)
                            {
                                points_stack.Push(current_point);
                                i++;
                            }

                        }
                        else
                        {
                            points_stack.Push(top_point);
                            points_stack.Push(current_point);
                            i++;

                        }

                    }
                    else
                    {
                        while (points_stack.Count != 1)
                        {
                            top_point2 = points_stack.Peek();
                            points_stack.Pop();
                            CGUtilities.Line l1 = new CGUtilities.Line(current_point, top_point2);
                            outLines.Add(l1);

                        }
                        points_stack.Pop();
                        points_stack.Push(top_point);
                        points_stack.Push(current_point);
                        i++;

                    }

                }

            }



        }

        public override string ToString()
        {
            return "Monotone Triangulation";
        }
    }
}
