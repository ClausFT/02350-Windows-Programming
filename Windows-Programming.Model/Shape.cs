using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Windows_Programming.Model
{
    // The Shape class descripes a shape with a position (X and Y), and a size (Width and Height).
    public class Shape : NotifyBase
    {

        public ObservableCollection<string> Propperties { get; set; }
        public ObservableCollection<string> Methods { get; set; }
        
        // The static integer counter field is used to set the integer Number property to a unique number for each Shape object.
        private static int counter = 0;

        // The Number integer property holds a unique integer for each Shape object to identify them in the View (GUI) layer.
        // The "{ get; set; }" syntax describes that a private field 
        //  and default getter setter methods should be generated 
        //  (the setter will be private because of the "private set;" part).
        // This is called Auto-Implemented Properties (http://msdn.microsoft.com/en-us/library/bb384054.aspx).
        public int Number { get; private set; }

        private int x;
        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("CanvasCenterX"); } }


        private int y;
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("CanvasCenterY"); } }


        private int width;
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width"); NotifyPropertyChanged("CanvasCenterX"); NotifyPropertyChanged("CenterX"); } }


        private int height;
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); NotifyPropertyChanged("CanvasCenterY"); NotifyPropertyChanged("CenterY"); } }

        // Derived properties.
        // Corresponds to making a Getter method in Java (for instance 'public int GetCenterX()'), 
        //  that does not have its own private field, but is calculated from other fields and properties. } }
        // The CanvasCenterX and CanvasCenterY derived properties are used by the Line class, 
        //  so it can be drawn from the center of one Shape to the center of another Shape.
        // NOTE: In the 02350SuperSimpleDemo these derived properties are called CenterX and CenterY, 
        //        but in this demo we need both these and derived properties for the coordinates of the Shape, 
        //        relative to the upper left corner of the Shape. This is an example of a breaking change, 
        //        that is changed during the lifetime of an application, because the requirements change.

        public int CanvasCenterX { get { return X + Width / 2; } set { X = value - Width / 2; NotifyPropertyChanged("X"); } }
        public int CanvasCenterY { get { return Y + Height / 2; } set { Y = value - Height / 2; NotifyPropertyChanged("Y"); } }
        // The CenterX and CenterY properties are used by the Shape animation to define the point of rotation.
        // NOTE: These derived properties are diffent from the Shape properties with the same names, 
        //        from the 02350SuperSimpleDemo, see above for an explanation.

        public int CenterX { get { return Width / 2; } }
        public int CenterY { get { return Height / 2; } }

        // ViewModel properties.
        // These properties should be in the ViewModel layer, but it is easier for the demo to put them here, 
        //  to avoid unnecessary complexity.
        // NOTE: This breaks the seperation of layers of the MVVM architecture pattern.
        //       To avoid this a ShapeViewModel class should be created that wraps all Shape objects, 
        //        but it adds to the complexity of the ViewModel layer and this demo and a simpler solution was chosen for the demo.
        //        (this also adds a reference to the PresentationCore class library which is part of .NET, 
        //         but should not be used in the Model layer, creating an unnecessary dependency for the Model layer class library).
        //       To learn how to avoid this and create an application with a more pure MVVM architecture pattern, 
        //        please ask the Teaching Assistants.
        private bool isSelected;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; NotifyPropertyChanged("IsSelected"); NotifyPropertyChanged("SelectedColor"); } }
        public Brush SelectedColor { get { return IsSelected ? Brushes.Red : Brushes.Yellow; } }

        // Constructor.
        // The constructor is in this case used to set the default values for the properties.
        public Shape()
        {
            // This just means that the integer field called counter is incremented before its value is used to set the Number integer property.
            Number = ++counter;
            X = Y = 200;
            // The "X = Y = value" syntax corresponds to the following:
            // X = 200;
            // Y = 200;
            Width = Height = 100;
            // The "Width = Height = value" syntax corresponds to the following:
            // Width = 200;
            // Height = 200;

            Methods = new ObservableCollection<string>();
            Propperties = new ObservableCollection<string>();
        }

        // By overwriting the ToString() method, the default representation of the class is changed from the full namespace (Java: package) name, 
        //  to the value of the Number integer property, which is meant to be unique for each Shape object.
        // The ToString() method is inheritied from the Object class, that all classes inherit from.
        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
