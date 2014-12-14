using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using Windows_Programming.Model.Enums;

namespace Windows_Programming.Model
{
    public class Shape : NotifyBase
    {
        public string ShapeTypeName { get; set; }
        public ShapeTypes ShapeType { get; set; }
        public ObservableCollection<ShapeAttribute> Properties { get; set; }
        public ObservableCollection<ShapeAttribute> Methods { get; set; }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }
        
        // The static integer counter field is used to set the integer Number property to a unique number for each Shape object.
        private static int counter = 0;

        public int Number { get; set; }

        private int x;
        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("CanvasCenterX"); } }

        private int y;
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("CanvasCenterY"); } }

        private int width;
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width"); NotifyPropertyChanged("CanvasCenterX"); NotifyPropertyChanged("CenterX"); } }


        private int height;
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); NotifyPropertyChanged("CanvasCenterY"); NotifyPropertyChanged("CenterY"); } }

        public int CanvasCenterX { get { return X + Width / 2; } set { X = value - Width / 2; NotifyPropertyChanged("X"); } }
        public int CanvasCenterY { get { return Y + Height / 2; } set { Y = value - Height / 2; NotifyPropertyChanged("Y"); } }
        public int CenterX { get { return Width / 2; } }
        public int CenterY { get { return Height / 2; } }

        public Shape()
        {
            // This just means that the integer field called counter is incremented before its value is used to set the Number integer property.
            Number = ++counter;
            X = Y = 0;
            Width = 144;
            Height = 107;
            Methods = new ObservableCollection<ShapeAttribute>();
            Properties = new ObservableCollection<ShapeAttribute>();
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
