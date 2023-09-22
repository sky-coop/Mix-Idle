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
        private void key_check()
        {
            VM_APP g1 = vm.search_app("世界树");
            if (g1 != null && g1.showing)
            {
                //g1_view_point
                Rectangle view = (Rectangle)vm_elems["vm_g1_radar_view"];
                Grid radar = (Grid)vm_elems["vm_g1_radar_grid"];
                Grid map = (Grid)vm_elems["vm_g1_map_grid"];
                key_move(view, 1, 1, 
                    0, radar.Width - view.Width,
                    0, radar.Height - view.Height);
                //g1_view_syn();
            }

            VM_APP g2 = vm.search_app("聚能光珠");
            if (g2 != null && g2.showing)
            #region
            {
                if (vm_elems["g2_classic_menu"].Visibility == Visibility.Visible &&
                    vm_elems["g2_game_container"].Visibility == Visibility.Hidden)
                {
                    FrameworkElement f = vm_elems["g2_classic_level_map"];
                    FrameworkElement f2 = vm_elems["g2_classic_level_map_2"];
                    FrameworkElement c = vm_elems["g2_classic_level_map_container"];

                    if (key_left())
                    {
                        f.Margin = new Thickness(
                            Math.Min(0, f.Margin.Left + 0.003 * f.Width), f.Margin.Top, 0, 0);
                    }
                    if (key_right())
                    {
                        f.Margin = new Thickness(
                            Math.Max(c.Width - f.Width, f.Margin.Left - 0.003 * f.Width), f.Margin.Top, 0, 0);
                    }
                    if (key_up())
                    {
                        f.Margin = new Thickness(f.Margin.Left,
                            Math.Min(0, f.Margin.Top + 0.003 * f.Height), 0, 0);
                    }
                    if (key_down())
                    {
                        f.Margin = new Thickness(f.Margin.Left,
                            Math.Max(c.Height - f.Height, f.Margin.Top - 0.003 * f.Height), 0, 0);
                    }
                    f2.Margin = f.Margin;
                }
            }
            #endregion
        }

        public bool key_up()
        {
            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
            {
                return true;
            }
            return false;
        }

        public bool key_left()
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
            {
                return true;
            }
            return false;
        }

        public bool key_down()
        {
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S))
            {
                return true;
            }
            return false;
        }

        public bool key_right()
        {
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
            {
                return true;
            }
            return false;
        }

        //Left Top
        public void margin_move(FrameworkElement f, double dx, double dy,
            double x_min, double x_max, double y_min, double y_max)
        {
            double left = f.Margin.Left + dx;
            double top = f.Margin.Top + dy;

            if (left < x_min)
            {
                left = x_min;
            }
            if (left > x_max)
            {
                left = x_max;
            }
            if (top < y_min)
            {
                top = y_min;
            }
            if (top > y_max)
            {
                top = y_max;
            }
            f.Margin = new Thickness(left, top, 0, 0);
        }

        public void key_move(FrameworkElement f, double x_speed, double y_speed,
            double x_min, double x_max, double y_min, double y_max)
        {
            if (key_left())
            {
                margin_move(f, -x_speed, 0, x_min, x_max, y_min, y_max);
            }
            if (key_right())
            {
                margin_move(f, x_speed, 0, x_min, x_max, y_min, y_max);
            }
            if (key_up())
            {
                margin_move(f, 0, -y_speed, x_min, x_max, y_min, y_max);
            }
            if (key_down())
            {
                margin_move(f, 0, y_speed, x_min, x_max, y_min, y_max);
            }
        }
    }
}
