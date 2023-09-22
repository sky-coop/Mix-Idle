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
        public void g1_level_draw(string name)
        {
            string str;
            rainbow_text rt;
            g1_milestone ms;
            g1_upgrade u;
            g1_resource r;
            g1_level level;
            g1_layer layer;
            g1_tab tab;
            g1_save_slot slot;
            g1_drawable draw;
            textblock tb;
            game_grid_element[,] game_Grid_Elements;
            game_grid game_Grid;
            List<List<Tuple<string, double2>>> ct;
            List<Tuple<string, double2>> costs;

            Grid main = (Grid)vm_elems["vm_g1_main_grid"];
            Grid scene_base = (Grid)vm_elems["vm_g1_scene_grid"];
            scene_base.Children.Clear();

            Grid map = g1_getMap();
            map.Children.Clear();
            map.Children.Add(scene_base);

            Grid ctrl = g1_getCTRL();
            Grid layer_grid = (Grid)vm_elems["vm_g1_layer_grid"];
            layer_grid.Children.Clear();


            g1_mode = g1_show_mode.normal;

            Grid scene = new Grid
            {
                Name = "vm_g1_" + name + "_scene_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            scene_base.Children.Add(scene);
            vm_assign(scene);

            Grid radar_main = (Grid)vm_elems["vm_g1_radar_main"];
            Grid radar_scenery = (Grid)vm_elems["vm_g1_radar_scenery"];
            radar_main.Children.Clear();
            radar_scenery.Children.Clear();

            StackPanel top = (StackPanel)vm_elems["vm_g1_top_panel"];
            top.Children.Clear();

            

            g1_level p = g1_levels[name];
            g1_current_level = p;
            bool success = g1_level_start(name);

            level = g1_current_level;

            main.Background = p.color.toBrush();
            map.Background = p.color.toBrush();
            g1_current_layer = null;

            if (name == "世界")
            {
                g1_map_redraw(2, 2);
            }
            if (name == "世界树")
            {
                g1_level_draw_世界树(success);
            }
            if (name == "自然树")
            {
                g1_level_draw_自然树(success);
            }
            if (name == "水晶树")
            {
                g1_level_draw_水晶树(success);
            }
            if (name == "合成树")
            {
                g1_level_draw_合成树(success);
            }

            #region 读显示数据
            foreach (g1_scenery scenery in p.sceneries)
            {
                scenery.changed = true;
            }

            Rectangle view = (Rectangle)vm_elems["vm_g1_radar_view"];
            view.Margin = new Thickness(level.view_point.X, level.view_point.Y, 0, 0);

            g1_layer temp = g1_current_level.watching_layer;
            if (temp != null)
            {
                if (temp.unlocked)
                {
                    g1_mode = g1_show_mode.right;
                }
                else
                {
                    g1_mode = g1_show_mode.normal;
                }
                g1_show();
                if (temp.unlocked)
                {
                    game_key_name("vm_g1_map_grid_layer__" + temp.name);
                }
                else
                {
                    game_key_name("vm_g1_right_ret");
                }
                g1_show();
                g1_upgrade_check();
            }
            #endregion 读显示数据
        }
    }
}
