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
    //用于读取
    public partial class gamestats
    {   

        [Serializable]
        public class save_unlocks
        {
            public bool[] tab_unlock = new bool[10];

            public bool[] fight_unlock = new bool[12];

            public bool potion = false;
            public bool food = false;

            public enemy current_enemy = null;
            public string current_field = null;

            public int number_mode;
        }
        save_unlocks unlocks = new save_unlocks();

        [Serializable]
        public class not_serial
        {

        }

        //工具
        private static string make_text<T>(T input)
        {
            return Convert.ToString(input);
        }

        private void test<T>(T input)
        {
            MessageBox.Show(make_text(input));
        }

        private resource find_resource(string name)
        {
            foreach (KeyValuePair<string, Dictionary<string, resource>> kp in res_table)
            {
                if (kp.Value.ContainsKey(name))
                {
                    return kp.Value[name];
                }
            }
            if (treasures.ContainsKey(name))
            {
                return treasures[name];
            }
            if (g1_res.ContainsKey(name))
            {
                return g1_res[name];
            }
            return null;
        }

        private upgrade find_upgrade(string name)
        {
            foreach (KeyValuePair<string, upgrade> kp in upgrades)
            {
                if (kp.Value.name == name)
                {
                    return kp.Value;
                }
            }
            return null;
        }

        private enemy find_enemy(string name)
        {
            foreach (KeyValuePair<string, Dictionary<string, enemy>> kp in enemies)
            {
                if (kp.Value.ContainsKey(name))
                {
                    return kp.Value[name];
                }
            }
            return null;
        }

        private SolidColorBrush getSCB(Color c)
        {
            return new SolidColorBrush(c);
        }

        // option
        #region
        object selected = new object(),
            sub_res = new object(),
            selected_and_sub = new object(),
            not_sub_res = new object();
        public void option_selected(object sender, MouseButtonEventArgs e)
        {
            all_option_unselect();
            TextBlock t = (TextBlock)sender;
            option_select(t);
            current_group = t.Text.Trim();
        }

        public void option_res_selected(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;

            all_other_option_unselect_sub_res();
            if (t.Tag.Equals(selected))
            {
                t.Tag = selected_and_sub;
                current_show_sub_res_group = t.Text.Trim();
            }
            else if (t.Tag.Equals(selected_and_sub))
            {
                t.Tag = selected;
                current_show_sub_res_group = "";
            }
            else if (t.Tag.Equals(sub_res))
            {
                t.Tag = not_sub_res;
                t.Background = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                t.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                current_show_sub_res_group = "";
            }
            else
            {
                t.Tag = sub_res;
                t.Background = new SolidColorBrush(Color.FromRgb(120, 120, 255));
                t.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                current_show_sub_res_group = t.Text.Trim();
            }
        }

        private void all_option_unselect()
        {
            foreach (TextBlock tb in m.options_grid.Children)
            {
                option_unselect(tb);
            }
        }

        private void all_other_option_unselect_sub_res()
        {
            foreach (TextBlock tb in m.options_grid.Children)
            {
                if (tb.Tag.Equals(selected_and_sub))
                {
                    tb.Tag = selected;
                }
                if (tb.Tag.Equals(sub_res))
                {
                    tb.Tag = not_sub_res;
                    tb.Background = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
            }
        }

        private void option_unselect(TextBlock tb)
        {
            SolidColorBrush bg;
            SolidColorBrush tx;
            if (tb.Tag.Equals(sub_res) || tb.Tag.Equals(selected_and_sub))
            {
                tb.Tag = sub_res;
                bg = new SolidColorBrush(Color.FromRgb(120, 120, 255));
                tx = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                tb.Tag = not_sub_res;
                bg = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                tx = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            tb.Background = bg;
            tb.Foreground = tx;
        }

        private void option_select(TextBlock tb)
        {
            SolidColorBrush bg = new SolidColorBrush(Color.FromRgb(40, 125, 125));
            SolidColorBrush tx = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            tb.Background = bg;
            tb.Foreground = tx;
            current_group = tb.Name;
            if (tb.Tag.Equals(sub_res) || tb.Tag.Equals(selected_and_sub))
            {
                tb.Tag = selected_and_sub;
            }
            else
            {
                tb.Tag = selected;
            }
            string grid_name = tb.Name + "_grid";
            view_change((Grid)(m.FindName(grid_name)));
        }

        //切换选项页
        private void view_change(Grid g)
        {
            List<Grid> to_remove = new List<Grid>();
            foreach (Grid gr in m.view_grid.Children)
            {
                to_remove.Add(gr);
            }
            foreach (Grid gr in to_remove)
            {
                m.view_grid.Children.Remove(gr);
            }
            m.view_grid.Children.Add(g);
        }
        #endregion

        //make option group
        private List<Rectangle> make_group(Grid g)
        {
            List<Rectangle> ret = new List<Rectangle>();
            foreach (FrameworkElement c in g.Children)
            {
                string[] str = c.Name.Split('_');
                string s1 = str[str.Length - 1];
                if (s1 == "grid")
                {
                    string s2 = c.Name.Substring(0, c.Name.Length - 5);
                    ret.Add(find_elem<Rectangle>(s2));
                }
            }
            return ret;
        }

        private void group_process(List<Rectangle> group, Rectangle r, bool have_target,
            SolidColorBrush active_color = null, SolidColorBrush normal_color = null,
            SolidColorBrush text_color = null)
        {
            if (r != null)
            {
                if (!group.Contains(r))
                {
                    throw new Exception();
                }
            }

            foreach (Rectangle x in group)
            {
                x.Tag = enable;
                string bg_name = x.Name + "_背景";
                string text_name = x.Name + "_文字";
                Rectangle bg = find_elem<Rectangle>(bg_name);
                if (normal_color != null)
                {
                    bg.Fill = normal_color;
                }
                else
                {
                    bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                }

                TextBlock text = find_elem<TextBlock>(text_name);
                if (text_color != null)
                {
                    text.Foreground = text_color;
                }
                else
                {
                    text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }

                if (have_target)
                {
                    find_elem<Grid>(x.Name + "_target_grid").Visibility = (Visibility)1;
                }
            }

            if (r != null)
            {
                r.Tag = disable;
                Rectangle bg = find_elem<Rectangle>(r.Name + "_背景");
                if (active_color != null)
                {
                    bg.Fill = active_color;
                }
                else
                {
                    bg.Fill = getSCB(Color.FromRgb(0, 180, 180));
                }

                if (have_target)
                {
                    find_elem<Grid>(r.Name + "_target_grid").Visibility = 0;
                }
            }
        }

        private void group_process_2(List<List<Rectangle>> group2, bool have_target, SolidColorBrush active_color = null)
        {
            foreach (List<Rectangle> group in group2)
            {
                group_process(group, null, have_target, active_color);
            }
        }



        public bool save(string filename = "auto")
        {
            save_unlock();
            savefile s = new savefile();
            save_time = get_now();
            return s.save(this, "./存档/" + filename + "/" + filename + ".a.mixidle");
        }

        private bool load(string filename = "auto")
        {
            savefile s = new savefile();

            return s.load(m, "./存档/" + filename + "/" + filename + ".a.mixidle");
        }

        private void save_unlock()
        {
            if (m.方块.Visibility == 0)
            {
                unlocks.tab_unlock[0] = true;
            }
            else
            {
                unlocks.tab_unlock[0] = false;
            }

            if (m.制造.Visibility == 0)
            {
                unlocks.tab_unlock[1] = true;
            }
            else
            {
                unlocks.tab_unlock[1] = false;
            }

            if (m.战斗.Visibility == 0)
            {
                unlocks.tab_unlock[2] = true;
            }
            else
            {
                unlocks.tab_unlock[2] = false;
            }

            if (m.魔法.Visibility == 0)
            {
                unlocks.tab_unlock[3] = true;
            }
            else
            {
                unlocks.tab_unlock[3] = false;
            }

            if (m.采矿.Visibility == 0)
            {
                unlocks.tab_unlock[4] = true;
            }
            else
            {
                unlocks.tab_unlock[4] = false;
            }

            if (m.核心.Visibility == 0)
            {
                unlocks.tab_unlock[5] = true;
            }
            else
            {
                unlocks.tab_unlock[5] = false;
            }

            if (m.娱乐.Visibility == 0)
            {
                unlocks.tab_unlock[6] = true;
            }
            else
            {
                unlocks.tab_unlock[6] = false;
            }

            if (m.混沌.Visibility == 0)
            {
                unlocks.tab_unlock[7] = true;
            }
            else
            {
                unlocks.tab_unlock[7] = false;
            }

            if (m.转生.Visibility == 0)
            {
                unlocks.tab_unlock[8] = true;
            }
            else
            {
                unlocks.tab_unlock[8] = false;
            }

            if (m.能量_grid.Visibility == 0)
            {
                unlocks.tab_unlock[9] = true;
            }
            else
            {
                unlocks.tab_unlock[9] = false;
            }

            int i = 0;
            foreach (Grid g in m.战斗_option_grid.Children)
            {
                if (g.Visibility == 0)
                {
                    unlocks.fight_unlock[i] = true;
                }
                else
                {
                    unlocks.fight_unlock[i] = false;
                }
                i++;
            }
            unlocks.current_field = enemy.curr_field;
            unlocks.current_enemy = enemy.current;
        }

        public LinearGradientBrush get_lgb()
        {
            LinearGradientBrush lgb = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            lgb.GradientStops.Add(new GradientStop(Color.FromArgb(63, 0, 0, 0), 0));
            lgb.GradientStops.Add(new GradientStop(Color.FromArgb(63, 0, 127, 255), 1));
            return lgb;
        }

        public void shift_color_byte(ref byte b)
        {
            b = (byte)(b + 128);
        }
        public SolidColorBrush shift_color(SolidColorBrush x)
        {
            byte r = x.Color.R;
            shift_color_byte(ref r);
            byte g = x.Color.G;
            shift_color_byte(ref g);
            byte b = x.Color.B;
            shift_color_byte(ref b);
            return getSCB(Color.FromRgb(r, g, b));
        }


        public void debug(object sender, MouseButtonEventArgs e)
        {
            string name = ((FrameworkElement)sender).Name;

            if (name == "调试a")
            {
                m.魔法.Visibility = 0;
                m.娱乐.Visibility = 0;
                // TODO: 给资源
                // TODO EX: 输入框改变资源
                /*
                pulse(1000);
                
                foreach (KeyValuePair<string, Dictionary<string, resource>> kp in res_table)
                {
                    foreach(KeyValuePair<string, resource> kp2 in kp.Value)
                    {
                        resource r = kp2.Value;
                        if (double.IsNaN(r.get_mul()))
                        {
                            test(r.name + "  " + number_format(r.get_mul()));
                            foreach(KeyValuePair<string, multiplier> kp3 in r.multipliers)
                            {
                                test(r.name + "  " + kp3.Key + "  " +  number_format(kp3.Value.value));
                            }
                        }
                    }
                }*/
            }
            else if (name == "调试b")
            {
                //g2_draw_checkpoint();
                g2_show_movement();
            }
            else
            {
                res_table["转生"]["转生点数"].add_value(res_table["转生"]["转生点数"].get_value() + 100000);
                /*
                m.制造.Visibility = 0;
                m.战斗.Visibility = 0;
                m.魔法.Visibility = 0;
                res_table["魔法"]["魔力"].add_value(50e6);*/
                /*
                m.能量_grid.Visibility = 0;
                res_table["特殊"]["能量"].add_value(5000);
                res_table["特殊"]["高阶能量"].add_value(1000);
                res_table["特殊"]["终极能量"].add_value(1000);*/

                /*upgrades["植物祭品"].set_init_special(1.925, 3);
                upgrades["动物祭品"].set_init_special(1.596, 2);*/

                /*
                block_producters["石头方块"].unlocked = true;
                res_table["方块"]["石头方块"].unlocked = true;
                m.方块_石头方块_grid.Visibility = 0;
                m.采矿.Visibility = 0;
                minep.unlocked = true;

                res_table["采矿"]["采矿点数"].unlocked = true;
                res_table["采矿"]["煤"].unlocked = true;
                res_table["采矿"]["铜矿"].unlocked = true;
                res_table["采矿"]["铁矿"].unlocked = true;
                upgrades["剑"].unlocked = true;
                m.制造_次_工具_剑_grid.Visibility = 0;
                upgrades["镐"].unlocked = true;
                m.制造_次_工具_镐_grid.Visibility = 0;*/

                /*
                res_table["方块"]["木头方块"].unlocked = true;
                res_table["方块"]["木头方块"].add_value(1e9);
                res_table["方块"]["石头方块"].add_value(1e15);
                res_table["采矿"]["煤"].add_value(1e6);
                res_table["采矿"]["铜矿"].add_value(1e6);
                res_table["采矿"]["铁矿"].add_value(1e6);
                res_table["魔法"]["烈焰粉末"].unlocked = true;
                res_table["魔法"]["烈焰粉末"].add_value(1e6);*/

                /*
                res_table["转生"]["转生点数"].add_value(5e9);
                res_table["方块"]["白色方块"].add_value(1e18);
                res_table["制造"]["白色粉末"].add_value(2e12);
                res_table["战斗"]["白色粒子"].add_value(1e8);
                res_table["战斗"]["绿色粒子"].add_value(10000);
                res_table["魔法"]["魔力"].add_value(1000000);
                制造.Visibility = 0;
                战斗.Visibility = 0;
                魔法.Visibility = 0;
                m.采矿.Visibility = 0;
                res_table["采矿"]["采矿点数"].unlocked = true;
                minep.unlocked = true;*/

            }

        }

        public void debug_gamespeed_down(object sender, MouseButtonEventArgs e)
        {
            game_speed_base /= new double2(2, 0);
            ((TextBlock)sender).Text = number_format(gamespeed());
        }

        public void debug_gamespeed_up(object sender, MouseButtonEventArgs e)
        {
            game_speed_base *= new double2(2, 0);
            //game_speed_base = double2.Pow(game_speed_base, 2);
            ((TextBlock)sender).Text = number_format(gamespeed());
        }

        public void debug_pulse(object sender, MouseButtonEventArgs e)
        {
            pulse_t = 100000;
            ((TextBlock)sender).Text = number_format(pulse_t);
        }
        public void debug_recover(object sender, MouseButtonEventArgs e)
        {
            offline = true;
            ((TextBlock)sender).Text = "recovered!";
        }


    }
}
