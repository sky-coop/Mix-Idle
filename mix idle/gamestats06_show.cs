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
        object can_upgrade = new object(), cannot_upgrade = new object();
        string current_group = "方块";
        string current_show_sub_res_group = "";
        double2 pp_gain = 0;
        private void show()
        {
            bool num_selecter = s_ticker("num_selecter", 0.1);
            if (num_selecter)
            {
                if (m.number_select_grid.Visibility != 0)
                {
                    m.number_select_grid.Visibility = 0;
                }
            }
            bool show_res_second = (current_group != current_show_sub_res_group) || (current_show_sub_res_group == "");
            m.res_text1.Text = current_group + "：";

            foreach (TextBlock t in m.res_grid1.Children)
            {
                if (t.Text != "")
                {
                    t.Text = "";
                }
            }
            foreach (TextBlock t in m.res_grid2.Children)
            {
                if (t.Text != "")
                {
                    t.Text = "";
                }
            }
            //主资源显示
            foreach (KeyValuePair<string, resource> r in res_table[current_group])
            {
                if (r.Value.show_loc != 0 && r.Value.unlocked)
                {
                    string str = "res_text1_" + make_text<int>(r.Value.show_loc);
                    TextBlock t = (TextBlock)m.FindName(str);
                    t.Text = r.Value.name + "：" + number_format(r.Value.get_value());
                    t.Foreground = r.Value.text_color();
                }
            }

            //备用资源显示
            if (show_res_second)
            {
                if (current_show_sub_res_group != "")
                {
                    m.res_text2.Text = current_show_sub_res_group + "：";
                    foreach (KeyValuePair<string, resource> r in res_table[current_show_sub_res_group])
                    {
                        if (r.Value.show_loc != 0 && r.Value.unlocked)
                        {
                            string str = "res_text2_" + make_text(r.Value.show_loc);
                            TextBlock t = (TextBlock)m.FindName(str);
                            t.Text = r.Value.name + "：" + number_format(r.Value.get_value());
                            t.Foreground = r.Value.text_color();
                        }
                    }
                }

            }
            else
            {
                m.res_text2.Text = "";
            }

            
            if (Mouse.DirectlyOver != null && Mouse.DirectlyOver is Rectangle)
            {
                string name = ((Rectangle)(Mouse.DirectlyOver)).Name;
                string[] strs = name.Split('_');
                Rectangle background = null;
                TextBlock textBlock = null;
                if (strs[0] == "heater")   //自己创建的
                {
                    if(framework_elements.ContainsKey(name + "_bg"))
                    {
                        background = (Rectangle)framework_elements[name + "_bg"];
                    }
                    if (framework_elements.ContainsKey(name + "_text"))
                    {
                        textBlock = (TextBlock)framework_elements[name + "_text"];
                    }
                }
                else
                {
                    background = (Rectangle)m.FindName(name + "_背景");
                    textBlock = (TextBlock)m.FindName(name + "_文字");
                }
                if (background != null)
                {
                    m.详细信息框.Background = background.Fill;
                    m.详细信息框.Foreground = textBlock.Foreground;
                    current_information = name;
                }
            }

            if (current_information != "")
            {
                Point pt = Mouse.GetPosition(null);
                double x1 = pt.X + 20;
                double y1 = pt.Y + 20;

                if (m.详细信息框.ActualWidth + x1 + 40 > m.主窗口.ActualWidth)
                {
                    x1 = x1 - m.详细信息框.ActualWidth - 30;
                }
                if (m.详细信息框.ActualHeight + y1 + 40 > m.主窗口.ActualHeight)
                {
                    y1 = y1 - m.详细信息框.ActualHeight + 20;
                }

                if (x1 < 0)
                {
                    x1 = 0;
                }
                if (x1 > m.主窗口.ActualWidth)
                {
                    x1 = m.主窗口.ActualWidth;
                }

                if (y1 < 0)
                {
                    y1 = 0;
                }
                if (y1 > m.主窗口.ActualHeight)
                {
                    y1 = m.主窗口.ActualHeight;
                }

                m.详细信息框.Margin = new Thickness(x1, y1, 0, 0);

            }




            //方块
            if (current_group == "方块")
            {
                {
                    string[] strs = current_information.Split('_');
                    if (strs.Length == 3)
                    {
                        if (strs[2] == "详细信息")
                        {
                            方块_介绍(block_producters[strs[1]]);
                        }
                        if (strs[2] == "收集")
                        {
                            方块_收集信息(block_producters[strs[1]]);
                        }
                        if (strs[2] == "升级")
                        {
                            方块_升级信息(block_producters[strs[1]]);
                        }
                    }
                    if (s_ticker("block_upgrade", 0.1))
                    {
                        if (!upgrading)
                        {
                            foreach (KeyValuePair<string, block_producter> kp in block_producters)
                            {
                                double2 cost = kp.Value.cost;
                                double2 use = find_resource(kp.Value.cost_res_type).get_value();
                                if (buy_int && buy_number > 1)
                                {
                                    cost = cost * (double2.Pow(kp.Value.cost_exponent, buy_number) - 1) / (kp.Value.cost_exponent - 1);
                                }
                                if (!buy_int)
                                {
                                    use *= (buy_percent / 100.0);
                                }
                                if (use >= cost)
                                {
                                    ((Rectangle)(m.FindName("方块_" + kp.Value.name + "_升级_背景"))).Fill = getSCB(Color.FromRgb(0, 255, 0));
                                    ((Rectangle)(m.FindName("方块_" + kp.Value.name + "_升级"))).Tag = can_upgrade;
                                }
                                else
                                {
                                    ((Rectangle)(m.FindName("方块_" + kp.Value.name + "_升级_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                                    ((Rectangle)(m.FindName("方块_" + kp.Value.name + "_升级"))).Tag = cannot_upgrade;
                                }
                            }
                        }
                    }

                    if (s_ticker("block_progress_bar", 10))
                    {
                        foreach (KeyValuePair<string, block_producter> kp in block_producters)
                        {
                            string name = kp.Value.name;
                            Rectangle r = (Rectangle)m.FindName("方块_" + name + "_进度条_顶");
                            r.Fill = find_resource(name).text_color();
                        }
                    }
                }
            }

            //制造
            else if (current_group == "制造")
            {
                if (current_information == "制造_菜单_材料")
                {
                    m.详细信息框.Text = "制造材料的地方，材料可以用来制造其他东西。\n另外，请注意材料窗口上的价格公式！";
                }
                if (current_information == "制造_菜单_工具")
                {
                    m.详细信息框.Text = "制造各种各样的工具。";
                }
                if (current_information == "制造_菜单_升级器")
                {
                    m.详细信息框.Text = "升级器可以大幅提升某些属性。";
                }
                if (current_information == "制造_菜单_食物")
                {
                    m.详细信息框.Text = "食物可以给你能量，能量可用于提升游戏速度。";
                }

                List<string> cost_res_type = new List<string>();


                if (current_information.Length > 8 && current_information.Substring(0, 8) == "制造_升级_材料")
                {
                    string[] strs = current_information.Split('_');
                    string material_name = strs[3];
                    upgrade u = find_upgrade(material_name);
                    double2 can_buy = can_buy_material_num(u);
                    double2 cost = buy_material_cost(u, 0);

                    m.详细信息框.Text = "使用 " + number_format(cost) + " " + u.get_auto_res() + "制造 " + number_format(can_buy) + " " + u.name + ".";
                }
                else if (current_information.Length > 5 && current_information.Substring(0, 5) == "制造_升级")
                {
                    string[] strs = current_information.Split('_');
                    string upgrade_name = strs[3];
                    upgrade u = find_upgrade(upgrade_name);

                    double2 will_buy = buy_number;
                    if (!buy_int)
                    {
                        will_buy = can_buy_upgrade_num(u);
                    }
                    if (will_buy > u.max_level - u.level)
                    {
                        will_buy = u.max_level - u.level;
                    }

                    m.详细信息框.Text = "提升 " + number_format(will_buy) + "级，花费为：" + upgrade_string_show(buy_upgrade_cost(u, will_buy));
                }

                if (s_ticker("craft_upgrade", 0.1))
                {
                    if (!upgrading)
                    {
                        foreach (Grid g in m.制造_主_grid.Children)
                        {
                            if (g.Name == "制造_菜单_材料_target_grid")
                            {
                                //制造材料显示
                                foreach (Grid g1 in g.Children)
                                {
                                    string res = g1.Name.Split('_')[3];
                                    string base_name = "制造_次_材料_" + res;
                                    string cover_name = "制造_升级_材料_" + res;
                                    upgrade u = find_upgrade(res);
                                    double2 will_buy = buy_number;
                                    if (!buy_int)
                                    {
                                        will_buy = can_buy_material_num(u);
                                    }
                                    double2 cost = buy_material_cost(u, will_buy);
                                    ((TextBlock)m.FindName(base_name + "_text")).Text = res;
                                    ((TextBlock)m.FindName(base_name + "_text")).Foreground = find_resource(res).text_color();

                                    ((TextBlock)m.FindName(base_name + "_数量")).Text = "制造: " + number_format(will_buy) + " " + res;
                                    ((TextBlock)m.FindName(base_name + "_数量")).Foreground = find_resource(res).text_color();

                                    ((TextBlock)m.FindName(base_name + "_价格")).Text = "花费: " + number_format(cost) + " " + u.cost_table[0][0].Item1;
                                    ((TextBlock)m.FindName(base_name + "_价格")).Foreground = find_resource(u.cost_table[0][0].Item1).text_color();

                                    ((TextBlock)m.FindName(base_name + "_公式")).Text = "C(n) = " + number_format(u.factor) + " * n ^ " + number_format(u.exponent);
                                    if (can_buy_material_num(u) == 0)
                                    {
                                        ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                                        ((Rectangle)(m.FindName(cover_name))).Tag = cannot_upgrade;
                                    }
                                    else
                                    {
                                        ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(0, 255, 0));
                                        ((Rectangle)(m.FindName(cover_name))).Tag = can_upgrade;
                                    }
                                }

                            }
                            //制造升级显示
                            else
                            {
                                //g:  制造_菜单_食物_target_grid
                                //g2: 制造_次_食物_缤纷沙拉_grid
                                foreach (Grid g2 in g.Children)
                                {
                                    string type = g2.Name.Split('_')[2];
                                    string 后缀 = g2.Name.Split('_')[3];
                                    string base_name = "制造_次_" + type + "_" + 后缀;
                                    string cover_name = "制造_升级_" + type + "_" + 后缀;
                                    upgrade u = find_upgrade(后缀);
                                    ((TextBlock)m.FindName(base_name + "_text")).Text = u.name;
                                    ((TextBlock)m.FindName(base_name + "_等级")).Text = "等级 " + number_format(u.level) + " / " + number_format(u.max_level);
                                    ((TextBlock)m.FindName(base_name + "_描述")).Text = u.description[u.level];
                                    if (u.level == u.max_level)
                                    {
                                        ((TextBlock)m.FindName(base_name + "_价格_1")).Text = "";
                                        ((TextBlock)m.FindName(base_name + "_价格_2")).Text = "";
                                        ((TextBlock)m.FindName(base_name + "_价格_3")).Text = "";
                                    }
                                    else
                                    {
                                        ((TextBlock)m.FindName(base_name + "_价格_1")).Text = number_format(u.cost_table[u.level][0].Item2) + " " + u.cost_table[u.level][0].Item1;
                                        ((TextBlock)m.FindName(base_name + "_价格_1")).Foreground = find_resource(u.cost_table[u.level][0].Item1).text_color();
                                        if (u.cost_table[u.level].Count > 1)
                                        {
                                            ((TextBlock)m.FindName(base_name + "_价格_2")).Text = number_format(u.cost_table[u.level][1].Item2) + " " + u.cost_table[u.level][1].Item1;
                                            ((TextBlock)m.FindName(base_name + "_价格_2")).Foreground = find_resource(u.cost_table[u.level][1].Item1).text_color();
                                        }
                                        else
                                        {
                                            ((TextBlock)m.FindName(base_name + "_价格_2")).Text = "";
                                        }
                                        if (u.cost_table[u.level].Count > 2)
                                        {
                                            ((TextBlock)m.FindName(base_name + "_价格_3")).Text = number_format(u.cost_table[u.level][2].Item2) + " " + u.cost_table[u.level][2].Item1;
                                            ((TextBlock)m.FindName(base_name + "_价格_3")).Foreground = find_resource(u.cost_table[u.level][2].Item1).text_color();
                                        }
                                        else
                                        {
                                            ((TextBlock)m.FindName(base_name + "_价格_3")).Text = "";
                                        }
                                    }

                                    if (can_buy_upgrade_num(u) == 0 || (buy_int && (can_buy_upgrade_num(u) < buy_number)))
                                    {
                                        ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                                        ((Rectangle)(m.FindName(cover_name))).Tag = cannot_upgrade;
                                    }
                                    else
                                    {
                                        ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(0, 255, 0));
                                        ((Rectangle)(m.FindName(cover_name))).Tag = can_upgrade;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //战斗
            else if (current_group == "战斗")
            {
                if (current_information == "战斗_场景_洁白世界")
                {
                    m.详细信息框.Text = "一个洁白的世界，充满了白色的非生命敌人。";
                }
                else if (current_information == "战斗_场景_草原")
                {
                    m.详细信息框.Text = "你能在其中感受到生命的气息。";
                }
                else if (current_information == "战斗_场景_死火山")
                {
                    m.详细信息框.Text = "这座死火山上有令人意想不到的东西。";
                }
                else if (current_information == "战斗_场景_机关屋")
                {
                    m.详细信息框.Text = "无人的机关屋，不知道是为了什么而建的。";
                }
                else if (current_information == "战斗_场景_魔境")
                {
                    m.详细信息框.Text = "巨大的能量流让深藏于地底的传送门被你找到。";
                }
                else if (current_information == "战斗_场景_information_fight")
                {
                    m.详细信息框.Text = "开始战斗。若战斗已开始，则停止战斗。\n（放置类游戏需要暂停？这是有原因的）";
                }
                else if (current_information == "战斗_场景_information_leveldown")
                {
                    double2 will_change = buy_number;
                    if (!buy_int)
                    {
                        double2 wn = enemy.current.max_level * buy_percent / 100.0;
                        will_change = wn.floor();
                    }
                    if (will_change > enemy.current.level - 1)
                    {
                        will_change = enemy.current.level - 1;
                    }
                    if (will_change < 1)
                    {
                        will_change = 1;
                    }
                    m.详细信息框.Text = "将当前敌人的等级降低 " + will_change + " 级。";
                }
                else if (current_information == "战斗_场景_information_levelup")
                {
                    double2 will_change = buy_number;
                    if (!buy_int)
                    {
                        double2 wn = enemy.current.max_level * buy_percent / 100.0;
                        will_change = wn.floor();
                    }
                    if (will_change > enemy.current.max_level - 
                        enemy.current.level)
                    {
                        will_change = enemy.current.max_level - 
                            enemy.current.level;
                    }
                    if (will_change < 1)
                    {
                        will_change = 1;
                    }
                    m.详细信息框.Text = "将当前敌人的等级提升 " + will_change + " 级。";
                }
                else if (current_information.Split('_').Length > 3 
                    && current_information.Split('_')[3] == "自动")
                {
                    string af_name = current_information.Split('_')[4];
                    attack_form af = attack_forms[af_name];

                    double2 damage_multi = af.attack_factor;
                    if (af.name == "连斩")
                    {
                        damage_multi = 连斩_count * 连斩_damage_boost + 连斩_damage_base;
                        if (damage_multi > 连斩_max_damage)
                        {
                            damage_multi = 连斩_max_damage;
                        }
                    }

                    m.详细信息框.Text = "自动攻击模式: " + af_name + "。\n" +
                                      "将攻击力×" + number_format(damage_multi) + "，攻击时间×" + number_format(af.at_factor) + "，减速值恢复速度×" + number_format(af.sr_factor) + "。";
                }
                else if (current_information.Split('_').Length > 3 
                    && current_information.Split('_')[3] == "手动")
                {
                    string af_name = current_information.Split('_')[4];
                    attack_form af = attack_forms[af_name];
                    m.详细信息框.Text = "手动攻击模式: " + af_name + "。\n" +
                                      "攻击力是普通攻击的 " + number_format(af.attack_factor * 100) + "%，冷却时间为 " + number_format(af.manual_anti_fast_click_cd) + "s。";
                }

                if(current_information.Split('_').Length > 3 && 
                    (current_information.Split('_')[3] == "手动" 
                    || current_information.Split('_')[3] == "自动"))
                {
                    string af_name = current_information.Split('_')[4];
                    attack_form af = attack_forms[af_name];

                    if (af.def_pierce_percent + you.pierce_percent > 0)
                    {
                        m.详细信息框.Text += "\n" + number_format((af.def_pierce_percent + you.pierce_percent) * 100) + "%";
                        if (af.def_pierce > 0)
                        {
                            m.详细信息框.Text += " + ";
                        }
                    }
                    if (af.def_pierce > 0)
                    {
                        if (af.def_pierce_percent + you.pierce_percent <= 0)
                        {
                            m.详细信息框.Text += "\n";
                        }
                        m.详细信息框.Text += number_format(af.def_pierce);
                    }
                    if (af.def_pierce_percent + you.pierce_percent > 0 || af.def_pierce > 0)
                    {
                        m.详细信息框.Text += " 的攻击力穿透敌人防御（真实伤害，不会超过攻击力）";
                    }

                    if (af.def_ignore_percent > 0)
                    {
                        m.详细信息框.Text += "\n" + number_format(af.def_ignore_percent * 100) + "%";
                        if (af.def_ignore > 0)
                        {
                            m.详细信息框.Text += " + ";
                        }
                    }
                    if (af.def_ignore > 0)
                    {
                        if (af.def_ignore_percent <= 0)
                        {
                            m.详细信息框.Text += "\n";
                        }
                        m.详细信息框.Text += number_format(af.def_ignore);
                    }
                    if (af.def_ignore_percent > 0 || af.def_ignore > 0)
                    {
                        m.详细信息框.Text += " 的敌人防御被无视（防御降低）";
                    }


                    if (af.def_down_percent > 0 || af.def_down > 0)
                    {
                        m.详细信息框.Text += "\n攻击后，降低敌人 ";
                    }
                    if (af.def_down_percent > 0)
                    {
                        m.详细信息框.Text += number_format(af.def_down_percent * 100) + "%";
                        if (af.def_down > 0)
                        {
                            m.详细信息框.Text += " + ";
                        }
                    }
                    if (af.def_down > 0)
                    {
                        m.详细信息框.Text += number_format(af.def_down);
                    }
                    if (af.def_down_percent > 0 || af.def_down > 0)
                    {
                        m.详细信息框.Text += " 的防御（永久降防）";
                    }


                    if (af.name == "连斩")
                    {
                        m.详细信息框.Text += "\n攻击后若没有击败本名敌人，伤害+" + number_format(连斩_damage_boost) + "倍，最大 " + number_format(连斩_max_damage) + " 倍（×" + number_format(连斩_max_damage) + "）";
                    }
                    if (af.overkill > 0)
                    {
                        m.详细信息框.Text += "\n攻击后若击败了本名敌人，" + number_format(af.overkill * 100) + "%的溢出伤害将攻击下一个敌人";
                    }
                }


                if (enemy.current != null)
                {
                    enemy_show();
                }
                player_show();
                fight_update();
            }
            //魔法
            else if (current_group == "魔法")
            {
                if (current_information == "魔法_菜单_祭坛")
                {
                    m.详细信息框.Text = "献上一些有魔法潜力的物品来换取魔力和其他奖励。";
                }
                else if (current_information == "魔法_菜单_附魔")
                {
                    m.详细信息框.Text = "对物品进行附魔以获得魔法物品。";
                }
                else if (current_information == "魔法_菜单_法术")
                {
                    m.详细信息框.Text = "各种各样的法术会提升你的各种属性。";
                }
                else if (current_information == "魔法_菜单_药水")
                {
                    m.详细信息框.Text = "亲自调制药水并立刻喝下去，获取属性。";
                }

                string[] sp = current_information.Split('_');
                if (sp.Length > 4 && sp[4] == "献祭")
                {
                    string 祭品_name = sp[3];
                    resource 祭品 = find_resource(祭品_name);
                    double2 amount = buy_number;
                    if (!buy_int)
                    {
                        amount = 祭品.get_value() * buy_percent / 100.0;
                    }
                    m.详细信息框.Text = "献上 " + number_format(amount) + " " + 祭品_name + " 以获得 " + number_format(amount * magic_altar.power_table[祭品_name] * magic_altar.get_power_mul()) + " 点祭坛能量";
                }

                if (sp.Length == 4 && sp[1] == "附魔")
                {
                    enchant ec = enchants[sp[2]];
                    double2 change = buy_number;
                    if (!buy_int)
                    {
                        change = (ec.max_level * (double2)buy_percent / 100.0).floor();
                    }
                    if ((sp[3] == "降级" || sp[3] == "减速") && change > ec.level - 1)
                    {
                        change = ec.level - 1;
                    }
                    if ((sp[3] == "升级" || sp[3] == "加速") && change > ec.max_level - ec.level)
                    {
                        change = ec.max_level - ec.level;
                    }
                    if (sp[3] == "减速")
                    {
                        m.详细信息框.Text = "降低速度等级 " + number_format(change) + " 级。";
                    }
                    if (sp[3] == "加速")
                    {
                        m.详细信息框.Text = "增加速度等级 " + number_format(change) + " 级。";
                    }

                    if (sp[3] == "降级")
                    {
                        m.详细信息框.Text = "降低等级 " + number_format(change) + " 级。";
                    }
                    if (sp[3] == "升级")
                    {
                        m.详细信息框.Text = "增加等级 " + number_format(change) + " 级。";
                    }
                }

                if (sp.Length == 4 && sp[3] == "学习")
                {
                    string 法术_name = sp[2];
                    spell s = (spell)(find_upgrade(法术_name));
                    if (!s.study_active)
                    {
                        m.详细信息框.Text = "开始学习此法术。";
                    }
                    else
                    {
                        m.详细信息框.Text = "停止学习此法术。";
                    }
                }

                if (sp.Length == 4 && sp[3] == "施法")
                {
                    string 法术_name = sp[2];
                    spell s = (spell)(find_upgrade(法术_name));
                    if (!s.cast_active)
                    {
                        m.详细信息框.Text = "开始施放此法术。";
                    }
                    else
                    {
                        m.详细信息框.Text = "停止施放此法术。";
                    }
                }

                //魔法_药水_攻击药水_降级
                if (sp.Length == 4 && sp[1] == "药水" && sp[3] == "降级")
                {
                    string name = sp[2];
                    enchant e = enchants[name];

                    double2 will_change = buy_number;
                    if (!buy_int)
                    {
                        will_change = (e.max_level * (double2)buy_percent / 100.0).floor();
                    }
                    if (will_change > e.level - 1)
                    {
                        will_change = e.level - 1;
                    }
                    if (will_change < 1)
                    {
                        will_change = 1;
                    }
                    m.详细信息框.Text = "将当前药水的等级降低 " + will_change + " 级。";
                }
                else if (sp.Length == 4 && sp[1] == "药水" && sp[3] == "升级")
                {
                    string name = sp[2];
                    enchant e = enchants[name];

                    double2 will_change = buy_number;
                    if (!buy_int)
                    {
                        will_change = (e.max_level * (double2)buy_percent / 100.0).floor();
                    }
                    if (will_change > e.max_level - e.level)
                    {
                        will_change = e.max_level - e.level;
                    }
                    if (will_change < 1)
                    {
                        will_change = 1;
                    }
                    m.详细信息框.Text = "将当前药水的等级提升 " + will_change + " 级。";
                }

                //魔法_法术_白色魔法_施法_升级
                if (sp.Length == 5 && sp[1] == "法术" && sp[4] == "降级")
                {
                    string name = sp[2];
                    upgrade e = upgrades[name];

                    int will_change = buy_number;
                    if (!buy_int)
                    {
                        will_change = (int)(e.max_level * buy_percent / 100.0);
                    }
                    if (will_change > e.level - 1)
                    {
                        will_change = e.level - 1;
                    }
                    if (will_change < 1)
                    {
                        will_change = 1;
                    }
                    m.详细信息框.Text = "将当前法术的等级降低 " + will_change + " 级。";
                }
                else if (sp.Length == 5 && sp[1] == "法术" && sp[4] == "升级")
                {
                    string name = sp[2];
                    upgrade e = upgrades[name];

                    int will_change = buy_number;
                    if (!buy_int)
                    {
                        will_change = (int)(e.max_level * buy_percent / 100.0);
                    }
                    if (will_change > e.max_level - e.level)
                    {
                        will_change = e.max_level - e.level;
                    }
                    if (will_change < 1)
                    {
                        will_change = 1;
                    }
                    m.详细信息框.Text = "将当前法术的等级提升 " + will_change + " 级。";
                }

                if (current_information == "魔法_祭坛_升级")
                {
                    decimal will_buy = can_buy_upgrade_num(upgrades["祭坛升级"]);
                    m.详细信息框.Text = "将祭坛等级提升 " + will_buy + " 级。";
                }

                if (s_ticker("magic_upgrade", 0.1))
                {
                    if (!upgrading)
                    {
                        foreach (Grid g in m.魔法_祭坛_祭品_grid.Children)
                        {
                            string[] gs = g.Name.Split('_');
                            string 祭品_name = gs[3];
                            if (find_resource(祭品_name).unlocked)
                            {
                                ((Grid)m.FindName("魔法_祭坛_祭品_" + 祭品_name + "_grid")).Visibility = 0;
                            }
                            else
                            {
                                ((Grid)m.FindName("魔法_祭坛_祭品_" + 祭品_name + "_grid")).Visibility = (Visibility)1;
                            }

                            ((TextBlock)m.FindName("魔法_祭坛_祭品_" + 祭品_name + "_text")).Text = "你有 " + number_format(find_resource(祭品_name).get_value()) + " " + 祭品_name;

                            string cover_name = "魔法_祭坛_祭品_" + 祭品_name + "_献祭";
                            if (find_resource(祭品_name).get_value() == 0)
                            {
                                ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                                ((Rectangle)(m.FindName(cover_name))).Tag = cannot_upgrade;
                            }
                            else
                            {
                                ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(0, 255, 0));
                                ((Rectangle)(m.FindName(cover_name))).Tag = can_upgrade;
                            }
                        }
                    }
                }
                m.魔法_祭坛_text1.Text = magic_altar.mode;
                m.魔法_祭坛_text2.Text = number_format(magic_altar.power) + " 祭坛能量";
                m.魔法_祭坛_text3.Text = "";
                if (magic_altar.mode == "魔力祭坛")
                {
                    m.魔法_祭坛_text3.Text = "+" + number_format(alter_effect()) + "魔力/s";
                }

                if (!upgrading)
                {
                    upgrade u = find_upgrade("祭坛升级");
                    m.魔法_祭坛_升级_text.Text = "祭坛等级 " + number_format(u.level) + " / " + number_format(u.max_level);
                    m.魔法_祭坛_升级_已有效果T.Text = u.description[u.level];
                    m.魔法_祭坛_升级_效果T.Text = u.description2[u.level];
                    if (u.level == u.max_level)
                    {
                        m.魔法_祭坛_升级_消耗1.Text = "无";
                        m.魔法_祭坛_升级_消耗2.Text = "";
                        m.魔法_祭坛_升级_消耗3.Text = "";
                        m.魔法_祭坛_升级_消耗4.Text = "";
                        m.魔法_祭坛_升级_消耗5.Text = "";
                        m.魔法_祭坛_升级_消耗6.Text = "";
                        m.魔法_祭坛_升级_消耗7.Text = "";
                    }
                    else
                    {
                        int i = 0;
                        for (; i < u.cost_table[u.level].Count; i++)
                        {
                            int n = i + 1;
                            Tuple<string, double2> cost = u.cost_table[u.level][i];
                            resource r = find_resource(cost.Item1);
                            ((TextBlock)m.FindName("魔法_祭坛_升级_消耗" + make_text(n))).Text = number_format(cost.Item2) + " " + cost.Item1;
                            ((TextBlock)m.FindName("魔法_祭坛_升级_消耗" + make_text(n))).Foreground = r.text_color();
                        }
                        for (; i < 7; i++)
                        {
                            int n = i + 1;
                            ((TextBlock)m.FindName("魔法_祭坛_升级_消耗" + make_text(n))).Text = "";
                        }
                    }

                    if (can_buy_upgrade_num(u) == 0 || (buy_int && (can_buy_upgrade_num(u) < buy_number)))
                    {
                        m.魔法_祭坛_升级_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        m.魔法_祭坛_升级.Tag = cannot_upgrade;
                    }
                    else
                    {
                        m.魔法_祭坛_升级_背景.Fill = getSCB(Color.FromRgb(0, 255, 0));
                        m.魔法_祭坛_升级.Tag = can_upgrade;
                    }
                }

                foreach (Grid g in m.魔法_菜单_附魔_target_grid.Children)
                {
                    string obj_name = g.Name.Split('_')[3];
                    string cover_name = "魔法_附魔_" + obj_name;
                    enchant ec = enchants[obj_name];

                    ((TextBlock)m.FindName(cover_name + "_text")).Text = obj_name;
                    ((TextBlock)m.FindName(cover_name + "_text")).Foreground = find_resource(obj_name).text_color();
                    ((TextBlock)m.FindName(cover_name + "_速度等级")).Text = "速度等级 " + number_format(ec.level) + " / " + number_format(ec.max_level);

                    int i = 0;
                    for (; ec.get_cost(i) != null; i++)
                    {
                        int n = i + 1;
                        Tuple<string, double2> cost = ec.get_cost(i);
                        resource r = find_resource(cost.Item1);
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n))).Text = number_format(cost.Item2) + " " + cost.Item1;
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n))).Foreground = r.text_color();

                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n) + "s")).Text = "(" + number_format(cost.Item2 / ec.get_time()) + "/s)";
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n) + "s")).Foreground = r.text_color();
                    }
                    for (; i < 3; i++)
                    {
                        int n = i + 1;
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n))).Text = "";
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n) + "s")).Text = "";
                    }
                    ((TextBlock)m.FindName(cover_name + "_数量")).Text = number_format(ec.get_produce() * find_resource(obj_name).get_mul()) + " " + ec.produce_res;
                    ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = find_resource(ec.produce_res).text_color();
                    ((TextBlock)m.FindName(cover_name + "_数量" + "s")).Text = "(" + number_format(ec.get_produce() * find_resource(obj_name).get_mul() / ec.get_time()) + "/s)";
                    ((TextBlock)m.FindName(cover_name + "_数量" + "s")).Foreground = find_resource(ec.produce_res).text_color();

                    ((TextBlock)m.FindName(cover_name + "_时间")).Text = "附魔时间: " + number_format(ec.current_time) + " / " + number_format(ec.get_time());

                    Rectangle top = (Rectangle)m.FindName(cover_name + "_进度条_顶");
                    Rectangle bottom = (Rectangle)m.FindName(cover_name + "_进度条_底");
                    double2 progress = ec.current_time / ec.get_time();
                    if (progress > 1)
                    {
                        progress = 1;
                    }
                    if (ec.active && progress == 0 && time_a_tick_game > ec.get_time())
                    {
                        progress = 1;
                        ((TextBlock)m.FindName(cover_name + "_时间")).Text = "附魔时间: " + number_format(ec.get_time()) + " / " + number_format(ec.get_time());
                    }
                    top.Width = bottom.Width * (1 - progress).d;

                    if (ec.level == 1)
                    {
                        ((Grid)m.FindName("魔法_附魔_" + obj_name + "_减速_grid")).Visibility = (Visibility)1;
                    }
                    else
                    {
                        ((Grid)m.FindName("魔法_附魔_" + obj_name + "_减速_grid")).Visibility = 0;
                    }

                    if (ec.level == ec.max_level)
                    {
                        ((Grid)m.FindName("魔法_附魔_" + obj_name + "_加速_grid")).Visibility = (Visibility)1;
                    }
                    else
                    {
                        ((Grid)m.FindName("魔法_附魔_" + obj_name + "_加速_grid")).Visibility = 0;
                    }

                    if (ec.can_buy)
                    {
                        ((Grid)m.FindName("魔法_次_附魔_" + obj_name + "_grid")).Background = get_lgb();

                    }
                    else
                    {
                        ((Grid)m.FindName("魔法_次_附魔_" + obj_name + "_grid")).Background = getSCB(Color.FromArgb(127, 0, 0, 0));
                    }
                }

                foreach (FrameworkElement f in m.魔法_菜单_法术_target_grid.Children)
                {
                    Grid g = null;
                    if (f is Grid)
                    {
                        g = (Grid)f;
                    }
                    else
                    {
                        continue;
                    }
                    string[] strs = g.Name.Split('_');
                    spell s = null;

                    if (strs[2].Substring(0, 1) == "第")
                    {
                        foreach (Grid g2 in g.Children)
                        {
                            string[] strs2 = g2.Name.Split('_');
                            s = ((spell)find_upgrade(strs2[2]));
                            spell_show2(s);
                        }
                    }
                    else
                    {
                        s = ((spell)find_upgrade(strs[2]));
                        spell_show2(s);
                    }
                    if (s == null)
                    {
                        continue;
                    }
                }
                foreach (Grid g in m.魔法_菜单_药水_target_grid.Children)
                {
                    string obj_name = g.Name.Split('_')[3];
                    string cover_name = "魔法_药水_" + obj_name;
                    enchant ec = enchants[obj_name];

                    ((TextBlock)m.FindName(cover_name + "_text")).Text = obj_name;
                    ((TextBlock)m.FindName(cover_name + "_等级")).Text = "等级 " + number_format(ec.level) + " / " + number_format(ec.max_level);

                    int i = 0;
                    for (; ec.get_cost(i) != null; i++)
                    {
                        int n = i + 1;
                        Tuple<string, double2> cost = ec.get_cost(i);
                        resource r = find_resource(cost.Item1);
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n))).Text = number_format(cost.Item2) + " " + cost.Item1;
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n))).Foreground = r.text_color();
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n) + "s")).Text = "(" + number_format(cost.Item2 / ec.get_time()) + "/s)";
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n) + "s")).Foreground = r.text_color();
                    }
                    for (; i < 3; i++)
                    {
                        int n = i + 1;
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n))).Text = "";
                        ((TextBlock)m.FindName(cover_name + "_价格" + make_text(n) + "s")).Text = "";
                    }

                    ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    string produce = "";
                    if (ec.name == "战斗经验药水")
                    {
                        produce = number_format(ec.get_effect_mul() * 1.5e6 * double2.Pow(30, ec.level) * you.get_exp_mul()) + " 战斗经验";
                        ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = getSCB(Color.FromRgb(0, 255, 0));
                    }
                    if (ec.name == "攻击药水")
                    {
                        produce = number_format(ec.get_effect_mul() * double2.Pow(1.5 + 0.1 * (double2)ec.level, 2)) + " 物品攻击";
                        ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = getSCB(Color.FromRgb(255, 0, 0));
                    }
                    if (ec.name == "魔力药水")
                    {
                        produce = number_format(ec.get_effect_mul() * 1200 * double2.Pow(11, ec.level) * find_resource("魔力").get_mul()) + " 魔力";
                        ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = find_resource("魔力").text_color();
                    }
                    if (ec.name == "烈焰药水")
                    {
                        produce = number_format(ec.get_effect_mul() * double2.Pow(0.2 + 0.01 * (double2)ec.level, 2)) + " 减速值下降速度";
                        ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = getSCB(Color.FromRgb(100, 200, 255));
                    }
                    if (ec.name == "幸运药水")
                    {
                        produce = number_format(ec.get_effect_mul() * double2.Pow(0.03 + 0.0025 * (double2)ec.level, 1.5)) + " 基础幸运值";
                        ((TextBlock)m.FindName(cover_name + "_数量")).Foreground = getSCB(Color.FromRgb(255, 200, 255));
                    }


                   ((TextBlock)m.FindName(cover_name + "_数量")).Text = produce;

                    ((TextBlock)m.FindName(cover_name + "_时间")).Text = "时间: " + number_format(ec.current_time) + " / " + number_format(ec.get_time());
                    

                    Rectangle top = (Rectangle)m.FindName(cover_name + "_进度条_顶");
                    Rectangle bottom = (Rectangle)m.FindName(cover_name + "_进度条_底");
                    double2 progress = ec.current_time / ec.get_time();
                    if (progress > 1)
                    {
                        progress = 1;
                    }
                    if (ec.active && progress == 0 && time_a_tick_game > ec.get_time())
                    {
                        progress = 1;
                        ((TextBlock)m.FindName(cover_name + "_时间")).Text = "附魔时间: " + number_format(ec.get_time()) + " / " + number_format(ec.get_time());
                    }
                    top.Width = bottom.Width * (1 - progress).d;

                    if (ec.changing_time)
                    {
                        ((TextBlock)m.FindName(cover_name + "_时间")).Text += " （随配制量增加而增加）";
                    }


                    if (ec.level == 1)
                    {
                        ((Grid)m.FindName("魔法_药水_" + obj_name + "_降级_grid")).Visibility = (Visibility)1;
                    }
                    else
                    {
                        ((Grid)m.FindName("魔法_药水_" + obj_name + "_降级_grid")).Visibility = 0;
                    }

                    if (ec.level == ec.max_level)
                    {
                        ((Grid)m.FindName("魔法_药水_" + obj_name + "_升级_grid")).Visibility = (Visibility)1;
                    }
                    else
                    {
                        ((Grid)m.FindName("魔法_药水_" + obj_name + "_升级_grid")).Visibility = 0;
                    }

                    if (ec.can_buy)
                    {
                        ((Grid)m.FindName("魔法_次_药水_" + obj_name + "_grid")).Background = ec.LinearGradientBrush;
                    }
                    else
                    {
                        ((Grid)m.FindName("魔法_次_药水_" + obj_name + "_grid")).Background = getSCB(Color.FromRgb(88, 0, 0));
                    }
                }
                spell_page_show();
            }
            //采矿
            else if (current_group == "采矿")
            {
                string[] ss = current_information.Split('_');
                if (ss[0] == "minefield")
                {
                    int i = Convert.ToInt32(ss[1]);
                    int j = Convert.ToInt32(ss[2]);

                    mine_cell mc = minef.graph[i, j];

                    string s = "";
                    bool gap = false;
                    if (mc.depth == int.MaxValue)
                    {
                        s = "已全部挖掘。";
                    }
                    else
                    {
                        s = "层数：" + number_format(mc.depth) + " / " + number_format(mc.max_depth) + "\n";
                        s += "幸运值：" + number_format(minef.luck * double2.Pow(mc.depth, minef.depth_luck_exponent)) + "\n";
                        s += "花费：" + number_format(mc.cost[mc.depth - 1]) + " 采矿点数\n";
                        s += "获得：" + number_format(mc.exp[mc.depth - 1] * global_xp_boost()) 
                            + " 采矿经验 (" + number_format(mc.exp[mc.depth - 1] * 
                            global_xp_boost() / mc.cost[mc.depth - 1]) + " 每采矿点)";
                        foreach (KeyValuePair<string, double2> kp in mc.loot[mc.depth - 1])
                        {
                            if (!gap && treasures.ContainsKey(kp.Key))
                            {
                                s += "\n";
                                gap = true;
                            }
                            s += "\n      " + number_format(kp.Value) + " " + kp.Key + " (" + number_format(kp.Value / mc.cost[mc.depth - 1]) + " 每采矿点)";
                        }
                    }

                    m.详细信息框.Text = s;
                }
                if (current_information == "采矿_数据_按键_挖掘顶层")
                {
                    int layer_2 = 1;

                    string s = "";
                    bool gap = false;
                    if (minef.get_all_top_index(layer_2).Count == 0)
                    {
                        s = "此面板已全部挖掘！";
                    }
                    else
                    {
                        int target = Math.Min(minef.get_depth_min() + layer_2 - 1, minef.max_depth);

                        int count = 0;
                        foreach(List<int> iter1 in minef.get_all_top_index(layer_2))
                        {
                            foreach(int iter2 in iter1)
                            {
                                count++;
                            }
                        }

                        s = "目标：挖掘 " + number_format(target) + " 层及之上的所有格子，共 " + number_format(count) + " 个\n";
                        s += "最大幸运值：" + number_format(minef.luck * double2.Pow(target, minef.depth_luck_exponent)) + "\n";
                        s += "花费：" + number_format(minef.get_all_top_cost(layer_2)) + " 采矿点数\n";
                        s += "获得：" + number_format(minef.get_all_top_exp(layer_2)) + " 采矿经验 (" 
                            + number_format(minef.get_all_top_exp(layer_2) / minef.get_all_top_cost(layer_2)) + " 每采矿点)";
                        foreach (KeyValuePair<string, double2> kp in minef.get_all_top_loot(layer_2))
                        {
                            if (!gap && treasures.ContainsKey(kp.Key))
                            {
                                s += "\n";
                                gap = true;
                            }
                            s += "\n      " + number_format(kp.Value) + " " + kp.Key + " (" + number_format(kp.Value / minef.get_all_top_cost(layer_2)) + " 每采矿点)";
                        }
                    }

                    m.详细信息框.Text = s;

                }

                if (current_information == "采矿_数据_按键_重新生成")
                {
                    string s = "";
                    s += "花费 " + number_format(minep.reset_cost) + " 采矿点数重新生成一个采矿区。\n";
                    s += "请注意，一旦采矿区生成，其数据不会发生任何改变，因此需要重新生成以刷新。";
                    m.详细信息框.Text = s;
                }
                if (current_information == "采矿_菜单_挖掘")
                {
                    string s = "挖掘泥土、石头以及矿物。";
                    m.详细信息框.Text = s;
                }
                if (current_information == "采矿_菜单_炼制")
                {
                    string s = "炼制挖到的矿物，或者炼制一些制造不出的道具。";
                    m.详细信息框.Text = s;
                }
                if (current_information == "采矿_菜单_宝物")
                {
                    string s = "查看和升级你挖到的宝物，还能查看每种矿物的幸运值需求。";
                    m.详细信息框.Text = s;
                }
                if (current_information == "采矿_炼制_熔炉升级")
                {
                    decimal will_buy = can_buy_upgrade_num(upgrades["熔炉升级"]);
                    m.详细信息框.Text = "将熔炉等级提升 " + will_buy + " 级。";
                }


                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        mine_cell mc = minef.graph[i, j];
                        if (mc.entering && 
                            mc.enter_not_change && 
                            mc.depth != int.MaxValue && 
                            res_table["采矿"]["采矿点数"].get_value() >= 
                            mc.cost[mc.depth - 1])
                        {
                            mine_enter(minefield_cover[i, j], null);
                            mine_move(minefield_cover[i, j], null);
                        }
                    }
                }

                m.采矿_数据_采矿等级_text.Text = "采矿等级 " + number_format(minep.level);

                m.采矿_数据_采矿经验_text.Text = "经验值：" + number_format(minep.exp) + " / " + number_format(minep.get_exp_to_level(1));
                m.采矿_数据_采矿经验_进度条_顶.Width = m.采矿_数据_采矿经验_进度条_底.Width * (minep.exp / minep.get_exp_to_level(1)).d;

                double2 mu = 1;
                double2 size = (1000 + minep.size_boost) * minep.get_size_mul();
                m.采矿_数据_格子边长_text.Text = "格子边长：" + number_format(size);
                m.采矿_数据_格子边长1_text.Text = "基础边长：" + number_format(1000);
                m.采矿_数据_格子边长2_text.Text = "增加：" + number_format(minep.size_boost);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.size_multi[0])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_格子边长3_text.Text = "等级增益：×" + number_format(mu);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.size_multi[1])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_格子边长4_text.Text = "物品增益：×" + number_format(mu);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.size_multi[2])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_格子边长5_text.Text = "其他增益：×" + number_format(mu);

                m.采矿_数据_格子边长current_text.Text = "当前：" + number_format(minef.cell_size);
                m.采矿_数据_每格资源_text.Text = "每格资源 = 边长 ^ 3 = " + number_format(size * size * size);


                m.采矿_数据_基础幸运_text.Text = "基础幸运：" + number_format((1 + minep.luck_boost) * minep.get_luck_mul());
                m.采矿_数据_基础幸运1_text.Text = "初始值：" + number_format(1);
                m.采矿_数据_基础幸运2_text.Text = "增加：" + number_format(minep.luck_boost);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.luck_multi[0])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_基础幸运3_text.Text = "等级增益：×" + number_format(mu);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.luck_multi[1])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_基础幸运4_text.Text = "物品增益：×" + number_format(mu);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.luck_multi[2])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_基础幸运5_text.Text = "其他增益：×" + number_format(mu);

                m.采矿_数据_基础幸运current_text.Text = "当前：" + number_format(minef.luck);
                m.采矿_数据_层数幸运_text.Text = "每层幸运 = 基础幸运 * 层数 ^ " + number_format(minep.depth_luck_exponent);


                m.采矿_数据_经验获取_text.Text = "基础经验：" + number_format((5 + minep.exp_boost) * minep.get_exp_mul());
                m.采矿_数据_经验获取1_text.Text = "初始值：" + number_format(5);
                m.采矿_数据_经验获取2_text.Text = "增加：" + number_format(minep.exp_boost);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.exp_multi[1])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_经验获取3_text.Text = "物品增益：×" + number_format(mu);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.exp_multi[2])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_经验获取4_text.Text = "其他增益：×" + number_format(mu);


                m.采矿_数据_采矿点获取_text.Text = "采矿点获取：" + number_format((1 + minep.point_boost) * minep.get_point_mul());
                m.采矿_数据_采矿点获取1_text.Text = "初始值：" + number_format(1);
                m.采矿_数据_采矿点获取2_text.Text = "增加：" + number_format(minep.point_boost);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.point_multi[1])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_采矿点获取3_text.Text = "物品增益：×" + number_format(mu);

                mu = 1;
                foreach (KeyValuePair<string, multiplier> kp in minep.point_multi[2])
                {
                    mu *= kp.Value.value;
                }
                m.采矿_数据_采矿点获取4_text.Text = "其他增益：×" + number_format(mu);

                if (!minef.holding && !minef.entering)
                {
                    if (res_table["采矿"]["采矿点数"].get_value() >= minep.reset_cost)
                    {
                        m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(0, 255, 255));
                    }
                    else
                    {
                        m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    }
                }
                m.采矿_数据_按键_重新生成_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));

                int layer = 1;
                if (!minef.holding2 && !minef.entering2)
                {
                    if (res_table["采矿"]["采矿点数"].get_value() >= minef.get_all_top_cost(layer))
                    {
                        m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(0, 255, 255));
                    }
                    else
                    {
                        m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    }
                }
                m.采矿_数据_按键_挖掘顶层_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));


                m.采矿_炼制_熔炉0_text.Text = "熔炉等级 " + number_format(furance.level);

                for (int i = 0; i < 4; i++)
                {
                    string name = "heater_0_" + Convert.ToString(i) + "_grid";
                    Grid g = (Grid)(framework_elements[name]);
                    if (i < furance.xs.Count)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }

                    name = "heater_1_" + Convert.ToString(i) + "_grid";
                    g = (Grid)(framework_elements[name]);
                    if (i < furance.ys.Count)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }

                    
                    //heater_0_2_button_3
                    name = "heater_0_" + Convert.ToString(i) + "_button_3_grid";
                    g = (Grid)(framework_elements[name]);
                    if (i > 0)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }


                    //heater_0_2_button_4
                    name = "heater_0_" + Convert.ToString(i) + "_button_4_grid";
                    g = (Grid)(framework_elements[name]);
                    if (i < 4 && furance.xs.Count > i + 1)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }

                    //heater_1_2_button_3
                    name = "heater_1_" + Convert.ToString(i) + "_button_3_grid";
                    g = (Grid)(framework_elements[name]);
                    if (i > 0)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }


                    //heater_1_2_button_4
                    name = "heater_1_" + Convert.ToString(i) + "_button_4_grid";
                    g = (Grid)(framework_elements[name]);
                    if (i < 4 && furance.ys.Count > i + 1)
                    {
                        g.Visibility = 0;
                    }
                    else
                    {
                        g.Visibility = (Visibility)1;
                    }
                }
                

                int k = 0;
                foreach (string s in xlist)
                {
                    ComboBoxItem comboBoxItem = m.采矿_炼制_熔炉_原料_combobox.Items[k] as ComboBoxItem;
                    bool b = comboBoxItem.IsEnabled = find_resource(s).unlocked;
                    k++;
                }
                k = 0;
                foreach (string s in ylist)
                {
                    ComboBoxItem comboBoxItem = m.采矿_炼制_熔炉_燃料_combobox.Items[k] as ComboBoxItem;
                    comboBoxItem.IsEnabled = find_resource(s).unlocked;
                    k++;
                }

                ComboBoxItem selected = null;
                if (m.采矿_炼制_熔炉_原料_combobox.SelectedIndex != -1)
                {
                    selected = m.采矿_炼制_熔炉_原料_combobox.SelectedItem as ComboBoxItem;
                    string name = selected.Content as string;
                    resource r = find_resource(name);
                }


                for(int i = 0; i < 2; i++)
                {
                    int count = 0;
                    if (i == 0)
                    {
                        count = furance.xs.Count;
                    }
                    if (i == 1)
                    {
                        count = furance.ys.Count;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        string si = i.ToString();
                        string sj = j.ToString();
                        string name;
                        bool enough = true;
                        double2 progress = 0;

                        name = "heater_" + si + "_" + sj + "_checkbox";
                        CheckBox c = (CheckBox)framework_elements[name];
                        bool open = (bool)c.IsChecked;

                        for (int p = 3; p < 8; p++)
                        {
                            //heater_0_2_text_6;
                            name = "heater_" + si + "_" + sj + "_text_" + Convert.ToString(p);
                            TextBlock t = (TextBlock)framework_elements[name];
                            if (i == 0)
                            {
                                heater_x x = furance.xs[j];
                                if (p == 3)
                                {
                                    t.Text = x.recipe.a_name + "：" + number_format(x.amount) + " / " + number_format(x.max_amount);
                                    progress = 1 - x.amount / x.max_amount;
                                    x.open = open;
                                }
                                if (p == 4)
                                {
                                    t.Text = x.recipe.b1_name + "：" + number_format(x.amount * x.recipe.b1_product) + " 待生产";
                                }
                                if (p == 5)
                                {
                                    if (x.recipe.b2_name != null)
                                    {
                                        t.Text = x.recipe.b2_name + "：" + number_format(x.amount * x.recipe.b2_product) + " 待生产";
                                    }
                                    else
                                    {
                                        t.Text = "";
                                    }
                                }
                                if (p == 6)
                                {
                                    t.Text = "火力需求：" + number_format(x.recipe.fire_req);
                                    if (x.recipe.fire_req > furance.fire)
                                    {
                                        t.Text += "（不足！）";
                                        enough = false;
                                    }
                                }
                                if (p == 7)
                                {
                                    t.Text = "速度值需求：" + number_format(x.recipe.base_speed_req) + " / 原料";
                                }
                            }
                            else
                            {
                                heater_y y = furance.ys[j];
                                if (p == 3)
                                {
                                    t.Text = y.recipe.a_name + "：" + number_format(y.amount) + " / " + number_format(y.max_amount);
                                    progress = 1 - y.amount / y.max_amount;
                                    y.open = open;
                                }
                                if (p == 4)
                                {
                                    t.Text = "火力：+" + number_format(furance.get_y_speed() / y.recipe.base_speed_req * y.recipe.fire_product) + "/s（" + number_format(y.amount * y.recipe.fire_product) + "）";
                                }
                                if (p == 5)
                                {
                                    t.Text = "熔炉等级需求：" + number_format(y.recipe.level_req);
                                    if (y.recipe.level_req > furance.level)
                                    {
                                        t.Text += "（不足！）";
                                        enough = false;
                                    }
                                }
                                if (p == 6)
                                {
                                    t.Text = "火力需求：" + number_format(y.recipe.fire_req);
                                    if (y.recipe.fire_req > furance.fire)
                                    {
                                        t.Text += "（不足！）";
                                        enough = false;
                                    }
                                }
                                if (p == 7)
                                {
                                    t.Text = "速度值需求：" + number_format(y.recipe.base_speed_req) + " / 燃料";
                                }
                            }
                        }
                        
                        name = "heater_" + si + "_" + sj + "_bg_1";
                        Rectangle r1 = (Rectangle)framework_elements[name];
                        name = "heater_" + si + "_" + sj + "_bg_2";
                        Rectangle r2 = (Rectangle)framework_elements[name];

                        r2.Width = r1.ActualWidth * progress.d;
                        if (!enough || !open)
                        {
                            if (i == 0)
                            {
                                r1.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 0, 127, 127));
                                r2.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 0, 0, 0));
                            }
                            else
                            {
                                r1.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 127, 0, 0));
                                r2.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 0, 0, 0));
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                r1.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 0, 255, 255));
                                r2.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 0, 127, 255));
                            }
                            else
                            {
                                r1.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 255, 0, 0));
                                r2.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * j), 255, 127, 255));
                            }
                        }
                    }
                }

                m.采矿_炼制_熔炉数据1_text.Text = "火力：" + number_format(furance.fire) + 
                    (furance.fire > furance.fire_min? "" : "（最低）" );
                m.采矿_炼制_熔炉数据2_text.Text = "火力下降：" + number_format(furance.fire_drop * 100) + "% / s";
                m.采矿_炼制_熔炉数据3_text.Text = "目前原料&燃料：" + furance.index_convert_to_string(furance.get_current_x_index()) + " & " + furance.index_convert_to_string(furance.get_current_y_index()) + "";

                m.采矿_炼制_熔炉数据4_text.Text = "原料速度值：" + number_format(furance.get_x_speed()) + " / s";
                if(furance.get_current_x_index() == -1)
                {
                    m.采矿_炼制_熔炉数据5_text.Text = "原料消耗速度：无";

                }
                else
                {
                    m.采矿_炼制_熔炉数据5_text.Text = "原料消耗速度：" + number_format(furance.get_x_speed() / furance.xs[furance.get_current_x_index()].recipe.base_speed_req)  +" / s";
                }

                m.采矿_炼制_熔炉数据6_text.Text = "燃料速度值：" + number_format(furance.get_y_speed()) + " / s";
                if (furance.get_current_y_index() == -1)
                {
                    m.采矿_炼制_熔炉数据7_text.Text = "燃料消耗速度：无";

                }
                else
                {
                    m.采矿_炼制_熔炉数据7_text.Text = "燃料消耗速度：" + number_format(furance.get_y_speed() / furance.ys[furance.get_current_y_index()].recipe.base_speed_req) + " / s";
                }

                upgrade f = upgrades["熔炉升级"];
                int index = f.level;

                if(f.level != f.max_level)
                {
                    m.采矿_炼制_熔炉升级消耗0_text.Text = number_format(f.cost_table[index][0].Item2) + " " + f.cost_table[index][0].Item1;
                    if (f.cost_table[index].Count == 2)
                    {
                        m.采矿_炼制_熔炉升级消耗1_text.Text = number_format(f.cost_table[index][1].Item2) + " " + f.cost_table[index][1].Item1;
                    }
                    else
                    {
                        m.采矿_炼制_熔炉升级消耗1_text.Text = "";
                    }

                    m.采矿_炼制_熔炉升级消耗0_text.Foreground = find_resource(f.cost_table[index][0].Item1).text_color();
                    if (f.cost_table[index].Count == 2)
                    {
                        m.采矿_炼制_熔炉升级消耗1_text.Foreground = find_resource(f.cost_table[index][1].Item1).text_color();
                    }

                    m.采矿_炼制_熔炉数据12_text.Text = "效果：" + f.description[index];
                }
                else
                {
                    m.采矿_炼制_熔炉升级消耗0_text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    m.采矿_炼制_熔炉升级消耗0_text.Text = "无";
                    m.采矿_炼制_熔炉升级消耗1_text.Text = "";
                    m.采矿_炼制_熔炉数据12_text.Text = "已达到最大等级！";
                }

                Rectangle fr = m.采矿_炼制_熔炉升级;
                Rectangle fr_bg = m.采矿_炼制_熔炉升级_背景;
                if (can_buy_upgrade(f))
                {
                    fr_bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    fr.Tag = cannot_upgrade;
                }
                else
                {
                    fr_bg.Fill = getSCB(Color.FromRgb(0, 255, 0));
                    fr.Tag = can_upgrade;
                }
                
                foreach(string s in 矿物)
                {
                    resource r = find_resource(s);
                    TextBlock t = ((TextBlock)m.FindName("采矿_宝物_幸运值_" + s + "_text"));
                    double2 luck = (1 + minep.luck_boost) * minep.get_luck_mul();
                    if (luck >= 0.1 * r.luck_req || r.unlocked)
                    {
                        if (t != null)
                        {
                            t.Text = s + "   幸运值需求：" + number_format(r.luck_req);
                            t.Foreground = r.text_color();
                        }
                    }
                    else
                    {
                        if (t != null)
                        {
                            t.Text = "";
                        }
                    }
                }

                foreach(KeyValuePair<string, treasure> kp in treasures)
                {
                    treasure tr = kp.Value;
                    string name = tr.name;
                    Grid g = (Grid)m.FindName("采矿_宝物_" + name + "_grid");
                    double2 luck = (1 + minep.luck_boost) * minep.get_luck_mul();
                    if (luck >= 0.1 * tr.luck_req || tr.unlocked)
                    {
                        g.Visibility = 0;
                        ((TextBlock)m.FindName("采矿_宝物_" + name + "_text")).Text = name;
                        ((TextBlock)m.FindName("采矿_宝物_" + name + "_text")).Foreground = tr.text_color();
                        ((Rectangle)m.FindName("采矿_宝物_" + name + "_bg")).Stroke = tr.text_color();

                        ((TextBlock)m.FindName("采矿_宝物_" + name + "_1_text")).Text = "持有数：" + number_format(tr.get_value());
                        ((TextBlock)m.FindName("采矿_宝物_" + name + "_2_text")).Text = "幸运值需求：" + number_format(tr.get_luck_req());

                        if(name == "荧光宝石")
                        {
                            resource r0 = res_table["转生"]["转生点数"];

                            double2 effect = 1;
                            if (r0.all_get == 0)
                            {
                                effect = double2.Max(1, double2.Pow(tr.get_value(), 0.1));
                            }
                            else
                            {
                                effect = double2.Max(1, double2.Pow(3, r0.get_value() / r0.all_get) * double2.Pow(tr.get_value(), 0.1));
                            }
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_3_text")).Text = "根据 未花费的转生点数比例 获得奖励：";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_4_text")).Text =
                                "将 挖掘所需的采矿点数 除以 Max(1, [3 ^ (未花费的转生点数 / 所有转生点数)] * (荧光宝石数 ^ 0.1)}) " +
                                "（目前 / " + number_format(effect) +"）";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_5_text")).Text = "注：获取的采矿经验随之下降，但降幅仅为采矿点数需求降幅的90%";
                        }
                        else if(name == "熔岩球")
                        {
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_3_text")).Text = "根据 熔炉等级 获得奖励：";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_4_text")).Text =
                                "将 燃料速度值 乘以 {1 + [(0.05 * 熔炉等级) ^ 3] * (熔岩球数 ^ 0.2)} 倍 " +
                                "（目前 ×" + number_format(1 + double2.Pow(0.05 * (double2)furance.level, 3) * double2.Pow(tr.get_value(), 0.2)) + "）";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_5_text")).Text = "";
                        }
                        else if (name == "魔方")
                        {
                            double2 mana = res_table["魔法"]["魔力"].get_value();
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_3_text")).Text = "根据 现有魔力 获得奖励：";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_4_text")).Text =
                                "将 所有方块获取（不包括采矿时） 乘以 {log10(魔方数 + 10) ^ [0.055 * log10(魔力 + 1) ^ 0.996]} 倍 " +
                                "（目前 ×" + number_format(treasure_3_1(tr.get_value(), mana)) + "）";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_5_text")).Text =
                                "将 格子边长 乘以 {log10(魔方数 + 10) ^ [0.016 * log10(魔力 + 1) ^ 0.996]} 倍 " +
                                "（目前 ×" + number_format(treasure_3_2(tr.get_value(), mana)) + "）";
                        }
                        else if (name == "脉冲符文")
                        {
                            double2 level = 0;
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_3_text")).Text = "根据 目前选中的敌人等级 获得奖励：";
                            if(enemy.current != null)
                            {
                                level = enemy.current.level;
                            }
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_4_text")).Text =
                                "将 所有能量获取 乘以 {[1 + 0.002 * log10(脉冲符文数 + 1)] ^ 敌人等级} 倍 " +
                                "（目前 ×" + number_format(double2.Pow(1 + 0.002 * (tr.get_value() + 1).Log10(), level)) + "）";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_5_text")).Text = "";
                        }
                        else if (name == "宝箱")
                        {
                            tr_page_update(2);

                            resource r0 = res_table["采矿"]["采矿点数"];
                            double2 effect = double2.Pow(tr.get_value() + 1, 0.04);
                            if(r0.prestige_get != 0)
                            {
                                effect = double2.Pow(effect, r0.get_value() / r0.prestige_get + 1);
                            }

                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_3_text")).Text = "根据 未花费的采矿点数比例 获得奖励：";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_4_text")).Text =
                                "将 所有宝物获取 乘以 {[(宝箱数 + 1) ^ 0.04] ^ (剩余采矿点数 / 本次转生的采矿点数 + 1)} 倍 " +
                                "（目前 ×" + number_format(effect) + "）";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_5_text")).Text = "";
                        }
                        else if (name == "光速徽章")
                        {
                            tr_page_update(2);

                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_3_text")).Text = "根据 历史生成的采矿区域数 获得奖励：";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_4_text")).Text =
                                "将 白色方块生产器时间 除以 {1 + [(光速徽章数 ^ 0.1) * (采矿区域数 ^ 0.5)]} 倍 " +
                                "（目前 / " + number_format(1 + double2.Pow(tr.get_value(), 0.1) * double2.Pow(minep.mine_amount, 0.5)) + "）";
                            ((TextBlock)m.FindName("采矿_宝物_" + name + "_5_text")).Text =
                                "将 白色方块生产器产量 除以 {1 + [Min(光速徽章数, 1) * (采矿区域数 ^ 0.3)]} 倍 " +
                                "（目前 / " + number_format(1 + double2.Min(tr.get_value(), 1) * double2.Pow(minep.mine_amount, 0.3)) + "）";
                        }

                    }
                    else
                    {
                        g.Visibility = Visibility.Hidden;
                    }
                }

                tr_page_show();
            }
            //核心
            else if (current_group == "核心")
            {

            }
            //娱乐
            else if (current_group == "娱乐")
            {
                if (num_selecter && (m.number_select_grid.Visibility != Visibility.Hidden))
                {
                    m.number_select_grid.Visibility = (Visibility)1;
                }
            }
            //混沌
            else if (current_group == "混沌")
            {

            }

            //转生
            if (current_group == "转生")
            {
                if (num_selecter && (m.number_select_grid.Visibility != Visibility.Hidden))
                {
                    m.number_select_grid.Visibility = (Visibility)1;
                }
                if (current_information == "转生_执行")
                {
                    转生_信息();
                }
                if (current_information == "转生_升级_对数增益")
                {
                    对数增益_信息();
                }
                if (current_information == "转生_升级_生成器")
                {
                    生成器_信息();
                }
                if (current_information == "转生_升级_资源保留")
                {
                    资源保留_信息();
                }
                if (current_information == "转生_升级_升级保留")
                {
                    升级保留_信息();
                }
                if (current_information == "转生_升级_制造")
                {
                    制造_信息();
                }
                if (current_information == "转生_升级_核心")
                {
                    核心_信息();
                }
                if (current_information == "转生_升级_方块增幅")
                {
                    方块增幅_信息();
                }
                if (current_information == "转生_升级_时间力量")
                {
                    时间力量_信息();
                }
                if (current_information == "转生_升级_战斗增幅")
                {
                    战斗增幅_信息();
                }
                if (current_information == "转生_升级_强化等级")
                {
                    强化等级_信息();
                }
                if (current_information == "转生_升级_战斗探索")
                {
                    战斗探索_信息();
                }
                if (current_information == "转生_升级_冷静")
                {
                    冷静_信息();
                }
                if (current_information == "转生_升级_魔法增幅")
                {
                    魔法增幅_信息();
                }
                if (current_information == "转生_升级_转化")
                {
                    转化_信息();
                }
                if (current_information == "转生_升级_采矿增幅")
                {
                    采矿增幅_信息();
                }
                if (current_information == "转生_升级_成就加成")
                {
                    成就加成_信息();
                }
                //转生升级显示
                if (s_ticker("prestige_upgrade", 0.1))
                {
                    if (!upgrading)
                    {
                        resource pp = res_table["转生"]["转生点数"];
                        foreach (KeyValuePair<string, prestige_upgrade> p in prestige_ups)
                        {
                            string cover_name = "转生_升级_" + p.Value.name;
                            string bg_name = cover_name + "_背景";
                            string text_name = cover_name + "_文字";
                            Rectangle cover = (Rectangle)(m.FindName(cover_name));
                            Rectangle bg = (Rectangle)(m.FindName(bg_name));
                            TextBlock text = (TextBlock)(m.FindName(text_name));
                            if (p.Value.level == p.Value.max_level)
                            {
                                bg.Fill = getSCB(Color.FromRgb(0, 255, 255));
                                bg.Stroke = getSCB(Color.FromRgb(0, 0, 255));
                                text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                                cover.Tag = cannot_upgrade;
                                continue;
                            }
                            if (!p.Value.unlocked)
                            {
                                bg.Fill = getSCB(Color.FromRgb(0, 0, 0));
                                bg.Stroke = getSCB(Color.FromRgb(255, 255, 255));
                                text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                                cover.Tag = cannot_upgrade;
                            }
                            else if (p.Value.cost_res_type == "转生点数" && pp.get_value() >= p.Value.cost[p.Value.level])
                            {
                                bg.Fill = getSCB(Color.FromRgb(255, 255, 63));
                                bg.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                                text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                                cover.Tag = can_upgrade;
                            }
                            else
                            {
                                bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                                bg.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                                text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                                cover.Tag = cannot_upgrade;
                            }
                        }
                    }
                }
                cal_pp_gain();
                m.转生_text.Text = "你有" + number_format(res_table["转生"]["转生点数"].get_value()) + "转生点。";


            }

            //成就
            if (s_ticker("achievement", 0.5))
            {
                foreach (KeyValuePair<int, achievement> kp in achievements_id)
                {
                    int ac = kp.Key;
                    int i = ac / 10;
                    int j = ac % 10;
                    achievement a = achievements_id[ac];
                    if (a.curr_level == 0)
                    {
                        achievefield_bg[i, j].Fill = getSCB(Color.FromRgb(75, 75, 75));
                        achievefield_texts[i, j].Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        double hue1 = 120 + 120 * (a.curr_level / a.max_level);
                        achievefield_bg[i, j].Fill = getSCB(HslToRgb(hue1, 191, 191));
                        achievefield_texts[i, j].Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }

                }
                foreach (KeyValuePair<int, achievement> k in achievements_id)
                {
                    int n = k.Key;
                    int i = n / 10;
                    int j = n % 10;

                    achievement e = k.Value;

                    achievefield_texts[i, j].Text = e.name;
                }
            }

            m.achieve_text2.Text = "成就点数：" + 成就点数.get_value();

            m.调试2.Text = number_format(gamespeed());
            m.游戏速度_text.Text = "游戏速度：" + number_format(gamespeed() * 100) + "%";

            //m.调试a.Text = number_format(g2_current.balls.Count);
            #region debug
            if (((Grid)(m.FindName("resource_grid"))).Visibility == Visibility.Visible)
            {
                inputer name_input = inputers["resource_name_input"];
                inputer amount_input = inputers["resource_amount_input"];
                TextBox tb1 = (TextBox)m.FindName("resource_name_input");
                TextBox tb2 = (TextBox)m.FindName("resource_amount_input");
                TextBlock tb3 = (TextBlock)m.FindName("resource_amount_text");
                string name = tb1.Text;
                string amount = tb2.Text;
                name = name.Trim(' ');
                amount = amount.Trim(' ');

                Rectangle confirm = (Rectangle)m.FindName("resource_confirm");
                confirm.Fill = getSCB(Color.FromRgb(0, 255, 127));

                resource x = find_resource(name);
                if (x != null)
                {
                    name_input.curr_state = inputer.state.normal;
                    name_input.text = name;
                    tb1.Background = getSCB(Color.FromRgb(0, 255, 0));
                    tb1.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    tb3.Text = name + " 现有数量：" + number_format(x.get_value());
                }
                else
                {
                    name_input.curr_state = inputer.state.error;
                    tb1.Background = getSCB(Color.FromRgb(200, 0, 0));
                    tb1.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    confirm.Fill = getSCB(Color.FromRgb(255, 0, 0));
                    tb3.Text = "";
                }

                Tuple<bool, double2> tuple = parse_double2(amount);
                if (tuple.Item1)
                {
                    amount_input.curr_state = inputer.state.normal;
                    amount_input.text = amount;
                    tb2.Background = getSCB(Color.FromRgb(0, 255, 0));
                    tb2.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
                else
                {
                    amount_input.curr_state = inputer.state.error;
                    tb2.Background = getSCB(Color.FromRgb(200, 0, 0));
                    tb2.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    confirm.Fill = getSCB(Color.FromRgb(255, 0, 0));
                }
            }
            #endregion debug
        }

        private void spell_show2(spell s)
        {
            if (s == null)
            {
                return;
            }

            string base_name = "魔法_法术_" + s.name;
            string cover_name = base_name + "_学习";
            string cover_name2 = base_name + "_施法";


            Rectangle top = (Rectangle)m.FindName(base_name + "_进度条_顶");
            Rectangle bottom = (Rectangle)m.FindName(base_name + "_进度条_底");
            if (s.level != s.max_level)
            {
                top.Width = bottom.ActualWidth * (1 - s.current_time / s.get_time()).d;
            }
            else
            {
                top.Width = 0;
            }

            bool can_study = true;
            if (s.cost_table[s.level] != null)
            {
                foreach (Tuple<string, double2> tuple in s.cost_table[s.level])
                {
                    resource r = find_resource(tuple.Item1);
                    if (r.get_value() <= 0)
                    {
                        can_study = false;
                    }
                }
            }

            if (can_study && s.level < s.max_level)
            {
                top.Fill = getSCB(Color.FromArgb(150, 0, 0, 0));
                ((Rectangle)(m.FindName(cover_name))).Tag = enable;
                if (!s.study_active && !s.studying && !s.entering)
                {
                    ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(0, 255, 0));
                    ((TextBlock)(m.FindName(cover_name + "_文字"))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
            }
            else
            {
                top.Fill = getSCB(Color.FromArgb(255, 63, 0, 0));
                ((Rectangle)(m.FindName(cover_name))).Tag = disable;
                if (!s.study_active && !s.studying && !s.entering)
                {
                    ((Rectangle)(m.FindName(cover_name + "_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                    ((TextBlock)(m.FindName(cover_name + "_文字"))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
            }

            if (s.study_active)
            {
                ((Rectangle)(m.FindName(cover_name))).Tag = enable;
            }

                    ((TextBlock)m.FindName(base_name + "_t1")).Text = s.name;
            ((TextBlock)m.FindName(base_name + "_t2")).Text = "等级 " + number_format(s.level) + " / " + number_format(s.max_level);
            if (s.level != s.max_level)
            {
                ((TextBlock)m.FindName(base_name + "_t3")).Text = number_format(s.current_time) + "s / " + number_format(s.get_time()) + "s";
                ((TextBlock)m.FindName(base_name + "_t4")).Text = number_format(100 * s.current_time / s.get_time()) + "%";
                ((TextBlock)m.FindName(base_name + "_消耗")).Text = "消耗: ";
                int i = 0;
                for (; i < s.cost_table[s.level].Count; i++)
                {
                    int n = i + 1;
                    Tuple<string, double2> cost = s.cost_table[s.level][i];
                    resource r = find_resource(cost.Item1);
                    double2 cost_pers = cost.Item2 / s.get_cost_mul() / s.get_time();
                    if (r.name == "能量")
                    {
                        cost_pers *= ex.cost_mul;
                    }
                    ((TextBlock)m.FindName(base_name + "_消耗" + make_text(n))).Text = number_format(cost_pers) + " " + cost.Item1 + "/s";
                    ((TextBlock)m.FindName(base_name + "_消耗" + make_text(n))).Foreground = r.text_color();
                }
                for (; i < 4; i++)
                {
                    int n = i + 1;
                    ((TextBlock)m.FindName(base_name + "_消耗" + make_text(n))).Text = "";
                }
            }
            else
            {
                ((TextBlock)m.FindName(base_name + "_t3")).Text = "";
                ((TextBlock)m.FindName(base_name + "_t4")).Text = "";
                ((TextBlock)m.FindName(base_name + "_消耗")).Text = "消耗: 无";
                ((TextBlock)m.FindName(base_name + "_消耗1")).Text = "";
                ((TextBlock)m.FindName(base_name + "_消耗2")).Text = "";
                ((TextBlock)m.FindName(base_name + "_消耗3")).Text = "";
                ((TextBlock)m.FindName(base_name + "_消耗4")).Text = "";
            }

            if (s.normal)
            {
                ((Rectangle)(m.FindName(cover_name2))).Tag = disable;

                if (s.level >= 1 && s.current_active_lv == 0)
                {
                    s.current_active_lv = 1;
                }

                int al = s.current_active_lv;
                int m_al = s.level;
                ((TextBlock)m.FindName(base_name + "_被动效果")).Text = "等级 " + number_format(m_al) + " 的被动效果：\n" + s.passive_des[m_al];
                ((TextBlock)m.FindName(base_name + "_主动效果")).Text = "等级 " + number_format(al) + " / " + number_format(m_al) + " 的主动效果：";
                if (s.casting)
                {
                    ((TextBlock)m.FindName(base_name + "_主动效果")).Text += "（已激活）\n";
                    ((TextBlock)m.FindName(base_name + "_主动效果")).Foreground = getSCB(Color.FromRgb(0, 255, 0));

                }
                else
                {
                    if (prestige_ups["转化"].level >= 3)
                    {
                        //例：0.0852 uncast_boost
                        //percent = "8.52"
                        //percent = "8."
                        //percent = "8"
                        string percent = number_format(get_spell_uncast_boost() * 100);
                        percent = percent.Substring(0, Math.Min(percent.Length, 2));
                        if(percent[percent.Length - 1] == '.')
                        {
                            percent = percent.Substring(0, 1);
                        }
                        ((TextBlock)m.FindName(base_name + "_主动效果")).Text += "（" + percent +"%激活）\n";
                        ((TextBlock)m.FindName(base_name + "_主动效果")).Foreground = getSCB(Color.FromRgb(180, 180, 255));
                    }
                    else
                    {
                        ((TextBlock)m.FindName(base_name + "_主动效果")).Text += "（未激活）\n";
                        ((TextBlock)m.FindName(base_name + "_主动效果")).Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    }
                }
                ((TextBlock)m.FindName(base_name + "_主动效果")).Text += s.active_des[al];



                bool can_cast = true;
                if (!s.cast_active && !s.casting && !s.entering2)
                {
                    ((Rectangle)(m.FindName(cover_name2 + "_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                    ((TextBlock)(m.FindName(cover_name2 + "_文字"))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
                if (al != 0)
                {
                    ((TextBlock)m.FindName(base_name + "_主动消耗")).Text = "消耗:";
                    foreach (Tuple<string, double2> tuple in s.cost_table_active[al])
                    {
                        resource r = find_resource(tuple.Item1);

                        if (r.get_value() <= 0)
                        {
                            can_cast = false;
                        }
                    }

                    if (can_cast)
                    {
                        ((Rectangle)(m.FindName(cover_name2))).Tag = enable;
                        if (!s.cast_active && !s.casting && !s.entering2)
                        {
                            ((Rectangle)(m.FindName(cover_name2 + "_背景"))).Fill = getSCB(Color.FromRgb(0, 255, 0));
                            ((TextBlock)(m.FindName(cover_name2 + "_文字"))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                    }
                    else
                    {
                        ((Rectangle)(m.FindName(cover_name2))).Tag = disable;
                        if (!s.cast_active && !s.casting && !s.entering2)
                        {
                            ((Rectangle)(m.FindName(cover_name2 + "_背景"))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                            //m.调试.Text = number_format(time_this_prestige);
                            ((TextBlock)(m.FindName(cover_name2 + "_文字"))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                    }
                    if (s.cast_active)
                    {
                        ((Rectangle)(m.FindName(cover_name2))).Tag = enable;
                    }

                    int i = 0;
                    for (; i < s.cost_table_active[al].Count; i++)
                    {
                        int n = i + 1;
                        Tuple<string, double2> cost = s.cost_table_active[al][i];
                        resource r = find_resource(cost.Item1);
                        double2 mul = 1;
                        if (r.name == "能量")
                        {
                            mul = ex.cost_mul;
                        }
                        ((TextBlock)m.FindName(base_name + "_主动消耗" + make_text(n))).Text = number_format(cost.Item2 * mul) + " " + cost.Item1 + "/s";
                        ((TextBlock)m.FindName(base_name + "_主动消耗" + make_text(n))).Foreground = r.text_color();
                    }
                    for (; i < 4; i++)
                    {
                        int n = i + 1;
                        ((TextBlock)m.FindName(base_name + "_主动消耗" + make_text(n))).Text = "";
                    }
                }
                else
                {
                    ((TextBlock)m.FindName(base_name + "_主动消耗")).Text = "消耗: 无";
                    ((TextBlock)m.FindName(base_name + "_主动消耗1")).Text = "";
                    ((TextBlock)m.FindName(base_name + "_主动消耗2")).Text = "";
                    ((TextBlock)m.FindName(base_name + "_主动消耗3")).Text = "";
                    ((TextBlock)m.FindName(base_name + "_主动消耗4")).Text = "";
                }

                if (al <= 1)
                {
                    ((Grid)m.FindName(base_name + "_施法_降级_grid")).Visibility = (Visibility)1;
                }
                else
                {
                    ((Grid)m.FindName(base_name + "_施法_降级_grid")).Visibility = 0;
                }

                if (al == m_al)
                {
                    ((Grid)m.FindName(base_name + "_施法_升级_grid")).Visibility = (Visibility)1;
                }
                else
                {
                    ((Grid)m.FindName(base_name + "_施法_升级_grid")).Visibility = 0;
                }
            }
            ((TextBlock)m.FindName(base_name + "_效果")).Text = s.description[s.level];
        }
    }
}
