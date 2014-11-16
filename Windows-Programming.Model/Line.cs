using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Windows_Programming.Model
{
    // The Line class has a reference to 2 shapes, that it connects.
    public class Line : NotifyBase
    {
        public Line()
        {
            Type = Relation.Relations.Inheritance;
        }
        // Properties.
        // Normally Auto-Implemented Properties (http://msdn.microsoft.com/en-us/library/bb384054.aspx) would be used, 
        //  but in this case additional work has to be done when the property is changed, 
        //  which is to raise an INotifyPropertyChanged event that notifies the View (GUI) that this model property has changed, 
        //  so the graphical representation can be updated.

        private Shape from;
        public Shape From { get { return from; } set { from = value; NotifyPropertyChanged("From"); } }

        private Shape to;
        public Shape To { get { return to; } set { to = value; NotifyPropertyChanged("To"); } }

        private int _fromX;
        public int FromX { get { return _fromX; } set { _fromX = value; NotifyPropertyChanged("FromX"); } }

        private int _fromY;
        public int FromY { get { return _fromY; } set { _fromY = value; NotifyPropertyChanged("FromY"); } }

        private int _toX;
        public int ToX { get { return _toX; } set { _toX = value; NotifyPropertyChanged("ToX"); } }

        private int _toY;
        public int ToY { get { return _toY; } set { _toY = value; NotifyPropertyChanged("ToY"); } }

        public Relation.Relations Type { get; set; }

        // Sets the coordinates to the shortest line between the two shapes
        public void SetShortestLine()
        {
            int[,] coord = { { From.X, From.Y + From.Height, To.X, To.Y }, //top-bottom
                             { From.X, From.Y, To.X, To.Y + To.Height }, //bottom-top
                             { From.X + From.Width, From.Y, To.X, To.Y },  //left-right
                             { From.X, From.Y, To.X + To.Width, To.Y } };  //right-left

            int widthUnit = From.Width / 4;
            int heightUnit = From.Height / 4;
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            double minLength = -1;

            //This loop calculates shortest line from top-bottom/bottom-top (Y axis is constant)
            for (int i = 0; i < 2; i++) //For each set top-bottom coordinat (first two elements in array)
            {
                for (int k = widthUnit/2; k <= From.Width - widthUnit/2; k = k + widthUnit)
                {
                    for (int j = widthUnit/2; j <= From.Width - widthUnit/2; j = j + widthUnit)
                    {
                        //Calculate the length of the line
                        double lineLength = LineLength(coord[i, 0] + k, coord[i, 1], coord[i, 2] + j, coord[i, 3]);

                        //If current line is the shortest so far, save length and coordinates
                        if (minLength == -1 || lineLength <= minLength)
                        {
                            minLength = lineLength;
                            x1 = coord[i, 0] + k;
                            y1 = coord[i, 1];
                            x2 = coord[i, 2] + j;
                            y2 = coord[i, 3];
                        }
                    }
                }
            }

            //This loop calculates shortest line from left-right/right-left (X axis is constant)
            for (int i = 2; i < 4; i++) //For each set left-right coordinat (last two elements in array)
            {
                for (int k = heightUnit/2; k <= From.Height - heightUnit/2; k = k + heightUnit)
                {
                    for (int j = heightUnit/2; j <= From.Height - heightUnit/2; j = j + heightUnit)
                    {
                        //Calculate the length of the line
                        double lineLength = LineLength(coord[i, 0], coord[i, 1] + k, coord[i, 2], coord[i, 3] + j);

                        //If current line is the shortest so far, save length and coordinates
                        if (minLength == -1 || lineLength <= minLength)
                        {
                            minLength = lineLength;
                            x1 = coord[i, 0];
                            y1 = coord[i, 1] + k;
                            x2 = coord[i, 2];
                            y2 = coord[i, 3] + j;
                        }
                    }
                }
            }

            FromX = x1;
            FromY = y1;
            ToX = x2;
            ToY = y2;
        }

        // Calculates the length of a line given its coordinates
        private double LineLength(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
