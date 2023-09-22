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
        bool g1_glow = false;
        bool g1_second = false;
        bool first_window_show = true;
        public void g1_show()
        {
            Grid map = (Grid)vm_elems["vm_g1_map_grid"];
            Grid app_grid = vm_get_app_grid();
            Grid center = (Grid)vm_elems["vm_g1_center_grid"];
            Grid right = (Grid)vm_elems["vm_g1_right_grid"];
            Grid radar = (Grid)vm_elems["vm_g1_radar_grid"];
            Grid radar_main = (Grid)vm_elems["vm_g1_radar_main"];
            Rectangle radar_view = (Rectangle)vm_elems["vm_g1_radar_view"];

            FrameworkElement element = get_mouse();
            Grid window = (Grid)vm_elems["vm_g1_window_container"];
            if (element != null && element.Name.Contains("_layer__"))
            #region 显示层信息
            {
                string base_name = Regex.Split(element.Name, "_layer__")[0];
                string layer_name = Regex.Split(element.Name, "_layer__")[1];
                if (layer_name.Contains("_"))
                {
                    if (layer_name.Contains("line"))
                    {
                        window.Visibility = Visibility.Hidden;
                        goto window_else;
                    }
                    layer_name = Regex.Split(layer_name, "_")[0];
                }
                g1_layer layer = g1_layers[layer_name];
                if (first_window_show == true)
                {
                    first_window_show = false;
                    window.Visibility = Visibility.Hidden;
                }
                else
                {
                    window.Visibility = Visibility.Visible;
                }
                int k = 1;
                TextBlock t = (TextBlock)vm_elems["vm_g1_window_t0"];
                t.Text = layer_name + "：";
                foreach (g1_resource r in layer.resources)
                {
                    t = (TextBlock)vm_elems["vm_g1_window_t" + k];
                    t.Text = number_format(r.get_value()) + " " + r.name;
                    t.Foreground = r.text_color();
                    t.Visibility = Visibility.Visible;
                    k++;
                }
                for(; k <= 10; k++)
                {
                    t = (TextBlock)vm_elems["vm_g1_window_t" + k];
                    t.Visibility = Visibility.Collapsed;
                }

                Grid g = (Grid)vm_elems[base_name];

                if (g.Equals(map))
                {
                    Point c = layer.c_position;
                    c.X += map.Margin.Left;
                    c.Y += map.Margin.Top;
                    Point bottom_center = new Point(c.X, c.Y - layer.size * 0.5 - 10);
                    double width = 0;
                    foreach (FrameworkElement f in window.Children)
                    {
                        if (f is StackPanel)
                        {
                            foreach (FrameworkElement f2 in (f as StackPanel).Children)
                            {
                                double pad = 0;
                                if (f2 is TextBlock)
                                {
                                    TextBlock tb = f2 as TextBlock;
                                    if (tb != null)
                                    {
                                        pad = tb.Padding.Left + tb.Padding.Right;
                                    }
                                }
                                width = Math.Max(width, f2.ActualWidth + pad);
                            }
                        }
                    }
                    Point left_top = new Point(
                        bottom_center.X - window.ActualWidth * 0.5,
                        bottom_center.Y - window.ActualHeight);
                    if (left_top.X < 0)
                    {
                        left_top.X = 0;
                    }
                    if (left_top.X + width > app_grid.Width)
                    {
                        left_top.X = app_grid.Width - width;
                    }
                    if (left_top.Y < 0)
                    {
                        left_top.Y = c.Y + layer.size * 0.5 + 10;
                        if (left_top.Y + window.ActualHeight > app_grid.Height)
                        {
                            left_top.Y = app_grid.Height - window.ActualHeight;
                            left_top.X = c.X + layer.size * 0.5 + 10;
                            if (left_top.X + width > app_grid.Width)
                            {
                                left_top.X = c.X - layer.size * 0.5 - 10 - width;
                            }
                        }
                    }
                    window.Margin = new Thickness(left_top.X, left_top.Y, 0, 0);
                }
                else if (g.Equals(radar_main))
                {
                    double left = radar.Margin.Left;
                    double bottom = radar.Height + radar.Margin.Bottom + window.ActualHeight + 6;
                    double top = app_grid.Height - bottom;
                    window.Margin = new Thickness(left, top, 0, 0);
                }
            }
            else
            {
                window.Visibility = Visibility.Hidden;
                first_window_show = true;
            }
            window_else:
            #endregion 显示层信息

            if (g1_current_layer != null && g1_current_layer.curr_tab != null)
            #region 更新页码
            {
                TextBlock page_text = (TextBlock)vm_elems["vm_g1_page_text"];
                Button page_back = (Button)vm_elems["vm_g1_page_back"];
                Button page_forward = (Button)vm_elems["vm_g1_page_forward"];

                if (g1_current_layer.curr_tab.page_now > 1)
                {
                    page_back.Visibility = Visibility.Visible;
                }
                else
                {
                    page_back.Visibility = Visibility.Hidden;
                }

                if (g1_current_layer.curr_tab.page_now <
                    g1_current_layer.curr_tab.page_max)
                {
                    page_forward.Visibility = Visibility.Visible;
                }
                else
                {
                    page_forward.Visibility = Visibility.Hidden;
                }
                page_text.Text = "第 " + g1_current_layer.curr_tab.page_now + " / " +
                    g1_current_layer.curr_tab.page_max + " 页";
            }
            #endregion 更新页码

            #region 返回上一关
            Grid ret_level = (Grid)vm_elems["vm_g1_center_ret_grid"];
            if (g1_current_level.prevs.Count > 0)
            {
                ret_level.Visibility = Visibility.Visible;
            }
            else
            {
                ret_level.Visibility = Visibility.Hidden;
            }
            #endregion 返回上一关

            #region scenery
            foreach (g1_scenery scenery in g1_current_level.sceneries)
            {
                if (scenery.unlocked)
                {
                    g1_draw_scenery(scenery);
                }
            }
            #endregion scenery

            #region 实时更新大小
            if (g1_current_level.size_change)
            {
                if (g1_current_level.name == "世界树")
                {
                    /*
                    if (s_ticker("test", 0.5))
                    {
                        yggdrasill_change_stage(1);
                    }*/
                }
            }
            #endregion 实时更新大小

            #region 闪烁
            g1_glow = s_ticker("g1_glow", 0.5);
            if (g1_glow)
            {
                Queue<g1_layer> q = g1_current_level.getAllLayers();
                foreach (g1_layer h in q)
                {
                    h.glowing = false;
                    foreach (KeyValuePair<string, g1_tab> t in h.tabs)
                    {
                        t.Value.glowing = false;
                    }
                }

                foreach (g1_upgrade u in g1_current_level.GetAllUpgrades())
                {
                    if (u.check && u.visitable && !(u is g1_milestone) && 
                        u.level < u.max_level)
                    {
                        bool affordable = true;
                        if (u.special)
                        {
                            affordable = g1_special_check(u);
                        }
                        List<Tuple<string, double2>> tuples = u.cost_table[u.level];
                        foreach (Tuple<string, double2> t in tuples)
                        {
                            resource r = find_resource(t.Item1);
                            if (r.get_value() < t.Item2)
                            {
                                affordable = false;
                            }
                        }
                        if (affordable)
                        {
                            u.tab.glowing = true;
                            u.g1_layer.glowing = true;
                        }
                    }
                }
                g1_second = !g1_second;
            }
            #endregion 闪烁

            #region 层绘制
            foreach (g1_layer h in g1_current_level.getAllLayers())
            {
                if (h.name == "世界树")
                {
                    h.size = 100 + 10 * (yggdrasill.level + 1).Log10().d;
                    h.stroke_t = 2 + 0.1 * (yggdrasill.level + 1).Log10().d;
                }
                g1_draw_layer_icon(h);
                g1_draw_layer_grid(h);
            }
            #endregion 层绘制

            #region 关卡::资源
            foreach (resource r in g1_current_level.resources)
            {
                double2 val = r.get_value();
                if (g1_current_level.name == "自然树" &&
                    r.name == "生命力")
                {
                    val = g1_cal_生命力_自然树().Item2;
                }
                if (g1_current_level.name == "水晶树" &&
                    r.name == "生命力")
                {
                    val = g1_cal_生命力_水晶树().Item2;
                }
                if (g1_current_level.name == "合成树" &&
                    r.name == "生命力")
                {
                    val = g1_cal_生命力_合成树().Item2;
                }

                rainbow_text rainbow = new rainbow_text(r.name);
                rainbow.add("你有 ", 255, 255, 255);
                rainbow.add(number_format(val), 0, 255, 255);
                rainbow.add(" " + r.name, r.r, r.g, r.b);

                rainbow.prepare("vm_g1_top_panel", HorizontalAlignment.Center,
                    VerticalAlignment.Top, new Thickness(0), double.NaN, double.NaN,
                    16);
                draw_r_text(rainbow);

                if (r.name == "世界点数")
                {
                    rainbow = new rainbow_text(g1_current_level.name + "_" +
                         r.name + "_effect");
                    rainbow.add("世界点数 ", 255, 255, 255);
                    rainbow.add("提供 ", 200, 200, 200);
                    rainbow.add("+" + number_format(g1_cal_wp_effect()), 255, 255, 0);
                    rainbow.add(" 娱乐币/s", 0, 255, 255);
                    rainbow.add(" 产生速率", 200, 200, 200);
                    rainbow.prepare("vm_g1_top_panel",
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new Thickness(0), double.NaN, double.NaN, 14);
                    draw_r_text(rainbow);
                }
                if (r.name == "生命力")
                {
                    if (g1_current_level.name == "自然树")
                    {
                        rainbow = new rainbow_text(g1_current_level.name + "_" +
                             r.name + "_effect");
                        rainbow.add("生命力 ", 127, 255, 0);
                        rainbow.add("提供 ", 200, 200, 200);
                        rainbow.add("×" + number_format(g1_cal_life_effect(true,
                            g1_current_level.name)), 0, 255, 255);
                        rainbow.add(" 树成长进度 ", 0, 255, 127);
                        rainbow.add("获取速率 ", 200, 200, 200);
                        rainbow.prepare("vm_g1_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rainbow);
                    }
                    if (g1_current_level.name == "水晶树")
                    {
                        rainbow = new rainbow_text(g1_current_level.name + "_" +
                             r.name + "_effect");
                        rainbow.add("生命力 ", 127, 255, 0);
                        rainbow.add("提供 ", 200, 200, 200);
                        rainbow.add("×" + number_format(g1_cal_life_effect(true,
                            g1_current_level.name)), 0, 255, 255);
                        rainbow.add(" 绿色水晶生成力", 127, 255, 127);
                        rainbow.prepare("vm_g1_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rainbow);
                    }
                    if (g1_current_level.name == "合成树")
                    {
                        rainbow = new rainbow_text(g1_current_level.name + "_" +
                             r.name + "_effect");
                        rainbow.add("生命力 ", 127, 255, 0);
                        rainbow.add("提供 ", 200, 200, 200);
                        rainbow.add("×" + number_format(double2.Log10
                            (g1_cal_life_effect(true, "合成树") + 9)), 0, 255, 255);
                        rainbow.add(" 生命球生成比率", 127, 255, 127);
                        rainbow.prepare("vm_g1_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rainbow);
                    }
                }
                if (r.name == "生命转化")
                {
                    rainbow = new rainbow_text(g1_current_level.name + "_" +
                         r.name + "_effect");
                    rainbow.add(r);
                    rainbow.add(" 提供 ", 200, 200, 200);
                    rainbow.add("×" + number_format(g1_cal_lifec_red_effect()), 0, 255, 255);
                    rainbow.add(" 红色水晶生成力", 255, 127, 127);
                    rainbow.prepare("vm_g1_top_panel",
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new Thickness(0), double.NaN, double.NaN, 14);
                    draw_r_text(rainbow);
                }
            }
            #endregion 关卡::资源


            #region 位置更新
            if (g1_mode == g1_show_mode.right)
            {
                right.Visibility = Visibility.Visible;
                center.Width = app_grid.Width - right.Width;
            }
            else
            {
                right.Visibility = Visibility.Hidden;
                center.Width = app_grid.Width;

            }
            Rectangle view = (Rectangle)vm_elems["vm_g1_radar_view"];
            //g1_view_point
            radar_view.Width = radar.Width / g1_mw * (center.Width / app_grid.Width);
            margin_move(radar_view, 0, 0,
                0, radar.Width - radar_view.Width,
                0, radar.Height - radar_view.Height);
            g1_current_level.view_point.X = radar_view.Margin.Left;
            g1_current_level.view_point.Y = radar_view.Margin.Top;
            g1_view_syn();
            #endregion 位置更新

            #region 层grid更新
            rainbow_text rt;
            if (g1_current_layer != null)
            {
                foreach(KeyValuePair<g1_upgrade, g1_tab> kp in g1_current_layer.upgrades)
                {
                    if (g1_current_layer.curr_tab.Equals(kp.Value))
                    {
                        if (kp.Key.attaching)
                        {

                        }
                        if(!kp.Key.attaching && kp.Value.page_now != kp.Key.page)
                        {
                            continue;
                        }
                        if (kp.Key is g1_milestone)
                        {
                            g1_milestone ms = kp.Key as g1_milestone;
                            Grid ret = draw_drawable(ms.draw);
                            if (ms.attaching || ms.visitable)
                            {
                                ret.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                ret.Visibility = Visibility.Hidden;
                            }
                        }
                        else if(kp.Key is g1_research)
                        {
                            g1_research r = kp.Key as g1_research;
                            Grid ret = draw_drawable(r.bar.draw);
                            if (r.attaching || r.visitable)
                            {
                                ret.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                ret.Visibility = Visibility.Hidden;
                            }
                        }
                        else
                        {
                            g1_draw_upgrade(kp.Key);
                        }
                    }
                }

                foreach (g1_drawable d in g1_current_layer.drawables)
                {
                    if (g1_current_layer.curr_tab.Equals(d.tab) && 
                        d.attaching)
                    {
                        d.target = "vm_g1_layer_" + g1_current_layer.name +
                            "_" + d.tab.name + "_attach_grid";
                        Grid draw = draw_drawable(d);
                        draw.Visibility = d.v;
                    }
                    if (g1_current_layer.curr_tab.Equals(d.tab) &&
                        d.tab.page_now == d.page)
                    {
                        if (d.name == "水晶树_公式e")
                        #region 更新公式页
                        {
                            double2 p = g1_cal_crystal_rgb_log_avg();
                            resource rr = find_resource("红色水晶");
                            resource rg = find_resource("绿色水晶");
                            resource rb = find_resource("蓝色水晶");
                            double2 min = double2.Min(rr.get_value(), rg.get_value());
                            min = double2.Min(min, rb.get_value());
                            double2 vir = new double2(Math.Max(1, min.d * 1e-6), min.i);
                            foreach (textblock t in d.textblocks)
                            {
                                if (t.name == "水晶树_公式_et4")
                                {
                                    t.content = "P：生成的水晶块/s("
                                        + number_format(p) + ")[另有×"
                                        + number_format(g1_crystal_mul) + "加成]";
                                }
                                if (t.name == "水晶树_公式_et5")
                                {
                                    t.content = "R：红色水晶数("
                                        + number_format(rr.get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_et6")
                                {
                                    t.content = "G：绿色水晶数("
                                        + number_format(rg.get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_et7")
                                {
                                    t.content = "B：蓝色水晶数("
                                        + number_format(rb.get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_etr")
                                {
                                    t.content = "+" + number_format(vir)
                                        + " 红色水晶 带来的效果：+" 
                                        + number_format(g1_cal_crystal_rgb_log_avg_virtual(vir, 0, 0) - p)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_etg")
                                {
                                    t.content = "+" + number_format(vir)
                                        + " 绿色水晶 带来的效果：+"
                                        + number_format(g1_cal_crystal_rgb_log_avg_virtual(0, vir, 0) - p)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_etb")
                                {
                                    t.content = "+" + number_format(vir)
                                        + " 蓝色水晶 带来的效果：+"
                                        + number_format(g1_cal_crystal_rgb_log_avg_virtual(0, 0, vir) - p)
                                        + " 水晶块/s";
                                }
                            }
                        }
                        #endregion 更新公式页
                        if (d.name.Contains("水晶树_公式n"))
                        #region 更新公式页
                        {
                            double2 p = g1_cal_crystal_all_log_avg();
                            double2 min = g1_crystal_all_min[0].get_value();
                            double2 vir = new double2(Math.Max(1, min.d * 1e-6), min.i);
                            foreach (textblock t in d.textblocks)
                            {
                                if (t.name == "水晶树_公式_n1t4")
                                {
                                    t.content = "P：生成的水晶块/s("
                                        + number_format(p) + ")[另有×"
                                        + number_format(g1_crystal_mul) + "加成]";
                                }
                                if (t.name == "水晶树_公式_n1t5")
                                {
                                    t.content = "R：红色水晶数("
                                        + number_format(find_resource("红色水晶").get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_n1t6")
                                {
                                    t.content = "G：绿色水晶数("
                                        + number_format(find_resource("绿色水晶").get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_n1t7")
                                {
                                    t.content = "B：蓝色水晶数("
                                        + number_format(find_resource("蓝色水晶").get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_n1t8")
                                {
                                    t.content = "Y：黄色水晶数("
                                        + number_format(find_resource("黄色水晶").get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_n1t9")
                                {
                                    t.content = "M：洋红色水晶数("
                                        + number_format(find_resource("洋红色水晶").get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_n1t10")
                                {
                                    t.content = "C：青色水晶数("
                                        + number_format(find_resource("青色水晶").get_value()) + ")";
                                }
                                if (t.name == "水晶树_公式_n1t11")
                                {
                                    t.content = "W：白色水晶数("
                                        + number_format(find_resource("白色水晶").get_value()) + ")";
                                }

                                if (t.name == "水晶树_公式_n2tr")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (vir, 0, 0, 0, 0, 0, 0);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 红色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_n2tg")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (0, vir, 0, 0, 0, 0, 0);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 绿色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_n2tb")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (0, 0, vir, 0, 0, 0, 0);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 蓝色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_n2ty")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (0, 0, 0, vir, 0, 0, 0);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 黄色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_n2tm")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (0, 0, 0, 0, vir, 0, 0);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 洋红色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_n2tc")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (0, 0, 0, 0, 0, vir, 0);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 青色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                                if (t.name == "水晶树_公式_n2tw")
                                {
                                    double2 v = g1_cal_crystal_all_log_avg_virtual
                                        (0, 0, 0, 0, 0, 0, vir);
                                    double2 diff = 0;
                                    if (v - p < p * 1e-12)
                                    {
                                        diff = 0;
                                    }
                                    else
                                    {
                                        diff = v - p;
                                    }
                                    t.content = "+" + number_format(vir)
                                        + " 白色水晶 带来的效果：+"
                                        + number_format(diff)
                                        + " 水晶块/s";
                                }
                            }
                        }
                        #endregion 更新公式页
                        Grid draw = draw_drawable(d);
                        draw.Visibility = d.v;
                        if (d is game_grid)
                        {
                            game_grid gg = d as game_grid;
                            if (draw.RowDefinitions.Count > gg.row())
                            {
                                draw.RowDefinitions.Clear();
                            }
                            for (int i = draw.RowDefinitions.Count; i < gg.row(); i++)
                            {
                                draw.RowDefinitions.Add(new RowDefinition());
                            }
                            if (draw.ColumnDefinitions.Count > gg.col())
                            {
                                draw.ColumnDefinitions.Clear();
                            }
                            for (int i = draw.ColumnDefinitions.Count; i < gg.col(); i++)
                            {
                                draw.ColumnDefinitions.Add(new ColumnDefinition());
                            }
                            if (gg.name == "m3")
                            {
                                Tuple<string, Point> message = temp_texts["m3_produce"];
                                string s = message.Item1;
                                Point pos = message.Item2;
                                if (s != "")
                                {
                                    Point base_relative =
                                        (Point)VisualTreeHelper.GetOffset(draw);
                                    Point core_relative =
                                        new Point(
                                            (pos.Y + 0.5) * draw.Width / draw.ColumnDefinitions.Count,
                                            (pos.X + 0.5) * draw.Height / draw.RowDefinitions.Count
                                            );
                                    Point p =
                                        new Point(base_relative.X + core_relative.X,
                                                  base_relative.Y + core_relative.Y + 10);
                                    float_message f = new float_message(
                                        "f" + float_message_id, d.target, HorizontalAlignment.Left,
                                        VerticalAlignment.Top, double.NaN, double.NaN,
                                        new thickness(p.X, p.Y, 0, 0),
                                        new textblock("", false, "", s, A(255, 255, 255),
                                        double.NaN, double.NaN, 12, new thickness(0, 0, 0, 0)), 1);
                                    f.background = A(50, 50, 50);
                                    float_message_id++;
                                    float_messages.Add(f);
                                }

                            }
                            foreach (game_grid_element e in gg.elems)
                            {
                                if (e != null)
                                {
                                    e.draw.target = d.name_base() + "_grid";

                                    string text = "";
                                    if (gg.name == "m3")
                                    {
                                        if (e.special != 0)
                                        {
                                            if (e.special < 10)
                                            {
                                                text = e.special.ToString();
                                            }
                                            else
                                            {
                                                int range = e.special % 10 + 1;
                                                int blast = e.special % 100 / 10;
                                                int change = e.special % 1000 / 100;

                                                for (int i = 0; i < change; i++)
                                                {
                                                    text += "C";
                                                }
                                                for (int i = 0; i < blast; i++)
                                                {
                                                    text += "B";
                                                }
                                                if (range > 1)
                                                {
                                                    text += range.ToString();
                                                }
                                            }
                                        }

                                    }
                                    e.draw.textblocks[0].content = text;
                                    e.draw.textblocks[0].size = draw.Width / draw.ColumnDefinitions.Count * 0.4 *
                                        (1.3 / (text.Length * 0.3 + 1));

                                    if (e.progress != 1 || e.selectable == false)
                                    {
                                        e.draw.masking = true;
                                    }
                                    else
                                    {
                                        e.draw.masking = false;
                                    }
                                    if (g1_current_level.name == "合成树" && e.type > 0)
                                    {
                                        resource r = g1_layers["合成资源"].resources[
                                            e.type - 1];
                                        color_copy(ref e.draw.rectangles[0].color, 255, r.r, r.g, r.b);
                                        color_copy(ref e.draw.rectangles[0].stroke_color, 
                                            255,
                                            (byte)(r.r / 2),
                                            (byte)(r.g / 2),
                                            (byte)(r.b / 2));
                                        e.draw.rectangles[0].width = draw.Width / draw.ColumnDefinitions.Count * 0.75;
                                        e.draw.rectangles[0].height = draw.Height / draw.RowDefinitions.Count * 0.75;
                                    }
                                    //e.draw.width = draw.Width / draw.ColumnDefinitions.Count * e.progress;
                                    //e.draw.height = draw.Height / draw.RowDefinitions.Count * e.progress;
                                    Grid draw_e = draw_drawable(e.draw);
                                    Grid.SetRow(draw_e, e.r);
                                    Grid.SetColumn(draw_e, e.c);
                                    
                                    if (e.type == 0)
                                    {
                                        draw_e.Visibility = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        draw_e.Visibility = Visibility.Visible;
                                    }
                                    /*
                                    if (e.type == 0)
                                    {
                                        e.progress = 1;
                                    }*/

                                    //e.progress = Math.Min(e.progress + 0.001, 1);
                                    scale(draw_e, e.progress, e.progress, 0.5, 0.5);
                                }
                            }
                        }
                    }
                }

                foreach(resource r in g1_current_layer.resources)
                {
                    rt = new rainbow_text(r.name);
                    rt.add("你有 ", 255, 255, 255);
                    rt.add(number_format(r.get_value()), 255, 255, 0);
                    rt.add(" " + r.name, r.r, r.g, r.b);
                    rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new Thickness(0), double.NaN, double.NaN, 16);
                    draw_r_text(rt);

                    //效果
                    #region level 世界
                    if (r.name == "世界树等级")
                    #region
                    {
                        rt = new rainbow_text(yggdrasill.exp_bar.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add(number_format(yggdrasill.produce()) + "/s", 0, 255, 255);
                        rt.add(" 世界点数", 255, 255, 255);
                        rt.add(" 基础生产速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(yggdrasill.exp_bar.name + "_gain");
                        rt.add("(+" + number_format(yggdrasill.exp_gain * global_xp_boost()
                            * yggdrasill.get_exp_mul()) + "/s)", 0, 255, 0);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_grid",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 90, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        yggdrasill.exp_bar.draw.s_content.text =
                            "EXP: " + number_format(yggdrasill.current_exp)
                            + " / " + number_format(yggdrasill.exp_need_now);
                        yggdrasill.exp_bar.draw.set_p_progress(yggdrasill.current_exp,
                            yggdrasill.exp_need_now);

                        draw_drawable(yggdrasill.exp_bar.draw);
                    }
                    #endregion
                    if (r.name == "生命力")
                    #region
                    {
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("×" + number_format(g1_cal_life_effect()), 0, 255, 255);
                        rt.add(" 世界树经验", 0, 255, 0);
                        rt.add(" 产生速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion
                    if (r.name == "生命效果")
                    #region
                    {
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("使 ", 200, 200, 200);
                        rt.add(find_resource("生命力"));
                        rt.add(" 的效果公式为", 200, 200, 200);
                        rt.add(" (生命力 ^ " + number_format(r.get_value()) + ")", 0, 255, 255);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion
                    if (r.name == "自然力量")
                    #region
                    {
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("×" + number_format(g1_cal_natural_effect()), 0, 255, 255);
                        rt.add(" 生命力", 127, 255, 0);
                        rt.add(" 获取倍率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion
                    if (r.name == "水晶球")
                    #region
                    {
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("+" + number_format(g1_cal_crystalball_effect()) + " ", 0, 255, 255);
                        rt.add(find_resource("生命效果"));
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion
                    #endregion level 世界

                    #region level 世界树
                    if (r.name == "0层点数")
                    #region 0
                    {
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("0层点数 ", 0, 205, 225);
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("+" + number_format(g1_cal_0_wp_effect()), 0, 255, 255);
                        rt.add(" 世界点数生产指数", 0, 255, 0);
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion 0
                    if (r.name == "1层点数")
                    #region 1
                    {
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add(r);
                        rt.add(" 先提供 ", 200, 200, 200);
                        rt.add("×" + number_format(g1_cal_1_0_effect_mul()) + " ", 0, 255, 255);
                        rt.add(find_resource("0层点数"));
                        rt.add(" 产量，再提供 ", 200, 200, 200);
                        rt.add("+" + number_format(g1_cal_1_0_effect_add()) + "/s ", 0, 255, 127);
                        rt.add(find_resource("0层点数"));
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(50, 0, 50, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    if (r.name == "水晶质量")
                    {
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add(r);
                        rt.add(" 提供 ", 200, 200, 200);
                        rt.add("×" + number_format(g1_cal_1_crystal_crystalpiece_effect()), 0, 255, 255);
                        rt.add(" 水晶块", 255, 255, 255);
                        rt.add(" 获取速率", 200, 200, 200);
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 0, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion 1
                    #endregion level 世界树

                    #region level 自然树
                    if (r.name == "种子成长度")
                    #region
                    {
                        resource r_seed = find_resource("种子");

                        rt = new rainbow_text(seed.exp_bar.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add(number_format(r_seed.get_value() * seed.produce()) + "/s",
                            0, 255, 255);
                        rt.add(" 自然点数", 0, 255, 255);
                        rt.add(" 基础生产速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(seed.exp_bar.name + "_gain");
                        rt.add("(+" + number_format(seed.get_exp_gain()) + "/s)", 0, 255, 0);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_grid",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 110, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        seed.exp_bar.draw.s_content.text =
                            "生长进度: " + number_format(seed.current_exp)
                            + " / " + number_format(seed.exp_need_now);
                        seed.exp_bar.draw.set_p_progress(seed.current_exp,
                            seed.exp_need_now);

                        draw_drawable(seed.exp_bar.draw);
                    }
                    #endregion
                    if (r.name == "树苗成长度")
                    #region
                    {
                        resource r_seed = find_resource("树苗");

                        rt = new rainbow_text(sapling.exp_bar.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add(number_format(r_seed.get_value() * sapling.produce()) + "/s",
                            0, 255, 255);
                        rt.add(" 自然点数", 0, 255, 255);
                        rt.add(" 基础生产速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(sapling.exp_bar.name + "_gain");
                        rt.add("(+" + number_format(sapling.get_exp_gain()) + "/s)", 0, 255, 0);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_grid",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 110, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        sapling.exp_bar.draw.s_content.text =
                            "生长进度: " + number_format(sapling.current_exp)
                            + " / " + number_format(sapling.exp_need_now);
                        sapling.exp_bar.draw.set_p_progress(sapling.current_exp,
                            sapling.exp_need_now);

                        draw_drawable(sapling.exp_bar.draw);
                    }
                    #endregion
                    if (r.name == "小树成长度")
                    #region
                    {
                        resource r_seed = find_resource("小树");

                        rt = new rainbow_text(smalltree.exp_bar.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add(number_format(r_seed.get_value() * smalltree.produce()) + "/s",
                            0, 255, 255);
                        rt.add(" 自然点数", 0, 255, 255);
                        rt.add(" 基础生产速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(smalltree.exp_bar.name + "_gain");
                        rt.add("(+" + number_format(smalltree.get_exp_gain()) + "/s)", 0, 255, 0);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_grid",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 110, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        smalltree.exp_bar.draw.s_content.text =
                            "生长进度: " + number_format(smalltree.current_exp)
                            + " / " + number_format(smalltree.exp_need_now);
                        smalltree.exp_bar.draw.set_p_progress(smalltree.current_exp,
                            smalltree.exp_need_now);

                        draw_drawable(smalltree.exp_bar.draw);
                    }
                    #endregion
                    if (g1_current_layer.name == "水")
                    #region
                    {
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("+" + number_format(g1_cal_water_effect()) + "/s", 0, 255, 255);
                        rt.add(" 树成长进度", 127, 255, 0);
                        rt.add(" 获取速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion
                    if (g1_current_layer.name == "营养")
                    #region
                    {
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("×" + number_format(g1_cal_nu_tree_effect()), 0, 255, 255);
                        rt.add(" 树成长进度", 127, 255, 0);
                        rt.add(" 获取速率", 200, 200, 200);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect2");
                        rt.add("提供 ", 200, 200, 200);
                        rt.add("+" + number_format(g1_cal_nu_water_effect()), 0, 255, 255);
                        rt.add(" 水 效果指数", 127, 255, 0);
                        rt.prepare("vm_g1_layer_" + g1_current_layer.name + "_right_top_panel",
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion
                    #endregion level 自然树

                    #region level 水晶树
                    if (r.name == "黄色水晶")
                    #region Y
                    {
                        resource rx = g1_crystal_all_max[0];
                        if (rx.Equals(none))
                        {
                            rx = find_resource("红色水晶");
                        }
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        if (g1_ups["水晶树_黄色水晶释放"].level >= 1)
                        {
                            rt.add(find_resource("黄色水晶"));
                            rt.add(" 提供 ", 200, 200, 200);
                            rt.add("×" + number_format(g1_cal_Y_effect()) + " ", 0, 255, 255);
                            rt.add(rx);
                            rt.add(" 获取倍率 ", 200, 200, 200);
                            if (g1_ups["水晶树_黄色水晶控制"].valid)
                            {
                                rt.add("(有效)", 0, 255, 0);
                            }
                            else
                            {
                                rt.add("(无效)", 255, 127, 127);
                            }
                        }
                        else
                        {
                            rt.list.Clear();
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect2");
                        if (g1_ups["水晶树_黄金比例"].level >= 1)
                        {
                            rt.add(find_resource("黄色水晶"));
                            rt.add(" 提供 ", 200, 200, 200);
                            rt.add("×" + number_format(double2.Pow(g1_cal_Y_effect(),
                                0.618)) + " ", 0, 255, 255);
                            rt.add("其他水晶", 255, 255, 255);
                            rt.add(" 获取倍率 ", 200, 200, 200);
                            if (g1_ups["水晶树_黄色水晶控制"].valid)
                            {
                                rt.add("(有效)", 0, 255, 0);
                            }
                            else
                            {
                                rt.add("(无效)", 255, 127, 127);
                            }
                        }
                        else
                        {
                            rt.list.Clear();
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion Y
                    if (r.name == "洋红色水晶")
                    #region M
                    {
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        if (g1_ups["水晶树_洋红色水晶释放"].level >= 1)
                        {
                            rt.add(r);
                            rt.add(" 提供 ", 200, 200, 200);
                            rt.add("×" + number_format(g1_cal_M_effect()) + " ", 0, 255, 255);
                            rt.add(find_resource("水晶块"));
                            rt.add(" 获取倍率 ", 200, 200, 200);
                        }
                        else
                        {
                            rt.list.Clear();
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect2");
                        if (g1_ups["水晶树_洋红色水晶释放"].level >= 1)
                        {
                            rt.add("距上次重置的时间： ", 200, 200, 200);
                            rt.add(number_format(g1_cal_M_remain_time()) + "s", 255, 127, 255);
                        }
                        else
                        {
                            rt.list.Clear();
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion M
                    if (r.name == "青色水晶")
                    #region C
                    {
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        double2 c_effect2 = g1_cal_C_effect();
                        if (g1_ups["水晶树_青色水晶释放"].level >= 1)
                        {
                            rt.add(r);
                            rt.add(" 提供 ", 200, 200, 200);
                            rt.add("×" + number_format(c_effect2) + " ", 0, 255, 255);
                            rt.add("所有水晶", 255, 255, 255);
                            rt.add(" 获取倍率", 200, 200, 200);
                        }
                        else
                        {
                            rt.list.Clear();
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 0, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);

                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect2");
                        if (g1_ups["水晶树_青色水晶释放"].level >= 1)
                        {
                            if (g1_ups["水晶树_超级转换"].level >= 1)
                            {
                                c_effect2 = double2.Pow(c_effect2, 0.5);
                            }
                            rt.add(r);
                            rt.add(" 提供 ", 200, 200, 200);
                            rt.add("/" + number_format(c_effect2) + " ", 255, 127, 127);
                            rt.add(find_resource("水晶块"));
                            rt.add(" 获取倍率", 200, 200, 200);
                        }
                        else
                        {
                            rt.list.Clear();
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 0, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion C
                    if (r.name == "白色水晶")
                    #region W
                    {
                        Panel target = (Panel)vm_elems["vm_g1_layer_" + g1_current_layer.name + "_right_top_panel"];
                        rt = new rainbow_text(g1_current_layer.name + "_" + r.name + "_effect");
                        if (g1_ups["水晶树_白色水晶释放"].level >= 1)
                        {
                            rt.add(r);
                            rt.add(" 提供 ", 200, 200, 200);
                            rt.add("×" + number_format(g1_cal_W_temp) + " ", 0, 255, 255);
                            rt.add("所有水晶", 255, 255, 255);
                            rt.add(" 获取倍率", 200, 200, 200);
                        }
                        rt.prepare(target.Name,
                            HorizontalAlignment.Center, VerticalAlignment.Top,
                            new Thickness(0, 0, 0, 0), double.NaN, double.NaN, 14);
                        draw_r_text(rt);
                    }
                    #endregion W
                    #endregion level 水晶树
                }


            }
            #endregion 层grid更新
        }

        public void g1_resource_syn()
        {
            resource r;
            g1_research x;
            string str = "";

            #region 世界树等级
            r = g1_res["世界树等级"];
            if (r.rev)
            {
                yggdrasill.set_level((decimal)r.get_value().d);
                r.rev = false;
            }
            else
            {
                r.set_value(yggdrasill.level);
            }
            #endregion 世界树等级

            #region 研究
            string[] strs = { "食物学", "战斗学", "语言与文字" };
            foreach(string s in strs)
            {
                r = g1_res[s];
                x = g1_ups["世界_" + s] as g1_research;
                if (r.rev)
                {
                    x.xp.level = r.get_value();
                    r.rev = false;
                }
                else
                {
                    r.set_value(x.xp.level);
                }
            }
            #endregion 研究

            #region 种子
            if (g1_res.ContainsKey("种子"))
            {
                r = g1_res["种子"];
                seed.n_seed = double2.Max(1, r.get_value());
            }

            if (g1_res.ContainsKey("种子成长度"))
            {
                r = g1_res["种子成长度"];
                if (r.rev)
                {
                    seed.set_level(r.get_value().d);
                    r.rev = false;
                }
                else
                {
                    r.set_value(seed.level);
                }
            }
            #endregion 种子

            #region 树苗
            if (g1_res.ContainsKey("树苗"))
            {
                r = g1_res["树苗"];
                sapling.n_seed = double2.Max(1, r.get_value());
            }

            if (g1_res.ContainsKey("树苗成长度"))
            {
                r = g1_res["树苗成长度"];
                if (r.rev)
                {
                    sapling.set_level(r.get_value().d);
                    r.rev = false;
                }
                else
                {
                    r.set_value(sapling.level);
                }
            }
            #endregion 树苗

            #region 小树
            if (g1_res.ContainsKey("小树"))
            {
                r = g1_res["小树"];
                smalltree.n_seed = double2.Max(1, r.get_value());
            }

            if (g1_res.ContainsKey("小树成长度"))
            {
                r = g1_res["小树成长度"];
                if (r.rev)
                {
                    smalltree.set_level(r.get_value().d);
                    r.rev = false;
                }
                else
                {
                    r.set_value(smalltree.level);
                }
            }
            #endregion 小树
        }
    }
}
