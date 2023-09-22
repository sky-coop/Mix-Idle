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
            time_boost_max_old = -1;

            framework_elements = new Dictionary<string, FrameworkElement>();

            number_mode = unlocks.number_mode;
            m.number_mode_combobox.SelectedIndex = number_mode;
            m.详细信息框.Visibility = (Visibility)2;

            none = new resource(0, 0, "无", getSCB(Color.FromRgb(0, 0, 0)));
            one = new resource(0, 1, "无", getSCB(Color.FromRgb(0, 0, 0)));

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
                    t.Visibility = (Visibility)0;
                }
                else
                {
                    t.Visibility = (Visibility)1;
                }
                i++;
            }
            if (unlocks.tab_unlock[9])
            {
                m.能量_grid.Visibility = (Visibility)0;
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



            foreach (Grid g in m.制造_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            

            i = 0;
            foreach (Grid g in m.战斗_option_grid.Children)
            {
                if (unlocks.fight_unlock[i])
                {
                    g.Visibility = (Visibility)0;
                }
                else
                {
                    g.Visibility = (Visibility)1;
                }
                i++;
            }

            foreach (Grid g in m.战斗_场景_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            m.战斗_场景_information_grid.Visibility = (Visibility)1;
            m.战斗_玩家_grid.Visibility = 0;

            //选项之间有线
            

            foreach (Grid g in m.魔法_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
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

            //no.3 战斗:
            #region

            战斗_options = make_group(m.战斗_option_grid);

            战斗_enemies = new List<List<Rectangle>>();
            战斗_洁白世界_enemies = make_group(m.战斗_场景_洁白世界_enemy_grid);
            战斗_enemies.Add(战斗_洁白世界_enemies);
            战斗_草原_enemies = make_group(m.战斗_场景_草原_enemy_grid);
            战斗_enemies.Add(战斗_草原_enemies);
            战斗_死火山_enemies = make_group(m.战斗_场景_死火山_enemy_grid);
            战斗_enemies.Add(战斗_死火山_enemies);
            战斗_机关屋_enemies = make_group(m.战斗_场景_机关屋_enemy_grid);
            战斗_enemies.Add(战斗_机关屋_enemies);
            战斗_魔境_enemies = make_group(m.战斗_场景_魔境_enemy_grid);
            战斗_enemies.Add(战斗_魔境_enemies);

            战斗_自动攻击风格 = make_group(m.战斗_玩家_攻击风格_自动_grid);
            //更多group……
            
            
            

            //敌人
            #region
            //洁白世界
            #region
            {
                //洁白世界::白色粒子
            }
            #endregion
            //草原
            #region
            {

                
                //草原::橙色粒子
                //resource res_橙色粒子 = new resource(4, 0, "橙色粒子",
                    //getSCB(Color.FromRgb(255, 127, 0)));
                //res_橙色粒子.unlocked = false;
                //res_group_战斗.Add("橙色粒子", res_橙色粒子);


                //草原::无色粒子
                
            }
            #endregion

            //死火山
            #region
            {

                //死火山::红色粒子
                
            }
            #endregion

            //敌人结束
            #endregion

            enemy.curr_field = unlocks.current_field;
            enemy.current = unlocks.current_enemy;

            if (enemy.curr_field != null)
            {
                group_process(战斗_options, ((Rectangle)m.FindName("战斗_场景_" + enemy.curr_field)), true);

                Grid field_grid = (Grid)(m.FindName("战斗_场景_" + enemy.curr_field + "_target_grid"));
                Grid enemy_grid = (Grid)(m.FindName("战斗_场景_" + enemy.curr_field + "_enemy_grid"));
                int count = 0;
                foreach (Grid g in m.战斗_场景_grid.Children)
                {
                    if (g.Equals(field_grid))
                    {
                        break;
                    }
                    count++;
                }

                foreach (FrameworkElement f in enemy_grid.Children)
                {
                    if (f is Grid)
                    {
                        Grid g = (Grid)f;
                        string name = g.Name.Split('_')[4];
                        if (name == enemy.current.name)
                        {
                            group_process(战斗_enemies[count], (Rectangle)(g.Children[2]), false);
                        }
                    }
                    else
                    {
                    }
                }
            }

            {
                Rectangle fight = m.战斗_场景_information_fight;
                Rectangle background = (Rectangle)m.FindName(fight.Name + "_背景");
                TextBlock text = (TextBlock)m.FindName(fight.Name + "_文字");
                if (fighting)
                {
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
                else
                {
                    background.Fill = getSCB(Color.FromRgb(0, 255, 195));
                    text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
            }

            //战斗_玩家_攻击风格_自动_重击
            attack_form af = you.auto_attack_form;
            group_process(战斗_自动攻击风格, (Rectangle)(m.FindName("战斗_玩家_攻击风格_自动_" + af.name)), false, getSCB(Color.FromRgb(100, 255, 100)));


            //战斗结束
            #endregion




            //no.4 魔法：
            #region
            魔法_options = make_group(m.魔法_option_grid);
            if (unlocks.potion)
            {
                m.魔法_菜单_药水_grid.Visibility = 0;

            }
            else
            {
                m.魔法_菜单_药水_grid.Visibility = (Visibility)1;
            }

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
            vm_elem_init();
            vm.dt_changed = true;
            vm_fullscreen = false;
            #endregion

            //no.8 混沌：
            #region
            #endregion


            //no.9 转生：
            #region





            //转生：升级
            m.转生_升级_资源保留_grid.Visibility = (Visibility)1;
            m.转生_升级_时间力量_grid.Visibility = (Visibility)1;
            m.转生_升级_魔法增幅_grid.Visibility = (Visibility)1;
            m.转生_升级_强化等级_grid.Visibility = (Visibility)1;
            m.转生_升级_战斗探索_grid.Visibility = (Visibility)1;
            m.转生_升级_冷静_grid.Visibility = (Visibility)1;
            //0-0 对数增益
            #region
            m.转生_升级_对数增益_grid.Visibility = 0;
            #endregion

            //1-0 生成器
            #region
            int level = prestige_ups["对数增益"].level;
            m.转生_升级_生成器_grid.Visibility = 0;
            #endregion

            //                                                            深度优先
            //对数增益——生成器
            #region
            bool task1 = false;
            bool task2 = false;
            link lk = links["对数增益_生成器"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_对数增益_grid;
            lk.b = m.转生_升级_生成器_grid;
            lk.update_progress(level);
            if (lk.complete)
            {
                prestige_ups["生成器"].unlocked = true;
                //引出接下来的升级（及链接）;
                m.转生_升级_资源保留_grid.Visibility = 0;
                task1 = true;
                //links["生成器_资源保留"].unlock();             task
            }
            #endregion

            //2-0 资源保留
            #region
            level = prestige_ups["生成器"].level;
            #endregion

            //生成器——资源保留
            #region
            lk = links["生成器_资源保留"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_生成器_grid;
            lk.b = m.转生_升级_资源保留_grid;


            lk.update_progress(level);
            if (task1)
            {
                links["生成器_资源保留"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["资源保留"].unlocked = true;
                //引出接下来的升级（及链接）;
                m.转生_升级_升级保留_grid.Visibility = 0;
                task1 = true;
            }
            #endregion
            
            //2-1 升级保留
            #region
            level = prestige_ups["资源保留"].level;
            #endregion

            //生成器——资源保留
            #region
            lk = links["资源保留_升级保留"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_资源保留_grid;
            lk.b = m.转生_升级_升级保留_grid;


            lk.update_progress(level);
            if (task1)
            {
                links["资源保留_升级保留"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["升级保留"].unlocked = true;
                //引出接下来的升级（及链接）;
            }
            #endregion




            //0-7 制造
            #region
            m.转生_升级_制造_grid.Visibility = 0;
            #endregion

            //1-6 方块增幅
            #region
            level = prestige_ups["制造"].level;
            m.转生_升级_方块增幅_grid.Visibility = 0;
            #endregion


            //                                                            深度优先
            //制造——方块增幅
            #region
            lk = links["制造_方块增幅"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_制造_grid;
            lk.b = m.转生_升级_方块增幅_grid;
            lk.update_progress(level);
            if (lk.complete)
            {
                prestige_ups["方块增幅"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
                m.转生_升级_魔法增幅_grid.Visibility = 0;
                task1 = true;
                //links["方块增幅_魔法增幅"].unlock();
                m.转生_升级_时间力量_grid.Visibility = 0;
                task2 = true;
                //links["方块增幅_时间力量"].unlock();
            }
            #endregion


            //1-5 时间力量
            #region
            level = prestige_ups["方块增幅"].level;
            #endregion

            //方块增幅——时间力量
            #region
            lk = links["方块增幅_时间力量"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_方块增幅_grid;
            lk.b = m.转生_升级_时间力量_grid;
            lk.update_progress(level);
            if (task2)
            {
                links["方块增幅_时间力量"].unlock();
            }
            task2 = false;
            if (lk.complete)
            {
                prestige_ups["时间力量"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
            }
            #endregion

            //2-6 魔法增幅
            #region
            level = prestige_ups["方块增幅"].level;
            #endregion

            //方块增幅——魔法增幅
            #region
            lk = links["方块增幅_魔法增幅"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_方块增幅_grid;
            lk.b = m.转生_升级_魔法增幅_grid;
            lk.update_progress(level);
            if (task1)
            {
                links["方块增幅_魔法增幅"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["魔法增幅"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
                m.转生_升级_采矿增幅_grid.Visibility = 0;
                task1 = true;

                m.转生_升级_转化_grid.Visibility = 0;
                task2 = true;
            }
            #endregion



            //3-6 采矿增幅
            #region
            level = prestige_ups["魔法增幅"].level;
            #endregion

            //方块增幅——魔法增幅
            #region
            lk = links["魔法增幅_采矿增幅"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_魔法增幅_grid;
            lk.b = m.转生_升级_采矿增幅_grid;
            lk.update_progress(level);
            if (task1)
            {
                links["魔法增幅_采矿增幅"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["采矿增幅"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
            }
            #endregion


            //2-5 转化
            #region
            level = prestige_ups["转化"].level;
            #endregion

            //魔法增幅——转化
            #region
            lk = links["魔法增幅_转化"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_魔法增幅_grid;
            lk.b = m.转生_升级_转化_grid;
            lk.update_progress(level);
            if (task2)
            {
                links["魔法增幅_转化"].unlock();
            }
            task2 = false;
            if (lk.complete)
            {
                prestige_ups["转化"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
            }
            #endregion


            //0-6 战斗增幅
            #region
            level = prestige_ups["制造"].level;
            m.转生_升级_战斗增幅_grid.Visibility = 0;
            #endregion

            //制造——战斗增幅
            #region
            lk = links["制造_战斗增幅"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_制造_grid;
            lk.b = m.转生_升级_战斗增幅_grid;
            lk.update_progress(level);
            if (lk.complete)
            {
                prestige_ups["战斗增幅"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
                m.转生_升级_强化等级_grid.Visibility = 0;
                task1 = true;
                //links["战斗增幅_强化等级"].unlock();
            }
            #endregion




            //0-5 强化等级
            #region
            level = prestige_ups["战斗增幅"].level;
            #endregion

            //战斗增幅——强化等级
            #region
            lk = links["战斗增幅_强化等级"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_战斗增幅_grid;
            lk.b = m.转生_升级_强化等级_grid;
            lk.update_progress(level);
            if (task1)
            {
                links["战斗增幅_强化等级"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["强化等级"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
                m.转生_升级_战斗探索_grid.Visibility = 0;
                task1 = true;
                //links["强化等级_战斗探索"].unlock();
            }
            #endregion

            //0-4 战斗探索
            #region
            level = prestige_ups["强化等级"].level;
            #endregion

            //强化等级——战斗探索
            #region
            lk = links["强化等级_战斗探索"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_强化等级_grid;
            lk.b = m.转生_升级_战斗探索_grid;
            lk.update_progress(level);
            if (task1)
            {
                links["强化等级_战斗探索"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["战斗探索"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
                m.转生_升级_冷静_grid.Visibility = 0;
                task1 = true;
                //links["战斗探索_冷静"].unlock();
            }
            #endregion

            //0-3 冷静
            #region
            level = prestige_ups["战斗探索"].level;
            #endregion

            //战斗探索——冷静
            #region
            lk = links["战斗探索_冷静"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_战斗探索_grid;
            lk.b = m.转生_升级_冷静_grid;
            lk.update_progress(level);
            if (task1)
            {
                links["战斗探索_冷静"].unlock();
            }
            task1 = false;
            if (lk.complete)
            {
                prestige_ups["冷静"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
            }
            #endregion

            //制造——核心
            #region
            lk = links["制造_核心"];
            lk.s = m.转生_main_grid;
            lk.a = m.转生_升级_制造_grid;
            lk.b = m.转生_升级_核心_grid;
            lk.update_progress(level);
            if (lk.complete)
            {
                prestige_ups["核心"].unlocked = true;
                //引出接下来的升级（及链接） 设定vis;
            }
            #endregion

            //5-0 成就加成
            m.转生_升级_成就加成_grid.Visibility = 0;
            #endregion

            //成就
            #region
            achieve_generate();

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

            float_messages = new List<float_message>();

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
        }
    }
}
