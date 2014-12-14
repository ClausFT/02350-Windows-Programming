using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Windows_Programming.Model
{
    public class ShapeAttribute
    {
        public string Value { get; set; }
        [XmlIgnore]
        public Shape Shape { get; set; }
    }
}