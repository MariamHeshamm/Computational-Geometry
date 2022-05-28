using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
using CGAlgorithms;
using System.Threading.Tasks;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
        public class EventPoint
        {
           public Point p;
           public int seg_Type;
           public int Line_index;
           public int first_index;
           public int second_index;
           
        public EventPoint(Point p , int type,int index)
        {
            this.p = p;
            this.seg_Type = type;
            this.Line_index = index;
        }
        public EventPoint(Point p, int type, int first, int second) 
        {
            this.p = p;
            this.seg_Type = type;
            this.first_index = first;
            this.second_index = second;
        }

        }
         public class SweepLine:Algorithm
        {
            public SortedDictionary<double, EventPoint> Events = new SortedDictionary<double, EventPoint>();
            public SortedList<double, int> Sweepline = new SortedList<double, int>();
            public List<Point> Intersection_Points = new List<Point>();
               
            //0 start point
            //1 end point
            //2 intersection point
             public SortedDictionary<double, EventPoint> IntializeEvents(SortedDictionary<double, EventPoint> Events, List<CGUtilities.Line> lines) 
             {
                 for (int i = 0; i < lines.Count; i++) 
                 {
                     Events.Add(lines[i].Start.X, new EventPoint(lines[i].Start, 0, i));
                     Events.Add(lines[i].End.X, new EventPoint(lines[i].End, 1, i));
                 }
                 return Events;// Now all the points have been transformed into Events and added in the queue based on the lower x-axis
             }

             public bool CheckIntersection(Line l1 ,Line l2) 
             {
                 Enums.TurnType first = HelperMethods.CheckTurn(l1, l2.Start);
                 Enums.TurnType second = HelperMethods.CheckTurn(l1, l2.End);
                 Enums.TurnType third = HelperMethods.CheckTurn(l2, l1.Start);
                 Enums.TurnType fourth = HelperMethods.CheckTurn(l2, l1.End);
                 if (first != second && third != fourth) //keda feh intersection tmm
                 {
                     return true;
                 }
                 return false;
             }
             public Point GetIntersectionPoint(Line l1, Line l2) 
             {
                 double a1 = l1.End.Y - l1.Start.Y;
                 double b1 = l1.Start.X - l1.End.X;
                 double c1 = a1 * (l1.Start.X) + b1 * (l1.Start.Y);

                 double a2 = l2.End.Y - l2.Start.Y;
                 double b2 = l2.Start.X - l2.End.X;
                 double c2 = a2 * (l2.Start.X) + b2 * (l2.Start.Y);

                 double determinant = a1 * b2 - a2 * b1;

                 double x = (b2 * c1 - b1 * c2) / determinant;
                 double y = (a1 * c2 - a2 * c1) / determinant;
                 return new Point(x, y);
             }

             public void HandleEvents(EventPoint Event, List<CGUtilities.Line> lines ) 
             {
                 bool intersect = false;

                 bool prev = true;
                 bool next = true;

                // double prevY = -1;
                 int prevSegment = -1;

                 //double nextY = -1;
                 int nextSegment = -1;

                 if (Event.seg_Type == 0) //hna start point
                 {
                    
                    Sweepline.Add(Event.p.Y, Event.Line_index);
                    int selectedValue = Event.Line_index;

                    int indexOfPrevious = Sweepline.IndexOfValue(selectedValue) - 1;
                    int indexOfNext = Sweepline.IndexOfValue(selectedValue) + 1;

                    try
                     {
                      //   prevY = Sweepline.Keys[indexOfPrevious];
                         prevSegment = Sweepline.Values[indexOfPrevious];
                     }
                     catch 
                     {
                         prev = false;
                     }
                     try
                     {
                       //  nextY = Sweepline.Keys[indexOfNext];
                         nextSegment = Sweepline.Values[indexOfNext];
                     }
                     catch 
                     {
                         next = false;
                     }
                     if (prev == true || next == true)
                     {
                         if (prev == true && next == true) 
                         {
                             intersect = CheckIntersection(lines[prevSegment],lines[Event.Line_index]);
                             if (intersect) 
                             {
                                 Point intersection_point = GetIntersectionPoint(lines[prevSegment], lines[Event.Line_index]);
                                 if (!Intersection_Points.Contains(intersection_point))
                                 {
                                     Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, prevSegment, Event.Line_index));
                                     Intersection_Points.Add(intersection_point);
                                 }
                             }
                             intersect = CheckIntersection(lines[Event.Line_index], lines[nextSegment]);
                             if (intersect)
                             {
                                 Point intersection_point = GetIntersectionPoint(lines[Event.Line_index], lines[nextSegment]);
                                 if (!Intersection_Points.Contains(intersection_point))
                                 {
                                     Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, Event.Line_index, nextSegment));
                                     Intersection_Points.Add(intersection_point);
                                 }
                             }
                         }
                         else if (prev == true && next == false) 
                         {
                             intersect = CheckIntersection(lines[prevSegment], lines[Event.Line_index]);
                             if (intersect)
                             {
                                 Point intersection_point = GetIntersectionPoint(lines[prevSegment], lines[Event.Line_index]);
                                 if (!Intersection_Points.Contains(intersection_point))
                                 {
                                     Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, prevSegment, Event.Line_index));
                                     Intersection_Points.Add(intersection_point);
                                 }
                             }
                         }
                         else if (prev == false && next == true) 
                         {
                             intersect = CheckIntersection(lines[Event.Line_index], lines[nextSegment]);
                             if (intersect)
                             {
                                 Point intersection_point = GetIntersectionPoint(lines[Event.Line_index], lines[nextSegment]);
                                 if (!Intersection_Points.Contains(intersection_point))
                                 {
                                     Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, Event.Line_index, nextSegment));
                                     Intersection_Points.Add(intersection_point);
                                 }
                             }
                         }
                     }
                     else 
                     {
                         return;
                     }
                 }
                 else if (Event.seg_Type == 1) //keda endpoint
                 {
                     int selectedValue = Event.Line_index;
                     int indexoflinetoberemovev = Sweepline.IndexOfValue(selectedValue);
                     int indexOfPrevious = Sweepline.IndexOfValue(selectedValue) - 1;
                     int indexOfNext = Sweepline.IndexOfValue(selectedValue) + 1;

                     try
                     {
                     //    prevY = Sweepline.Keys[indexOfPrevious];
                         prevSegment = Sweepline.Values[indexOfPrevious];
                     }
                     catch
                     {
                         prev = false;
                     }
                     try
                     {
                      //   nextY = Sweepline.Keys[indexOfNext];
                         nextSegment = Sweepline.Values[indexOfNext];
                     }
                     catch
                     {
                         next = false;
                     }
                     if (prev == true || next == true)
                     {
                         if (prev == true && next == true)
                         {
                             intersect = CheckIntersection(lines[prevSegment], lines[nextSegment]);
                             if (intersect)
                             {
                                 Point intersection_point = GetIntersectionPoint(lines[prevSegment], lines[nextSegment]);
                                 if (!Intersection_Points.Contains(intersection_point))
                                 {
                                     Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, prevSegment, nextSegment));
                                     Intersection_Points.Add(intersection_point);
                                 }
                             }
                         }
                         else if (prev == true && next == false)
                         {
                             double key = Sweepline.ElementAt(indexoflinetoberemovev).Key;
                             Sweepline.Remove(key);
                             return;
                         }
                         else if (prev == false && next == true)
                         {
                             double key = Sweepline.ElementAt(indexoflinetoberemovev).Key;
                             Sweepline.Remove(key);
                             return;
                         }
                     }
                     else
                     {
                         double key = Sweepline.ElementAt(indexoflinetoberemovev).Key;
                         Sweepline.Remove(key);
                         return;
                     }
                     double key_ = Sweepline.ElementAt(indexoflinetoberemovev).Key;//selectedValue dah index 3adyy
                     Sweepline.Remove(key_);
                 }
                 else if (Event.seg_Type == 2) //keda midpoint
                 {
                     int Segment_One = Event.first_index;//keda rg3t index 2wl segment
                     int Segment_Two = Event.second_index;//keda rg3t index tani segment

                     int selectedValue1 = Sweepline.IndexOfValue(Segment_One);
                     int selectedValue2 = Sweepline.IndexOfValue(Segment_Two);

                   //  if (selectedValue1 < selected2)
                     //{*/
                         int s1 = Sweepline.ElementAt(selectedValue1).Value;
                         int s2 = Sweepline.ElementAt(selectedValue2).Value;
                         int indexOfPrevious = Sweepline.IndexOfValue(s1) - 1;
                         int indexOfNext = Sweepline.IndexOfValue(s2) + 1;

                         try
                         {
                           //  prevY = Sweepline.Keys[indexOfPrevious];
                             prevSegment = Sweepline.Values[indexOfPrevious];
                         }
                         catch
                         {
                             prev = false;
                         }
                         try
                         {
                          //   nextY = Sweepline.Keys[indexOfNext];
                             nextSegment = Sweepline.Values[indexOfNext];
                         }
                         catch
                         {
                             next = false;
                         }

                         if (prev == true || next == true)
                         {
                             if (prev == true && next == true)
                             {
                                 intersect = CheckIntersection(lines[Segment_One], lines[nextSegment]);
                                 if (intersect)
                                 {
                                     Point intersection_point = GetIntersectionPoint(lines[Segment_One], lines[nextSegment]);
                                     if (!Intersection_Points.Contains(intersection_point))
                                     {
                                         Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, Segment_One, nextSegment));
                                         Intersection_Points.Add(intersection_point);
                                     }
                                 }
                                 intersect = CheckIntersection(lines[Segment_Two], lines[prevSegment]);
                                 if (intersect)
                                 {
                                     Point intersection_point = GetIntersectionPoint(lines[Segment_Two], lines[prevSegment]);
                                     if (!Intersection_Points.Contains(intersection_point))
                                     {
                                     Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, Segment_Two, prevSegment));
                                     Intersection_Points.Add(intersection_point);
                                     }
                                 }
                             }
                             else if ((prev == true && next == false)) 
                             {
                                 intersect = CheckIntersection(lines[Segment_Two], lines[prevSegment]);
                                 if (intersect)
                                 {
                                     Point intersection_point = GetIntersectionPoint(lines[Segment_Two], lines[prevSegment]);
                                     if (!Intersection_Points.Contains(intersection_point))
                                     {
                                         Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, Segment_Two, prevSegment));
                                         Intersection_Points.Add(intersection_point);
                                     }
                                 }
                             }
                             else if (prev == false && next == true) 
                             {
                                 intersect = CheckIntersection(lines[Segment_One], lines[nextSegment]);
                                
                                 if (intersect)
                                 {
                                     Point intersection_point = GetIntersectionPoint(lines[Segment_One], lines[nextSegment]);
                                     if (!Intersection_Points.Contains(intersection_point))
                                     {
                                         Events.Add(intersection_point.X, new EventPoint(intersection_point, 2, Segment_One, nextSegment));
                                         Intersection_Points.Add(intersection_point);
                                     }
                                 }
                             }
                         }
                         double oldY1 = Sweepline.Keys[selectedValue1];
                         double oldY2 = Sweepline.Keys[selectedValue2];
                         Sweepline.Remove(oldY1);
                         Sweepline.Remove(oldY2);
                         Sweepline.Add(oldY1, Segment_Two);
                         Sweepline.Add(oldY2, Segment_One);
                   }
             }
        
            public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
            {
                Events = IntializeEvents(Events,lines);
                while (Events.Count != 0)
                {
                    EventPoint currentEvent = Events.First().Value;
                    Events.Remove(Events.First().Key);
                    HandleEvents(currentEvent, lines);
                }
                outPoints = Intersection_Points;
            }
       
        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
