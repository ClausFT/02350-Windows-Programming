using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Ink;
using Windows_Programming.Model;
using Windows_Programming;
using Windows_Programming.Model.Enums;
using Windows_Programming.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Windows_Programming.Command;

namespace Windows_Programming.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Type for the shape to be created
        private RelationTypes Type { get; set; }
        //Selected line for adding relation text
        private Line SelectedLine { get; set; }
        // A reference to the Undo/Redo controller.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();
        private SaveLoadController saveLoadController = new SaveLoadController();


        // Keeps track of the state, depending on whether a line is being added or not.
        private bool _isAddingLine;
        private bool HasSelectedLine { get { return (SelectedLine != null); } }
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape _addingLineFrom;
        // Saves the initial point that the mouse has during a move operation.
        private Point _moveShapePoint;
        // Used for making the shapes transparent when a new line is being added.
        public double ModeOpacity { get { return _isAddingLine ? 0.4 : 1.0; } }

        // Collections containing shapes and lines
        public ObservableCollection<Shape> Shapes { get; set; }
        public static ObservableCollection<Line> Lines { get; set; }


        // Commands that the UI can be bound to.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }

        // Commands that the UI can be bound to.
        public ICommand AddShapeCommand { get; private set; }
        public ICommand RemoveShapeCommand { get; private set; }
        public ICommand RemoveLinesCommand { get; private set; }

        public ICommand AddAssociationCommand { get; private set; }
        public ICommand AddInheritanceCommand { get; private set; }
        public ICommand AddAggregationCommand { get; private set; }
        public ICommand AddCompositionCommand { get; private set; }

        // Commands that the UI can be bound to.
        public ICommand MouseDownShapeCommand { get; private set; }
        public ICommand MouseDownLineCommand { get; private set; }
        public ICommand MouseMoveShapeCommand { get; private set; }
        public ICommand MouseUpShapeCommand { get; private set; }

        public MainViewModel()
        {
            Shapes = new ObservableCollection<Shape>();
            Lines = new ObservableCollection<Line>();

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);

            AddShapeCommand = new RelayCommand(AddShape);
            RemoveShapeCommand = new RelayCommand<IList>(RemoveShape, CanRemoveShape);
            RemoveLinesCommand = new RelayCommand<IList>(RemoveLines, CanRemoveLines);

            AddAssociationCommand = new RelayCommand(AddAssociation, CanAddLine);
            AddInheritanceCommand = new RelayCommand(AddInheritance, CanAddLine);
            AddAggregationCommand = new RelayCommand(AddAggregation, CanAddLine);
            AddCompositionCommand = new RelayCommand(AddComposition, CanAddLine);

            MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
            MouseDownLineCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownLine);
            MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
            MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);
        }
        public void Save()
        {
            Diagram diagram = new Diagram();
            diagram.shapes = Shapes.ToList();
            diagram.lines = Lines.ToList();
            saveLoadController.Save(diagram, "C:\\Users\\Benjamin\\test.txt");
        }
        public void Load()
        {
            Diagram diagram;
            diagram = saveLoadController.Load("C:\\Users\\Benjamin\\test.txt");
            Shapes = new ObservableCollection<Shape>(diagram.shapes);
            Lines = new ObservableCollection<Line>(diagram.lines);

        }
        // Adds a Shape with an AddShapeCommand.
        public void AddShape()
        {
            RemoveLineFocus();
            undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
        }

        // Checks if the chosen Shapes can be removed, which they can if exactly 1 is chosen.
        public bool CanRemoveShape(IList _shapes)
        {
            return _shapes.Count == 1;
        }

        public bool CanAddLine()
        {
            return Shapes.Count > 1;
        }

        // Removes the chosen Shapes with a RemoveShapesCommand.
        public void RemoveShape(IList _shapes)
        {
            undoRedoController.AddAndExecute(new RemoveShapesCommand(Shapes, Lines, _shapes.Cast<Shape>().ToList()));
        }

        public void AddAssociation()
        {
            RemoveLineFocus();
            Type = RelationTypes.Association;
            _isAddingLine = true;
            RaisePropertyChanged("ModeOpacity");
        }

        public void AddInheritance()
        {
            RemoveLineFocus();
            Type = RelationTypes.Inheritance;
            _isAddingLine = true;
            RaisePropertyChanged("ModeOpacity");
        }

        public void AddAggregation()
        {
            RemoveLineFocus();
            Type = RelationTypes.Aggregation;
            _isAddingLine = true;
            RaisePropertyChanged("ModeOpacity");
        }

        public void AddComposition()
        {
            RemoveLineFocus();
            Type = RelationTypes.Composition;
            _isAddingLine = true;
            RaisePropertyChanged("ModeOpacity");
        }

        // Checks if the chosen Lines can be removed, which they can if at least one is chosen.
        public bool CanRemoveLines(IList _edges)
        {
            return _edges.Count >= 1;
        }

        // Removes the chosen Lines with a RemoveLinesCommand.
        public void RemoveLines(IList _lines)
        {
            undoRedoController.AddAndExecute(new RemoveLinesCommand(Lines, _lines.Cast<Line>().ToList()));
        }

        public void MouseDownShape(MouseButtonEventArgs e)
        {
            if (!_isAddingLine) e.MouseDevice.Target.CaptureMouse();
        }

        private void RemoveLineFocus()
        {
            if (!HasSelectedLine)
                return;

            SelectedLine.StrokeThickness = 1;
            SelectedLine = null;
            RelationText = string.Empty;
        }

        //Contains the relation text for the selected line
        private string _relationText;
        public string RelationText
        {
            get { return _relationText; }
            set
            {
                _relationText = value;
                if (HasSelectedLine)
                    SelectedLine.Text = RelationText;
                RaisePropertyChanged();
            }
        }

        public void MouseDownLine(MouseButtonEventArgs e)
        {
            if (HasSelectedLine)
                SelectedLine.StrokeThickness = 1;

            FrameworkElement lineVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            SelectedLine = (Line)lineVisualElement.DataContext;
            SelectedLine.StrokeThickness = 2;
            RelationText = (!string.IsNullOrEmpty(SelectedLine.Text)) ? SelectedLine.Text : string.Empty;
        }

        // This is only used for moving a Shape, and only if the mouse is already captured.
        public void MouseMoveShape(MouseEventArgs e)
        {
            // Checks that the mouse is captured and that a line is not being drawn.
            if (Mouse.Captured == null || _isAddingLine)
                return;

            RemoveLineFocus();
            FrameworkElement shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            Shape shapeModel = (Shape)shapeVisualElement.DataContext; // Get shape element
            Canvas canvas = FindParentOfType<Canvas>(shapeVisualElement);
            Point mousePosition = Mouse.GetPosition(canvas);
            if (_moveShapePoint == default(Point)) _moveShapePoint = mousePosition;
            shapeModel.CanvasCenterX = (int)mousePosition.X;
            shapeModel.CanvasCenterY = (int)mousePosition.Y;

            foreach (Line element in Lines)
                element.SetShortestLine();

        }

        public void MouseUpShape(MouseButtonEventArgs e)
        {
            // Used for adding a Line.
            if (_isAddingLine)
            {
                FrameworkElement shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
                Shape shape = (Shape)shapeVisualElement.DataContext;
                if (_addingLineFrom == null) { _addingLineFrom = shape; }
                else if (_addingLineFrom.Number != shape.Number)
                {
                    Line line = new Line { From = _addingLineFrom, To = shape, Type = Type };
                    undoRedoController.AddAndExecute(new AddLineCommand(Lines, line));
                    RemoveLineFocus(); //If a line was selected, remove focus
                    line.SetShortestLine();
                    line.StrokeThickness = 2;
                    SelectedLine = line;
                    _isAddingLine = false;
                    _addingLineFrom = null;
                    RaisePropertyChanged("ModeOpacity");
                }
            }
            else // Used for moving a Shape.
            {
                FrameworkElement shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
                Shape shape = (Shape)shapeVisualElement.DataContext; // From the shapes visual element, the Shape object which is the DataContext is retrieved.
                Canvas canvas = FindParentOfType<Canvas>(shapeVisualElement); // Canvas holding the shapes visual element
                Point mousePosition = Mouse.GetPosition(canvas); //Mouse position relative to the canvas is gotten here.
                undoRedoController.AddAndExecute(new MoveShapeCommand(shape, (int)_moveShapePoint.X, (int)_moveShapePoint.Y, (int)mousePosition.X, (int)mousePosition.Y));
                _moveShapePoint = new Point();
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
        }

        // Recursive method for finding the parent of a visual element of a certain type, 
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }
    }
}