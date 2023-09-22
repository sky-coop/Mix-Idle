using System;
using System.IO;
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
        object holding = new object(), not_holding = new object();
        string current_information = "";

        //信息框显示
        public void rectangle_cover_move(object sender, MouseEventArgs e)
        {
            if(current_group == "娱乐")
            {
                vm.locking_time = 0;
            }
            m.详细信息框.Visibility = 0;
        }

        public void rectangle_cover_enter(object sender, MouseEventArgs e)
        {
            if (!(sender is Rectangle))
            {
                return;
            }
            Rectangle r = ((Rectangle)(sender));
            string name = r.Name;
            string[] strs = name.Split('_');
            Rectangle background = null;
            TextBlock text = null;

            if (strs[0] == "heater")   //自己创建的
            {
                background = (Rectangle)framework_elements[name + "_bg"];
                text = (TextBlock)framework_elements[name + "_text"];
                background.Fill = getSCB(Color.FromRgb(150, 150, 150));
            }
            else if (vm_elems.ContainsKey(name))
            {
                if (flag_have(r, 10))
                {
                    return;
                }
                if (vm_elems.ContainsKey(name + "_bg"))
                {
                    background = (Rectangle)vm_elems[name + "_bg"];
                    m.调试a.Text = background.Name;
                }
                else
                {
                    background = r;
                }
                if (vm_elems.ContainsKey(name + "_text"))
                {
                    text = (TextBlock)vm_elems[name + "_text"];
                }
                if (!flag_have(r, 0))
                {
                    flag_add(r, 0);
                    background.Fill = color_mul(background.Fill, 0.8);
                }

            }
            if (r.Tag == null || r.Tag.Equals(enable))
            {
                if (strs[1] == "升级")
                {
                    return;
                }
                background = find_elem<Rectangle>(name + "_背景");
                if (background != null)
                {
                    text = find_elem<TextBlock>(name + "_文字");
                    SolidColorBrush s = (SolidColorBrush)background.Fill;
                    background.Fill = text.Foreground;
                    text.Foreground = s;
                }
            }

            if (strs.Length == 4 && strs[0] == "魔法" && strs[1] == "法术" && strs[3] == "学习")
            {
                spell s = (spell)upgrades[strs[2]];
                s.entering = true;
            }
            if (strs.Length == 4 && strs[0] == "魔法" && strs[1] == "法术" && strs[3] == "施法")
            {
                spell s = (spell)upgrades[strs[2]];
                s.entering2 = true;
            }

            //no.5 采矿
            {
                if (name == "采矿_数据_按键_重新生成")
                {
                    if (res_table["采矿"]["采矿点数"].get_value() >= minep.reset_cost)
                    {
                        m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(0, 175, 175));
                        m.采矿_数据_按键_重新生成_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    else
                    {
                        m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        m.采矿_数据_按键_重新生成_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    minef.entering = true;
                }

                int layer = 1;
                if (name == "采矿_数据_按键_挖掘顶层")
                {
                    if (res_table["采矿"]["采矿点数"].get_value() >= minef.get_all_top_cost(layer))
                    {
                        m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(0, 175, 175));
                        m.采矿_数据_按键_挖掘顶层_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    else
                    {
                        m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        m.采矿_数据_按键_挖掘顶层_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    minef.entering2 = true;
                }
            }
        }

        public void rectangle_cover_leave(object sender, MouseEventArgs e)
        {
            if (!(sender is Rectangle))
            {
                return;
            }
            Rectangle r = ((Rectangle)(sender));
            string name = r.Name;
            string[] strs = name.Split('_');


            m.详细信息框.Visibility = (Visibility)2;
            m.详细信息框.Text = "";
            current_information = "";

            Rectangle background = null;
            TextBlock text = null;
            if (strs[0] == "heater")   //自己创建的
            {
                background = (Rectangle)framework_elements[name + "_bg"];
                text = (TextBlock)framework_elements[name + "_text"];
                background.Fill = getSCB(Color.FromRgb(255, 255, 255));
            }
            else if (vm_elems.ContainsKey(name))
            {
                if (flag_have(r, 10))
                {
                    return;
                }
                if (vm_elems.ContainsKey(name + "_bg"))
                {
                    background = (Rectangle)vm_elems[name + "_bg"];
                }
                else
                {
                    background = r;
                }
                if (vm_elems.ContainsKey(name + "_text"))
                {
                    text = (TextBlock)vm_elems[name + "_text"];
                }

                if (flag_have(r, 0))
                {
                    flag_remove(r, 0);
                    background.Fill = color_mul(background.Fill, 1.25);
                }
                if (flag_have(r, 1))
                {
                    flag_remove(r, 1);
                    background.Fill = color_mul(background.Fill, 2);
                }
                if (flag_have(r, 2))
                {
                    flag_remove(r, 2);
                    background.Fill = color_mul_r(background.Fill, 2);
                }

            }
            if (r.Tag == null || r.Tag.Equals(enable))
            {
                if (strs[1] == "升级")
                {
                    return;
                }
                background = find_elem<Rectangle>(name + "_背景");
                if (background != null)
                {
                    text = find_elem<TextBlock>(name + "_文字");
                    SolidColorBrush s = (SolidColorBrush)background.Fill;
                    background.Fill = text.Foreground;
                    text.Foreground = s;
                }
            }

            if (strs.Length == 4 && strs[0] == "魔法" && strs[1] == "法术" && strs[3] == "学习")
            {
                spell s = (spell)upgrades[strs[2]];
                s.entering = false;
            }
            if (strs.Length == 4 && strs[0] == "魔法" && strs[1] == "法术" && strs[3] == "施法")
            {
                spell s = (spell)upgrades[strs[2]];
                s.entering2 = false;
            }

            //no.5 采矿
            if (strs[0] == "采矿")
            {
                {
                    if (name == "采矿_数据_按键_重新生成")
                    {
                        if (res_table["采矿"]["采矿点数"].get_value() >= minep.reset_cost)
                        {
                            m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(0, 255, 255));
                            m.采矿_数据_按键_重新生成_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                        else
                        {
                            m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                            m.采矿_数据_按键_重新生成_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                        minef.holding = false;
                        minef.entering = false;
                    }

                    int layer = 1;
                    if (name == "采矿_数据_按键_挖掘顶层")
                    {
                        if (res_table["采矿"]["采矿点数"].get_value() >= minef.get_all_top_cost(layer))
                        {
                            m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(0, 255, 255));
                            m.采矿_数据_按键_挖掘顶层_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                        else
                        {
                            m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(255, 255, 255));
                            m.采矿_数据_按键_挖掘顶层_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                        minef.holding2 = false;
                        minef.entering2 = false;
                    }
                }
            }
        }

        bool upgrading = false;
        object enable = new object();
        object disable = new object();
        //执行按下操作
        public void rectangle_cover_down(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Rectangle))
            {
                return;
            }
            Rectangle r = (Rectangle)(sender);
            string name = r.Name;
            Rectangle background = (Rectangle)m.FindName(name + "_背景");
            TextBlock text = (TextBlock)m.FindName(name + "_文字");
            string[] strs = name.Split('_');

            if(strs[0] == "heater")
            #region
            {
                if (strs.Length == 5)
                {
                    //heater_0_2_button_1
                    if (strs[4] == "1")
                    {
                        string res_name = null;
                        double2 can_add = 0;
                        if (strs[1] == "0")
                        {
                            heater_x x = furance.xs[Convert.ToInt32(strs[2])];
                            res_name = x.recipe.a_name;
                            can_add = x.max_amount - x.amount;
                        }
                        if (strs[1] == "1")
                        {
                            heater_y y = furance.ys[Convert.ToInt32(strs[2])];
                            res_name = y.recipe.a_name;
                            can_add = y.max_amount - y.amount;
                        }
                        if (find_resource(res_name).get_value() <= 0)
                        {
                            return;
                        }
                        if (can_add <= 0)
                        {
                            return;
                        }
                    }
                    background = (Rectangle)framework_elements[name + "_bg"];
                    text = (TextBlock)framework_elements[name + "_text"];
                    background.Fill = getSCB(Color.FromRgb(120, 120, 120));
                }
            }
            #endregion
            else if (vm_elems.ContainsKey(name))
            {
                if (flag_have(r, 10))
                {
                    return;
                }
                if (vm_elems.ContainsKey(name + "_bg"))
                {
                    background = (Rectangle)vm_elems[name + "_bg"];
                }
                else
                {
                    background = r;
                }
                //text = (TextBlock)game[name + "_text"];
                if (!flag_have(r, 1) || r.Tag.Equals(enable))
                {
                    flag_add(r, 1);
                    background.Fill = color_mul(background.Fill, 0.5);
                }
            }

            //选取购买数的类型
            if (strs[0] == "num")
            {
                background.Fill = getSCB(Color.FromRgb(100, 100, 100));
            }

            //no.1 方块
            #region
            if (strs[0] == "方块")
            {
                if (strs.Length > 2 && strs[2] == "升级" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(0, 127, 0));
                    upgrading = true;
                }
                if (strs.Length > 2 && strs[2] == "收集")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
            }
            #endregion


            //no.2 制造
            #region
            if (strs[0] == "制造")
            {
                if (strs[1] == "菜单")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (strs[1] == "升级" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(0, 127, 0));
                    upgrading = true;
                }
            }
            #endregion

            //no.3 战斗
            #region
            if (strs[0] == "战斗")
            {
                if (r.Name == "战斗_场景_information_fight")
                {
                    if (!fighting)
                    {
                        background.Fill = getSCB(Color.FromRgb(0, 127, 200));
                        text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    else
                    {
                        background.Fill = getSCB(Color.FromRgb(127, 127, 127));
                        text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                }

                if (r.Name == "战斗_场景_information_levelup")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }

                if (r.Name == "战斗_场景_information_leveldown")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }

                if (strs[1] == "场景" && strs.Length == 3)
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (strs.Length > 3 && strs[2] == "洁白世界" && strs[3] == "enemy")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }

                if (strs.Length > 3 && strs[3] == "自动")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (strs.Length > 3 && strs[3] == "手动")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
            }
            #endregion

            //no.4 魔法
            #region
            if (strs[0] == "魔法")
            {
                if (strs.Length > 4 && strs[4] == "献祭" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                    upgrading = true;
                }
                if (r.Name == "魔法_祭坛_升级" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                    upgrading = true;
                }
                if (strs[1] == "菜单")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (strs.Length == 4 && (strs[1] == "附魔" || strs[1] == "药水"))
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (strs.Length == 5 && strs[3] == "施法")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }

                if (strs.Length == 4 && strs[3] == "学习" && r.Tag.Equals(enable))
                {
                    spell s = (spell)find_upgrade(strs[2]);
                    if (!s.studying)
                    {
                        s.study_active = true;
                    }
                    else
                    {
                        s.study_active = false;
                    }
                    background.Fill = getSCB(Color.FromRgb(0, 127, 0));
                    text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
                if (strs.Length == 4 && strs[3] == "施法" && r.Tag.Equals(enable))
                {
                    spell s = (spell)find_upgrade(strs[2]);
                    if (!s.casting)
                    {
                        s.cast_active = true;
                    }
                    else
                    {
                        s.cast_active = false;
                    }
                    background.Fill = getSCB(Color.FromRgb(0, 127, 0));
                    text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
                if (name == "魔法_法术_翻页前")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (name == "魔法_法术_翻页后")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
            }
            #endregion

            //no.5 采矿
            #region
            if (strs[0] == "采矿")
            {
                if (strs[1] == "菜单")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }

                if (name == "采矿_数据_按键_重新生成")
                {
                    if (res_table["采矿"]["采矿点数"].get_value() >= minep.reset_cost)
                    {
                        m.采矿_数据_按键_重新生成_背景.Fill = getSCB(Color.FromRgb(0, 215, 215));
                        m.采矿_数据_按键_重新生成_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        minef.holding = true;
                    }
                }
                if (name == "采矿_数据_按键_挖掘顶层")
                {
                    int layer = 1;
                    if(res_table["采矿"]["采矿点数"].get_value() >= minef.get_all_top_cost(layer))
                    {
                        m.采矿_数据_按键_挖掘顶层_背景.Fill = getSCB(Color.FromRgb(0, 215, 215));
                        m.采矿_数据_按键_挖掘顶层_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        minef.holding2 = true;
                    }
                }

                if (name == "采矿_炼制_原料添加")
                {
                    ComboBox cb = m.采矿_炼制_熔炉_原料_combobox;
                    if (cb.SelectedIndex == -1)
                    {
                        return;
                    }
                    ComboBoxItem t = (ComboBoxItem)cb.SelectedItem;
                    resource res = find_resource(t.Content as string);
                    if (res.get_value() > 0)
                    {
                        background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                    }
                }
                if (name == "采矿_炼制_燃料添加")
                {
                    ComboBox cb = m.采矿_炼制_熔炉_燃料_combobox;
                    if (cb.SelectedIndex == -1)
                    {
                        return;
                    }
                    ComboBoxItem t = (ComboBoxItem)cb.SelectedItem;
                    resource res = find_resource(t.Content as string);
                    if (res.get_value() > 0)
                    {
                        background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                    }
                }
                if (r.Name == "采矿_炼制_熔炉升级" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                    upgrading = true;
                }
                if (name == "采矿_宝物_翻页前")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
                if (name == "采矿_宝物_翻页后")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
            }
            #endregion

            //no.7 娱乐
            #region
            if (strs[0] == "娱乐")
            {
                if (strs[1] == "全屏")
                {
                    background.Fill = getSCB(Color.FromRgb(100, 100, 100));
                }
            }
            if (name.Contains("vm_vconfig_menu_"))
            {
                background = (Rectangle)vm_elems[name + "_背景"];
                background.Fill = getSCB(Color.FromRgb(127, 160, 127));
            }
            #endregion //no.7 娱乐


            //no.9 转生
            #region
            if (strs[0] == "转生")
            {
                if (strs[1] == "执行")
                {
                    background.Fill = getSCB(Color.FromRgb(50, 50, 50));
                }
                if (strs[1] == "升级" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(200, 200, 0));
                    upgrading = true;
                }
            }
            #endregion

        }

        string checkname = null;
        //执行按键操作
        public void rectangle_cover_up(object sender, MouseButtonEventArgs e)
        {
            if (current_group == "娱乐")
            {
                vm.locking_time = 0;
            }
            string name;
            if (!(sender is Rectangle))
            {
                if(sender is Image)
                {
                    image_l_esc(sender as Image);
                }
                return;
            }
            Rectangle r = (Rectangle)(sender);
            name = r.Name;
            Rectangle background = (Rectangle)m.FindName(name + "_背景");
            TextBlock text = (TextBlock)m.FindName(name + "_文字");
            string[] strs = name.Split('_');

            if (strs.Length == 5 && strs[0] == "heater")
            #region
            {
                //heater_0_2_button_1
                if (strs[4] == "1")
                #region
                {
                    string res_name = null;
                    double2 can_add = 0;
                    if (strs[1] == "0")
                    {
                        heater_x x = furance.xs[Convert.ToInt32(strs[2])];
                        res_name = x.recipe.a_name;
                        can_add = x.max_amount - x.amount;
                    }
                    if (strs[1] == "1")
                    {
                        heater_y y = furance.ys[Convert.ToInt32(strs[2])];
                        res_name = y.recipe.a_name;
                        can_add = y.max_amount - y.amount;
                    }
                    double2 val = find_resource(res_name).get_value();
                    double2 will_add = buy_number;
                    if (!buy_int)
                    {
                        will_add = val * buy_percent / 100;
                    }
                    will_add = double2.Min(will_add, val);
                    will_add = double2.Min(will_add, can_add);

                    find_resource(res_name).add_value(-will_add);
                    if (strs[1] == "0")
                    {
                        heater_x x = furance.xs[Convert.ToInt32(strs[2])];
                        x.amount += will_add;
                    }
                    if (strs[1] == "1")
                    {
                        heater_y y = furance.ys[Convert.ToInt32(strs[2])];
                        y.amount += will_add;
                    }
                }
                #endregion
                else if (strs[4] == "2")
                #region
                {
                    string res_name = null;
                    double2 gain = 0;
                    if (strs[1] == "0")
                    {
                        heater_x x = furance.xs[Convert.ToInt32(strs[2])];
                        res_name = x.recipe.a_name;
                        gain = x.amount;

                        furance.xs.Remove(x);
                    }
                    if (strs[1] == "1")
                    {
                        heater_y y = furance.ys[Convert.ToInt32(strs[2])];
                        res_name = y.recipe.a_name;
                        gain = y.amount;

                        furance.ys.Remove(y);
                    }

                    find_resource(res_name).add_value(gain, true);
                }
                #endregion
                else if (strs[4] == "3" || strs[4] == "4")
                #region
                {
                    int move = 0;
                    int index = Convert.ToInt32(strs[2]);
                    if (strs[4] == "3")
                    {
                        move = index - 1;
                    }
                    else
                    {
                        move = index + 1;
                    }


                    if (strs[1] == "0")
                    {
                        furance.xs = Swap(furance.xs, index, move);
                    }
                    if (strs[1] == "1")
                    {
                        furance.ys = Swap(furance.ys, index, move);
                    }

                    string na;
                    na = "heater_" + strs[1] + "_" + strs[2] + "_checkbox";
                    CheckBox c1 = (CheckBox)framework_elements[na];
                    na = "heater_" + strs[1] + "_" + Convert.ToString(move) + "_checkbox";
                    CheckBox c2 = (CheckBox)framework_elements[na];
                    bool b = (bool)c1.IsChecked;
                    c1.IsChecked = c2.IsChecked;
                    c2.IsChecked = b;
                }
                #endregion
                background = (Rectangle)framework_elements[name + "_bg"];
                text = (TextBlock)framework_elements[name + "_text"];
                background.Fill = getSCB(Color.FromRgb(150, 150, 150));
                return;
            }
            #endregion
            else if (vm_elems.ContainsKey(name))
            #region
            {
                if (flag_have(r, 10))
                {
                    return;
                }
                if (vm_elems.ContainsKey(name + "_bg"))
                {
                    background = (Rectangle)vm_elems[name + "_bg"];
                }
                else
                {
                    background = r;
                }

                if (flag_have(r, 1) || r.Tag.Equals(enable))
                {
                    flag_remove(r, 1);
                    background.Fill = color_mul(background.Fill, 2);
                    game_key(r);
                }
            }
            #endregion

            if (name == "manual_save")
            #region
            {
                bool all_space = true;
                foreach (char c in m.save_name.Text)
                {
                    if(c != ' ')
                    {
                        all_space = false;
                    }
                }
                if (all_space || m.save_name.Text.Length == 0 || m.save_name.Text[0] == '/' || m.save_name.Text[0] == '\\')
                {
                    MessageBox.Show("未保存成功，存档文件名不能为空或以“/”、“\\”为开头，因为这可能会导致存档文件夹的根目录出现一堆文件", "注意");
                }
                else
                {
                    if (save(m.save_name.Text))
                    {
                        MessageBox.Show("存档文件“" + m.save_name.Text + "”保存成功！", "提示");
                    }
                    else
                    {
                        MessageBox.Show("保存失败，您输入了Windows文件系统所禁用的字符（包括“?”、“*”、[\"]、“<”、“>”、“|”、“:”），请重新起一个名字", "注意");
                    }
                }
            }
            #endregion
            if (name == "manual_load")
            #region
            {
                bool all_space = true;
                foreach (char c in m.load_name.Text)
                {
                    if (c != ' ')
                    {
                        all_space = false;
                    }
                }
                if (all_space || m.load_name.Text.Length == 0 || m.load_name.Text[0] == '/' || m.load_name.Text[0] == '\\')
                {
                    MessageBox.Show("未读取成功，存档文件名不能为空或以“/”、“\\”为开头，因为这可能会导致存档文件夹的根目录出现一堆文件", "注意");
                }
                else
                {
                    string t = "./存档/" + m.load_name.Text + "/" + m.load_name.Text + ".a.mixidle";
                    if (File.Exists(t))
                    {
                        load(m.load_name.Text);
                        MessageBox.Show("存档文件“" + m.load_name.Text + "”读取成功！", "提示");
                    }
                    else
                    {
                        if (checkname == m.load_name.Text)
                        {
                            if (load(checkname))
                            {
                                MessageBox.Show("开始一个新游戏！", "提示");
                            }
                            else
                            {
                                MessageBox.Show("创建新游戏失败，您输入了Windows文件系统所禁用的字符（包括“?”、“*”、[\"]、“<”、“>”、“|”、“:”），请重新起一个名字", "注意");
                            }
                        }
                        else
                        {
                            MessageBox.Show("存档文件“" + m.load_name.Text + "”不存在，若要创建新游戏，请再次点击读档键！", "注意");
                            checkname = m.load_name.Text;
                        }
                    }
                }
            }
            #endregion

            if (strs[0] == "num")
            {
                foreach (Grid g in m.number_select_grid.Children)
                {
                    string to_enable_name = "num_" + g.Name.Split('_')[1];
                    ((Rectangle)(m.FindName(to_enable_name))).Tag = enable;
                    string bg_name = to_enable_name + "_背景";
                    string text_name = to_enable_name + "_文字";
                    ((Rectangle)(m.FindName(bg_name))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                    ((TextBlock)(m.FindName(text_name))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
                r.Tag = disable;
                background.Fill = getSCB(Color.FromRgb(0, 180, 180));
                change_buy_config(r);
            }

            //no.1 方块
            #region
            if (strs[0] == "方块")
            {
                if (strs.Length > 2 && strs[2] == "升级" && r.Tag.Equals(can_upgrade))
                {
                    方块生产器升级(block_producters[strs[1]]);
                    upgrading = false;
                }
                if (strs.Length > 2 && strs[2] == "收集")
                {
                    方块生产器收集(block_producters[strs[1]]);
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                }
            }
            #endregion


            //no.2 制造
            #region
            if (strs[0] == "制造")
            {
                if (strs[1] == "菜单")
                {
                    group_process(制造_options, r, true);
                }
                if (strs[1] == "升级" && r.Tag.Equals(can_upgrade))
                {
                    if (strs[2] == "材料")
                    {
                        string material_name = r.Name.Split('_')[3];
                        upgrade u = find_upgrade(material_name);
                        buy_material(u);
                        background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        upgrading = false;
                    }
                    else
                    {
                        string obj_name = r.Name.Split('_')[3];
                        upgrade u = find_upgrade(obj_name);
                        buy_upgrade(u);
                        background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        upgrading = false;
                    }
                }
            }
            #endregion

            //no.3 战斗
            #region
            if (strs[0] == "战斗")
            {
                //选择敌人
                if (strs.Length == 6 && strs[3] == "enemy")
                {
                    string enemy_name = strs[4];
                    change_enemy(find_enemy(enemy_name));
                }

                if (r.Name == "战斗_场景_information_fight")
                {
                    if (!fighting)
                    {
                        background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        fighting = true;
                    }
                    else
                    {
                        background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                        text.Foreground = getSCB(Color.FromRgb(0, 255, 195));
                        fighting = false;
                    }
                }
                if (r.Name == "战斗_场景_information_levelup")
                {
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    enemy en = enemy.current;
                    double2 s = 1;
                    if (buy_int)
                    {
                        s = buy_number;
                    }
                    else
                    {
                        double2 xn = buy_percent / 100.0 * en.max_level;
                        double2 x = new double2(double_floor(xn.d), xn.i);
                        if (x > 1)
                        {
                            s = x;
                        }
                    }
                    change_enemy_level(s);
                }

                if (r.Name == "战斗_场景_information_leveldown")
                {
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    enemy en = enemy.current;
                    double2 s = 1;
                    if (buy_int)
                    {
                        s = buy_number;
                    }
                    else
                    {
                        double2 xn = buy_percent / 100.0 * en.max_level;
                        double2 x = new double2(double_floor(xn.d), xn.i);
                        if (x > 1)
                        {
                            s = x;
                        }
                    }
                    change_enemy_level(-s);
                }

                if (strs.Length > 3 && strs[3] == "enemy")
                {
                    group_process_2(战斗_enemies, false);
                    if (strs[2] == "洁白世界")
                    {
                        group_process(战斗_洁白世界_enemies, r, false);
                    }
                    else if (strs[2] == "草原")
                    {
                        group_process(战斗_草原_enemies, r, false);
                    }
                }

                else if (strs.Length == 3 && strs[1] == "场景")
                {
                    group_process(战斗_options, r, true);
                }

                if (strs.Length > 3 && strs[3] == "自动")
                {
                    group_process(战斗_自动攻击风格, r, false, getSCB(Color.FromRgb(100, 255, 100)));
                    you.auto_attack_form = attack_forms[strs[4]];
                }
                if (strs.Length > 3 && strs[3] == "手动")
                {
                    attack_form af = attack_forms[strs[4]];
                    if (af.skill)
                    {
                        af.skill_time_current = af.skill_time;
                        af.skilling = true;
                    }
                    else
                    {
                        attack(af, 1);
                        background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    }
                }
            }
            #endregion

            //no.4 魔法
            #region
            if (strs[0] == "魔法")
            {
                if (strs.Length > 4 && strs[4] == "献祭" && r.Tag.Equals(can_upgrade))
                {
                    background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    string 祭品_name = strs[3];
                    resource 祭品 = find_resource(祭品_name);
                    double2 amount = buy_number;
                    if (!buy_int)
                    {
                        amount = 祭品.get_value() * buy_percent / 100.0;
                    }
                    if (祭品.get_value() < amount)
                    {
                        amount = 祭品.get_value();
                    }
                    magic_altar.eat(祭品_name, amount);
                    祭品.add_value(-amount);

                    if (祭品.get_value() < 0.0001)
                    {
                        祭品.set_value(0);
                    }
                    upgrading = false;
                }
                if (r.Name == "魔法_祭坛_升级" && r.Tag.Equals(can_upgrade))
                {
                    upgrade u = find_upgrade("祭坛升级");
                    buy_upgrade(u);
                    background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    upgrading = false;
                }

                if (strs[1] == "菜单")
                {
                    group_process(魔法_options, r, true);
                }

                if (strs.Length == 3 && (strs[1] == "附魔" || strs[1] == "药水"))
                {
                    enchant ec = enchants[strs[2]];
                    if (!ec.active)
                    {
                        background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        text.Text = "停止附魔";
                        if (ec.is_potion)
                        {
                            text.Text = "停止配制";
                        }
                        ec.active = true;
                    }
                    else
                    {
                        background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                        text.Foreground = getSCB(Color.FromRgb(0, 255, 195));
                        text.Text = "开始附魔";
                        if (ec.is_potion)
                        {
                            text.Text = "开始配制";
                        }
                        ec.active = false;
                    }
                }

                if (strs.Length == 4 && (strs[1] == "附魔" || strs[1] == "药水"))
                {
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    enchant ec = enchants[strs[2]];
                    decimal change = buy_number;
                    if (!buy_int)
                    {
                        change = decimal.Floor(ec.max_level * (decimal)(buy_percent / 100.0));
                        if (change < 1)
                        {
                            change = 1;
                        }
                    }
                    if ((strs[3] == "降级" || strs[3] == "减速") && change > ec.level - 1)
                    {
                        change = ec.level - 1;
                    }
                    if ((strs[3] == "升级" || strs[3] == "加速") && change > ec.max_level - ec.level)
                    {
                        change = ec.max_level - ec.level;
                    }
                    if (strs[3] == "降级" || strs[3] == "减速")
                    {
                        ec.change_level(-change);
                    }
                    if (strs[3] == "升级" || strs[3] == "加速")
                    {
                        ec.change_level(change);
                    }
                }

                if (strs.Length == 5 && strs[3] == "施法")
                {
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    spell sp = (spell)find_upgrade(strs[2]);
                    int change = buy_number;
                    if (!buy_int)
                    {
                        change = (int)(sp.level * buy_percent / 100.0);
                        if (change < 1)
                        {
                            change = 1;
                        }
                    }
                    if (strs[4] == "降级" && change > sp.current_active_lv - 1)
                    {
                        change = sp.current_active_lv - 1;
                    }
                    if (strs[4] == "升级" && change > sp.level - sp.current_active_lv)
                    {
                        change = sp.level - sp.current_active_lv;
                    }
                    if (strs[4] == "降级")
                    {
                        sp.change_level(-change);
                    }
                    if (strs[4] == "升级")
                    {
                        sp.change_level(change);
                    }
                }
                if (strs.Length == 4 && strs[3] == "学习" && r.Tag.Equals(enable))
                {
                    spell s = (spell)find_upgrade(strs[2]);
                    if (s.study_active)
                    {
                        s.studying = true;
                    }
                    else
                    {
                        s.studying = false;
                    }
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(0, 255, 255));
                }
                if (strs.Length == 4 && strs[3] == "施法" && r.Tag.Equals(enable))
                {
                    spell s = (spell)find_upgrade(strs[2]);
                    if (s.cast_active)
                    {
                        s.casting = true;
                    }
                    else
                    {
                        s.casting = false;
                    }
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(0, 255, 255));
                }
                if (name == "魔法_法术_翻页前")
                {
                    spell_page--;
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
                if (name == "魔法_法术_翻页后")
                {
                    spell_page++;
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
            }
            #endregion

            //no.5 采矿
            #region
            if (strs[0] == "采矿")
            {
                if (name == "采矿_数据_按键_重新生成")
                {
                    if (res_table["采矿"]["采矿点数"].get_value() >= minep.reset_cost)
                    {
                        mine_regenerate();

                        res_table["采矿"]["采矿点数"].add_value(-minep.reset_cost);
                        minef.holding = false;
                    }
                }
                if (name == "采矿_数据_按键_挖掘顶层")
                {
                    int layer = 1;
                    double2 cost = minef.get_all_top_cost(layer);
                    if(res_table["采矿"]["采矿点数"].get_value() >= cost)
                    {

                        res_table["采矿"]["采矿点数"].add_value(-cost);
                        Dictionary<string, double2> d = minef.get_all_top_loot(layer);
                        foreach (KeyValuePair<string, double2> kp in d)
                        {
                            find_resource(kp.Key).add_value(kp.Value, true);
                        }
                        minep.gain_exp(minef.get_all_top_exp(layer) *
                            global_xp_boost());
                        minef.get_all_top_depth_up(layer);

                        minef.holding2 = false;
                    }
                }
                if (strs[1] == "菜单")
                {
                    group_process(采矿_options, r, true);
                }
                if (name == "采矿_炼制_原料添加")
                {
                    ComboBox cb = m.采矿_炼制_熔炉_原料_combobox;
                    if (cb.SelectedIndex == -1)
                    {
                        return;
                    }
                    ComboBoxItem t = (ComboBoxItem)cb.SelectedItem;
                    resource res = find_resource(t.Content as string);
                    if (res.get_value() > 0)
                    {
                        double2 num = buy_number;
                        if (!buy_int)
                        {
                            num = buy_percent / 100 * res.get_value();
                        }
                        if (num > res.get_value())
                        {
                            num = res.get_value();
                        }
                        res.add_value(-num);
                        furance.add_x(new heater_x(x_recipes[t.Content as string], num));
                    }
                }
                if (name == "采矿_炼制_燃料添加")
                {
                    ComboBox cb = m.采矿_炼制_熔炉_燃料_combobox;
                    if (cb.SelectedIndex == -1)
                    {
                        return;
                    }
                    ComboBoxItem t = (ComboBoxItem)cb.SelectedItem;
                    resource res = find_resource(t.Content as string);
                    if (res.get_value() > 0)
                    {
                        double2 num = buy_number;
                        if (!buy_int)
                        {
                            num = buy_percent / 100 * res.get_value();
                        }
                        if (num > res.get_value())
                        {
                            num = res.get_value();
                        }
                        res.add_value(-num);
                        furance.add_y(new heater_y(y_recipes[t.Content as string], num));
                    }
                }
                if (r.Name == "采矿_炼制_熔炉升级" && r.Tag.Equals(can_upgrade))
                {
                    upgrade u = find_upgrade("熔炉升级");
                    buy_upgrade(u);
                    background.Fill = getSCB(Color.FromRgb(255, 255, 255));
                    upgrading = false;
                }
                if (name == "采矿_宝物_翻页前")
                {
                    tr_page--;
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
                if (name == "采矿_宝物_翻页后")
                {
                    tr_page++;
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
            }
            #endregion

            //no.7 娱乐
            #region
            {
                if (strs[0] == "娱乐")
                {
                    if (strs[1] == "全屏")
                    {
                        Grid game_grid = (Grid)m.FindName("娱乐_grid");
                        Grid game_container = (Grid)m.FindName("娱乐_container");
                        Grid container = (Grid)m.FindName("Container");
                        if (game_grid.Children.Contains(game_container))
                        {
                            text.Text = "退出全屏";
                            double x_mul = container.ActualWidth / game_container.ActualWidth;
                            double y_mul = container.ActualHeight / game_container.ActualHeight;
                            double mul = Math.Min(x_mul, y_mul);
                            game_grid.Children.Remove(game_container);
                            container.Visibility = Visibility.Visible;
                            container.Children.Add(game_container);
                            scale_mul(game_container, mul, mul);
                            //Console.WriteLine(game_container.Margin.Left);
                            vm_fullscreen = true;
                        }
                        else
                        {
                            text.Text = "进入全屏";
                            container.Visibility = Visibility.Hidden;
                            container.Children.Remove(game_container);
                            game_grid.Children.Add(game_container);
                            scale(game_container, 1, 1);
                            vm_fullscreen = false;
                        }

                        background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                        text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    }
                }

                if (name.Contains("vm_vconfig_menu_"))
                {
                    group_process(vm_vconfig_menu_group, r, true, 
                        getSCB(Color.FromRgb(225, 225, 150)), getSCB(Color.FromRgb(170, 140, 200)));
                }
            }
            #endregion

            //no.9 转生
            #region
            {
                if (name == "转生_执行")
                {
                    prestige_base();
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                }
                if (name.Contains("转生_升级") && r.Tag.Equals(can_upgrade))
                {
                    string str = strs[2];
                    buy_prestige_upgrade(str);
                    background.Fill = getSCB(Color.FromRgb(0, 0, 0));
                    upgrading = false;
                }
            }
            #endregion

            #region debug
            if (name == "resource_confirm")
            {
                inputer name_input = inputers["resource_name_input"];
                inputer amount_input = inputers["resource_amount_input"];
                if(name_input.curr_state == inputer.state.normal &&
                    amount_input.curr_state == inputer.state.normal)
                {
                    find_resource(name_input.text).set_value(
                        parse_double2(amount_input.text).Item2);
                    resource_exit(null, null);
                }
            }
            #endregion debug
        }
        public void rectangle_cover_rdown(object sender, MouseEventArgs e)
        {
            if (!(sender is Rectangle))
            {
                return;
            }
            Rectangle r = ((Rectangle)(sender));
            string name = r.Name;
            string[] strs = name.Split('_');
            
            Rectangle background = null;
            if (strs[0] == "heater")   //自己创建的
            {
            }
            else if (vm_elems.ContainsKey(name))
            {
                if (flag_have(r, 10))
                {
                    return;
                }
                if (vm_elems.ContainsKey(name + "_bg"))
                {
                    background = (Rectangle)vm_elems[name + "_bg"];
                }
                else
                {
                    background = r;
                }
                if (!flag_have(r, 2))
                {
                    flag_add(r, 2);
                    background.Fill = color_mul_r(background.Fill, 0.5);
                }
            }
        }

        public void rectangle_cover_rup(object sender, MouseEventArgs e)
        {
            if (!(sender is Rectangle))
            {
                return;
            }
            Rectangle r = ((Rectangle)(sender));
            string name = r.Name;
            string[] strs = name.Split('_');

            Rectangle background = null;
            if (strs[0] == "heater")   //自己创建的
            {
            }
            else if (vm_elems.ContainsKey(name))
            {
                if (flag_have(r, 10))
                {
                    return;
                }
                if (vm_elems.ContainsKey(name + "_bg"))
                {
                    background = (Rectangle)vm_elems[name + "_bg"];
                }
                else
                {
                    background = r;
                }
                if (flag_have(r, 2))
                {
                    flag_remove(r, 2);
                    background.Fill = color_mul_r(background.Fill, 2);
                    game_rkey(r);
                }
            }
        }

        private void image_l_esc(Image i)
        {
            string name = i.Name;
            if (name == "vm_button_0")
            {
                if (vm.opened)
                {
                    if (vm.dt_top)
                    {
                        vm.dt_top = false;
                        return;
                    }
                    if (vm.show_list.Count > 0)
                    {
                        VM_APP a = vm.show_list.Last();
                        vm.show_list.Remove(a);
                        vm_elems[a.name].Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        vm.events.Add("已经返回到桌面");
                    }
                }
            }
            if (name == "vm_button_1")
            {
                if (vm.opened)
                {
                    vm.dt_top = true;
                }
            }
            //button2关闭dt_top

            if (name == "vm_button_3")
            {
                if (vm.opened)
                {
                    vm.locking = !vm.locking;
                    vm.locking_time = 0;
                }
            }
            if (name == "vm_button_4")
            {
                if (vm.opened)
                {
                    vm.close();
                }
                else
                {
                    vm.open();
                }
            }
            if (name.Contains("vm_os_app_icon"))
            {
                string app_name = (string)i.Tag;
                vm.run(app_name);
            }
        }


        private void init_num_select()
        {
            Rectangle r = m.num_x1;
            foreach (Grid g in m.number_select_grid.Children)
            {
                string to_enable_name = "num_" + g.Name.Split('_')[1];
                ((Rectangle)(m.FindName(to_enable_name))).Tag = enable;
                string bg_name = to_enable_name + "_背景";
                string text_name = to_enable_name + "_文字";
                ((Rectangle)(m.FindName(bg_name))).Fill = getSCB(Color.FromRgb(255, 255, 255));
                ((TextBlock)(m.FindName(text_name))).Foreground = getSCB(Color.FromRgb(0, 0, 0));
            }
            r.Tag = disable;
            m.num_x1_背景.Fill = getSCB(Color.FromRgb(0, 180, 180));
            change_buy_config(r);
        }

        private void mine_enter(object sender, MouseEventArgs e)
        {
            Rectangle a = (Rectangle)sender;
            int n = (int)(a.Tag);
            int i = n / 10;
            int j = n % 10;
            mine_cell mc = minef.graph[i, j];

            mc.entering = true;
            mc.enter_not_change = true;
            if (mc.depth != int.MaxValue && res_table["采矿"]["采矿点数"].get_value() >= mc.cost[mc.depth - 1])
            {
                Rectangle k = minefield_bg[i, j];
                byte r = ((SolidColorBrush)k.Fill).Color.R;
                byte g = ((SolidColorBrush)k.Fill).Color.G;
                byte b = ((SolidColorBrush)k.Fill).Color.B;
                shift_color_byte(ref r);
                shift_color_byte(ref g);
                shift_color_byte(ref b);
                /*
                r = (byte)(255 - r);
                g = (byte)(255 - g);
                b = (byte)(255 - b);*/
                k.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));

                TextBlock t = minefield_texts[i, j];
                r = ((SolidColorBrush)t.Foreground).Color.R;
                g = ((SolidColorBrush)t.Foreground).Color.G;
                b = ((SolidColorBrush)t.Foreground).Color.B;
                shift_color_byte(ref r);
                shift_color_byte(ref g);
                shift_color_byte(ref b);
                /*
                r = (byte)(255 - r);
                g = (byte)(255 - g);
                b = (byte)(255 - b);*/
                t.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));

                mc.enter_not_change = false;
            }
        }

        private void leave(object sender, MouseEventArgs e)
        {
            m.详细信息框.Visibility = (Visibility)2;
            m.详细信息框.Text = "";
            current_information = "";
        }

        private void mine_leave(object sender, MouseEventArgs e)
        {
            Rectangle a = (Rectangle)sender;
            int n = (int)(a.Tag);
            int i = n / 10;
            int j = n % 10;
            mine_cell mc = minef.graph[i, j];

            leave(sender, e);
            mine_enter(sender, e);
            mc.entering = false;
            mc.enter_not_change = false;
        }

        private void mine_down(object sender, MouseButtonEventArgs e)
        {
            Rectangle a = (Rectangle)sender;
            int n = (int)(a.Tag);
            int i = n / 10;
            int j = n % 10;
            mine_cell mc = minef.graph[i, j];

            if (mc.depth != int.MaxValue && res_table["采矿"]["采矿点数"].get_value() >= mc.cost[mc.depth - 1])
            {
                Rectangle k = minefield_bg[i, j];
                byte r = ((SolidColorBrush)k.Fill).Color.R;
                byte g = ((SolidColorBrush)k.Fill).Color.G;
                byte b = ((SolidColorBrush)k.Fill).Color.B;
                shift_color_byte(ref r);
                shift_color_byte(ref g);
                shift_color_byte(ref b);
                k.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));

                TextBlock t = minefield_texts[i, j];
                r = ((SolidColorBrush)t.Foreground).Color.R;
                g = ((SolidColorBrush)t.Foreground).Color.G;
                b = ((SolidColorBrush)t.Foreground).Color.B;
                shift_color_byte(ref r);
                shift_color_byte(ref g);
                shift_color_byte(ref b);
                t.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));
            }
            mc.entering = false;
        }

        private void mine_up(object sender, MouseButtonEventArgs e)
        {
            Rectangle a = (Rectangle)sender;
            int n = (int)(a.Tag);
            int i = n / 10;
            int j = n % 10;

            mine_cell mc = minef.graph[i, j];
            if (mc.depth != int.MaxValue)
            {
                if (res_table["采矿"]["采矿点数"].get_value() >= mc.cost[mc.depth - 1])
                {
                    res_table["采矿"]["采矿点数"].add_value(-mc.cost[mc.depth - 1]);
                    minep.gain_exp(mc.exp[mc.depth - 1] *
                        global_xp_boost());
                    foreach (KeyValuePair<string, double2> kp in mc.loot[mc.depth - 1])
                    {
                        resource r = find_resource(kp.Key);
                        r.add_value(kp.Value, true);
                    }

                    mc.depth++;
                    if (mc.depth > mc.max_depth)
                    {
                        mc.depth = int.MaxValue;
                    }
                }
                get_cell(minef.graph[i, j], i, j);
                mine_enter(sender, e);
            }
        }

        private void mine_move(object sender, MouseEventArgs e)
        {
            Rectangle a = (Rectangle)sender;
            int n = (int)(a.Tag);
            int i = n / 10;
            int j = n % 10;
            
            m.详细信息框.Visibility = 0;
            m.详细信息框.Background = minefield_bg[i, j].Fill;
            m.详细信息框.Foreground = minefield_texts[i, j].Foreground;
            current_information = a.Name;

            Point pt = Mouse.GetPosition(null);
            double x = pt.X + 20;
            double y = pt.Y + 20;

            if (m.详细信息框.ActualWidth + x + 40 > m.主窗口.ActualWidth)
            {
                x = x - m.详细信息框.ActualWidth + 20;
            }
            if (m.详细信息框.ActualHeight + y + 40 > m.主窗口.ActualHeight)
            {
                y = y - m.详细信息框.ActualHeight + 20;
            }

            m.详细信息框.Margin = new Thickness(x, y, 0, 0);
        }

        private void achieve_move(object sender, MouseEventArgs e)
        {
            int n = (int)(((Rectangle)sender).Tag);
            achievement a = achievements_id[n];
            int i = n / 10;
            int j = n % 10;
            achievefield_hint[i, j].Visibility = (Visibility)1;

            total_up_levels -= a.up_level;
            m.achieve_hint_total_text.Text = Convert.ToString(total_up_levels);
            if (total_up_levels == 0)
            {
                m.achieve_hint_grid.Visibility = (Visibility)1;
            }

            a.up_level = 0;


            //详细信息框
            string s = "成就 " + a.name + "    等级 " + a.curr_level + " / " + a.max_level + "\n";
            
            if (a.curr_level == a.max_level)
            {
                s += "···本成就已全部完成！";
                //显示“全部完成”;
            }
            else
            {
                s += "···要求：" + a.levels[a.curr_level].des;
                s += "\n···奖励：" + a.levels[a.curr_level].reward + " 成就点数";
            }
            if (a.curr_level > 0)
            {
                //显示“已完成”;
                s += "\n\n···目前已完成：";
                for(int k = 0; k < a.curr_level; k++)
                {
                    s += "\n······等级 " + Convert.ToString(k + 1) + "：" + a.levels[k].des + " (" + a.levels[k].reward + ")";
                }
            }
            m.详细信息框.Text = s;

            m.详细信息框.Visibility = 0;
            m.详细信息框.Background = achievefield_bg[i, j].Fill;
            m.详细信息框.Foreground = achievefield_texts[i, j].Foreground;
            current_information = a.name;

            Point pt = Mouse.GetPosition(null);
            double x = pt.X + 20;
            double y = pt.Y + 20;

            if (m.详细信息框.ActualWidth + x + 40 > m.主窗口.ActualWidth)
            {
                x = x - m.详细信息框.ActualWidth + 20;
            }
            if (m.详细信息框.ActualHeight + y + 40 > m.主窗口.ActualHeight)
            {
                y = y - m.详细信息框.ActualHeight + 20;
            }

            m.详细信息框.Margin = new Thickness(x, y, 0, 0);
        }


        public void setting(object sender, MouseButtonEventArgs e)
        {
            m.settings_grid.Visibility = 0;
        }

        public void setting_exit(object sender, MouseButtonEventArgs e)
        {
            m.settings_grid.Visibility = (Visibility)1;
        }

        public void achieve(object sender, MouseButtonEventArgs e)
        {
            m.achieves_grid.Visibility = 0;
        }

        public void achieve_exit(object sender, MouseButtonEventArgs e)
        {
            m.achieves_grid.Visibility = (Visibility)1;
        }

        public void resource_enter(object sender, RoutedEventArgs e)
        {
            m.resource_grid.Visibility = 0;
        }

        public void resource_exit(object sender, MouseButtonEventArgs e)
        {
            m.resource_grid.Visibility = (Visibility)1;
        }

        public void help(object sender, MouseButtonEventArgs e)
        {
            m.helps_grid.Visibility = 0;
        }

        public void help_exit(object sender, MouseButtonEventArgs e)
        {
            m.helps_grid.Visibility = (Visibility)1;
        }


        public void time_down(object sender, RoutedEventArgs e)
        {
            double2 will = buy_number;
            if (!buy_int)
            {
                will = buy_percent / 100 * ex.time_boost_max;
            }

            if (will >= ex.time_boost)
            {
                ex.time_boost = 0;
            }
            else
            {
                ex.time_boost -= will;
            }
        }
        public void time_up(object sender, RoutedEventArgs e)
        {
            double2 will = buy_number;
            if (!buy_int)
            {
                will = buy_percent / 100 * ex.time_boost_max;
            }

            if (ex.time_boost + will > ex.time_boost_max)
            {
                ex.time_boost = ex.time_boost_max;
            }
            else
            {
                ex.time_boost += will;
            }
        }

        public void nmc_selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            unlocks.number_mode = number_mode = ((ComboBox)sender).SelectedIndex;
        }
    }
}
