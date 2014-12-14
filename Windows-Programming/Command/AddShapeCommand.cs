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
        private ObservableCollection<Shape> shapes;
        private Shape shape;

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
}
