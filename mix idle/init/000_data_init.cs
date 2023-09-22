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
        public void data_init()
        {
            time_boost_max_old = -1;

            none = new resource(0, 0, "无", getSCB(Color.FromRgb(0, 0, 0)));
            one = new resource(0, 1, "无", getSCB(Color.FromRgb(0, 0, 0)));

            float_messages = new List<float_message>();

            unlocks.fight_unlock[0] = true;
        }
    }
}
