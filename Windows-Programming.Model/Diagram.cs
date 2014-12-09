using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Windows_Programming.Model
{

    [Serializable()]
    public class Diagram
    {
        public List<Shape> shapes { get; set; }

        public List<Line> lines { get; set; }


    }
}
