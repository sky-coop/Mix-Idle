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
        private void game_rkey_name(string n)
        {
            vm.locking_time = 0;
            game_rkey((Rectangle)vm_elems[n]);
        }

        private void game_rkey(Rectangle r)
        {
            vm.locking_time = 0;
            g2_area a = g2_current;
            string name = r.Name;
            Rectangle background = null;
            if (vm_elems.ContainsKey(name + "_bg"))
            {
                background = (Rectangle)vm_elems[name + "_bg"];
            }
            else
            {
                background = r;
            }
            TextBlock text = null;
            string[] strs = name.Split('_');

            //test("rkey " + name);

            if (vm_elems.ContainsKey(name + "_text"))
            {
                text = (TextBlock)vm_elems[name + "_text"];
            }


            #region level 合成

            if (name.Contains("_m3button__"))
            {
                string r_name = Regex.Split(name, "_m3button__")[1];
                string[] parts = r_name.Split('_');
                int i = Convert.ToInt32(parts[0]);
                int j = Convert.ToInt32(parts[1]);
                game_grid m3 = game_grids["m3"];
                if (m3.select_chain.Count == 0)
                {
                    m3_explode(i, j);
                }
                else
                {
                    game_grid_element last = m3.select_chain[0];
                    game_grid_element curr = m3.elems[i, j];

                    if (last.special >= 10)
                    {
                        last.draw.s_fill.color.a = 160;
                        last.draw.cover_color = A(0, 0, 0, 0);
                        m3.select_chain.Clear();
                        foreach (game_grid_element e in m3.elems)
                        {
                            e.selectable = true;
                        }

                        int temp = curr.type;
                        curr.type = last.type;
                        last.type = temp;
                        temp = curr.special;
                        curr.special = last.special;
                        last.special = temp;

                        m3_check(i, j);
                        m3_check(last.r, last.c);
                        m3_explode(i, j);

                    }
                }
            }
            #endregion level 合成

            //g2_game_option_create_creater.Name = "g2_game_option_create_creater_" + si + "_" + sj;
            if (name.Contains("g2_game_option_create_creater"))
            {
                //g2_game_create_container.Name = "g2_game_create_container";
                Grid gcc = (Grid)vm_elems["g2_game_create_container"];
                gcc.Visibility = Visibility.Visible;

                string si = strs[5];
                string sj = strs[6];
                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);
                int n = i + j * 4;
                a.edit.right_selected_index = n;

                game_key_name("g2_game_create_btn_1");
                if (a.edit.creater[n] != null)
                {
                    foreach (Tuple<int, int, int, int> t in a.edit.creater[n].edit_chain)
                    {
                        string tname = "g2_game_creater_" +
                            t.Item1 + "_" +
                            t.Item2 + "_" +
                            t.Item3 + "_" +
                            t.Item4;
                        game_key_name(tname);
                    }
                    a.edit.result.copy_attr_by(a.edit.creater[n]);
                }
            }
            if (name.Contains("g2_game_item_edit"))
            {
                Grid gcc = (Grid)vm_elems["g2_game_create_container"];
                gcc.Visibility = Visibility.Visible;

                string si = strs[4];
                string sj = strs[5];
                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);
                int n = i + j * 2;
                a.edit.item_right_selected_index = n;

                game_key_name("g2_game_create_btn_1");
                if (a.items.ContainsKey(n))
                {
                    if (a.items[n] != null)
                    {
                        foreach (Tuple<int, int, int, int> t in a.items[n].c.edit_chain)
                        {
                            string tname = "g2_game_creater_" +
                                t.Item1 + "_" +
                                t.Item2 + "_" +
                                t.Item3 + "_" +
                                t.Item4;
                            game_key_name(tname);
                        }
                        a.edit.result.copy_attr_by(a.items[n].c);
                    }
                }

            }

        }

        private void game_key_name(string n)
        {
            vm.locking_time = 0;
            if (n.Contains("fake"))
            {
                Rectangle fake = new Rectangle();
                fake.Name = n;
                game_key(fake);
            }
            else
            {
                game_key((Rectangle)vm_elems[n]);
            }
        }

        private void game_key(Rectangle r)
        {
            vm.locking_time = 0;
            g2_area a = g2_current;
            string name = r.Name;
            Rectangle background = null;
            if (vm_elems.ContainsKey(name + "_bg"))
            {
                background = (Rectangle)vm_elems[name + "_bg"];
            }
            else
            {
                background = r;
            }
            TextBlock text = null;
            string[] strs = name.Split('_');

            if (vm_elems.ContainsKey(name + "_text"))
            {
                text = (TextBlock)vm_elems[name + "_text"];
            }

            #region vm_g1

            if (name.Contains("g1") && name.Contains("_layer__"))
            {
                if (g1_current_layer != null)
                {
                    vm_elems["vm_g1_layer_" + g1_current_layer.name + "_grid"].Visibility 
                        = Visibility.Hidden;
                }

                string layer_name = Regex.Split(name, "__")[1];
                g1_mode = g1_show_mode.right;
                g1_current_layer = g1_layers[layer_name];
                g1_current_level.watching_layer = g1_current_layer;
                vm_elems["vm_g1_layer_" + g1_current_layer.name + "_grid"].Visibility
                    = Visibility.Visible;
            }
            if (name == "vm_g1_right_ret")
            {
                if (g1_current_layer != null)
                {
                    vm_elems["vm_g1_layer_" + g1_current_layer.name + "_grid"].Visibility
                        = Visibility.Hidden;
                }

                Grid right = (Grid)vm_elems["vm_g1_right_grid"];
                g1_mode = g1_show_mode.normal;
                right.Visibility = Visibility.Hidden;
                g1_current_layer = null;
                g1_current_level.watching_layer = g1_current_layer;
            }
            if (name == "vm_g1_center_ret")
            {
                g1_level last = g1_current_level.prevs[0];
                g1_level_draw(last.name);
            }

            //升级
            if (name.Contains("_g1_") && name.Contains("upgrade__"))
            {
                string u_name = Regex.Split(name, "__")[1];
                g1_layer layer = g1_current_layer;
                g1_upgrade u = layer.find_upgrade(u_name);
                if (u.no_cost)
                {
                    buy_upgrade_no_cost(layer.find_upgrade(u_name), 1);
                }
                else
                {
                    foreach(Tuple<string, double2> tuple in u.cost_table[u.level])
                    {
                        if(tuple.Item1 == "迷宫元素")
                        {
                            find_resource("已用迷宫元素").add_value(
                                tuple.Item2, true);
                        }
                    }
                    buy_upgrade(layer.find_upgrade(u_name), 1);
                }
            }
            //升级（研究）
            if (name.Contains("_g1_") && name.Contains("research__"))
            {
                string u_name = Regex.Split(name, "__")[2];
                g1_layer layer = g1_current_layer;
                g1_research u = layer.find_upgrade(u_name) as g1_research;
                /*
                if (u.no_cost)
                {
                    buy_upgrade_no_cost(layer.find_upgrade(u_name), 1);
                }
                else
                {
                    buy_upgrade(layer.find_upgrade(u_name), 1);
                }*/
                u.level = 0;
                u.put(u.will());
            }
            if (name.Contains("_tabs_grid__"))
            {
                string r_name = Regex.Split(name, "__")[1];
                g1_tab t = g1_current_layer.tabs[r_name];
                group_process(g1_current_layer.group, r, false,
                    t.c_active.color.toBrush(),
                    t.tab.s_fill.color.toBrush(),
                    t.tab.s_content.color.toBrush());
                g1_current_layer.curr_tab = t;
            }

            #region level 合成

            if (name.Contains("_m3button__"))
            {
                string r_name = Regex.Split(name, "_m3button__")[1];
                string[] parts = r_name.Split('_');
                int i = Convert.ToInt32(parts[0]);
                int j = Convert.ToInt32(parts[1]);
                game_grid m3 = game_grids["m3"];
                if(m3.select_chain.Count == 0)
                {
                    m3.select_chain.Add(m3.elems[i, j]);
                    foreach(game_grid_element e in m3.elems)
                    {
                        e.selectable = false;
                    }
                    m3.elems[i, j].selectable = true;
                    m3.elems[i, j].draw.s_fill.color.a = 255;
                    m3.elems[i, j].draw.cover_color = A(100, 0, 150, 150);

                    int range = m3.elems[i, j].special % 10;
                    if (m3.elems[i, j].special >= 10)
                    {
                        range++;
                    }
                    range = Math.Max(1, range);
                    foreach (game_grid_element s in m3.surround(i, j, range))
                    {
                        s.selectable = true;
                    }
                }
                else
                {
                    game_grid_element last = m3.select_chain[0];
                    last.draw.s_fill.color.a = 160;
                    last.draw.cover_color = A(0, 0, 0, 0);
                    m3.select_chain.Clear();
                    foreach (game_grid_element e in m3.elems)
                    {
                        e.selectable = true;
                    }
                    if (last.r == i && last.c == j)
                    {
                    }
                    else
                    {
                        game_grid_element curr = m3.elems[i, j];

                        bool s = false;
                        if (curr.special >= 10)
                        {
                            s = true;
                        }

                        int temp = curr.type;
                        curr.type = last.type;
                        last.type = temp;
                        temp = curr.special;
                        curr.special = last.special;
                        last.special = temp;

                        bool b = false;
                        if (m3_check(i, j))
                        {
                            b = true;
                        }
                        if (m3_check(last.r, last.c))
                        {
                            b = true;
                        }


                        if (!b)
                        {
                            temp = curr.type;
                            curr.type = last.type;
                            last.type = temp;
                            temp = curr.special;
                            curr.special = last.special;
                            last.special = temp;
                        }
                    }
                }
            }
            #endregion level 合成

            #endregion vm_g1




            if (name == "g2_mode_selecter_0")
            {
                vm_elems["g2_classic_menu"].Visibility = Visibility.Visible;

            }
            if (name == "g2_mode_selecter_3")
            {
                g2_level_create("editor");
                g2_start(levels["editor"]);

            }
            if (name.Contains("g2_classic_level"))
            {
                g2_level_create(name);
                g2_start(levels[name]);
            }
            if (name.Contains("g2_game_eoption"))
            #region
            {
                string sn = strs[3];
                int n = Convert.ToInt32(sn);
                for (int i = 0; i < 3; i++)
                {
                    Rectangle bg = (Rectangle)vm_elems["g2_game_eoption_" + i.ToString() + "_bg"];
                    if (i == n)
                    {
                        bg.Fill = getSCB(Color.FromRgb(200, 200, 75));
                    }
                    else
                    {
                        bg.Fill = getSCB(Color.FromRgb(150, 150, 150));
                    }
                }

                Grid goo = (Grid)vm_elems["g2_game_option_option_container"];
                Grid goc = (Grid)vm_elems["g2_game_option_create_container"];

                goo.Visibility = Visibility.Hidden;
                goc.Visibility = Visibility.Hidden;
                if (n == 0)
                {
                    goo.Visibility = Visibility.Visible;
                }
                else if (n == 1)
                {
                    goc.Visibility = Visibility.Visible;
                }
            }
            #endregion
            if (name == "g2_game_option_exit")
            {
                Grid gc = (Grid)vm_elems["g2_game_container"];
                gc.Visibility = Visibility.Hidden;
                g2_level_exit();
            }
            if (name == "g2_game_option_restart")
            {
                g2_level_restart();
            }
            if (name == "g2_game_option_undo")
            #region
            {
                a.grid_changed = true;
                a.item_changed = true;
                a.wall_changed = true;

                List<string> ms = a.movements;
                int no = 0;
                while (ms.Count > 0)
                {
                    no++;
                    string s = ms.Last();
                    string[] sparts = s.Split('@');
                    string[] codes = sparts[0].Split(' ');
                    if (codes[0] == "test")
                    {
                        break;
                    }

                    ms.RemoveAt(ms.Count - 1);
                    if (codes[0] == "create")
                    {
                        string[] parts = sparts[0].Split('&');
                        string[] paras = parts[2].Split('$');
                        string[] cells = parts[1].Split(' ');

                        string si = paras[0].Split(' ')[0];
                        string sj = paras[0].Split(' ')[1];
                        string sx = paras[1].Split(' ')[0];
                        string sy = paras[1].Split(' ')[1];

                        int p = 0;
                        for (int i = Convert.ToInt32(si); i < Convert.ToInt32(si) + Convert.ToInt32(sx); i++)
                        {
                            for (int j = Convert.ToInt32(sj); j < Convert.ToInt32(sj) + Convert.ToInt32(sy); j++)
                            {
                                a.area[i, j] = g2_editor_saver[Convert.ToInt32(cells[p])];
                                a.check_point_clear(i, j);
                                a.area[i, j].color_change = true;
                                p++;
                            }
                        }
                        break;
                    }
                    else if (codes[0] == "light")
                    {
                        string si = codes[1];
                        string sj = codes[2];
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);

                        a.area[i, j].entered = false;
                        a.area[i, j].color_change = true;
                    }
                    else if (codes[0] == "unlight")
                    {
                        string si = codes[1];
                        string sj = codes[2];
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);

                        a.area[i, j].entered = true;
                        a.area[i, j].color_change = true;
                    }
                    else if (codes[0] == "insert")
                    {
                        string sn = codes[1];
                        string si = codes[2];
                        string sj = codes[3];
                        int n = Convert.ToInt32(sn);
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);

                        if (a.items[n].i < 1000)
                        {
                            a.items[n].i++;
                        }

                        bool e = a.area[i, j].entered;
                        int size = a.area[i, j].all_size;
                        List<g2_cell.type> empty = new List<g2_cell.type>();
                        a.area[i, j] = new g2_cell(empty, 0, 0, size);
                        a.check_point_clear(i, j, size);
                        a.area[i, j].color_change = true;
                        a.area[i, j].entered = e;
                        if (a.area[i, j].is_type(g2_cell.type.empty))
                        {
                            Rectangle sr = (Rectangle)vm_elems["g2_game_object_grid_stroke_" + si + "_" + sj];
                            sr.Tag = (int)sr.Tag - 1;
                        }
                        break;
                    }
                    else if (codes[0] == "end")
                    {
                        a.condition.t[g2_complete_condition.type.normal] = false;
                    }
                    else if (codes[0].Contains("shoot"))
                    {
                        string sn = codes[1];
                        string si = codes[2];
                        string sj = codes[3];
                        int n = Convert.ToInt32(sn);
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);
                        bool view = false;

                        if (g2_balls.ContainsKey(n))
                        {
                            g2_balls[n].live = false;
                            view = true;
                        }
                        if (codes[0] == "shoot")
                        {
                            if (!view && no == 1)
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (g2_balls.ContainsKey(n))
                            {
                            }
                            else
                            {
                                no--;
                            }
                        }
                    }
                    else if (codes[0] == "key")
                    {
                        string sn = codes[1];
                        string si = codes[2];
                        string sj = codes[3];
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);

                        a.area[i, j].key = sn;
                        a.area[i, j].add_type(g2_cell.type.key);
                        g2_use_key("");
                    }
                    else if (codes[0] == "unlock")
                    {
                        string sn = codes[1];
                        string si = codes[2];
                        string sj = codes[3];
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);

                        a.area[i, j].door = sn;
                        a.area[i, j].add_type(g2_cell.type.door);
                        g2_use_key("");
                    }
                    else if (codes[0] == "ssize")
                    {
                        break;
                    }
                    else if (codes[0] == "lose")
                    {
                        string sn = codes[1];
                        string si = codes[2];
                        string sj = codes[3];
                        int n = Convert.ToInt32(sn);
                        int i = Convert.ToInt32(si);
                        int j = Convert.ToInt32(sj);

                        a.area[i, j] = g2_editor_saver[n];
                        a.check_point_clear(i, j);
                        a.area[i, j].color_change = true;
                    }
                    else if (codes[0] == "esize")
                    {
                        string sl = codes[1];
                        string st = codes[2];
                        string sr = codes[3];
                        string sb = codes[4];

                        int l0 = Convert.ToInt32(sl);
                        int t0 = Convert.ToInt32(st);
                        int r0 = Convert.ToInt32(sr);
                        int b0 = Convert.ToInt32(sb);

                        g2_size_change(a, -l0, -t0, -r0, -b0, false);
                    }
                }

                g2_tick();
                a.grid_changed = true;
                a.item_changed = true;
                a.wall_changed = true;
                g2_tick();
            }
            #endregion
            if (name == "g2_game_option_startexit")
            #region
            {
                if (a.editing)
                {
                    a.editing = false;
                    a.testing = true;
                    a.game_win = false;
                    a.add_movement("test");
                    game_key_name("g2_game_eoption_2");
                }
                else
                {
                    a.editing = true;
                    a.testing = false;
                    a.game_win = false;

                    bool test = false;
                    while (!test)
                    {
                        game_key_name("g2_game_option_undo");
                        List<string> ms = a.movements;
                        if (ms.Last() == "")
                        {
                            ms.RemoveAt(ms.Count - 1);
                        }
                        if (ms.Last().Split('@')[0] == "test")
                        {
                            test = true;
                            ms.RemoveAt(ms.Count - 1);
                        }
                    }

                    if (a.selected != -1)
                    {
                        int i = a.selected % 2;
                        int j = a.selected / 2;
                        game_key_name("g2_game_item_select_" + i + "_" + j);
                    }
                }
                if (a.condition.is_type(g2_complete_condition.type.normal))
                {
                    a.condition.t[g2_complete_condition.type.normal] = false;
                }
                a.grid_changed = true;
                a.item_changed = true;
                a.wall_changed = true;
            }
            #endregion
            //g2_game_option_create_creater.Name = "g2_game_option_create_creater_" + si + "_" + sj;
            if (name.Contains("g2_game_option_create_creater"))
            #region
            {
                string si = strs[5];
                string sj = strs[6];
                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);
                int n = i + j * 4;
                if (a.edit.left_selected_index != n)
                {
                    a.edit.left_selected_index = n;
                }
                else
                {
                    a.edit.left_selected_index = -1;
                }
            }
            #endregion
            if (name.Contains("g2_game_creater"))
            #region
            {
                string sbase = r.Name;
                string si = strs[3];
                string sj = strs[4];
                string sp = strs[5];
                string sq = strs[6];

                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);
                int p = Convert.ToInt32(sp);
                int q = Convert.ToInt32(sq);

                g2_edit_cell_inf eci = g2_ci[i, j, p, q];
                g2_cell c2 = eci.c;
                if (eci.cur != g2_edit_cell_inf.type.select)
                {
                    a.edit.result.edit_chain.Add(new Tuple<int, int, int, int>(i, j, p, q));
                    eci.cur = g2_edit_cell_inf.type.select;
                    foreach (g2_cell.type t in c2.cell_type)
                    {
                        a.edit.result.add_type(t);
                    }
                    if (c2.is_type(g2_cell.type.start))
                    {
                        a.edit.result.dir = c2.dir;
                    }
                    if (c2.is_type(g2_cell.type.reflecter))
                    {
                        a.edit.result.dir = c2.dir;
                    }
                    if (c2.is_type(g2_cell.type.door))
                    {
                        a.edit.result.door = c2.door;
                    }
                    if (c2.is_type(g2_cell.type.key))
                    {
                        a.edit.result.key = c2.key;
                    }
                    if (c2.is_type(g2_cell.type.in_out))
                    {
                        foreach (Point po in c2.in_dirs)
                        {
                            a.edit.result.in_dirs.Add(po);
                        }
                        foreach (Point po in c2.out_dirs)
                        {
                            a.edit.result.out_dirs.Add(po);
                        }
                    }
                    if (c2.is_type(g2_cell.type.portal))
                    {
                        a.edit.result.portal = c2.portal;
                    }
                }
                else
                {
                    a.edit.result.edit_chain.RemoveAt(a.edit.result.edit_chain.Count - 1);
                    eci.cur = g2_edit_cell_inf.type.normal;
                    foreach (g2_cell.type t in c2.cell_type)
                    {
                        a.edit.result.delete_type(t);
                    }
                    if (c2.is_type(g2_cell.type.door))
                    {
                        a.edit.result.door = "";
                    }
                    if (c2.is_type(g2_cell.type.key))
                    {
                        a.edit.result.key = "";
                    }
                    if (c2.is_type(g2_cell.type.in_out))
                    {
                        a.edit.result.in_dirs.Clear();
                        a.edit.result.out_dirs.Clear();
                    }
                    if (c2.is_type(g2_cell.type.portal))
                    {
                        a.edit.result.portal = "";
                    }
                }

                for (int k = 0; k < a.edit.result.edit_chain.Count; k++)
                {
                    Tuple<int, int, int, int> tp = a.edit.result.edit_chain[k];
                    g2_edit_cell_inf ep = g2_ci[tp.Item1, tp.Item2, tp.Item3, tp.Item4];
                    if (k == a.edit.result.edit_chain.Count - 1)
                    {
                        ep.old = g2_edit_cell_inf.type.normal;
                        ep.locked = false;
                    }
                    else
                    {
                        ep.locked = true;
                    }
                }
            }
            #endregion
            //"g2_game_option_create_sizechange_btn_" + si + "_" + sk;
            if (name.Contains("g2_game_option_create_sizechange_btn"))
            #region
            {
                string srow = strs[6];
                string scol = strs[7];
                int row = Convert.ToInt32(srow);
                int col = Convert.ToInt32(scol);

                int dsize = 1;
                if (col == 1)
                {
                    dsize = -1;
                }
                if (row == 0)
                {
                    g2_size_change(a, dsize, 0, 0, 0);
                }
                else if (row == 1)
                {
                    g2_size_change(a, 0, 0, dsize, 0);
                }
                else if (row == 2)
                {
                    g2_size_change(a, 0, dsize, 0, 0);
                }
                else if (row == 3)
                {
                    g2_size_change(a, 0, 0, 0, dsize);
                }
            }
            #endregion
            if (name.Contains("g2_game_create_btn"))
            #region 
            {
                string si = strs[4];
                int i = Convert.ToInt32(si);

                switch (i)
                {
                    case 0:
                        int n;
                        n = a.edit.right_selected_index;
                        if (n != -1)
                        {
                            a.edit.creater[n] = a.edit.result;
                            
                            if(a.edit.result.is_type(g2_cell.type.start) ||
                                a.edit.result.is_type(g2_cell.type.in_out))
                            {
                                a.edit.creater[n].add_type(g2_cell.type.oblique);
                            }
                        }

                        n = a.edit.item_right_selected_index;
                        if (n != -1)
                        {
                            int amount = 1;
                            if (a.items.ContainsKey(n))
                            {
                                amount = a.items[n].i;
                            }
                            a.items[n] = new g2_area.item(a.edit.result, amount);
                            if (a.edit.result.is_type(g2_cell.type.start) ||
                                a.edit.result.is_type(g2_cell.type.in_out))
                            {
                                a.edit.creater[n].add_type(g2_cell.type.oblique);
                            }
                        }

                        game_key_name("g2_game_create_btn_1");
                        g2_create_tick_count++;
                        g2_create_tick();
                        Grid g2_game_create_container = (Grid)vm_elems["g2_game_create_container"];
                        g2_game_create_container.Visibility = Visibility.Hidden;
                        break;
                    case 1:
                        a.edit.create();
                        foreach (g2_edit_cell_inf geci in g2_ci)
                        {
                            if (geci != null)
                            {
                                geci.locked = false;
                                geci.cur = g2_edit_cell_inf.type.normal;
                            }
                        }
                        if (a.edit.item_right_selected_index != -1)
                        {
                            a.edit.result.add_type(g2_cell.type.item);
                        }
                        break;
                    case 2:
                        Grid gcag = (Grid)vm_elems["g2_game_create_attrs_grid"];
                        TextBlock gcbt = (TextBlock)vm_elems["g2_game_create_btn_2_text"];
                        if (gcag.Visibility == Visibility.Hidden)
                        {
                            gcag.Visibility = Visibility.Visible;
                            gcbt.Text = "关闭属性设计器";
                        }
                        else
                        {
                            gcag.Visibility = Visibility.Hidden;
                            gcbt.Text = "属性设计器";
                        }
                        break;
                    case 3:
                        g2_game_create_container = (Grid)vm_elems["g2_game_create_container"];
                        g2_game_create_container.Visibility = Visibility.Hidden;
                        
                        n = a.edit.right_selected_index;
                        if (n != -1)
                        {
                            a.edit.creater[n] = new g2_cell();
                            a.edit.creater[n].add_type(g2_cell.type.empty);
                        }

                        n = a.edit.item_right_selected_index;
                        if (n != -1)
                        {
                            a.items.Remove(n);
                        }
                        break;
                }
            }
            #endregion
            if (name.Contains("g2_game_create_attr_show"))
            #region
            {
                string si = strs[5];
                string sj = strs[6];
                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);

                ref int sel = ref g2_t.selected;
                int n = i + 5 * j;
                if (sel == n)
                {
                    sel = -1;
                }
                else
                {
                    sel = n;
                }
            }
            #endregion
            //g2_game_item_edit.Name = "g2_game_item_edit_" + si + "_" + sj;
            if (name.Contains("g2_game_item_edit"))
            #region
            {
                string si = strs[4];
                string sj = strs[5];
                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);
                int n = i + j * 2;

                if (a.items.ContainsKey(n))
                {
                    Grid gac = (Grid)vm_elems["g2_game_amount_container"];
                    gac.Visibility = Visibility.Visible;
                    a.edit.item_left_selected_index = n;

                    TextBox tb = (TextBox)vm_elems["g2_game_amount_tb"];
                    tb.Text = a.items[n].i.ToString();
                }
            }
            #endregion
            if (name.Contains("g2_game_item_select"))
            #region
            {
                string si = strs[4];
                string sj = strs[5];
                int i = Convert.ToInt32(si);
                int j = Convert.ToInt32(sj);

                int old = a.selected;
                int old_i = old % 2;
                int old_j = old / 2;
                string old_si = old_i.ToString();
                string old_sj = old_j.ToString();

                if (old_i == i && old_j == j)
                {
                    a.selected = -1;
                    Rectangle old_bg = (Rectangle)vm_elems["g2_game_item_select_" + old_si + "_" + old_sj + "_bg"];
                    old_bg.Fill = color_mul(old_bg.Fill, 0.5);
                }
                else
                {
                    if (a.selected != -1)
                    {
                        Rectangle old_bg = (Rectangle)vm_elems["g2_game_item_select_" + old_si + "_" + old_sj + "_bg"];
                        old_bg.Fill = color_mul(old_bg.Fill, 0.5);
                    }
                    a.selected = i + 2 * j;
                    background.Fill = color_mul(background.Fill, 2);
                }
            }
            #endregion
            if (name.Contains("g2_game_object_grid_edit"))
            #region
            {
                int index = a.edit.left_selected_index;
                if (index != -1)
                {
                    string si = strs[5];
                    string sj = strs[6];
                    int i = Convert.ToInt32(si);
                    int j = Convert.ToInt32(sj);

                    g2_cell c = a.edit.creater[index];
                    if (c == null)
                    {
                        List<g2_cell.type> ts = new List<g2_cell.type>();
                        c = new g2_cell(ts);
                    }
                    g2_cell old = a.area[i, j];
                    bool enter = old.entered;

                    Rectangle shower = (Rectangle)vm_elems["g2_game_edit_shower"];
                    if (shower.Visibility == Visibility.Hidden)
                    {
                        return;
                    }
                    g2_editor_saver[g2_cell_edit_id] = c;
                    string create = "create " + g2_cell_edit_id + "&";
                    for (int m = 0; m < g2_edit_s * g2_edit_ax; m++)
                    {
                        for (int n = 0; n < g2_edit_s * g2_edit_ay; n++)
                        {
                            g2_cell_edit_id++;
                            g2_editor_saver[g2_cell_edit_id] = a.area[g2_edit_x + m, g2_edit_y + n];
                            create += g2_cell_edit_id + " ";
                        }
                    }
                    a.add_movement(create + "&" + g2_edit_x + " " + g2_edit_y + "$" + g2_edit_s * g2_edit_ax + " " + g2_edit_s * g2_edit_ay);
                    g2_cell_edit_id++;

                    List<g2_cell.type> types = new List<g2_cell.type>();
                    foreach (g2_cell.type t in c.cell_type)
                    {
                        if (t != g2_cell.type.empty)
                        {
                            types.Add(t);
                        }
                    }

                    if (types.Contains(g2_cell.type.special))
                    {
                        for (int m = 0; m < g2_edit_s * g2_edit_ax; m++)
                        {
                            for (int n = 0; n < g2_edit_s * g2_edit_ay; n++)
                            {
                                g2_cell cell = a.area[g2_edit_x + m, g2_edit_y + n];
                                if (c.not_first())
                                {
                                    continue;
                                }

                                if (types.Contains(g2_cell.type.light) &&
                                    cell.is_enterable() &&
                                    !cell.entered)
                                {
                                    cell.entered = true;
                                    cell.color_change = true;
                                }
                                if (types.Contains(g2_cell.type.unlight) && cell.entered)
                                {
                                    cell.entered = false;
                                    cell.color_change = true;
                                    a.add_movement("unlight " + (g2_edit_x + m) + " " + (g2_edit_y + n));
                                }
                            }
                        }
                    }
                    else
                    {
                        a.insert_cell(c.cell_type, g2_edit_x, g2_edit_y, g2_edit_ax, g2_edit_ay, g2_edit_s,
                            (int)c.dir.X, (int)c.dir.Y, c.key, c.door);

                        for (int m = 0; m < g2_edit_s * g2_edit_ax; m++)
                        {
                            for (int n = 0; n < g2_edit_s * g2_edit_ay; n++)
                            {
                                g2_cell cell = a.area[g2_edit_x + m, g2_edit_y + n];
                                cell.in_dirs = c.in_dirs;
                                cell.out_dirs = c.out_dirs;
                                a.check_point_clear(g2_edit_x + m, g2_edit_y + n);

                                if (!cell.is_enterable() && cell.entered)
                                {
                                    cell.entered = false;
                                    cell.color_change = true;
                                    a.add_movement("unlight " + (g2_edit_x + m) + " " + (g2_edit_y + n));
                                }
                            }
                        }
                    }

                    a.grid_changed = true;
                    a.item_changed = true;
                    a.wall_changed = true;

                }

            }
            #endregion
            if (name.Contains("g2_game_object_grid_stroke"))
            #region
            {
                if (a.selected != -1)
                {
                    string si = strs[5];
                    string sj = strs[6];
                    int i = Convert.ToInt32(si);
                    int j = Convert.ToInt32(sj);

                    int sizex = 1;
                    int sizey = 1;

                    g2_cell c = null;
                    if (a.items[a.selected].i > 0)
                    {
                        c = a.items[a.selected].c;
                    }
                    if (c != null)
                    {
                        g2_cell old = a.area[i, j];
                        int index = a.selected;
                        bool enter = old.entered;

                        a.add_movement("insert " + index + " " + i + " " + j);

                        a.insert_cell(c.cell_type, i, j, sizex, sizey, old.all_size, (int)c.dir.X, (int)c.dir.Y, c.key, c.door);
                        for (int m = 0; m < old.all_size * sizex; m++)
                        {
                            for (int n = 0; n < old.all_size * sizey; n++)
                            {
                                a.area[i + m, j + n].in_dirs = c.in_dirs;
                                a.area[i + m, j + n].out_dirs = c.out_dirs;
                                a.check_point_clear(i + m, j + n);
                            }
                        }

                        r.Fill = color_mul(r.Fill, 1.25);
                        if (a.items[index].i < 1000)
                        {
                            a.items[index].i--;
                        }

                        a.grid_changed = true;
                        a.item_changed = true;
                        a.wall_changed = true;

                    }
                }
            }
            #endregion

            if (name.Contains("vm_vstore_item"))
            {
                string target = name + "_detail";
                Grid gd = (Grid)vm_elems[target];
                gd.Visibility = Visibility.Visible;
            }


        }
    }
}
