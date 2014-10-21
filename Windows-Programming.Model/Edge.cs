using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Programming.Model
{
    class Edge : NotifyBase
    {
        // Properties.
        private Node endA;
        private Node endB;
        public Node EndA { get { return endA; } set { endA = value; NotifyPropertyChanged("EndA"); } }
        public Node EndB { get { return endB; } set { endB = value; NotifyPropertyChanged("EndB"); } }
    }
}
