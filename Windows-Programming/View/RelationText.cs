using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Windows_Programming.Model.Enums;

namespace Windows_Programming.View
{
    class RelationText : System.Windows.Controls.TextBlock
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof (string),typeof (RelationText));
        public static readonly DependencyProperty MidXProperty = DependencyProperty.Register("MidX", typeof(double), typeof(RelationText));
        public static readonly DependencyProperty MidYProperty = DependencyProperty.Register("MidY", typeof(double), typeof(RelationText));

  
        [TypeConverter(typeof(string))]
        public string Label
        {
            get { return (string)base.GetValue(LabelProperty); }
            set { base.SetValue(LabelProperty, value); }
        }

        [TypeConverter(typeof(double))]
        public double MidX
        {
            get { return (double)base.GetValue(MidXProperty); }
            set { base.SetValue(MidXProperty, value); }
        }

        [TypeConverter(typeof(double))]
        public double MidY
        {
            get { return (double)base.GetValue(MidYProperty); }
            set { base.SetValue(MidYProperty, value);
            }
        }
    }
}