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
        public void g1_level_draw_合成树(bool success)
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


            g1_map_redraw(2, 1);

            if (success)
            {
                #region level 合成树
                str = "合成分数";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 255, 100)));
                g1_res[str] = r;
                level.clear();
                level.resources.Add(find_resource("生命力"));
                level.resources.Add(r);

                #region layer 合成资源
                str = "分数球";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 255, 0)));
                g1_res[str] = r;
                layer = new g1_layer("合成资源", r);
                level.roots.Add(layer);
                g1_layers[layer.name] = layer;

                str = "回归球";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(127, 255, 255)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "增量球";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(255, 150, 255)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "生命球";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(127, 255, 0)));
                g1_res[str] = r;
                layer.resources.Add(r);

                //TODO 其他难度的球

                str = "分数球生成比率";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(255, 255, 0)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "回归球生成比率";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(127, 255, 255)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "增量球生成比率";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(255, 150, 255)));
                g1_res[str] = r;
                layer.resources.Add(r);

                str = "生命球生成比率";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(127, 255, 0)));
                g1_res[str] = r;
                layer.resources.Add(r);


                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(200, 225),
                    /* text */A(255, 255, 255), "R", "Consolas", 0.75,
                    /* fill */A(125, 125, 125), 100,
                    /* line */A(255, 255, 255), 5,
                    /*stroke*/A(255, 255, 255), 2);
                #endregion layer 合成资源

                #region layer M3
                str = "三消分数";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(150, 200, 150)));
                g1_res[str] = r;
                layer = new g1_layer("三消", r);
                layer.prev(g1_layers["合成资源"]);
                g1_layers[layer.name] = layer;

                layer.unlocked = true;
                layer.prepare("vm_g1_map_grid", new Point(400, 225),
                    /* text */A(255, 255, 0), "M3", "Consolas", 0.5,
                    /* fill */A(75, 75, 75), 100,
                    /* line */A(150, 200, 150), 5,
                    /*stroke*/A(150, 200, 150), 2);

                str = "M3活动时间";
                r = new g1_resource(0, str,
                    getSCB(Color.FromRgb(0, 255, 0)));
                g1_res[str] = r;

                str = "M3活动时间上限";
                r = new g1_resource(1, str,
                    getSCB(Color.FromRgb(255, 255, 0)));
                g1_res[str] = r;
                #endregion layer M3

                #region layer 合成树 合成资源
                layer = level.find_layer("合成资源");
                #region tab 合成资源_主页
                str = "合成资源_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(150, 150, 150)), new solid_type(A(255, 255, 255)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.curr_tab = tab;
                layer.tabs.Add(tab.name, tab);

                #endregion tab 合成资源_主页
                #region tab 合成资源_升级
                str = "合成资源_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(150, 150, 150)), new solid_type(A(255, 255, 255)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                /*
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
                */

                #endregion tab 合成资源_升级
                #endregion layer 合成树 M3
                #region layer 合成树 M3
                layer = level.find_layer("三消");
                #region tab 三消_主页
                str = "三消_主页";
                tab = new g1_tab(str, "", 75, 30, new thickness(20, 410, 0, 0),
                    new solid_type(A(120, 160, 120)), new solid_type(A(200, 255, 200)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("主页", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.curr_tab = tab;
                layer.tabs.Add(tab.name, tab);

                game_Grid_Elements = new game_grid_element[8, 8];
                for (int i = 0; i < game_Grid_Elements.GetLength(0); i++)
                {
                    for (int j = 0; j < game_Grid_Elements.GetLength(1); j++)
                    {
                        game_grid_element x = new game_grid_element(
                            new drawable(tab.name + "_m3button__" + i + "_" + j, "",
                            HorizontalAlignment.Stretch, VerticalAlignment.Stretch,
                            double.NaN, double.NaN, new thickness(0, 0, 0, 0)));
                        //TODO 边框，颜色和内容
                        x.draw.setFill(new solid_type(A(160, 175, 175, 175), HorizontalAlignment.Stretch,
                            VerticalAlignment.Stretch));
                        x.draw.setStroke(new solid_type(A(80, 255, 255, 255), HorizontalAlignment.Stretch,
                            VerticalAlignment.Stretch), 1.5);
                        x.draw.setMask(new solid_type(A(150, 0, 0, 0), HorizontalAlignment.Stretch,
                            VerticalAlignment.Stretch));
                        x.draw.masking = false;
                        x.draw.addRectangle(new rectangle(
                            "球", false, "", A(255, 255, 255), A(120, 180, 255), 1,
                            20, 20, 10000, 10000,
                            new thickness(0, 0, 0, 0), HorizontalAlignment.Center,
                            VerticalAlignment.Center));
                        x.draw.addTextblock(new textblock("球类型", false, "", "",
                            A(0, 0, 0), double.NaN, double.NaN, 10, new thickness(0, 0, 0, 0),
                            HorizontalAlignment.Center, VerticalAlignment.Center));
                        x.draw.clickable = drawable.click.lr;
                        x.r = i;
                        x.c = j;
                        game_Grid_Elements[i, j] = x;
                    }
                }
                game_Grid = new game_grid(game_Grid_Elements, "m3",
                    level, layer, tab, 1,
                    "vm_g1_layer_" + layer.name + "_" + tab.name + "_1_grid",
                    HorizontalAlignment.Center, VerticalAlignment.Top, 300, 300,
                    new thickness(0, 50, 0, 0));
                game_Grid.setFill(new solid_type(A(100, 100, 100), HorizontalAlignment.Stretch,
                    VerticalAlignment.Stretch));
                game_Grid.setStroke(new solid_type(A(255, 255, 255), HorizontalAlignment.Stretch,
                    VerticalAlignment.Stretch), 2.5);
                game_grids[game_Grid.name] = game_Grid;
                #endregion tab 三消_主页
                #region tab 三消_升级
                str = "三消_升级";
                tab = new g1_tab(str, "", 75, 30, new thickness(115, 410, 0, 0),
                    new solid_type(A(120, 160, 120)), new solid_type(A(200, 255, 200)),
                    new solid_type(A(0, 0, 0)), 1.5,
                    new solid_text("升级", 14, A(0, 0, 0), HorizontalAlignment.Center,
                    VerticalAlignment.Center), true);
                layer.tabs.Add(tab.name, tab);

                /*
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
                */

                #endregion tab 水晶聚合处_升级
                #endregion layer 合成树 M3
                #endregion
            }
        }
    }
}
