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
        //private UndoRedoController undoRedoController = new UndoRedoController();

        public readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        // The Redo stack, holding the Undo/Redo commands that have been executed and then unexecuted (undone).
        public readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        // Keeps track of the state, depending on whether a line is being added or not.
        private bool _isAddingLine;
        private bool _isRemovingShape;
        private bool HasSelectedLine { get { return (SelectedLine != null); } }
        //Saves initial position of a shape
        private int _initX;
        private int _initY;
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape _addingLineFrom;
        // Used for making the shapes transparent when a new line is being added.
        public double ModeOpacity { get { return _isAddingLine || _isRemovingShape ? 0.4 : 1.0; } }

        // Collections containing shapes and lines
        public ObservableCollection<Shape> Shapes { get; set; }
        public ObservableCollection<Line> Lines { get; set; }

        #region Commands

        // Commands that the UI can be bound to.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand NewCommand { get; private set; }

        // for Interface

        public ICommand AddInterfaceCommand { get; private set; }


        // Commands that the UI can be bound to.
        public ICommand AddClassCommand { get; private set; }
        public ICommand RemoveShapeCommand { get; private set; }

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

        #endregion
        
        public MainViewModel()
        {
            Shapes = new ObservableCollection<Shape>();
            Lines = new ObservableCollection<Line>();

            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);
            SaveCommand = new RelayCommand(Save);
            LoadCommand = new RelayCommand(Load);
            NewCommand = new RelayCommand(New);

            AddClassCommand = new RelayCommand(AddClass);
            RemoveShapeCommand = new RelayCommand(RemoveShape, CanRemoveShape);

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

        #region Undo/redo controller

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
            return undoStack.Any();
        }
        public void Undo()
        {
            if (!undoStack.Any()) throw new InvalidOperationException();
            IUndoRedoCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.UnExecute();
            UpdateLines();
        }
        public bool CanRedo()
        {
            return redoStack.Any();
        }
        public void Redo()
        {
            if (!redoStack.Any()) throw new InvalidOperationException();
            IUndoRedoCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
            UpdateLines();
        }

        #endregion

        #region Save/load

        public void Save()
        {
            SaveLoadController saveLoadController = new SaveLoadController();
            Diagram diagram = new Diagram();
            diagram.shapes = Shapes.ToList();
            diagram.lines = Lines.ToList();
            saveLoadController.Save(diagram);
        }

        public void Load()
        {
            if (Shapes.Count > 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to discard your current diagram?", "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }
            SaveLoadController saveLoadController = new SaveLoadController();
            Diagram diagram;
            diagram = saveLoadController.Load();
            if (diagram == null)
                return;

            Shapes = new ObservableCollection<Shape>(diagram.shapes);

            RaisePropertyChanged("Shapes");
            Lines = new ObservableCollection<Line>(diagram.lines);

            Lines.ToList().ForEach(x => { x.From = Shapes.Single(y => y.Number == x.FromID); x.To = Shapes.Single(y => y.Number == x.ToID); });
            RaisePropertyChanged("Lines");
        }
        public void New()
        {
            if (Shapes.Count > 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to discard your current diagram?", "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }
            Shapes = new ObservableCollection<Shape>();
            RaisePropertyChanged("Shapes");
            Lines = new ObservableCollection<Line>();
            RaisePropertyChanged("Lines");
        }

        #endregion

        #region Shapes

        // Checks if the chosen Shapes can be removed, which they can if exactly 1 is chosen.
        public bool CanRemoveShape()
        {
            return Shapes.Count > 0;
        }

        private void AddAttribute(object i)
        {
            RemoveLineFocus();
            FrameworkElement shapeVisualElement = (FrameworkElement)i;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            Shape shapeModel = (Shape)shapeVisualElement.DataContext;

            if (shapeModel != null)
            {
                ShapeAttribute shapeAttribute = new ShapeAttribute();
                shapeAttribute.Shape = shapeModel;
                AddAndExecute(new AddAttributeCommand(shapeModel.Properties, shapeAttribute));
                UpdateLines();
            }

                    
        }

        private void AddMethod(object l)
        {
            RemoveLineFocus();
            FrameworkElement shapeVisualElement = (FrameworkElement)l;
            Shape shapeModel = (Shape)shapeVisualElement.DataContext;

            if (shapeModel != null)
            {
                ShapeAttribute shapeAttribute = new ShapeAttribute();
                shapeAttribute.Shape = shapeModel;
                AddAndExecute(new AddMethodCommand(shapeModel.Methods, shapeAttribute));
                UpdateLines();
            }
               
        }

        // Adds a Class.
        public void AddClass()
        {
            RemoveLineFocus();
            Shape klass = new Shape();
            klass.ShapeType = ShapeTypes.Class;
            klass.ShapeTypeName = "Class";
            AddAndExecute(new AddShapeCommand(Shapes, klass));
        }

        public void AddInterface()
        {
            RemoveLineFocus();
            Shape interf = new Shape();
            interf.ShapeType = ShapeTypes.Interface;
            interf.ShapeTypeName = "Interface";
            AddAndExecute(new AddShapeCommand(Shapes, interf));
        }

        // Removes the chosen Shapes with a RemoveShapesCommand.
        public void RemoveShape()
        {
            RemoveLineFocus();
            _isRemovingShape = true;
            RaisePropertyChanged("ModeOpacity");
        }

        #endregion

        #region Relations

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

        private void RemoveLineFocus()
        {
            if (!HasSelectedLine)
                return;

            SelectedLine.StrokeThickness = 1;
            SelectedLine = null;
            RelationText = string.Empty;
        }

        private void UpdateLines()
        {
            foreach (Line element in Lines)
                element.SetShortestLine();
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

        public bool CanAddLine()
        {
            return Shapes.Count > 1;
        }

        #endregion

        #region Mouse events

        public void MouseDownShape(MouseButtonEventArgs e)
        {
            if (!_isAddingLine) e.MouseDevice.Target.CaptureMouse();
            FrameworkElement shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            Shape shapeModel = (Shape)shapeVisualElement.DataContext; // Get shape element
            _initX = shapeModel.CanvasCenterX;
            _initY = shapeModel.CanvasCenterY;
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
            shapeModel.CanvasCenterX = (int)mousePosition.X;
            shapeModel.CanvasCenterY = (int)mousePosition.Y;
            UpdateLines();

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


                AddAndExecute(new MoveShapeCommand(shape, _initX, _initY, (int)mousePosition.X, (int)mousePosition.Y));
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
            UpdateLines();
        }

        // Recursive method for finding the parent of a visual element of a certain type, 
        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }

        #endregion   
    }
}