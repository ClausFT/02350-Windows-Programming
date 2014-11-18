using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Windows_Programming.Model.Utils;

namespace Windows_Programming.Model
{
    public class LineHeads
    {
        private static double _theta;
        private static double _sin;
        private static double _cos;
        private static Point _lineStart;
        private static Point _lineEnd;

        public static void Association(StreamGeometryContext context, double x1, double y1, double x2, double y2)
        {
            const int headWidth = 11;
            const int headHeight = 7;
            InitiateCommonProperties(x1, y1, x2, y2);

            Point arrowPoint1 = new Point(
                    x2 + (headWidth * _cos - headHeight * _sin),
                    y2 + (headWidth * _sin + headHeight * _cos));
            Point arrowpoint2 = new Point(
                    x2 + (headWidth * _cos + headHeight * _sin),
                    y2 - (headHeight * _cos - headWidth * _sin));

            context.BeginFigure(_lineStart, true, false);
            context.LineTo(_lineEnd, true, true);
            context.LineTo(arrowPoint1, true, true);
            context.LineTo(_lineEnd, true, true);
            context.LineTo(arrowpoint2, true, true);

        }

        public static void Inheritance(StreamGeometryContext context, double x1, double y1, double x2, double y2)
        {
            const int headWidth = 11;
            const int headHeight = 7;
            InitiateCommonProperties(x1, y1, x2, y2);

            Point arrowPoint1 = new Point(
                    x2 + (headWidth * _cos - headHeight * _sin),
                    y2 + (headWidth * _sin + headHeight * _cos));
            Point arrowPoint2 = new Point(
                    x2 + (headWidth * _cos + headHeight * _sin),
                    y2 - (headHeight * _cos - headWidth * _sin));
            Point middle = Compute.MidPoint(arrowPoint1, arrowPoint2);

            context.BeginFigure(_lineStart, true, false);
            context.LineTo(middle, true, true);
            context.LineTo(arrowPoint1, true, true);
            context.LineTo(_lineEnd, true, true);
            context.LineTo(arrowPoint2, true, true);
            context.LineTo(middle, true, true);
        }

        //Used for aggregation and composition
        public static void Rhombe(StreamGeometryContext context, double x1, double y1, double x2, double y2)
        {
            const int headWidth = 20;
            const int headHeight = 12;
            InitiateCommonProperties(x1, y1, x2, y2);

            Point pt3 = new Point(
                   x2 + (headWidth * _cos - headHeight * _sin),
                   y2 + (headWidth * _sin + headHeight * _cos));
            Point pt4 = new Point(
                    x2 + (headWidth * _cos + headHeight * _sin),
                    y2 - (headHeight * _cos - headWidth * _sin));
            Point bottomMidPoint = Compute.MidPoint(pt3, pt4);
            Point middlePoint1 = new Point(
                x2 + (headWidth / 2 * _cos - headHeight / 2 * _sin),
                y2 + (headWidth / 2 * _sin + headHeight / 2 * _cos));
            Point middlepoint2 = new Point(
            x2 + (headWidth / 2 * _cos + headHeight / 2 * _sin),
            y2 - (headHeight / 2 * _cos - headWidth / 2 * _sin));

            context.BeginFigure(_lineStart, true, false);
            context.LineTo(bottomMidPoint, true, true);
            context.LineTo(middlePoint1, true, true);
            context.LineTo(_lineEnd, true, true);
            context.LineTo(middlepoint2, true, true);
            context.LineTo(bottomMidPoint, true, true);
        }

        private static void InitiateCommonProperties(double x1, double y1, double x2, double y2)
        {
            _theta = Compute.Theta(x1, y1, x2, y2);
            _sin = Math.Sin(_theta);
            _cos = Math.Cos(_theta);
            _lineStart = new Point(x1, y1);
            _lineEnd = new Point(x2, y2);
        }
    }
}
