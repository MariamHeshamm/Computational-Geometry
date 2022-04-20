using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1 || points.Count == 2 || points.Count == 3)//case1 and case 2
            {
                outPoints = points;
                return;
            }

            Enums.PointInPolygon ok = 0;
             for (int i = 0; i < points.Count(); i++) 
            {
                for (int j = 0; j < points.Count; j++) 
                {
                    for (int k = 0; k < points.Count; k++) 
                    {
                        if (i == points.Count)
                            i--;
                        for (int l = 0; l < points.Count; l++)
                        {
                            if (j == points.Count)
                                j--;
                            if (k == points.Count)
                                k--;
                            if (points[l] != points[i] && points[l] != points[j] && points[l] != points[k])
                            {
                                 ok = HelperMethods.PointInTriangle(points[l], points[i], points[j], points[k]);
                                if (ok == Enums.PointInPolygon.Inside || ok == Enums.PointInPolygon.OnEdge)
                                {
                                    points.RemoveAt(l);
                                }
                            }
                        }
                    }
                }
            }
        outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
