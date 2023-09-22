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
        [Serializable]
        public class g1_upgrade : upgrade
        {
            public bool check = true;

            public bool no_cost = false;
            public string store_name()
            {
                return g1_level.name + "_" + name;
            }
            public string name_base;
            public int page = 1;
            public g1_tab tab;

            public g1_level g1_level;
            public g1_layer g1_layer;
            public bool attaching = false;
            public bool special = false;
            public bool visitable = false;
            public new g1_upgrade reseter;

            public bool accing = false;
            public double2 acc_time = 0;
            public double2 acc_time2 = 0;
            public double2 count = 0;

            public bool valid = true;

            public string target;

            public HorizontalAlignment ha;
            public VerticalAlignment va;

            public double width;
            public double height;

            public thickness t;

            public ARGB fill_color;
            public ARGB stroke_color;

            public ARGB name_color;
            public double name_size;
            public string name_show;
            public double name_weight = 5;

            public rainbow_text content;
            public double content_weight = 8;

            public ARGB effect_color;
            public double effect_size;
            public string effect;
            public double effect_weight = 6;

            public ARGB cost_color;
            public double cost_size;
            public double cost_weight = 6;

            public Dictionary<g1_upgrade, int> prevs =
                new Dictionary<g1_upgrade, int>();
            public Dictionary<g1_upgrade, int> nexts =
                new Dictionary<g1_upgrade, int>();

            public g1_upgrade(string NAME, g1_level g1_Level, g1_layer g1_Layer):
                base(NAME, "世界树")
            {
                g1_level = g1_Level;
                g1_layer = g1_Layer;
            }

            public void attach_to(g1_tab t)
            {
                t.attach.Add(this);
                attaching = true;
            }

            public new void set_init_cost(List<List<Tuple<string, double2>>> COST, int LEVEL, int MAX_LEVEL)
            {
                base.set_init_cost(COST, LEVEL, MAX_LEVEL);
                if (can_reset)
                {
                    reseter = new g1_upgrade(name, g1_level, g1_layer);
                    reseter.visitable = visitable;
                }
            }

            public void prev(g1_upgrade u, int n)
            {
                prevs.Add(u, n);
                u.nexts.Add(this, n);
            }

            public void next(g1_upgrade u, int n)
            {
                nexts.Add(u, n);
                u.prevs.Add(this, n);
            }

            public void prepare(string TARGET, HorizontalAlignment HA, 
                VerticalAlignment VA, thickness T, double WIDTH, double HEIGHT,
                ARGB fill_c, ARGB stroke_c,
                ARGB name_c, double name_s, string NAME,
                rainbow_text CONTENT,
                ARGB effect_c, double effect_s, string EFFECT,
                ARGB cost_c, double cost_s)
            {
                target = TARGET;
                ha = HA;
                va = VA;
                t = T;
                width = WIDTH;
                height = HEIGHT;

                fill_color = fill_c;
                stroke_color = stroke_c;

                name_color = name_c;
                name_size = name_s;
                name_show = NAME;

                content = CONTENT;

                effect_color = effect_c;
                effect_size = effect_s;
                effect = EFFECT;

                cost_color = cost_c;
                cost_size = cost_s;
            }

            public void set_weight(double name_w, double content_w,
                double effect_w, double cost_w)
            {
                name_weight = name_w;
                content_weight = content_w;
                effect_weight = effect_w;
                cost_weight = cost_w;
            }
            
            public new void reset()
            {
                if (can_reset)
                {
                    base.reset();
                    visitable = reseter.visitable;
                }
            }
        }
        Dictionary<string, g1_upgrade> g1_ups = new Dictionary<string, g1_upgrade>();
        
        [Serializable]
        public class g1_repeatable : g1_upgrade
        {
            public g1_repeatable(string NAME, g1_level g1_Level, g1_layer g1_Layer,
                int max = -1, bool auto = true) :
                base(NAME, g1_Level, g1_Layer)
            {
                max_level = max;
                auto_cost = auto;
            }
        }

        public void g1_draw_upgrade(g1_upgrade u)
        {
            Grid target = (Grid)vm_elems[u.target];
            string name_base = target.Name + "_upgrade__" + u.name;
            bool e = exist_elem(name_base + "_grid", target);

            u.name_base = name_base;

            if (!e)
            {
                Grid content = new Grid
                {
                    Name = name_base + "_grid",
                    HorizontalAlignment = u.ha,
                    VerticalAlignment = u.va,
                    Width = u.width,
                    Height = u.height,
                    Margin = u.t.GetThickness(),
                };
                target.Children.Add(content);
                vm_assign(content);

                for (int i = 0; i < 4; i++)
                {
                    content.RowDefinitions.Add(new RowDefinition());
                }
                content.RowDefinitions[0].Height = new GridLength(u.name_weight, GridUnitType.Star);
                content.RowDefinitions[1].Height = new GridLength(u.content_weight, GridUnitType.Star);
                content.RowDefinitions[2].Height = new GridLength(u.effect_weight, GridUnitType.Star);
                content.RowDefinitions[3].Height = new GridLength(u.cost_weight, GridUnitType.Star);

                Rectangle bg = new Rectangle
                {
                    Name = name_base + "_bg",
                    Width = u.width,
                    Height = u.height,
                    Fill = u.fill_color.toBrush(),
                    Stroke = u.stroke_color.toBrush(),
                    StrokeThickness = 1,
                };
                Grid.SetRowSpan(bg, 100);
                content.Children.Add(bg);
                vm_assign(bg);

                Grid[] parts = new Grid[4];
                for (int i = 0; i < 4; i++)
                {
                    Grid p = new Grid
                    {
                        Name = name_base + "_grid_" + i,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    parts[i] = p;
                    Grid.SetRow(p, i);
                    content.Children.Add(p);
                    vm_assign(p);
                }
                if(u.content != null)
                {
                    u.content.target = parts[1].Name;
                }

                TextBlock t0 = new TextBlock
                {
                    Name = name_base + "_t0",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = u.name_show,
                    FontSize = u.name_size,
                    Foreground = u.name_color.toBrush(),
                };
                parts[0].Children.Add(t0);
                vm_assign(t0);

                if(u.content != null)
                {
                    WrapPanel w = draw_r_text(u.content);
                }

                if (u.effect_color != null)
                {
                    TextBlock t2 = new TextBlock
                    {
                        Name = name_base + "_t2",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = u.effect,
                        FontSize = u.effect_size,
                        Foreground = u.effect_color.toBrush(),
                        TextWrapping = TextWrapping.Wrap,
                    };
                    parts[2].Children.Add(t2);
                    vm_assign(t2);
                }
                
                if (u.cost_color != null)
                {
                    TextBlock t3 = new TextBlock
                    {
                        Name = name_base + "_t3",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = u.cost_size,
                        Foreground = u.cost_color.toBrush(),
                        TextWrapping = TextWrapping.Wrap,
                    };
                    parts[3].Children.Add(t3);
                    vm_assign(t3);
                }

                Rectangle cover = new Rectangle
                {
                    Name = name_base,
                    Width = u.width,
                    Height = u.height,
                    Fill = A(0, 0, 0, 0).toBrush(),
                };
                vm_set_lbtn(cover);
                Grid.SetRowSpan(cover, 100);
                content.Children.Add(cover);
                vm_assign(cover);

                Rectangle mask = new Rectangle
                {
                    Name = name_base + "_mask",
                    Width = u.width,
                    Height = u.height,
                    Fill = A(127, 255, 255, 0).toBrush(),
                    Visibility = Visibility.Hidden,
                };
                Grid.SetRowSpan(mask, 100);
                content.Children.Add(mask);
                vm_assign(mask);
            }

            TextBlock text0 = find_elem<TextBlock>(name_base + "_t0");
            WrapPanel text1 = null;
            TextBlock text2 = find_elem<TextBlock>(name_base + "_t2");
            TextBlock text3 = find_elem<TextBlock>(name_base + "_t3");

            rainbow_text r = u.content;
            if(r != null)
            {
                text1 = find_elem<WrapPanel>(r.target + "__" + r.name);
            }

            if (text0 != null)
            {
                text0.Text = u.name_show;
            }
            if (text1 != null)
            {
                draw_r_text(u.content);
            }
            if (text2 != null)
            {
                text2.Text = u.effect;
                text2.Foreground = u.effect_color.toBrush();
            }
            if (text3 != null)
            {
                string str = "已购买";
                if (u.level < u.max_level)
                {
                    str = "";
                    bool first = true;
                    List<Tuple<string, double2>> tuples = u.cost_table[u.level];
                    foreach (Tuple<string, double2> t in tuples)
                    {
                        if (!first)
                        {
                            str += ", ";
                        }
                        str += number_format(t.Item2) + " " + t.Item1;
                        first = false;
                    }
                }
                text3.Text = "价格：" + str;
            }

            Grid content_ = (Grid)vm_elems[name_base + "_grid"];
            content_.RowDefinitions[0].Height = new GridLength(u.name_weight, GridUnitType.Star);
            content_.RowDefinitions[1].Height = new GridLength(u.content_weight, GridUnitType.Star);
            content_.RowDefinitions[2].Height = new GridLength(u.effect_weight, GridUnitType.Star);
            content_.RowDefinitions[3].Height = new GridLength(u.cost_weight, GridUnitType.Star);
            if (u.attaching || u.visitable)
            {
                content_.Visibility = Visibility.Visible;
            }
            else
            {
                content_.Visibility = Visibility.Hidden;
            }
        }

        public void g1_mask_hidden(g1_upgrade u)
        {
            Rectangle mask = (Rectangle)vm_elems[u.name_base + "_mask"];
            mask.Visibility = Visibility.Hidden;
        }

        public void g1_mask_grey(g1_upgrade u)
        {
            Rectangle mask = (Rectangle)vm_elems[u.name_base + "_mask"];
            mask.Visibility = Visibility.Visible;
            mask.Fill = A(60, 0, 0, 0).toBrush();
        }

        public void g1_mask_yellow(g1_upgrade u)
        {
            Rectangle mask = (Rectangle)vm_elems[u.name_base + "_mask"];
            mask.Visibility = Visibility.Visible;
            mask.Fill = A(60, 255, 255, 0).toBrush();
        }

        public void g1_upgrade_check()
        {
            List<g1_upgrade> g1_Upgrades = g1_current_level.GetAllUpgrades();
            foreach (g1_upgrade u in g1_Upgrades)
            {
                if (u is g1_research)
                {
                    g1_research_check(u as g1_research);
                }
            }
            foreach (KeyValuePair<string, g1_upgrade> u in g1_ups)
            {
                if (u.Value is g1_milestone)
                {
                    g1_milestone_check(u.Value as g1_milestone);
                }
            }


            g1_layer layer = g1_current_layer;

            if (layer != null)
            {
                foreach(KeyValuePair<g1_upgrade, g1_tab> kp in layer.upgrades)
                {
                    g1_upgrade u = kp.Key;
                    g1_upgrade_update(u);
                    if (!kp.Value.Equals(layer.curr_tab) || 
                        u.page != layer.curr_tab.page_now)
                    {
                        continue;
                    }
                    if (u is g1_milestone)
                    {
                        continue;
                    }
                    if (u is g1_research)
                    {
                        continue;
                    }

                    if (u.level >= u.max_level)
                    {
                        g1_mask_yellow(u);
                    }
                    else
                    {
                        bool affordable = true;
                        if (u.special)
                        {
                            affordable = g1_special_check(u);
                        }
                        List<Tuple<string, double2>> tuples = u.cost_table[u.level];
                        foreach(Tuple<string, double2> t in tuples)
                        {
                            resource r = find_resource(t.Item1);
                            if (r.get_value() < t.Item2)
                            {
                                affordable = false;
                            }
                        }
                        if (affordable)
                        {
                            g1_mask_hidden(u);
                        }
                        else
                        {
                            g1_mask_grey(u);
                        }
                    }
                }
            }
        }

        public bool g1_special_check(g1_upgrade u)
        {
            if (u.name == "爱心宝石")
            {
                if (g1_crystal_rgb_max.Count != 1 || 
                    g1_crystal_rgb_max[0].name != "红色水晶")
                {
                    return false;
                }
            }
            if (u.name == "坚固叶片")
            {
                if (g1_crystal_rgb_max.Count != 1 ||
                    g1_crystal_rgb_max[0].name != "绿色水晶")
                {
                    return false;
                }
            }
            if (u.name == "精致时钟")
            {
                if (g1_crystal_rgb_max.Count != 1 ||
                    g1_crystal_rgb_max[0].name != "蓝色水晶")
                {
                    return false;
                }
            }
            if (u.name == "透亮圆盘")
            {
                if (g1_cal_A_2_art1_condition > 1.05)
                {
                    return false;
                }
            }
            if (u.name == "超大水晶")
            {
                if (g1_crystal_curr_max[0].get_value() * 1000 <= 
                    find_resource("水晶块").get_value())
                {
                    return false;
                }
            }
            if (u.name == "冰糖果冻")
            {
                if (g1_current_level.active_time - g1_cal_A_2_art3_condition > 120)
                {
                    return false;
                }
            }
            return true;
        }

        public void g1_milestone_check(g1_milestone ms)
        {
            if (ms.completed)
            {
                return;
            }
            resource r = find_resource(ms.get_auto_res());
            double2 n = ms.get_auto_value();

            ms.draw.masking = false;
            if (r.get_value() >= n)
            {
                ms.completed = true;
                ms.draw.masking = true;
                g1_upgrade_effect(ms, 1);
                //TODO 显示信息(通知)
            }

            double2 a = r.get_value();
            double2 b = n;
            if (ms.log_show)
            {
                a = (a + 1).Log10();
                b = (b + 1).Log10();
            }
            ms.draw.set_p_progress(a, b);
        }

        public void g1_research_check(g1_research research)
        {
            research.bar.draw.s_content.text = "研究进度：" +
                research.xp.current_exp + " / " + research.xp.exp_need_now;
            research.bar.draw.textblocks[0].content = research.name + ": 等级 " + 
                number_format(research.xp.level);
            research.bar.draw.textblocks[2].content = "已投入 " + number_format(research.see()) 
                + " 研究点数，点击将花费 " + number_format(research.will()) + " 研究点数";
            research.bar.draw.set_p_progress(research.xp.current_exp, research.xp.exp_need_now);
        }

        public void g1_upgrade_refresh(g1_layer layer)
        {
            foreach (KeyValuePair<g1_upgrade, g1_tab> kp in layer.upgrades)
            {
                foreach (KeyValuePair<g1_upgrade, int> n in kp.Key.nexts)
                {
                    if (kp.Key.level >= n.Value)
                    {
                        layer.unlock_upgrade(n.Key.name);
                    }
                }
            }
        }

        public void g1_upgrade_effect(g1_upgrade u, decimal level)
        {
            foreach (KeyValuePair<g1_upgrade, int> n in u.nexts)
            {
                if(level >= n.Value)
                {
                    u.g1_layer.unlock_upgrade(n.Key.name);
                }
            }

            //禁止编写里程碑效果↓
            if (g1_current_level.name == "世界")
            #region level 世界
            {
                #region 世界树
                if (u.name == "成长")
                {
                    switch (level)
                    {
                        case 1:
                            yggdrasill.exp_gain += 1;
                            u.effect = "目前效果：+1 世界树经验/s";
                            break;
                    }
                }
                if (u.name == "河流")
                {
                    switch (level)
                    {
                        case 1:
                            yggdrasill.exp_gain += 2;
                            g1_current_level.find_scenery("世界_河流").unlocked = true;
                            u.effect = "目前效果：+2 世界树经验/s";

                            find_resource("世界树等级").rev = true;
                            break;
                    }
                }
                if (u.name == "森林")
                {
                    switch (level)
                    {
                        case 1:
                            g1_current_level.find_layer("森林").unlocked = true;
                            u.effect = "目前效果：解锁森林层";
                            break;
                    }
                }
                if (u.name == "C代表什么")
                {
                    switch (level)
                    {
                        case 1:
                            g1_current_level.find_layer("文明").unlocked = true;
                            u.effect = "目前效果：解锁 C 层";
                            find_resource("世界树等级").rev = true;
                            break;
                    }
                }
                if (u.name == "虚假的可购买")
                {
                    switch (level)
                    {
                        case 1:
                            yggdrasill.exp_gain += 12;
                            u.effect = "目前效果：+12 世界树经验/s";
                            break;
                    }
                }

                #endregion 世界树
                #region 森林
                if (u.name == "世界树重置")
                {
                    resource wp = find_resource("世界点数");
                    resource r = find_resource("生命力");
                    r.add_value(g1_cal_life_point(), true);

                    u.level = 0;
                    u.check = false;
                    yggdrasill.reset();
                    g1_resource_syn();
                    g1_layer layer = g1_current_level.find_layer("世界树");
                    foreach(KeyValuePair<g1_upgrade, g1_tab> kp in layer.upgrades)
                    {
                        kp.Key.reset();
                    }
                    g1_upgrade_refresh(layer);

                    wp.set_value(wp.get_value() * 0.8);
                }
                if (u.name == "自然树")
                {
                    g1_current_level.find_layer("自然树").unlocked = true;
                    buy_upgrade_no_cost(g1_ups["世界_成长"], 1);
                    g1_ups["世界_成长"].can_reset = false;
                    yggdrasill.reseter.exp_gain += 1;
                }
                if (u.name == "水晶树")
                {
                    g1_current_level.find_layer("水晶树").unlocked = true;
                    buy_upgrade_no_cost(g1_ups["世界_河流"], 1);
                    g1_ups["世界_河流"].can_reset = false;
                    yggdrasill.reseter.exp_gain += 2;
                }
                #endregion 森林
                #region 自然树
                if (u.name == "自然树生命力")
                {
                    u.level = 0;
                    resource r = find_resource("生命力");
                    r.store_to_pool("自然树", r.get_value() * 0.1);
                }
                #endregion 自然树
                #region 水晶树
                if (u.name == "水晶树生命力")
                {
                    u.level = 0;
                    resource r = find_resource("生命力");
                    r.store_to_pool("水晶树", r.get_value() * 0.1);
                }
                if (u.name == "水晶树生命转化")
                {
                    u.level = 0;
                    resource r = find_resource("生命效果");
                    double2 effect = r.get_value() * 0.05;
                    r.add_value(-effect);
                    find_resource("生命转化").add_value(effect, true);

                    r = find_resource("绿色水晶");
                    if (r != null)
                    {
                        r.set_value(r.get_value() / 1e20);
                    }
                }
                #endregion 水晶树
                #region 合成树
                if (u.name == "合成树生命力")
                {
                    u.level = 0;
                    resource r = find_resource("生命力");
                    r.store_to_pool("合成树", r.get_value() * 0.1);
                }
                #endregion 合成树
                #region 文明
                if (u.name == "升级文明")
                {
                    switch (level)
                    {
                        case 1:
                            find_resource("文明水平").add_value(1, true);
                            u.effect = "当前Civilisation规模： Clan\n" +
                                       "下一个Civilisation规模： Countryside";
                            //TODO 显示下一个rainbow text
                            break;
                    }
                }
                if (u.name == "研究重置")
                {
                    u.level = 0;
                    foreach(KeyValuePair<g1_upgrade, g1_tab> kp in g1_current_layer.upgrades)
                    {
                        if(kp.Key is g1_research)
                        {
                            g1_research rese = kp.Key as g1_research;
                            rese.reset();
                        }
                    }
                }
                if (u.name == "房屋")
                {
                    g1_current_level.find_scenery("世界_房屋").unlocked = true;
                }
                if (u.name == "桥")
                {
                    g1_layer layer = g1_layers["文明"];
                    layer.line_thickness = 15;
                }

                #endregion 文明
            }
            #endregion level 世界
            if (g1_current_level.name == "世界树")
            #region level 世界树
            {
                #region 0
                if (u.name == "回收迷宫元素")
                {
                    u.level = 0;
                    find_resource("已用迷宫元素").add_value(1, true);
                    find_resource("迷宫元素").add_value(
                        find_resource("已用迷宫元素").get_value(), true);
                    find_resource("已用迷宫元素").set_value(0);
                    //重置此上的所有层
                    foreach (g1_layer layer in g1_current_level.getAllLayers())
                    {
                        if (layer.name != "世界树0层")
                        {
                            layer.reset();
                        }
                    }
                }
                if (u.name == "制造迷宫元素")
                {
                    u.level = 0;
                    find_resource("迷宫元素").add_value(1);
                    u.cost_table[0][0] = new Tuple<string, double2>(
                        u.cost_table[0][0].Item1, u.cost_table[0][0].Item2 * 5);
                }
                if (u.name == "世界的基础")
                {
                    find_resource("0层点数").add_value(1, true);
                    u.effect = "目前效果：+0.01/s 0层点数";
                }
                #endregion 0
                #region 1
                if (u.name == "闪亮水晶")
                {
                    find_resource("世界树等级").rev = true;
                }
                #endregion 1
            }
            #endregion level 世界树
            if (g1_current_level.name == "自然树")
            #region level 自然树
            {
                #region 种子
                if (u.name == "播种")
                {
                    u.level = 0;
                    resource r = find_resource("保存的种子");
                    resource r2 = find_resource("种子");
                    r2.add_value(r.get_value(), true);
                    r.set_value(0);
                }
                if (u.name == "森林之歌")
                {
                    apply_mul(seed.produce_muls, "森林之歌", 3);
                    apply_mul(sapling.produce_muls, "森林之歌", 3);
                    u.effect = "目前效果：×3 自然点数产量";
                }
                if (u.name == "浓缩")
                {
                    seed.exp_base_gain += 9;
                    u.effect = "目前效果：+9 生长进度/s";
                    find_resource("种子成长度").rev = true;
                }
                if (u.name == "返老还童术")
                {
                    seed.exp_base_gain += 15;
                    apply_mul(seed.exp_gain_multipliers, "返老还童术", 2);
                    apply_mul(sapling.exp_gain_multipliers, "返老还童术", 2);
                    apply_mul(smalltree.exp_gain_multipliers, "返老还童术", 2);
                    u.effect = "目前效果：+15 生长进度/s，双倍生长速度";
                    find_resource("种子成长度").rev = true;
                }
                if (u.name == "不想长大")
                {
                    seed.exp_base_gain += 475;
                    apply_mul(seed.exp_gain_multipliers, "不想长大", 1.5);
                    apply_mul(sapling.exp_gain_multipliers, "不想长大", 1.5);
                    apply_mul(smalltree.exp_gain_multipliers, "不想长大", 1.5);
                    u.effect = "目前效果：+475 生长进度/s，×1.5 生长速度";
                    find_resource("种子成长度").rev = true;
                }
                if (u.name == "过生长")
                {
                    apply_mul(seed.exp_gain_multipliers, "过生长", 2);
                    apply_mul(sapling.exp_gain_multipliers, "过生长", 2);
                    apply_mul(smalltree.exp_gain_multipliers, "过生长", 2);
                    u.effect = "目前效果：×2 生长速度";
                    find_resource("种子成长度").rev = true;
                }
                #endregion 种子
                #region 树苗
                if (u.name == "种子树苗")
                {
                    u.level = 0;
                    resource r = find_resource("种子");
                    resource r2 = find_resource("树苗");
                    r2.add_value(r.get_value(), true);
                    r.set_value(0);
                }
                if (u.name == "光合作用")
                {
                    apply_mul(sapling.exp_gain_multipliers, "光合作用", 5);
                    apply_mul(smalltree.exp_gain_multipliers, "光合作用", 5);
                    u.effect = "目前效果：×5 生长速度";
                    find_resource("树苗成长度").rev = true;
                }
                if (u.name == "吸水")
                {
                    sapling.exp_base_gain += 9900;
                    u.effect = "目前效果：+9900 生长进度/s，×3 水获取";
                    find_resource("树苗成长度").rev = true;
                }
                if (u.name == "营养供给")
                {
                    find_resource("树苗成长度").rev = true;
                }
                if (u.name == "巨型树苗")
                {
                    sapling.exp_base_gain += 2.99e6;
                    u.effect = "目前效果：已生效，见上方描述";
                    sapling.produce_mul.apply(u.name, 2, true);
                    apply_mul(sapling.exp_gain_multipliers, u.name, 2);
                    apply_mul(smalltree.exp_gain_multipliers, u.name, 2);
                    find_resource("树苗成长度").rev = true;
                }
                #endregion 树苗
                #region 小树
                if (u.name == "树苗小树")
                {
                    u.level = 0;
                    resource r = find_resource("树苗");
                    resource r2 = find_resource("小树");
                    r2.add_value(r.get_value(), true);
                    r.set_value(0);
                }
                #endregion 小树
                #region 水
                if (u.name == "水源")
                {
                    u.effect = "目前效果：+1 水/s";
                }
                if (u.name == "营养液")
                {
                    g1_current_level.find_layer("营养").unlocked = true;
                }
                #endregion 水
                #region 营养
                if (u.name == "矿物质")
                {
                    u.effect = "目前效果：+0.1 营养/s";
                }
                #endregion 营养
            }
            #endregion level 自然树
            if (g1_current_level.name == "水晶树")
            #region level 水晶树
            {
                #region A
                if (u.name == "不只是艺术品")
                {
                    g1_layer l = u.g1_layer;
                    g1_tab t = l.tabs["水晶聚合处_艺术品"];
                    t.unlocked = true;
                }
                if (u.name == "超大水晶切换")
                {
                    u.level = 0;
                    g1_next_crystal(ref g1_cal_A_2_art2_select);
                }
                if (u.tab.name == "水晶聚合处_艺术品")
                {
                    u.level = 0;
                    resource crystal = find_resource("水晶块");
                    double2 old = crystal.get_value();
                    crystal.store_to_pool(u.name, double2.max);
                    foreach (g1_layer layer in g1_current_level.getAllLayers())
                    {
                        if (layer.name.Contains("色水晶"))
                        {
                            layer.reset();
                        }
                    }

                    if (g1_levels["水晶树"].difficulty == g1_level.type.easy)
                    {

                    }
                    else
                    {
                        g1_upgrade x = g1_ups["水晶树_黄金比例"];
                        x.cost_table[0][0] = new Tuple<string, double2>("白色水晶", 1e15);
                        x = g1_ups["水晶树_记忆晶石"];
                        x.cost_table[0][0] = new Tuple<string, double2>("白色水晶", 1e15);
                        x = g1_ups["水晶树_超级转换"];
                        x.cost_table[0][0] = new Tuple<string, double2>("白色水晶", 1e15);
                    }

                    g1_cal_A_2_art3_condition = g1_current_level.active_time;
                    crystal.set_value(double2.Pow(old, g1_cal_A_2_art3_effect(false)) - 1);
                }
                #endregion A
                #region R
                #endregion R
                #region G
                if (u.name == "生命混合")
                {
                    double2 gain = g1_cal_G_lifemix_gain();
                    resource lifep = find_resource("生命力");
                    lifep.store_to_pool("水晶树", 0);
                    lifep.pools["水晶树"] += gain;
                    u.acc_time *= 0.99;
                }
                #endregion G
                #region B
                if (u.name == "环境加成")
                {
                }
                if (u.name == "稳定结晶")
                {
                    u.effect = "目前效果：×3 蓝色水晶生成力";
                }
                #endregion B
                #region F
                if (u.name.Contains("粉碎") && u.name.Contains("色水晶"))
                {
                    u.level = 0;
                    string crystal_name = Regex.Split(u.name, "粉碎")[1];
                    resource crystal = find_resource(crystal_name);
                    resource crystal_m = find_resource(crystal_name + "原料");
                    double2 amount = crystal.get_value() * 0.5;
                    crystal.add_value(-amount);
                    crystal_m.add_value(amount);

                    resource crystal_b = find_resource("水晶块");
                    crystal_b.set_value(crystal_b.get_value() * 0.5);
                }
                if (u.name == "红水晶镐")
                {
                    u.level = 0;
                    u.acc_time = 0;
                    u.count++;
                    if (u.check)
                    {
                        g1_upgrade x = g1_ups["水晶树_绿水晶探测器"];
                        if (x.check)
                        {
                            x.first_expensive(1000);
                        }
                        x = g1_ups["水晶树_蓝水晶转盘"];
                        if (x.check)
                        {
                            x.first_expensive(1000);
                        }
                    }
                    u.check = false;
                    u.accing = true;
                    u.first_expensive(2);
                }
                if (u.name == "绿水晶探测器")
                {
                    u.level = 0;
                    u.count++;
                    if (u.check)
                    {
                        g1_upgrade x = g1_ups["水晶树_红水晶镐"];
                        if (x.check)
                        {
                            x.first_expensive(1000);
                        }
                        x = g1_ups["水晶树_蓝水晶转盘"];
                        if (x.check)
                        {
                            x.first_expensive(1000);
                        }
                    }
                    u.check = false;
                    u.first_expensive(5);
                }
                if (u.name == "蓝水晶转盘")
                {
                    Random a = new Random();
                    u.level = 0;
                    u.acc_time = a.NextDouble();
                    u.acc_time2 = a.NextDouble();
                    u.count++;
                    if (u.check)
                    {
                        g1_upgrade x = g1_ups["水晶树_红水晶镐"];
                        if (x.check)
                        {
                            x.first_expensive(1000);
                        }
                        x = g1_ups["水晶树_绿水晶探测器"];
                        if (x.check)
                        {
                            x.first_expensive(1000);
                        }
                    }
                    u.check = false;
                    u.first_expensive(15);
                }
                if (u.name == "红色斧头")
                {
                    u.level = 0;
                    u.acc_time = 0;
                    u.count++;
                    if (u.check)
                    {
                        g1_upgrade x = g1_ups["水晶树_水晶回收器"];
                        if (x.check)
                        {
                            x.first_expensive(1e10);
                        }
                        x = g1_ups["水晶树_海浪宝石"];
                        if (x.check)
                        {
                            x.first_expensive(1e10);
                        }
                    }
                    u.check = false;
                    u.accing = true;
                    u.first_expensive(50);
                }
                if (u.name == "水晶回收器")
                {
                    u.level = 0;
                    u.count++;
                    if (u.check)
                    {
                        g1_upgrade x = g1_ups["水晶树_红色斧头"];
                        if (x.check)
                        {
                            x.first_expensive(1e10);
                        }
                        x = g1_ups["水晶树_海浪宝石"];
                        if (x.check)
                        {
                            x.first_expensive(1e10);
                        }
                    }
                    u.check = false;
                    u.first_expensive(200);
                }
                if (u.name == "海浪宝石")
                {
                    u.level = 0;
                    u.acc_time = 1;
                    u.count++;
                    if (u.check)
                    {
                        g1_upgrade x = g1_ups["水晶树_红色斧头"];
                        if (x.check)
                        {
                            x.first_expensive(1e10);
                        }
                        x = g1_ups["水晶树_水晶回收器"];
                        if (x.check)
                        {
                            x.first_expensive(1e10);
                        }
                    }
                    u.check = false;
                    u.accing = true;
                    u.first_expensive(1000);
                }
                #endregion F
                #region Y
                if (u.name == "转化黄色水晶")
                {
                    u.level = 0;
                    resource r1 = find_resource("红色水晶");
                    resource r2 = find_resource("绿色水晶");
                    resource x = find_resource("黄色水晶");
                    x.add_value(g1_cal_Y_convert(), true);
                    r1.set_value(0);
                    r2.set_value(0);
                }
                if (u.name == "黄色水晶控制")
                {
                    u.level = 0;
                    u.valid = !u.valid;
                }
                if (u.name == "黄金比例")
                {
                    g1_upgrade x = g1_ups["水晶树_记忆晶石"];
                    x.first_expensive(1e6);
                    x = g1_ups["水晶树_超级转换"];
                    x.first_expensive(1e6);
                }
                #endregion Y
                #region M
                if (u.name == "转化洋红色水晶")
                {
                    u.level = 0;
                    resource r1 = find_resource("红色水晶");
                    resource r2 = find_resource("蓝色水晶");
                    resource x = find_resource("洋红色水晶");
                    x.add_value(g1_cal_M_convert(), true);
                    r1.set_value(0);
                    r2.set_value(0);

                    double2 time = g1_cal_M_remain_time();
                    if (g1_ups["水晶树_记忆晶石"].level >= 1)
                    {
                        time *= 0.8;
                    }
                    else
                    {
                        time = 0;
                    }
                    g1_cal_M_time = g1_levels["水晶树"].in_game_time + time;
                }
                if (u.name == "记忆晶石")
                {
                    g1_upgrade x = g1_ups["水晶树_黄金比例"];
                    x.first_expensive(1e6);
                    x = g1_ups["水晶树_超级转换"];
                    x.first_expensive(1e6);
                }
                #endregion M
                #region C
                if (u.name == "转化青色水晶")
                {
                    u.level = 0;
                    resource r1 = find_resource("绿色水晶");
                    resource r2 = find_resource("蓝色水晶");
                    resource x = find_resource("青色水晶");
                    x.add_value(g1_cal_C_convert(), true);
                    r1.set_value(0);
                    r2.set_value(0);
                }
                if (u.name == "超级转换")
                {
                    g1_upgrade x = g1_ups["水晶树_黄金比例"];
                    x.first_expensive(1e6);
                    x = g1_ups["水晶树_记忆晶石"];
                    x.first_expensive(1e6);
                }
                #endregion C
                #region W
                if (u.name == "转化白色水晶")
                {
                    u.level = 0;
                    resource r1 = find_resource("黄色水晶");
                    resource r2 = find_resource("洋红色水晶");
                    resource r3 = find_resource("青色水晶");
                    resource x = find_resource("白色水晶");
                    x.add_value(g1_cal_W_convert(), true);
                    r1.set_value(2);
                    r2.set_value(2);
                    r3.set_value(2);
                }
                #endregion W
            }
            #endregion level 水晶树

            //里程碑！！
            #region 里程碑
            //仅编写一次性效果即可
            #region 世界 世界树
            if (u.name == "世界树_里程碑_1")
            {
                g1_ups["世界_世界树重置"].visitable = true;
            }
            if (u.name == "世界树_里程碑_2")
            {
                g1_layer layer = g1_layers["世界树"];
                layer.find_drawable("世界树_slot").v = Visibility.Visible;
            }
            #endregion 世界 世界树
            #region 世界 森林
            if (u.name == "森林_里程碑_1")
            {
                find_resource("生命效果").add_value(0.05, true);
            }
            #endregion 世界 森林
            #region 自然树 种子
            if (u.name == "种子_里程碑_1")
            {
                g1_levels["自然树"].find_layer("树苗").unlocked = true;
            }
            #endregion 自然树 种子
            #region 自然树 树苗
            if (u.name == "树苗_里程碑_1")
            {
                g1_levels["自然树"].find_layer("小树").unlocked = true;
            }
            #endregion 自然树 树苗
            #endregion 里程碑

            g1_resource_syn();
        }

        public void g1_upgrade_update(g1_upgrade u)
        {
            if (g1_current_level.name == "世界")
            #region level 世界
            {
                if (u.name == "世界树重置")
                {
                    u.effect = "当前：+" + g1_cal_life_point() + " 生命力";
                }
                #region 自然树
                if (u.name == "自然树生命力")
                {
                    resource r = find_resource("生命力");
                    u.effect = "当前：已投入 " + g1_cal_生命力_自然树().Item2 + " 生命力\n" +
                        "（将投入 " + r.get_value() * 0.1 + " 生命力）";
                }
                if (u.name == "生态循环")
                {
                    u.effect = "效果：+(^ " + number_format(g1_cal_npower_eco_effect()) +")/s 生命力";
                }
                if (u.name == "自然循环")
                {
                    u.effect = "效果：×" + number_format(g1_cal_npower_npoint_effect()) + " 自然点数";
                }
                #endregion 自然树
                #region 水晶树
                if (u.name == "水晶树生命力")
                {
                    resource r = find_resource("生命力");
                    u.effect = "当前：已投入 " + g1_cal_生命力_水晶树().Item2 + " 生命力\n" +
                        "（将投入 " + r.get_value() * 0.1 + " 生命力）";
                }
                if (u.name == "水晶树生命转化")
                {
                    resource r = find_resource("生命效果");
                    u.effect = "当前：将置换 " + r.get_value() * 0.05 + " 生命效果";
                }
                if (u.name == "折射")
                {
                    u.effect = "当前：初始水晶块 = " + number_format(
                        g1_cal_refraction_effect());
                }
                #endregion 水晶树
                #region 合成树
                if (u.name == "合成树生命力")
                {
                    resource r = find_resource("生命力");
                    u.effect = "当前：已投入 " + g1_cal_生命力_合成树().Item2 + " 生命力\n" +
                        "（将投入 " + r.get_value() * 0.1 + " 生命力）";
                }
                #endregion 合成树
                #region 文明
                if (u.name == "研究重置")
                {
                    u.name_show = "重置所有研究等级，回收 " +
                        number_format(find_resource("研究点数").all_in_pool()) +" 研究点数";
                }
                if (u.name == "经验总结")
                {
                    u.effect = "效果：+" + number_format(g1_cal_exp_summary_gain(), true) + "/s 文明水平";
                }
                if (u.name == "房屋")
                {
                    u.effect = "效果：×" + number_format(g1_cal_life_cp_syn(), true) + 
                        " 文明水平和生命力获取";
                }
                if (u.name == "桥")
                {
                    g1_layer layer = g1_layers["文明"];
                    u.effect = "效果：×" + number_format(g1_cal_ylv_to_cp_syn(), true) +
                        " 文明水平获取，×" + number_format(g1_cal_cp_to_yxp_syn(), true) +
                        " 世界树经验获取";
                }
                if (u.name == "食物学")
                {
                    g1_research r = u as g1_research;
                    r.bar.draw.textblocks[1].content =
                        "效果: 食物的能量效果×" + number_format(g1_cal_research_food_food_gain()) +
                        ", 文明水平获取 +" + number_format(g1_cal_research_food_cp_gain()) + "倍";
                }
                if (u.name == "战斗学")
                {
                    g1_research r = u as g1_research;
                    r.bar.draw.textblocks[1].content =
                        "效果: 战斗的攻击力×" + number_format(g1_cal_research_battle_attack_gain()) +
                        ", 文明水平获取 +" + number_format(g1_cal_research_battle_cp_gain()) + "倍";
                }
                if (u.name == "语言与文字")
                {
                    g1_research r = u as g1_research;
                    r.bar.draw.textblocks[1].content =
                        "效果: 文明水平获取 +" + number_format(g1_cal_research_language_cp_gain()) + "倍";
                }
                if (u.name == "水源工程")
                {
                    g1_research r = u as g1_research;
                    r.bar.draw.textblocks[1].content =
                        "效果: 药水配制速度×" + number_format(g1_cal_research_water_potion_gain()) +
                        "，水获取×" + number_format(g1_cal_research_water_water_gain()) +
                        "，文明水平获取 +" + number_format(g1_cal_research_water_cp_gain()) + "倍";
                }
                if (u.name == "林业")
                {
                    g1_research r = u as g1_research;
                    r.bar.draw.textblocks[1].content =
                        "效果: 木头方块获取×" + number_format(g1_cal_research_wood_wood_gain()) +
                        "，生命力获取×" + number_format(g1_cal_research_wood_life_gain()) +
                        "，文明水平获取 +" + number_format(g1_cal_research_wood_cp_gain()) + "倍";
                }
                #endregion 文明
            }
            #endregion level 世界

            if (g1_current_level.name == "世界树")
            #region level 世界树
            {
                /*
                if (u.name_show == "打开本层！")
                {
                    string r_name = Regex.Split(u.name, "打开")[1];
                    r_name = Regex.Split(u.name, "层")[0];
                    int n = Convert.ToInt32(r_name);


                }*/
                #region 0
                if (u.name == "回收迷宫元素")
                {
                    u.effect = "将回收 " + number_format(find_resource("已用迷宫元素").
                        get_value()) + " 迷宫元素";
                }
                if (u.name == "迷宫元素沉积")
                {
                    u.effect = "效果：×" + number_format(g1_cal_maze_0_effect()) + 
                        " 上一个升级的效果";
                }
                #endregion 0
                #region 1
                if (u.name == "闪亮水晶")
                {
                    u.effect = "效果：×10 1层点数获取，+"
                        + g1_cal_1_crystal_effect() + "/s 水晶质量获取";
                }
                #endregion 1
            }
            #endregion level 世界树
            if (g1_current_level.name == "自然树")
            #region level 自然树
            {
                #region 树苗
                if (u.name == "营养供给")
                {
                    u.effect = "效果：×" + number_format(g1_cal_s2_nu_effect()) +
                        " 营养获取速度";
                }

                #endregion 树苗
                #region 水
                if (u.name == "水分保持")
                {
                    u.effect = "效果：×" + number_format(g1_cal_water_np_mul()) +
                        " 水获取速度";
                }
                if (u.name == "汇聚")
                {
                    u.effect = "效果：×" + number_format(g1_cal_water_water_mul()) +
                        " 水获取速度";
                }
                if (u.name == "营养液")
                {
                    u.effect = "效果：×" + number_format(g1_cal_water_from_tree_mul()) +
                        " 水获取速度";
                }
                if (u.name == "非常水的升级")
                {
                    u.effect = "效果：×2 水获取速度，+" + 
                        number_format(g1_cal_water_nu_gain) + "营养/s";
                }
                #endregion 水

                #region 营养
                if (u.name == "光照")
                {
                    u.effect = "效果：×" + number_format(g1_cal_nu_light_effect()) +
                        " 营养获取速度";
                }
                if (u.name == "肥料")
                {
                    u.effect = "效果：×" + number_format(g1_cal_nu_fat_effect()) +
                        " 营养获取速度";
                }
                if (u.name == "气流")
                {
                    u.effect = "效果：×" + number_format(g1_cal_nu_air_effect()) +
                        " 营养获取速度";
                }
                #endregion 营养
            }
            #endregion level 自然树
            if (g1_current_level.name == "水晶树")
            #region level 水晶树
            {
                #region A
                resource crystal = find_resource("水晶块");

                if (u.name == "超大水晶切换")
                {
                    resource cur = g1_cal_A_2_art2_select;
                    u.effect = "目前：×" + number_format(g1_cal_A_2_art2_effect(false)) + " " 
                        + cur.name + "获取";
                    u.effect_color = A(cur.r, cur.g, cur.b);
                }


                if (u.name == "爱心宝石")
                {
                    u.effect = "艺术品价值：" + number_format(crystal.see_pool(u.name) * art_mul) + 
                               " (+" + number_format(crystal.get_value() * art_mul) + ")\n" +
                               "效果：×" + number_format(g1_cal_R_art1_effect(false)) +
                               " (×" + number_format(g1_cal_R_art1_effect(true)) + 
                               ") 生命转化效果底数";
                }
                if (u.name == "坚固叶片")
                {
                    u.effect = "艺术品价值：" + number_format(crystal.see_pool(u.name) * art_mul) +
                               " (+" + number_format(crystal.get_value() * art_mul) + ")\n" +
                               "效果：×" + number_format(g1_cal_G_art1_effect(false)) +
                               " (×" + number_format(g1_cal_G_art1_effect(true)) +
                               ") 水晶生成力";
                }
                if (u.name == "精致时钟")
                {
                    u.effect = "艺术品价值：" + number_format(crystal.see_pool(u.name) * art_mul) +
                               " (+" + number_format(crystal.get_value() * art_mul) + ")\n" +
                               "效果：×" + number_format(g1_cal_B_art1_effect(false)) +
                               " (×" + number_format(g1_cal_B_art1_effect(true)) +
                               ") 水晶块获取速度";
                }
                if (u.name == "透亮圆盘")
                {
                    u.effect = "艺术品价值：" + number_format(crystal.see_pool(u.name) * art_mul) +
                               " (+" + number_format(crystal.get_value() * art_mul) + ")" +
                               "[条件为 ^ " + number_format(g1_cal_A_2_art1_condition) + "]\n" +
                               "效果：×" + number_format(g1_cal_A_2_art1_effect1(false)) +
                               "(×" + number_format(g1_cal_A_2_art1_effect1(true)) +
                               ")" + g1_crystal_curr_min[0].name +
                               "获取，/" + number_format(g1_cal_A_2_art1_effect2(false)) +
                               "(/" + number_format(g1_cal_A_2_art1_effect2(true)) +
                               ")水晶块获取";
                }
                if (u.name == "超大水晶")
                {
                    u.effect = "艺术品价值：" + number_format(crystal.see_pool(u.name)) +
                               " (+" + number_format(crystal.get_value()) + ")\n" +
                               "效果：×" + number_format(g1_cal_A_2_art2_effect(false)) +
                               " (×" + number_format(g1_cal_A_2_art2_effect(true)) +
                               ")选择的水晶获取";
                }
                if (u.name == "冰糖果冻")
                {
                    u.effect = "艺术品价值：" + number_format(crystal.see_pool(u.name)) +
                               " (+" + number_format(crystal.get_value()) + ")\n" +
                               "效果：保留 ^ " + number_format(g1_cal_A_2_art3_effect(false)) +
                               " (^ " + number_format(g1_cal_A_2_art3_effect(true)) + ")的水晶块";
                }
                #endregion A
                #region R
                if (u.name == "平静变化")
                {
                    if (g1_crystal_rgb_min[0].name == "红色水晶")
                    {
                        u.effect = "效果：×" + number_format(g1_cal_R_balance_1_effect())
                            + " 红色水晶生成力";
                    }
                    else
                    {
                        u.effect = "效果：×" + number_format(g1_cal_R_balance_2_effect())
                            + " [蓝色/绿色]水晶生成力";
                    }
                }
                if (u.name == "循环转化")
                {
                    if (g1_cal_R_cconvert_temp.Item1)
                    {
                        u.effect = "效果：×" + number_format(g1_cal_R_cconvert_temp.Item2)
                            + " 水晶块获取";
                    }
                    else
                    {
                        u.effect = "效果：×" + number_format(g1_cal_R_cconvert_temp.Item2)
                            + " 红色水晶生成力";
                    }
                }
                #endregion R
                #region G
                if (u.name == "吸收与利用")
                {
                    u.effect = "效果：×" + number_format(g1_cal_G_absorb_effect())
                        + " 绿色水晶生成力";
                }
                if (u.name == "生命混合")
                {
                    u.effect = "效果：×" + number_format(g1_cal_G_lifemix_effect())
                        + " 水晶块获取，下一次购买 +(^ " + number_format(g1_ups["水晶树_生命混合"].acc_time) 
                        + ")生命力\n" 
                        + "[+" + g1_cal_G_lifemix_gain() + " 生命力]";
                }
                #endregion G
                #region B
                if (u.name == "环境加成")
                {
                    u.effect = "效果：×" + number_format(g1_cal_B_duplicate_effect())
                        +" 蓝色水晶生成力";
                }
                #endregion B
                #region F
                if (u.name.Contains("粉碎") && u.name.Contains("色水晶"))
                {
                    string crystal_name = Regex.Split(u.name, "粉碎")[1];
                    resource crystal_ = find_resource(crystal_name);
                    double2 amount_ = crystal_.get_value() * 0.5;
                    
                    double2 amount = crystal.get_value() * 0.5;

                    u.effect = "将粉碎 " + number_format(amount_) + 
                        " " + crystal_name + "，摧毁 " + number_format(amount) + " 水晶块";
                }
                if (u.name == "红水晶镐")
                {
                    u.effect = "效果：[×" + number_format(g1_cal_F_Rpickaxe_effect()) +
                        " / ×" + number_format(100 * double2.Max(1,
                        double2.Pow(g1_ups["水晶树_红水晶镐"].count, 0.35))) + "] 水晶生成力";
                }
                if (u.name == "绿水晶探测器")
                {
                    u.effect = "效果：[×" + number_format(g1_cal_F_Gscanner_effect()) +
                        " / ×" + number_format(100 * double2.Max(1, 
                        double2.Pow(g1_ups["水晶树_绿水晶探测器"].count, 0.5))) + "] 水晶生成力";
                }
                if (u.name == "蓝水晶转盘")
                {
                    u.effect = "效果：[×" + number_format(g1_cal_F_Bspin_effect()) +
                           " / ×" + number_format(100 * double2.Max(1,
                           double2.Pow(g1_ups["水晶树_蓝水晶转盘"].count, 0.65))) + "] 水晶生成力";
                }
                if (u.name == "红色斧头")
                {
                    u.effect = "效果：[×" + number_format(g1_cal_F_Raxe_effect()) +
                        " / ×" + number_format(1205 * double2.Max(1,
                        double2.Pow(g1_ups["水晶树_红色斧头"].count, 0.8))) + "] 艺术品价值";
                }
                if (u.name == "水晶回收器")
                {
                    u.effect = "效果：[×" + number_format(g1_cal_F_Grecycler_effect()) +
                        " / ×" + number_format(666 * double2.Max(1,
                        double2.Pow(g1_ups["水晶树_水晶回收器"].count, 0.9))) + "] 水晶块获取";
                }
                if (u.name == "海浪宝石")
                {
                    u.effect = "效果：[×" + number_format(g1_cal_F_Bsea_effect()) +
                           " / ×" + number_format(100 * double2.Max(1,
                           g1_ups["水晶树_海浪宝石"].count)) + "] 时间流速";
                }
                #endregion F
                #region Y
                if (u.name == "转化黄色水晶")
                {
                    u.effect = "将获取 " + number_format(g1_cal_Y_convert()) + " 黄色水晶";
                }
                if (u.name == "黄色水晶控制")
                {
                    resource r = g1_crystal_all_max[0];
                    if (r.Equals(none))
                    {
                        r = find_resource("红色水晶");
                    }
                    string s = "";
                    if (u.valid)
                    {
                        s = "有效";
                    }
                    else
                    {
                        s = "无效";
                    }
                    u.effect = "效果： ×"  + g1_cal_Y_effect() +
                        " " +  r.name + "获取（" + s + "）";
                }
                #endregion Y
                #region M
                if (u.name == "转化洋红色水晶")
                {
                    u.effect = "将获取 " + number_format(g1_cal_M_convert()) + " 洋红色水晶";
                }
                #endregion M
                #region C
                if (u.name == "转化青色水晶")
                {
                    u.effect = "将获取 " + number_format(g1_cal_C_convert()) + " 青色水晶";
                }
                #endregion C
                #region W
                if (u.name == "转化白色水晶")
                {
                    u.effect = "将获取 " + number_format(g1_cal_W_convert()) + " 白色水晶";
                }
                #endregion W
            }
            #endregion level 水晶树
        }

        public void yggdrasill_change_stage(int ds)
        {
            g1_level level = g1_levels["世界树"];
            g1_layer layer = null;
            int stage = level.stage;
            if (ds != 0)
            {
                if (ds > 0)
                {
                    for(int i = stage + 1; i <= stage + ds; i++)
                    {
                        layer = g1_layers["世界树" + i + "层"];
                        layer.unlocked = true;
                    }
                }
                if (ds < 0)
                {
                    for (int i = stage - 1; i >= stage + ds; i--)
                    {
                        layer = g1_layers["世界树" + i + "层"];
                        layer.unlocked = false;
                    }
                }
                level.stage += ds;
                for(int i = 0; i <= level.stage_max; i++)
                {
                    layer = g1_layers["世界树" + i + "层"];
                    layer.c_position.Y += ds * 40;
                }

                if (g1_current_level.Equals(level))
                {
                    g1_map_redraw(1,
                        Math.Max(1, (3.6 + level.stage * 0.4) / 4.5));
                    g1_level_draw_base(level.name);
                }
            }
        }
    }
}
