using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
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
using System.Threading;



namespace Windows_Programming.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Type for the shape to be created
        private RelationTypes Type { get; set; }
        //Selected line for adding relation text
        private Line SelectedLine { get; set; }
        // A reference to the Undo/Redo controller.
        //private UndoRedoController undoRedoController = new UndoRedoController();
        private SaveLoadController saveLoadController = new SaveLoadController();

        public readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        // The Redo stack, holding the Undo/Redo commands that have been executed and then unexecuted (undone).
        public readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        // Keeps track of the state, depending on whether a line is being added or not.
        private bool _isAddingLine;
        private bool _isRemovingShape;
        private bool HasSelectedLine { get { return (SelectedLine != null); } }
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape _addingLineFrom;
        // Saves the initial point that the mouse has during a move operation.
        private Point _moveShapePoint;
        // Used for making the shapes transparent when a new line is being added.
        public double ModeOpacity { get { return _isAddingLine || _isRemovingShape ? 0.4 : 1.0; } }

        // Collections containing shapes and lines
        public ObservableCollection<Shape> Shapes { get; set; }
        public ObservableCollection<Line> Lines { get; set; }

        // Commands that the UI can be bound to.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }

        // for Interface

        public ICommand AddInterfaceCommand { get; private set; }


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

        public ICommand AddAttributeCommand { get; private set; }
        public ICommand AddMethodCommand { get; private set; }

        
        public MainViewModel()
        {
            Shapes = new ObservableCollection<Shape>();
            Lines = new ObservableCollection<Line>();

            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);

            AddShapeCommand = new RelayCommand(AddShape);
            RemoveShapeCommand = new RelayCommand(RemoveShape, CanRemoveShape);
            RemoveLinesCommand = new RelayCommand<IList>(RemoveLines, CanRemoveLines);

            AddInterfaceCommand = new RelayCommand(AddInterface);

            AddAssociationCommand = new RelayCommand(AddAssociation, CanAddLine);
            AddInheritanceCommand = new RelayCommand(AddInheritance, CanAddLine);
            AddAggregationCommand = new RelayCommand(AddAggregation, CanAddLine);
            AddCompositionCommand = new RelayCommand(AddComposition, CanAddLine);

            MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
            MouseDownLineCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownLine);
            MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
            MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);

            AddAttributeCommand = new RelayCommand<object>(AddAttribute);
            AddMethodCommand = new RelayCommand<object>(AddMethod);
        }
        public void AddAndExecute(IUndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
            command.Execute();
        }

        public void Add(IUndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
        }
        public bool CanUndo()
        {
            // Lambda expression to check that the 'undoStack' collection is not empty.
            return undoStack.Any();
            // This is equivalent to the following in Java (can also be done like this in .NET):

            // return undoStack.Count > 0;
        }
        public void Undo()
        {
            if (!undoStack.Any()) throw new InvalidOperationException();
            IUndoRedoCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.UnExecute();

            //Not so pretty hack for updating line positions when undoing
            foreach (Line element in Lines)
                element.SetShortestLine();
        }
        public bool CanRedo()
        {
            // Lambda expression to check that the 'redoStack' collection is not empty.
            return redoStack.Any();
            // This is equivalent to the following in Java (can also be done like this in .NET):

            // return redoStack.Count > 0;
        }
        // Redoes the Undo/Redo command that was last unexecuted (undone), if possible.
        public void Redo()
        {
            if (!redoStack.Any()) throw new InvalidOperationException();
            IUndoRedoCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();

            foreach (Line element in Lines)
                element.SetShortestLine();
        }

        private void AddAttribute(object i)
        {
            FrameworkElement shapeVisualElement = (FrameworkElement)i;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            Shape shapeModel = (Shape)shapeVisualElement.DataContext;

            if (shapeModel != null)
                AddAndExecute(new AddAttributeCommand(shapeModel.Propperties, new ShapeAttribute ("")));
        }

        private void AddMethod(object l)
        {

            FrameworkElement shapeVisualElement = (FrameworkElement)l;
            Shape shapeModel = (Shape)shapeVisualElement.DataContext;

            if (shapeModel != null)
                AddAndExecute(new AddMethodCommand(shapeModel.Methods,new ShapeAttribute ("")));
            
            
            
            //MethodPanel.Children.Add(new TextBox());
        }



        public void Save()
        {
            Diagram diagram = new Diagram();
            diagram.shapes = Shapes.ToList();
            diagram.lines = Lines.ToList();
            saveLoadController.Save(diagram);
            //Thread saveThread = new Thread(new ParameterizedThreadStart(saveLoadController.Save));
            //saveThread.Start(diagram);
            //while (!saveThread.IsAlive);
            //Thread.Sleep(1);
            //saveThread.Abort();
            //saveThread.Join();

        }

        public void Load()
        {
            Diagram diagram;
            diagram = saveLoadController.Load();
            if (diagram == null)
                return;
            Console.Out.WriteLine(diagram);
            Shapes = new ObservableCollection<Shape>(diagram.shapes);

            RaisePropertyChanged("Shapes");
            Lines = new ObservableCollection<Line>(diagram.lines);

            //Lines.ToList().ForEach(x => { x.From = Shapes.Single(y => y.Number == x.FromID); x.To = Shapes.Single(y => y.Number == x.ToID); });
            foreach (Line line in Lines)
            {
                foreach (Shape shape in Shapes)
                {
                    if (shape.Number == line.FromID)
                    {
                        line.From = shape;
                        break;
                    }

                    if (shape.Number == line.ToID)
                    {
                        line.To = shape;
                        break;
                    }
                }


                //foreach (Shape shape in Shapes)
                //{
                //    if (shape.Number == line.ToID)
                //    {
                //        line.To = shape;
                //        break;
                //    }
                //}

            }
            RaisePropertyChanged("Lines");

            foreach (Line element in Lines)
                element.SetShortestLine();

            RaisePropertyChanged("Lines");

        }
        // Adds a Shape with an AddShapeCommand.
        public void AddShape()
        {
            RemoveLineFocus();
            AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
        }

        // Checks if the chosen Shapes can be removed, which they can if exactly 1 is chosen.
        public bool CanRemoveShape()
        {
            return Shapes.Count > 0;
        }

        public bool CanAddLine()
        {
            return Shapes.Count > 1;
        }

        // Removes the chosen Shapes with a RemoveShapesCommand.
        public void RemoveShape()
        {
            RemoveLineFocus();
            _isRemovingShape = true;
            RaisePropertyChanged("ModeOpacity");
        }

        public void AddInterface()
        {
            RemoveLineFocus();
            AddAndExecute(new AddShapeCommand(Shapes, new Shape(ShapeType.interfaceShape)));
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
            AddAndExecute(new RemoveLinesCommand(Lines, _lines.Cast<Line>().ToList()));
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

        private void SetLineFocus(Line line)
        {
            if (HasSelectedLine)
                return;

            SelectedLine = line;
            SelectedLine.StrokeThickness = 2;
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
                RemoveLineFocus();

            FrameworkElement lineVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            SetLineFocus((Line)lineVisualElement.DataContext);
            RelationText = (!string.IsNullOrEmpty(SelectedLine.Text)) ? SelectedLine.Text : string.Empty;
        }

        // This is used for moving a Shape, and only if the mouse is already captured.
        public void MouseMoveShape(MouseEventArgs e)
        {                
            // Checks that the mouse is captured and that a line is not being drawn.
            if (Mouse.Captured == null || _isAddingLine)
                return;

            if (_isRemovingShape)
            {
                _isRemovingShape = false;
                RaisePropertyChanged("ModeOpacity");
                return;
            }

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
            FrameworkElement shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            Shape shape = (Shape)shapeVisualElement.DataContext;

            // Used for adding a Line.
            if (_isAddingLine)
            {
                
                if (_addingLineFrom == null) { _addingLineFrom = shape; }
                else if (_addingLineFrom.Number != shape.Number)
                {
                    Line line = new Line { From = _addingLineFrom, To = shape, Type = Type };
                    AddAndExecute(new AddLineCommand(Lines, line));
                    RemoveLineFocus(); //If a line was selected, remove focus
                    line.SetShortestLine();
                    SetLineFocus(line);
                    _isAddingLine = false;
                    _addingLineFrom = null;
                    RaisePropertyChanged("ModeOpacity");
                }
            }
            else if (_isRemovingShape) //Used for removing a shape
            {
                IList _shapeToRemove = new ArrayList();
                _shapeToRemove.Add(shape);
                AddAndExecute(new RemoveShapesCommand(Shapes, Lines, _shapeToRemove.Cast<Shape>().ToList()));
            }
            else // Used for moving a Shape.
            {
                Canvas canvas = FindParentOfType<Canvas>(shapeVisualElement); // Canvas holding the shapes visual element
                Point mousePosition = Mouse.GetPosition(canvas); //Mouse position relative to the canvas is gotten here.
                AddAndExecute(new MoveShapeCommand(shape, (int)_moveShapePoint.X, (int)_moveShapePoint.Y, (int)mousePosition.X, (int)mousePosition.Y));
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