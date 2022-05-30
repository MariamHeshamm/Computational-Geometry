using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public int orientation(Point a, Point b, Point c)
        {
            double res = (b.Y - a.Y) * (c.X - b.X) -(c.Y - b.Y) * (b.X - a.X);

            if (res == 0)
                return 0;
            if (res > 0)
                return 1;
            return -1;
        }

        public bool isTangent(Point LP, Point RP, List<Point> polygon)
        {
            Line l = new Line(LP, RP);
            int turnRight_counter = 0;
            int turnLeft_counter = 0;

            Enums.TurnType e = 0;

            for (int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i] != LP && polygon[i] != RP)
                {
                    e = HelperMethods.CheckTurn(l, polygon[i]);
                    if (e == Enums.TurnType.Right)
                    {
                        turnRight_counter++;
                    }
                    else if (e == Enums.TurnType.Left)
                    {
                        turnLeft_counter++;
                    }
                }

            }
            if ((turnLeft_counter != polygon.Count - 2) && (turnRight_counter != polygon.Count - 2))
            {
                return false;
            }

            return true;
        }

        public List<Point> Divide(List<Point> point, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (point.Count() < 6)
            {
                outPoints = new List<Point>();
                GrahamScan jarvis = new GrahamScan();
                jarvis.Run(point, null, null, ref outPoints, ref outLines, ref outPolygons);
                return outPoints;

            }
            /*if (point.Count() < 6)
            {
                outPoints = new List<Point>();
                JarvisMarch jarvis = new JarvisMarch();
                jarvis.Run(point, null, null, ref outPoints, ref outLines, ref outPolygons);
                return outPoints;

            }*/
            else
            {

                Point midPoint = (point[point.Count() / 2]);
                List<Point> RPoints = new List<Point>();
                List<Point> LPoints = new List<Point>();

                for (int i = 0; i < (point.Count() / 2) + 1; i++)
                {
                    LPoints.Add(point[i]);
                }
                for (int i = (point.Count() / 2) + 1; i < point.Count(); i++)
                {
                    RPoints.Add(point[i]);
                }
                List<Point> LCH = Divide(LPoints, ref outPoints, ref outLines, ref outPolygons);
                List<Point> RCH = Divide(RPoints, ref outPoints, ref outLines, ref outPolygons);
                List<Point> merge = Merge(LCH, RCH);
                return merge;

            }

        }
        public List<Point> Merge(List<Point> LCH, List<Point> RCH)
        {
            List<Point> temp1 = LCH.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            List<Point> temp2 = RCH.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            Point MLP = temp1[temp1.Count() - 1];
            int mxIndx = LCH.IndexOf(temp1[temp1.Count() - 1]);

            Point MRP = temp2[0];
            int minIndx = RCH.IndexOf(temp2[0]);

            Point ULP = MLP;
            Point URP = MRP;

            //using jarvis
            /*if(mxIndx+1==LCH.Count())
            {
                mxIndx -= LCH.Count();
            }
            if (minIndx == 0)
            {
                minIndx = RCH.Count();
            }
            Point NextLP=LCH[mxIndx+1];
            Point PreRP=RCH[minIndx-1];
            */ 

            //using graham
            if (mxIndx == 0)
            {
                mxIndx = LCH.Count();
            }
            if (minIndx == RCH.Count() - 1)
            {
                minIndx -= RCH.Count();
            }
            Point NextLP = LCH[mxIndx - 1];
            Point PreRP = RCH[minIndx + 1];


            Enums.TurnType e = 0;
            Enums.TurnType e2 = 0;

            Line l1 = new Line(URP, ULP);

            e = HelperMethods.CheckTurn(l1, NextLP);
            bool upLeft = true;
            bool upRight = true;
            Point OldULP = ULP;
            Point OldURP = URP;
            bool done = false;



            while(!done)
            {
                done = true;
                upLeft = false;
                upRight = false;
                while (orientation(URP,ULP,NextLP)>0/*(e == Enums.TurnType.Right)||!isTangent(ULP,URP,LCH)*/)
                {
                    ULP = NextLP;
                    int indxULP = LCH.IndexOf(ULP);

                    //NextLP=LCH[indxULP+1];  //jarvis
                    if (indxULP == 0)
                    {
                        indxULP = LCH.Count();
                    }
                    //NextLP = LCH[indxULP - 1];  //graham
                    NextLP=LCH[(LCH.Count+LCH.IndexOf(ULP)-1)%LCH.Count];
                    l1 = new Line(URP, ULP);
                    e = HelperMethods.CheckTurn(l1, NextLP);
                    upLeft = true;
                }
                //upLeft = false;
                Line l2 = new Line(ULP, URP);
                e2 = HelperMethods.CheckTurn(l2, PreRP);


                while (orientation(ULP, URP, PreRP) < 0/*(e2 == Enums.TurnType.Left) || !isTangent(ULP, URP, RCH)*/)
                {
                    URP = PreRP;
                    int indxURP = RCH.IndexOf(URP);
                    if (indxURP == RCH.Count-1)
                    {
                        indxURP -= RCH.Count();
                    }
                    //PreRP=RCH[indxURP-1]; //jarvis
                    //PreRP = RCH[indxURP + 1];  // graham
                    PreRP=RCH[(RCH.IndexOf(URP)+1)% RCH.Count];
                    l2 = new Line(ULP, URP);
                    e2 = HelperMethods.CheckTurn(l2, PreRP);
                    upRight = true;
                    done = false;
                }
                //upRight = false;

            } 


            Point DLP = MLP;
            Point DRP = MRP;
            Point OldDLP = DLP;
            Point OldDRP = DRP;
            int temp3 = LCH.IndexOf(DLP);
            int temp = RCH.IndexOf(DRP);

            if (temp == 0)
            {
                temp += RCH.Count();
            }
            /*else if (temp == RCH.Count()-1)
            {
                temp += RCH.Count();
            }*/
            if (LCH.IndexOf(DLP) + 1 == LCH.Count())
            {
                temp3 -= LCH.Count();
            }
            /*if (temp3 == 0)
            {
                temp3 += LCH.Count();
            }*/
            //Point NextRP = RCH[temp+1];//jarvis
            //Point NextRP = RCH[temp - 1];//graham
            Point NextRP = RCH[(RCH.Count + RCH.IndexOf(DRP) - 1) % RCH.Count];

            //Point PreLP = LCH[temp3-1];//jarvis
            Point PreLP = LCH[temp3 + 1];//graham

            e = 0;
            e2 = 0;

            l1 = new Line(DRP, DLP);

            e = HelperMethods.CheckTurn(l1, PreLP);
            //bool done2 = false;
            bool downLeft = true;
            bool downRight = true;
            done = false;
            while(!done)
            {
                downLeft = false;
                downRight = false;
                done = true;
                while (orientation(DRP, DLP, PreLP) < 0/*(e == Enums.TurnType.Left)|| !isTangent(DLP, DRP, LCH)*/)
                {
                    DLP = PreLP;
                    int indxDLP = LCH.IndexOf(DLP);
                    //jarvis
                    /*if(indxDLP==0)
                    {
                        indxDLP=LCH.Count();
                    }*/

                    //graham
                    if (indxDLP == LCH.Count() - 1)
                    {
                        indxDLP -= (LCH.Count());
                    }

                    //PreLP = LCH[indxDLP - 1];//jarvis
                    // PreLP = LCH[indxDLP + 1];//graham
                    PreLP = LCH[(LCH.IndexOf(DLP) + 1) % LCH.Count];
                    //NextRP = RCH[(RCH.Count + RCH.IndexOf(DRP) - 1) % RCH.Count];



                    l1 = new Line(DRP, DLP);
                    e = HelperMethods.CheckTurn(l1, PreLP);
                    downLeft = true;
                }
                Line l3 = new Line(DLP, DRP);
                e2 = HelperMethods.CheckTurn(l3, NextRP);


                while (orientation(DLP, DRP, NextRP) > 0/*(e2 == Enums.TurnType.Right) || !isTangent(DLP, DRP, RCH)*/)
                {
                    DRP = NextRP;
                    int indxDRP = RCH.IndexOf(DRP);

                    //NextRP = RCH[indxDRP + 1];//jarvis
                    if (indxDRP == 0)
                    {
                        indxDRP = RCH.Count();
                    }
                    //NextRP = RCH[indxDRP - 1];//graham
                    NextRP = RCH[(RCH.Count + RCH.IndexOf(DRP) - 1) % RCH.Count];

                    l3 = new Line(DLP, DRP);
                    e2 = HelperMethods.CheckTurn(l3, NextRP);
                    downRight = true;
                    done = false;
                }

            }
            List<Point> tang1 = new List<Point>();
            List<Point> tang2 = new List<Point>();
            List<Point> final_list = new List<Point>();
            int indxOfULP = LCH.IndexOf(ULP);
            int indxOfDLP = LCH.IndexOf(DLP);
            int indxOfDRP = RCH.IndexOf(DRP);
            int indxOfURP = RCH.IndexOf(URP);
            tang1.Add(DLP);
            if (indxOfDLP == 0)
            {
                indxOfDLP = LCH.Count() - 1;
            }

            //jarvis
            /*for(int i=indxOfDLP;i>=indxOfULP;i--)
            {
                tang1.Add(LCH[i]);
            }*/
            if (indxOfDLP == LCH.Count() - 1)
            {
                indxOfDLP = 0;
            }
            for (int i = indxOfULP; i >= indxOfDLP; i--)
            {
                tang1.Add(LCH[i]);
            }
            /*if(tang1.Contains(DLP)==false)
            {

            }*/
            //jarvis
            /*for (int i = indxOfDRP; i <= indxOfURP; i++)
            {
                tang2.Add(RCH[i]);
            }*/
            for (int i = indxOfURP; i <= indxOfDRP; i++)
            {
                tang2.Add(RCH[i]);
            }
            final_list = tang1;
            final_list.AddRange(tang2);



            return final_list;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            /*if (points.Count == 1 || points.Count == 2 || points.Count == 3)
            {
                outPoints = points;
                return;
            }*/
            List<Point> temp = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            int indx = points.IndexOf(temp[0]);
            //Point temp2 = points[points.Count()-1];
            //var pair = points.MaxBy(x => x.Key);

            List<Point> ans = Divide(temp, ref outPoints, ref outLines, ref outPolygons);
            outPoints = ans;




        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
