using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        Enums.TurnType e = 0;

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            if (points.Count == 1 || points.Count == 2 || points.Count == 3)
            {
                outPoints = points;
                return;
            }
            int left = 0, right = 0;
            Line l = null;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (points[i] != points[j]) { 
                    left = 0; right = 0; 
                    for (int k = 0; k < points.Count; k++)
                    {
                        //if (points[k] != points[i] && points[k] != points[j] && points[i] != points[j])
                        if (points[k] != points[i] && points[k] != points[j])
                        {
                            l = new Line(points[i], points[j]);
                            e = HelperMethods.CheckTurn(l, points[k]);
                            if (e == Enums.TurnType.Left)
                            {
                                left += 1;
                                //then pi pj not extreme
                            }
                            if (e == Enums.TurnType.Right)
                            {
                                right += 1;
                                //then pi pj not extreme
                            }
                            //if (e == Enums.TurnType.Colinear)
                            //{
                            //    Colinear += 1;
                            //    //then pi pj not extreme
                            //}
                        }
                    }

                    }
                    //(i!=0 || j!=0) 7attha 3shan lama kan byd5ol fl2wl 5ales kan by3ml add mn8er ma yd5ol flconditions
                    if (((right == 0 || left == 0  ) && (i!=0 || j!=0))/* && (right > 0 || left > 0 || Colinear > 0)*/)
                    {
                        outLines.Add(l);
                        outPoints.Add(points[i]);
                        outPoints.Add(points[j]);

                        //                        outLines.Add(l);
                    }
                }
            }

            //deh 3mltha l2n la2et eno bytl3 points mtkrara
            List< Point> p = new List<Point>();
            for (int i = 0; i < outPoints.Count; i++) 
            {
                if (!p.Contains(outPoints[i]))
                {
                    p.Add(outPoints[i]);
                }
                else 
                {
                    continue;
                }
            }
            outPoints = p;
            //hna la2et f testcase eltraingle points 3la nfs elline f 3shan keda lama shltha esht8l
            bool onTheline = false;
            for (int i = 0; i < outPoints.Count; i++)
            {
                for (int j = 0; j < outPoints.Count; j++) 
                {
                    if (outPoints[i] != outPoints[j])
                    {
                        for (int k = 0; k < outPoints.Count; k++)
                        {
                            if (outPoints[i] != outPoints[k] && outPoints[j] != outPoints[k])
                            {
                                onTheline = HelperMethods.PointOnSegment(outPoints[k], outPoints[i], outPoints[j]);

                                if (onTheline == true)
                                {
                                    outPoints.RemoveAt(k);
                                }
                            }
                        }
                    }             
                }
            }

            /////////////////////

            //outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
