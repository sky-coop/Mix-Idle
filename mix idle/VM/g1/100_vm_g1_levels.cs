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
        g1_level g1_current_level;
        public void g1_levels_init()
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
            g1_research research;
            textblock tb;
            List<List<Tuple<string, double2>>> ct;
            List<Tuple<string, double2>> costs;

            g1_current_level = null;
            g1_res = new Dictionary<string, g1_resource>();
            g1_layers = new Dictionary<string, g1_layer>();
            g1_achieves = new Dictionary<string, g1_achievement>();
            g1_levels = new Dictionary<string, g1_level>();
            g1_ups = new Dictionary<string, g1_upgrade>();
            yggdrasill = new Yggdrasill();
            yggdrasill.init_reseter();

            void research_text()
            {
                research.bar.draw.addTextblock(new textblock("t1", false, "",
                    "", A(0, 255, 0), double.NaN, 22, 16, new thickness(0, 2, 0, 0),
                   HorizontalAlignment.Center));
                research.bar.draw.addTextblock(new textblock("t2", false, "",
                    "", A(127, 255, 0), double.NaN, 28, 12,
                    new thickness(0, 45, 0, 0),
                    HorizontalAlignment.Center));
                research.bar.draw.addTextblock(new textblock("t3", false, "",
                    "", A(255, 180, 0), double.NaN, 17, 12,
                    new thickness(0, 73, 0, 0),
                    HorizontalAlignment.Center));
                research.bar.draw.clickable = drawable.click.left;
            }

            #region level 世界
            #region 世界 scenery
            str = "世界点数";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(255, 255, 255)));
            g1_res.Add(str, r);
            level = new g1_level("世界", r, A(50, 50, 50));
            g1_levels.Add(level.name, level);

            List<Point> points = new List<Point>();
            #region 河流
            points.Add(new Point(0, 550));
            points.Add(new Point(250, 450));
            points.Add(new Point(425, 320));
            points.Add(new Point(550, 190));
            points.Add(new Point(650, 110));
            points.Add(new Point(740, 70));
            points.Add(new Point(900, 75));
            points.Add(new Point(1020, 100));
            points.Add(new Point(1250, 200));
            points.Add(new Point(1400, 300));
            points.Add(new Point(1450, 450));
            points.Add(new Point(1400, 600));
            points.Add(new Point(1430, 730));
            points.Add(new Point(1360, 900));
            points.Add(new Point(1310, 900));
            points.Add(new Point(1380, 730));
            points.Add(new Point(1350, 600));
            points.Add(new Point(1400, 450));
            points.Add(new Point(1360, 320));
            points.Add(new Point(1220, 235));
            points.Add(new Point(1020, 150));
            points.Add(new Point(900, 120));
            points.Add(new Point(740, 120));
            points.Add(new Point(650, 165));
            points.Add(new Point(550, 250));
            points.Add(new Point(425, 380));
            points.Add(new Point(250, 500));
            points.Add(new Point(0, 600));
            #endregion
            curve c = new curve(points, 0.5, A(0, 199, 140), 4, true,
                A(50, 148, 197));
            c.name = "世界_河流";
            c.target = "vm_g1_" + level.name + "_scene_grid";
            c.draw_point = false;
            level.sceneries.Add(c);
            
            blocks b = new blocks();
            b.b.Add(new Tuple<ARGB, ARGB, Point, double, double, double>(
                A(255, 150, 0), A(255, 255, 0), new Point(290, 490), 50, 50, 330));
            b.b.Add(new Tuple<ARGB, ARGB, Point, double, double, double>(
                A(255, 160, 0), A(255, 255, 0), new Point(350, 450), 40, 50, 325));
            b.b.Add(new Tuple<ARGB, ARGB, Point, double, double, double>(
                A(255, 170, 0), A(255, 255, 0), new Point(400, 410), 50, 40, 320));
            b.b.Add(new Tuple<ARGB, ARGB, Point, double, double, double>(
                A(255, 180, 0), A(255, 255, 0), new Point(455, 365), 40, 40, 315));
            b.name = "世界_房屋";
            b.target = "vm_g1_" + level.name + "_scene_grid";
            level.sceneries.Add(b);
            #endregion
            #region layer 世界树
            str = "世界树等级";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 255, 0)));
            g1_res.Add(str, r);
            layer = new g1_layer("世界树", r);
            level.roots.Add(layer);
            g1_layers.Add(layer.name, layer);
            
            layer.unlocked = true;
            layer.glowing = true;
            layer.prepare("vm_g1_map_grid", new Point(200, 200),
                /* text */A(163, 73, 164), "Y", "Symbol", 0.75,
                /* fill */A(239, 228, 176), 100,
                /* line */A(0, 255, 0), 5,
                /*stroke*/A(0, 255, 0), 2);

            #endregion layer 世界树
            #region layer 森林
            str = "生命力";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(127, 255, 0)));
            g1_res.Add(str, r);
            layer = new g1_layer("森林", r);
            layer.prev(level.find_layer("世界树"));
            g1_layers.Add(layer.name, layer);

            str = "生命效果";
            r = new g1_resource(0.25, str,
                getSCB(Color.FromRgb(173, 255, 47)));
            g1_res.Add(str, r);
            layer.resources.Add(r);

            layer.prepare("vm_g1_map_grid", new Point(600, 350),
                /* text */A(40, 255, 255), "F", "Microsoft YaHei UI", 0.75,
                /* fill */A(75, 150, 0), 100,
                /* line */A(135, 206, 235), 5,
                /*stroke*/A(135, 206, 235), 2);
            #endregion layer 森林
            #region layer 文明
            str = "文明水平";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(255, 255, 0)));
            g1_res.Add(str, r);
            layer = new g1_layer("文明", r);
            layer.prev(level.find_layer("世界树"));
            g1_layers.Add(layer.name, layer);

            str = "研究点数";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 255, 0)));
            g1_res.Add(str, r);
            layer.resources.Add(r);

            layer.prepare("vm_g1_map_grid", new Point(260, 610),
                /* text */A(255, 255, 0), "C", "Consolas", 0.75,
                /* fill */A(30, 144, 255), 100,
                /* line */A(255, 97, 0), 5,
                /*stroke*/A(255, 97, 0), 2);
            #endregion layer 文明
            #region layer 自然树
            str = "自然力量";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 255, 127)));
            g1_res.Add(str, r);
            layer = new g1_layer("自然树", r);
            layer.prev(level.find_layer("森林"));
            g1_layers.Add(layer.name, layer);

            layer.prepare("vm_g1_map_grid", new Point(750, 180),
                /* text */A(0, 255, 127), "自然", "Microsoft YaHei UI", 0.3,
                /* fill */A(139, 117, 0), 100,
                /* line */A(0, 250, 154), 5,
                /*stroke*/A(0, 250, 154), 2);
            #endregion layer 自然树
            #region layer 水晶树
            str = "水晶球";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(200, 200, 200)));
            g1_res.Add(str, r);
            layer = new g1_layer("水晶树", r);
            layer.prev(level.find_layer("森林"));
            g1_layers.Add(layer.name, layer);

            str = "生命转化倍增";
            r = new g1_resource(100, str,
                getSCB(Color.FromRgb(255, 127, 127)));
            g1_res.Add(str, r);

            str = "生命转化";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(255, 0, 0)));
            g1_res.Add(str, r);
            layer.resources.Add(r);

            layer.unlocked = true;
            layer.prepare("vm_g1_map_grid", new Point(900, 200),
                /* text */A(139, 35, 35), "水晶", "Microsoft YaHei UI", 0.3,
                /* fill */A(191, 239, 255), 100,
                /* line */A(255, 20, 147), 5,
                /*stroke*/A(255, 20, 147), 2);
            #endregion layer 自然树
            #region layer 合成树
            str = "合成熟练度";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(255, 255, 100)));
            g1_res.Add(str, r);
            layer = new g1_layer("合成树", r);
            layer.prev(level.find_layer("森林"));
            g1_layers.Add(layer.name, layer);

            layer.unlocked = true;
            layer.prepare("vm_g1_map_grid", new Point(810, 335),
                /* text */A(255, 233, 55), "合成", "Microsoft YaHei UI", 0.3,
                /* fill */A(80, 160, 240), 100,
                /* line */A(255, 255, 127), 5,
                /*stroke*/A(255, 255, 127), 2);
            #endregion layer 合成树

            #region layer 世界 世界树
            layer = level.find_layer("世界树");

            #region tab 世界树_主页
            str = "世界树_主页";
            tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                new solid_type(A(25, 25, 112)), new solid_type(A(127, 0, 255)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("主页", 14, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);

            str = "0层点数";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 205, 225)));
            g1_res[str] = r;

            //TODO: 提醒进入世界树
            u = new g1_upgrade("世界树提示", level, layer);
            #region 世界 - 世界树 - 世界树提示
            g1_ups.Add(u.store_name(), u);
            u.can_reset = false;
            layer.add_upgrade(u, tab.name, 1);
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界点数", 0));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            rt = new rainbow_text(u.name);
            rt.add("点击下方按钮进入", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("以取得更好的进展", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(80, 150, 0, 0),
                240, 60, A(175, 25, 25, 82), A(30, 114, 255),
                A(0, 255, 0), 14, "提示！",
                rt,
                null, 0, "",
                null, 0);
            u.set_weight(3, 4, 0, 0);
            #endregion

            slot = new g1_save_slot(new g1_drawable("世界树_slot",
                level, layer, tab, 1, "vm_g1_layer_" + layer.name + "_" + tab.name + 
                "_1_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                240, 130, new thickness(0, 230, 0, 0)));
            #region
            slot.draw.v = Visibility.Hidden;
            slot.draw.setFill(new solid_type(A(0, 87, 43)));
            slot.draw.setStroke(new solid_type(A(255, 255, 0)), 2);

            slot.draw.addGrid(new grid("main", "", 240, 130, new thickness(0, 0, 0, 0),
                Visibility.Visible));
            slot.draw.addTextblock(new textblock("__世界树_slot_title", true, "main",
                "世界树已经成长到可以被进入了", A(0, 255, 127), double.NaN, double.NaN, 14,
                new thickness(0, 15, 0, 0), HorizontalAlignment.Center));
            tb = new textblock("__世界树_slot_t1", true, "main",
                "", A(255, 255, 0), double.NaN, double.NaN, 14,
                new thickness(0, 40, 0, 0), HorizontalAlignment.Center);
            tb.add_resource("0层点数");
            slot.draw.addTextblock(tb);
            slot.draw.addButton(new button("__世界树_slot_enter", true, "main", "进入", 100, 40,
                new thickness(0, 0, 0, 10), HorizontalAlignment.Center, VerticalAlignment.Bottom));
            #endregion
            #endregion tab 世界树_主页

            #region tab 世界树_升级
            str = "世界树_升级";
            tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                new solid_type(A(25, 25, 112)), new solid_type(A(127, 0, 255)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("升级", 14, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            layer.curr_tab = tab;

            u = new g1_upgrade("成长", level, layer);
            #region 世界 - 世界树 - 成长
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            u.visitable = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界点数", 15));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "世界树_升级", 1);
            layer.unlock_upgrade(u.name);
            rt = new rainbow_text(u.name);
            rt.add("让", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("获得生机，随时间获得", 255, 255, 255);
            rt.add("+1 经验/s", 255, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                150, 120, A(175, 25, 25, 82), A(30, 114, 255),
                A(0, 255, 0), 14, "成长",
                rt,
                A(0, 255, 180), 11, "目前效果：+0 世界树经验/s",
                A(255, 125, 64), 12);
            u.set_weight(3, 4, 4, 3);
            #endregion

            u = new g1_upgrade("河流", level, layer);
            #region 世界 - 世界树 - 河流
            u.prev(g1_ups["世界_成长"], 1);
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界树等级", 2));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "世界树_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("消耗", 255, 255, 255);
            rt.add("世界树等级", 0, 255, 0);
            rt.add("来生成一条", 255, 255, 255);
            rt.add("河流", 0, 150, 255);
            rt.add("以养育其他生命，并为", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("提供", 255, 255, 255);
            rt.add("+2 经验/s", 255, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", 
                HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(200, 120, 0, 0),
                150, 120, A(175, 25, 25, 82), A(30, 114, 255),
                A(0, 150, 255), 14, "河流",
                rt,
                A(0, 255, 180), 11, "目前效果：+0 世界树经验/s",
                A(255, 125, 64), 12);
            u.set_weight(3, 7, 3, 3);
            #endregion

            u = new g1_upgrade("森林", level, layer);
            #region 世界 - 世界树 - 森林
            u.prev(g1_ups["世界_河流"], 1);
            g1_ups.Add(u.store_name(), u);
            u.can_reset = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界点数", 300));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "世界树_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("解锁", 255, 255, 255);
            rt.add("森林", 0, 210, 0);
            rt.add("层！", 255, 255, 255);
            rt.add("森林", 0, 210, 0);
            rt.add("层中有各种各样的树，完成它们以加速", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("的生长", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" + 
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 240, 0, 0),
                150, 120, A(175, 25, 25, 82), A(30, 114, 255),
                A(0, 210, 0), 14, "森林",
                rt,
                A(0, 255, 180), 11, "目前效果：无",
                A(255, 125, 64), 12);
            u.set_weight(3, 7, 3, 3);
            #endregion

            u = new g1_upgrade("C代表什么", level, layer);
            #region 世界 - 世界树 - C代表什么
            u.prev(g1_ups["世界_河流"], 1);
            g1_ups.Add(u.store_name(), u);
            u.can_reset = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界树等级", 10));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "世界树_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("解锁", 255, 255, 255);
            rt.add(" C ", 255, 255, 0);
            rt.add("层！", 255, 255, 255);
            rt.add("此层可以让", 255, 255, 255);
            rt.add("本游戏", 255, 127, 0);
            rt.add("对", 255, 255, 255);
            rt.add("主游戏", 127, 127, 255);
            rt.add("提供奖励", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(200, 240, 0, 0),
                150, 120, A(175, 25, 25, 82), A(30, 114, 255),
                A(255, 255, 0), 14, "C代表什么？",
                rt,
                A(0, 255, 180), 11, "目前效果：无",
                A(255, 125, 64), 12);
            u.set_weight(3, 7, 3, 3);
            #endregion

            u = new g1_upgrade("虚假的可购买", level, layer);
            #region 世界 - 世界树 - 虚假的可购买
            u.prev(g1_ups["世界_森林"], 1);
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界点数", 2.5e9));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "世界树_升级", 2);
            rt = new rainbow_text(u.name);
            rt.add("解锁下一个升级，下一个升级可以被升级多次，" +
                "同时为", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("提供", 255, 255, 255);
            rt.add("+12 经验/s", 255, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                150, 120, A(175, 34, 139, 34), A(124, 252, 0),
                A(152, 245, 255), 14, "虚假的可购买！",
                rt,
                A(250, 235, 215), 11, "目前效果：无",
                A(255, 153, 18), 12);
            u.set_weight(3, 7, 3, 3);
            #endregion

            #endregion tab 世界树_升级

            #region tab 世界树_可购买
            str = "世界树_可购买";
            tab = new g1_tab(str, "", 75, 30, new thickness(210, 410, 0, 0),
                new solid_type(A(25, 25, 112)), new solid_type(A(127, 0, 255)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("可购买", 14, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            #endregion tab 世界树_可购买

            #region tab 世界树_里程碑
            str = "世界树_里程碑";
            tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                new solid_type(A(25, 25, 112)), new solid_type(A(127, 0, 255)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("里程碑", 14, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            #endregion tab 世界树_里程碑
            
            ms = new g1_milestone("世界树_里程碑_1", level, layer);
            #region 世界树_里程碑_1
            g1_ups.Add(ms.store_name(), ms);
            ms.can_reset = false;
            ms.visitable = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界树等级", 4));
            ct.Add(costs);
            ms.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(ms, str, 1);
            rt = new rainbow_text(ms.name);
            rt.add("你可以在", 255, 255, 255);
            rt.add("森林", 0, 210, 0);
            rt.add("层重置", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("来获取", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.prepare("", 
                HorizontalAlignment.Center, VerticalAlignment.Top,
                new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
            ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 60, new thickness(0, 120, 0, 0),
                new solid_type(A(175, 0, 39, 39)),
                new solid_type(A(175, 0, 139, 139)),
                new solid_type(A(0, 255, 0)), 1,
                new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                    new ARGB(find_resource(ms.get_auto_res()).text_color()),
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new thickness(0, 5, 0, 0)),
                rt,
                new solid_type(A(50, 200, 200, 0)));
            #endregion 世界树_里程碑_1

            ms = new g1_milestone("世界树_里程碑_2", level, layer);
            #region 世界树_里程碑_2
            g1_ups.Add(ms.store_name(), ms);
            ms.prev(g1_ups["世界_世界树_里程碑_1"], 1);
            ms.next(g1_ups["世界_世界树提示"], 1);
            ms.can_reset = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界树等级", 10));
            ct.Add(costs);
            ms.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(ms, str, 1);
            rt = new rainbow_text(ms.name);
            rt.add("你可以在主页中进入", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("，并且此后每10倍的", 255, 255, 255);
            rt.add("世界树等级", 0, 255, 0);
            rt.add("能够解锁一个新的", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("层，最多解锁100层", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Top,
                new Thickness(0, 25, 0, 0), double.NaN, double.NaN, 12);
            ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 60, new thickness(0, 180, 0, 0),
                new solid_type(A(175, 0, 39, 39)),
                new solid_type(A(175, 0, 139, 139)),
                new solid_type(A(0, 255, 0)), 1,
                new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                    new ARGB(find_resource(ms.get_auto_res()).text_color()),
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new thickness(0, 5, 0, 0)),
                rt,
                new solid_type(A(50, 200, 200, 0)));
            #endregion 世界树_里程碑_2



            #endregion layer 世界 世界树
            #region layer 世界 森林
            layer = level.find_layer("森林");
            #region tab 森林_主页
            str = "森林_主页";
            tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                new solid_type(A(245, 222, 155)), new solid_type(A(0, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("主页", 14, A(56, 94, 15), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            layer.curr_tab = tab;

            u = new g1_upgrade("世界树重置", level, layer);
            #region 世界 - 森林 - 世界树重置
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界树等级", 1));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "森林_主页", 1);
            rt = new rainbow_text(u.name);
            rt.add("重置", 0, 255, 255);
            rt.add("世界树等级", 0, 255, 0);
            rt.add("以及一些", 200, 200, 200);
            rt.add("世界树", 0, 225, 0);
            rt.add("升级，并消耗", 200, 200, 200);
            rt.add("20%", 255, 255, 0);
            rt.add("的", 200, 200, 200);
            rt.add("世界点数", 255, 255, 255);
            rt.add("，获得基于", 200, 200, 200);
            rt.add("世界点数", 255, 255, 255);
            rt.add("和", 200, 200, 200);
            rt.add("世界点数产生速度", 0, 255, 0);
            rt.add("的", 200, 200, 200);
            rt.add("生命力", 127, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 240, 0, 0),
                240, 120, 
                A(175, 139, 69, 19), 
                A(255, 165, 79),
                A(0, 250, 154), 16, "世界树等级 → 生命力",
                rt,
                A(0, 235, 215), 12, "当前： +1 生命力",
                null, 0);
            u.set_weight(3, 7, 3, 0);
            #endregion

            #endregion tab 森林_主页
            #region tab 森林_升级
            str = "森林_升级";
            tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                new solid_type(A(245, 222, 155)), new solid_type(A(0, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("升级", 14, A(56, 94, 15), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);


            u = new g1_upgrade("自然树", level, layer);
            #region 世界 - 森林 - 自然树
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            u.visitable = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命力", 300));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "森林_升级", 1);
            layer.unlock_upgrade(u.name);
            rt = new rainbow_text(u.name);
            rt.add("解锁", 255, 255, 255);
            rt.add("自然树", 0, 210, 0);
            rt.add("层！你可以在其中培育不同种类的树，获取", 255, 255, 255);
            rt.add("自然力量", 0, 255, 127);
            rt.add("。同时使", 255, 255, 255);
            rt.add("“成长”", 0, 255, 0);
            rt.add("升级不再被重置。", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 120, 0, 0),
                150, 120, A(175, 47, 79, 79), A(193, 255, 193),
                A(0, 210, 0), 14, "自然树",
                rt,
                null, 0, "",
                A(255, 106, 106), 12);
            u.set_weight(3, 7, 0, 3);
            #endregion

            u = new g1_upgrade("水晶树", level, layer);
            #region 世界 - 森林 - 水晶树
            g1_ups.Add(u.store_name(), u);
            u.prev(g1_ups["世界_自然树"], 1);
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命力", 300e6));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "森林_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("解锁", 255, 255, 255);
            rt.add("水晶树", 200, 200, 200);
            rt.add("层！你可以在其中挖掘不同颜色的水晶，获取", 255, 255, 255);
            rt.add(find_resource("水晶球"));
            rt.add("。同时使", 255, 255, 255);
            rt.add("“河流”", 0, 255, 0);
            rt.add("升级不再被重置。", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(200, 120, 0, 0),
                150, 120, A(175, 47, 79, 79), A(193, 255, 193),
                A(0, 210, 0), 14, "水晶树",
                rt,
                null, 0, "",
                A(255, 106, 106), 12);
            u.set_weight(3, 7, 0, 3);
            #endregion 
            #endregion tab 森林_升级
            #region tab 森林_里程碑
            str = "森林_里程碑";
            tab = new g1_tab(str, "", 75, 30, new thickness(305, 410, 0, 0),
                new solid_type(A(245, 222, 155)), new solid_type(A(0, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("里程碑", 14, A(56, 94, 15), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);

            ms = new g1_milestone("森林_里程碑_1", level, layer);
            #region 森林_里程碑_1
            g1_ups.Add(ms.store_name(), ms);
            ms.can_reset = false;
            ms.visitable = true;
            ms.log_show = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命力", 100e3));
            ct.Add(costs);
            ms.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(ms, str, 1);
            layer.unlock_upgrade(u.name);
            rt = new rainbow_text(ms.name);
            rt.add(find_resource("生命效果"));
            rt.add("+0.05", 255, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Top,
                new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
            ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 60, new thickness(0, 120, 0, 0),
                new solid_type(A(175, 0, 39, 39)),
                new solid_type(A(175, 0, 139, 139)),
                new solid_type(A(0, 255, 0)), 1,
                new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                    new ARGB(find_resource(ms.get_auto_res()).text_color()),
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new thickness(0, 5, 0, 0)),
                rt,
                new solid_type(A(50, 200, 200, 0)));
            #endregion 森林_里程碑_1

            #endregion tab 森林_里程碑
            #endregion layer 世界 森林
            #region layer 世界 文明
            layer = level.find_layer("文明");
            #region tab 文明_主页
            str = "文明_主页";
            tab = new g1_tab(str, "", 62, 30, new thickness(15, 410, 0, 0),
                new solid_type(A(179, 238, 58)), new solid_type(A(255, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("主页", 14, A(47, 79, 79), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            layer.curr_tab = tab;


            //civilisation 文明

            //clan 部落
            //countryside 乡村
            //castle 城堡
            //city 城市
            //country 国家
            //continental 大陆的
            //celestial 天空的
            //cosmic 宇宙的
            u = new g1_upgrade("升级文明", level, layer);
            #region 世界 - 文明 - 升级文明
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            u.visitable = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界点数", 30e3));
            ct.Add(costs);
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("世界点数", 100e9));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "文明_主页", 1);
            layer.unlock_upgrade(u.name);
            rt = new rainbow_text(u.name);
            rt.add("提升你的", 255, 255, 255);
            rt.add("文明", 255, 255, 0);
            rt.add("规模到", 255, 255, 255);
            rt.add("部落（Clan）", 255, 150, 0);
            rt.add("，解锁一些升级并获得", 255, 255, 255);
            rt.add("文明水平", 255, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 210, 0, 0),
                240, 150,
                A(175, 139, 69, 19),
                A(255, 165, 79),
                A(0, 250, 154), 16, "升级文明",
                rt,
                A(0, 235, 215), 12, "当前Civilisation规模： 无\n" +
                                    "下一个Civilisation规模： Clan",
                A(255, 150, 0), 12);
            u.set_weight(3, 7, 4, 4);
            #endregion
            #endregion tab 文明_主页
            #region tab 文明_升级
            str = "文明_升级";
            tab = new g1_tab(str, "", 62, 30, new thickness(92, 410, 0, 0),
                new solid_type(A(179, 238, 58)), new solid_type(A(255, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("升级", 14, A(47, 79, 79), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            
            u = new g1_upgrade("经验总结", level, layer);
            #region 世界 - 文明 - 经验总结
            g1_ups.Add(u.store_name(), u);
            u.prev(g1_ups["世界_升级文明"], 1);   //不一定是1
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("文明水平", 1));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "文明_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("随时间缓慢获取", 255, 255, 255);
            rt.add("文明水平", 255, 255, 0);
            rt.add("，速率随", 255, 255, 255);
            rt.add("文明水平", 255, 255, 0);
            rt.add("快速降低，同时使", 255, 255, 255);
            rt.add("所有经验", 0, 255, 255);
            rt.add("（包括主游戏的！如战斗经验、采矿经验）", 0, 255, 0);
            rt.add("获取量", 255, 255, 255);
            rt.add("+20%。", 255, 255, 0);  
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 60, 0, 0),
                300, 100, A(175, 89, 76, 62), A(255, 193, 193),
                A(0, 255, 0), 14, "经验总结",
                rt,
                A(255, 255, 0), 12, "目前效果：+0 文明水平/s",
                A(255, 150, 106), 12);
            u.set_weight(3, 8, 2.5, 2.5);
            #endregion 

            u = new g1_upgrade("房屋", level, layer);
            #region 世界 - 文明 - 房屋
            g1_ups.Add(u.store_name(), u);
            u.prev(g1_ups["世界_升级文明"], 1);   //不一定是1
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("文明水平", 5));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "文明_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("建造一些房屋，使", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("和", 255, 255, 255);
            rt.add("文明水平", 255, 255, 0);
            rt.add("二者一起促进它们自身的获取", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 160, 0, 0),
                300, 100, A(175, 89, 76, 62), A(255, 193, 193),
                A(0, 255, 0), 14, "房屋",
                rt,
                A(255, 255, 0), 12, "当前：×1 文明水平和生命力",
                A(255, 150, 106), 12);
            u.set_weight(3, 8, 2.5, 2.5);
            #endregion

            u = new g1_upgrade("桥", level, layer);
            #region 世界 - 文明 - 桥
            g1_ups.Add(u.store_name(), u);
            u.prev(g1_ups["世界_房屋"], 1);   //不一定是1
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("文明水平", 20));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "文明_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("建造连通", 255, 255, 255);
            rt.add("世界树", 0, 225, 0);
            rt.add("的桥梁，使", 255, 255, 255);
            rt.add("世界树等级", 0, 255, 0);
            rt.add("加成", 255, 255, 255);
            rt.add("文明水平", 255, 255, 0);
            rt.add("的获取，", 255, 255, 255);
            rt.add("文明水平", 255, 255, 0);
            rt.add("加成", 255, 255, 255);
            rt.add("世界树经验", 0, 255, 0);
            rt.add("的获取", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 260, 0, 0),
                300, 100, A(175, 89, 76, 62), A(255, 193, 193),
                A(0, 255, 0), 14, "桥",
                rt,
                A(255, 255, 0), 12, "当前：×1 文明水平获取，×1 世界树经验获取",
                A(255, 150, 106), 12);
            u.set_weight(3, 8, 5, 2.5);
            #endregion 

            #endregion tab 文明_升级
            #region tab 文明_可购买
            str = "文明_可购买";
            tab = new g1_tab(str, "", 62, 30, new thickness(169, 410, 0, 0),
                new solid_type(A(179, 238, 58)), new solid_type(A(255, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("可购买", 14, A(47, 79, 79), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            #endregion tab 文明_可购买
            #region tab 文明_研究
            str = "文明_研究";
            tab = new g1_tab(str, "", 62, 30, new thickness(246, 410, 0, 0),
                new solid_type(A(179, 238, 58)), new solid_type(A(255, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("研究", 14, A(47, 79, 79), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);

            u = new g1_upgrade("研究重置", level, layer);
            #region 世界 - 文明 - 研究重置
            g1_ups.Add(u.store_name(), u);
            u.prev(g1_ups["世界_升级文明"], 1);   //不一定是1
            u.can_reset = true;
            u.check = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("研究点数", 0));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "文明_研究", 1);
            u.attach_to(tab);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + 
                "_attach_grid", //attach特有的
                HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(50, 60, 0, 0),
                300, 30, A(175, 0, 100, 100), A(127, 127, 0),
                A(127, 255, 0), 14, "重置所有研究等级，回收 0 研究点数",
                null,
                null, 0, "",
                null, 0);
            u.set_weight(1, 0, 0, 0);
            #endregion

            str = "食物学";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(255, 255, 0)));
            g1_res.Add(str, r);
            research = new g1_research(str, level, layer, new EXP_BAR(str), this,
                find_resource("研究点数"), 10, 5, 3);
            #region 世界 - 文明 - 食物学
            g1_ups.Add(research.store_name(), research);
            research.prev(g1_ups["世界_升级文明"], 1);   //不一定是1
            layer.add_upgrade(research, tab.name, 1);
            research.bar.prepare("research__" + str,
                "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                research.page + "_grid", HorizontalAlignment.Left, VerticalAlignment.Top,
                350, 90, new thickness(25, 90, 0, 0),
                new solid_type(A(150, 0, 80, 80)),
                new solid_type(A(150, 0, 175, 175)), 0,
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("研究进度", 15, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 24, 0, 0)));
            research_text();
            #endregion


            //战斗学 
            str = "战斗学";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 255, 255)));
            g1_res.Add(str, r);
            research = new g1_research(str, level, layer, new EXP_BAR(str), this,
                find_resource("研究点数"), 50, 10, 3);
            #region 世界 - 文明 - 战斗学
            g1_ups.Add(research.store_name(), research);
            research.prev(g1_ups["世界_升级文明"], 1);   //不一定是1
            layer.add_upgrade(research, tab.name, 1);
            research.bar.prepare("research__" + str,
                "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                research.page + "_grid", HorizontalAlignment.Left, VerticalAlignment.Top,
                350, 90, new thickness(25, 180, 0, 0),
                new solid_type(A(150, 0, 100, 50)),
                new solid_type(A(150, 0, 200, 100)), 0,
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("研究进度", 15, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 24, 0, 0)));
            research_text();
            #endregion

            //语言与文字 （纯 +文明水平）
            str = "语言与文字";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 255, 0)));
            g1_res.Add(str, r);
            research = new g1_research(str, level, layer, new EXP_BAR(str), this,
                find_resource("研究点数"), 240, 40, 2);
            #region 世界 - 文明 - 语言与文字
            g1_ups.Add(research.store_name(), research);
            research.prev(g1_ups["世界_升级文明"], 1);   //不一定是1
            layer.add_upgrade(research, tab.name, 1);
            research.bar.prepare("research__" + str,
                "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                research.page + "_grid", HorizontalAlignment.Left, VerticalAlignment.Top,
                350, 90, new thickness(25, 270, 0, 0),
                new solid_type(A(150, 80, 80, 0)),
                new solid_type(A(150, 150, 150, 0)), 0,
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("研究进度", 15, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 24, 0, 0)));
            research_text();
            #endregion

            //水源工程  +药水制造速度  +水获取
            str = "水源工程";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(100, 255, 100)));
            g1_res.Add(str, r);
            research = new g1_research(str, level, layer, new EXP_BAR(str), this,
                find_resource("研究点数"), 600, 100, 3);
            #region 世界 - 文明 - 水源工程
            g1_ups.Add(research.store_name(), research);
            layer.add_upgrade(research, tab.name, 2);
            research.bar.prepare("research__" + str,
                "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                research.page + "_grid", HorizontalAlignment.Left, VerticalAlignment.Top,
                350, 90, new thickness(25, 90, 0, 0),
                new solid_type(A(150, 0, 80, 80)),
                new solid_type(A(150, 0, 175, 175)), 0,
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("研究进度", 15, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 24, 0, 0)));
            research_text();
            #endregion

            //林业
            str = "林业";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(100, 255, 100)));
            g1_res.Add(str, r);
            research = new g1_research(str, level, layer, new EXP_BAR(str), this,
                find_resource("研究点数"), 1250, 250, 3);
            #region 世界 - 文明 - 林业
            g1_ups.Add(research.store_name(), research);
            layer.add_upgrade(research, tab.name, 2);
            research.bar.prepare("research__" + str,
                "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                research.page + "_grid", HorizontalAlignment.Left, VerticalAlignment.Top,
                350, 90, new thickness(25, 180, 0, 0),
                new solid_type(A(150, 0, 100, 50)),
                new solid_type(A(150, 0, 200, 100)), 0,
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("研究进度", 15, A(255, 255, 0), HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 24, 0, 0)));
            research_text();
            #endregion
            #endregion tab 文明_研究
            #region tab 文明_里程碑
            str = "文明_里程碑";
            tab = new g1_tab(str, "", 62, 30, new thickness(323, 410, 0, 0),
                new solid_type(A(179, 238, 58)), new solid_type(A(255, 255, 0)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("里程碑", 14, A(47, 79, 79), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);

            ms = new g1_milestone("文明_里程碑_1", level, layer);
            #region 文明_里程碑_1
            g1_ups.Add(ms.store_name(), ms);
            ms.prev(g1_ups["世界_升级文明"], 1);
            ms.next(g1_ups["世界_水源工程"], 1);
            ms.can_reset = false;
            ms.log_show = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("食物学", 1000));
            ct.Add(costs);
            ms.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(ms, str, 1);
            rt = new rainbow_text(ms.name);
            rt.add("解锁研究：", 255, 255, 255);
            rt.add("水源工程", 100, 255, 100);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Top,
                new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
            ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 60, new thickness(0, 60, 0, 0),
                new solid_type(A(175, 139, 101, 8)),
                new solid_type(A(175, 139, 136, 120)),
                new solid_type(A(0, 255, 0)), 1,
                new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                    new ARGB(find_resource(ms.get_auto_res()).text_color()),
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new thickness(0, 5, 0, 0)),
                rt,
                new solid_type(A(50, 200, 200, 0)));
            #endregion 文明_里程碑_1

            ms = new g1_milestone("文明_里程碑_2", level, layer);
            #region 文明_里程碑_2
            g1_ups.Add(ms.store_name(), ms);
            ms.prev(g1_ups["世界_升级文明"], 1);
            ms.next(g1_ups["世界_林业"], 1);
            ms.can_reset = false;
            ms.log_show = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("战斗学", 1000));
            ct.Add(costs);
            ms.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(ms, str, 1);
            rt = new rainbow_text(ms.name);
            rt.add("解锁研究：", 255, 255, 255);
            rt.add("林业", 100, 255, 100);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Top,
                new Thickness(0, 35, 0, 0), double.NaN, double.NaN, 12);
            ms.prepare(ms.name, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                ms.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 60, new thickness(0, 120, 0, 0),
                new solid_type(A(175, 139, 101, 8)),
                new solid_type(A(175, 139, 136, 120)),
                new solid_type(A(0, 255, 0)), 1,
                new solid_text(ms.get_auto_value() + " " + ms.get_auto_res(), 16,
                    new ARGB(find_resource(ms.get_auto_res()).text_color()),
                    HorizontalAlignment.Center, VerticalAlignment.Top,
                    new thickness(0, 5, 0, 0)),
                rt,
                new solid_type(A(50, 200, 200, 0)));
            #endregion 文明_里程碑_2

            #endregion tab 文明_里程碑
            #endregion layer 世界 文明
            #region layer 世界 自然树
            layer = level.find_layer("自然树");
            #region tab 自然树_主页
            str = "自然树_主页";
            tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                new solid_type(A(255, 255, 224)), new solid_type(A(192, 255, 62)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("主页", 14, A(135, 35, 35), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            layer.curr_tab = tab;

            u = new g1_upgrade("自然树生命力", level, layer);
            #region 世界 - 自然树 - 自然树生命力
            g1_ups.Add(u.store_name(), u);
            u.visitable = true;
            u.can_reset = true;
            u.check = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命力", 0));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "自然树_主页", 1);
            rt = new rainbow_text(u.name);
            rt.add("投入", 255, 255, 255);
            rt.add("10%", 255, 255, 0);
            rt.add("的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("到树种子中，使它生长更快，投入的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("不可回收", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 100, 0, 0),
                240, 110,
                A(175, 139, 69, 19),
                A(255, 165, 79),
                A(0, 250, 154), 16, "生命力 → 种子",
                rt,
                A(0, 235, 215), 12, "当前：已投入 0 生命力",
                null, 0);
            u.set_weight(3, 4, 4, 0);
            #endregion

            slot = new g1_save_slot(new g1_drawable("自然树_slot",
                level, layer, tab, 1, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                240, 130, new thickness(0, 230, 0, 0)));
            #region
            slot.draw.setFill(new solid_type(A(66, 54, 20)));
            slot.draw.setStroke(new solid_type(A(255, 255, 0)), 2);

            slot.draw.addGrid(new grid("choose", "", 240, 130, new thickness(0, 0, 0, 0),
                Visibility.Visible));
            slot.draw.addButton(new button("__自然树_slot_b1", true, "choose", "轻木", 70, 30,
                new thickness(10, 10, 0, 0)));
            slot.draw.addTextblock(new textblock("__自然树_slot_t1", true, "choose",
                "（简单模式）生长最快，寿命最短，推荐首次使用", A(0, 255, 0),
                140, 30, 12, new thickness(90, 10, 0, 0)));
            slot.draw.addButton(new button("__自然树_slot_b2", true, "choose", "苹果树", 70, 30,
                new thickness(10, 50, 0, 0)));
            slot.draw.addTextblock(new textblock("__自然树_slot_t2", true, "choose",
                "（普通模式）较为平均的树，有特别的产物", A(255, 255, 0),
                140, 30, 12, new thickness(90, 50, 0, 0)));
            slot.draw.addButton(new button("__自然树_slot_b3", true, "choose", "柏树", 70, 30,
                new thickness(10, 90, 0, 0)));
            slot.draw.addTextblock(new textblock("__自然树_slot_t3", true, "choose",
                "（困难模式）生长最慢，但后期发展较为强力", A(255, 150, 0),
                140, 30, 12, new thickness(90, 90, 0, 0)));

            slot.draw.addGrid(new grid("information", "", double.NaN, double.NaN,
                new thickness(0, 0, 0, 0), Visibility.Hidden));
            slot.draw.addTextblock(new textblock("__自然树_slot_title", true, "information",
                "简单模式: 轻木", A(0, 255, 0), double.NaN, double.NaN, 16,
                new thickness(10, 5, 0, 0)));
            tb = new textblock("__自然树_slot_life", true, "information",
                "0 生命力", A(0, 255, 0), double.NaN, double.NaN, 14,
                new thickness(15, 30, 0, 0));
            tb.add_cal(g1_cal_生命力_自然树);
            slot.draw.addTextblock(tb);
            tb = new textblock("__自然树_slot_content", true, "information",
                "0 自然点数", A(0, 255, 0), double.NaN, double.NaN, 14,
                new thickness(15, 50, 0, 0));
            tb.add_resource("自然点数");
            slot.draw.addTextblock(tb);
            tb = new textblock("__自然树_slot_gain", true, "information",
                "按下“完成”将所有自然点数转化为自然力量，结束本次培育", 
                A(255, 255, 0), 135, double.NaN, 12,
                new thickness(15, 70, 0, 0));
            slot.draw.addTextblock(tb);

            slot.draw.addButton(new button("__自然树_slot_enter", true, "information",
                "进入！", 70, 30,
                new thickness(160, 20, 0, 0)));
            slot.draw.addButton(new button("__自然树_slot_finish", true, "information",
                "完成", 70, 30,
                new thickness(160, 80, 0, 0)));

            #endregion

            #endregion tab 自然树_主页
            #region tab 自然树_升级
            str = "自然树_升级";
            tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                new solid_type(A(255, 255, 224)), new solid_type(A(192, 255, 62)),
                new solid_type(A(255, 255, 255)), 1.5,
                new solid_text("升级", 14, A(135, 35, 35), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);

            u = new g1_upgrade("生态循环", level, layer);
            #region 世界 - 自然树 - 生态
            g1_ups.Add(u.store_name(), u);
            u.visitable = true;
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("自然力量", 20e3));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "自然树_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("本层中的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("会以", 255, 255, 255);
            rt.add(" ^ 0.4", 255, 255, 0);
            rt.add("/s ", 0, 255, 255);
            rt.add("的速度自我复制，且", 255, 255, 255);
            rt.add("自然力量", 0, 255, 127);
            rt.add("以较低的效率增强这一效果", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(5, 0, 5, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(75, 80, 0, 0),
                250, 140,
                A(175, 139, 101, 8),
                A(255, 165, 79),
                A(0, 225, 154), 16, "生态循环",
                rt,
                A(0, 255, 127), 12, "当前：+(^ 0.3)/s 生命力",
                A(255, 150, 0), 12);
            u.set_weight(3, 4, 4, 3);
            #endregion

            u = new g1_upgrade("自然循环", level, layer);
            #region 世界 - 自然树 - 自然
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            u.visitable = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("自然力量", 400e3));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "自然树_升级", 1);
            layer.unlock_upgrade(u.name);
            rt = new rainbow_text(u.name);
            rt.add("将", 255, 255, 255);
            rt.add("自然点数", 0, 255, 127);
            rt.add("的获取", 255, 255, 255);
            rt.add(" ×1.5", 255, 255, 0);
            rt.add("，且", 255, 255, 255);
            rt.add("自然力量", 0, 255, 127);
            rt.add("以较低的效率增强这一效果", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(5, 0, 5, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(75, 220, 0, 0),
                250, 140,
                A(175, 139, 101, 8),
                A(255, 165, 79),
                A(0, 225, 154), 16, "自然循环",
                rt,
                A(0, 255, 127), 12, "当前：×1 自然点数",
                A(255, 150, 0), 12);
            u.set_weight(3, 4, 4, 3);
            #endregion

            #endregion tab 自然树_升级
            #endregion layer 世界 自然树
            #region layer 世界 水晶树
            layer = level.find_layer("水晶树");
            #region tab 水晶树_主页
            str = "水晶树_主页";
            tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                new solid_type(A(255, 225, 255)), new solid_type(A(0, 250, 154)),
                new solid_type(A(148, 0, 211)), 1.5,
                new solid_text("主页", 14, A(255, 0, 0), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            layer.curr_tab = tab;
            
            u = new g1_upgrade("水晶树生命力", level, layer);
            #region 世界 - 水晶树 - 水晶树生命力
            g1_ups.Add(u.store_name(), u);
            u.visitable = true;
            u.can_reset = true;
            u.check = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命力", 0));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, tab.name, 1);
            rt = new rainbow_text(u.name);
            rt.add("投入", 255, 255, 255);
            rt.add("10%", 255, 255, 0);
            rt.add("的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("到水晶树中，使", 255, 255, 255);
            rt.add("绿色水晶", 0, 255, 0);
            rt.add("成长更快，投入的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("不可回收", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 90, 0, 0),
                300, 90,
                fill_c: A(175, 32, 108, 100),
                stroke_c: A(255, 246, 143),
                name_c: A(0, 250, 154), 14, "生命力 → 水晶树",
                rt,
                effect_c: A(127, 255, 0), 12, "当前：已投入 0 生命力",
                null, 0);
            u.set_weight(3, 4, 4, 0);
            #endregion

            u = new g1_upgrade("水晶树生命转化", level, layer);
            #region 世界 - 水晶树 - 水晶树生命转化
            g1_ups.Add(u.store_name(), u);
            u.visitable = true;
            u.can_reset = true;
            u.check = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命效果", 0));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, tab.name, 1);
            rt = new rainbow_text(u.name);
            rt.add("置换", 255, 255, 255);
            rt.add("5%", 255, 255, 0);
            rt.add("的", 255, 255, 255);
            rt.add(find_resource("生命效果"));
            rt.add("为", 255, 255, 255);
            rt.add(find_resource("生命转化"));
            rt.add("，使", 255, 255, 255);
            rt.add("红色水晶", 255, 0, 0);
            rt.add("成长更快，投入的", 255, 255, 255);
            rt.add(find_resource("生命效果"));
            rt.add("在结束水晶层时", 255, 255, 255);
            rt.add("可以回收", 255, 255, 0);
            rt.add("。", 255, 255, 255);
            rt.add("（注意：点击此升级会删除你的所有绿色水晶！）", 0, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 180, 0, 0),
                300, 90,
                fill_c: A(175, 139, 26, 26),
                stroke_c: A(255, 246, 143),
                name_c: A(0, 250, 154), 14, "生命效果 → 生命转化",
                rt,
                effect_c: A(173, 255, 47), 12, "当前：已投入 0 生命效果",
                null, 0);
            u.set_weight(3, 6, 3, 0);
            #endregion

            slot = new g1_save_slot(new g1_drawable("水晶树_slot",
                level, layer, tab, 1, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 90, new thickness(0, 270, 0, 0)));
            #region
            slot.draw.setFill(new solid_type(A(66, 54, 20)));
            slot.draw.setStroke(new solid_type(A(255, 255, 0)), 2);

            slot.draw.addGrid(new grid("choose", "", 300, 90, new thickness(0, 0, 0, 0),
                Visibility.Visible));
            slot.draw.addButton(new button("__水晶树_slot_b1", true, "choose", "纯色水晶", 70, 25,
                new thickness(6, 1.5, 0, 0)));
            slot.draw.addTextblock(new textblock("__水晶树_slot_t1", true, "choose",
                "（简单模式）仅3种颜色的水晶，上手简单，有工厂辅助", A(0, 255, 0),
                200, 25, 10.5, new thickness(90, 2, 0, 0)));
            slot.draw.addButton(new button("__水晶树_slot_b2", true, "choose", "混色水晶", 70, 25,
                new thickness(6, 30.5, 0, 0)));
            slot.draw.addTextblock(new textblock("__水晶树_slot_t2", true, "choose",
                "（普通模式）7种颜色的水晶，前期发展慢但后期发展更强", A(255, 255, 0),
                200, 25, 10.5, new thickness(90, 31, 0, 0)));
            slot.draw.addButton(new button("__水晶树_slot_b3", true, "choose", "变色水晶", 70, 25,
                new thickness(6, 59.5, 0, 0)));
            slot.draw.addTextblock(new textblock("__水晶树_slot_t3", true, "choose",
                "（困难模式）水晶之间的关系变得更加复杂，需要很好的规划", A(255, 150, 0),
                200, 25, 10.5, new thickness(90, 60, 0, 0)));

            slot.draw.addGrid(new grid("information", "", double.NaN, double.NaN,
                new thickness(0, 0, 0, 0), Visibility.Hidden));
            slot.draw.addTextblock(new textblock("__水晶树_slot_title", true, "information",
                "简单模式: 纯色水晶", A(0, 255, 0), double.NaN, double.NaN, 14,
                new thickness(10, 4, 0, 0)));
            tb = new textblock("__水晶树_slot_life", true, "information",
                "0 生命力", A(0, 255, 0), double.NaN, double.NaN, 12,
                new thickness(15, 22, 0, 0));
            tb.add_cal(g1_cal_生命力_水晶树);
            slot.draw.addTextblock(tb);
            tb = new textblock("__水晶树_slot_content", true, "information",
                "0 水晶块", A(0, 255, 0), double.NaN, double.NaN, 12,
                new thickness(15, 38, 0, 0));
            tb.add_resource("水晶块");
            slot.draw.addTextblock(tb);
            tb = new textblock("__水晶树_slot_gain", true, "information",
                "按下“完成”将所有水晶块转化为水晶球，结束本次探索",
                A(255, 255, 0), 200, double.NaN, 12,
                new thickness(15, 54, 0, 0));
            slot.draw.addTextblock(tb);

            slot.draw.addButton(new button("__水晶树_slot_enter", true, "information",
                "进入！", 70, 30,
                new thickness(215, 10, 0, 0)));
            slot.draw.addButton(new button("__水晶树_slot_finish", true, "information",
                "完成", 70, 30,
                new thickness(215, 50, 0, 0)));

            #endregion
            
            #endregion tab 水晶树_主页

            #region tab 水晶树_升级
            str = "水晶树_升级";
            tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                new solid_type(A(255, 225, 255)), new solid_type(A(0, 250, 154)),
                new solid_type(A(148, 0, 211)), 1.5,
                new solid_text("升级", 14, A(255, 0, 0), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            
            u = new g1_upgrade("折射", level, layer);
            #region 世界 - 水晶树 - 折射
            g1_ups.Add(u.store_name(), u);
            layer.add_upgrade(u, tab.name, 1);
            layer.unlock_upgrade(u.name);
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("水晶球", 1e12));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            rt = new rainbow_text(u.name);
            rt.add("使初始水晶块数变为", 255, 255, 255);
            rt.add(find_resource("水晶球"));
            rt.add("的", 255, 255, 255);
            rt.add("(^ 0.4)", 255, 255, 0);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(5, 0, 5, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(75, 100, 0, 0),
                250, 100,
                A(175, 100, 100, 100), A(200, 200, 200),
                A(204, 225, 204), 16, "折射",
                rt,
                A(0, 255, 255), 12, "当前：初始水晶块 = 0",
                A(255, 255, 0), 12);
            u.set_weight(3, 3, 3, 3);
            #endregion 世界 - 水晶树 - 折射

            u = new g1_upgrade("献祭水晶", level, layer);
            #region 世界 - 水晶树 - 献祭水晶
            g1_ups.Add(u.store_name(), u);
            u.prev(g1_ups["世界_折射"], 1);
            layer.add_upgrade(u, tab.name, 1);
            u.can_reset = false; //使用了生命转化 不重置
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("水晶球", 1e40)),
                new Tuple<string, double2>("生命转化", 0.05));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            rt = new rainbow_text(u.name);
            rt.add("借助", 255, 255, 255);
            rt.add(find_resource("生命转化"));
            rt.add("的力量，根据", 255, 255, 255);
            rt.add(find_resource("水晶球"));
            rt.add("数量", 255, 255, 255);
            rt.add("极大地", 255, 255, 0);
            rt.add("倍增水晶的获取，倍率为：", 255, 255, 255);
            rt.add("", 255, 255, 255);
            rt.add("RGB水晶：水晶球 ^ (1 / 3)", 255, 255, 255);
            rt.add("", 255, 255, 255);
            rt.add("白色水晶：水晶球 ^ (1 / 6)", 255, 255, 255);
            rt.add("", 255, 255, 255);
            rt.add("YMC水晶：水晶球 ^ (1 / 9)", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(5, 0, 5, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(75, 200, 0, 0),
                250, 160,
                A(175, 100, 100, 100), A(200, 200, 200),
                A(204, 225, 204), 16, "献祭水晶",
                rt,
                null, 12, "",
                A(255, 255, 0), 12);
            u.set_weight(2, 5, 0, 2);
            #endregion 世界 - 水晶树 - 献祭水晶


            #endregion tab 自然树_升级

            #endregion layer 世界 自然树
            #region layer 世界 合成树
            layer = level.find_layer("合成树");
            #region tab 合成树_主页
            str = "合成树_主页";
            tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                new solid_type(A(180, 180, 0)), new solid_type(A(255, 255, 0)),
                new solid_type(A(150, 225, 150)), 1.5,
                new solid_text("主页", 14, A(0, 50, 50), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            layer.curr_tab = tab;

            u = new g1_upgrade("合成树生命力", level, layer);
            #region 世界 - 水晶树 - 合成树生命力
            g1_ups.Add(u.store_name(), u);
            u.visitable = true;
            u.can_reset = true;
            u.check = false;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("生命力", 0));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, tab.name, 1);
            rt = new rainbow_text(u.name);
            rt.add("投入", 255, 255, 255);
            rt.add("10%", 255, 255, 0);
            rt.add("的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("到合成树中，使", 255, 255, 255);
            rt.add("生命球", 0, 255, 0);
            rt.add("的生成比率提升，投入的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("不可回收", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(10, 0, 10, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center,
                VerticalAlignment.Top, new thickness(0, 90, 0, 0),
                300, 90,
                fill_c: A(175, 32, 108, 100),
                stroke_c: A(255, 246, 143),
                name_c: A(0, 250, 154), 14, "生命力 → 合成树",
                rt,
                effect_c: A(127, 255, 0), 12, "当前：已投入 0 生命力",
                null, 0);
            u.set_weight(3, 4, 4, 0);
            #endregion

            

            slot = new g1_save_slot(new g1_drawable("合成树_slot",
                level, layer, tab, 1, "vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Center, VerticalAlignment.Top,
                300, 90, new thickness(0, 270, 0, 0)));
            #region
            slot.draw.setFill(new solid_type(A(66, 54, 20)));
            slot.draw.setStroke(new solid_type(A(255, 255, 0)), 2);

            slot.draw.addGrid(new grid("choose", "", 300, 120, new thickness(0, 0, 0, 0),
                Visibility.Visible));
            slot.draw.addButton(new button("__合成树_slot_b1", true, "choose", "四色面板", 70, 25,
                new thickness(6, 1.5, 0, 0)));
            slot.draw.addTextblock(new textblock("__合成树_slot_t1", true, "choose",
                "（简单模式）只有4种颜色球在游戏面板上，易于消除", A(0, 255, 0),
                200, 25, 10.5, new thickness(90, 2, 0, 0)));
            slot.draw.addButton(new button("__合成树_slot_b2", true, "choose", "五色面板", 70, 25,
                new thickness(6, 30.5, 0, 0)));
            slot.draw.addTextblock(new textblock("__合成树_slot_t2", true, "choose",
                "（普通模式）在简单模式的基础上增加了一种较强大的颜色", A(255, 255, 0),
                200, 25, 10.5, new thickness(90, 31, 0, 0)));
            slot.draw.addButton(new button("__合成树_slot_b3", true, "choose", "六色面板", 70, 25,
                new thickness(6, 59.5, 0, 0)));
            slot.draw.addTextblock(new textblock("__合成树_slot_t3", true, "choose",
                "（困难模式）在普通模式的基础上增加了一种更强大的颜色", A(255, 150, 0),
                200, 25, 10.5, new thickness(90, 60, 0, 0)));

            slot.draw.addGrid(new grid("information", "", double.NaN, double.NaN,
                new thickness(0, 0, 0, 0), Visibility.Hidden));
            slot.draw.addTextblock(new textblock("__合成树_slot_title", true, "information",
                "简单模式: 纯色水晶", A(0, 255, 0), double.NaN, double.NaN, 14,
                new thickness(10, 4, 0, 0)));
            tb = new textblock("__合成树_slot_life", true, "information",
                "0 生命力", A(0, 255, 0), double.NaN, double.NaN, 12,
                new thickness(15, 22, 0, 0));
            tb.add_cal(g1_cal_生命力_合成树);
            slot.draw.addTextblock(tb);
            tb = new textblock("__合成树_slot_content", true, "information",
                "0 水晶块", A(0, 255, 0), double.NaN, double.NaN, 12,
                new thickness(15, 38, 0, 0));
            tb.add_resource("合成分数");
            slot.draw.addTextblock(tb);
            tb = new textblock("__合成树_slot_gain", true, "information",
                "按下“完成”将所有合成分数转化为合成熟练度，结束本次探索",
                A(255, 255, 0), 200, double.NaN, 12,
                new thickness(15, 54, 0, 0));
            slot.draw.addTextblock(tb);

            slot.draw.addButton(new button("__合成树_slot_enter", true, "information",
                "进入！", 70, 30,
                new thickness(215, 10, 0, 0)));
            slot.draw.addButton(new button("__合成树_slot_finish", true, "information",
                "完成", 70, 30,
                new thickness(215, 50, 0, 0)));

            #endregion

            #endregion tab 水晶树_主页

            #region tab 水晶树_升级
            str = "水晶树_升级";
            tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                new solid_type(A(180, 180, 0)), new solid_type(A(255, 255, 0)),
                new solid_type(A(150, 225, 150)), 1.5,
                new solid_text("升级", 14, A(0, 50, 50), HorizontalAlignment.Center,
                VerticalAlignment.Center), true);
            layer.tabs.Add(tab.name, tab);
            /*
            u = new g1_upgrade("生态循环", level, layer);
            #region 世界 - 自然树 - 生态
            g1_ups.Add(u.store_name(), u);
            u.visitable = true;
            u.can_reset = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("自然力量", 20e3));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "自然树_升级", 1);
            rt = new rainbow_text(u.name);
            rt.add("本层中的", 255, 255, 255);
            rt.add("生命力", 127, 255, 0);
            rt.add("会以", 255, 255, 255);
            rt.add(" ^ 0.4", 255, 255, 0);
            rt.add("/s ", 0, 255, 255);
            rt.add("的速度自我复制，且", 255, 255, 255);
            rt.add("自然力量", 0, 255, 127);
            rt.add("以较低的效率增强这一效果", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(5, 0, 5, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(75, 80, 0, 0),
                250, 140,
                A(175, 139, 101, 8),
                A(255, 165, 79),
                A(0, 225, 154), 16, "生态循环",
                rt,
                A(0, 255, 127), 12, "当前：+(^ 0.3)/s 生命力",
                A(255, 150, 0), 12);
            u.set_weight(3, 4, 4, 3);
            #endregion

            u = new g1_upgrade("自然循环", level, layer);
            #region 世界 - 自然树 - 自然
            g1_ups.Add(u.store_name(), u);
            u.can_reset = true;
            u.visitable = true;
            ct = new List<List<Tuple<string, double2>>>();
            costs = upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("自然力量", 400e3));
            ct.Add(costs);
            u.set_init_cost(ct, 0, ct.Count);
            layer.add_upgrade(u, "自然树_升级", 1);
            layer.unlock_upgrade(u.name);
            rt = new rainbow_text(u.name);
            rt.add("将", 255, 255, 255);
            rt.add("自然点数", 0, 255, 127);
            rt.add("的获取", 255, 255, 255);
            rt.add(" ×1.5", 255, 255, 0);
            rt.add("，且", 255, 255, 255);
            rt.add("自然力量", 0, 255, 127);
            rt.add("以较低的效率增强这一效果", 255, 255, 255);
            rt.prepare("",
                HorizontalAlignment.Center, VerticalAlignment.Center,
                new Thickness(5, 0, 5, 0), double.NaN, double.NaN, 12);
            u.prepare("vm_g1_layer_" + layer.name + "_" + tab.name + "_" +
                u.page + "_grid", HorizontalAlignment.Left,
                VerticalAlignment.Top, new thickness(75, 220, 0, 0),
                250, 140,
                A(175, 139, 101, 8),
                A(255, 165, 79),
                A(0, 225, 154), 16, "自然循环",
                rt,
                A(0, 255, 127), 12, "当前：×1 自然点数",
                A(255, 150, 0), 12);
            u.set_weight(3, 4, 4, 3);
            #endregion
            */
            #endregion tab 自然树_升级

            #endregion layer 世界 自然树
            yggdrasill.exp_bar.prepare("世界树经验条", "vm_g1_layer_世界树_grid",
                HorizontalAlignment.Center, VerticalAlignment.Top, 240, 30,
                new thickness(0, 55, 0, 0), new solid_type(A(240, 255, 240)),
                new solid_type(A(0, 255, 0)), 0.5, new solid_type(A(0, 199, 140)), 2,
                new solid_text("经验值 0 / 100", 14, A(8, 46, 84), 
                HorizontalAlignment.Center, VerticalAlignment.Center));
            #endregion level 世界
            
            #region level 世界树
            level = new g1_level("世界树", find_resource("世界点数") as g1_resource, 
                A(66, 132, 111));
            level.resources.Add(find_resource("世界树等级"));
            level.prev(g1_levels["世界"]);
            level.size_change = true;
            level.stage = 1;
            level.stage_max = 100;
            g1_levels[level.name] = level;
            #endregion
            
            #region level 自然树
            str = "自然点数";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(0, 255, 127)));
            g1_res[str] = r;
            level = new g1_level("自然树", r, A(36, 109, 67));
            level.prev(g1_levels["世界"]);
            g1_levels[level.name] = level;
            #endregion

            #region level 水晶树
            str = "水晶块";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(240, 255, 240)));
            g1_res[str] = r;
            level = new g1_level("水晶树", r, A(100, 100, 100));
            level.prev(g1_levels["世界"]);
            g1_levels[level.name] = level;
            #endregion

            #region level 合成树
            str = "合成分数";
            r = new g1_resource(0, str,
                getSCB(Color.FromRgb(255, 255, 0)));
            g1_res[str] = r;
            level = new g1_level("合成树", r, A(100, 100, 0));
            level.prev(g1_levels["世界"]);
            g1_levels[level.name] = level;
            #endregion
        }

        //开始一关
        public bool g1_level_start(string name)
        {
            g1_level p = g1_levels[name];
            if (!p.started)
            {
                p.start(time_all_acutally, time_all_real());
                return true;
            }
            return false;
        }

        public void g1_level_draw_base(string name)
        {
            Grid main = (Grid)vm_elems["vm_g1_main_grid"];
            Grid scene_base = (Grid)vm_elems["vm_g1_scene_grid"];
            scene_base.Children.Clear();

            Grid map = g1_getMap();
            map.Children.Clear();
            map.Children.Add(scene_base);

            Grid ctrl = g1_getCTRL();
            Grid layer_grid = (Grid)vm_elems["vm_g1_layer_grid"];
            layer_grid.Children.Clear();

            g1_mode = g1_show_mode.normal;

            Grid scene = new Grid
            {
                Name = "vm_g1_" + name + "_scene_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            scene_base.Children.Add(scene);
            vm_assign(scene);

            Grid radar_main = (Grid)vm_elems["vm_g1_radar_main"];
            Grid radar_scenery = (Grid)vm_elems["vm_g1_radar_scenery"];
            radar_main.Children.Clear();
            radar_scenery.Children.Clear();

            StackPanel top = (StackPanel)vm_elems["vm_g1_top_panel"];
            top.Children.Clear();



            g1_level p = g1_levels[name];
            g1_current_level = p;
            bool success = g1_level_start(name);

            g1_level level = g1_current_level;

            main.Background = p.color.toBrush();
            map.Background = p.color.toBrush();
            g1_current_layer = null;

            //主步骤
            g1_level_draw(name);

            #region 读显示数据
            foreach (g1_scenery scenery in p.sceneries)
            {
                scenery.changed = true;
            }

            Rectangle view = (Rectangle)vm_elems["vm_g1_radar_view"];
            view.Margin = new Thickness(level.view_point.X, level.view_point.Y, 0, 0);

            g1_layer temp = g1_current_level.watching_layer;
            if (temp != null)
            {
                if (temp.unlocked)
                {
                    g1_mode = g1_show_mode.right;
                }
                else
                {
                    g1_mode = g1_show_mode.normal;
                }
                g1_show();
                if (temp.unlocked)
                {
                    game_key_name("vm_g1_map_grid_layer__" + temp.name);
                }
                else
                {
                    game_key_name("vm_g1_right_ret");
                }
                g1_show();
                g1_upgrade_check();
            }
            #endregion 读显示数据
        }
    }
}
