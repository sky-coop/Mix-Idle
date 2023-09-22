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
        public void game_load()
        {
            data_init();
            framework_elements = new Dictionary<string, FrameworkElement>();

            number_mode = unlocks.number_mode;
            m.number_mode_combobox.SelectedIndex = number_mode;


            foreach (TextBlock tb in m.options_grid.Children)
            {
                tb.Tag = not_sub_res;
            }
            current_group = "方块";
            current_show_sub_res_group = "";
            all_option_unselect();
            all_other_option_unselect_sub_res();
            option_select(m.方块);


            fps_container = new List<double2>();

            #region
            int i = 0;
            foreach(TextBlock t in m.options_grid.Children)
            {
                if (unlocks.tab_unlock[i])
                {
                    t.Visibility = 0;
                }
                else
                {
                    t.Visibility = (Visibility)1;
                }
                i++;
            }
            if (unlocks.tab_unlock[9])
            {
                m.能量_grid.Visibility = 0;
            }
            else
            {
                m.能量_grid.Visibility = (Visibility)1;
            }
            #endregion

            for(int p = 0; p < 3; p++)
            {
                if (ecb[p])
                {
                    ((CheckBox)m.FindName("能量" + Convert.ToString(p + 1) + "_checkbox")).IsChecked = true;
                }
                else
                {
                    ((CheckBox)m.FindName("能量" + Convert.ToString(p + 1) + "_checkbox")).IsChecked = false;
                }
            }

            //no.1 方块:
            #region

            block_producter bp = block_producters["白色方块"];
            ((TextBlock)(m.FindName("方块_" + bp.name))).Text = bp.name + " 生产器 等级" + number_format(bp.level);
            if (bp.unlocked)
            {
                m.方块_白色方块_grid.Visibility = 0;
            }
            else
            {
                m.方块_白色方块_grid.Visibility = (Visibility)1;
            }

            bp = block_producters["泥土方块"];
            ((TextBlock)(m.FindName("方块_" + bp.name))).Text = bp.name + " 生产器 等级" + number_format(bp.level);
            if (bp.unlocked)
            {
                m.方块_泥土方块_grid.Visibility = 0;
            }
            else
            {
                m.方块_泥土方块_grid.Visibility = (Visibility)1;
            }

            bp = block_producters["木头方块"];
            ((TextBlock)(m.FindName("方块_" + bp.name))).Text = bp.name + " 生产器 等级" + number_format(bp.level);
            if (bp.unlocked)
            {
                m.方块_木头方块_grid.Visibility = 0;
            }
            else
            {
                m.方块_木头方块_grid.Visibility = (Visibility)1;
            }

            bp = block_producters["糖方块"];
            ((TextBlock)(m.FindName("方块_" + bp.name))).Text = bp.name + " 生产器 等级" + number_format(bp.level);
            if (bp.unlocked)
            {
                m.方块_糖方块_grid.Visibility = 0;
            }
            else
            {
                m.方块_糖方块_grid.Visibility = (Visibility)1;
            }

            bp = block_producters["石头方块"];
            ((TextBlock)(m.FindName("方块_" + bp.name))).Text = bp.name + " 生产器 等级" + number_format(bp.level);
            if (bp.unlocked)
            {
                m.方块_石头方块_grid.Visibility = 0;
            }
            else
            {
                m.方块_石头方块_grid.Visibility = (Visibility)1;
            }
            #endregion

            //no.2 制造：
            #region

            制造_options = make_group(m.制造_option_grid);
            if (unlocks.food)
            {
                m.制造_菜单_食物_grid.Visibility = 0;
            }
            else
            {
                m.制造_菜单_食物_grid.Visibility = (Visibility)1;
            }

            foreach (KeyValuePair<string, upgrade> kp in upgrades)
            {
                upgrade u = kp.Value;
                if (u.material)
                {
                    string name = kp.Key;
                    Grid g = (Grid)(m.FindName("制造_次_材料_" + name + "_grid"));
                    if (u.unlocked)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }
                }
            }

            
            foreach(Grid g in m.制造_菜单_工具_target_grid.Children)
            {
                string name = g.Name.Split('_')[3];
                upgrade u = upgrades[name];
                if (u.unlocked)
                {
                    g.Visibility = 0;
                }
                else
                {
                    g.Visibility = (Visibility)1;
                }
            }
            foreach (Grid g in m.制造_菜单_升级器_target_grid.Children)
            {
                string name = g.Name.Split('_')[3];
                upgrade u = upgrades[name];
                if (u.unlocked)
                {
                    g.Visibility = 0;
                }
                else
                {
                    g.Visibility = (Visibility)1;
                }
            }
            foreach (Grid g in m.制造_菜单_食物_target_grid.Children)
            {
                string name = g.Name.Split('_')[3];
                upgrade u = upgrades[name];
                if (u.unlocked)
                {
                    g.Visibility = 0;
                }
                else
                {
                    g.Visibility = (Visibility)1;
                }
            }


            #endregion

            




            //no.4 魔法：
            #region
            

            LinearGradientBrush lgb = get_lgb();


            foreach (Grid g in m.魔法_祭坛_祭品_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }

            if (res_table["战斗"]["白色粒子"].unlocked)
            {
                m.魔法_祭坛_祭品_白色粒子_grid.Visibility = 0;
            }
            if (res_table["战斗"]["绿色粒子"].unlocked)
            {
                m.魔法_祭坛_祭品_绿色粒子_grid.Visibility = 0;
            }
            if (res_table["战斗"]["红色粒子"].unlocked)
            {
                m.魔法_祭坛_祭品_红色粒子_grid.Visibility = 0;
            }

            if (res_table["战斗"]["无色粒子"].unlocked)
            {
                m.魔法_祭坛_祭品_无色粒子_grid.Visibility = 0;
            }


            #region
            if (upgrades["祭坛升级"].unlocked)
            {
                m.魔法_祭坛_升级0_grid.Visibility = 0;
            }
            else
            {
                m.魔法_祭坛_升级0_grid.Visibility = (Visibility)1;
            }
            #endregion


            foreach(KeyValuePair<string, enchant> kp in enchants)
            {
                enchant e = kp.Value;
                string name_of_grid = "";
                if (e.is_potion)
                {
                    name_of_grid = "魔法_次_药水_" + e.name + "_grid";
                }
                else
                {
                    name_of_grid = "魔法_次_附魔_" + e.name + "_grid";
                }

                Grid g = (Grid)m.FindName(name_of_grid);
                if (e.unlocked)
                {
                    g.Visibility = 0;
                }
                else
                {
                    g.Visibility = Visibility.Hidden;
                }
            }
            

            foreach(KeyValuePair<string, upgrade> kp in upgrades) 
            {
                upgrade u = kp.Value;
                string name = kp.Key;
                if(u is spell)
                {
                    Grid g = ((Grid)(m.FindName("魔法_法术_" + name + "_grid")));
                    if (u.unlocked)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }
                }
            }

            #region
            foreach(KeyValuePair<string,enchant> kp in enchants)
            {
                enchant e = kp.Value;
                string name = kp.Key;
                if (e.is_potion)
                {
                    e.LinearGradientBrush = get_lgb();
                    Grid g = ((Grid)(m.FindName("魔法_次_药水_" + name + "_grid")));
                    if (e.unlocked)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }
                }
            }

            //魔法_附魔_魔法粉末
            
            foreach (KeyValuePair<string, enchant> kp in enchants)
            {
                enchant e = kp.Value;
                string name = kp.Key;
                string s = "附魔";
                if (e.is_potion)
                {
                    s = "药水";
                }

                string cover_name = "魔法_" + s + "_" + name;
                string bg_name = cover_name + "_背景";
                string text_name = cover_name + "_文字";
                Rectangle r = ((Rectangle)(m.FindName(bg_name)));
                TextBlock t = ((TextBlock)(m.FindName(text_name)));
                if (e.active)
                {
                    r.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    t.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
                else
                {
                    r.Fill = getSCB(Color.FromRgb(0, 255, 195));
                    t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
            }

            //魔法_法术_白色魔法_施法
            foreach (KeyValuePair<string, upgrade> kp in upgrades)
            {
                upgrade u = kp.Value;
                string name = kp.Key;
                
                if (u is spell)
                {
                    spell s = (spell)u;
                    string cover_name = "魔法_法术_" + name + "_学习";
                    string bg_name = cover_name + "_背景";
                    string text_name = cover_name + "_文字";
                    Rectangle r = ((Rectangle)(m.FindName(bg_name)));
                    TextBlock t = ((TextBlock)(m.FindName(text_name)));


                    if (s.studying)
                    {
                        r.Fill = getSCB(Color.FromRgb(0, 255, 255));
                    }
                    else
                    {
                        r.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    }
                    t.Foreground = getSCB(Color.FromRgb(0, 0, 0));

                    if (name == "探索魔法" || name == "法术创作")
                    {
                        continue;
                    }

                    cover_name = "魔法_法术_" + name + "_施法";
                    bg_name = cover_name + "_背景";
                    text_name = cover_name + "_文字";
                    r = ((Rectangle)(m.FindName(bg_name)));
                    t = ((TextBlock)(m.FindName(text_name)));
                    if (s.casting)
                    {
                        r.Fill = getSCB(Color.FromRgb(0, 255, 255));
                    }
                    else
                    {
                        r.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    }
                    t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
            }
            #endregion

            #endregion

            //no.5 采矿：
            #region
            采矿_options = make_group(m.采矿_option_grid);

            mine_generate();
            get_field();


            heater_generate();

            #endregion

            //no.6 核心：
            #region
            #endregion

            //no.7 娱乐：
            #region
            vm_elems = new Dictionary<string, FrameworkElement>();
            if (vm.opened)
            {
                m.vm_main_grid.Visibility = Visibility.Visible;
            }
            vm.dt_changed = true;
            vm_fullscreen = false;
            #endregion

            //no.8 混沌：
            #region
            #endregion


            //no.9 转生：
            #region
            List<string> punlocked = new List<string>(){ "对数增益", "制造", "成就加成" };
            List<string> pall = new List<string>();
            foreach (KeyValuePair<string, prestige_upgrade> pair in prestige_ups)
            {
                pair.Value.unlocked = false;
            }
            foreach (KeyValuePair<string, link> pair in links)
            {
                string s1 = pair.Key.Split('_')[0];
                string s2 = pair.Key.Split('_')[1];
                link link = pair.Value;
                link.s = m.转生_main_grid;
                link.a = (Grid)m.FindName("转生_升级_" + s1 + "_grid");
                link.b = (Grid)m.FindName("转生_升级_" + s2 + "_grid");
                link.update_progress(0);
            }


            foreach (FrameworkElement g in m.转生_main_grid.Children)
            {
                if (g is Grid)
                {
                    pall.Add(g.Name.Split('_')[2]);
                }
            }
            for(int k = 0; k < punlocked.Count; k++) 
            { 
                string s1 = punlocked[k];
                prestige_ups[s1].unlocked = true;
                foreach (string s2 in pall)
                {
                    if(links.ContainsKey(s1 + "_" + s2))
                    {
                        link link = links[s1 + "_" + s2];
                        link.update_progress(prestige_ups[s1].level);
                        if (link.complete)
                        {
                            punlocked.Add(s2);
                        }
                    }
                }
            }
            foreach(KeyValuePair<string, prestige_upgrade> pair in prestige_ups)
            {
                prestige_upgrade prestige_upgrade = pair.Value;
                string s1 = pair.Key;
                foreach(string s2 in pall)
                {
                    if (links.ContainsKey(s1 + "_" + s2))
                    {
                        link link = links[s1 + "_" + s2];
                        if (prestige_upgrade.unlocked)
                        {
                            link.b.Visibility = Visibility.Visible;
                            link.unlock();
                        }
                    }
                }
            }
            #endregion

            visual_init();

            //成就
            #region

            total_up_levels = 0;
            foreach (KeyValuePair<int, achievement> kp in achievements_id)
            {
                int id = kp.Key;
                achievement a = achievements_id[id];
                if (a.up_level > 0)
                {
                    int k = id / 10;
                    int j = id % 10;
                    achievefield_hint[k, j].Visibility = 0;
                    achievefield_hint_texts[k, j].Text = Convert.ToString(a.up_level);
                    total_up_levels += a.up_level;
                }
            }
            if (total_up_levels > 0)
            {
                m.achieve_hint_grid.Visibility = 0;
                m.achieve_hint_total_text.Text = Convert.ToString(total_up_levels);
            }

            #endregion


            buy_int = true;
            buy_number = 1;
            rectangle_cover_up(m.num_x1, null);
            checkname = "";


            ticker = new DispatcherTimer();
            stopwatch = new Stopwatch();
            time_start();

            //MessageBox.Show(get_online_now());

            offline_time_current = (get_now() - save_time);
            if (offline_time_current < 0)
            {
                MessageBox.Show("时间错误：现在的系统时间早于存档保存时间，" +
                    "离线收益已被永久禁用。若要恢复，请联系作者。", "提示");
                offline = false;
            }
            if (offline)
            {
                offline_time_remain += offline_time_current;
                m.offline_grid.Visibility = Visibility.Visible;
                offline_update();
                offline_produce();
            }
            else
            {
                MessageBox.Show("离线收益已被禁用", "提示");
            }
        }
    }
}
