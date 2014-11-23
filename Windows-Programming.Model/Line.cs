using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Windows_Programming.Model.Enums;
using Windows_Programming.Model.Utils;

namespace Windows_Programming.Model
{
    // The Line class has a reference to 2 shapes, that it connects.
    public class Line : NotifyBase
    {
        public Line()
        {
            StrokeThickness = 1;
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

        private string _text;
        public string Text { get { return _text; } set { _text = value; NotifyPropertyChanged("Text"); } }

        private Thickness _textMargin;
        public Thickness TextMargin { get { return _textMargin; } set { _textMargin = value; NotifyPropertyChanged("TextMargin"); } }

        private double _strokeThickness;
        public double StrokeThickness { get { return _strokeThickness; } set { _strokeThickness = value; NotifyPropertyChanged("StrokeThickness"); } }

        public RelationTypes Type { get; set; }

        // Sets the coordinates to the shortest line between the two shapes
        public void SetShortestLine()
        {
            int[,] coord = { { From.X, From.Y + From.Height, To.X, To.Y+1 }, //bottom-top
                             { From.X, From.Y+1, To.X, To.Y + To.Height+1 }, //top-bottom
                             { From.X + From.Width, From.Y, To.X+1, To.Y },  //right-left
                             { From.X+1, From.Y, To.X + To.Width+1, To.Y } };  //left-right

            int widthUnit = From.Width / 25;
            int heightUnit = From.Height/ 25;
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            double minLength = -1;
            const int margin = 5;

            //This loop calculates shortest line from top-bottom/bottom-top (Y axis is constant)
            for (int i = 0; i < 2; i++) //For each set top-bottom coordinat (first two elements in array)
            {
                for (int k = margin; k < From.Width; k = k + widthUnit)
                {
                    for (int j = margin; j < From.Width; j = j + widthUnit)
                    {
                        //Calculate the length of the line
                        double lineLength = Compute.LineLength(coord[i, 0] + k, coord[i, 1], coord[i, 2] + j, coord[i, 3]);

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
                for (int k = margin; k < From.Height; k = k + heightUnit)
                {
                    for (int j = margin; j < From.Height; j = j + heightUnit)
                    {
                        //Calculate the length of the line
                        double lineLength = Compute.LineLength(coord[i, 0], coord[i, 1] + k, coord[i, 2], coord[i, 3] + j);

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

            //Compute the new midpoint of the line
            Point p = Compute.MidPoint(new Point(FromX, FromY), new Point(ToX, ToY));
            TextMargin = new Thickness(p.X+2, p.Y+2, 0, 0);
        }
    }
}