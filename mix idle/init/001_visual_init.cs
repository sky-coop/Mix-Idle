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
        public void visual_init()
        {
            m.详细信息框.Visibility = (Visibility)2;

            int i = 0;

            foreach (Grid g in m.制造_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }

            #region 战斗
            i = 0;
            foreach (Grid g in m.战斗_option_grid.Children)
            {
                if (unlocks.fight_unlock[i])
                {
                    g.Visibility = 0;
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
            #endregion 战斗


            #region 魔法
            foreach (Grid g in m.魔法_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            if (unlocks.potion)
            {
                m.魔法_菜单_药水_grid.Visibility = 0;
            }
            else
            {
                m.魔法_菜单_药水_grid.Visibility = (Visibility)1;
            }
            #endregion 魔法


            foreach (Grid g in m.采矿_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }



            制造_options = make_group(m.制造_option_grid);

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
            //战斗_玩家_攻击风格_自动_重击
            attack_form af = you.auto_attack_form;
            if (af != null)
            {
                group_process(战斗_自动攻击风格, (Rectangle)(m.FindName("战斗_玩家_攻击风格_自动_" + af.name)),
                    false, getSCB(Color.FromRgb(100, 255, 100)));
            }
            else
            {
                group_process(战斗_自动攻击风格, m.战斗_玩家_攻击风格_自动_普通,
                    false, getSCB(Color.FromRgb(100, 255, 100)));
            }
            //no.3 战斗:
            #region
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
            #endregion

            魔法_options = make_group(m.魔法_option_grid);
            采矿_options = make_group(m.采矿_option_grid);

            vm_elem_init();


            achieve_generate();
        }
    }
}
