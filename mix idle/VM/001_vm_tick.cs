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
        public void vm_desktop_tick(double2 t)
        #region
        {
            if (vm.dt_changed)
            {
                vm_desktop_change();
                vm.dt_changed = false;
            }

            TextBlock time_t = (TextBlock)vm_elems["vm_os_tool_time_text"];
            time_t.Text = time_transfer(s_tickers["vm_desktop"].last);
        }

        public void vm_desktop_change()
        {
            VM_FILE dt_def = vm.search_file_by_path("Root/System/Desktop/Definition.sys");
            string[] parts = dt_def.read().Split(' ');
            int dt_h = Convert.ToInt32(parts[0]);
            int dt_w = Convert.ToInt32(parts[1]);

            VM_FILE dt_arr = vm.search_file_by_path("Root/System/Desktop/Arrangement.sys");
            List<string> arrs = vm_line_split(dt_arr.read());
            bool[,] b = new bool[dt_h, dt_w];
            string[,] n = new string[dt_h, dt_w];

            foreach (string s in arrs)
            {
                if (s != "")
                {
                    string[] ps = s.Split(' ');
                    int x = Convert.ToInt32(ps[1]);
                    int y = Convert.ToInt32(ps[2]);
                    b[x, y] = true;
                    n[x, y] = ps[0];
                }
            }

            for (int i = 0; i < dt_h; i++)
            {
                for (int j = 0; j < dt_w; j++)
                {
                    Grid g = (Grid)vm_elems["vm_os_app_grid_" + i + "_" + j];
                    if (!b[i, j])
                    {
                        g.Visibility = Visibility.Hidden;
                        continue;
                    }
                    g.Visibility = Visibility.Visible;

                    Image image = (Image)vm_elems["vm_os_app_icon_" + i + "_" + j];
                    image.Source = vm_icon(n[i, j]);
                    image.Tag = n[i, j];

                    TextBlock text = (TextBlock)vm_elems["vm_os_app_name_" + i + "_" + j];
                    text.Text = n[i, j];
                }
            }
        }
        #endregion


        public void vm_vos_tick(double2 t)
        {

        }

        public void vm_sos_tick(double2 t)
        {

        }

        public void vm_vconfig_tick(double2 t)
        {
            Rectangle close_mask = (Rectangle)vm_elems["s0_close_time_mask"];
            if ((bool)((CheckBox)vm_elems["s0_close_c1"]).IsChecked)
            {
                close_mask.Visibility = Visibility.Hidden;
            }
            else
            {
                close_mask.Visibility = Visibility.Visible;
            }

            Rectangle lock_mask = (Rectangle)vm_elems["s0_lock_time_mask"];
            if ((bool)((CheckBox)vm_elems["s0_lock_c1"]).IsChecked)
            {
                lock_mask.Visibility = Visibility.Hidden;
            }
            else
            {
                lock_mask.Visibility = Visibility.Visible;
            }

            config_save();
        }

        public void vm_vstore_tick(double2 t)
        {

        }

        public void vm_vfs_tick(double2 t)
        {

        }

        public void vm_valarm_tick(double2 t)
        {

        }

        public void vm_vtask_tick(double2 t)
        {

        }

        public void vm_vupdate_tick(double2 t)
        {

        }

        public void vm_g1_tick(double2 t)
        {
            g1_tick(t);
        }

        public void vm_g2_tick(double2 t)
        {
            //时间过长可能需要估计
            g2_tick_counter += double2.Min(t, 5);
            for (; g2_tick_counter > 0; g2_tick_counter -= 0.01)
            {
                g2_tick();
            }
        }

        public void vm_se_tick(double2 t)
        {

        }

        public void vm_lock_tick(double2 t)
        {

        }

        public void vm_mail_tick(double2 t)
        {

        }

        public void vm_g3_tick(double2 t)
        {

        }
    }
}
