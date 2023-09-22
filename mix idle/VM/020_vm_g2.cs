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
        class g2_level_link
        {
            public string name;

            public bool state = false;

            public g2_level_information a;
            public g2_level_information b;

            public g2_level_link(string nam, g2_level_information first, g2_level_information second, bool s = false)
            {
                name = nam;
                a = first;
                b = second;
                state = s;
            }
        }

        [Serializable]
        class g2_level_information
        {
            public string name;
            public int state = -1;
            public List<g2_level_link> prevs = new List<g2_level_link>();
            public List<g2_level_link> nexts = new List<g2_level_link>();

            public g2_level_information(string nam)
            {
                name = nam;
            }
        }
        Dictionary<string, g2_level_information> infs = new Dictionary<string, g2_level_information>();

        [Serializable]
        class g2_complete_condition
        {
            public enum type
            {
                normal = 0,
                count = 1,
                ball = 2,
                energy = 3,
            }
            public Dictionary<type, bool> t = new Dictionary<type, bool>();

            public int count_req = 0;
            public int ball_req = 0;
            public double energy_req = 0;

            public g2_complete_condition()
            {
                t.Add(type.normal, false);
            }

            public void set_type(type x)
            {
                t.Clear();
                t.Add(x, false);
            }

            public void add_type(type x)
            {
                t.Add(x, false);
            }

            public bool is_type(type x)
            {
                if (t.ContainsKey(x))
                {
                    return true;
                }
                return false;
            }
        }

        [Serializable]
        class g2_item_relation_table
        {
            public Dictionary<g2_cell.type, List<g2_cell.type>> conflict = new Dictionary<g2_cell.type, List<g2_cell.type>>();
            public Dictionary<g2_cell.type, List<g2_cell.type>> combine = new Dictionary<g2_cell.type, List<g2_cell.type>>();
            public Dictionary<g2_cell.type, List<g2_cell.type>> need = new Dictionary<g2_cell.type, List<g2_cell.type>>();

            public Dictionary<g2_cell.type, bool> can_design = new Dictionary<g2_cell.type, bool>();
            public Dictionary<g2_cell.type, List<g2_cell.type>> attach = new Dictionary<g2_cell.type, List<g2_cell.type>>();
            public int selected = -1;

            public void add_must(g2_cell.type a, g2_cell.type b)
            {
                if (!need.ContainsKey(a))
                {
                    need[a] = new List<g2_cell.type>();
                }
                need[a].Add(b);

                List<g2_cell.type> types = new List<g2_cell.type>();
                types.Add(a);
                types.Add(b);
                add(combine, types);
            }

            public void add(Dictionary<g2_cell.type, List<g2_cell.type>> target,
                List<g2_cell.type> types)
            {
                foreach(g2_cell.type c1 in types)
                {
                    foreach(g2_cell.type c2 in types)
                    {
                        if (!c1.Equals(c2))
                        {
                            if (!target.ContainsKey(c1))
                            {
                                target[c1] = new List<g2_cell.type>();
                            }
                            if (!target[c1].Contains(c2))
                            {
                                target[c1].Add(c2);
                            }
                        }
                    }
                }
            }

            public bool is_(Dictionary<g2_cell.type, List<g2_cell.type>> target,
                g2_cell.type a, g2_cell.type b)
            {
                if (target.ContainsKey(a) && target[a].Contains(b))
                {
                    return true;
                }
                return false;
            }

            public bool is_conflict(g2_cell a, g2_cell b)
            {
                bool ret = false;
                foreach (g2_cell.type ta in a.cell_type)
                {
                    foreach (g2_cell.type tb in b.cell_type)
                    {
                        if (is_(conflict, ta, tb))
                        {
                            ret = true; //一个冲突全部冲突
                        }
                    }
                }
                return ret;
            }

            public bool is_combinable(g2_cell a, g2_cell b)
            {
                if (a.cell_type.Count == 0)  //为空 所有可选择
                {
                    return true;
                }

                bool ret = false;            //完全无关 不可选择
                foreach (g2_cell.type ta in a.cell_type)
                {
                    foreach (g2_cell.type tb in b.cell_type)
                    {
                        if (is_(combine, ta, tb))
                        {
                            ret = true;      //部分匹配即可
                        }
                    }
                }
                return ret;
            }

            public bool is_must(g2_cell a, g2_cell b)
            {
                bool ret = false;            //完全无关 非必须
                foreach (g2_cell.type ta in a.cell_type)
                {
                    foreach (g2_cell.type tb in b.cell_type)
                    {
                        if (is_(need, ta, tb))
                        {
                            ret = true;      //一个需要 整个需要
                        }
                    }
                }
                return ret;
            }

            public bool is_selectable(g2_cell a, g2_cell b)
            {
                if (is_conflict(a, b))
                {
                    return false;
                }
                if (is_combinable(a, b))
                {
                    return true;
                }
                if (is_must(a, b))
                {
                    return true;
                }
                return false;
            }

            public bool is_designable(g2_cell a)
            {
                foreach (g2_cell.type t in a.cell_type)
                {
                    if (can_design.ContainsKey(t))
                    {
                        if (can_design[t])
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public bool is_correct(g2_cell a)
            {
                foreach(g2_cell.type t in a.cell_type)
                {
                    if (need.ContainsKey(t))
                    {
                        foreach (g2_cell.type t2 in need[t])
                        {
                            List<g2_cell.type> types = new List<g2_cell.type>();
                            types.Add(t2);
                            g2_cell c = new g2_cell(types);
                            if (is_conflict(c, a))
                            {
                                continue;
                            }
                            if (!a.cell_type.Contains(t2))
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        g2_item_relation_table g2_t;
        
        [Serializable]
        class g2_edit_cell_inf
        {
            public enum type
            {
                normal = 0,
                conflict = 1,
                must = 2,
                select = 3
            }
            public type old;
            public type cur;

            public g2_cell c;
            public bool locked;

            public g2_edit_cell_inf(g2_cell x)
            {
                old = type.normal;
                cur = type.normal;
                c = x;
                locked = false;
            }
        }
        g2_edit_cell_inf[,,,] g2_ci;

        [Serializable]
        class g2_edit
        {
            public int right_selected_index = -1;
            public int left_selected_index = -1;
            public int item_right_selected_index = -1;
            public int item_left_selected_index = -1;

            public int curr_amount = -1;

            public g2_cell result;
            public g2_cell[] creater = new g2_cell[16];

            public g2_edit()
            {
                create();
            }
            
            public void create()
            {
                List<g2_cell.type> types = new List<g2_cell.type>();
                result = new g2_cell(types);
            }

            public void confirm()
            {
                creater[right_selected_index] = result;
            }
        }

        [Serializable]
        class g2_area
        {
            public gamestats gs;
            public string name;
            public string show_name = "";
            public bool game_win = false;
            public g2_complete_condition condition = new g2_complete_condition();
            public double cell_size = 0;

            public enum mode
            {
                classic = 0,
                consecutive = 1
            }

            [Serializable]
            public class item
            {
                public g2_cell c;
                public int i;

                public item(g2_cell cell, int amount = 1)
                {
                    c = cell;
                    i = amount;
                }
            }

            public bool editing = false;
            public g2_edit edit = new g2_edit();
            public bool testing = false;

            public mode area_mode;
            public List<g2_ball> balls = new List<g2_ball>();
            public List<g2_ball> temp_balls = new List<g2_ball>();
            public g2_check_point[,] check;

            public double2 energy = new double2(0, 0);
            public double2 need_time = new double2(0, 0);
            public double2 need_energy = new double2(0, 0);

            public int width;
            public int height;
            public g2_cell[,] area;
            public bool grid_changed = true;
            public bool wall_changed = true;
            public bool item_changed = true;
            public int selected = -1;

            public int enter_count = 0;
            public List<string> movements = new List<string>();
            public long tick_value = 0;
            public Dictionary<int, item> items = new Dictionary<int, item>();

            public int item_counter = 0;
            public void auto_item_add(g2_cell c, int n)
            {
                items[item_counter] = new item(c, n);
                item_counter++;
            }

            public g2_area(string nam)
            {
                name = nam;
            }

            public string description;
            public g2_area(gamestats g, string nam, mode m, int w, int h, double2 t, double2 e, string des)
            {
                gs = g;
                name = nam;
                area_mode = m;
                width = w;
                height = h;
                area = new g2_cell[w, h];
                check = new g2_check_point[10 * w + 1, 10 * h + 1];
                need_time = t;
                need_energy = e;
                description = des;

                movements = new List<string>();
                List<g2_cell.type> empty = new List<g2_cell.type>();
                for (int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        area[i, j] = new g2_cell(empty);
                    }
                }
            }

            public void add_movement(string m)
            {
                string[] strs = m.Split(' ');
                movements.Add(m + "@" + tick_value);
            }

            public bool contain_xy(int x, int y)
            {
                if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
                {
                    return false;
                }
                return true;
            }

            public bool insert_cell(List<g2_cell.type> types, int x, int y, int ax = 1, int ay = 1, int s = 1,
                int dir_x = 0, int dir_y = 0, string key = "", string door = "",
                string ins = "", string outs = "")
            {
                if (x < 0 || x + ax * s > width)
                {
                    return false;
                }
                if (y < 0 || y + ay * s > height)
                {
                    return false;
                }
                for (int i = x; i < x + ax * s; i += s)
                {
                    for (int j = y; j < y + ay * s; j += s)
                    {
                        for (int m = 0; m < s; m++)
                        {
                            for(int n = 0; n < s; n++)
                            {
                                bool old_entered = area[i, j].entered;
                                ref g2_cell c = ref area[i + m, j + n];
                                if (area[i + m, j + n].all_size > 1)
                                {
                                    g2_cell temp = area[i + m, j + n];
                                    int fx = i + m - temp.part_of_x;
                                    int fy = j + n - temp.part_of_y;
                                    gs.g2_editor_saver[gs.g2_cell_edit_id] = area[fx, fy];
                                    add_movement("lose " + gs.g2_cell_edit_id + " " + fx + " " + fy);
                                    gs.g2_cell_edit_id++;

                                    for (int p = 0; p < temp.all_size; p++)
                                    {
                                        for (int q = 0; q < temp.all_size; q++)
                                        {
                                            List<g2_cell.type> empty = new List<g2_cell.type>();
                                            empty.Add(g2_cell.type.empty);
                                            area[i + m - temp.part_of_x + p, j + n - temp.part_of_y + q] = new g2_cell(empty);
                                        }
                                    }
                                }
                                //i = 3 j = 2
                                //area[2, 3] = p(0, 0) = (3, 2);
                                //area[3, 3] = p(0, 1) = (3, 3);
                                //area[2, 4] = p(1, 0) = (4, 2);
                                //area[3, 4] = p(1, 1) = (4, 3);
                                c = new g2_cell(types, m, n, s)
                                {
                                    entered = old_entered
                                };
                                if (c.is_wall())
                                {
                                    c.delete_type(g2_cell.type.safe);
                                }
                                else
                                {
                                    if (!c.is_type(g2_cell.type.safe))
                                    {
                                        c.add_type(g2_cell.type.safe);
                                    }
                                }
                                if (c.is_type(g2_cell.type.start))
                                {
                                    c.dir = new Point(dir_x, dir_y);
                                }
                                if (c.is_type(g2_cell.type.reflecter))
                                {
                                    c.dir = new Point(dir_x, dir_y);
                                }
                                if (types.Contains(g2_cell.type.door))
                                {
                                    c.door = door;
                                }
                                if (types.Contains(g2_cell.type.key))
                                {
                                    c.key = key;
                                }
                                if (types.Contains(g2_cell.type.in_out))
                                {
                                    foreach (char ch in ins)
                                    {
                                        c.in_dirs.Add(point_spin(Convert.ToInt32(ch)));
                                    }
                                    foreach (char ch in outs)
                                    {
                                        c.out_dirs.Add(point_spin(Convert.ToInt32(ch)));
                                    }
                                }

                                if (s > 1 && types.Contains(g2_cell.type.oblique))
                                {
                                    if (m == 0 && n == 0 && c.contains_dir(new Point(-1, -1)))
                                    {
                                    }
                                    else if (m == 0 && n == (s - 1) && c.contains_dir(new Point(-1, 1)))
                                    {
                                    }
                                    else if (m == (s - 1) && n == 0 && c.contains_dir(new Point(1, -1)))
                                    {
                                    }
                                    else if (m == (s - 1) && n == (s - 1) && c.contains_dir(new Point(1, 1)))
                                    {
                                    }
                                    else
                                    {
                                        c.delete_type(g2_cell.type.oblique);
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }

            public void check_point_clear(int x, int y, int s)
            {
                List<g2_cell.type> empty = new List<g2_cell.type>();
                for (int i = 0; i <= 10 * s; i++)
                {
                    for (int j = 0; j <= 10 * s; j++)
                    {
                        check[10 * x + i, 10 * y + j] = null;
                    }
                }
            }

            public void check_point_clear(int x, int y)
            {
                List<g2_cell.type> empty = new List<g2_cell.type>();
                for (int i = 0; i <= 10; i++)
                {
                    for(int j = 0; j <= 10; j++)
                    {
                        check[10 * x + i, 10 * y + j] = null;
                    }
                }
            }

            public bool exist_d(int x, int y, int dx, int dy)
            {
                int x2 = x + dx;
                int y2 = y + dy;

                if (x2 < 0 || x2 >= width || y2 < 0 || y2 >= height)
                {
                    return false;
                }
                return true;
            }

            public bool is_wall(int x, int y, int dx, int dy)
            {
                int x2 = x + dx;
                int y2 = y + dy;

                if (x2 < 0 || x2 >= width || y2 < 0 || y2 >= height)
                {
                    return true;
                }
                else
                {
                    g2_cell c = area[x2, y2];
                    if (c.is_wall())
                    {
                        return true;
                    }
                    return false;
                }
            }

            /* 0000 - 无边角
            // 1010 - 左边两个角封住
            // 1100 - 上边两个角封住
            // 0101 - 右边两个角封住
            // 0011 - 下边两个角封住
            // 0111 - 仅左上角无角
            // 1011 - 仅右上角无角
            // 1101 - 仅左下角无角
            // 1110 - 仅右上角无角
            // 1111 - 超级墙
            */
            public int get_wall_type(int x, int y)
            {
                g2_cell c = area[x, y];
                
                if (x == 0 && y == 5)
                {

                }
                if (x == 0 && y == 4)
                {

                }

                int ret = 0;
                if (c.cell_type.Contains(g2_cell.type.super_wall))
                {
                    return 15;
                }
                else if (c.is_wall())
                {
                    if (is_wall(x, y, -1, 0))
                    {
                        ret |= 10;
                    }
                    if (is_wall(x, y, 1, 0))
                    {
                        ret |= 5;
                    }
                    if (is_wall(x, y, 0, -1))
                    {
                        ret |= 12;
                    }
                    if (is_wall(x, y, 0, 1))
                    {
                        ret |= 3;
                    }
                    
                    if (c.is_type(g2_cell.type.oblique) && c.contains_dir(new Point(-1, -1)))
                    {
                        if (!is_wall(x, y, -1, -1))
                        {
                            ret &= 15 - 8;
                            if (is_wall(x, y, -1, 0))
                            {
                                area[x - 1, y].oblique_switch = false;
                                area[x - 1, y].oblique_dirs.Add(new Point(1, -1));
                            }
                            if (is_wall(x, y, 0, -1))
                            {
                                area[x, y - 1].oblique_switch = false;
                                area[x, y - 1].oblique_dirs.Add(new Point(-1, 1));
                            }
                        }
                    }
                    if (c.is_type(g2_cell.type.oblique) && c.contains_dir(new Point(1, -1)))
                    {
                        if (!is_wall(x, y, 1, -1))
                        {
                            ret &= 15 - 4;
                            if (is_wall(x, y, 1, 0))
                            {
                                area[x + 1, y].oblique_switch = false;
                                area[x + 1, y].oblique_dirs.Add(new Point(-1, -1));
                            }
                            if (is_wall(x, y, 0, -1))
                            {
                                area[x, y - 1].oblique_switch = false;
                                area[x, y - 1].oblique_dirs.Add(new Point(1, 1));
                            }
                        }
                    }
                    if (c.is_type(g2_cell.type.oblique) && c.contains_dir(new Point(-1, 1)))
                    {
                        if (!is_wall(x, y, -1, 1))
                        {
                            ret &= 15 - 2;
                            if (is_wall(x, y, -1, 0))
                            {
                                area[x - 1, y].oblique_switch = false;
                                area[x - 1, y].oblique_dirs.Add(new Point(1, 1));
                            }
                            if (is_wall(x, y, 0, 1))
                            {
                                area[x, y + 1].oblique_switch = false;
                                area[x, y + 1].oblique_dirs.Add(new Point(-1, -1));
                            }
                        }
                    }
                    if (c.is_type(g2_cell.type.oblique) && c.contains_dir(new Point(1, 1)))
                    {
                        if (!is_wall(x, y, 1, 1))
                        {
                            ret &= 15 - 1;
                            if (is_wall(x, y, 1, 0))
                            {
                                area[x + 1, y].oblique_switch = false;
                                area[x + 1, y].oblique_dirs.Add(new Point(-1, 1));
                            }
                            if (is_wall(x, y, 0, 1))
                            {
                                area[x, y + 1].oblique_switch = false;
                                area[x, y + 1].oblique_dirs.Add(new Point(1, -1));
                            }
                        }
                    }
                    
                    if (!c.oblique_switch && c.oblique_dirs_contains_dir(new Point(-1, -1)))
                    {
                        ret &= 15 - 8;
                    }
                    if (!c.oblique_switch && c.oblique_dirs_contains_dir(new Point(1, -1)))
                    {
                        ret &= 15 - 4;
                    }
                    if (!c.oblique_switch && c.oblique_dirs_contains_dir(new Point(-1, 1)))
                    {
                        ret &= 15 - 2;
                    }
                    if (!c.oblique_switch && c.oblique_dirs_contains_dir(new Point(1, 1)))
                    {
                        ret &= 15 - 1;
                    }
                    return ret;
                }
                else
                {
                    return -1;
                }
            }

            public void create_check_point(int x, int y)
            {
                g2_cell c = area[x, y];
                /*
                foreach(g2_check_point cp in check)
                {
                    if (cp != null)
                    {
                        cp.delete_type(g2_check_point.type.empty);
                    }
                }*/
                //墙
                #region
                if (is_wall(x, y, 0, 0))
                {
                    int w = get_wall_type(x, y);
                    int wa = w & 8;
                    int wb = w & 4;
                    int wc = w & 2;
                    int wd = w & 1;
                    
                    if (wa == 8)
                    {
                        wa = 1;
                    }
                    if (wb == 4)
                    {
                        wb = 1;
                    }
                    if (wc == 2)
                    {
                        wc = 1;
                    }
                    if (wd == 1)
                    {
                        wd = 1;
                    }

                    // wa = 1
                    // * * * |
                    // * * * |
                    // * * * |
                    // - - -

                    // wa = 0
                    // + + + |
                    // + * * |
                    // + * * |
                    // - - -

                    
                    for (int i = 0; i <= 10; i++)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            g2_check_point g2_Check_Point = check[10 * x + i, 10 * y + j];
                            if (g2_Check_Point != null)
                            {
                                g2_Check_Point.delete_type(g2_check_point.type.death);
                            }
                            /*
                            if (g2_Check_Point != null)
                            {
                                g2_Check_Point.delete_type(g2_check_point.type.empty);
                            }*/
                        }
                    }

                    for (int i = (1 - wa); i <= 2; i++)
                    {
                        for (int j = (1 - wa); j <= 2; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j], 
                                g2_check_point.type.death, null);
                        }
                    }

                    for (int i = 3; i <= 7; i++)
                    {
                        for (int j = 0; j <= 2; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.death, null);
                        }
                    }

                    for (int i = 8; i <= (9 + wb); i++)
                    {
                        for (int j = (1 - wb); j <= 2; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.death, null);
                        }
                    }

                    for (int i = 0; i <= 10; i++)
                    {
                        for (int j = 3; j <= 7; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.death, null);
                        }
                    }

                    for (int i = (1 - wc); i <= 2; i++)
                    {
                        for (int j = 8; j <= (9 + wc); j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.death, null);
                        }
                    }

                    for (int i = 3; i <= 7; i++)
                    {
                        for (int j = 8; j <= 10; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.death, null);
                        }
                    }

                    for (int i = 8; i <= (9 + wd); i++)
                    {
                        for (int j = 8; j <= (9 + wd); j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.death, null);
                        }
                    }
                }
                #endregion

                int size = c.all_size;
                if (c.part_of_x != 0 || c.part_of_y != 0)
                {
                    return;
                }

                //起点
                #region
                if (c.is_type(g2_cell.type.start)
                    || c.is_type(g2_cell.type.in_out)
                    )
                {
                    List<Point> in_dirs = new List<Point>();
                    List<Point> out_dirs = new List<Point>();
                    if (c.is_type(g2_cell.type.start))
                    {
                        out_dirs.Add(c.dir);
                    }
                    else
                    {
                        in_dirs = c.in_dirs;
                        out_dirs = c.out_dirs;
                    }

                    for(int i = 3 * size; i <= 7 * size; i++)
                    {
                        for(int j = 3 * size; j <= 7 * size; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.empty, null);
                        }
                    }


                    foreach (Point dir in out_dirs)
                    {
                        List<Point> rev_death = new List<Point>();
                        Point rev = new Point(-dir.X, -dir.Y);
                        rev_death.Add(rev);
                        //→
                        if (dir.X == 1 && dir.Y == 0)
                        {
                            for (int i = 5 * size; i <= 10 * size; i++)
                            {
                                for (int j = 4 * size; j <= 6 * size; j++)
                                {
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point g2_Check_Point = check[10 * x + i, 10 * y + j];
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //←
                        if (dir.X == -1 && dir.Y == 0)
                        {
                            for (int i = 0; i <= 5 * size; i++)
                            {
                                for (int j = 4 * size; j <= 6 * size; j++)
                                {
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point g2_Check_Point = check[10 * x + i, 10 * y + j];
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //↑
                        if (dir.X == 0 && dir.Y == -1)
                        {
                            for (int i = 4 * size; i <= 6 * size; i++)
                            {
                                for (int j = 0; j <= 5 * size; j++)
                                {
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //↓
                        if (dir.X == 0 && dir.Y == 1)
                        {
                            for (int i = 4 * size; i <= 6 * size; i++)
                            {
                                for (int j = 5 * size; j <= 10 * size; j++)
                                {
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //↗
                        if (dir.X == 1 && dir.Y == -1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 5 * size; i <= 10 * size; i++)
                                {
                                    int j = 10 * size - i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //↘
                        if (dir.X == 1 && dir.Y == 1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 5 * size; i <= 10 * size; i++)
                                {
                                    int j = i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //↖
                        if (dir.X == -1 && dir.Y == -1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 0; i <= 5 * size; i++)
                                {
                                    int j = i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                        //↙
                        if (dir.X == -1 && dir.Y == 1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 0; i <= 5 * size; i++)
                                {
                                    int j = 10 * size - i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    if (i >= 2 * size && i <= 8 * size && j >= 2 * size && j <= 8 * size)
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.empty, null);
                                    }
                                    else
                                    {
                                        g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                            g2_check_point.type.death, rev_death);
                                    }
                                }
                            }
                        }

                    }
                    foreach (Point dir in in_dirs)
                    {
                        //→
                        if (dir.X == 1 && dir.Y == 0)
                        {
                            for (int i = 5 * size; i <= 10 * size; i++)
                            {
                                for (int j = 4 * size; j <= 6 * size; j++)
                                {
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //←
                        if (dir.X == -1 && dir.Y == 0)
                        {
                            for (int i = 0; i <= 5 * size; i++)
                            {
                                for (int j = 4 * size; j <= 6 * size; j++)
                                {
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //↑
                        if (dir.X == 0 && dir.Y == -1)
                        {
                            for (int i = 4 * size; i <= 6 * size; i++)
                            {
                                for (int j = 0; j <= 5 * size; j++)
                                {
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //↓
                        if (dir.X == 0 && dir.Y == 1)
                        {
                            for (int i = 4 * size; i <= 6 * size; i++)
                            {
                                for (int j = 5 * size; j <= 10 * size; j++)
                                {
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //↗
                        if (dir.X == 1 && dir.Y == -1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 5 * size; i <= 10 * size; i++)
                                {
                                    int j = 10 * size - i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //↘
                        if (dir.X == 1 && dir.Y == 1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 5 * size; i <= 10 * size; i++)
                                {
                                    int j = i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //↖
                        if (dir.X == -1 && dir.Y == -1)
                        {
                            for (int k = (int)(-1.5 * size); k <= 1.5 * size; k++)
                            {
                                for (int i = 0; i <= 5 * size; i++)
                                {
                                    int j = i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }

                        //↙
                        if (dir.X == -1 && dir.Y == 1)
                        {
                            for (int k = -1 * size; k <= 1 * size; k++)
                            {
                                for (int i = 0; i <= 5 * size; i++)
                                {
                                    int j = 10 * size - i + k;
                                    if (j < 0 || j > 10 * size)
                                    {
                                        continue;
                                    }
                                    g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                        g2_check_point.type.empty, null);
                                }
                            }
                        }
                    }

                }
                #endregion
                if (c.is_type(g2_cell.type.in_out))
                {
                    g2_check_point.add_type(ref check[10 * x + 5 * size, 10 * y + 5 * size],
                        g2_check_point.type.in_out, null);
                }

                if (c.is_type(g2_cell.type.reflecter))
                {
                    Point dir = c.dir;
                    List<Point> ps = new List<Point>();
                    ps.Add(dir);

                    if (dir.X == 1 && dir.Y == 0)
                    {
                        for(int i = 0; i <= 10 * size; i++)
                        {
                            if (check[10 * x + i, 10 * y + 5 * size] != null)
                            {
                                check[10 * x + i, 10 * y + 5 * size].delete_type(g2_check_point.type.empty);
                                check[10 * x + i, 10 * y + 5 * size].delete_type(g2_check_point.type.death);
                                check[10 * x + i, 10 * y + 5 * size].delete_type(g2_check_point.type.in_out);
                            }
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + 5 * size],
                                g2_check_point.type.reflect, ps);
                        }
                    }
                    if (dir.X == 0 && dir.Y == 1)
                    {
                        for (int i = 0; i <= 10 * size; i++)
                        {
                            if (check[10 * x + 5 * size, 10 * y + i] != null)
                            {
                                check[10 * x + 5 * size, 10 * y + i].delete_type(g2_check_point.type.empty);
                                check[10 * x + 5 * size, 10 * y + i].delete_type(g2_check_point.type.death);
                                check[10 * x + 5 * size, 10 * y + i].delete_type(g2_check_point.type.in_out);
                            }

                            g2_check_point.add_type(ref check[10 * x + 5 * size, 10 * y + i],
                                g2_check_point.type.reflect, ps);
                        }
                    }
                    if (dir.X == 1 && dir.Y == 1)
                    {
                        for (int i = 1 * size; i <= 9 * size; i++)
                        {
                            if (check[10 * x + i, 10 * y + i] != null)
                            {
                                check[10 * x + i, 10 * y + i].delete_type(g2_check_point.type.empty);
                                check[10 * x + i, 10 * y + i].delete_type(g2_check_point.type.death);
                                check[10 * x + i, 10 * y + i].delete_type(g2_check_point.type.in_out);
                            }
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + i],
                                g2_check_point.type.reflect, ps);
                        }
                    }
                    if (dir.X == -1 && dir.Y == 1)
                    {
                        for (int i = 1 * size; i <= 9 * size; i++)
                        {
                            if (check[10 * (x + size) - i, 10 * y + i] != null)
                            {
                                check[10 * (x + size) - i, 10 * y + i].delete_type(g2_check_point.type.empty);
                                check[10 * (x + size) - i, 10 * y + i].delete_type(g2_check_point.type.death);
                                check[10 * (x + size) - i, 10 * y + i].delete_type(g2_check_point.type.in_out);
                            }
                            g2_check_point.add_type(ref check[10 * (x + size) - i, 10 * y + i],
                                g2_check_point.type.reflect, ps);
                        }
                    }
                }
                /*
                if (c.is_type(g2_cell.type.empty) || 
                    c.only_type(g2_cell.type.x))
                {
                    for (int i = 0; i <= 10 * size; i++)
                    {
                        for (int j = 0; j <= 10 * size; j++)
                        {
                            g2_check_point.add_type(ref check[10 * x + i, 10 * y + j],
                                g2_check_point.type.empty, null);
                        }
                    }
                }*/
            }

            public void create_check_point()
            {
                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        create_check_point(i, j);
                    }
                }
            }

            public Point? ball_all_at(g2_ball b)
            {
                //1.81, 2.5
                //1.81-2.01, 2.5-2.7
                //1-2, 2

                decimal x1 = decimal.Floor((decimal)b.ball_pos.X);
                decimal x2 = decimal.Floor((decimal)(b.ball_pos.X + b.size()));
                decimal y1 = decimal.Floor((decimal)b.ball_pos.Y);
                decimal y2 = decimal.Floor((decimal)(b.ball_pos.Y + b.size()));

                //-0.15 - 0.05
                if (x1 < 0 || y1 < 0 || x2 >= width || y2 >= height)
                {
                    b.live = false;
                    return null;
                }

                int f1 = (int)x1;
                int g1 = (int)y1;
                g2_cell c = area[f1, g1];

                int f2 = f1 + c.all_size - 1;
                int g2 = g1 + c.all_size - 1;

                if (x2 <= f2 && y2 <= g2)
                {
                    return new Point(f1, g1);
                }
                else
                {
                    return null;
                }
            }

            public List<Point> ball_at(g2_ball b)
            {

                List<Point> ret = new List<Point>();
                //1.81, 2.5
                //1.81-2.01, 2.5-2.7
                //1-2, 2

                decimal x1 = decimal.Floor((decimal)b.ball_pos.X);
                decimal x2 = decimal.Floor((decimal)(b.ball_pos.X + b.size()));
                decimal y1 = decimal.Floor((decimal)b.ball_pos.Y);
                decimal y2 = decimal.Floor((decimal)(b.ball_pos.Y + b.size()));

                //-0.15 - 0.05
                if (x1 < 0 || y1 < 0 || x2 >= width || y2 >= height)
                {
                    b.live = false;
                    return null;
                }

                for(decimal i = x1; i <= x2; i++)
                {
                    for(decimal j = y1; j <= y2; j++)
                    {
                        ret.Add(new Point((int)i, (int)j));
                    }
                }
                return ret;
            }

            public static bool point_contains(List<Point> ps, int x, int y)
            {
                if (ps == null)
                {
                    return false;
                }
                foreach (Point p in ps)
                {
                    if (p.X == x && p.Y == y)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void move_and_check(g2_ball b)
            {
                if (!b.live)
                {
                    return;
                }
                double x = b.ball_pos.X;
                double y = b.ball_pos.Y;

                double length = Math.Sqrt(b.move_x * b.move_x + b.move_y * b.move_y);
                double dx = 0;
                double dy = 0;
                if (length != 0)
                {
                    dx = b.move_x * b.ball_speed / length;
                    dy = b.move_y * b.ball_speed / length;
                    b.ball_pos.X += dx;
                    b.ball_pos.Y += dy;
                }

                List<Point> cells = ball_at(b);
                ball_all_at(b);

                if (cells == null)
                {
                    return;
                }
                foreach (Point p in cells)
                {
                    g2_cell c = area[(int)p.X, (int)p.Y];
                    for(int i = 0; i <= 10; i++)
                    {
                        for(int j = 0; j <= 10; j++)
                        {
                            double posx = (p.X * 10 + i) / 10;
                            double posy = (p.Y * 10 + j) / 10;
                            g2_check_point g = check[(int)p.X * 10 + i, (int)p.Y * 10 + j];
                            if (g == null)
                            {
                                continue;
                            }
                            if (g.have_type(g2_check_point.type.empty))
                            {
                            }
                            else
                            {
                                double disx = b.ball_pos.X + b.size() / 2 - posx;
                                double disy = b.ball_pos.Y + b.size() / 2 - posy;
                                double distance = Math.Sqrt(disx * disx + disy * disy);
                                if (g.have_type(g2_check_point.type.death) && !c.is_type(g2_cell.type.safe))
                                {
                                    List<Point> d = g.point_dic[g2_check_point.type.death];
                                    if ((d == null) || point_contains(d, b.move_x, b.move_y))
                                    {
                                        if (distance < b.size() / 2)
                                        {
                                            b.live = false;
                                            return;
                                        }
                                    }
                                }
                            }


                            /*
                            Rectangle g2_check_r = new Rectangle();
                            string rn = "g2_check_r";
                            if (gs.game.ContainsKey(rn))
                            {
                                g2_check_r = (Rectangle)gs.game["g2_check_r"];
                            }
                            else
                            {
                                Grid gb = (Grid)gs.game["g2_game_ball_grid"];
                                gb.Children.Add(g2_check_r);
                            }
                            g2_check_r.Name = rn;
                            g2_check_r.Fill = gs.getSCB(Color.FromArgb(255, 0, 0, 0));
                            g2_check_r.Stroke = gs.getSCB(Color.FromArgb(255, 0, 0, 0));
                            g2_check_r.HorizontalAlignment = HorizontalAlignment.Left;
                            g2_check_r.VerticalAlignment = VerticalAlignment.Top;
                            g2_check_r.StrokeThickness = 1;
                            g2_check_r.Width = (r3.X - r1.X) * cell_size * 10;
                            g2_check_r.Height = (r2.Y - r1.Y) * cell_size * 10;
                            g2_check_r.Margin = new Thickness(r1.X * cell_size, r1.Y * cell_size, 0, 0);
                            gs.game_assign(g2_check_r);*/
                            
                            if (g.have_type(g2_check_point.type.reflect) ||
                                g.have_type(g2_check_point.type.in_out))
                            {
                                int vx = 0, vy = 0;
                                for (int m = -1; m <= 1; m++)
                                {
                                    for (int n = -1; n <= 1; n++)
                                    {
                                        if (m == 0 && n == 0)
                                        {
                                            continue;
                                        }
                                        if (b.move_x * m + b.move_y * n == 0)
                                        {
                                            vx = m;
                                            vy = n;
                                        }
                                    }
                                }

                                double rx1 = b.ball_pos.X + b.size() / 2 - 0.6 * dx;
                                double ry1 = b.ball_pos.Y + b.size() / 2 - 0.6 * dy;
                                double rx2 = b.ball_pos.X + b.size() / 2 + 0.6 * dx;
                                double ry2 = b.ball_pos.Y + b.size() / 2 + 0.6 * dy;

                                Point r1 = new Point(rx1 - 0.04 * vx, ry1 - 0.04 * vy);
                                Point r2 = new Point(rx1 + 0.04 * vx, ry1 + 0.04 * vy);
                                Point r3 = new Point(rx2 - 0.04 * vx, ry2 - 0.04 * vy);
                                Point r4 = new Point(rx2 + 0.04 * vx, ry2 + 0.04 * vy);

                                bool in_rect = false;
                                if (vx == 0)
                                {
                                    if ((r1.X - posx) * (r3.X - posx) <= 0)
                                    {
                                        if ((r1.Y - posy) * (r2.Y - posy) <= 0)
                                        {
                                            in_rect = true;
                                        }
                                    }
                                }
                                else if (vy == 0)
                                {
                                    if ((r1.X - posx) * (r2.X - posx) <= 0)
                                    {
                                        if ((r1.Y - posy) * (r3.Y - posy) <= 0)
                                        {
                                            in_rect = true;
                                        }
                                    }
                                }
                                else
                                {
                                    int k1 = vy / vx;
                                    int k2 = -1 / k1;

                                    double r1r3 = (k1 * posx - k1 * r1.X - posy + r1.Y) *
                                                  (k1 * posx - k1 * r3.X - posy + r3.Y);
                                    double r1r2 = (k2 * posx - k2 * r1.X - posy + r1.Y) *
                                                  (k2 * posx - k2 * r2.X - posy + r2.Y);
                                    if (r1r3 <= 0)
                                    {
                                        if (r1r2 <= 0)
                                        {
                                            in_rect = true;
                                        }
                                    }
                                }

                                if (g.ball_reactioned.ContainsKey(b.id))
                                {

                                }
                                else
                                {
                                    g.ball_reactioned.Add(b.id, false);
                                }

                                if (in_rect && !g.ball_reactioned[b.id])
                                {
                                    g.ball_reactioned[b.id] = true;

                                    b.ball_pos = new Point(posx - b.size() / 2, posy - b.size() / 2);

                                    List<Point> d = null;
                                    if (g.have_type(g2_check_point.type.reflect))
                                    {
                                        d = g.point_dic[g2_check_point.type.reflect];
                                        Point point = d[0];

                                        int nx = 0, ny = 0;
                                        for (int m = -1; m <= 1; m++)
                                        {
                                            for (int n = -1; n <= 1; n++)
                                            {
                                                if (m == 0 && n == 0)
                                                {
                                                    continue;
                                                }
                                                if (point.X * m + point.Y * n == 0)
                                                {
                                                    nx = m;
                                                    ny = n;
                                                }
                                            }
                                        }
                                        if (nx * b.move_x + ny * b.move_y == 0)
                                        {
                                            b.live = false;
                                            return;
                                        }
                                        if (nx * b.move_x + ny * b.move_y > 0)
                                        {
                                            nx = -nx;
                                            ny = -ny;
                                        }

                                        //R = I - 2(I·N)N / |N|^2
                                        int ntwo_IN = (b.move_x * nx + b.move_y * ny) * (-2);
                                        int N_length_2 = (nx * nx) + (ny * ny);
                                        Point r0 = new Point(ntwo_IN * nx / N_length_2, ntwo_IN * ny / N_length_2);
                                        Point R = new Point(b.move_x + r0.X, b.move_y + r0.Y);
                                        b.move_x = (int)R.X;
                                        b.move_y = (int)R.Y;
                                    }
                                    if (g.have_type(g2_check_point.type.in_out) && balls.Count < 100)
                                    {
                                        foreach (Point point in c.out_dirs)
                                        {
                                            add_movement("shootx " + ((int)gs.g2_ball_id) + " " + (int)p.X + " " + (int)p.Y);
                                            temp_balls.Add(gs.g2_get_ball
                                                (new Point(posx - 0.5 * c.all_size, posy - 0.5 * c.all_size), 
                                                (int)point.X, (int)point.Y, c.all_size));
                                        }
                                        b.live = false;
                                    }
                                }
                            }
                        }
                    }
                    
                }

                Point? curr = ball_all_at(b);
                if (curr != null)
                {
                    Point curr0 = (Point)curr;
                    g2_cell c1 = area[(int)curr0.X, (int)curr0.Y];
                    g2_cell c2 = area[(int)(curr0.X - c1.part_of_x), (int)(curr0.Y - c1.part_of_y)];


                    if (!c2.entered)
                    {
                        c2.color_change = true;
                        c2.entered = true;
                        grid_changed = true;
                    }
                    
                    if (condition.is_type(g2_complete_condition.type.normal))
                    {
                        if (c2.is_type(g2_cell.type.end))
                        {
                            condition.t[g2_complete_condition.type.normal] = true;
                            add_movement("end");
                        }
                    }
                    //energy判断

                    game_win = true;
                    foreach(KeyValuePair<g2_complete_condition.type, bool> kp in condition.t)
                    {
                        if (!kp.Value)
                        {
                            game_win = false;
                        }
                    }
                }
            }
        }
        [NonSerialized]
        Dictionary<string, g2_area> levels = new Dictionary<string, g2_area>();


        [Serializable]
        class g2_cell
        {
            //建议制作： 推箱子
            public enum type
            {
                super_wall = -2,
                wall = -1,
                empty = 0,
                start = 1,
                end = 2,
                safe = 3,

                reflecter = 11,
                round_reflecter = 12,
                in_out = 13,
                spinner = 14,
                portal = 15,

                x = 50,
                key = 51,
                door = 52,

                light = 1000,
                unlight = 1001,

                item = 9999,
                special = 10000,
                oblique = 10001,
            }

            public List<type> cell_type = new List<type>();



            public string portal;

            public string key;
            public string door;

            public Point dir;
            
            public bool color_change = false;
            public bool entered = false;
            public bool no_light_movement = false;

            public List<Point> in_dirs = new List<Point>();
            public List<Point> out_dirs = new List<Point>();

            public List<Point> oblique_dirs = new List<Point>();
            public bool oblique_switch = true;

            public g2_cell attach;

            public List<Tuple<int, int, int, int>> edit_chain = new List<Tuple<int, int, int, int>>();

            public bool not_first()
            {
                if (part_of_x == 0 && part_of_y == 0)
                {
                    return false;
                }
                return true;
            }

            public bool is_wall()
            {
                bool ret = false;
                if (cell_type.Contains(type.super_wall))
                {
                    ret = true;
                }
                if (cell_type.Contains(type.wall))
                {
                    ret = true;
                }
                return ret;
            }

            public bool is_enterable()
            {
                if (is_wall() &&
                    (is_type(type.in_out) || is_type(type.start)))
                {
                    return true;
                }
                if (is_wall())
                {
                    return false;
                }
                return true;
            }

            public bool is_type(type x)
            {
                if (x == type.empty && cell_type.Contains(type.x))
                {
                    return false;
                }
                if (x == type.empty && cell_type.Count == 0)
                {
                    return true;
                }
                if (cell_type.Contains(x))
                {
                    return true;
                }
                return false;
            }

            public bool only_type(type x)
            {
                if (x == type.x && is_type(type.x) && 
                    (cell_type.Contains(type.empty) || cell_type.Contains(type.safe)) &&
                    cell_type.Count <= 2)
                {
                    return true;
                }
                if (cell_type.Count == 1 && cell_type[0] == x)
                {
                    return true;
                }
                return false;
            }

            public bool have_but_not_only_type(type x)
            {
                if (cell_type.Count > 1)
                {
                    return true;
                }
                if (cell_type.Count == 1)
                {
                    return !is_type(x);
                }
                return false;
            }

            public void delete_type(type x)
            {
                List<type> n = new List<type>();
                foreach(type t in cell_type)
                {
                    if (x != t)
                    {
                        n.Add(t);
                    }
                }
                cell_type = n;
            }

            public void add_type(type x)
            {
                if (cell_type.Contains(x))
                {

                }
                else
                {
                    cell_type.Add(x);
                }
            }

            public bool contains_dir(Point p)
            {
                if(dir == p)
                {
                    return true;
                }
                foreach(Point p1 in in_dirs)
                {
                    if (p1 == p)
                    {
                        return true;
                    }
                }
                foreach (Point p1 in out_dirs)
                {
                    if (p1 == p)
                    {
                        return true;
                    }
                }
                foreach (Point p1 in oblique_dirs)
                {
                    if (p1 == p)
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool oblique_dirs_contains_dir(Point p)
            {
                foreach (Point p1 in oblique_dirs)
                {
                    if (p1 == p)
                    {
                        return true;
                    }
                }
                return false;
            }

            public int part_of_x = 0;
            public int part_of_y = 0;
            public int all_size = 1;

            public g2_cell(List<type> t, int px = 0, int py = 0, int size = 1)
            {
                cell_type = t;
                part_of_x = px;
                part_of_y = py;
                all_size = size;
            }

            public g2_cell()
            {
                cell_type = new List<type>();
            }

            public void copy_attr_by(g2_cell x)
            {
                portal = x.portal;
                key = x.key;
                door = x.door;
                dir = x.dir;

                in_dirs.Clear();
                foreach(Point p in x.in_dirs)
                {
                    in_dirs.Add(p);
                }
                out_dirs.Clear();
                foreach (Point p in x.out_dirs)
                {
                    out_dirs.Add(p);
                }

                edit_chain.Clear();
                foreach(Tuple<int, int, int, int> t in x.edit_chain)
                {
                    edit_chain.Add(t);
                }

                if (x.attach != null)
                {
                    attach.copy_attr_by(x.attach);
                }
            }
        }

        [Serializable]
        public class g2_ball
        {
            public decimal id = 0;

            //public double size = 0.2;
            public double base_size = 0.2;
            public double size_mul = 1;
            public double size()
            {
                return base_size * size_mul;
            }
            
            public Point ball_pos = new Point(-1, -1);
            public bool live = true;

            public double ball_speed = 0.03;
            public int move_x = 0;
            public int move_y = 0;

            public g2_ball(Point pos, int mx, int my, int s)
            {
                ball_pos = pos;
                move_x = mx;
                move_y = my;
                size_mul = s;
            }
        }

        [Serializable]
        class g2_check_point
        {
            public enum type
            {
                death = -1,
                empty = 0,
                end = 2,

                reflect = 11,
                round_reflect = 12,
                in_out = 13,
                spinner = 14,
            }
            public Dictionary<type, List<Point>> point_dic;
            public Dictionary<decimal, bool> ball_reactioned = new Dictionary<decimal, bool>();

            public bool have_type(type t)
            {
                if (point_dic.ContainsKey(t))
                {
                    return true;
                }
                return false;
            }

            public g2_check_point(Dictionary<type, List<Point>> dic)
            {
                point_dic = dic;
            }

            public void delete_type(type t)
            {
                while (point_dic.ContainsKey(t))
                {
                    point_dic.Remove(t);
                }
            }

            public static void add_type(ref g2_check_point c, type t, List<Point> ps)
            {
                if (c == null)
                {
                    Dictionary<type, List<Point>> d = new Dictionary<type, List<Point>>();
                    d.Add(t, ps);
                    c = new g2_check_point(d);
                }
                else
                {
                    if (c.have_type(t))
                    {
                        if (ps == null)
                        {
                            return;
                        }
                        foreach (Point p in ps)
                        {
                            bool add = true;
                            if (c.point_dic[t] == null)
                            {
                                c.point_dic[t] = new List<Point>();
                            }
                            foreach (Point dp in c.point_dic[t])
                            {
                                if (dp.X == p.X && dp.Y == p.Y)
                                {
                                    add = false;
                                    break;
                                }
                            }
                            if (add)
                            {
                                c.point_dic[t].Add(p);
                            }
                        }
                    }
                    else
                    {
                        c.point_dic.Add(t, ps);
                    }
                }
            }
        }


        private void g2_shoot_key(object sender, MouseButtonEventArgs e)
        {
            g2_area a = g2_current;
            g2_shoot();
        }

        private void g2_create_close(object sender, MouseButtonEventArgs e)
        {
            Grid g2_game_create_container = (Grid)vm_elems["g2_game_create_container"];
            g2_game_create_container.Visibility = Visibility.Hidden;
        }

        private void g2_amount_inf_btn(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)vm_elems["g2_game_amount_tb"];
            tb.Text = "1000";
        }

        private void g2_amount_close(object sender, MouseButtonEventArgs e)
        {
            Grid g2_game_amount_container = (Grid)vm_elems["g2_game_amount_container"];
            g2_game_amount_container.Visibility = Visibility.Hidden;

            g2_area a = g2_current;
            inputer ip = inputers["g2_amount"];
            if (ip.curr_state == inputer.state.error)
            {
                a.items[a.edit.item_left_selected_index].i = 1;
            }
            else
            {
                a.items[a.edit.item_left_selected_index].i = a.edit.curr_amount;
            }
        }
        private void g2_create_hide(object sender, RoutedEventArgs e)
        {
            Rectangle reshower = (Rectangle)vm_elems["g2_game_create_reshower"];
            reshower.Visibility = Visibility.Visible;

            Grid g2_game_create_grid = (Grid)vm_elems["g2_game_create_grid"];
            g2_game_create_grid.Visibility = Visibility.Hidden;
        }
        private void g2_create_reshow(object sender, MouseButtonEventArgs e)
        {
            Rectangle reshower = (Rectangle)vm_elems["g2_game_create_reshower"];
            reshower.Visibility = Visibility.Hidden;
            CheckBox c = (CheckBox)vm_elems["g2_game_create_cb"];
            c.IsChecked = false;

            Grid g2_game_create_grid = (Grid)vm_elems["g2_game_create_grid"];
            g2_game_create_grid.Visibility = Visibility.Visible;
        }

        private void g2_shoot()
        {
            g2_area a = g2_current;

            bool shot = false;
            for (int i = 0; i < a.width; i++)
            {
                for (int j = 0; j < a.height; j++)
                {
                    g2_cell c = a.area[i, j];

                    if (c.is_type(g2_cell.type.start) &&
                        !c.not_first() && 
                        a.energy == a.need_energy &&
                        a.balls.Count < 100)
                    {
                        shot = true;

                        //TODO 把所有shoot合并成一条
                        a.add_movement("shoot " + ((int)g2_ball_id) + " " + i + " " + j);
                        a.balls.Add(g2_get_ball(new Point(i, j), (int)c.dir.X, (int)c.dir.Y, c.all_size));
                    }
                }
            }
            if (shot)
            {
                a.energy = 0;
            }
        }

        private void Classic_menu_background_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm_elems["g2_classic_menu"].Visibility = Visibility.Hidden;
        }




        private SolidColorBrush wall_color()
        {
            return getSCB(Color.FromRgb(0, 0, 0));
        }

        bool g2_gamestart = false;
        g2_area g2_current = null;
        private void g2_start(g2_area a, bool new_game = true)
        {
            g2_gamestart = true;
            g2_current = a;

            g2_area_size_btn_refresh();

            if (new_game)
            {
                game_key_name("g2_game_eoption_2");
                Rectangle geo2 = (Rectangle)vm_elems["g2_game_eoption_2_bg"];
                geo2.Fill = color_mul(geo2.Fill, 1.25);
            }

            Grid gc = (Grid)vm_elems["g2_game_container"];
            gc.Visibility = Visibility.Visible;

            Grid ggc = (Grid)vm_elems["g2_game_grid_container"];
            Grid g = (Grid)vm_elems["g2_game_object_grid"];
            Grid ge = (Grid)vm_elems["g2_game_edit_grid"];
            Grid gb = (Grid)vm_elems["g2_game_ball_grid"];
            g.Children.Clear();
            ge.Children.Clear();

            double xs = (ggc.Width - 20) / a.width;
            double ys = (ggc.Height - 20) / a.height;
            double size = Math.Min(xs, ys);
            g.Width = a.width * size;
            g.Height = a.height * size;
            ge.Width = g.Width;
            ge.Height = g.Height;
            gb.Width = g.Width;
            gb.Height = g.Height;
            a.cell_size = size;

            g.ColumnDefinitions.Clear();
            g.RowDefinitions.Clear();
            ge.ColumnDefinitions.Clear();
            ge.RowDefinitions.Clear();
            for (int i = 0; i < a.width; i++)
            {
                g.ColumnDefinitions.Add(new ColumnDefinition());
                ge.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < a.height; j++)
            {
                g.RowDefinitions.Add(new RowDefinition());
                ge.RowDefinitions.Add(new RowDefinition());
            }

            Rectangle g2_game_edit_shower = new Rectangle
            {
                Name = "g2_game_edit_shower",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(100, 255, 255, 0)),
                Visibility = Visibility.Hidden
            };
            ge.Children.Add(g2_game_edit_shower);
            vm_assign(g2_game_edit_shower);
            
            for (int i = 0; i < a.width; i++)
            {
                for (int j = 0; j < a.height; j++)
                {
                    g2_cell c = a.area[i, j];
                    Rectangle g2_game_grid_stroke = new Rectangle
                    {
                        Name = "g2_game_object_grid_stroke_" + i.ToString() + "_" + j.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(75, 75, 75)),
                        Stroke = getSCB(Color.FromRgb(180, 180, 180)),
                        StrokeThickness = size * 0.02 * c.all_size,
                        Tag = 0
                    };
                    Grid.SetRow(g2_game_grid_stroke, j);
                    Grid.SetRowSpan(g2_game_grid_stroke, c.all_size);
                    Grid.SetColumn(g2_game_grid_stroke, i);
                    Grid.SetColumnSpan(g2_game_grid_stroke, c.all_size);

                    g2_game_grid_stroke.MouseEnter += rectangle_cover_enter;
                    g2_game_grid_stroke.MouseLeave += rectangle_cover_leave;
                    g2_game_grid_stroke.MouseLeftButtonDown += rectangle_cover_down;
                    g2_game_grid_stroke.MouseLeftButtonUp += rectangle_cover_up;
                    g2_game_grid_stroke.MouseMove += rectangle_cover_move;

                    g.Children.Add(g2_game_grid_stroke);
                    vm_assign(g2_game_grid_stroke);
                }
            }

            if (new_game)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        string si = i.ToString();
                        string sj = j.ToString();
                        vm_elems["g2_game_item_container_" + si + "_" + sj].Visibility = Visibility.Hidden;
                    }
                }
                foreach (KeyValuePair<int, g2_area.item> kp in a.items)
                {
                    int i = kp.Key % 2;
                    int j = kp.Key / 2;
                    string si = i.ToString();
                    string sj = j.ToString();
                    vm_elems["g2_game_item_container_" + si + "_" + sj].Visibility = Visibility.Visible;

                    Rectangle g2_game_item_select_bg = (Rectangle)vm_elems["g2_game_item_select_" + si + "_" + sj + "_bg"];
                    g2_game_item_select_bg.Fill = getSCB(Color.FromRgb(75, 100, 125));

                    Grid r = (Grid)vm_elems["g2_game_item_" + si + "_" + sj + "_item"];
                    r = g2_draw_cell(kp.Value.c, r.Name, m.vm_grid.Height * 0.05, 1, false);

                    TextBlock t = (TextBlock)vm_elems["g2_game_item_amount_" + si + "_" + sj];
                    if (kp.Value.i < 1000)
                    {
                        t.Text = "×" + kp.Value.i.ToString();
                    }
                    else
                    {
                        t.Text = "×∞";
                    }

                }
            }
            
            a.create_check_point();
            g2_tick();
        }

        private void g2_use_key(string key)
        {
            g2_area a = g2_current;
            for (int i = 0; i < a.width; i++)
            {
                for (int j = 0; j < a.height; j++)
                {
                    g2_cell c = a.area[i, j];
                    string si = i.ToString();
                    string sj = j.ToString();
                    if (c.is_type(g2_cell.type.door))
                    {
                        foreach(char ch in key)
                        {
                            string s = ch.ToString().ToUpper();
                            for (int k = 0; k < c.door.Length; k++)
                            {
                                if (c.door[k] == s[0])
                                {
                                    a.add_movement("unlock " + c.door + " " + si + " " + sj);
                                    c.door = c.door.Remove(k, 1);
                                    break;
                                }
                            }
                        }
                        if (c.door == "")
                        {
                            c.delete_type(g2_cell.type.door);
                            c.delete_type(g2_cell.type.wall);
                            a.check_point_clear(i, j);
                            if (c.is_type(g2_cell.type.key))
                            {
                                c.add_type(g2_cell.type.safe);
                            }
                        }
                        else
                        {
                            c.add_type(g2_cell.type.door);
                            c.add_type(g2_cell.type.wall);
                            if (c.is_type(g2_cell.type.safe))
                            {
                                c.delete_type(g2_cell.type.safe);
                            }
                        }
                    }
                    if (c.is_type(g2_cell.type.key))
                    {
                        if (c.key == "")
                        {
                            c.delete_type(g2_cell.type.key);
                            c.delete_type(g2_cell.type.safe);
                        }
                        else
                        {
                            c.add_type(g2_cell.type.key);
                        }
                    }
                    if (!c.not_first())
                    {
                        TextBlock t = (TextBlock)vm_elems["g2_game_grid_itemcell_" + i.ToString() + "_" + j.ToString() + "_kdtext"];
                        t.Text = "";
                        if (c.only_type(g2_cell.type.x))
                        {
                            t.Text = "×";
                        }
                        if (c.is_type(g2_cell.type.door))
                        {
                            t.Text += c.door;
                        }
                        if (c.is_type(g2_cell.type.key))
                        {
                            t.Text += c.key;
                        }
                    }
                }
            }
            a.grid_changed = true;
            a.item_changed = true;
            a.wall_changed = true;
        }

        private void g2_wall_update()
        {
            g2_area a = g2_current;
            for (int i = 0; i < a.width; i++)
            {
                for (int j = 0; j < a.height; j++)
                {
                    a.get_wall_type(i, j);
                }
            }

            for (int i = 0; i < a.width; i++)
            {
                for(int j = 0; j < a.height; j++)
                {
                    int w = a.get_wall_type(i, j);

                    //		Key	"g2_game_grid_itemcell_0_0_wgrid_0_0_wall_0"	string
                    g2_cell c = a.area[i, j];
                    int fx = i - c.part_of_x;
                    int fy = j - c.part_of_y;

                    string basename = "g2_game_grid_itemcell_" + fx + "_" + fy + "_wgrid_" + 
                        c.part_of_x + "_" + c.part_of_y + "_wall_";
                    if (!vm_elems.ContainsKey(basename + "0"))
                    {
                        continue;
                    }
                    FrameworkElement f0 = vm_elems[basename + "0"];
                    FrameworkElement f1 = vm_elems[basename + "1"];
                    FrameworkElement f2 = vm_elems[basename + "2"];
                    FrameworkElement f3 = vm_elems[basename + "3"];
                    FrameworkElement f4 = vm_elems[basename + "4"];
                    f0.Visibility = Visibility.Hidden;
                    f1.Visibility = Visibility.Hidden;
                    f2.Visibility = Visibility.Hidden;
                    f3.Visibility = Visibility.Hidden;
                    f4.Visibility = Visibility.Hidden;
                    if (w == -1)
                    {
                        continue;
                    }
                    else
                    {
                        f0.Visibility = Visibility.Visible;

                        int wa = w & 8;
                        int wb = w & 4;
                        int wc = w & 2;
                        int wd = w & 1;
                        

                        if (wa == 8)
                        {
                            f1.Visibility = Visibility.Visible;
                        }
                        if (wb == 4)
                        {
                            f2.Visibility = Visibility.Visible;
                        }
                        if (wc == 2)
                        {
                            f3.Visibility = Visibility.Visible;
                        }
                        if (wd == 1)
                        {
                            f4.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        bool g2_edit_hold = false;
        Point g2_edit_hold_ps;
        Point g2_edit_hold_pe;
        int g2_edit_x;
        int g2_edit_y;
        int g2_edit_ax;
        int g2_edit_ay;
        int g2_edit_s;
        private void g2_draw_shower()
        {
            //g2_game_edit_shower.Name = "g2_game_edit_shower";
            g2_area a = g2_current;
            if (!vm_elems.ContainsKey("g2_game_edit_shower"))
            {
                return;
            }
            Rectangle shower = (Rectangle)vm_elems["g2_game_edit_shower"];
            
            shower.Visibility = Visibility.Hidden;

            if (!g2_edit_size_corr)
            {
                return;
            }

            IInputElement input = Mouse.DirectlyOver;
            if (input != null)
            {
                FrameworkElement f = input as FrameworkElement;

                //"g2_game_object_grid_edit_" + si + "_" + sj;
                if (f.Name.Contains("g2_game_object_grid_edit"))
                {
                    string[] strs = f.Name.Split('_');

                    string si = strs[5];
                    string sj = strs[6];

                    int i = Convert.ToInt32(si);
                    int j = Convert.ToInt32(sj);

                    if (((int)f.Tag) / 2 == 0)
                    {
                        f.Tag = (int)f.Tag + 2;
                    }


                    if (g2_edit_size + i > a.width || g2_edit_size + j > a.height)
                    {
                        return;
                    }
                    shower.Visibility = Visibility.Visible;
                    shower.Fill = getSCB(Color.FromArgb(100, 0, 0, 0));
                    shower.Stroke = getSCB(Color.FromArgb(255, 0, 255, 255));
                    shower.StrokeThickness = 0.02 * a.cell_size * g2_edit_size;
                    Grid.SetRow(shower, j);
                    Grid.SetColumn(shower, i);
                    Grid.SetRowSpan(shower, g2_edit_size);
                    Grid.SetColumnSpan(shower, g2_edit_size);
                    g2_edit_x = i;
                    g2_edit_y = j;
                    g2_edit_ax = 1;
                    g2_edit_ay = 1;
                    g2_edit_s = g2_edit_size;

                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        shower.Fill = getSCB(Color.FromArgb(150, 0, 0, 0));
                        shower.Stroke = getSCB(Color.FromArgb(255, 0, 255, 0));
                        if (!g2_edit_hold)
                        {
                            g2_edit_hold = true;
                            g2_edit_hold_ps = new Point(i, j);
                            g2_edit_hold_pe = new Point(i, j);
                        }
                    }
                    else
                    {
                        g2_edit_hold = false;
                    }

                    if (g2_edit_hold)
                    {
                        g2_edit_hold_pe = new Point(i, j);

                        int i1 = (int)g2_edit_hold_ps.X;
                        int i2 = (int)g2_edit_hold_pe.X;
                        int j1 = (int)g2_edit_hold_ps.Y;
                        int j2 = (int)g2_edit_hold_pe.Y;

                        int sx = Math.Min(i1, i2);
                        int sy = Math.Min(j1, j2);
                        int ex = Math.Max(i1, i2);
                        int ey = Math.Max(j1, j2);

                        if (g2_edit_size + ex > a.width || g2_edit_size + ey > a.height)
                        {
                            return;
                        }

                        int lx = ex - sx + 1;
                        int ly = ey - sy + 1;

                        //s = 2
                        //lx = 1  ax = 2
                        //lx = 2  ax = 2
                        //lx = 3  ax = 4
                        //lx = 4  ax = 4

                        int ax = lx / g2_edit_size;
                        if (lx % g2_edit_size > 0)
                        {
                            ax++;
                        }
                        int ay = ly / g2_edit_size;
                        if (ly % g2_edit_size > 0)
                        {
                            ay++;
                        }

                        g2_edit_x = sx;
                        g2_edit_y = sy;
                        g2_edit_ax = ax;
                        g2_edit_ay = ay;

                        Grid.SetRow(shower, sy);
                        Grid.SetColumn(shower, sx);
                        Grid.SetRowSpan(shower, ay * g2_edit_size);
                        Grid.SetColumnSpan(shower, ax * g2_edit_size);
                    }
                }
                else
                {
                    g2_edit_hold = false;
                }
            }
            else
            {
                g2_edit_hold = false;
            }
        }

        bool g2_edit_size_corr = false;
        int g2_edit_size = 1;
        private void g2_tick()
        {
            Rectangle rgrey = (Rectangle)vm_elems["g2_game_option_restart_grey"];
            Rectangle rbg = (Rectangle)vm_elems["g2_game_option_restart_bg"];
            rgrey.Fill = rbg.Fill;


            if (g2_gamestart)
            {
                g2_edit_tick();

                Grid g = (Grid)vm_elems["g2_game_object_grid"];
                Grid ge = (Grid)vm_elems["g2_game_edit_grid"];
                Grid gb = (Grid)vm_elems["g2_game_ball_grid"];
                g2_area a = g2_current;
                a.tick_value++;

                TextBlock t1 = (TextBlock)vm_elems["g2_game_text1_text"];
                t1.Text = a.description;

                for (int i = 0; i < 3; i++)
                {
                    TextBlock text = (TextBlock)vm_elems["g2_game_text2_text" + i.ToString()];
                    text.Text = "";
                    text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }

                //目标判断
                {
                    if (a.condition.is_type(g2_complete_condition.type.count))
                    {
                        if (a.enter_count >= a.condition.count_req)
                        {
                            a.condition.t[g2_complete_condition.type.count] = true;
                        }
                        else
                        {
                            a.condition.t[g2_complete_condition.type.count] = false;
                        }
                    }
                    if (a.condition.is_type(g2_complete_condition.type.ball))
                    {
                        if (a.balls.Count >= a.condition.ball_req)
                        {
                            a.condition.t[g2_complete_condition.type.ball] = true;
                        }
                        else
                        {
                            a.condition.t[g2_complete_condition.type.ball] = false;
                        }
                    }
                }


                int t_count = 0;
                TextBlock t2 = null;
                if (a.condition.is_type(g2_complete_condition.type.normal))
                {
                    t2 = (TextBlock)vm_elems["g2_game_text2_text" + t_count.ToString()];
                    if (a.condition.t[g2_complete_condition.type.normal])
                    {
                        t2.Foreground = getSCB(Color.FromRgb(0, 255, 0));
                    }
                    t2.Text = "到达终点";
                    t_count++;
                }
                if (a.condition.is_type(g2_complete_condition.type.count))
                {
                    t2 = (TextBlock)vm_elems["g2_game_text2_text" + t_count.ToString()];
                    if (a.condition.t[g2_complete_condition.type.count])
                    {
                        t2.Foreground = getSCB(Color.FromRgb(0, 255, 0));
                    }
                    t2.Text = "点亮格子 (" + a.enter_count + " / " + a.condition.count_req + ")";
                    t_count++;
                }
                if (a.condition.is_type(g2_complete_condition.type.ball))
                {
                    t2 = (TextBlock)vm_elems["g2_game_text2_text" + t_count.ToString()];
                    if (a.condition.t[g2_complete_condition.type.ball])
                    {
                        t2.Foreground = getSCB(Color.FromRgb(0, 255, 0));
                    }
                    t2.Text = "拥有球 (" + a.balls.Count + " / " + a.condition.ball_req + ")";
                    t_count++;
                }

                if (Keyboard.IsKeyDown(Key.Space))
                {
                    g2_shoot();
                }

                Grid egrid = (Grid)vm_elems["g2_game_edit_grid"];
                Grid eogrid = (Grid)vm_elems["g2_game_eoptions_grid"];
                Grid segrid = (Grid)vm_elems["g2_game_option_startexit_grid"];
                TextBlock setext = (TextBlock)vm_elems["g2_game_option_startexit_text"];
                if (a.editing)
                {
                    egrid.Visibility = Visibility.Visible;
                    eogrid.Visibility = Visibility.Visible;
                    segrid.Visibility = Visibility.Visible;
                    setext.Text = "Start";
                }
                else if (a.testing)
                {
                    egrid.Visibility = Visibility.Hidden;
                    eogrid.Visibility = Visibility.Hidden;
                    segrid.Visibility = Visibility.Visible;
                    setext.Text = "Exit";
                }
                else
                {
                    egrid.Visibility = Visibility.Hidden;
                    eogrid.Visibility = Visibility.Hidden;
                    segrid.Visibility = Visibility.Hidden;
                }
                if (s_ticker("g2_item", 0.1))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            string si = i.ToString();
                            string sj = j.ToString();

                            Rectangle g2_game_item_edit = (Rectangle)vm_elems["g2_game_item_edit_" + si + "_" + sj];
                            if (a.editing)
                            {
                                g2_game_item_edit.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                g2_game_item_edit.Visibility = Visibility.Hidden;
                            }
                            vm_elems["g2_game_item_container_" + si + "_" + sj].Visibility = Visibility.Hidden;
                        }
                    }

                    foreach (KeyValuePair<int, g2_area.item> kp in a.items)
                    {
                        int i = kp.Key % 2;
                        int j = kp.Key / 2;
                        string si = i.ToString();
                        string sj = j.ToString();
                        vm_elems["g2_game_item_container_" + si + "_" + sj].Visibility = Visibility.Visible;

                        Grid r = (Grid)vm_elems["g2_game_item_" + si + "_" + sj + "_item"];
                        r = g2_draw_cell(kp.Value.c, r.Name, m.vm_grid.Height * 0.05, 1, false);

                        TextBlock t = (TextBlock)vm_elems["g2_game_item_amount_" + si + "_" + sj];
                        if (kp.Value.i < 1000)
                        {
                            t.Text = "×" + kp.Value.i.ToString();
                        }
                        else
                        {
                            t.Text = "×∞";
                        }
                    }
                }


                //"g2_game_option_create_size_textbox";
                g2_edit_size_corr = false;
                TextBox gocs_textbox = (TextBox)vm_elems["g2_game_option_create_size_textbox"];
                string gocs_t = gocs_textbox.Text;
                int gocs_i = -1;
                int.TryParse(gocs_t, out gocs_i);
                gocs_textbox.Background = getSCB(Color.FromRgb(255, 0, 0));
                gocs_textbox.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                if (gocs_i > 0)
                {
                    gocs_textbox.Background = getSCB(Color.FromRgb(0, 255, 0));
                    gocs_textbox.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    g2_edit_size_corr = true;
                    g2_edit_size = gocs_i;
                }
                if (gocs_i > Math.Min(a.width, a.height))
                {
                    g2_edit_size_corr = false;
                    gocs_textbox.Background = getSCB(Color.FromRgb(255, 127, 0));
                    gocs_textbox.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
                g2_draw_shower();

                foreach(g2_check_point gcp in a.check)
                {
                    if(gcp != null)
                    {
                        gcp.ball_reactioned.Clear();
                    }
                }

                //能量处理
                double e_mul = 1;
                if (a.balls.Count == 0)
                {
                    e_mul = 100;
                }

                double2 energy_gap = a.need_energy - a.energy;
                a.energy += double2.Min(energy_gap, e_mul * a.need_energy * (0.01 / a.need_time));

                double2 percent = a.energy / a.need_energy;

                Rectangle f1 = (Rectangle)vm_elems["g2_game_energy_bg"];
                Rectangle f2 = (Rectangle)vm_elems["g2_game_energy_bg2"];
                TextBlock etext = (TextBlock)vm_elems["g2_game_energy_text"];
                f2.Width = f1.Width * percent.d;
                if (percent.d == 1)
                {
                    f2.Fill = getSCB(Color.FromRgb(255, 255, 255));
                }
                else
                {
                    f2.Fill = getSCB(Color.FromRgb(230, 230, 230));
                }

                etext.Text = "能量：" + number_format(a.energy) + " / " + number_format(a.need_energy);
                if (percent.d == 1)
                {
                    etext.Text += " (点击发射!)";
                }

                double size = g2_current.cell_size;
                List<g2_ball> temp = new List<g2_ball>();
                foreach (g2_ball b in a.balls)
                {
                    a.move_and_check(b);
                    Rectangle r = (Rectangle)vm_elems["g2_ball_" + b.id.ToString()];
                    if (b.live)
                    {
                        r.Margin = new Thickness(b.ball_pos.X * size, b.ball_pos.Y * size, 0, 0);
                    }
                    else
                    {
                        g2_balls.Remove(b.id);
                        vm_elems.Remove("g2_ball_" + b.id.ToString());
                        gb.Children.Remove(r);
                        temp.Add(b);
                    }

                    Point? b_pos = a.ball_all_at(b);
                    if (b_pos != null)
                    {
                        Point p = (Point)b_pos;
                        g2_cell c = a.area[(int)p.X, (int)p.Y];
                        if (c.is_type(g2_cell.type.key) && c.key != "")
                        {
                            g2_use_key(c.key);
                            a.add_movement("key " + c.key + " " + (int)p.X + " " + (int)p.Y);
                            c.key = "";
                            c.delete_type(g2_cell.type.key);
                            c.delete_type(g2_cell.type.safe);
                        }

                    }
                    if (a.game_win)
                    {
                        g2_complete(a.name);
                    }
                }
                foreach(g2_ball b in temp)
                {
                    a.balls.Remove(b);
                }
                foreach(g2_ball b in a.temp_balls)
                {
                    a.balls.Add(b);
                }
                a.temp_balls.Clear();
                
                foreach (KeyValuePair<int, g2_area.item> kp in a.items)
                {
                    int index = kp.Key;

                    int i = index % 2;
                    int j = index / 2;
                    string si = i.ToString();
                    string sj = j.ToString();

                    TextBlock t = (TextBlock)vm_elems["g2_game_item_amount_" + si + "_" + sj];
                    t.Text = "×" + kp.Value.i.ToString();
                    if (kp.Value.i < 1000)
                    {
                        t.Text = "×" + kp.Value.i.ToString();
                    }
                    else
                    {
                        t.Text = "× ∞";
                    }
                }

                if (a.grid_changed)
                {
                    a.enter_count = 0;
                    for(int i = 0; i < a.width; i++)
                    {
                        for(int j = 0; j < a.height; j++)
                        {
                            g2_cell c = a.area[i, j];
                            string si = i.ToString();
                            string sj = j.ToString();
                            Rectangle g2_game_grid_stroke = (Rectangle)vm_elems["g2_game_object_grid_stroke_" + si + "_" + sj];
                            if (c.all_size > 1)
                            {
                                int fx = i - c.part_of_x;
                                int fy = j - c.part_of_y;
                                for(int p1 = 0; p1 < c.all_size; p1++)
                                {
                                    for (int p2 = 0; p2 < c.all_size; p2++)
                                    {
                                        string sp1 = (fx + p1).ToString();
                                        string sp2 = (fy + p2).ToString();
                                        string strokep_name = "g2_game_object_grid_stroke_" + sp1 + "_" + sp2;

                                        if (!a.contain_xy(fx + p1, fy + p2))
                                        {
                                            if (c.all_size > 1)
                                            {
                                                p1 = 0;
                                                p2 = -1;
                                            }
                                            c.all_size = 1;
                                            c.part_of_x = 0;
                                            c.part_of_y = 0;
                                            c.color_change = true;

                                            continue;
                                        }

                                        Rectangle g2_game_grid_strokep = (Rectangle)vm_elems[strokep_name];
                                        if (p1 == 0 && p2 == 0)
                                        {
                                            g2_game_grid_strokep.Visibility = 0;
                                            Grid.SetRowSpan(g2_game_grid_strokep, c.all_size);
                                            Grid.SetColumnSpan(g2_game_grid_strokep, c.all_size);
                                        }
                                        else
                                        {
                                            g2_game_grid_strokep.Visibility = Visibility.Hidden;
                                        }
                                        a.area[fx + p1, fy + p2].cell_type = c.cell_type;
                                        a.area[fx + p1, fy + p2].portal = c.portal;
                                        a.area[fx + p1, fy + p2].key = c.key;
                                        a.area[fx + p1, fy + p2].door = c.door;
                                        a.area[fx + p1, fy + p2].dir = c.dir;
                                        a.area[fx + p1, fy + p2].in_dirs = c.in_dirs;
                                        a.area[fx + p1, fy + p2].out_dirs = c.out_dirs;
                                        a.area[fx + p1, fy + p2].color_change = true;

                                        a.area[fx + p1, fy + p2].all_size = c.all_size;
                                        a.area[fx + p1, fy + p2].part_of_x = p1;
                                        a.area[fx + p1, fy + p2].part_of_y = p2;

                                    }
                                }
                            }
                            g2_game_grid_stroke.StrokeThickness = size * 0.02 * c.all_size;
                            g2_game_grid_stroke.Visibility = Visibility.Hidden;
                            Grid.SetRow(g2_game_grid_stroke, j);
                            Grid.SetRowSpan(g2_game_grid_stroke, c.all_size);
                            Grid.SetColumn(g2_game_grid_stroke, i);
                            Grid.SetColumnSpan(g2_game_grid_stroke, c.all_size);
                            if (!c.not_first())
                            {
                                g2_game_grid_stroke.Visibility = Visibility.Visible;
                                if (c.entered)
                                {
                                    a.enter_count += c.all_size * c.all_size;
                                }
                            }

                            string editname = "g2_game_object_grid_edit_" + si + "_" + sj;
                            Rectangle g2_edit = new Rectangle();
                            if (vm_elems.ContainsKey(editname))
                            {
                                g2_edit = (Rectangle)vm_elems[editname];
                            }
                            g2_edit.Name = editname;
                            g2_edit.HorizontalAlignment = HorizontalAlignment.Stretch;
                            g2_edit.VerticalAlignment = VerticalAlignment.Stretch;
                            g2_edit.Fill = getSCB(Color.FromArgb(0, 0, 0, 0));
                            vm_set_lbtn(g2_edit);
                            if (!ge.Children.Contains(g2_edit))
                            {
                                ge.Children.Add(g2_edit);
                            }
                            vm_assign(g2_edit);

                            g2_edit.Visibility = Visibility.Hidden;
                            Grid.SetRow(g2_edit, j);
                            Grid.SetRowSpan(g2_edit, 1);
                            Grid.SetColumn(g2_edit, i);
                            Grid.SetColumnSpan(g2_edit, 1);
                            if (a.editing)
                            {
                                g2_edit.Visibility = Visibility.Visible;
                            }

                            if (c.not_first())
                            {
                                continue;
                            }
                            
                            if (c.is_type(g2_cell.type.empty))
                            {
                                flag_remove(g2_game_grid_stroke, 10);
                            }
                            if (!c.is_type(g2_cell.type.empty))
                            {
                                flag_add(g2_game_grid_stroke, 10);
                            }
                            

                            {
                                c.color_change = false;
                                bool po = c.entered;

                                if (po && flag_have(g2_game_grid_stroke, 15))
                                {

                                }
                                else if (!po && !flag_have(g2_game_grid_stroke, 15))
                                {

                                }
                                else
                                {
                                    if (po)
                                    {
                                        flag_add(g2_game_grid_stroke, 15);
                                    }
                                    else
                                    {
                                        flag_remove(g2_game_grid_stroke, 15);
                                    }

                                    if (c.entered && !c.no_light_movement)
                                    {
                                        a.add_movement("light " + i + " " + j);
                                    }
                                    g2_game_grid_stroke.Fill = color_mul(g2_game_grid_stroke.Fill, 2, po);
                                    g2_game_grid_stroke.Stroke = color_mul(g2_game_grid_stroke.Stroke, 1.4, po);

                                    if (!c.is_type(g2_cell.type.empty))
                                    {
                                        a.item_changed = true;
                                    }
                                }
                            }
                        }
                    }
                    a.grid_changed = false;
                }
                if (a.item_changed)
                {
                    for (int i = 0; i < a.width; i++)
                    {
                        for (int j = 0; j < a.height; j++)
                        {
                            g2_cell c = a.area[i, j];

                            string si = i.ToString();
                            string sj = j.ToString();


                            Grid g2_game_grid_itemcell = null;

                            g2_game_grid_itemcell = g2_draw_cell(c,
                               "g2_game_grid_itemcell_" + si + "_" + sj, a.cell_size * c.all_size, c.all_size, c.entered);
                            Grid.SetRow(g2_game_grid_itemcell, j);
                            Grid.SetRowSpan(g2_game_grid_itemcell, c.all_size);
                            Grid.SetColumn(g2_game_grid_itemcell, i);
                            Grid.SetColumnSpan(g2_game_grid_itemcell, c.all_size);
                            if (!g.Children.Contains(g2_game_grid_itemcell))
                            {
                                g.Children.Add(g2_game_grid_itemcell);
                            }
                            if (c.not_first())
                            {
                                g2_game_grid_itemcell.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                g2_game_grid_itemcell.Visibility = 0;
                            }
                        }
                    }
                    /*
                    g2_wall_update();
                    a.create_check_point();*/
                    a.item_changed = false;
                    a.wall_changed = true;
                    //g2_draw_checkpoint();
                }
                if (a.wall_changed)
                {
                    g2_wall_update();
                    a.create_check_point();
                    a.wall_changed = false;
                }
            }
        }

        private Grid g2_draw_cell(g2_cell item, string name, double size_mul, int size, bool entered)
        {
            g2_cell c = item;
            Grid g2_game_grid_itemcell = new Grid();
            if (vm_elems.ContainsKey(name))
            {
                g2_game_grid_itemcell = (Grid)vm_elems[name];
            }
            g2_game_grid_itemcell.Name = name;
            g2_game_grid_itemcell.Width = size_mul;
            g2_game_grid_itemcell.Height = size_mul;
            g2_game_grid_itemcell.HorizontalAlignment = HorizontalAlignment.Left;
            g2_game_grid_itemcell.VerticalAlignment = VerticalAlignment.Top;
            g2_game_grid_itemcell.Children.Clear();
            vm_assign(g2_game_grid_itemcell);

            
            Grid g2_game_grid_itemcell_wallgrid = new Grid();
            string ws = name + "_wallgrid";
            if (vm_elems.ContainsKey(ws))
            {
                g2_game_grid_itemcell_wallgrid = (Grid)vm_elems[ws];
            }
            g2_game_grid_itemcell_wallgrid.Name = ws;
            g2_game_grid_itemcell_wallgrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            g2_game_grid_itemcell_wallgrid.VerticalAlignment = VerticalAlignment.Stretch;
            g2_game_grid_itemcell_wallgrid.Children.Clear();
            g2_game_grid_itemcell_wallgrid.RowDefinitions.Clear();
            g2_game_grid_itemcell_wallgrid.ColumnDefinitions.Clear();
            for (int i = 0; i < size; i++)
            {
                g2_game_grid_itemcell_wallgrid.RowDefinitions.Add(new RowDefinition());
                g2_game_grid_itemcell_wallgrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            g2_game_grid_itemcell.Children.Add(g2_game_grid_itemcell_wallgrid);
            vm_assign(g2_game_grid_itemcell_wallgrid);

            Grid g2_game_grid_itemcell_basegrid = new Grid();
            string bs = name + "_basegrid";
            if (vm_elems.ContainsKey(bs))
            {
                g2_game_grid_itemcell_basegrid = (Grid)vm_elems[bs];
            }
            g2_game_grid_itemcell_basegrid.Name = bs;
            g2_game_grid_itemcell_basegrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            g2_game_grid_itemcell_basegrid.VerticalAlignment = VerticalAlignment.Stretch;
            g2_game_grid_itemcell_basegrid.Children.Clear();
            g2_game_grid_itemcell.Children.Add(g2_game_grid_itemcell_basegrid);
            vm_assign(g2_game_grid_itemcell_basegrid);

            Grid g2_game_grid_itemcell_kdgrid = new Grid();
            string ks = name + "_kdgrid";
            if (vm_elems.ContainsKey(ks))
            {
                g2_game_grid_itemcell_kdgrid = (Grid)vm_elems[ks];
            }
            g2_game_grid_itemcell_kdgrid.Name = ks;
            g2_game_grid_itemcell_kdgrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            g2_game_grid_itemcell_kdgrid.VerticalAlignment = VerticalAlignment.Stretch;
            g2_game_grid_itemcell_kdgrid.Children.Clear();
            g2_game_grid_itemcell.Children.Add(g2_game_grid_itemcell_kdgrid);
            vm_assign(g2_game_grid_itemcell_kdgrid);

            TextBlock g2_game_grid_itemcell_kdtext = new TextBlock();
            string kt = name + "_kdtext";
            if (vm_elems.ContainsKey(kt))
            {
                g2_game_grid_itemcell_kdtext = (TextBlock)vm_elems[kt];
            }
            g2_game_grid_itemcell_kdtext.Name = kt;
            g2_game_grid_itemcell_kdtext.HorizontalAlignment = HorizontalAlignment.Center;
            g2_game_grid_itemcell_kdtext.VerticalAlignment = VerticalAlignment.Center;
            g2_game_grid_itemcell_kdtext.Foreground = getSCB(Color.FromRgb(255, 255, 255));
            if (c.is_type(g2_cell.type.x))
            {
                g2_game_grid_itemcell_kdtext.Foreground = getSCB(Color.FromRgb(255, 255, 0));
            }
            g2_game_grid_itemcell_kdtext.FontSize = size_mul * 0.5;

            g2_game_grid_itemcell_kdtext.Text = "";
            if (c.only_type(g2_cell.type.x))
            {
                g2_game_grid_itemcell_kdtext.Text = "×";
            }
            if (c.is_type(g2_cell.type.door))
            {
                g2_game_grid_itemcell_kdtext.Text += c.door;
            }
            if (c.is_type(g2_cell.type.key))
            {
                g2_game_grid_itemcell_kdtext.Text += c.key;
            }
            g2_game_grid_itemcell_kdgrid.Children.Add(g2_game_grid_itemcell_kdtext);
            vm_assign(g2_game_grid_itemcell_kdtext);

            if (c.is_wall())
            {
                for(int i = 0; i < size; i++)
                {
                    for(int j = 0; j < size; j++)
                    {
                        Grid g2_game_grid_itemcell_wgrid = new Grid();
                        string wg = name + "_wgrid_" + i + "_" + j;
                        if (vm_elems.ContainsKey(wg))
                        {
                            g2_game_grid_itemcell_wgrid = (Grid)vm_elems[wg];
                        }
                        g2_game_grid_itemcell_wgrid.Name = wg;
                        g2_game_grid_itemcell_wgrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                        g2_game_grid_itemcell_wgrid.VerticalAlignment = VerticalAlignment.Stretch;
                        g2_game_grid_itemcell_wgrid.Children.Clear();
                        Grid.SetRow(g2_game_grid_itemcell_wgrid, j);
                        Grid.SetColumn(g2_game_grid_itemcell_wgrid, i);
                        g2_game_grid_itemcell_wallgrid.Children.Add(g2_game_grid_itemcell_wgrid);
                        vm_assign(g2_game_grid_itemcell_wgrid);

                        double cellsize = size_mul / size;

                        Rectangle[] g2_game_grid_itemcell_wall = new Rectangle[5];
                        for (int k = 0; k < g2_game_grid_itemcell_wall.Count(); k++)
                        {
                            g2_game_grid_itemcell_wall[k] = new Rectangle();
                            string wn = wg + "_wall_" + k.ToString();
                            if (vm_elems.ContainsKey(wn))
                            {
                                g2_game_grid_itemcell_wall[k] = (Rectangle)vm_elems[wn];
                            }
                            Rectangle r = g2_game_grid_itemcell_wall[k];
                            r.Name = wn;
                            r.Fill = wall_color();
                            r.Width = cellsize * 0.3;
                            r.Height = cellsize * 0.3;
                            r.HorizontalAlignment = HorizontalAlignment.Left;
                            r.VerticalAlignment = VerticalAlignment.Top;
                            r.Margin = new Thickness(-0.01 * cellsize, -0.01 * cellsize, 0, 0);

                            g2_game_grid_itemcell_wgrid.Children.Add(r);
                            vm_assign(r);
                        }
                        g2_game_grid_itemcell_wall[0].Width = cellsize * 1.02;
                        g2_game_grid_itemcell_wall[0].Height = cellsize * 1.02;
                        g2_game_grid_itemcell_wall[0].RadiusX = cellsize * 0.3;
                        g2_game_grid_itemcell_wall[0].RadiusY = cellsize * 0.3;
                        g2_game_grid_itemcell_wall[1].Margin = new Thickness(-0.01 * cellsize, -0.01 * cellsize, 0, 0);
                        g2_game_grid_itemcell_wall[2].Margin = new Thickness(cellsize * 0.71, -0.01 * cellsize, 0, 0);
                        g2_game_grid_itemcell_wall[3].Margin = new Thickness(-0.01 * cellsize, cellsize * 0.71, 0, 0);
                        g2_game_grid_itemcell_wall[4].Margin = new Thickness(cellsize * 0.71, cellsize * 0.71, 0, 0);

                        if (c.is_type(g2_cell.type.super_wall))
                        {
                            g2_game_grid_itemcell_wall[1].Visibility = Visibility.Visible;
                            g2_game_grid_itemcell_wall[2].Visibility = Visibility.Visible;
                            g2_game_grid_itemcell_wall[3].Visibility = Visibility.Visible;
                            g2_game_grid_itemcell_wall[4].Visibility = Visibility.Visible;
                        }
                        else
                        {
                            g2_game_grid_itemcell_wall[1].Visibility = Visibility.Hidden;
                            g2_game_grid_itemcell_wall[2].Visibility = Visibility.Hidden;
                            g2_game_grid_itemcell_wall[3].Visibility = Visibility.Hidden;
                            g2_game_grid_itemcell_wall[4].Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            

            if (c.is_type(g2_cell.type.start))
            {
                Point dir = item.dir;
                string s = name + "_startpath";
                Rectangle g2_game_grid_start_path = new Rectangle();
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_start_path = (Rectangle)vm_elems[s];
                }
                g2_game_grid_start_path.Name = s;
                g2_game_grid_start_path.Width = size_mul;
                g2_game_grid_start_path.Height = 0.2 * size_mul;
                g2_game_grid_start_path.Fill = getSCB(Color.FromRgb(75, 75, 75));
                g2_game_grid_start_path.HorizontalAlignment = HorizontalAlignment.Left;
                g2_game_grid_start_path.VerticalAlignment = VerticalAlignment.Top;
                g2_game_grid_start_path.Margin = new Thickness(0.5 * size_mul, 0.4 * size_mul, 0, 0);
                RotateTransform rotate = get_rotate(dir);
                rotate.CenterX = 0;
                rotate.CenterY = size_mul * 0.1;
                spin(g2_game_grid_start_path, rotate);
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_start_path);
                vm_assign(g2_game_grid_start_path);


                s = name + "_startcenter";
                Rectangle g2_game_grid_start_center = new Rectangle();
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_start_center = (Rectangle)vm_elems[s];
                }
                g2_game_grid_start_center.Name = s;
                g2_game_grid_start_center.Width = 0.4 * size_mul;
                g2_game_grid_start_center.Height = 0.4 * size_mul;
                g2_game_grid_start_center.Fill = getSCB(Color.FromRgb(100, 200, 200));
                g2_game_grid_start_center.Stroke = getSCB(Color.FromRgb(75, 75, 75));
                g2_game_grid_start_center.StrokeThickness = 0.1 * size_mul;
                g2_game_grid_start_center.RadiusX = 10 * size_mul;
                g2_game_grid_start_center.RadiusY = 10 * size_mul;
                g2_game_grid_start_center.HorizontalAlignment = HorizontalAlignment.Center;
                g2_game_grid_start_center.VerticalAlignment = VerticalAlignment.Center;
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_start_center);
                vm_assign(g2_game_grid_start_center);

                if (entered)
                {
                    g2_game_grid_start_path.Fill = color_mul(g2_game_grid_start_path.Fill, 2);
                    g2_game_grid_start_center.Stroke = color_mul(g2_game_grid_start_center.Stroke, 2);
                    g2_game_grid_start_center.Fill = color_mul(g2_game_grid_start_center.Fill, 1.2);
                }
            }
            if (c.is_type(g2_cell.type.reflecter))
            {
                Rectangle g2_game_grid_reflecter = new Rectangle();
                string s = name + "_reflecter";
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_reflecter = (Rectangle)vm_elems[s];
                }
                g2_game_grid_reflecter.Name = s;
                g2_game_grid_reflecter.Fill = getSCB(Color.FromRgb(50, 255, 50));
                g2_game_grid_reflecter.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                g2_game_grid_reflecter.StrokeThickness = 0.5;
                g2_game_grid_reflecter.Width = size_mul;
                g2_game_grid_reflecter.Height = 0.08 * size_mul;
                g2_game_grid_reflecter.RadiusX = g2_game_grid_reflecter.Height / 2;
                g2_game_grid_reflecter.RadiusY = g2_game_grid_reflecter.Height / 2;
                g2_game_grid_reflecter.HorizontalAlignment = HorizontalAlignment.Center;
                g2_game_grid_reflecter.VerticalAlignment = VerticalAlignment.Center;
                RotateTransform rotate = get_rotate(c.dir);
                rotate.CenterX = g2_game_grid_reflecter.Width * 0.5;
                rotate.CenterY = g2_game_grid_reflecter.Height * 0.5;
                spin(g2_game_grid_reflecter, rotate);
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_reflecter);
                vm_assign(g2_game_grid_reflecter);
            }
            if (c.is_type(g2_cell.type.in_out))
            {
                foreach (Point p in c.in_dirs)
                {
                    string si = (p.X + 100).ToString();
                    string sj = (p.Y + 100).ToString();
                    Rectangle g2_game_grid_inout = new Rectangle();
                    string s = name + "_in_" + si + "_" + sj;
                    if (vm_elems.ContainsKey(s))
                    {
                        g2_game_grid_inout = (Rectangle)vm_elems[s];
                    }
                    g2_game_grid_inout.Name = s;
                    g2_game_grid_inout.Width = size_mul;
                    g2_game_grid_inout.Height = 0.2 * size_mul;
                    g2_game_grid_inout.Fill = getSCB(Color.FromArgb(150, 50, 175, 50));
                    g2_game_grid_inout.HorizontalAlignment = HorizontalAlignment.Left;
                    g2_game_grid_inout.VerticalAlignment = VerticalAlignment.Top;
                    g2_game_grid_inout.Margin = new Thickness(0.5 * size_mul, 0.4 * size_mul, 0, 0);
                    RotateTransform rotate = get_rotate(p);
                    rotate.CenterX = 0;
                    rotate.CenterY = g2_game_grid_inout.Height * 0.5;
                    spin(g2_game_grid_inout, rotate);
                    g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_inout);
                    vm_assign(g2_game_grid_inout);
                }
                foreach (Point p in c.out_dirs)
                {
                    string si = (p.X + 100).ToString();
                    string sj = (p.Y + 100).ToString();
                    Rectangle g2_game_grid_inout = new Rectangle();
                    string s = name + "_out_" + si + "_" + sj;
                    if (vm_elems.ContainsKey(s))
                    {
                        g2_game_grid_inout = (Rectangle)vm_elems[s];
                    }
                    g2_game_grid_inout.Name = s;
                    g2_game_grid_inout.Width = size_mul;
                    g2_game_grid_inout.Height = 0.2 * size_mul;
                    g2_game_grid_inout.Fill = getSCB(Color.FromArgb(150, 175, 50, 50));
                    g2_game_grid_inout.HorizontalAlignment = HorizontalAlignment.Left;
                    g2_game_grid_inout.VerticalAlignment = VerticalAlignment.Top;
                    g2_game_grid_inout.Margin = new Thickness(0.5 * size_mul, 0.4 * size_mul, 0, 0);
                    RotateTransform rotate = get_rotate(p);
                    rotate.CenterX = 0;
                    rotate.CenterY = g2_game_grid_inout.Height * 0.5;
                    spin(g2_game_grid_inout, rotate);
                    g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_inout);
                    vm_assign(g2_game_grid_inout);
                }
                Rectangle g2_game_grid_inoutcenter = new Rectangle();
                string s0 = name + "_inout";
                if (vm_elems.ContainsKey(s0))
                {
                    g2_game_grid_inoutcenter = (Rectangle)vm_elems[s0];
                }
                g2_game_grid_inoutcenter.Name = s0;
                g2_game_grid_inoutcenter.Width = 0.4 * size_mul;
                g2_game_grid_inoutcenter.Height = 0.4 * size_mul;
                g2_game_grid_inoutcenter.Fill = getSCB(Color.FromRgb(0, 175, 175));
                g2_game_grid_inoutcenter.Stroke = getSCB(Color.FromRgb(125, 125, 50));
                g2_game_grid_inoutcenter.StrokeThickness = 0.1 * size_mul;
                g2_game_grid_inoutcenter.RadiusX = 10 * size_mul;
                g2_game_grid_inoutcenter.RadiusY = 10 * size_mul;
                g2_game_grid_inoutcenter.HorizontalAlignment = HorizontalAlignment.Center;
                g2_game_grid_inoutcenter.VerticalAlignment = VerticalAlignment.Center;
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_inoutcenter);
                vm_assign(g2_game_grid_inoutcenter);

                if (entered)
                {
                    foreach (Rectangle f in g2_game_grid_itemcell_basegrid.Children)
                    {
                        if (f.Fill != null)
                        {
                            f.Fill = color_mul(f.Fill, 1.45);
                        }
                        if (f.Stroke != null)
                        {
                            f.Stroke = color_mul(f.Stroke, 1.45);
                        }
                    }
                }
            }
            if (c.is_type(g2_cell.type.end))
            {
                Rectangle g2_game_grid_end = new Rectangle();
                string s = name + "_end";
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_end = (Rectangle)vm_elems[s];
                }
                g2_game_grid_end.Name = s;
                g2_game_grid_end.Fill = getSCB(Color.FromRgb(0, 200, 200));
                if (entered)
                {
                    g2_game_grid_end.Fill = getSCB(Color.FromRgb(0, 255, 255));
                }
                g2_game_grid_end.Width = size_mul;
                g2_game_grid_end.Height = size_mul;
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_end);
                vm_assign(g2_game_grid_end);
            }
            if (c.is_type(g2_cell.type.light))
            {
                Rectangle g2_game_grid_light_bg = new Rectangle();
                string s = name + "_light_bg";
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_light_bg = (Rectangle)vm_elems[s];
                }
                g2_game_grid_light_bg.Name = s;
                g2_game_grid_light_bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                g2_game_grid_light_bg.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                g2_game_grid_light_bg.StrokeThickness = size_mul * 0.02;
                g2_game_grid_light_bg.HorizontalAlignment = HorizontalAlignment.Stretch;
                g2_game_grid_light_bg.VerticalAlignment = VerticalAlignment.Stretch;
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_light_bg);
                vm_assign(g2_game_grid_light_bg);

                TextBlock g2_game_grid_light = new TextBlock();
                s = name + "_light";
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_light = (TextBlock)vm_elems[s];
                }
                g2_game_grid_light.Name = s;
                g2_game_grid_light.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                g2_game_grid_light.HorizontalAlignment = HorizontalAlignment.Center;
                g2_game_grid_light.VerticalAlignment = VerticalAlignment.Center;
                g2_game_grid_light.FontSize = size_mul * 0.3;
                g2_game_grid_light.Text = "Light";
                g2_game_grid_light.FontFamily = new FontFamily("Comic Sans MS");
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_light);
                vm_assign(g2_game_grid_light);
            }
            if (c.is_type(g2_cell.type.unlight))
            {
                Rectangle g2_game_grid_unlight_bg = new Rectangle();
                string s = name + "_unlight_bg";
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_unlight_bg = (Rectangle)vm_elems[s];
                }
                g2_game_grid_unlight_bg.Name = s;
                g2_game_grid_unlight_bg.Fill = getSCB(Color.FromRgb(0, 0, 0));
                g2_game_grid_unlight_bg.Stroke = getSCB(Color.FromRgb(255, 255, 255));
                g2_game_grid_unlight_bg.StrokeThickness = size_mul * 0.02;
                g2_game_grid_unlight_bg.HorizontalAlignment = HorizontalAlignment.Stretch;
                g2_game_grid_unlight_bg.VerticalAlignment = VerticalAlignment.Stretch;
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_unlight_bg);
                vm_assign(g2_game_grid_unlight_bg);

                TextBlock g2_game_grid_unlight = new TextBlock();
                s = name + "_unlight";
                if (vm_elems.ContainsKey(s))
                {
                    g2_game_grid_unlight = (TextBlock)vm_elems[s];
                }
                g2_game_grid_unlight.Name = s;
                g2_game_grid_unlight.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                g2_game_grid_unlight.HorizontalAlignment = HorizontalAlignment.Center;
                g2_game_grid_unlight.VerticalAlignment = VerticalAlignment.Center;
                g2_game_grid_unlight.FontSize = size_mul * 0.3;
                g2_game_grid_unlight.Text = " Un-\nlight";
                g2_game_grid_unlight.FontFamily = new FontFamily("Comic Sans MS");
                g2_game_grid_itemcell_basegrid.Children.Add(g2_game_grid_unlight);
                vm_assign(g2_game_grid_unlight);
            }

            return g2_game_grid_itemcell;
        }

        decimal g2_ball_id = 0;
        Dictionary<decimal, g2_ball> g2_balls = new Dictionary<decimal, g2_ball>();
        public g2_ball g2_get_ball(Point p, int mx, int my, int ssize = 1)
        {
            g2_ball b = new g2_ball(new Point((p.X + 0.4 * ssize), (p.Y + 0.4 * ssize)), mx, my, ssize)
            {
                id = g2_ball_id
            };
            g2_balls.Add(b.id, b);

            Rectangle g2_ball_r = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness((p.X + 0.4 * ssize) * g2_current.cell_size, (p.Y + 0.4 * ssize) * g2_current.cell_size, 0, 0),
                Fill = getSCB(Color.FromRgb(255, 255, 255)),
                Width = g2_current.cell_size * b.size(),
                Height = g2_current.cell_size * b.size(),
                RadiusX = 500,
                RadiusY = 500
            };
            Grid g = (Grid)vm_elems["g2_game_ball_grid"];
            g.Children.Add(g2_ball_r);
            vm_elems.Add("g2_ball_" + b.id.ToString(), g2_ball_r);

            g2_ball_id++;
            return b;
        }

        private void g2_draw_checkpoint()
        {
            g2_area a = g2_current;
            int nx = a.width * 10 + 1;
            int ny = a.height * 10 + 1;
            Rectangle[,] checkpoints = new Rectangle[nx, ny];

            for(int i = 0; i < nx; i++)
            {
                for(int j = 0; j < ny; j++)
                {
                    if (a.check[i, j] == null)
                    {
                        continue;
                    }
                    if (a.check[i, j].have_type(g2_check_point.type.empty))
                    {
                        continue;
                    }
                    
                    if (a.check[i, j].have_type(g2_check_point.type.death))
                    {
                        checkpoints[i, j] = new Rectangle();
                        Rectangle r = checkpoints[i, j];
                        r.Name = "g2_game_ball_checkpoint_" + i.ToString() + "_" + j.ToString();
                        r.Width = a.cell_size * 0.04;
                        r.Height = a.cell_size * 0.04;
                        r.RadiusX = a.cell_size;
                        r.RadiusY = a.cell_size;
                        r.Fill = getSCB(Color.FromRgb(255, 0, 0));
                        if (a.check[i, j].point_dic[g2_check_point.type.death] != null)
                        {
                            r.Fill = getSCB(Color.FromRgb(255, 150, 0));
                        }
                        r.HorizontalAlignment = HorizontalAlignment.Left;
                        r.VerticalAlignment = VerticalAlignment.Top;
                        r.Margin = new Thickness(a.cell_size * 0.1 * i, a.cell_size * 0.1 * j, 0, 0);
                        Grid g = (Grid)vm_elems["g2_game_ball_grid"];
                        g.Children.Add(r);
                        vm_assign(r);
                    }/*
                    if (a.check[i, j].have_type(g2_check_point.type.reflect))
                    {
                        checkpoints[i, j] = new Rectangle();
                        Rectangle r = checkpoints[i, j];
                        r.Name = "g2_game_ball_checkpoint_" + i.ToString() + "_" + j.ToString();
                        r.Width = a.cell_size * 0.04;
                        r.Height = a.cell_size * 0.04;
                        r.RadiusX = a.cell_size;
                        r.RadiusY = a.cell_size;
                        r.Fill = getSCB(Color.FromRgb(255, 0, 0));
                        r.HorizontalAlignment = HorizontalAlignment.Left;
                        r.VerticalAlignment = VerticalAlignment.Top;
                        r.Margin = new Thickness(a.cell_size * 0.1 * i, a.cell_size * 0.1 * j, 0, 0);
                        Grid g = (Grid)game["g2_game_ball_grid"];
                        g.Children.Add(r);
                        game_assign(r);
                    }*/
                }
            }
        }


        private void g2_unlock(string name)
        {
            g2_level_information p = infs[name];
            p.state = Math.Max(p.state, 0);
            //改变关卡颜色，注册事件

            Rectangle r = (Rectangle)vm_elems[name];
            Rectangle bg = (Rectangle)vm_elems[name + "_bg"];
            if (p.state == 0)
            {
                bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
            }

            r.MouseEnter += rectangle_cover_enter;
            r.MouseLeave += rectangle_cover_leave;
            r.MouseLeftButtonDown += rectangle_cover_down;
            r.MouseLeftButtonUp += rectangle_cover_up;
        }

        private void g2_complete(string name)
        {
            g2_level_information p = infs[name];
            p.state = 1;
            //改变关卡颜色

            Rectangle r = (Rectangle)vm_elems[name];
            Rectangle bg = (Rectangle)vm_elems[name + "_bg"];
            bg.Fill = getSCB(Color.FromRgb(255, 255, 0));

            foreach (g2_level_link l in p.nexts)
            {
                l.state = true;
                Line line = (Line)vm_elems[l.name];
                line.Stroke = getSCB(Color.FromRgb(0, 225, 100));
                //改变线颜色

                bool unlock = true;
                foreach (g2_level_link l2 in infs[l.b.name].prevs)
                {
                    if (l2.state == false)
                    {
                        unlock = false;
                    }
                }
                if (unlock)
                {
                    g2_unlock(l.b.name);
                }
            }

            //回收能量
        }

        private void g2_level_exit()
        {
            g2_area a = g2_current;

            foreach (g2_ball bx in a.balls)
            {
                bx.live = false;
            }
            g2_tick();
            g2_gamestart = false;
        }

        private void g2_level_restart()
        {
            g2_area a = g2_current;

            if (a.testing)
            {
                game_key_name("g2_game_option_startexit");
                game_key_name("g2_game_option_startexit");
            }
            else
            {
                if (a.editing)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            int x = i + j * 4;
                            g2_draw_cell(new g2_cell(), "g2_game_option_create_creater_" + i + "_" + j + "_item", 50, 1, false);
                            vm_elems["g2_game_option_create_creater_" + i + "_" + j + "_select"].Visibility = Visibility.Hidden;
                        }
                    }
                }

                g2_level_exit();
                g2_level_create(a.name);
                g2_start(levels[a.name]);
            }
        }



        Dictionary<int, g2_cell> g2_editor_saver = new Dictionary<int, g2_cell>();
        int g2_cell_edit_id = 0;


        private void game_button_key(object sender, RoutedEventArgs e)
        {
            FrameworkElement f = (FrameworkElement)sender;
            string name = f.Name;
            string[] strs = name.Split('_');

            g2_area a = g2_current;

            RadioButton r = f as RadioButton;

            if (name.Contains("g2_game_create_attr_"))
            {
                g2_cell c = a.edit.result;
                if (name.Contains("g2_game_create_attr_start"))
                {
                    string si = strs[6];
                    string sj = strs[7];
                    int i = Convert.ToInt32(si);
                    int j = Convert.ToInt32(sj);
                    switch (i + 4 * j)
                    {
                        case 0:
                            // con = "←"; 
                            c.dir = new Point(-1, 0);
                            break;
                        case 1:
                            //con = "→"; 
                            c.dir = new Point(1, 0);
                            break;
                        case 2:
                            //con = "↑"; 
                            c.dir = new Point(0, -1);
                            break;
                        case 3:
                            //con = "↓"; 
                            c.dir = new Point(0, 1);
                            break;
                        case 4:
                            //con = "↖"; 
                            c.dir = new Point(-1, -1);
                            break;
                        case 5:
                            //con = "↗"; 
                            c.dir = new Point(1, -1);
                            break;
                        case 6:
                            //con = "↙"; 
                            c.dir = new Point(-1, 1);
                            break;
                        case 7:
                            //con = "↘"; 
                            c.dir = new Point(1, 1);
                            break;
                    }
                }
                else if (name.Contains("g2_game_create_attr_reflecter"))
                {
                    string si = strs[6];
                    string sj = strs[7];
                    int i = Convert.ToInt32(si);
                    int j = Convert.ToInt32(sj);
                    switch (i + 2 * j)
                    {
                        case 0:
                            // con = "←→"; 
                            c.dir = new Point(1, 0);
                            break;
                        case 1:
                            //con = "↑  ↓"; 
                            c.dir = new Point(0, 1);
                            break;
                        case 2:
                            //con = "↙↗"; 
                            c.dir = new Point(-1, 1);
                            break;
                        case 3:
                            //con = "↖↘"; 
                            c.dir = new Point(1, 1);
                            break;
                    }
                }
                else if (name.Contains("g2_game_create_attr_inout"))
                {
                    //属性按键 TODO
                }
            }
        }

        private void g2_show_movement()
        {
            Console.WriteLine();

            g2_area a = g2_current;
            if(a == null)
            {
                return;
            }
            Console.WriteLine(a.name + "  " + "movements:\n");

            foreach(string s in a.movements)
            {
                Console.WriteLine(s);
            }
        }

        private RotateTransform get_rotate(Point p)
        {
            double angle = 0;
            if (p.X == 1 && p.Y == 0)
            {
                angle = 0;
            }
            if (p.X == 1 && p.Y == 1)
            {
                angle = 45;
            }
            if (p.X == 1 && p.Y == -1)
            {
                angle = 315;
            }

            if (p.X == -1 && p.Y == 0)
            {
                angle = 180;
            }
            if (p.X == -1 && p.Y == 1)
            {
                angle = 135;
            }
            if (p.X == -1 && p.Y == -1)
            {
                angle = 225;
            }

            if (p.X == 0 && p.Y == 1)
            {
                angle = 90;
            }
            if (p.X == 0 && p.Y == -1)
            {
                angle = 270;
            }

            return new RotateTransform(angle);
        }
    }

    
}
