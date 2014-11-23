using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Windows_Programming.Model.Utils
{
    public class Compute
    {
        // Calculates the length of a line given its coordinates
        public static double LineLength(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        // Calculates the midpoint of a given coordinate set
        public static Point MidPoint(Point p1, Point p2)
        {
            Point p = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            return p;
        }

        public static double Theta(double x1, double y1, double x2, double y2)
        {
            return Math.Atan2(y1 - y2, x1 - x2);
        }
    }
}
