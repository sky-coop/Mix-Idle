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
        public void g1_level_draw_水晶树(bool success)
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

            if (level.difficulty == g1_level.type.easy)
            {
                g1_map_redraw(1, 2); //简单难度
            }
            else
            {
                g1_map_redraw(1, 2.5);
            }

            if (success)
            {
                #region level 水晶树
                /*
                str = "水晶块";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(240, 255, 240)));
                g1_res[str] = r;*/
                resource crystal = find_resource("水晶块");

                level.clear();
                level.resources.Add(find_resource("生命转化"));
                level.resources.Add(find_resource("生命力"));
                //level.resources.Add(r);
                level.resources.Add(find_resource("水晶块"));

                //   D
                // R G B
                // Y M C
                //   W

                #region layer 水晶聚合处
                str = "红色水晶生成力";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(255, 150, 150)));
                g1_res[str] = r;
                layer = new g1_layer("水晶聚合处", r);
                level.roots.Add(layer);
                g1_layers[layer.name] = layer;

                str = "绿色水晶生成力";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(127, 255, 127)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "蓝色水晶生成力";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(150, 150, 255)));
                g1_res[str] = r;
                layer.resources.Add(r);

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(400, 225),
                    /* text */A(255, 255, 255), "A", "Consolas", 0.75,
                    /* fill */A(125, 125, 125), 100,
                    /* line */A(255, 255, 255), 5,
                    /*stroke*/A(255, 255, 255), 2);
                #endregion layer 水晶聚合处
                #region layer 红色水晶
                str = "红色水晶";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 63, 63)));
                g1_res[str] = r;
                layer = new g1_layer("红色水晶", r);
                layer.prev(g1_layers["水晶聚合处"]);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(200, 450),
                    /* text */A(255, 0, 0), "R", "Consolas", 0.75,
                    /* fill */A(47, 79, 79), 100,
                    /* line */A(255, 0, 0), 5,
                    /*stroke*/A(255, 0, 0), 2);
                #endregion layer 红色水晶
                #region layer 绿色水晶
                str = "绿色水晶";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(63, 255, 63)));
                g1_res[str] = r;
                layer = new g1_layer("绿色水晶", r);
                layer.prev(g1_layers["水晶聚合处"]);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(400, 450),
                    /* text */A(0, 255, 0), "G", "Consolas", 0.75,
                    /* fill */A(136, 90, 175), 100,
                    /* line */A(0, 255, 0), 5,
                    /*stroke*/A(0, 255, 0), 2);
                #endregion layer 绿色水晶
                #region layer 蓝色水晶
                str = "蓝色水晶";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(127, 127, 255)));
                g1_res[str] = r;
                layer = new g1_layer("蓝色水晶", r);
                layer.prev(g1_layers["水晶聚合处"]);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(600, 450),
                    /* text */A(0, 0, 255), "B", "Consolas", 0.75,
                    /* fill */A(139, 101, 8), 100,
                    /* line */A(0, 0, 255), 5,
                    /*stroke*/A(0, 0, 255), 2);
                #endregion layer 蓝色水晶
                if (level.difficulty == g1_level.type.easy)
                {
                    #region layer 纯色工厂
                    str = "红色水晶原料";
                    r = new g1_resource(0, str,
                        getSCB(Color.FromRgb(255, 150, 150)));
                    g1_res[str] = r;
                    layer = new g1_layer("纯色工厂", r);
                    layer.prev(g1_layers["红色水晶"]);
                    layer.prev(g1_layers["绿色水晶"]);
                    layer.prev(g1_layers["蓝色水晶"]);
                    g1_layers[layer.name] = layer;

                    str = "绿色水晶原料";
                    r = new g1_resource(0, str,
                        getSCB(Color.FromRgb(127, 255, 127)));
                    g1_res[str] = r;
                    layer.resources.Add(r);

                    str = "蓝色水晶原料";
                    r = new g1_resource(0, str,
                        getSCB(Color.FromRgb(150, 150, 255)));
                    g1_res[str] = r;
                    layer.resources.Add(r);

                    layer.unlocked = true;
                    layer.prepare("vm_g1_map_grid", new Point(400, 675),
                        /* text */A(50, 50, 50), "F", "Consolas", 0.75,
                        /* fill */A(225, 225, 225), 100,
                        /* line */A(100, 100, 100), 5,
                        /*stroke*/A(100, 100, 100), 2);
                    #endregion layer 纯色工厂
                }
                else
                {
                    #region layer 黄色水晶
                    str = "黄色水晶";
                    r = new g1_resource(2, str,
                        getSCB(Color.FromRgb(255, 255, 127)));
                    r.reset_base = 2;
                    g1_res[str] = r;
                    layer = new g1_layer("黄色水晶", r);
                    layer.prev(g1_layers["红色水晶"]);
                    layer.prev(g1_layers["绿色水晶"]);
                    g1_layers[layer.name] = layer;

                    layer.unlocked = true;
                    layer.prepare("vm_g1_map_grid", new Point(200, 675),
                        /* text */A(255, 255, 0), "Y", "Consolas", 0.75,
                        /* fill */A(100, 100, 255), 100,
                        /* line */A(255, 255, 0), 5,
                        /*stroke*/A(255, 255, 0), 2);
                    #endregion layer 黄色水晶
                    #region layer 洋红色水晶
                    str = "洋红色水晶";
                    r = new g1_resource(2, str,
                        getSCB(Color.FromRgb(255, 127, 255)));
                    r.reset_base = 2;
                    g1_res[str] = r;
                    layer = new g1_layer("洋红色水晶", r);
                    layer.prev(g1_layers["红色水晶"]);
                    layer.prev(g1_layers["蓝色水晶"]);
                    g1_layers[layer.name] = layer;

                    layer.unlocked = true;
                    layer.prepare("vm_g1_map_grid", new Point(400, 675),
                        /* text */A(255, 0, 255), "M", "Consolas", 0.75,
                        /* fill */A(100, 200, 100), 100,
                        /* line */A(255, 0, 255), 5,
                        /*stroke*/A(255, 0, 255), 2);
                    #endregion layer 洋红色水晶
                    #region layer 青色水晶
                    str = "青色水晶";
                    r = new g1_resource(2, str,
                        getSCB(Color.FromRgb(127, 255, 255)));
                    r.reset_base = 2;
                    g1_res[str] = r;
                    layer = new g1_layer("青色水晶", r);
                    layer.prev(g1_layers["绿色水晶"]);
                    layer.prev(g1_layers["蓝色水晶"]);
                    g1_layers[layer.name] = layer;

                    layer.unlocked = true;
                    layer.prepare("vm_g1_map_grid", new Point(600, 675),
                        /* text */A(0, 255, 255), "C", "Consolas", 0.75,
                        /* fill */A(255, 100, 100), 100,
                        /* line */A(0, 255, 255), 5,
                        /*stroke*/A(0, 255, 255), 2);
                    #endregion layer 青色水晶
                    #region layer 白色水晶
                    str = "白色水晶";
                    r = new g1_resource(2, str,
                        getSCB(Color.FromRgb(255, 255, 255)));
                    r.reset_base = 2;
                    g1_res[str] = r;
                    layer = new g1_layer("白色水晶", r);
                    layer.prev(g1_layers["黄色水晶"]);
                    layer.prev(g1_layers["洋红色水晶"]);
                    layer.prev(g1_layers["青色水晶"]);
                    g1_layers[layer.name] = layer;

                    layer.unlocked = true;
                    layer.prepare("vm_g1_map_grid", new Point(400, 900),
                        /* text */A(255, 255, 255), "W", "Consolas", 0.75,
                        /* fill */A(100, 100, 100), 100,
                        /* line */A(255, 255, 255), 5,
                        /*stroke*/A(255, 255, 255), 2);
                    #endregion layer 青色水晶
                }

                //TODO 难度区分

                #region layer 水晶树 水晶聚合处
                layer = level.find_layer("水晶聚合处");
                #region tab 水晶聚合处_主页
                str = "水晶聚合处_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(150, 150, 150)), new solid_type(A(255, 255, 255)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                u = new g1_upgrade("超大水晶切换", level, layer);
                #region 水晶树 - A - 超大水晶切换
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("点击可切换", 255, 255, 255);
                rt.add("艺术品：超大水晶", 255, 150, 255);
                rt.add("的目标", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 90, 0, 0),
                    300, 90, A(175, 50, 50, 50), A(150, 150, 150),
                    A(255, 255, 255), 14, "超大水晶切换",
                    rt,
                    A(180, 180, 180), 12, "目前：×1 红色水晶获取",
                    null, 12);
                u.set_weight(2, 3, 2, 0);
                #endregion 水晶树 - A - 超大水晶切换
                #endregion tab 水晶聚合处_主页
                #region tab 水晶聚合处_升级
                str = "水晶聚合处_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(150, 150, 150)), new solid_type(A(255, 255, 255)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                u = new g1_upgrade("不只是艺术品", level, layer);
                #region 水晶树 - A - 不只是艺术品
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("红色水晶", 10)),
                    new Tuple<string, double2>("绿色水晶", 10)),
                    new Tuple<string, double2>("蓝色水晶", 10));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("解锁新的选项：", 255, 255, 255);
                rt.add("艺术品", 255, 255, 0);
                rt.add("。", 255, 255, 255);
                rt.add("艺术品", 0, 255, 255);
                rt.add("可以带来很多强大的加成", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 90, 0, 0),
                    300, 90, A(175, 0, 0, 0), A(75, 75, 75),
                    A(255, 255, 255), 14, "不只是艺术品",
                    rt,
                    null, 12, "",
                    A(180, 180, 180), 12);
                u.set_weight(2, 3, 0, 2);
                #endregion 水晶树 - A - 不只是艺术品


                u = new g1_upgrade("平淡升级", level, layer);
                #region 水晶树 - A - 平淡升级
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 10e6));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("将", 255, 255, 255);
                rt.add(find_resource("水晶块"));
                rt.add("的获取", 255, 255, 255);
                rt.add("翻倍", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 180, 0, 0),
                    300, 90, A(175, 0, 0, 0), A(75, 75, 75),
                    A(255, 255, 255), 14, "平淡升级",
                    rt,
                    null, 12, "",
                    A(180, 180, 180), 12);
                u.set_weight(2, 3, 0, 2);
                #endregion 水晶树 - A - 平淡升级

                u = new g1_upgrade("有趣的艺术品", level, layer);
                #region 水晶树 - A - 有趣的艺术品
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_不只是艺术品"], 1);
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 602e21));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("解锁第2页的艺术品", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 270, 0, 0),
                    300, 90, A(175, 0, 0, 0), A(75, 75, 75),
                    A(255, 255, 255), 14, "有趣的艺术品",
                    rt,
                    null, 12, "",
                    A(180, 180, 180), 12);
                u.set_weight(2, 3, 0, 2);
                #endregion 水晶树 - A - 有趣的艺术品
                #endregion tab 水晶聚合处_升级
                #region tab 水晶聚合处_公式
                str = "水晶聚合处_公式";
                tab = new g1_tab(str, "", 75, 30, new thickness(210, 410, 0, 0),
                    new solid_type(A(150, 150, 150)), new solid_type(A(255, 255, 255)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("公式", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                if (level.difficulty == g1_level.type.easy)
                {
                    #region drawable 水晶树_公式 easy
                    draw = new g1_drawable("水晶树_公式e", level, layer, tab, 1,
                        "vm_g1_layer_" + layer.name + "_" + tab.name + "_1_grid",
                        HorizontalAlignment.Center, VerticalAlignment.Top, 360, 270,
                        new thickness(0, 90, 0, 0));

                    draw.setFill(new solid_type(A(180, 120, 120, 120)));
                    draw.setStroke(new solid_type(A(255, 255, 255)), 2);
                    draw.addGrid(new grid("水晶树_公式_egrid", "", 360, 270,
                        new thickness(0, 0, 0, 0), Visibility.Visible));
                    draw.addTextblock(new textblock("水晶树_公式_et1", true, "水晶树_公式_egrid",
                        "聚合水晶块的速度公式：", A(200, 200, 200), double.NaN, double.NaN, 15,
                        new thickness(15, 6, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et2", true, "水晶树_公式_egrid",
                        "P = 3 * 10 ^ (GAVG(L(R), L(G), L(B)))", A(255, 255, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 24, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et3", true, "水晶树_公式_egrid",
                        "其中：", A(200, 200, 200),
                        double.NaN, double.NaN, 15, new thickness(15, 42, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et4", true, "水晶树_公式_egrid",
                        "P：每秒生成的水晶块数", A(255, 255, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 60, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et5", true, "水晶树_公式_egrid",
                        "R：红色水晶数", A(255, 175, 175),
                        double.NaN, double.NaN, 14, new thickness(25, 76, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et6", true, "水晶树_公式_egrid",
                        "G：绿色水晶数", A(175, 255, 175),
                        double.NaN, double.NaN, 14, new thickness(25, 92, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et7", true, "水晶树_公式_egrid",
                        "B：蓝色水晶数", A(175, 175, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 108, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et8", true, "水晶树_公式_egrid",
                        "L(x) = lg(MAX(x, 1))，lg是以10为底的对数运算", A(255, 127, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 124, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et9", true, "水晶树_公式_egrid",
                        "GAVG(a, b, c) = (abc) ^ (1 / 3)",
                        A(255, 255, 0), double.NaN, double.NaN, 14, new thickness(25, 140, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et10", true, "水晶树_公式_egrid",
                        "GAVG的运算结果是多个数的几何平均数",
                        A(0, 255, 255), double.NaN, double.NaN, 14, new thickness(25, 156, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_et11", true, "水晶树_公式_egrid",
                        "总结：三种颜色水晶的数量级最好相近，\n" +
                        "          极端的资源分配会导致被几何平均减弱效果",
                        A(255, 255, 255), double.NaN, double.NaN, 14, new thickness(15, 172, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_etr", true, "水晶树_公式_egrid",
                        "+1 红色水晶 带来的效果： +1 水晶块/s",
                        A(255, 175, 175), double.NaN, double.NaN, 13, new thickness(15, 207, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_etg", true, "水晶树_公式_egrid",
                        "+1 绿色水晶 带来的效果： +1 水晶块/s",
                        A(175, 255, 175), double.NaN, double.NaN, 13, new thickness(15, 226, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_etb", true, "水晶树_公式_egrid",
                        "+1 蓝色水晶 带来的效果： +1 水晶块/s",
                        A(175, 175, 255), double.NaN, double.NaN, 13, new thickness(15, 245, 0, 0)));
                    #endregion drawable 水晶树_公式 easy

                }
                else
                {
                    #region drawable 水晶树_公式 normal 1
                    draw = new g1_drawable("水晶树_公式n1", level, layer, tab, 1,
                        "vm_g1_layer_" + layer.name + "_" + tab.name + "_1_grid",
                        HorizontalAlignment.Center, VerticalAlignment.Top, 360, 270,
                        new thickness(0, 90, 0, 0));

                    draw.setFill(new solid_type(A(180, 120, 120, 120)));
                    draw.setStroke(new solid_type(A(255, 255, 255)), 2);
                    draw.addGrid(new grid("水晶树_公式_n1grid", "", 360, 270,
                        new thickness(0, 0, 0, 0), Visibility.Visible));
                    draw.addTextblock(new textblock("水晶树_公式_n1t1", true, "水晶树_公式_n1grid",
                        "聚合水晶块的速度公式：", A(200, 200, 200), double.NaN, double.NaN, 15,
                        new thickness(15, 6, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t2", true, "水晶树_公式_n1grid",
                        "P = 7 * 10 ^ (GAVG(L(R), L(G), L(B), L(Y), L(M), L(C), L(W)))", A(255, 255, 255),
                        double.NaN, double.NaN, 12, new thickness(25, 24, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t3", true, "水晶树_公式_n1grid",
                        "其中：", A(200, 200, 200),
                        double.NaN, double.NaN, 15, new thickness(15, 40, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t4", true, "水晶树_公式_n1grid",
                        "P：每秒生成的水晶块数", A(255, 255, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 56, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t5", true, "水晶树_公式_n1grid",
                        "R：红色水晶数", A(255, 175, 175),
                        double.NaN, double.NaN, 14, new thickness(25, 72, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t6", true, "水晶树_公式_n1grid",
                        "G：绿色水晶数", A(175, 255, 175),
                        double.NaN, double.NaN, 14, new thickness(25, 88, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t7", true, "水晶树_公式_n1grid",
                        "B：蓝色水晶数", A(175, 175, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 104, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t8", true, "水晶树_公式_n1grid",
                        "Y：黄色水晶数", A(255, 255, 175),
                        double.NaN, double.NaN, 14, new thickness(25, 120, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t9", true, "水晶树_公式_n1grid",
                        "M：洋红色水晶数", A(255, 175, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 136, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t10", true, "水晶树_公式_n1grid",
                        "C：青色水晶数", A(175, 255, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 152, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t11", true, "水晶树_公式_n1grid",
                        "W：白色水晶数", A(255, 255, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 168, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t12", true, "水晶树_公式_n1grid",
                        "L(x) = lg(MAX(x, 1))，lg是以10为底的对数运算", A(255, 127, 255),
                        double.NaN, double.NaN, 14, new thickness(25, 184, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t13", true, "水晶树_公式_n1grid",
                        "GAVG(a, b, c, d, e, f, g) = (abcdefg) ^ (1 / 7)",
                        A(255, 255, 0), double.NaN, double.NaN, 14, new thickness(25, 200, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t14", true, "水晶树_公式_n1grid",
                        "GAVG的运算结果是多个数的几何平均数",
                        A(0, 255, 255), double.NaN, double.NaN, 14, new thickness(25, 216, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n1t15", true, "水晶树_公式_n1grid",
                        "总结：七种颜色水晶的数量级最好相近，\n" +
                        "          极端的资源分配会导致被几何平均减弱效果",
                        A(255, 255, 255), double.NaN, double.NaN, 14, new thickness(15, 232, 0, 0)));


                    #endregion drawable 水晶树_公式 normal 1
                    #region drawable 水晶树_公式 normal 2
                    draw = new g1_drawable("水晶树_公式n2", level, layer, tab, 2,
                        "vm_g1_layer_" + layer.name + "_" + tab.name + "_2_grid",
                        HorizontalAlignment.Center, VerticalAlignment.Top, 360, 130,
                        new thickness(0, 90, 0, 0));

                    draw.setFill(new solid_type(A(180, 120, 120, 120)));
                    draw.setStroke(new solid_type(A(255, 255, 255)), 2);
                    draw.addGrid(new grid("水晶树_公式_n2grid", "", double.NaN, double.NaN,
                        new thickness(0, 0, 0, 0), Visibility.Visible));
                    draw.addTextblock(new textblock("水晶树_公式_n2tr", true, "水晶树_公式_n2grid",
                        "+1 红色水晶 带来的效果： +1 水晶块/s",
                        A(255, 175, 175), double.NaN, double.NaN, 13, new thickness(15, 6, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n2tg", true, "水晶树_公式_n2grid",
                        "+1 绿色水晶 带来的效果： +1 水晶块/s",
                        A(175, 255, 175), double.NaN, double.NaN, 13, new thickness(15, 23, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n2tb", true, "水晶树_公式_n2grid",
                        "+1 蓝色水晶 带来的效果： +1 水晶块/s",
                        A(175, 175, 255), double.NaN, double.NaN, 13, new thickness(15, 40, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n2ty", true, "水晶树_公式_n2grid",
                        "+1 黄色水晶 带来的效果： +1 水晶块/s",
                        A(255, 255, 175), double.NaN, double.NaN, 13, new thickness(15, 57, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n2tm", true, "水晶树_公式_n2grid",
                        "+1 洋红色水晶 带来的效果： +1 水晶块/s",
                        A(255, 175, 255), double.NaN, double.NaN, 13, new thickness(15, 74, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n2tc", true, "水晶树_公式_n2grid",
                        "+1 青色水晶 带来的效果： +1 水晶块/s",
                        A(175, 255, 255), double.NaN, double.NaN, 13, new thickness(15, 91, 0, 0)));
                    draw.addTextblock(new textblock("水晶树_公式_n2tw", true, "水晶树_公式_n2grid",
                        "+1 白色水晶 带来的效果： +1 水晶块/s",
                        A(255, 255, 255), double.NaN, double.NaN, 13, new thickness(15, 108, 0, 0)));

                    #endregion drawable 水晶树_公式 normal 2
                }

                #endregion tab 水晶聚合处_公式
                #region tab 水晶聚合处_艺术品
                str = "水晶聚合处_艺术品";
                tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                    new solid_type(A(150, 150, 150)), new solid_type(A(255, 255, 255)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("艺术品", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), false);
                layer.tabs.Add(tab.name, tab);

                #region drawable 水晶树_艺术品
                draw = new g1_drawable("水晶树_艺术品", level, layer, tab, 1,
                    "",
                    HorizontalAlignment.Center, VerticalAlignment.Top, 350, 40,
                    new thickness(0, 80, 0, 0));
                draw.attaching = true;

                draw.setFill(new solid_type(A(180, 120, 120, 120)));
                draw.setStroke(new solid_type(A(255, 255, 255)), 2);
                draw.addGrid(new grid("水晶树_艺术品_grid", "", 350, 40,
                    new thickness(0, 0, 0, 0), Visibility.Visible));
                draw.addTextblock(new textblock("水晶树_艺术品_t1", true, "水晶树_艺术品_grid",
                    "提示：制作任意艺术品都将重置所有水晶层，\n", A(255, 255, 255), double.NaN, double.NaN, 12,
                    new thickness(0, 5, 0, 0), HorizontalAlignment.Center));
                draw.addTextblock(new textblock("水晶树_艺术品_t2", true, "水晶树_艺术品_grid",
                    "并将所有水晶块转化为艺术品价值", A(255, 255, 255), double.NaN, double.NaN, 12,
                    new thickness(0, 22.5, 0, 0), HorizontalAlignment.Center));
                #endregion drawable 水晶树_艺术品

                u = new g1_upgrade("爱心宝石", level, layer);
                #region 水晶树 - 艺术品 - 爱心宝石
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.special = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("[制作条件：", 255, 255, 0);
                rt.add(find_resource("红色水晶"));
                rt.add("比", 255, 255, 0);
                rt.add(find_resource("绿色水晶"));
                rt.add("和", 255, 255, 0);
                rt.add(find_resource("蓝色水晶"));
                rt.add("多]", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(25, 120, 0, 0),
                    350, 80, A(175, 90, 0, 0), A(255, 0, 0),
                    A(255, 150, 150), 14, "爱心宝石",
                    rt,
                    A(255, 200, 200), 12, "艺术品价值：0 (+123.4B)\n" +
                                          "效果：×1 (×2) 生命转化效果底数",
                    null, 0);
                u.set_weight(3, 2.5, 5, 0);
                crystal.get_from_pool(u.name, double2.max);
                #endregion 水晶树 - 艺术品 - 爱心宝石

                u = new g1_upgrade("坚固叶片", level, layer);
                #region 水晶树 - 艺术品 - 坚固叶片
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.special = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("[制作条件：", 255, 255, 0);
                rt.add(find_resource("绿色水晶"));
                rt.add("比", 255, 255, 0);
                rt.add(find_resource("红色水晶"));
                rt.add("和", 255, 255, 0);
                rt.add(find_resource("蓝色水晶"));
                rt.add("多]", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(25, 200, 0, 0),
                    350, 80, A(175, 0, 70, 0), A(0, 255, 0),
                    A(150, 255, 150), 14, "坚固叶片",
                    rt,
                    A(200, 255, 200), 12, "艺术品价值：0 (+123.4B)\n" +
                                          "效果：×1 (×2) 水晶生成力",
                    null, 0);
                u.set_weight(3, 2.5, 5, 0);
                crystal.get_from_pool(u.name, double2.max);
                #endregion 水晶树 - 艺术品 - 坚固叶片

                u = new g1_upgrade("精致时钟", level, layer);
                #region 水晶树 - 艺术品 - 精致时钟
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.special = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("[制作条件：", 255, 255, 0);
                rt.add(find_resource("蓝色水晶"));
                rt.add("比", 255, 255, 0);
                rt.add(find_resource("红色水晶"));
                rt.add("和", 255, 255, 0);
                rt.add(find_resource("绿色水晶"));
                rt.add("多]", 255, 255, 0);
                rt.add("，根据本次在水晶层探索的总时间获取额外的", 255, 255, 255);
                rt.add("水晶块获取速度", 240, 240, 240);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(25, 280, 0, 0),
                    350, 80, A(175, 0, 0, 90), A(0, 0, 255),
                    A(150, 150, 255), 14, "精致时钟",
                    rt,
                    A(200, 200, 255), 12, "艺术品价值：0 (+123.4B)\n" +
                                          "效果：×1 (×2) 水晶块获取速度",
                    null, 0);
                u.set_weight(3, 4.5, 5, 0);
                crystal.get_from_pool(u.name, double2.max);
                #endregion 水晶树 - 艺术品 - 精致时钟

                u = new g1_upgrade("透亮圆盘", level, layer);
                #region 水晶树 - 艺术品 - 透亮圆盘
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_有趣的艺术品"], 1);
                u.can_reset = true;
                u.special = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 2);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("[制作条件：在RGB三种水晶中，", 255, 255, 0);
                rt.add("最多的一种", 0, 255, 255);
                rt.add("不超过", 255, 255, 0);
                rt.add("最少的一种", 0, 255, 255);
                rt.add("的", 255, 255, 0);
                rt.add("1.05次方", 0, 255, 0);
                rt.add("]", 255, 255, 0);
                rt.add("，倍增", 255, 255, 255);
                rt.add("最少的水晶", 0, 255, 127);
                rt.add("的获取，但降低", 255, 255, 255);
                rt.add("水晶块获取", 240, 240, 240);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(25, 120, 0, 0),
                    350, 90, A(175, 100, 100, 50), A(255, 255, 140),
                    A(255, 255, 150), 14, "透亮圆盘",
                    rt,
                    A(200, 200, 100), 11, "艺术品价值：0 (+123.4B)[条件为 ^ 1.02]\n" +
                                          "效果：×1(×2)红色水晶获取，/1(/2)水晶块获取",
                    null, 0);
                u.set_weight(2.5, 4.5, 6, 0);
                crystal.get_from_pool(u.name, double2.max);
                #endregion 水晶树 - 艺术品 - 透亮圆盘

                g1_cal_A_2_art2_select = find_resource("红色水晶");
                u = new g1_upgrade("超大水晶", level, layer);
                #region 水晶树 - 艺术品 - 超大水晶
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_有趣的艺术品"], 1);
                u.next(g1_ups["水晶树_超大水晶切换"], 1);
                u.can_reset = true;
                u.special = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 2);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("[制作条件：任意一种水晶的数量的1000倍超过水晶块的数量]", 255, 255, 0);
                rt.add("，可以在主页选择", 255, 255, 255);
                rt.add("任意一种水晶", 127, 255, 0);
                rt.add("进行倍增", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(25, 210, 0, 0),
                    350, 75, A(175, 110, 50, 110), A(255, 150, 255),
                    A(255, 150, 255), 14, "超大水晶",
                    rt,
                    A(200, 100, 200), 12, "艺术品价值：0 (+123.4B)\n" +
                                          "效果：×1(×2)选择的水晶获取",
                    null, 0);
                u.set_weight(3, 4.5, 5.2, 0);
                crystal.get_from_pool(u.name, double2.max);
                #endregion 水晶树 - 艺术品 - 超大水晶

                g1_cal_A_2_art3_condition = 0;
                u = new g1_upgrade("冰糖果冻", level, layer);
                #region 水晶树 - 艺术品 - 冰糖果冻
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_有趣的艺术品"], 1);
                u.can_reset = true;
                u.special = true;
                u.check = false;
                layer.add_upgrade(u, tab.name, 2);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 0));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("[制作条件：距离制作上一个艺术品的实际时间（不受游戏速度影响）不超过120秒]", 255, 255, 0);
                rt.add("，制作任何艺术品时保留一定的水晶块", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(25, 285, 0, 0),
                    350, 75, A(175, 50, 125, 125), A(100, 255, 255),
                    A(127, 255, 255), 14, "冰糖果冻",
                    rt,
                    A(66, 225, 225), 12, "艺术品价值：0 (+123.4B)\n" +
                                        "效果：保留 ^ 0 (^ 0.1)的水晶块",
                    null, 0);
                u.set_weight(3, 4.5, 5.2, 0);
                crystal.get_from_pool(u.name, double2.max);
                #endregion 水晶树 - 艺术品 - 冰糖果冻
                #endregion tab 水晶聚合处_艺术品
                #endregion layer 水晶树 水晶聚合处
                #region layer 水晶树 红色水晶
                layer = level.find_layer("红色水晶");
                #region tab 红色水晶_主页
                str = "红色水晶_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(150, 0, 0)), new solid_type(A(255, 0, 0)),
                    new solid_type(A(0, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 255, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 红色水晶_主页

                #region tab 红色水晶_升级
                str = "红色水晶_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(150, 0, 0)), new solid_type(A(255, 0, 0)),
                    new solid_type(A(0, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 255, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("平静变化", level, layer);
                #region 水晶树 - R - 平静变化
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 25e6));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("若", 255, 255, 255);
                rt.add(find_resource("红色水晶"));
                rt.add("比并列的两种水晶少，使", 255, 255, 255);
                rt.add(find_resource("红色水晶生成力"));
                rt.add("随", 255, 255, 255);
                rt.add(find_resource("红色水晶"));
                rt.add("数量的提升而提升，", 255, 255, 255);
                rt.add("否则使", 0, 255, 255);
                rt.add(find_resource("绿色水晶生成力"));
                rt.add("和", 0, 255, 255);
                rt.add(find_resource("蓝色水晶生成力"));
                rt.add("以极低的倍率被提升", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 45, 0, 0),
                    300, 105, A(175, 90, 0, 0), A(255, 0, 0),
                    A(255, 150, 150), 14, "平静变化",
                    rt,
                    A(255, 200, 200), 12, "目前效果：×1 红色水晶生成力",
                    A(255, 127, 175), 12);
                u.set_weight(3, 6.5, 2, 2);
                #endregion 水晶树 - R - 平静变化

                u = new g1_upgrade("循环转化", level, layer);
                #region 水晶树 - R - 循环转化
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_平静变化"], 1);
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("红色水晶", 25e6));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("将", 255, 255, 255);
                rt.add(find_resource("水晶块"));
                rt.add("数与", 255, 255, 255);
                rt.add(find_resource("红色水晶"));
                rt.add("数的", 255, 255, 255);
                rt.add("1000", 255, 255, 0);
                rt.add("倍进行比较，前者少于后者时倍增", 255, 255, 255);
                rt.add(find_resource("水晶块"));
                rt.add("获取，否则以更低的倍率倍增", 255, 255, 255);
                rt.add(find_resource("红色水晶生成力"));
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 150, 0, 0),
                    300, 105, A(175, 90, 0, 0), A(255, 0, 0),
                    A(255, 150, 150), 14, "循环转化",
                    rt,
                    A(255, 200, 200), 12, "目前效果：×1 红色水晶生成力",
                    A(255, 127, 175), 12);
                u.set_weight(3, 6.5, 2, 2);
                #endregion 水晶树 - R - 循环转化
                #endregion tab 红色水晶_升级
                #endregion layer 水晶树 红色水晶
                #region layer 水晶树 绿色水晶
                layer = level.find_layer("绿色水晶");
                #region tab 绿色水晶_主页
                str = "绿色水晶_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(0, 150, 0)), new solid_type(A(0, 255, 0)),
                    new solid_type(A(0, 0, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 绿色水晶_主页

                #region tab 绿色水晶_升级
                str = "绿色水晶_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(0, 150, 0)), new solid_type(A(0, 255, 0)),
                    new solid_type(A(0, 0, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("吸收与利用", level, layer);
                #region 水晶树 - G - 吸收与利用
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("红色水晶", 2e3)),
                    new Tuple<string, double2>("蓝色水晶", 2e3));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add(find_resource("绿色水晶生成力"));
                rt.add("随", 255, 255, 255);
                rt.add(find_resource("红色水晶"));
                rt.add("和", 255, 255, 255);
                rt.add(find_resource("蓝色水晶"));
                rt.add("数量的提升而提升，", 255, 255, 255);
                rt.add("但", 0, 255, 255);
                rt.add(find_resource("绿色水晶"));
                rt.add("的数量减弱这个效果", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 45, 0, 0),
                    300, 105, A(175, 0, 70, 0), A(0, 255, 0),
                    A(150, 255, 150), 14, "吸收与利用",
                    rt,
                    A(200, 255, 200), 12, "目前效果：×1 绿色水晶生成力",
                    A(175, 255, 0), 12);
                u.set_weight(3, 5, 3, 3);
                #endregion 水晶树 - G - 吸收与利用

                u = new g1_upgrade("生命混合", level, layer);
                #region 水晶树 - G - 生命混合
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_吸收与利用"], 1);
                u.can_reset = true;
                u.acc_time = 0.99;
                layer.add_upgrade(u, tab.name, 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 10e9));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add(find_resource("生命力"));
                rt.add("以较弱的效率倍增", 255, 255, 255);
                rt.add(find_resource("水晶块"));
                rt.add("获取。每次购买此升级时还能够复制本层中", 255, 255, 255);
                rt.add("(^ 0.99)", 255, 255, 0);
                rt.add("的", 255, 255, 255);
                rt.add(find_resource("生命力"));
                rt.add("，但下一次购买时的效率", 255, 255, 255);
                rt.add("-1%", 255, 127, 127);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 150, 0, 0),
                    300, 105, A(175, 0, 70, 0), A(0, 255, 0),
                    A(150, 255, 150), 14, "生命混合",
                    rt,
                    A(200, 255, 200), 11, "效果：×1 水晶块获取，下一次购买 +(^ 0.9999)生命力\n" +
                                          "[+12.34Qid 生命力]",
                    A(175, 255, 0), 12);
                u.set_weight(2, 6, 4, 2);
                #endregion 水晶树 - G - 生命混合
                #endregion tab 绿色水晶_升级
                #endregion layer 水晶树 绿色水晶
                #region layer 水晶树 蓝色水晶
                layer = level.find_layer("蓝色水晶");
                #region tab 蓝色水晶_主页
                str = "蓝色水晶_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(0, 0, 150)), new solid_type(A(0, 0, 255)),
                    new solid_type(A(255, 255, 0)), 1.5,
                    new solid_text("主页", 14, A(255, 255, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 蓝色水晶_主页

                #region tab 蓝色水晶_升级
                str = "蓝色水晶_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(0, 0, 150)), new solid_type(A(0, 0, 255)),
                    new solid_type(A(255, 255, 0)), 1.5,
                    new solid_text("升级", 14, A(255, 255, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("环境加成", level, layer);
                #region 水晶树 - B - 环境加成
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                layer.add_upgrade(u, tab.name, 1);
                layer.unlock_upgrade(u.name);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水晶块", 100e3));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add(find_resource("蓝色水晶生成力"));
                rt.add("随", 255, 255, 255);
                rt.add(find_resource("蓝色水晶"));
                rt.add("数量的提升而提升", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 45, 0, 0),
                    300, 105, A(175, 0, 0, 90), A(0, 0, 255),
                    A(150, 150, 255), 14, "环境加成",
                    rt,
                    A(200, 200, 255), 12, "目前效果：×1 蓝色水晶生成力",
                    A(0, 175, 255), 12);
                u.set_weight(3, 3, 3, 3);
                #endregion 水晶树 - B - 环境加成

                u = new g1_upgrade("稳定结晶", level, layer);
                #region 水晶树 - B - 稳定结晶
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["水晶树_环境加成"], 1);
                u.can_reset = false;
                layer.add_upgrade(u, tab.name, 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("蓝色水晶", 1e9));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add(find_resource("蓝色水晶生成力"));
                rt.add("×3", 255, 255, 0);
                rt.add("，", 255, 255, 255);
                rt.add("此升级不会被其他层重置", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 150, 0, 0),
                    300, 105, A(175, 0, 0, 90), A(0, 0, 255),
                    A(150, 150, 255), 14, "稳定结晶",
                    rt,
                    A(200, 200, 255), 12, "目前效果：×1 蓝色水晶生成力",
                    A(0, 175, 255), 12);
                u.set_weight(3, 3, 3, 3);
                #endregion 水晶树 - B - 稳定结晶
                #endregion tab 蓝色水晶_升级
                #endregion layer 水晶树 蓝色水晶
                if (level.difficulty == g1_level.type.easy)
                {
                    #region layer 水晶树 纯色工厂
                    layer = level.find_layer("纯色工厂");
                    #region tab 纯色工厂_主页
                    str = "纯色工厂_主页";
                    tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                        new solid_type(A(100, 100, 100)), new solid_type(A(0, 0, 0)),
                        new solid_type(A(255, 255, 255)), 1.5,
                        new solid_text("主页", 14, A(225, 225, 225), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);

                    u = new g1_upgrade("粉碎红色水晶", level, layer);
                    #region 水晶树 - F - 粉碎红色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("红色水晶", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("粉碎", 255, 255, 255);
                    rt.add("50%", 255, 255, 0);
                    rt.add("的", 255, 255, 255);
                    rt.add(find_resource("红色水晶"));
                    rt.add("并获得等量的", 255, 255, 255);
                    rt.add(find_resource("红色水晶原料"));
                    rt.add("，此过程还将摧毁", 255, 255, 255);
                    rt.add("50%", 255, 255, 0);
                    rt.add("的", 255, 255, 255);
                    rt.add(find_resource("水晶块"));
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 90, 0, 0),
                        300, 90, A(175, 90, 0, 0), A(255, 75, 75),
                        A(255, 150, 150), 14, "粉碎红色水晶",
                        rt,
                        A(255, 180, 180), 12, "将粉碎 0 红色水晶，摧毁 0 水晶块",
                        null, 12);
                    u.set_weight(3, 5, 3, 0);
                    #endregion 水晶树 - F - 粉碎红色水晶

                    u = new g1_upgrade("粉碎绿色水晶", level, layer);
                    #region 水晶树 - F - 粉碎绿色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("绿色水晶", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("粉碎", 255, 255, 255);
                    rt.add("50%", 255, 255, 0);
                    rt.add("的", 255, 255, 255);
                    rt.add(find_resource("绿色水晶"));
                    rt.add("并获得等量的", 255, 255, 255);
                    rt.add(find_resource("绿色水晶原料"));
                    rt.add("，此过程还将摧毁", 255, 255, 255);
                    rt.add("50%", 255, 255, 0);
                    rt.add("的", 255, 255, 255);
                    rt.add(find_resource("水晶块"));
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 180, 0, 0),
                        300, 90, A(175, 0, 70, 0), A(75, 255, 75),
                        A(150, 255, 150), 14, "粉碎绿色水晶",
                        rt,
                        A(180, 255, 180), 12, "将粉碎 0 绿色水晶，摧毁 0 水晶块",
                        null, 12);
                    u.set_weight(3, 5, 3, 0);
                    #endregion 水晶树 - F - 粉碎绿色水晶

                    u = new g1_upgrade("粉碎蓝色水晶", level, layer);
                    #region 水晶树 - F - 粉碎蓝色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("蓝色水晶", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("粉碎", 255, 255, 255);
                    rt.add("50%", 255, 255, 0);
                    rt.add("的", 255, 255, 255);
                    rt.add(find_resource("蓝色水晶"));
                    rt.add("并获得等量的", 255, 255, 255);
                    rt.add(find_resource("蓝色水晶原料"));
                    rt.add("，此过程还将摧毁", 255, 255, 255);
                    rt.add("50%", 255, 255, 0);
                    rt.add("的", 255, 255, 255);
                    rt.add(find_resource("水晶块"));
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 270, 0, 0),
                        300, 90, A(175, 0, 0, 90), A(75, 75, 255),
                        A(150, 150, 255), 14, "粉碎蓝色水晶",
                        rt,
                        A(180, 180, 255), 12, "将粉碎 0 蓝色水晶，摧毁 0 水晶块",
                        null, 12);
                    u.set_weight(3, 5, 3, 0);
                    #endregion 水晶树 - F - 粉碎蓝色水晶
                    #endregion tab 纯色工厂_主页


                    #region tab 纯色工厂_升级
                    str = "纯色工厂_升级";
                    tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                        new solid_type(A(100, 100, 100)), new solid_type(A(0, 0, 0)),
                        new solid_type(A(255, 255, 255)), 1.5,
                        new solid_text("升级", 14, A(225, 225, 225), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    layer.curr_tab = tab;

                    u = new g1_upgrade("粉碎机", level, layer);
                    #region 水晶树 - F - 粉碎机
                    g1_ups[u.store_name()] = u;
                    u.next(g1_ups["水晶树_粉碎红色水晶"], 1);
                    u.next(g1_ups["水晶树_粉碎绿色水晶"], 1);
                    u.next(g1_ups["水晶树_粉碎蓝色水晶"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 1.5e9));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("在", 255, 255, 255);
                    rt.add("主页", 255, 255, 0);
                    rt.add("解锁粉碎水晶的能力，", 255, 255, 255);
                    rt.add("可以通过这种方式丢弃不想要的水晶，并获得原料", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 80, 0, 0),
                        300, 70, A(175, 0, 0, 0), A(75, 75, 75),
                        A(255, 255, 255), 14, "粉碎机",
                        rt,
                        null, 12, "",
                        A(180, 180, 180), 12);
                    u.set_weight(2, 3, 0, 2);
                    #endregion 水晶树 - F - 粉碎机

                    u = new g1_upgrade("红水晶镐", level, layer);
                    #region 水晶树 - F - 红水晶镐
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_粉碎机"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("红色水晶原料", 1e6));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("获得一把不会损坏的镐，倍增", 255, 255, 255);
                    rt.add("水晶生成力", 0, 255, 255);
                    rt.add("，", 255, 255, 255);
                    rt.add("但它的效果随时间下降", 255, 127, 127);
                    rt.add("，", 255, 255, 255);
                    rt.add("重复制作来刷新并略微增强效果", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                         u.page + "_grid", HorizontalAlignment.Left,
                         VerticalAlignment.Top, new thickness(50, 150, 0, 0),
                         300, 70, A(175, 90, 0, 0), A(255, 75, 75),
                         A(255, 150, 150), 14, "红水晶镐",
                         rt,
                         A(255, 180, 180), 12, "效果：[×1 / ×100] 水晶生成力",
                         A(255, 200, 200), 12);
                    u.set_weight(2, 4, 2, 2);
                    #endregion 水晶树 - F - 红水晶镐

                    u = new g1_upgrade("绿水晶探测器", level, layer);
                    #region 水晶树 - F - 绿水晶探测器
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_粉碎机"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("绿色水晶原料", 1e6));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("获得探测器，倍增", 255, 255, 255);
                    rt.add("水晶生成力", 0, 255, 255);
                    rt.add("，", 255, 255, 255);
                    rt.add("但它的效果随", 127, 255, 127);
                    rt.add(find_resource("水晶块"));
                    rt.add("数的增加而下降", 127, 255, 127);
                    rt.add("，", 255, 255, 255);
                    rt.add("多次重复制作可略微增强效果", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                         u.page + "_grid", HorizontalAlignment.Left,
                         VerticalAlignment.Top, new thickness(50, 220, 0, 0),
                         300, 70, A(175, 0, 70, 0), A(75, 255, 75),
                         A(150, 255, 150), 14, "绿水晶探测器",
                         rt,
                         A(180, 255, 180), 12, "效果：[×1 / ×100] 水晶生成力",
                         A(200, 255, 200), 12);
                    u.set_weight(2, 4, 2, 2);
                    crystal.get_from_pool(u.name, double2.max);
                    #endregion 水晶树 - F - 绿水晶探测器

                    u = new g1_upgrade("蓝水晶转盘", level, layer);
                    #region 水晶树 - F - 蓝水晶转盘
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_粉碎机"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("蓝色水晶原料", 1e6));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("每次制作转盘都转动它！根据其百分比数值倍增", 255, 255, 255);
                    rt.add("水晶生成力", 0, 255, 255);
                    rt.add("，", 255, 255, 255);
                    rt.add("多次重复制作可略微增强效果", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                         u.page + "_grid", HorizontalAlignment.Left,
                         VerticalAlignment.Top, new thickness(50, 290, 0, 0),
                         300, 70, A(175, 0, 0, 90), A(75, 75, 255),
                         A(150, 150, 255), 14, "蓝水晶转盘",
                         rt,
                         A(180, 180, 255), 12, "效果：[×1 / ×100] 水晶生成力",
                         A(200, 200, 255), 12);
                    u.set_weight(2, 4, 2, 2);
                    #endregion 水晶树 - F - 蓝水晶转盘


                    u = new g1_upgrade("红色斧头", level, layer);
                    #region 水晶树 - F - 红色斧头
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_红水晶镐"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 2);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("红色水晶原料", 1e30));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("获得一把有魔力的斧头，倍增", 255, 255, 255);
                    rt.add("艺术品价值", 0, 255, 255);
                    rt.add("，", 255, 255, 255);
                    rt.add("但它的效果随时间缓慢下降", 255, 127, 127);
                    rt.add("，", 255, 255, 255);
                    rt.add("重复制作来刷新并略微增强效果", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                         u.page + "_grid", HorizontalAlignment.Left,
                         VerticalAlignment.Top, new thickness(50, 90, 0, 0),
                         300, 90, A(175, 90, 0, 0), A(255, 75, 75),
                         A(255, 150, 150), 14, "红色斧头",
                         rt,
                         A(255, 180, 180), 12, "效果：[×1 / ×1205] 艺术品价值",
                         A(255, 200, 200), 12);
                    u.set_weight(2, 5, 2, 2);
                    #endregion 水晶树 - F - 红色斧头


                    u = new g1_upgrade("水晶回收器", level, layer);
                    #region 水晶树 - F - 水晶回收器
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_绿水晶探测器"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 2);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("绿色水晶原料", 1e30));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("获得水晶回收器，倍增", 255, 255, 255);
                    rt.add("水晶块获取", 0, 255, 255);
                    rt.add("，", 255, 255, 255);
                    rt.add("但它的效果随", 127, 255, 127);
                    rt.add("水晶", 0, 255, 0);
                    rt.add("与", 127, 255, 127);
                    rt.add("水晶原料", 0, 255, 0);
                    rt.add("之比值的增加而下降", 127, 255, 127);
                    rt.add("，", 255, 255, 255);
                    rt.add("多次重复制作可略微增强效果", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                         u.page + "_grid", HorizontalAlignment.Left,
                         VerticalAlignment.Top, new thickness(50, 180, 0, 0),
                         300, 90, A(175, 0, 70, 0), A(75, 255, 75),
                         A(150, 255, 150), 14, "水晶回收器",
                         rt,
                         A(180, 255, 180), 12, "效果：[×1 / ×666] 水晶块获取",
                         A(200, 255, 200), 12);
                    u.set_weight(2, 5, 2, 2);
                    #endregion 水晶树 - F - 水晶回收器


                    u = new g1_upgrade("海浪宝石", level, layer);
                    #region 水晶树 - F - 海浪宝石
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_蓝水晶转盘"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 2);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("蓝色水晶原料", 1e30));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("周期性倍增", 255, 255, 255);
                    rt.add("时间流速（仅在水晶树有效）", 0, 255, 255);
                    rt.add("，", 255, 255, 255);
                    rt.add("多次重复制作", 0, 255, 255);
                    rt.add("（或制作转盘）", 127, 127, 255);
                    rt.add("可重置周期，并略微增强效果", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                         u.page + "_grid", HorizontalAlignment.Left,
                         VerticalAlignment.Top, new thickness(50, 270, 0, 0),
                         300, 90, A(175, 0, 0, 90), A(75, 75, 255),
                         A(150, 150, 255), 14, "海浪宝石",
                         rt,
                         A(180, 180, 255), 12, "效果：[×1 / ×100] 时间流速",
                         A(200, 200, 255), 12);
                    u.set_weight(2, 4, 2, 2);
                    #endregion 水晶树 - F - 海浪宝石
                    //制作工具
                    #endregion tab 纯色工厂_升级
                    #endregion layer 水晶树 纯色工厂
                }
                else
                {
                    #region layer 水晶树 黄色水晶
                    layer = level.find_layer("黄色水晶");
                    #region tab 黄色水晶_主页
                    str = "黄色水晶_主页";
                    tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                        new solid_type(A(175, 175, 50)), new solid_type(A(255, 255, 100)),
                        new solid_type(A(0, 0, 255)), 1.5,
                        new solid_text("主页", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    layer.curr_tab = tab;

                    u = new g1_upgrade("转化黄色水晶", level, layer);
                    #region 水晶树 - Y - 转化黄色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("将所有", 255, 255, 255);
                    rt.add(find_resource("红色水晶"));
                    rt.add("和", 255, 255, 255);
                    rt.add(find_resource("绿色水晶"));
                    rt.add("转化为", 255, 255, 255);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("", 255, 255, 255);
                    rt.add("转化公式：", 255, 255, 0);
                    rt.add("3", 200, 200, 0);
                    rt.add(" * ", 255, 255, 255);
                    rt.add("10", 200, 200, 0);
                    rt.add(" ^ ", 255, 255, 255);
                    rt.add("AAVG(", 255, 255, 0);
                    rt.add("L(R)", 255, 127, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(G)", 127, 255, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("0", 127, 127, 255);
                    rt.add(")", 255, 255, 0);
                    rt.add("", 255, 255, 255);
                    rt.add("AAVG为算术平均数函数", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 260, 0, 0),
                        300, 100, A(175, 90, 90, 0), A(255, 255, 75),
                        A(255, 255, 150), 14, "转化黄色水晶",
                        rt,
                        A(255, 255, 180), 12, "将获取 0 黄色水晶",
                        null, 12);
                    u.set_weight(3, 5, 3, 0);
                    #endregion 水晶树 - Y - 转化黄色水晶

                    u = new g1_upgrade("黄色水晶控制", level, layer);
                    #region 水晶树 - Y - 黄色水晶控制
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("控制黄色水晶的效果，点击可切换是否有效", 255, 255, 0);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 140, 0, 0),
                        300, 100, A(175, 90, 90, 0), A(255, 255, 75),
                        A(255, 255, 150), 14, "黄色水晶控制",
                        rt,
                        A(255, 255, 180), 12, "效果： ×1 红色水晶获取（有效）",
                        null, 12);
                    u.set_weight(3, 3, 3, 0);
                    #endregion 水晶树 - Y - 黄色水晶控制
                    #endregion tab 黄色水晶_主页
                    #region tab 黄色水晶_升级
                    str = "黄色水晶_升级";
                    tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                        new solid_type(A(175, 175, 50)), new solid_type(A(255, 255, 100)),
                        new solid_type(A(0, 0, 255)), 1.5,
                        new solid_text("升级", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);

                    u = new g1_upgrade("黄色水晶释放", level, layer);
                    #region 水晶树 - Y - 黄色水晶释放
                    g1_ups[u.store_name()] = u;
                    u.next(g1_ups["水晶树_黄色水晶控制"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 100e6));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("释放", 255, 255, 255);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("的能力，使它能够倍增", 255, 255, 255);
                    rt.add("最多的水晶", 0, 255, 255);
                    rt.add("的获取，", 255, 255, 255);
                    rt.add("在", 255, 255, 255);
                    rt.add("主页", 255, 255, 0);
                    rt.add("可以关闭此功能", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                        300, 120, A(175, 90, 90, 0), A(255, 255, 75),
                        A(255, 255, 150), 14, "黄色水晶释放",
                        rt,
                        null, 12, "",
                        A(255, 255, 200), 12);
                    u.set_weight(3, 5, 0, 3);
                    #endregion 水晶树 - Y - 黄色水晶释放

                    u = new g1_upgrade("黄金比例", level, layer);
                    #region 水晶树 - Y - 黄金比例
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_黄色水晶释放"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("白色水晶", 1e15));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("使", 255, 255, 255);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("的数量不会低于", 255, 255, 255);
                    rt.add("1Qa(1e15)", 255, 255, 0);
                    rt.add("，", 255, 255, 255);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("的效果", 255, 255, 255);
                    rt.add("翻倍", 0, 255, 0);
                    rt.add("，并且以", 255, 255, 255);
                    rt.add("(^ 0.618)", 127, 255, 0);
                    rt.add("的效率倍增", 255, 255, 255);
                    rt.add("其他水晶", 200, 200, 200);
                    rt.add("获取。", 255, 255, 255);
                    rt.add("购买此升级会使", 255, 150, 0);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("和", 255, 150, 0);
                    rt.add(find_resource("青色水晶"));
                    rt.add("的对应升级变贵", 255, 150, 0);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 240, 0, 0),
                        300, 120, A(175, 90, 90, 0), A(255, 255, 75),
                        A(255, 255, 150), 14, "黄金比例",
                        rt,
                        null, 12, "",
                        A(255, 255, 200), 12);
                    u.set_weight(3, 8, 0, 3);
                    #endregion 水晶树 - Y - 黄金比例

                    #endregion tab 黄色水晶_升级
                    #endregion layer 水晶树 黄色水晶
                    #region layer 水晶树 洋红色水晶
                    g1_cal_M_time = 0;

                    layer = level.find_layer("洋红色水晶");
                    #region tab 洋红色水晶_主页
                    str = "洋红色水晶_主页";
                    tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                        new solid_type(A(175, 50, 175)), new solid_type(A(255, 100, 255)),
                        new solid_type(A(0, 200, 0)), 1.5,
                        new solid_text("主页", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    layer.curr_tab = tab;

                    u = new g1_upgrade("转化洋红色水晶", level, layer);
                    #region 水晶树 - M - 转化洋红色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("将所有", 255, 255, 255);
                    rt.add(find_resource("红色水晶"));
                    rt.add("和", 255, 255, 255);
                    rt.add(find_resource("蓝色水晶"));
                    rt.add("转化为", 255, 255, 255);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("", 255, 255, 255);
                    rt.add("转化公式：", 255, 127, 255);
                    rt.add("3", 200, 100, 200);
                    rt.add(" * ", 255, 255, 255);
                    rt.add("10", 200, 100, 200);
                    rt.add(" ^ ", 255, 255, 255);
                    rt.add("AAVG(", 255, 255, 0);
                    rt.add("L(R)", 255, 127, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("0", 127, 255, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(B)", 127, 127, 255);
                    rt.add(")", 255, 255, 0);
                    rt.add("", 255, 255, 255);
                    rt.add("AAVG为算术平均数函数", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 260, 0, 0),
                        300, 100, A(175, 90, 0, 90), A(255, 75, 255),
                        A(255, 150, 255), 14, "转化洋红色水晶",
                        rt,
                        A(255, 180, 255), 12, "将获取 0 洋红色水晶",
                        null, 12);
                    u.set_weight(3, 5, 3, 0);
                    #endregion 水晶树 - M - 转化洋红色水晶

                    #endregion tab 洋红色水晶_主页


                    #region tab 洋红色水晶_升级
                    str = "洋红色水晶_升级";
                    tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                        new solid_type(A(175, 50, 175)), new solid_type(A(255, 100, 255)),
                        new solid_type(A(0, 200, 0)), 1.5,
                        new solid_text("升级", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);

                    u = new g1_upgrade("洋红色水晶释放", level, layer);
                    #region 水晶树 - M - 洋红色水晶释放
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 5e9)),
                        new Tuple<string, double2>("红色水晶", 20e6)),
                        new Tuple<string, double2>("蓝色水晶", 20e6));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("释放", 255, 255, 255);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("的能力，使它能够根据", 255, 255, 255);
                    rt.add("距上一次转化洋红色水晶的时间", 255, 127, 255);
                    rt.add("倍增", 255, 255, 255);
                    rt.add(find_resource("水晶块"));
                    rt.add("获取", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                        300, 120, A(175, 90, 0, 90), A(255, 75, 255),
                        A(255, 150, 255), 14, "洋红色水晶释放",
                        rt,
                        null, 12, "",
                        A(255, 200, 255), 12);
                    u.set_weight(3, 5, 0, 3);
                    #endregion 水晶树 - M - 洋红色水晶释放

                    u = new g1_upgrade("记忆晶石", level, layer);
                    #region 水晶树 - M - 记忆晶石
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_洋红色水晶释放"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("白色水晶", 1e15));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("使", 255, 255, 255);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("的数量不会低于", 255, 255, 255);
                    rt.add("1Qa(1e15)", 255, 255, 0);
                    rt.add("，进行", 255, 255, 255);
                    rt.add("洋红色水晶重置", 255, 0, 255);
                    rt.add("不再重置时间，而会保留", 255, 255, 255);
                    rt.add("80%", 255, 255, 0);
                    rt.add("，并且提升此水晶的效果至", 255, 255, 255);
                    rt.add("(^ 1.8)", 255, 255, 0);
                    rt.add("。", 255, 255, 255);
                    rt.add("购买此升级会使", 255, 150, 0);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("和", 255, 150, 0);
                    rt.add(find_resource("青色水晶"));
                    rt.add("的对应升级变贵", 255, 150, 0);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 240, 0, 0),
                        300, 120, A(175, 90, 0, 90), A(255, 75, 255),
                        A(255, 150, 255), 14, "记忆晶石",
                        rt,
                        null, 12, "",
                        A(255, 200, 255), 12);
                    u.set_weight(3, 8, 0, 3);
                    #endregion 水晶树 - M - 记忆晶石
                    #endregion tab 洋红色水晶_升级
                    #endregion layer 水晶树 洋红色水晶
                    #region layer 水晶树 青色水晶
                    layer = level.find_layer("青色水晶");
                    #region tab 青色水晶_主页
                    str = "青色水晶_主页";
                    tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                        new solid_type(A(50, 175, 175)), new solid_type(A(100, 255, 255)),
                        new solid_type(A(255, 0, 0)), 1.5,
                        new solid_text("主页", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    layer.curr_tab = tab;

                    u = new g1_upgrade("转化青色水晶", level, layer);
                    #region 水晶树 - C - 转化青色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("将所有", 255, 255, 255);
                    rt.add(find_resource("绿色水晶"));
                    rt.add("和", 255, 255, 255);
                    rt.add(find_resource("蓝色水晶"));
                    rt.add("转化为", 255, 255, 255);
                    rt.add(find_resource("青色水晶"));
                    rt.add("", 255, 255, 255);
                    rt.add("转化公式：", 0, 255, 255);
                    rt.add("3", 0, 200, 200);
                    rt.add(" * ", 255, 255, 255);
                    rt.add("10", 0, 200, 200);
                    rt.add(" ^ ", 255, 255, 255);
                    rt.add("AAVG(", 255, 255, 0);
                    rt.add("0", 255, 127, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(G)", 127, 255, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(B)", 127, 127, 255);
                    rt.add(")", 255, 255, 0);
                    rt.add("", 255, 255, 255);
                    rt.add("AAVG为算术平均数函数", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 260, 0, 0),
                        300, 100, A(175, 0, 90, 90), A(0, 255, 255),
                        A(150, 255, 255), 14, "转化青色水晶",
                        rt,
                        A(180, 255, 255), 12, "将获取 0 青色水晶",
                        null, 12);
                    u.set_weight(3, 5, 3, 0);
                    #endregion 水晶树 - C - 转化青色水晶

                    #endregion tab 青色水晶_主页


                    #region tab 青色水晶_升级
                    str = "青色水晶_升级";
                    tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                        new solid_type(A(50, 175, 175)), new solid_type(A(100, 255, 255)),
                        new solid_type(A(255, 0, 0)), 1.5,
                        new solid_text("升级", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);

                    u = new g1_upgrade("青色水晶释放", level, layer);
                    #region 水晶树 - C - 青色水晶释放
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 15e12));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("释放", 255, 255, 255);
                    rt.add(find_resource("青色水晶"));
                    rt.add("的能力，使它极大地倍增", 255, 255, 255);
                    rt.add("所有水晶", 127, 255, 255);
                    rt.add("的获取，但同时以相同的倍率降低", 255, 255, 255);
                    rt.add(find_resource("水晶块"));
                    rt.add("的获取", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                        300, 120, A(175, 0, 90, 90), A(75, 255, 255),
                        A(150, 255, 255), 14, "青色水晶释放",
                        rt,
                        null, 12, "",
                        A(200, 255, 255), 12);
                    u.set_weight(3, 5, 0, 3);
                    #endregion 水晶树 - C - 青色水晶释放

                    u = new g1_upgrade("超级转换", level, layer);
                    #region 水晶树 - C - 超级转换
                    g1_ups[u.store_name()] = u;
                    u.prev(g1_ups["水晶树_青色水晶释放"], 1);
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("白色水晶", 1e15));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("使", 255, 255, 255);
                    rt.add(find_resource("青色水晶"));
                    rt.add("的数量不会低于", 255, 255, 255);
                    rt.add("1Qa(1e15)", 255, 255, 0);
                    rt.add("，", 255, 255, 255);
                    rt.add(find_resource("青色水晶"));
                    rt.add("的负面效果降至", 255, 255, 255);
                    rt.add("(^ 0.5)", 255, 255, 0);
                    rt.add("。", 255, 255, 255);
                    rt.add("购买此升级会使", 255, 150, 0);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("和", 255, 150, 0);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("的对应升级变贵", 255, 150, 0);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 240, 0, 0),
                        300, 120, A(175, 0, 90, 90), A(75, 255, 255),
                        A(150, 255, 255), 14, "超级转换",
                        rt,
                        null, 12, "",
                        A(200, 255, 255), 12);
                    u.set_weight(3, 8, 0, 3);
                    #endregion 水晶树 - C - 超级转换
                    #endregion tab 青色水晶_升级
                    #endregion layer 水晶树 青色水晶
                    #region layer 水晶树 白色水晶
                    layer = level.find_layer("白色水晶");
                    #region tab 白色水晶_主页
                    str = "白色水晶_主页";
                    tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                        new solid_type(A(175, 175, 175)), new solid_type(A(255, 255, 255)),
                        new solid_type(A(0, 0, 0)), 1.5,
                        new solid_text("主页", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);
                    layer.curr_tab = tab;

                    u = new g1_upgrade("转化白色水晶", level, layer);
                    #region 水晶树 - W - 转化白色水晶
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    u.check = false;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("水晶块", 0));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("将所有", 255, 255, 255);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("、", 255, 255, 255);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("和", 255, 255, 255);
                    rt.add(find_resource("青色水晶"));
                    rt.add("转化为", 255, 255, 255);
                    rt.add(find_resource("白色水晶"));
                    rt.add("", 255, 255, 255);
                    rt.add("转化公式：", 255, 255, 255);
                    rt.add("6", 200, 200, 200);
                    rt.add(" * ", 255, 255, 255);
                    rt.add("10", 200, 200, 200);
                    rt.add(" ^ ", 255, 255, 255);
                    rt.add("AAVG(", 255, 255, 0);
                    rt.add("0", 255, 127, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("0", 127, 255, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("0", 127, 127, 255);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(Y)", 255, 255, 127);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(M)", 255, 127, 255);
                    rt.add(", ", 255, 255, 255);
                    rt.add("L(C)", 127, 255, 255);
                    rt.add(")", 255, 255, 0);
                    rt.add("", 255, 255, 255);
                    rt.add("AAVG为算术平均数函数", 255, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 240, 0, 0),
                        300, 120, A(175, 90, 90, 90), A(255, 255, 255),
                        A(255, 255, 255), 14, "转化白色水晶",
                        rt,
                        A(255, 255, 255), 12, "将获取 0 白色水晶",
                        null, 12);
                    u.set_weight(3, 7, 3, 0);
                    #endregion 水晶树 - W - 转化白色水晶

                    #endregion tab 白色水晶_主页


                    #region tab 白色水晶_升级
                    str = "白色水晶_升级";
                    tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                        new solid_type(A(175, 175, 175)), new solid_type(A(255, 255, 255)),
                        new solid_type(A(0, 0, 0)), 1.5,
                        new solid_text("升级", 14, A(25, 25, 25), HorizontalAlignment.Center,
                        VerticalAlignment.Center), true);
                    layer.tabs.Add(tab.name, tab);



                    u = new g1_upgrade("白色水晶释放", level, layer);
                    #region 水晶树 - W - 白色水晶释放
                    g1_ups[u.store_name()] = u;
                    u.can_reset = true;
                    layer.add_upgrade(u, tab.name, 1);
                    layer.unlock_upgrade(u.name);
                    ct = new List<List<Tuple<string, double2>>>();
                    costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                        new Tuple<string, double2>("黄色水晶", 10e9)),
                        new Tuple<string, double2>("洋红色水晶", 10e9)),
                        new Tuple<string, double2>("青色水晶", 10e9));
                    ct.Add(costs);
                    u.set_init_cost(ct, 0, ct.Count);
                    rt = new rainbow_text(u.name);
                    rt.add("释放", 255, 255, 255);
                    rt.add(find_resource("白色水晶"));
                    rt.add("的能力，使它根据", 255, 255, 255);
                    rt.add(find_resource("黄色水晶"));
                    rt.add("、", 255, 255, 255);
                    rt.add(find_resource("洋红色水晶"));
                    rt.add("和", 255, 255, 255);
                    rt.add(find_resource("青色水晶"));
                    rt.add("数量级的方差", 255, 255, 0);
                    rt.add("倍增", 255, 255, 255);
                    rt.add("所有水晶", 200, 200, 200);
                    rt.add("的获取，", 255, 255, 255);
                    rt.add("方差越小效果越好", 0, 255, 255);
                    rt.prepare("",
                        HorizontalAlignment.Center, VerticalAlignment.Center,
                        new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
                    u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                        u.page + "_grid", HorizontalAlignment.Left,
                        VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                        300, 120, A(175, 70, 70, 70), A(255, 255, 255),
                        A(255, 255, 255), 14, "白色水晶释放",
                        rt,
                        null, 12, "",
                        A(200, 200, 200), 12);
                    u.set_weight(3, 5, 0, 3);
                    #endregion 水晶树 - W - 白色水晶释放

                    //TODO 白色水晶：保留YMC升级，微量加速时间流逝
                    #endregion tab 白色水晶_升级
                    #endregion layer 水晶树 白色水晶
                }
                #endregion
            }
        }
    }
}
