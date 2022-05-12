using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
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
         /*   OrderedSet<int> s = new OrderedSet<int>();
            //s.Add(1);
            //s.Add(2);
            //s.Add(3);
            s.Add(0);
            s.Add(1);
            s.Add(2);
            KeyValuePair<int, int> to_get_upper_lowe = new KeyValuePair<int, int>();
            to_get_upper_lowe = s.DirectUpperAndLower(0); //hwa hna 3shan 4 gedida f 7tha fllist w keda elupper bt3ha hwa rkm 3
            int low = to_get_upper_lowe.Key; //ely t7t zero hwa 1 next
            int up = to_get_upper_lowe.Value;// ely fo2 elzero hwa 0 f h5liha b a5r rkm ely hwa 2 h3ml keda fl angles brdo pre
        
            */
           /* OrderedSet<int> s = new OrderedSet<int>();
            s.Add(1);
            s.Add(2);
            s.Add(2);*/ // mbtsm7sh bltekrar
            if (points.Count == 1 || points.Count == 2 || points.Count == 3)
            {
                outPoints = points;
                return;
            }
            OrderedSet<Tuple<double, int>> all_points = new OrderedSet<Tuple<double, int>>(); //deh b2a elmfrod ely ha7ot feha sorted angles blpoint  
            double k = 2000;//rkm random mn 3andy
            Point B = new Point((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
           
            B.X = (B.X + points[2].X) / 2;
            B.Y = (B.Y + points[2].Y) / 2;

            Point NB = new Point(B.X + k, B.Y); //keda dah el x-axis
            Line Base_line = new Line(B, NB);
            for (int i = 0; i < 3; i++) 
            {
                double angle = calculate_angle_Between_point_and_baseline(Base_line.Start, Base_line.End, Base_line.Start, points[i]);
               all_points.Add(new Tuple<double, int>(angle,i));//hwa hna kda 3mlt add llpoints blangles sorted by3ml llangle sort w ba3d keda ykyb gmbha index elpoint
            }

            for (int i = 3; i < points.Count; i++) 
            {
                 //dlw2ti elmfrod ngeb pr and next bl upper and lower
                KeyValuePair<Tuple<double, int>, Tuple<double, int>> to_get_upper_lower = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
               // Point p = points[i];
                double angle = calculate_angle_Between_point_and_baseline(Base_line.Start, Base_line.End, B, points[i]);
                to_get_upper_lower = all_points.DirectUpperAndLower(new Tuple<double,int>(angle,i));
                Tuple<double, int> upper = null;
                upper = to_get_upper_lower.Key;//next
                Tuple<double, int> lower = null;
                lower = to_get_upper_lower.Value;//pre
                if (lower == null) 
                {
                    lower = all_points.GetLast();
                }
                if (upper == null) 
                {
                    upper = all_points.GetFirst();
                }
                Enums.TurnType e = 0;
                Line l = null;
                l = new Line(points[lower.Item2], points[upper.Item2]);
                e = HelperMethods.CheckTurn(l, points[i]);
                if (e == Enums.TurnType.Right) //outside the polygon
                {
                    KeyValuePair<Tuple<double, int>, Tuple<double, int>> to_get_upper_lower2 = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                    Tuple<double, int> newLower = null;
                    to_get_upper_lower2 = all_points.DirectUpperAndLower(lower);
                    newLower = to_get_upper_lower2.Value;// newPre = CH.pre(Pre) //hna 7sl nfs elmoshkle en newlower b null f h5lih ya5od a5r value f all_points
                    if (newLower == null) 
                    {
                        newLower = all_points.GetLast();
                    }
                    l = new Line(points[i],points[lower.Item2]);
                    e = HelperMethods.CheckTurn(l, points[newLower.Item2]);
                    while (e == Enums.TurnType.Left || e == Enums.TurnType.Colinear) //hwa bdam d5l hna m3na keda lower point hnshlha 3shan m722tsh shrt elsupporting line
                    {
                        all_points.Remove(lower);
                        lower = newLower;
                        to_get_upper_lower2 = all_points.DirectUpperAndLower(lower);
                        newLower = to_get_upper_lower2.Value;
                        if (newLower == null) 
                        {
                            newLower = all_points.GetLast();
                        }
                        l = new Line(points[i], points[lower.Item2]);
                        e = HelperMethods.CheckTurn(l, points[newLower.Item2]);
                    }
                    KeyValuePair<Tuple<double, int>, Tuple<double, int>> to_get_upper_lower3 = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                    Tuple<double, int> newUpper = null;
                    to_get_upper_lower3 = all_points.DirectUpperAndLower(upper);
                    newUpper = to_get_upper_lower3.Key;
                    if (newUpper == null)
                    {
                        newUpper = all_points.GetFirst();
                    }
                    l = new Line(points[i], points[upper.Item2]);
                    e = HelperMethods.CheckTurn(l, points[newUpper.Item2]);

                    while (e == Enums.TurnType.Right || e == Enums.TurnType.Colinear) //lama 3mlt collinear zbt elspecial cases
                    {
                        all_points.Remove(upper);
                        upper = newUpper;
                        to_get_upper_lower3 = all_points.DirectUpperAndLower(upper);
                        newUpper = to_get_upper_lower3.Key;
                        if (newUpper == null)
                        {
                            newUpper = all_points.GetFirst();
                        }
                        l = new Line(points[i], points[upper.Item2]);
                        e = HelperMethods.CheckTurn(l, points[newUpper.Item2]);
                    }
                    all_points.Add(new Tuple<double, int>(angle, i));
                   /* all_points.Remove(lower);
                    all_points.Remove(upper);
                    //ok = HelperMethods.PointInTriangle(points[l], points[i], points[j], points[k]);
                    Enums.PointInPolygon ok = 0;
                    for (int o = 0; o < all_points.Count; o++) 
                    {
                        ok = HelperMethods.PointInTriangle(points[0], points[i], points[lower.Item2], points[upper.Item2]);
                        if (ok == Enums.PointInPolygon.Inside) 
                        {
                            points.RemoveAt(o);
                        }
                    }*/
                }
                

            }
            for (int o = 0; o < all_points.Count; o++)
            {
                outPoints.Add(points[all_points[o].Item2]);
            }
            //llspecial cases
         /*  Enums.PointInPolygon ok = 0;

           for (int i = 0; i < outPoints.Count(); i++)
            {
                for (int j = 0; j < outPoints.Count; j++)
                {
                    for (int p = 0; p < outPoints.Count; p++)
                    {
                        if (i == outPoints.Count)
                            i--;
                        for (int l = 0; l < outPoints.Count; l++)
                        {
                            if (j == outPoints.Count)
                                j--;
                            if (p == outPoints.Count)
                                p--;
                            if (outPoints[l] != outPoints[i] && outPoints[l] != outPoints[j] && outPoints[l] != outPoints[p])
                            {
                                ok = HelperMethods.PointInTriangle(outPoints[l], outPoints[i], outPoints[j], outPoints[p]);
                                if (ok == Enums.PointInPolygon.Inside || ok == Enums.PointInPolygon.OnEdge)
                                {
                                    outPoints.RemoveAt(l);
                                }
                            }
                        }
                    }
                }
            }    */            
            /*
        If turn(Pre, Next, P) is right \\ Outside the polygon
        \\ Left Supporting Line
        newPre = CH.pre(Pre)
        While turn(P, Pre, newPre) is Left
        Pre = newPre
        newPre = CH.pre(Pre)*/

        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
