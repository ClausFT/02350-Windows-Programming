﻿using Windows_Programming.Model;
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

    // Undo/Redo command for adding a Shape.
    public class AddAttributeCommand : IUndoRedoCommand //TODO: Make AddMethodCommand
    {
        // Fields.
        // The 'shapes' field holds the current collection of shapes, 
        //  and the reference points to the same collection as the one the MainViewModel point to, 
        //  therefore when this collection is changed in a object of this class, 
        //  it also changes the collection that the MainViewModel uses.
        // For a description of an ObservableCollection see the MainViewModel class.
        private ObservableCollection<string> _attributes;
        // The 'shape' field holds a new shape, that is added to the 'shapes' collection, 
        //  and if undone, it is removed from the collection.
        private string _attribute;

        // Constructor for saving and changing the current state of the diagram 
        //  (or at least the relevant parts).
        public AddAttributeCommand(ObservableCollection<string> attributes, string attribute)
        {
            _attributes = attributes;
            _attribute = attribute;
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            _attributes.Add(_attribute);
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            _attributes.Remove(_attribute);
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
        private ObservableCollection<string> _methods;
        // The 'shape' field holds a new shape, that is added to the 'shapes' collection, 
        //  and if undone, it is removed from the collection.
        private string _method;

        // Constructor for saving and changing the current state of the diagram 
        //  (or at least the relevant parts).
        public AddMethodCommand(ObservableCollection<string> methods, string method)
        {
            _methods = methods;
            _method = method;
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            _methods.Add(_method);
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            _methods.Remove(_method);
        }
    }


}
