using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class InsertingDiagonals : Algorithm
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
        public double farthest_distance(CGUtilities.Line line, CGUtilities.Point point)
        {
            double A = (line.End.X - line.Start.X) * (point.Y - line.Start.Y);
            double B = (line.End.Y - line.Start.Y) * (point.X - line.Start.X);
            double AB_norm = Math.Sqrt(Math.Pow((line.End.X - line.Start.X), 2) + Math.Pow((line.End.Y - line.Start.Y), 2));
            double distance = Math.Abs((A - B) / AB_norm);

            return distance;

        }
        public void insertingDiagonals(List<CGUtilities.Point> points, ref List<CGUtilities.Line> outLines)
        {
            if (points.Count() > 3)
            {
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

                CGUtilities.Line finalLine;
                CGUtilities.Line l = new CGUtilities.Line(prevPoint, nextPoint);
                //CGUtilities.Line l = new CGUtilities.Line(nextPoint, prevPoint);
                //check if there is a point in triangle prevc,c,nextc
                CGUtilities.Enums.PointInPolygon pointInside = 0;
                bool pointInTriangle = false;
                double distance = 0;
                double mxDistance = 0;
                int mxDistanceIndx = 0;

                /*for(int j=0;j<points.Count();j++)
                {
                    pointInside = CGUtilities.HelperMethods.PointInTriangle(points[j], prevPoint, points[c], nextPoint);

                    if (pointInside == CGUtilities.Enums.PointInPolygon.Inside || pointInside == CGUtilities.Enums.PointInPolygon.OnEdge)
                    {
                        pointInTriangle = true;
                    }

                }*/

                for (int i = 0; i < points.Count(); i++)
                {

                    //CGUtilities.Enums.PointInPolygon onEdge = CGUtilities.Enums.PointInPolygon.OnEdge;

                    if (points[i] != prevPoint && points[i] != nextPoint)
                    {
                        pointInside = CGUtilities.HelperMethods.PointInTriangle(points[i], prevPoint, points[c], nextPoint);

                        if (pointInside == CGUtilities.Enums.PointInPolygon.Inside)
                        {
                            distance = farthest_distance(l, points[i]);
                            if (distance > mxDistance)
                            {
                                mxDistance = distance;
                                mxDistanceIndx = i;

                            }
                            pointInTriangle = true;
                        }
                    }


                }
                if (!pointInTriangle)
                {
                    outLines.Add(l);
                    finalLine = l;
                }
                else
                {
                    CGUtilities.Line l1 = new CGUtilities.Line(points[c], points[mxDistanceIndx]);
                    outLines.Add(l1);
                    finalLine = l1;
                }

                List<CGUtilities.Point> p1 = new List<CGUtilities.Point>();
                List<CGUtilities.Point> p2 = new List<CGUtilities.Point>();

                //insert upper points from next to prev that turns right and left points from mxdist to c 
                // CGUtilities.Enums.TurnType e = 0;
                //p1.Add(finalLine.Start);
                //p2.Add(finalLine.Start);
                int startLoop = points.IndexOf(finalLine.End);
                bool p1End = false;
                // int sz = points.IndexOf(finalLine.Start);
                for (int i = startLoop; i < points.Count() + startLoop; i++)
                {
                    //e = CGUtilities.HelperMethods.CheckTurn(finalLine, points[i]);
                    /*if (e == CGUtilities.Enums.TurnType.Right)
                    {
                        p1.Add(points[i]);
                    }*/
                    /*else if (e == CGUtilities.Enums.TurnType.Left)
                    {
                        p2.Add(points[i]);
                    }*/
                    int indx = i % points.Count();
                    if ((indx != points.IndexOf(finalLine.Start)) && !p1End)
                    {
                        p1.Add(points[indx]);

                    }
                    else if (indx == points.IndexOf(finalLine.Start))
                    {
                        p1.Add(points[indx]);
                        p2.Add(points[indx]);
                        p1End = true;

                    }
                    //p1.Add(points[indx]);
                    //p2.Add(points[indx]);
                    else if (indx != points.IndexOf(finalLine.End))
                    {

                        p2.Add(points[indx]);
                    }


                }
                p2.Add(points[points.IndexOf(finalLine.End)]);

                insertingDiagonals(p1, ref outLines);
                insertingDiagonals(p2, ref outLines);



            }

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
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            bool checkTurn = checkCCW(points, lines);
            //int mxYIndx = lines.IndexOf(temp[0]);

            if (!checkTurn)
            {
                points = revPolygon(points);
            }
            insertingDiagonals(points, ref outLines);


            /*if (distance > init_height)
            {
                init_height = distance;
                mx_point = p[i];
            }*/

        }

        public override string ToString()
        {
            return "Inserting Diagonals";
        }
    }
}
