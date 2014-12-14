using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Windows_Programming.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows_Programming.ViewModel;

namespace Windows_Programming.Command
{
    public class AddLineCommand : IUndoRedoCommand
    {
        private ObservableCollection<Line> lines;
        private Line line;

        public AddLineCommand(ObservableCollection<Line> _lines, Line _line)
        {
            lines = _lines;
            line = _line;
        }

        // Methods.
        // This method is for doing and redoing the command.
        public void Execute()
        {
            lines.Add(line);
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            lines.Remove(line);
        }
    }
}
