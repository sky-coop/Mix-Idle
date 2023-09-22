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
        public void g1_level_draw_自然树(bool success)
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


            g1_map_redraw(1, 3);
            if (success)
            {
                #region level 自然树
                str = "自然点数";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(0, 255, 127)));
                g1_res[str] = r;
                level.clear();
                level.resources.Add(find_resource("生命力"));
                level.resources.Add(r);


                #region layer 种子仓库
                str = "保存的种子";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(238, 238, 179)));
                g1_res[str] = r;
                layer = new g1_layer("种子仓库", r);
                level.roots.Add(layer);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(200, 200),
                    /* text */A(46, 139, 87), "B", "Consolas", 0.75,
                    /* fill */A(220, 220, 220), 100,
                    /* line */A(255, 255, 255), 5,
                    /*stroke*/A(255, 255, 255), 2);
                #endregion
                #region layer 水
                str = "水";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(0, 191, 255)));
                g1_res[str] = r;
                layer = new g1_layer("水", r);
                level.roots.Add(layer);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(600, 200),
                    /* text */A(25, 25, 112), "W", "Consolas", 0.75,
                    /* fill */A(30, 144, 255), 100,
                    /* line */A(0, 255, 255), 5,
                    /*stroke*/A(0, 255, 255), 2);
                #endregion
                #region layer 种子
                str = "种子";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(152, 251, 152)));
                g1_res[str] = r;
                layer = new g1_layer("种子", r);

                //种子成长度
                str = "种子成长度";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(127, 255, 0)));
                g1_res[str] = r;

                layer.resources.Add(r);
                layer.prev(level.find_layer("种子仓库"));
                layer.prev(level.find_layer("水"));
                //level.roots.Add(layer);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(400, 200),
                    /* text */A(184, 134, 11), "S", "Consolas", 0.75,
                    /* fill */A(0, 255, 127), 100,
                    /* line */A(0, 255, 0), 5,
                    /*stroke*/A(0, 255, 0), 2);

                seed = new g1_seed(g1_current_level.difficulty);
                seed.init_reseter();
                #endregion layer 种子
                #region layer 树苗
                str = "树苗";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(192, 255, 62)));
                g1_res[str] = r;
                layer = new g1_layer("树苗", r);

                //树苗成长度
                str = "树苗成长度";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(127, 255, 0)));
                g1_res[str] = r;

                layer.resources.Add(r);
                layer.prev(level.find_layer("种子"));
                g1_layers[layer.name] = layer;

                layer.unlocked = false;
                layer.prepare("vm_g1_map_grid", new Point(400, 400),
                    /* text */A(214, 164, 11), "S2", "Consolas", 0.5,
                    /* fill */A(66, 255, 127), 100,
                    /* line */A(0, 255, 127), 5,
                    /*stroke*/A(0, 255, 127), 2);

                sapling = new g1_sapling(g1_current_level.difficulty);
                sapling.init_reseter();
                #endregion layer 树苗
                #region layer 小树
                str = "小树";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(152, 255, 152)));
                g1_res[str] = r;
                layer = new g1_layer(str, r);

                //树苗成长度
                str = str + "成长度";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(127, 255, 0)));
                g1_res[str] = r;

                layer.resources.Add(r);
                layer.prev(level.find_layer("树苗"));
                g1_layers[layer.name] = layer;

                layer.unlocked = false;
                layer.prepare("vm_g1_map_grid", new Point(400, 600),
                    /* text */A(234, 184, 11), "S3", "Consolas", 0.5,
                    /* fill */A(132, 255, 127), 100,
                    /* line */A(0, 255, 255), 5,
                    /*stroke*/A(0, 255, 255), 2);

                smalltree = new g1_smalltree(g1_current_level.difficulty);
                smalltree.init_reseter();
                #endregion layer 小树
                #region layer 营养
                str = "营养";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 255, 0)));
                g1_res[str] = r;
                layer = new g1_layer("营养", r);
                layer.prev(level.find_layer("水"));
                g1_layers[layer.name] = layer;

                layer.unlocked = false;
                layer.prepare("vm_g1_map_grid", new Point(600, 400),
                    /* text */A(255, 255, 0), "N", "Consolas", 0.75,
                    /* fill */A(0, 191, 255), 100,
                    /* line */A(255, 255, 0), 5,
                    /*stroke*/A(255, 255, 0), 2);
                #endregion

                layer = level.find_layer("种子仓库");
                #region layer 自然树 种子仓库
                #region tab 种子仓库_主页
                str = "种子仓库_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(0, 0, 0)), new solid_type(A(70, 130, 180)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 255, 127), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                #endregion tab 种子仓库_主页



                #region tab 种子仓库_升级
                str = "种子仓库_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(0, 0, 0)), new solid_type(A(70, 130, 180)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 255, 127), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 种子仓库_升级
                #endregion layer 自然树 种子仓库

                layer = level.find_layer("种子");
                #region layer 自然树 种子
                #region tab 种子_主页
                str = "种子_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("播种", level, layer);
                #region 自然树 - 种子 - 播种
                g1_ups[u.store_name()] = u;
                u.no_cost = true;
                u.visitable = true;
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("保存的种子", 1));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "种子_主页", 1);
                rt = new rainbow_text(u.name);
                rt.add("种下", 255, 255, 255);
                rt.add("所有", 255, 255, 0);
                rt.add("保存的种子", 238, 238, 179);
                rt.add("，开始收获", 255, 255, 255);
                rt.add("自然点数", 0, 255, 127);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 14);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Center,
                    VerticalAlignment.Top, new thickness(0, 150, 0, 0),
                    240, 90,
                    A(175, 139, 69, 19),
                    A(255, 165, 79),
                    A(0, 250, 154), 18, "播种",
                    rt,
                    null, 0, "",
                    null, 0);
                u.set_weight(3, 4, 0, 0);
                #endregion
                #endregion tab 种子_主页

                #region tab 种子_升级
                str = "种子_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                u = new g1_upgrade("森林之歌", level, layer);
                #region 自然树 - 种子 - 森林之歌
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子", 2));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "种子_升级", 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add("自然点数", 0, 255, 127);
                rt.add("的产量 ", 255, 255, 255);
                rt.add("×3", 255, 255, 0);
                rt.add("，", 255, 255, 255);
                rt.add("但是什么时候才能买到这个升级呢？", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 140, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "森林之歌",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 自然点数产量",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 3, 3);
                #endregion

                u = new g1_upgrade("浓缩", level, layer);
                #region 自然树 - 种子 - 浓缩
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子成长度", 10));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "种子_升级", 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("使每粒", 255, 255, 255);
                rt.add("种子", 152, 251, 152);
                rt.add("的基础生长进度获取变为", 255, 255, 255);
                rt.add("10/s", 127, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 140, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "浓缩",
                    rt,
                    A(0, 255, 180), 11, "目前效果：+0 生长进度/s",
                    A(255, 175, 0), 12);
                u.set_weight(3, 4, 4, 3);
                #endregion

                u = new g1_upgrade("返老还童术", level, layer);
                #region 自然树 - 种子 - 返老还童术
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_浓缩"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子成长度", 50));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "种子_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("使每粒", 255, 255, 255);
                rt.add("种子", 152, 251, 152);
                rt.add("的基础生长进度获取变为", 255, 255, 255);
                rt.add("25/s", 127, 255, 0);
                rt.add("，并将生长进度获取速度", 255, 255, 255);
                rt.add("翻倍", 127, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 250, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "返老还童术",
                    rt,
                    A(0, 255, 180), 11, "目前效果：+0 生长进度/s",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 5, 2.5);
                #endregion

                u = new g1_upgrade("不想长大", level, layer);
                #region 自然树 - 种子 - 不想长大
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_返老还童术"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子成长度", 200));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "种子_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("使每粒", 255, 255, 255);
                rt.add("种子", 152, 251, 152);
                rt.add("的基础生长进度获取变为", 255, 255, 255);
                rt.add("500/s", 127, 255, 0);
                rt.add("，并将生长进度获取速度", 255, 255, 255);
                rt.add("×1.5", 127, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 250, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "不想长大？",
                    rt,
                    A(0, 255, 180), 11, "目前效果：+0 生长进度/s",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 5, 2.5);
                #endregion

                u = new g1_upgrade("过生长", level, layer);
                #region 自然树 - 种子 - 过生长
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_不想长大"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子成长度", 1000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "种子_升级", 2);
                rt = new rainbow_text(u.name);
                rt.add("你发现了吗？", 0, 255, 255);
                rt.add("“倍增生长进度”的升级对之后的层（如", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("层）", 255, 255, 255);
                rt.add("也有效果，购买此升级将生长进度获取速度再次", 255, 255, 255);
                rt.add("×2", 127, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 140, 0, 0),
                    300, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "过生长！",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 生长速度",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 2, 2.5);
                #endregion

                #endregion tab 种子_升级

                #region tab 种子_里程碑
                str = "种子_里程碑";
                tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("里程碑", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                ms = new g1_milestone("种子_里程碑_1", level, layer);
                #region 种子_里程碑_1
                g1_ups[ms.store_name()] = ms;
                ms.can_reset = false;
                ms.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子成长度", 100));
                ct.Add(costs);
                ms.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(ms, str, 1);
                rt = new rainbow_text(ms.name);
                rt.add("解锁", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("层！", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("层能够生产更多", 255, 255, 255);
                rt.add("自然点数", 0, 255, 127);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
                ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                    300, 60, new thickness(0, 140, 0, 0),
                    new solid_type(A(175, 0, 66, 33)),
                    new solid_type(A(175, 83, 83, 166)),
                    new solid_type(A(0, 255, 127)), 1,
                    new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                        new ARGB(find_resource(ms.get_auto_res()).text_color()),
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new thickness(0, 5, 0, 0)),
                    rt,
                    new solid_type(A(50, 200, 200, 0)));
                #endregion 种子_里程碑_1


                ms = new g1_milestone("种子_里程碑_2", level, layer);
                #region 种子_里程碑_2
                g1_ups[ms.store_name()] = ms;
                ms.can_reset = false;
                ms.prev(g1_ups["自然树_种子_里程碑_1"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子成长度", 10e3));
                ct.Add(costs);
                ms.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(ms, str, 1);
                rt = new rainbow_text(ms.name);
                rt.add("将", 255, 255, 255);
                rt.add("种子", 152, 251, 152);
                rt.add("的基础生长进度获取变为", 255, 255, 255);
                rt.add("(种子成长度 ^ 0.5)倍", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
                ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                    300, 60, new thickness(0, 200, 0, 0),
                    new solid_type(A(175, 0, 66, 33)),
                    new solid_type(A(175, 83, 83, 166)),
                    new solid_type(A(0, 255, 127)), 1,
                    new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                        new ARGB(find_resource(ms.get_auto_res()).text_color()),
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new thickness(0, 5, 0, 0)),
                    rt,
                    new solid_type(A(50, 200, 200, 0)));
                #endregion 种子_里程碑_1
                #endregion tab 种子_里程碑

                seed.exp_bar.prepare("种子经验条", "vm_g1_layer_种子_grid",
                    HorizontalAlignment.Center, VerticalAlignment.Top, 240, 30,
                    new thickness(0, 75, 0, 0), new solid_type(A(238, 233, 233)),
                    new solid_type(A(127, 255, 0)), 0.5, new solid_type(A(255, 250, 205)), 2,
                    new solid_text("进度 0 / 100", 14, A(35, 65, 90),
                    HorizontalAlignment.Center, VerticalAlignment.Center));


                #endregion layer 自然树 种子

                layer = level.find_layer("水");
                #region layer 自然树 水
                #region tab 水_主页
                str = "水_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(25, 25, 112)), new solid_type(A(65, 105, 225)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 191, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 水_主页



                #region tab 水_升级
                str = "水_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(25, 25, 112)), new solid_type(A(65, 105, 225)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 191, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("水源", level, layer);
                #region 自然树 - 水 - 水源
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 60));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "水_升级", 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("随时间产生", 255, 255, 255);
                rt.add("水", 0, 191, 255);
                rt.add("，让树长得更快", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 80, 0, 0),
                    150, 140, A(175, 25, 25, 82), A(30, 114, 255),
                    A(0, 255, 0), 14, "水源",
                    rt,
                    A(0, 255, 180), 11, "目前效果：+0 水/s",
                    A(255, 175, 0), 12);
                u.set_weight(3, 4, 4, 3);
                #endregion

                u = new g1_upgrade("水分保持", level, layer);
                #region 自然树 - 水 - 水分保持
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["自然树_水源"], 1);
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 200));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "水_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("水", 0, 191, 255);
                rt.add("的获取速度受当前的", 255, 255, 255);
                rt.add("自然点数", 0, 255, 127);
                rt.add("提升", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 80, 0, 0),
                    150, 140, A(175, 25, 25, 82), A(30, 114, 255),
                    A(0, 255, 0), 14, "水分保持",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 水获取速度",
                    A(255, 175, 0), 12);
                u.set_weight(3, 4, 4, 3);
                #endregion

                u = new g1_upgrade("汇聚", level, layer);
                #region 自然树 - 水 - 汇聚
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["自然树_水分保持"], 1);
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 500));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "水_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("水", 0, 191, 255);
                rt.add("的获取速度受当前的", 255, 255, 255);
                rt.add("水", 0, 191, 255);
                rt.add("提升", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 220, 0, 0),
                    150, 140, A(175, 25, 25, 82), A(30, 114, 255),
                    A(0, 255, 0), 14, "汇聚",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 水获取速度",
                    A(255, 175, 0), 12);
                u.set_weight(3, 4, 4, 3);
                #endregion

                u = new g1_upgrade("营养液", level, layer);
                #region 自然树 - 水 - 营养液
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["自然树_汇聚"], 1);
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 1000)),
                    new Tuple<string, double2>("水", 2000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "水_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("解锁", 255, 255, 255);
                rt.add("营养", 255, 255, 0);
                rt.add("层！", 255, 255, 255);
                rt.add("并使", 255, 255, 255);
                rt.add("水", 0, 191, 255);
                rt.add("的获取速度受", 255, 255, 255);
                rt.add("所有", 255, 255, 0);
                rt.add("的", 255, 255, 255);
                rt.add("植物成长度", 0, 255, 127);
                rt.add("提升", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 220, 0, 0),
                    150, 140, A(175, 25, 25, 82), A(30, 114, 255),
                    A(0, 255, 0), 14, "营养液",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 水获取速度",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 3, 4);
                #endregion

                u = new g1_upgrade("非常水的升级", level, layer);
                #region 自然树 - 水 - 非常水的升级
                g1_ups[u.store_name()] = u;
                u.prev(g1_ups["自然树_营养液"], 1);
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 100e6)),
                    new Tuple<string, double2>("营养", 5000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, tab.name, 2);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add(find_resource("水"));
                rt.add("的获取速度", 255, 255, 255);
                rt.add("×2", 255, 255, 0);
                rt.add("，并根据", 255, 255, 255);
                rt.add(find_resource("水"));
                rt.add("的获取速度获得额外的", 255, 255, 255);
                rt.add(find_resource("营养"));
                rt.add("[(水获取速度 ^ 0.5)/s]", 0, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 80, 0, 0),
                    150, 140, A(175, 25, 65, 112), A(70, 174, 255),
                    A(0, 255, 0), 14, "非常水的升级",
                    rt,
                    A(0, 255, 180), 11, "效果：×1 水获取速度",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 3, 4);
                #endregion

                #endregion tab 水_升级

                #region tab 水_里程碑
                str = "水_里程碑";
                tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("里程碑", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                ms = new g1_milestone("水_里程碑_1", level, layer);
                #region 水_里程碑_1
                g1_ups[ms.store_name()] = ms;
                ms.can_reset = false;
                ms.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("水", 10000));
                ct.Add(costs);
                ms.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(ms, str, 1);
                rt = new rainbow_text(ms.name);
                rt.add("将", 255, 255, 255);
                rt.add("水", 0, 191, 255);
                rt.add("的效果从 ", 255, 255, 255);
                rt.add("^ 0.5", 255, 255, 0);
                rt.add(" 提升到 ", 255, 255, 255);
                rt.add("^ 0.6", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
                ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                    300, 60, new thickness(0, 140, 0, 0),
                    new solid_type(A(175, 0, 66, 33)),
                    new solid_type(A(175, 83, 83, 166)),
                    new solid_type(A(0, 255, 127)), 1,
                    new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                        new ARGB(find_resource(ms.get_auto_res()).text_color()),
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new thickness(0, 5, 0, 0)),
                    rt,
                    new solid_type(A(50, 200, 200, 0)));
                #endregion 水_里程碑_1
                #endregion tab 水_里程碑

                #endregion layer 自然树 水

                layer = level.find_layer("树苗");
                #region layer 自然树 树苗
                #region tab 树苗_主页
                str = "树苗_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("种子树苗", level, layer);
                #region 自然树 - 树苗 - 种子树苗
                g1_ups[u.store_name()] = u;
                u.no_cost = true;
                u.visitable = true;
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("种子", 1)),
                    new Tuple<string, double2>("种子成长度", 100));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "树苗_主页", 1);
                rt = new rainbow_text(u.name);
                rt.add("需要达到", 0, 255, 255);
                rt.add(" 100 ", 255, 255, 0);
                rt.add("种子成长度", 127, 255, 0);
                rt.add("。", 0, 255, 255);
                rt.add("将", 255, 255, 255);
                rt.add("所有", 255, 255, 0);
                rt.add("种子", 152, 251, 152);
                rt.add("变为", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("，", 255, 255, 255);
                rt.add("你将暂时不能获得更多", 0, 255, 255);
                rt.add("种子成长度", 127, 255, 0);
                rt.add("，但仍能购买", 0, 255, 255);
                rt.add("种子", 152, 251, 152);
                rt.add("层的升级", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 14);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Center,
                    VerticalAlignment.Top, new thickness(0, 150, 0, 0),
                    240, 120,
                    A(175, 139, 69, 19),
                    A(255, 165, 79),
                    A(0, 250, 154), 18, "种子 → 树苗",
                    rt,
                    null, 0, "",
                    null, 0);
                u.set_weight(3, 7, 0, 0);
                #endregion
                #endregion tab 树苗_主页

                #region tab 树苗_升级
                str = "树苗_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                u = new g1_upgrade("光合作用", level, layer);
                #region 自然树 - 树苗 - 光合作用
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("树苗成长度", 10)),
                    new Tuple<string, double2>("水", 5000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "树苗_升级", 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("的生长速度", 255, 255, 255);
                rt.add("×5", 255, 255, 0);
                rt.add("，请注意这对上面的", 255, 255, 255);
                rt.add("种子", 152, 251, 152);
                rt.add("层无效", 255, 255, 255);
                rt.add("但对下面的层有效", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 11);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 140, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "光合作用",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 树苗生长速度",
                    A(255, 175, 0), 11);
                u.set_weight(3, 7, 3, 5);
                #endregion

                u = new g1_upgrade("吸水", level, layer);
                #region 自然树 - 树苗 - 吸水
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_光合作用"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("树苗成长度", 35));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "树苗_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("的基础生长进度获取变为", 255, 255, 255);
                rt.add("10000/s", 127, 255, 0);
                rt.add("，并使", 255, 255, 255);
                rt.add("水", 0, 191, 255);
                rt.add("的获取速度", 255, 255, 255);
                rt.add("×3", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 11);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 140, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "吸水",
                    rt,
                    A(0, 255, 180), 11, "目前效果：+0 生长进度/s",
                    A(255, 175, 0), 11);
                u.set_weight(3, 7, 5, 4);
                #endregion

                u = new g1_upgrade("营养供给", level, layer);
                #region 自然树 - 树苗 - 营养供给
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_吸水"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("树苗成长度", 150));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "树苗_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add("营养", 255, 255, 0);
                rt.add("的获取速度", 255, 255, 255);
                rt.add(" ×3", 255, 255, 0);
                rt.add("，此效果随", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("的", 255, 255, 255);
                rt.add("数量", 0, 255, 255);
                rt.add("的提升而提升", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 11);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 250, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "营养供给",
                    rt,
                    A(0, 255, 180), 11, "目前效果：×1 营养获取速度",
                    A(255, 175, 0), 11);
                u.set_weight(3, 7, 5, 4);
                #endregion

                u = new g1_upgrade("巨型树苗", level, layer);
                #region 自然树 - 树苗 - 巨型树苗
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_营养供给"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("树苗成长度", 600));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "树苗_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("使", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("的基础生长进度获取变为", 255, 255, 255);
                rt.add(number_format(3e6) + "/s", 127, 255, 0);
                rt.add("，", 255, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("的成长速度", 255, 255, 255);
                rt.add("×2", 255, 255, 0);
                rt.add("，仅本层产出的", 255, 255, 255);
                rt.add("自然点数", 0, 255, 127);
                rt.add("×2", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 11);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 250, 0, 0),
                    150, 110, A(175, 110, 139, 61), A(192, 255, 62),
                    A(255, 246, 143), 14, "巨型树苗",
                    rt,
                    A(0, 255, 180), 11, "目前效果：无效果",
                    A(255, 175, 0), 11);
                u.set_weight(3, 8, 6, 3);
                #endregion
                #endregion tab 树苗_升级

                #region tab 树苗_里程碑
                str = "树苗_里程碑";
                tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("里程碑", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                ms = new g1_milestone("树苗_里程碑_1", level, layer);
                #region 树苗_里程碑_1
                g1_ups[ms.store_name()] = ms;
                ms.can_reset = false;
                ms.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("树苗成长度", 100));
                ct.Add(costs);
                ms.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(ms, str, 1);
                rt = new rainbow_text(ms.name);
                rt.add("解锁", 255, 255, 255);
                rt.add("小树", 0, 255, 127);
                rt.add("层！", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
                ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                    300, 60, new thickness(0, 140, 0, 0),
                    new solid_type(A(175, 0, 66, 33)),
                    new solid_type(A(175, 83, 83, 166)),
                    new solid_type(A(0, 255, 127)), 1,
                    new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                        new ARGB(find_resource(ms.get_auto_res()).text_color()),
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new thickness(0, 5, 0, 0)),
                    rt,
                    new solid_type(A(50, 200, 200, 0)));
                #endregion 树苗_里程碑_1
                #endregion tab 树苗_里程碑

                sapling.exp_bar.prepare("树苗经验条", "vm_g1_layer_树苗_grid",
                    HorizontalAlignment.Center, VerticalAlignment.Top, 240, 30,
                    new thickness(0, 75, 0, 0), new solid_type(A(238, 233, 233)),
                    new solid_type(A(127, 255, 0)), 0.5, new solid_type(A(255, 250, 205)), 2,
                    new solid_text("进度 0 / 100", 14, A(35, 65, 90),
                    HorizontalAlignment.Center, VerticalAlignment.Center));
                #endregion layer 自然树 树苗

                layer = level.find_layer("小树");
                #region layer 自然树 小树
                #region tab 小树_主页
                str = "小树_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("树苗小树", level, layer);
                #region 自然树 - 小树 - 树苗小树
                g1_ups[u.store_name()] = u;
                u.no_cost = true;
                u.visitable = true;
                u.can_reset = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("树苗", 1)),
                    new Tuple<string, double2>("树苗成长度", 100));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "小树_主页", 1);
                rt = new rainbow_text(u.name);
                rt.add("需要达到", 0, 255, 255);
                rt.add(" 100 ", 255, 255, 0);
                rt.add("树苗成长度", 127, 255, 0);
                rt.add("。", 0, 255, 255);
                rt.add("将", 255, 255, 255);
                rt.add("所有", 255, 255, 0);
                rt.add("树苗", 192, 255, 62);
                rt.add("变为", 255, 255, 255);
                rt.add("小树", 152, 255, 152);
                rt.add("，", 255, 255, 255);
                rt.add("你将暂时不能获得更多", 0, 255, 255);
                rt.add("树苗成长度", 127, 255, 0);
                rt.add("，但仍能购买", 0, 255, 255);
                rt.add("树苗", 192, 255, 62);
                rt.add("层的升级", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 14);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Center,
                    VerticalAlignment.Top, new thickness(0, 150, 0, 0),
                    240, 120,
                    A(175, 139, 69, 19),
                    A(255, 165, 79),
                    A(0, 250, 154), 18, "树苗 → 小树",
                    rt,
                    null, 0, "",
                    null, 0);
                u.set_weight(3, 7, 0, 0);
                #endregion
                #endregion tab 小树_主页

                #region tab 小树_升级
                str = "小树_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 小树_升级

                #region tab 小树_里程碑
                str = "小树_里程碑";
                tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                    new solid_type(A(0, 205, 102)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("里程碑", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                ms = new g1_milestone("小树_里程碑_1", level, layer);
                #region 小树_里程碑_1
                g1_ups[ms.store_name()] = ms;
                ms.can_reset = false;
                ms.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("小树成长度", 100));
                ct.Add(costs);
                ms.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(ms, str, 1);
                rt = new rainbow_text(ms.name);
                rt.add("解锁", 255, 255, 255);
                rt.add("大树", 0, 255, 127);
                rt.add("层！", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
                ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                    300, 60, new thickness(0, 140, 0, 0),
                    new solid_type(A(175, 0, 66, 33)),
                    new solid_type(A(175, 83, 83, 166)),
                    new solid_type(A(0, 255, 127)), 1,
                    new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                        new ARGB(find_resource(ms.get_auto_res()).text_color()),
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new thickness(0, 5, 0, 0)),
                    rt,
                    new solid_type(A(50, 200, 200, 0)));
                #endregion 小树_里程碑_1
                #endregion tab 小树_里程碑

                smalltree.exp_bar.prepare("小树经验条", "vm_g1_layer_小树_grid",
                    HorizontalAlignment.Center, VerticalAlignment.Top, 240, 30,
                    new thickness(0, 75, 0, 0), new solid_type(A(238, 233, 233)),
                    new solid_type(A(127, 255, 0)), 0.5, new solid_type(A(255, 250, 205)), 2,
                    new solid_text("进度 0 / 100", 14, A(35, 65, 90),
                    HorizontalAlignment.Center, VerticalAlignment.Center));


                #endregion layer 自然树 小树

                layer = level.find_layer("营养");
                #region layer 自然树 营养
                #region tab 营养_主页
                str = "营养_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(127, 175, 0)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                #endregion tab 水_主页

                #region tab 营养_升级
                str = "营养_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(127, 175, 0)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);
                layer.curr_tab = tab;

                u = new g1_upgrade("矿物质", level, layer);
                #region 自然树 - 营养 - 矿物质
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 2000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "营养_升级", 1);
                layer.unlock_upgrade(u.name);
                rt = new rainbow_text(u.name);
                rt.add("随时间产生", 255, 255, 255);
                rt.add("营养", 255, 255, 0);
                rt.add("，让树长得更快，并使", 255, 255, 255);
                rt.add("水", 0, 191, 255);
                rt.add("的作用提升", 255, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 80, 0, 0),
                    150, 140, A(175, 35, 75, 0), A(30, 114, 255),
                    A(0, 255, 0), 14, "矿物质",
                    rt,
                    A(0, 255, 255), 11, "目前效果：+0 营养/s",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 4, 3);
                #endregion

                u = new g1_upgrade("光照", level, layer);
                #region 自然树 - 营养 - 光照
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_矿物质"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 4000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "营养_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("营养", 255, 255, 0);
                rt.add("的获取速度获得一个倍率，", 255, 255, 255);
                rt.add("但此倍率随当前", 0, 255, 255);
                rt.add("营养", 255, 255, 0);
                rt.add("的升高而降低", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 80, 0, 0),
                    150, 140, A(175, 35, 75, 0), A(30, 114, 255),
                    A(0, 255, 0), 14, "光照",
                    rt,
                    A(0, 255, 255), 11, "目前效果：×1 营养获取速率",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 4, 3);
                #endregion

                u = new g1_upgrade("肥料", level, layer);
                #region 自然树 - 营养 - 肥料
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_光照"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 15000));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "营养_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("营养", 255, 255, 0);
                rt.add("的获取速度获得一个倍率，", 255, 255, 255);
                rt.add("但此倍率随当前", 0, 255, 255);
                rt.add("植物成长度", 0, 255, 127);
                rt.add("的升高而降低", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(50, 220, 0, 0),
                    150, 140, A(175, 35, 75, 0), A(30, 114, 255),
                    A(0, 255, 0), 14, "肥料",
                    rt,
                    A(0, 255, 255), 11, "目前效果：×1 营养获取速率",
                    A(255, 175, 0), 12);
                u.set_weight(3, 7, 4, 3);
                #endregion

                u = new g1_upgrade("气流", level, layer);
                #region 自然树 - 营养 - 气流
                g1_ups[u.store_name()] = u;
                u.can_reset = true;
                u.prev(g1_ups["自然树_肥料"], 1);
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("自然点数", 200e3));
                ct.Add(costs);
                u.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(u, "营养_升级", 1);
                rt = new rainbow_text(u.name);
                rt.add("营养", 255, 255, 0);
                rt.add("的获取速度获得一个倍率，", 255, 255, 255);
                rt.add("此倍率随当前", 255, 255, 255);
                rt.add("生命力", 0, 255, 127);
                rt.add("的升高而升高。", 255, 255, 255);
                rt.add("提示：下一个升级在", 0, 255, 255);
                rt.add(find_resource("水"));
                rt.add("层级中", 0, 255, 255);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Center,
                    new Thickness(0), double.NaN, double.NaN, 12);
                u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    u.page + "_grid", HorizontalAlignment.Left,
                    VerticalAlignment.Top, new thickness(200, 220, 0, 0),
                    150, 140, A(175, 35, 75, 0), A(30, 114, 255),
                    A(0, 255, 0), 14, "气流",
                    rt,
                    A(0, 255, 255), 11, "目前效果：×1 营养获取速率",
                    A(255, 175, 0), 12);
                u.set_weight(3, 8, 3, 3);
                #endregion

                #endregion tab 营养_升级

                #region tab 营养_里程碑
                str = "营养_里程碑";
                tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                    new solid_type(A(127, 175, 0)), new solid_type(A(255, 255, 0)),
                    new solid_type(A(255, 255, 255)), 1.5,
                    new solid_text("里程碑", 14, A(0, 0, 255), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                ms = new g1_milestone("营养_里程碑_1", level, layer);
                #region 营养_里程碑_1
                g1_ups[ms.store_name()] = ms;
                ms.can_reset = false;
                ms.visitable = true;
                ct = new List<List<Tuple<string, double2>>>();
                costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                    new Tuple<string, double2>("营养", 5000));
                ct.Add(costs);
                ms.set_init_cost(ct, 0, ct.Count);
                layer.add_upgrade(ms, str, 1);
                rt = new rainbow_text(ms.name);
                rt.add("将", 255, 255, 255);
                rt.add("营养", 255, 255, 0);
                rt.add("的所有效果提升 ", 255, 255, 255);
                rt.add("^ 1.1", 255, 255, 0);
                rt.prepare("",
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
                ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                    ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                    300, 60, new thickness(0, 140, 0, 0),
                    new solid_type(A(175, 0, 66, 33)),
                    new solid_type(A(175, 83, 83, 166)),
                    new solid_type(A(0, 255, 127)), 1,
                    new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                        new ARGB(find_resource(ms.get_auto_res()).text_color()),
                        HorizontalAlignment.Center, VerticalAlignment.Top,
                        new thickness(0, 5, 0, 0)),
                    rt,
                    new solid_type(A(50, 200, 200, 0)));
                #endregion 营养_里程碑_1
                #endregion tab 营养_里程碑

                #endregion layer 自然树 营养
                #endregion level 自然树
            }
        }
    }
}
