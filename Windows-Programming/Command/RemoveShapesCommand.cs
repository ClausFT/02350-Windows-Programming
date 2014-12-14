using Windows_Programming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Windows_Programming.Command
{
    // Undo/Redo command for removing Shapes.
    // It also removes the Lines connected to the Shapes.
    public class RemoveShapesCommand : IUndoRedoCommand
    {
        private ObservableCollection<Shape> shapes;
        private ObservableCollection<Line> lines;
        private List<Shape> shapesToRemove;
        private List<Line> linesToRemove;

        public RemoveShapesCommand(ObservableCollection<Shape> _shapes, ObservableCollection<Line> _lines, List<Shape> _shapesToRemove)
        {
            shapes = _shapes;
            lines = _lines;
            shapesToRemove = _shapesToRemove;
            linesToRemove = _lines.Where(x => _shapesToRemove.Any(y => y.Number == x.From.Number || y.Number == x.To.Number)).ToList();
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            linesToRemove.ForEach(x => lines.Remove(x));
            shapesToRemove.ForEach(x => shapes.Remove(x));
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            shapesToRemove.ForEach(x => shapes.Add(x));
            linesToRemove.ForEach(x => lines.Add(x));
        }
    }
}
