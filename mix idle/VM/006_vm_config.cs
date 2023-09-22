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
        public bool vm_fullscreen = false;
        public double2 vm_fullscreen_time = 0;

        public bool vm_close_Ischecked = false;
        public int vm_close_time_index = 0;
        public int vm_lock_time_index = 0;

        public void config_save()
        {
            vm_close_Ischecked = (bool)((CheckBox)vm_elems["s0_close_c1"]).IsChecked;
            vm_close_time_index = ((ComboBox)vm_elems["s0_close_time"]).SelectedIndex;
            vm.auto_lock = (bool)((CheckBox)vm_elems["s0_lock_c1"]).IsChecked;
            vm_lock_time_index = ((ComboBox)vm_elems["s0_lock_time"]).SelectedIndex;
        }

        public void config_load()
        {
            ((CheckBox)vm_elems["s0_close_c1"]).IsChecked = vm_close_Ischecked;
            ((ComboBox)vm_elems["s0_close_time"]).SelectedIndex = vm_close_time_index;
            ((CheckBox)vm_elems["s0_lock_c1"]).IsChecked = vm.auto_lock;
            ((ComboBox)vm_elems["s0_lock_time"]).SelectedIndex = vm_lock_time_index;
        }
    }
}
