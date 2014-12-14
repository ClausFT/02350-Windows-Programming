using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_Programming.Model;

namespace Windows_Programming.Command
{
    public class AddMethodCommand : IUndoRedoCommand
    {
        private ObservableCollection<ShapeAttribute> _methods;
        private ShapeAttribute _method;

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
