using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Windows_Programming.Model.Enums;

namespace Windows_Programming.View
{
    public class Relation : System.Windows.Shapes.Shape
    {
        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Relation), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(RelationTypes), typeof(Relation));

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

        [TypeConverter(typeof(RelationTypes))]
        public RelationTypes Type
        {
            get { return (RelationTypes)base.GetValue(TypeProperty); }
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
            switch (Type)
            {
                case RelationTypes.Association:
                    LineHeads.Association(context, X1, Y1, X2, Y2);
                    break;

                case RelationTypes.Inheritance:
                    LineHeads.Inheritance(context, X1, Y1, X2, Y2);
                    break;

                case RelationTypes.Aggregation:
                    LineHeads.Rhombe(context, X1, Y1, X2, Y2);
                    break;

                case RelationTypes.Composition:
                    LineHeads.Rhombe(context, X1, Y1, X2, Y2);
                    Fill = Brushes.Black;
                    break;
            }
        }
    }
}