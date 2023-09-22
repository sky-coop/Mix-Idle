using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace mix_idle
{
    public partial class gamestats
    {
        public double2 treasure_3_1(double2 n, double2 n2)
        {
            return double2.Pow(double2.Log10(n + 10),
                               0.055 * double2.Pow(double2.Log10(n2 + 1), 0.996));
        }
        public double2 treasure_3_2(double2 n, double2 n2)
        {
            return double2.Pow(double2.Log10(n + 10),
                               0.018 * double2.Pow(double2.Log10(n2 + 1), 0.996));
        }
    }
}
