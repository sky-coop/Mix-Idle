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
        public void g1_level_draw_世界树(bool success)
        {
            string str;
            rainbow_text rt;
            g1_milestone ms;
            g1_upgrade u;
            g1_resource r;
            g1_level level;
            g1_layer layer;
            g1_tab tab;
            g1_save_slot slot;
            g1_drawable draw;
            textblock tb;
            game_grid_element[,] game_Grid_Elements;
            game_grid game_Grid;
            List<List<Tuple<string, double2>>> ct;
            List<Tuple<string, double2>> costs;

            level = g1_current_level;

            g1_map_redraw(1,
                Math.Max(1, (3.6 + level.stage * 0.4) / 4.5));

            if (success)
            {
                int l = 400;
                int t = 120 + level.stage * 40;
                int pos_count = 2;
                bool left = false;
                for (int i = 0; i <= level.stage_max; i++)
                {
                    #region 世界树层 初始化
                    byte green = (byte)(155 + i);

                    //种子仓库
                    if (i != 0)
                    {
                        str = i + "层点数";
                        r = new g1_resource(0, str,
                            getSCB(Color.FromRgb(0, (byte)((255 + green) / 2), 225)));
                        g1_res[str] = r;
                    }
                    else
                    {
                        r = find_resource("0层点数") as g1_resource;
                    }
                    layer = new g1_layer("世界树" + i + "层", r);
                    if (i == 0)
                    {
                        level.roots.Add(layer);
                    }
                    else
                    {
                        layer.prev(g1_layers["世界树" + (i - 1) + "层"]);
                    }
                    g1_layers[layer.name] = layer;

                    layer.unlocked = false;
                    if (i <= level.stage)
                    {
                        layer.unlocked = true;
                    }
                    layer.prepare("vm_g1_map_grid", new Point(l, t),
                        /* text */A(0, green, 225), i.ToString(), "Consolas", 0.5,
                        /* fill */A(0, 75, (byte)(green / 2)), 80,
                        /* line */A(0, green, 255), 5,
                        /*stroke*/A(0, green, 255), 2);

                    if (pos_count == 0)
                    {
                        left = false;
                    }
                    if (pos_count == 4)
                    {
                        left = true;
                    }
                    //向上移动
                    if (pos_count == 0 || pos_count == 4
                        || (pos_count == 1 && left)
                        || (pos_count == 3 && !left))
                    {
                        t -= 70;
                    }
                    else
                    {
                        t -= 10;
                    }
                    if (left)
                    {
                        l -= 160;
                        pos_count--;
                    }
                    else
                    {
                        l += 160;
                        pos_count++;
                    }
                    #endregion 世界树层 初始化
                    #region tab 世界树层_主页
                    str = "世界树_" + i + "_主页";
                    tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                        new solid_type(A(0, (byte)(green / 2), green)), new solid_type(A(0, (byte)(green / 2), 0)),
                        new solid_type(A(255, 255, 255)), 1.5,
                        new solid_text("主页", 14, A(0, green, 255), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    layer.curr_tab = tab;

                    if (i > 0)
                    {
                        u = new g1_upgrade("打开" + i + "层", level, layer);
                        #region 世界树 - i - 打开本层
                        g1_ups[u.store_name()] = u;
                        u.can_reset = true;
                        u.visitable = true;
                        ct = new List<List<Tuple<string, double2>>>();
                        costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                            new Tuple<string, double2>("迷宫元素", i));
                        ct.Add(costs);
                        u.set_init_cost(ct, 0, ct.Count);
                        layer.add_upgrade(u, tab.name, 1);
                        layer.unlock_upgrade(u.name);
                        rt = new rainbow_text(u.name);
                        rt.add("解锁", 255, 255, 255);
                        rt.add("第" + i + "层", 255, green, 255);
                        rt.add("的升级，开始建设它并获得", 255, 255, 255);
                        rt.add("点数", 0, (byte)((255 + green) / 2), 225);
                        rt.add("！", 255, 255, 255);
                        rt.prepare("",
                            HorizontalAlignment.Center, VerticalAlignment.Center,
                            new Thickness(0), double.NaN, double.NaN, 14);
                        u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                            u.page + "_grid", HorizontalAlignment.Left,
                            VerticalAlignment.Top, new thickness(50, 260, 0, 0),
                            300, 100, A(175, 60, green, 170), A(192, 255, 62),
                            A(0, (byte)((255 + green) / 2), 255), 18, "打开本层！",
                            rt,
                            null, 0, "",
                            A(255, green, 255), 14);
                        u.set_weight(3, 4, 0, 3);
                        #endregion
                    }
                    #endregion tab 世界树层_主页
                    #region tab 世界树层_升级
                    str = "世界树_" + i + "_升级";
                    tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                        new solid_type(A(0, (byte)(green / 2), green)), new solid_type(A(0, (byte)(green / 2), 0)),
                        new solid_type(A(255, 255, 255)), 1.5,
                        new solid_text("升级", 14, A(0, green, 255), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    #endregion tab 世界树层_升级
                }
                #region 0
                layer = g1_layers["世界树0层"];

                str = "迷宫元素";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 135, 255)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "已用迷宫元素";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 135, 255)));
                g1_res[str] = r;

                tab = layer.tabs["世界树_0_主页"];
                #region 0 主页

                u = new g1_upgrade("回收迷宫元素", level, layer);
                #region 世界树 - 0 - 回收迷宫元素
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                u.check = false;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("已用迷宫元素", 1));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("回收", 255, 255, 255);
                rt.add("所有", 255, 255, 0);
                rt.add(find_resource("迷宫元素"));
                rt.add("，", 255, 255, 255);
                rt.add("但重置本层之上的所有层！", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 13);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 150, 0, 0),
                    300, 100, A(175, 60, 120, 170), A(192, 255, 62),
                    A(0, 255, 255), 16, "回收迷宫元素",
                    rt,
                    A(255, 255, 180), 13, "将回收 0 迷宫元素",
                    null, 0);
                u.set_weight(3, 3, 3, 0);
                #endregion


                u = new g1_upgrade("制造迷宫元素", level, layer);
                #region 世界树 - 0 - 制造迷宫元素
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("世界点数", 20e3));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("制造", 255, 255, 255);
                rt.add(" 1 ", 255, 255, 0);
                rt.add(find_resource("迷宫元素"));
                rt.add("，", 255, 255, 255);
                rt.add("建议慎重地使用它们！", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 13);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 260, 0, 0),
                    300, 100, A(175, 60, 120, 170), A(192, 255, 62),
                    A(0, 255, 255), 16, "制造迷宫元素",
                    rt,
                    A(255, 255, 180), 13, "购买将获取 1 迷宫元素",
                    A(255, 175, 0), 13);
                u.set_weight(3, 3, 3, 3);
                #endregion
                #endregion 0 主页

                tab = layer.tabs["世界树_0_升级"];
                #region 0 升级
                u = new g1_upgrade("世界的基础", level, layer);
                #region 世界树 - 0 - 世界的基础
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("世界点数", 25e3));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("立刻获得", 255, 255, 255);
                rt.add("1点", 255, 255, 0);
                rt.add(find_resource("0层点数"));
                rt.add("，随后以极慢的速度自然生成它", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                    300, 80, A(175, 60, 120, 170), A(192, 255, 62),
                    A(0, 255, 255), 15, "世界的基础",
                    rt,
                    A(255, 255, 180), 12, "目前效果：+0/s 0层点数",
                    A(255, 175, 0), 12);
                u.set_weight(3, 4, 3, 3);
                #endregion

                u = new g1_upgrade("迷宫元素沉积", level, layer);
                #region 世界树 - 0 - 迷宫元素沉积
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["世界树_世界的基础"], 1);
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("0层点数", 50));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, tab.name, 1);
                rt = new rainbow_text(u.name);
                rt.add("根据", 255, 255, 255);
                rt.add("剩余", 255, 255, 0);
                rt.add(find_resource("迷宫元素"));
                rt.add("的数量增强", 255, 255, 255);
                rt.add("上一个升级", 0, 255, 255);
                rt.add("的效果", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 200, 0, 0),
                    300, 80, A(175, 60, 120, 170), A(192, 255, 62),
                    A(0, 255, 255), 15, "迷宫元素沉积",
                    rt,
                    A(255, 255, 180), 12, "效果：×2 上一个升级的效果",
                    A(255, 175, 0), 12);
                u.set_weight(3, 4, 3, 3);
                #endregion
                #endregion 0 升级

                #endregion 0
                #region 1
                layer = g1_layers["世界树1层"];

                str = "水晶质量";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(173, 255, 47)));
                g1_res[str] = r;
                layer.resources.Add(r);

                tab = layer.tabs["世界树_1_主页"];
                #region 1 主页

                #endregion 1 主页

                tab = layer.tabs["世界树_1_升级"];
                #region 1 升级
                u = new g1_upgrade("闪亮水晶", level, layer);
                #region 世界树 - 1 - 闪亮水晶
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["世界树_打开1层"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("迷宫元素", 1)),
                    new Tuple<string, double2>("世界树等级", 12));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, tab.name, 1);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add(find_resource("1层点数"));
                rt.add("的获取", 255, 255, 225);
                rt.add("×10", 255, 255, 0);
                rt.add("，并根据", 255, 255, 255);
                rt.add(find_resource("1层点数"));
                rt.add("的值生成", 255, 255, 255);
                rt.add(find_resource("水晶质量"));
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                    300, 80, A(175, 60, 120, 170), A(192, 255, 62),
                    A(0, 255, 255), 14, "闪亮水晶",
                    rt,
                    A(255, 255, 180), 11, "目前效果：×1 1层点数获取，+0/s 水晶质量获取",
                    A(255, 175, 0), 12);
                u.set_weight(3, 5, 3, 3);
                #endregion
                #endregion 1 升级
                #endregion 1
            }
        }
    }
}
