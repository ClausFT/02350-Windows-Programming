﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Windows_Programming.Model
{
    public class Relation : System.Windows.Shapes.Shape
    {

        public enum Relations
        {
            Association,
            Inheritance,
            Aggregation,
            Composition
        };

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(Relations), typeof(Relation));

        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadWidth
        {
            get { return (double)base.GetValue(HeadWidthProperty); }
            set { base.SetValue(HeadWidthProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadHeight
        {
            get { return (double)base.GetValue(HeadHeightProperty); }
            set { base.SetValue(HeadHeightProperty, value); }
        }

        [TypeConverter(typeof(Relations))]
        public Relations Type
        {
            get { return (Relations)base.GetValue(TypeProperty); }
            set { base.SetValue(TypeProperty, value); }
        }

        
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            double theta = Math.Atan2(Y1 - Y2, X1 - X2);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt1 = new Point(X1, this.Y1);
            Point pt2 = new Point(X2, this.Y2);
            Point pt3 = new Point(
                    X2 + (HeadWidth * cost - HeadHeight * sint),
                    Y2 + (HeadWidth * sint + HeadHeight * cost));
            Point pt4 = new Point(
                    X2 + (HeadWidth * cost + HeadHeight * sint),
                    Y2 - (HeadHeight * cost - HeadWidth * sint));

            switch (Type)
            {
                case Relations.Association:
                    context.BeginFigure(pt1, true, false);
                    context.LineTo(pt2, true, true);
                    context.LineTo(pt3, true, true);
                    context.LineTo(pt2, true, true);
                    context.LineTo(pt4, true, true);
                    break;

                case Relations.Inheritance:
                    Point pt5 = MidPoint(pt3, pt4);
                    context.BeginFigure(pt1, true, false);
                    context.LineTo(pt5, true, true);
                    context.LineTo(pt3, true, true);
                    context.LineTo(pt2, true, true);
                    context.LineTo(pt4, true, true);
                    context.LineTo(pt5, true, true);

                    break;
                case Relations.Aggregation:

                    break;
                case Relations.Composition:

                    break;
            }
        }

        private Point MidPoint(Point p1, Point p2)
        {
            Point p = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            return p;
        }
    }
}
