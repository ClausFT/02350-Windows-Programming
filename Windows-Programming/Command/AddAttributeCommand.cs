using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_Programming.Model;

namespace Windows_Programming.Command
{
    public class AddAttributeCommand : IUndoRedoCommand
    {
        private ObservableCollection<ShapeAttribute> _attributes;
        private ShapeAttribute _attribute;

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
            if (_attribute.Shape.Properties.Count > 1) //Adjust height
                _attribute.Shape.Height += 22;
        }

        // This method is for undoing the command.
        public void UnExecute()
        {
            _attributes.Remove(_attribute);
            if (_attribute.Shape.Properties.Count > 0)
                _attribute.Shape.Height -= 22;

        }   
    }
}
