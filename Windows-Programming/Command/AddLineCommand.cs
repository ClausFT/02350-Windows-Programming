﻿using System;
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
    // Undo/Redo command for adding a Line.
    public class AddLineCommand : IUndoRedoCommand
    {
        // Fields.
        // The 'lines' field holds the current collection of lines, 
        //  and the reference points to the same collection as the one the MainViewModel point to, 
        //  it also changes the collection that the MainViewModel uses.
        // For a description of an ObservableCollection see the MainViewModel class.
        private ObservableCollection<Line> lines;
        // The 'line' field holds a new line, that is added to the 'lines' collection, 
        //  and if undone, it is removed from the collection.
        private Line line;

        // Constructor for saving and changing the current state of the diagram 
        //  (or at least the relevant parts).
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
