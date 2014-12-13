using Windows_Programming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Windows_Programming.Command
{
    // Undo/Redo command for adding a Shape.
    public class AddShapeCommand : IUndoRedoCommand
    {
        // Fields.
        // The 'shapes' field holds the current collection of shapes, 
        //  and the reference points to the same collection as the one the MainViewModel point to, 
        //  therefore when this collection is changed in a object of this class, 
        //  it also changes the collection that the MainViewModel uses.
        // For a description of an ObservableCollection see the MainViewModel class.
        private ObservableCollection<Shape> shapes;
        // The 'shape' field holds a new shape, that is added to the 'shapes' collection, 
        //  and if undone, it is removed from the collection.
        private Shape shape;

        // Constructor for saving and changing the current state of the diagram 
        //  (or at least the relevant parts).
        public AddShapeCommand(ObservableCollection<Shape> _shapes, Shape _shape)
        {
            shapes = _shapes;
            shape = _shape;
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            shapes.Add(shape);
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            shapes.Remove(shape);
        }
    }


    //public class AddInterfaceCommand: IUndoRedoCommand
    //{
    //    private ObservableCollection<InterfaceShape> interfaceList;
    //    // The 'shape' field holds a new shape, that is added to the 'shapes' collection, 
    //    //  and if undone, it is removed from the collection.
    //    private Shape currentInterface;

    //    // Constructor for saving and changing the current state of the diagram 
    //    //  (or at least the relevant parts).
    //    public AddInterfaceCommand(ObservableCollection<InterfaceShape> _shapes, InterfaceShape _shape)
    //    {
    //        interfaceList = _shapes;
    //        currentInterface = _shape;
    //    }

    //    // Methods.
    //    // This method is for doing and redoing the command.
    //    public void Execute()
    //    {
    //        interfaceList.Add(currentInterface);
    //    }

    //    // This method is for undoing the command.
    //    public void UnExecute()
    //    {
    //        interfaceList.Remove(currentInterface);
    //    }
    //}



    
    // Undo/Redo command for adding a Shape.
    public class AddAttributeCommand : IUndoRedoCommand //TODO: Make AddMethodCommand
    {
        // Fields.
        // The 'shapes' field holds the current collection of shapes, 
        //  and the reference points to the same collection as the one the MainViewModel point to, 
        //  therefore when this collection is changed in a object of this class, 
        //  it also changes the collection that the MainViewModel uses.
        // For a description of an ObservableCollection see the MainViewModel class.
        private ObservableCollection<ShapeAttribute> _attributes;
        // The 'shape' field holds a new shape, that is added to the 'shapes' collection, 
        //  and if undone, it is removed from the collection.
        private ShapeAttribute _attribute;

        // Constructor for saving and changing the current state of the diagram 
        //  (or at least the relevant parts).
        public AddAttributeCommand(ObservableCollection<ShapeAttribute> attributes, ShapeAttribute attribute)
        {
            _attributes = attributes;
            _attribute = attribute;
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            _attributes.Add(_attribute);
            if (_attribute.Shape.Propperties.Count > 1)
                _attribute.Shape.Height += 22;
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            _attributes.Remove(_attribute);
            if (_attribute.Shape.Propperties.Count > 0)
                _attribute.Shape.Height -= 22;

        }   
    }


    public class AddMethodCommand : IUndoRedoCommand 
    {
        // Fields.
        // The 'shapes' field holds the current collection of shapes, 
        //  and the reference points to the same collection as the one the MainViewModel point to, 
        //  therefore when this collection is changed in a object of this class, 
        //  it also changes the collection that the MainViewModel uses.
        // For a description of an ObservableCollection see the MainViewModel class.
        private ObservableCollection<ShapeAttribute> _methods;
        // The 'shape' field holds a new shape, that is added to the 'shapes' collection, 
        //  and if undone, it is removed from the collection.
        private ShapeAttribute _method;

        // Constructor for saving and changing the current state of the diagram 
        //  (or at least the relevant parts).
        public AddMethodCommand(ObservableCollection<ShapeAttribute> methods, ShapeAttribute method)
        {
            _methods = methods;
            _method = method;
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            _methods.Add(_method);
            if (_method.Shape.Methods.Count > 1)
                _method.Shape.Height += 22;
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            _methods.Remove(_method);
            if (_method.Shape.Methods.Count > 0)
                _method.Shape.Height -= 22;
        }
    }


}
