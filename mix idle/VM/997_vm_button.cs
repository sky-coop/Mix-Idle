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
using System.Text.RegularExpressions;

namespace mix_idle
{
    public partial class gamestats
    {
        public void button_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string name = b.Name;
            button_effect(name, b);
        }

        public void button_effect(string name, Button button = null)
        {
            vm.locking_time = 0;
            if (name == "vm_close_confirm")
            {
                vm.closing = false;
                vm.close(true);
            }
            if (name == "vm_close_cancel")
            {
                vm.closing = false;
            }

            if (name.Contains("vm_vstore_item") && name.Contains("detail_exit"))
            {
                Grid g = (Grid)vm_elems[(string)button.Tag];
                g.Visibility = Visibility.Hidden;
            }

            if (name.Contains("vm_vstore_item") && name.Contains("detail_install"))
            #region
            {
                //TODO: if 判断购买。
                if (true)
                {
                    VM_APP a = (VM_APP)button.Tag;
                    vm.install(a);
                    button.Content = "已安装";
                    button.IsEnabled = false;

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
                            if (b[i, j])
                            {
                                continue;
                            }
                            dt_arr.append(a.name + " " + i + " " + j + "\n");
                            goto end;
                        }
                    }
                end:
                    vm.dt_changed = true;
                }
            }
            #endregion


            if (name == "vm_g1_page_back")
            {
                g1_current_layer.curr_tab.page_now = Math.Max(1,
                    g1_current_layer.curr_tab.page_now - 1);
            }
            if (name == "vm_g1_page_forward")
            {
                g1_current_layer.curr_tab.page_now = Math.Min(
                    g1_current_layer.curr_tab.page_max,
                    g1_current_layer.curr_tab.page_now + 1);
            }

            if (name.Contains("_slot_b"))
            {
                string x = "";
                ARGB c = null;
                g1_level.type t = g1_level.type.normal;
                #region 自然树
                if (name.Contains("自然树_slot_b1"))
                {
                    x = "简单模式: 轻木";
                    c = A(0, 255, 0);
                    t = g1_level.type.easy;
                }
                if (name.Contains("自然树_slot_b2"))
                {
                    x = "普通模式：苹果树";
                    c = A(255, 255, 0);
                    t = g1_level.type.normal;
                }
                if (name.Contains("自然树_slot_b3"))
                {
                    x = "困难模式：柏树";
                    c = A(255, 150, 0);
                    t = g1_level.type.hard;
                }
                #endregion 自然树
                #region 水晶树
                if (name.Contains("水晶树_slot_b1"))
                {
                    x = "简单模式: 纯色水晶";
                    c = A(0, 255, 0);
                    t = g1_level.type.easy;
                }
                if (name.Contains("水晶树_slot_b2"))
                {
                    x = "普通模式：混色水晶";
                    c = A(255, 255, 0);
                    t = g1_level.type.normal;
                }
                if (name.Contains("水晶树_slot_b3"))
                {
                    x = "困难模式：变色水晶";
                    c = A(255, 150, 0);
                    t = g1_level.type.hard;
                }
                #endregion 水晶树
                #region 合成树
                if (name.Contains("合成树_slot_b1"))
                {
                    x = "简单模式: 四色面板";
                    c = A(0, 255, 0);
                    t = g1_level.type.easy;
                }
                if (name.Contains("合成树_slot_b2"))
                {
                    x = "普通模式：五色面板";
                    c = A(255, 255, 0);
                    t = g1_level.type.normal;
                }
                if (name.Contains("合成树_slot_b3"))
                {
                    x = "困难模式：六色面板";
                    c = A(255, 150, 0);
                    t = g1_level.type.hard;
                }
                #endregion 合成树
                string L_name = Regex.Split(name, "_slot_b")[0];
                string base_name = Regex.Split(L_name, "___")[0];
                string slot_name = Regex.Split(L_name, "___")[1];

                Grid G = button.Parent as Grid;

                Grid I = (Grid)vm_elems[base_name + "_information"];
                if(I != null)
                {
                    grid g = G.Tag as grid;
                    g.v = Visibility.Hidden;

                    grid i = I.Tag as grid;
                    i.v = Visibility.Visible;
                }

                string module_base_name = base_name + "___" + slot_name + "_slot_";
                TextBlock title = (TextBlock)vm_elems[module_base_name + "title"];
                if(title != null)
                {
                    textblock title_ = title.Tag as textblock;
                    title_.content = x;
                    title_.color = c;
                }

                g1_levels[slot_name].difficulty = t;

                if(slot_name == "水晶树")
                {
                    resource crystal = g1_res["水晶块"];
                    crystal.set_value(0);
                    if (g1_ups["世界_折射"].level >= 1)
                    {
                        crystal.set_value(double2.Max(crystal.get_value(),
                            g1_cal_refraction_effect()));
                    }
                }
            }
            if (name.Contains("_slot_enter"))
            {
                string r_name = Regex.Split(name, "_slot_enter")[0];
                string level_name = Regex.Split(r_name, "___")[1];

                g1_level_draw(level_name);
            }
            if (name.Contains("_slot_finish"))
            {
                string r_name = Regex.Split(name, "_slot_finish")[0];
                string level_name = Regex.Split(r_name, "___")[1];
                string base_name = Regex.Split(name, "___")[0];

                g1_level level = g1_levels[level_name];
                level.end();

                Grid I = button.Parent as Grid;

                Grid G = (Grid)vm_elems[base_name + "_choose"];
                if(G != null)
                {
                    grid i = I.Tag as grid;
                    i.v = Visibility.Hidden;

                    grid g = G.Tag as grid;
                    g.v = Visibility.Visible;
                }

                if (level.name == "自然树")
                {
                    resource r = find_resource("生命力");
                    r.get_from_pool("自然树", double2.max);
                    resource main = find_resource("自然点数");
                    find_resource("自然力量").add_value(main.get_value() *
                        g1_cal_craft_effect(), true);
                    main.set_value(0);
                }
                if (level.name == "水晶树")
                {
                    resource r = find_resource("生命力");
                    r.get_from_pool("水晶树", double2.max);
                    resource main = find_resource("水晶块");
                    find_resource("水晶球").add_value(main.get_value() *
                        g1_cal_craft_effect(), true);
                    main.set_value(0);

                    main = find_resource("生命转化");
                    find_resource("生命效果").add_value(main.get_value(), true);
                    main.set_value(0);
                }
                if (level.name == "合成树")
                {
                    resource r = find_resource("生命力");
                    r.get_from_pool("合成树", double2.max);
                    resource main = find_resource("合成分数");
                    find_resource("合成熟练度").add_value(main.get_value() *
                        g1_cal_craft_effect(), true);
                    main.set_value(0);
                }
            }
        }
    }
}
