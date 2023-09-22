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
        [Serializable]
        public class VM_APPS_VER
        {
            public Dictionary<string, VM_APP_VER> apps_ver = new Dictionary<string, VM_APP_VER>();
        }
        VM_APPS_VER vm_ver_ctrl = new VM_APPS_VER();

        [Serializable]
        public class VM_APP_VER
        {
            public string name;
            public List<Tuple<int, int>> app_ver = new List<Tuple<int, int>>();

            public VM_APP_VER(string n)
            {
                name = n;
            }
        }

        //切换版本
        public void vm_vos_update(int v, int sv)
        {

        }

        //切换版本
        public void vm_sos_update(int v, int sv)
        {

        }
    }
}
