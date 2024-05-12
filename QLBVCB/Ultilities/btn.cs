using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLBVCB.Ultilities
{
    public class btn : RadioButton
    {
        static btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(btn), new FrameworkPropertyMetadata(typeof(btn)));
        }
    }
}
