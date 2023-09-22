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
        [NonSerialized]
        Dictionary<string, List<g2_cell.type>> g2_cell_types = new Dictionary<string, List<g2_cell.type>>();
        private void g2_level_init()
        {
            g2_gamestart = false;


            levels = new Dictionary<string, g2_area>();
            g2_links = new Dictionary<string, g2_level_link>();
            g2_cell_types = new Dictionary<string, List<g2_cell.type>>();

            List<g2_cell.type> p;

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.empty);
            g2_cell_types.Add("empty", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.wall);
            g2_cell_types.Add("wall", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.super_wall);
            g2_cell_types.Add("super wall", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.wall);
            p.Add(g2_cell.type.start);
            p.Add(g2_cell.type.oblique);
            g2_cell_types.Add("start", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.key);
            g2_cell_types.Add("key", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.wall);
            p.Add(g2_cell.type.door);
            g2_cell_types.Add("door", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.key);
            p.Add(g2_cell.type.wall);
            p.Add(g2_cell.type.door);
            g2_cell_types.Add("keydoor", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.key);
            p.Add(g2_cell.type.x);
            g2_cell_types.Add("x key", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.wall);
            p.Add(g2_cell.type.door);
            p.Add(g2_cell.type.x);
            g2_cell_types.Add("x door", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.key);
            p.Add(g2_cell.type.wall);
            p.Add(g2_cell.type.door);
            p.Add(g2_cell.type.x);
            g2_cell_types.Add("x keydoor", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.x);
            g2_cell_types.Add("x", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.wall);
            p.Add(g2_cell.type.oblique);
            p.Add(g2_cell.type.in_out);
            g2_cell_types.Add("in out", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.end);
            g2_cell_types.Add("end", p);

            p = new List<g2_cell.type>();
            p.Add(g2_cell.type.reflecter);
            g2_cell_types.Add("reflecter", p);

            Grid g2_classic_level_map = (Grid)vm_elems["g2_classic_level_map"];

            string level_name = "g2_classic_level_1";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }

            level_name = "g2_classic_level_2";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_1"], infs["g2_classic_level_2"], g2_classic_level_map);

            level_name = "g2_classic_level_3";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_2"], infs["g2_classic_level_3"], g2_classic_level_map);

            level_name = "g2_classic_level_4";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_3"], infs["g2_classic_level_4"], g2_classic_level_map);

            level_name = "g2_classic_level_5";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_3"], infs["g2_classic_level_5"], g2_classic_level_map);

            level_name = "g2_classic_level_6";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_3"], infs["g2_classic_level_6"], g2_classic_level_map);

            level_name = "g2_classic_level_7";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_5"], infs["g2_classic_level_7"], g2_classic_level_map);

            level_name = "g2_classic_level_8";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_4"], infs["g2_classic_level_8"], g2_classic_level_map);

            level_name = "g2_classic_level_9";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_6"], infs["g2_classic_level_9"], g2_classic_level_map);

            level_name = "g2_classic_level_10";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_9"], infs["g2_classic_level_10"], g2_classic_level_map);

            level_name = "g2_classic_level_11";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_8"], infs["g2_classic_level_11"], g2_classic_level_map);
            g2_link_create(infs["g2_classic_level_9"], infs["g2_classic_level_11"], g2_classic_level_map);

            level_name = "g2_classic_level_12";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_7"], infs["g2_classic_level_12"], g2_classic_level_map);
            g2_link_create(infs["g2_classic_level_10"], infs["g2_classic_level_12"], g2_classic_level_map);

            level_name = "g2_classic_level_13";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_10"], infs["g2_classic_level_13"], g2_classic_level_map);

            level_name = "g2_classic_level_14";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_11"], infs["g2_classic_level_14"], g2_classic_level_map);
            g2_link_create(infs["g2_classic_level_12"], infs["g2_classic_level_14"], g2_classic_level_map);

            level_name = "g2_classic_level_15";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }
            g2_link_create(infs["g2_classic_level_13"], infs["g2_classic_level_15"], g2_classic_level_map);
            g2_link_create(infs["g2_classic_level_14"], infs["g2_classic_level_15"], g2_classic_level_map);

            level_name = "g2_classic_level_18";
            levels.Add(level_name, new g2_area(level_name));
            if (!infs.ContainsKey(level_name))
            {
                infs.Add(level_name, new g2_level_information(level_name));
            }





            foreach (KeyValuePair<string, g2_level_information> kp in infs)
            {
                g2_level_information k = kp.Value;
                if (k.state == 1)
                {
                    g2_complete(k.name);
                }
            }

            for (int i = 1; i <= 15; i++)
            {
                g2_unlock("g2_classic_level_" + i.ToString());
            }
            g2_unlock("g2_classic_level_18");
        }

        private void g2_level_create(string name)
        {
            g2_area a;
            if (name == "g2_classic_level_1")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 5, 30, 3000,
                    "请在合适的地方放置反射道具，让光珠到达蓝色的终点。");
                a.show_name = "关卡 1：反射";
                a.items.Clear();


                //  -> .  .  .  .  \
                //  *  .  .  .  .  .
                //  *  .  .  .  .  .
                //  *  .  .  .  .  .
                //  *  .  .  .  .  []
                a.insert_cell(g2_cell_types["wall"], 0, 1, 1, 4);
                a.insert_cell(g2_cell_types["start"], 0, 0, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["end"], 5, 4);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 1);
            }
            #endregion
            if (name == "g2_classic_level_2")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 4, 4, 30, 3000,
                    "道具有固定的方向，不可旋转。");
                a.items.Clear();


                //  .  .  .  .  
                //  .  .  .  .
                //  .  .  .  .
                // ↑  .  .  []
                
                a.insert_cell(g2_cell_types["start"], 0, 3, 1, 1, 1, 0, -1);
                a.insert_cell(g2_cell_types["end"], 3, 3);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 1);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 1);
            }
            #endregion
            if (name == "g2_classic_level_3")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 4, 5, 30, 3000,
                    "地图中可能存在一些预置物品，任何物品都不能被收回或摧毁。");
                a.items.Clear();


                //  [] *  /  ←  
                //  .  *  .  .
                //  .  .  .  \
                //  .  .  .  .
                //  .  .  -  .

                a.insert_cell(g2_cell_types["start"], 3, 0, 1, 1, 1, -1, 0);
                a.insert_cell(g2_cell_types["reflecter"], 2, 0, 1, 1, 1, -1, 1);
                a.insert_cell(g2_cell_types["reflecter"], 2, 4, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["reflecter"], 3, 2, 1, 1, 1, 1, 1);
                a.insert_cell(g2_cell_types["wall"], 1, 0, 1, 2);
                a.insert_cell(g2_cell_types["end"], 0, 0);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 1);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 1);
            }
            #endregion
            if (name == "g2_classic_level_4")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 6, 30, 3000,
                    "光珠还可以斜着运动，并在墙之间穿过。");
                a.items.Clear();


                //  .  .  .  .  *  *
                // []  .  .  .  *  *
                //  *  *  *  .  .  .
                //  *  *  .  *  .  .
                //  *  .  *  *  .  .
                // ↗  *  *  *  .  .

                a.insert_cell(g2_cell_types["wall"], 0, 2, 4, 4);
                a.insert_cell(g2_cell_types["wall"], 4, 0, 2, 2);
                a.insert_cell(g2_cell_types["start"], 0, 5, 1, 1, 1, 1, -1);
                a.insert_cell(g2_cell_types["empty"], 1, 4);
                a.insert_cell(g2_cell_types["empty"], 2, 3);
                a.insert_cell(g2_cell_types["empty"], 3, 2);
                a.insert_cell(g2_cell_types["end"], 0, 1);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 0)
                };
                a.auto_item_add(h, 1);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(0, 1)
                };
                a.auto_item_add(h, 1);
            }
            #endregion
            if (name == "g2_classic_level_5")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 4, 30, 3000,
                    "反射道具的两面都可以使用。");
                a.items.Clear();


                // *  *  *  *  .  \
                // *  *  *  .  .  .
                //→  .  .  /  .  *
                // *  *  * []  *  *

                a.insert_cell(g2_cell_types["wall"], 0, 0, 4, 2);
                a.insert_cell(g2_cell_types["wall"], 0, 3, 6, 1);
                a.insert_cell(g2_cell_types["wall"], 5, 2);
                a.insert_cell(g2_cell_types["empty"], 3, 1);

                a.insert_cell(g2_cell_types["start"], 0, 2, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["reflecter"], 5, 0, 1, 1, 1, 1, 1);
                a.insert_cell(g2_cell_types["reflecter"], 3, 2, 1, 1, 1, -1, 1);
                a.insert_cell(g2_cell_types["end"], 3, 3);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 4);
            }
            #endregion
            if (name == "g2_classic_level_6")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 5, 4, 30, 3000,
                    "小写字母为钥匙，可以打开对应大写字母的门。");
                a.items.Clear();

                // ->  .  .  *  *
                //  .  .  .  A []
                //  .  .  .  *  *
                //  .  .  .  a  .

                a.insert_cell(g2_cell_types["wall"], 3, 0, 2, 1);
                a.insert_cell(g2_cell_types["wall"], 3, 2, 2, 1);
                a.insert_cell(g2_cell_types["door"], 3, 1, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["key"], 3, 3, 1, 1, 1, 1, 1, "a", "");

                a.insert_cell(g2_cell_types["start"], 0, 0, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["end"], 4, 1);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 3);
            }
            #endregion
            if (name == "g2_classic_level_7")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 7, 6, 30, 3000,
                    "“×”号所在位置不能放置道具。");
                a.items.Clear();

                //  .  .  .  +  +  +  ←
                //  +  +  +  +  .  +  .
                //  .  .  .  +  +  +  +
                //  +  +  +  +  +  +  +
                //  .  .  .  +  .  +  +
                //  +  +  +  +  +  + []

                a.insert_cell(g2_cell_types["x"], 0, 0, 7, 6);
                a.insert_cell(g2_cell_types["empty"], 0, 0, 3, 1);
                a.insert_cell(g2_cell_types["empty"], 0, 2, 3, 1);
                a.insert_cell(g2_cell_types["empty"], 0, 4, 3, 1);
                a.insert_cell(g2_cell_types["empty"], 4, 4);
                a.insert_cell(g2_cell_types["empty"], 4, 1);
                a.insert_cell(g2_cell_types["empty"], 6, 1);

                a.insert_cell(g2_cell_types["start"], 6, 0, 1, 1, 1, -1, 0);
                a.insert_cell(g2_cell_types["end"], 6, 5);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 2);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 3);
            }
            #endregion
            if (name == "g2_classic_level_8")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 5, 30, 3000,
                    "光珠从两边击打反射板时会消失。");
                a.items.Clear();

                //  .  . []  .  .  .
                //  .  .  .  \  .  .
                //  .  .  .  .  .  .
                //  .  .  .  .  .  ↖
                //  .  .  .  .  .  .

                a.insert_cell(g2_cell_types["reflecter"], 3, 1, 1, 1, 1, 1, 1);
                a.insert_cell(g2_cell_types["start"], 5, 3, 1, 1, 1, -1, -1);
                a.insert_cell(g2_cell_types["end"], 2, 0);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 0)
                };
                a.auto_item_add(h, 2);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(0, 1)
                };
                a.auto_item_add(h, 1);
            }
            #endregion
            if (name == "g2_classic_level_9")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 5, 30, 3000,
                    "不同的钥匙解锁不同的门，可以解锁多个相同的门。");
                a.items.Clear();

                //  .  .  .  .  .  b
                //  .  .  B  B  A  B
                //  .  .  B  .  . <-
                //  B  .  A  .  .  .
                // []  .  B  a  .  .

                a.insert_cell(g2_cell_types["door"], 2, 1, 4, 1, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["door"], 2, 1, 1, 4, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["door"], 4, 1, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["door"], 2, 3, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["door"], 0, 3, 1, 1, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["key"], 3, 4, 1, 1, 1, 1, 1, "a", "");
                a.insert_cell(g2_cell_types["key"], 5, 0, 1, 1, 1, 1, 1, "b", "");

                a.insert_cell(g2_cell_types["start"], 5, 2, 1, 1, 1, -1, 0);
                a.insert_cell(g2_cell_types["end"], 0, 4);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 2);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 3);
            }
            #endregion
            if (name == "g2_classic_level_10")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 7, 30, 3000,
                    "一格中可能有两个钥匙或门，双层门需要两把钥匙打开。");
                a.items.Clear();

                //  .  .  .  a  B Ca
                //  .  .  .  *  *  *
                //  .  .  .  .  A bc
                //  .  .  .  *  *  *
                //  .  .  .  . dd AA
                //  .  . DD AB  * CC
                // ↑  b BA  c  * []

                a.insert_cell(g2_cell_types["wall"], 3, 1, 3, 1);
                a.insert_cell(g2_cell_types["wall"], 3, 3, 3, 1);
                a.insert_cell(g2_cell_types["wall"], 4, 5, 1, 2);

                a.insert_cell(g2_cell_types["door"], 4, 0, 1, 1, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["door"], 4, 2, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["door"], 5, 4, 1, 1, 1, 1, 1, "", "AA");
                a.insert_cell(g2_cell_types["door"], 3, 5, 1, 1, 1, 1, 1, "", "AB");
                a.insert_cell(g2_cell_types["door"], 2, 6, 1, 1, 1, 1, 1, "", "BA");
                a.insert_cell(g2_cell_types["door"], 2, 5, 1, 1, 1, 1, 1, "", "DD");
                a.insert_cell(g2_cell_types["door"], 5, 5, 1, 1, 1, 1, 1, "", "CC");
                a.insert_cell(g2_cell_types["keydoor"], 5, 0, 1, 1, 1, 1, 1, "a", "C");

                a.insert_cell(g2_cell_types["key"], 3, 0, 1, 1, 1, 1, 1, "a", "");
                a.insert_cell(g2_cell_types["key"], 1, 6, 1, 1, 1, 1, 1, "b", "");
                a.insert_cell(g2_cell_types["key"], 5, 2, 1, 1, 1, 1, 1, "bc", "");
                a.insert_cell(g2_cell_types["key"], 3, 6, 1, 1, 1, 1, 1, "c", "");
                a.insert_cell(g2_cell_types["key"], 4, 4, 1, 1, 1, 1, 1, "dd", "");

                a.insert_cell(g2_cell_types["start"], 0, 6, 1, 1, 1, 0, -1);
                a.insert_cell(g2_cell_types["end"], 5, 6);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 10);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 10);
            }
            #endregion
            if (name == "g2_classic_level_11")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 4, 30, 3000,
                    "地图上新的道具叫方向转换器，球从绿色方向进入，从红色方向离开。");
                a.items.Clear();

                // ↘  .  .  a  *  []
                //  .  .  .  .  A  *
                //  .  .  .  .  .  .
                //  . <-/ .  . ->/  .


                a.insert_cell(g2_cell_types["wall"], 4, 0, 2, 2);
                a.insert_cell(g2_cell_types["door"], 4, 1, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["key"], 3, 0, 1, 1, 1, 1, 1, "a", "");

                a.insert_cell(g2_cell_types["in out"], 1, 3, 1, 1, 1, 1, 1, "", "", "7", "4");
                a.insert_cell(g2_cell_types["in out"], 4, 3, 1, 1, 1, 1, 1, "", "", "4", "7");

                a.insert_cell(g2_cell_types["start"], 0, 0, 1, 1, 1, 1, 1);
                a.insert_cell(g2_cell_types["end"], 5, 0);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 5);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 5);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 0)
                };
                a.auto_item_add(h, 5);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(0, 1)
                };
                a.auto_item_add(h, 5);
            }
            #endregion
            if (name == "g2_classic_level_12")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 7, 5, 30, 3000,
                    "金色的门或钥匙下隐藏着“×”。");
                a.items.Clear();

                //  .  .  .  B+  .  .  +
                //  .  .  b  A+ a+  .  +
                // ->  .  .  A+  +  + []
                //  .  .  b  A+ a+  .  +
                //  .  .  .  B+  .  .  +


                a.insert_cell(g2_cell_types["x door"], 3, 0, 1, 5, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["x door"], 3, 1, 1, 3, 1, 1, 1, "", "A");

                a.insert_cell(g2_cell_types["key"], 2, 1, 1, 1, 1, 1, 1, "b", "");
                a.insert_cell(g2_cell_types["key"], 2, 3, 1, 1, 1, 1, 1, "b", "");

                a.insert_cell(g2_cell_types["x key"], 4, 1, 1, 1, 1, 1, 1, "a", "");
                a.insert_cell(g2_cell_types["x key"], 4, 3, 1, 1, 1, 1, 1, "a", "");

                a.insert_cell(g2_cell_types["x"], 4, 2, 3, 1);
                a.insert_cell(g2_cell_types["x"], 6, 0, 1, 5);

                a.insert_cell(g2_cell_types["start"], 0, 2, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["end"], 6, 2);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 3);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 2);
            }
            #endregion
            if (name == "g2_classic_level_13")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 9, 5, 30, 3000,
                    "你可以在光珠运动的途中放置道具，有时会带来不一样的效果。");
                a.items.Clear();


                //    ->   .    .    .    .    .    .    .    |
                //
                //    .    .    .    .    .    .    .    .    .
                //
                //    AB   *    .    .    .    .    .    .    b
                //
                //    []   *    .    .    .    a    .    .    .
                //
                //    |    .    .    .    .    .    .    .    .
                //



                a.insert_cell(g2_cell_types["wall"], 1, 2, 1, 2);
                a.insert_cell(g2_cell_types["door"], 0, 2, 1, 1, 1, 1, 1, "", "AB");
                a.insert_cell(g2_cell_types["key"], 5, 3, 1, 1, 1, 1, 1, "a", "");
                a.insert_cell(g2_cell_types["key"], 8, 2, 1, 1, 1, 1, 1, "b", "");

                a.insert_cell(g2_cell_types["reflecter"], 8, 0, 1, 1, 1, 0, 1);
                a.insert_cell(g2_cell_types["reflecter"], 0, 4, 1, 1, 1, 0, 1);

                a.insert_cell(g2_cell_types["start"], 0, 0, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["end"], 0, 3);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 6);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(0, 1)
                };
                a.auto_item_add(h, 1);
            }
            #endregion
            if (name == "g2_classic_level_14")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 7, 6, 30, 3000,
                    "方向转换器也可以被手动放置。\n当遇到复杂的地图时，建议一步步分析并采用排除法。");
                a.items.Clear();


                //    .    /    +    /    +    +    +
                //
                //    a    /    .    /    A    +    []
                //
                //    .    -    .    -    .    +    A
                //
                //   ->    \    .    \    .    +    .
                //
                //    .    \    +    \    .    +    .
                //
                //    .    .    +    .    .    .    .
                //
                
                a.insert_cell(g2_cell_types["door"], 4, 1, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["door"], 6, 2, 1, 1, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["key"], 0, 1, 1, 1, 1, 1, 1, "a", "");

                a.insert_cell(g2_cell_types["x"], 2, 0, 1, 1);
                a.insert_cell(g2_cell_types["x"], 2, 4, 1, 1);
                a.insert_cell(g2_cell_types["x"], 2, 5, 1, 1);
                a.insert_cell(g2_cell_types["x"], 4, 0, 3, 1);
                a.insert_cell(g2_cell_types["x"], 5, 0, 1, 5);

                a.insert_cell(g2_cell_types["reflecter"], 1, 0, 1, 2, 1, -1, 1);
                a.insert_cell(g2_cell_types["reflecter"], 3, 0, 1, 2, 1, -1, 1);
                a.insert_cell(g2_cell_types["reflecter"], 1, 3, 1, 2, 1, 1, 1);
                a.insert_cell(g2_cell_types["reflecter"], 3, 3, 1, 2, 1, 1, 1);
                a.insert_cell(g2_cell_types["reflecter"], 1, 2, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["reflecter"], 3, 2, 1, 1, 1, 1, 0);

                a.insert_cell(g2_cell_types["start"], 0, 3, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["end"], 6, 1);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 3);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 3);

                h = new g2_cell(g2_cell_types["in out"]);
                h.in_dirs.Add(new Point(0, 1));
                h.out_dirs.Add(new Point(-1, 1));
                a.auto_item_add(h, 1);

            }
            #endregion
            if (name == "g2_classic_level_15")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 7, 7, 30, 3000,
                    "某些地图有新的过关条件，而不是到达终点。通常情况下，一条路走到尽头会点亮最多的格子。");
                a.items.Clear();

                a.condition.set_type(g2_complete_condition.type.count);
                a.condition.count_req = 30;

                //    .    .    A    .    .    +    +
                //
                //    .    .    A    .    .    +   ->o
                //                                  ↙
                //    .    .    A    .    .    +    .
                //
                //    .    .    A   ↑    .    +    .
                //
                //    .    .    A    .    .    +    .
                //
                //    .    .    A    .    .    +    +
                //
                //    .    -    A    .    .    +    a+
                //

                a.insert_cell(g2_cell_types["door"], 2, 0, 1, 7, 1, 1, 1, "", "A");

                a.insert_cell(g2_cell_types["x"], 5, 0, 2, 7);

                a.insert_cell(g2_cell_types["in out"], 6, 1, 1, 1, 1, 1, 1, "", "", "4", "3");
                a.insert_cell(g2_cell_types["empty"], 6, 2, 1, 3);
                a.insert_cell(g2_cell_types["x key"], 6, 6, 1, 1, 1, 1, 1, "a", "");
                a.insert_cell(g2_cell_types["reflecter"], 1, 6, 1, 1, 1, 1, 0);

                a.insert_cell(g2_cell_types["start"], 3, 3, 1, 1, 1, 0, -1);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 2);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 3);

            }
            #endregion
            if (name == "g2_classic_level_18")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 7, 6, 30, 3000,
                    "方向转换器可能会有多个入口和多个出口，从任意一个入口进入，从所有出口离开。");
                a.items.Clear();


                //    *   []    A    B    C    .  ←o
                //                               ↗↑
                //    *    .    A    b    C    .    .
                //
                //    *    .    .    B    .    .    .
                //                  ↓
                //    *    .    .  ↖o↗  .    .    .
                //                  ↑
                //    .    *    .    .    .    *    .
                //
                //    *    a    *   ↑    *    c    *
                //


                a.insert_cell(g2_cell_types["wall"], 0, 0, 1, 4);
                a.insert_cell(g2_cell_types["wall"], 1, 4);
                a.insert_cell(g2_cell_types["wall"], 5, 4);
                a.insert_cell(g2_cell_types["wall"], 0, 5, 7, 1);
                a.insert_cell(g2_cell_types["door"], 2, 0, 1, 2, 1, 1, 1, "", "A");
                a.insert_cell(g2_cell_types["door"], 3, 0, 1, 1, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["door"], 3, 2, 1, 1, 1, 1, 1, "", "B");
                a.insert_cell(g2_cell_types["door"], 4, 0, 1, 1, 1, 1, 1, "", "C");
                a.insert_cell(g2_cell_types["door"], 4, 1, 1, 1, 1, 1, 1, "", "C");
                a.insert_cell(g2_cell_types["key"], 1, 5, 1, 1, 1, 1, 1, "a", "");
                a.insert_cell(g2_cell_types["key"], 3, 1, 1, 1, 1, 1, 1, "b", "");
                a.insert_cell(g2_cell_types["key"], 5, 5, 1, 1, 1, 1, 1, "c", "");

                a.insert_cell(g2_cell_types["in out"], 3, 3, 1, 1, 1, 1, 1, "", "", "26", "57");
                a.insert_cell(g2_cell_types["in out"], 6, 0, 1, 1, 1, 1, 1, "", "", "23", "4");

                a.insert_cell(g2_cell_types["start"], 3, 5, 1, 1, 1, 0, -1);
                a.insert_cell(g2_cell_types["end"], 1, 0);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 1);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, 999);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 0)
                };
                a.auto_item_add(h, 3);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(0, 1)
                };
                a.auto_item_add(h, 4);
            }
            #endregion
            if (name == "editor")
            #region
            {
                a = levels[name] = new g2_area(this, name, g2_area.mode.classic, 6, 6, 30, 111111111111111111111111111111111111111111111111111111111111.0,
                    "这是关卡介绍预览，可在“O”选项卡中更改它。");
                a.items.Clear();
                for(int i = 0; i < 16; i++)
                {
                    ref g2_cell c = ref a.edit.creater[i];
                    c = new g2_cell();
                    c.add_type(g2_cell.type.empty);
                }

                a.condition.add_type(g2_complete_condition.type.count);
                a.condition.count_req = 30;
                a.condition.add_type(g2_complete_condition.type.ball);
                a.condition.ball_req = 60;

                a.editing = true;
                g2_editor_saver = new Dictionary<int, g2_cell>();
                g2_cell_edit_id = 0;
                

                a.insert_cell(g2_cell_types["start"], 0, 0, 1, 1, 1, 1, 0);
                a.insert_cell(g2_cell_types["empty"], 2, 2, 1, 1, 2);
                a.insert_cell(g2_cell_types["end"], 0, 2);


                g2_cell h;

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 1)
                };
                a.auto_item_add(h, 10);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(-1, 1)
                };
                a.auto_item_add(h, int.MaxValue);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(1, 0)
                };
                a.auto_item_add(h, int.MaxValue);

                h = new g2_cell(g2_cell_types["reflecter"])
                {
                    dir = new Point(0, 1)
                };
                a.auto_item_add(h, int.MaxValue);

                h = new g2_cell(g2_cell_types["in out"]);
                h.in_dirs.Add(new Point(1, 1));
                h.out_dirs.Add(new Point(-1, 0));
                a.auto_item_add(h, int.MaxValue);

                h = new g2_cell(g2_cell_types["in out"]);
                h.in_dirs.Add(new Point(0, 1));
                h.out_dirs.Add(new Point(-1, 1));
                a.auto_item_add(h, int.MaxValue);

                h = new g2_cell(g2_cell_types["in out"]);
                h.in_dirs.Add(new Point(1, 1));
                h.in_dirs.Add(new Point(1, 0));
                h.in_dirs.Add(new Point(1, -1));
                h.in_dirs.Add(new Point(0, 1));
                h.in_dirs.Add(new Point(0, -1));
                h.in_dirs.Add(new Point(-1, 1));
                h.in_dirs.Add(new Point(-1, 0));
                h.in_dirs.Add(new Point(-1, -1));
                h.out_dirs.Add(new Point(1, 1));
                h.out_dirs.Add(new Point(1, 0));
                h.out_dirs.Add(new Point(1, -1));
                h.out_dirs.Add(new Point(0, 1));
                h.out_dirs.Add(new Point(0, -1));
                h.out_dirs.Add(new Point(-1, 1));
                h.out_dirs.Add(new Point(-1, 0));
                h.out_dirs.Add(new Point(-1, -1));
                a.auto_item_add(h, int.MaxValue);
            }
            #endregion
        }

        [NonSerialized]
        Dictionary<string, g2_level_link> g2_links = new Dictionary<string, g2_level_link>();
        private void g2_link_create(g2_level_information a, g2_level_information b, Grid g)
        {
            string name = a.name + "_link_" + b.name;
            g2_level_link l = new g2_level_link(name, a, b);
            a.nexts.Add(l);
            b.prevs.Add(l);
            g2_links.Add(name, l);

            Line line = new Line();
            Grid g1 = (Grid)vm_elems[a.name + "_grid"];
            Grid g2 = (Grid)vm_elems[b.name + "_grid"];
            line.Name = name;
            line.X1 = g1.Margin.Left + g1.Width / 2;
            line.Y1 = g1.Margin.Top + g1.Height / 2;
            line.X2 = g2.Margin.Left + g2.Width / 2;
            line.Y2 = g2.Margin.Top + g2.Height / 2;
            line.Stroke = getSCB(Color.FromRgb(0, 0, 0));
            line.StrokeThickness = 3;
            g.Children.Add(line);
            vm_elems.Add(line.Name, line);
        }
        

        private void g2_size_change(g2_area a, int dleft, int dtop, int dright, int dbottom, bool movement = true)
        {
            if (movement)
            {
                a.add_movement("ssize");
                for (int i = 0; i < -dleft; i++)
                {
                    for (int j = 0; j < a.height; j++)
                    {
                        int m = i;
                        int n = j;
                        g2_editor_saver[g2_cell_edit_id] = a.area[m, n];
                        a.add_movement("lose " + g2_cell_edit_id + " " + m + " " + n);
                        g2_cell_edit_id++;
                    }
                }
                for (int i = 0; i < -dright; i++)
                {
                    for (int j = 0; j < a.height; j++)
                    {
                        int m = a.width - 1 - i;
                        int n = j;
                        g2_editor_saver[g2_cell_edit_id] = a.area[m, n];
                        a.add_movement("lose " + g2_cell_edit_id + " " + m + " " + n);
                        g2_cell_edit_id++;
                    }
                }
                for (int i = 0; i < a.width; i++)
                {
                    for (int j = 0; j < -dtop; j++)
                    {
                        int m = i;
                        int n = j;
                        g2_editor_saver[g2_cell_edit_id] = a.area[m, n];
                        a.add_movement("lose " + g2_cell_edit_id + " " + m + " " + n);
                        g2_cell_edit_id++;
                    }
                }
                for (int i = 0; i < a.width; i++)
                {
                    for (int j = 0; j < -dbottom; j++)
                    {
                        int m = i;
                        int n = a.height - 1 - j;
                        g2_editor_saver[g2_cell_edit_id] = a.area[m, n];
                        a.add_movement("lose " + g2_cell_edit_id + " " + m + " " + n);
                        g2_cell_edit_id++;
                    }
                }
            }

            int old_width = a.width;
            int old_height = a.height;

            a.width += dleft + dright;
            a.height += dtop + dbottom;

            g2_cell[,] old_area = a.area;
            a.area = new g2_cell[a.width, a.height];
            a.check = new g2_check_point[10 * a.width + 1, 10 * a.height + 1];

            List<g2_cell.type> empty = new List<g2_cell.type>();
            for (int i = 0; i < a.width; i++)
            {
                for (int j = 0; j < a.height; j++)
                {
                    a.area[i, j] = new g2_cell(empty);
                }
            }


            
            for (int i = Math.Max(0, dleft); i < Math.Min(a.width, old_width + dleft); i++)
            {
                for(int j = Math.Max(0, dtop); j < Math.Min(a.height, old_height + dtop); j++)
                {
                    a.area[i, j] = old_area[i - dleft, j - dtop];
                    a.area[i, j].no_light_movement = false;
                    if (a.area[i, j].entered)
                    {
                        a.area[i, j].color_change = true;
                        a.area[i, j].no_light_movement = true;
                    }
                    //a.area[i, j].entered = false;
                }
            }

            if (movement)
            {
                a.add_movement("esize " + dleft + " " + dtop + " " + dright + " " + dbottom);
            }
            g2_start(a, false);
            a.grid_changed = true;
            a.item_changed = true;
            a.wall_changed = true;
        }

        private void g2_area_size_btn_refresh()
        {
            g2_area a = g2_current;

            //g2_game_option_create_sizechange_btn_grid.Name = 
            //         "g2_game_option_create_sizechange_btn_" + si + "_" + sj + "_grid";
            Grid get_gocsbg(int i, int j)
            {
                return (Grid)vm_elems["g2_game_option_create_sizechange_btn_" + i + "_" + j + "_grid"];
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    get_gocsbg(i, j).Visibility = Visibility.Hidden;
                }
            }
            if (a.width <= 0)
            {
                get_gocsbg(0, 0).Visibility = Visibility.Visible;
                get_gocsbg(1, 0).Visibility = Visibility.Visible;
            }
            if (a.height <= 0)
            {
                get_gocsbg(2, 0).Visibility = Visibility.Visible;
                get_gocsbg(3, 0).Visibility = Visibility.Visible;
            }
            if (a.width > 0 && a.height > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        get_gocsbg(i, j).Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void g2_edit_tick()
        {
            g2_amount_tick();
            g2_create_tick();
        }


        private void g2_amount_tick()
        {
            g2_area a = g2_current;
            Grid gac = (Grid)vm_elems["g2_game_amount_container"];

            if (gac.Visibility == Visibility.Visible)
            {
                inputer ip = inputers["g2_amount"];
                TextBox tb = (TextBox)vm_elems["g2_game_amount_tb"];
                TextBlock t1 = (TextBlock)vm_elems["g2_game_amount_text1"];
                TextBlock t2 = (TextBlock)vm_elems["g2_game_amount_text2"];
                TextBlock t3 = (TextBlock)vm_elems["g2_game_amount_text3"];

                bool b = int.TryParse(tb.Text, out a.edit.curr_amount);
                t2.Text = "输入1000或以上代表无限数量。";
                if (b)
                {
                    if (a.edit.curr_amount >= 1000)
                    {
                        ip.curr_state = inputer.state.warning;
                        tb.Background = getSCB(Color.FromRgb(255, 127, 0));
                        tb.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                        t2.Text += "（目前为无限）";
                    }
                    else
                    {
                        ip.curr_state = inputer.state.normal;
                        tb.Background = getSCB(Color.FromRgb(0, 255, 0));
                        tb.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    t3.Foreground = getSCB(Color.FromRgb(0, 255, 127));
                    t3.Text = "输入正确，退出后将保存";
                }
                else
                {
                    ip.curr_state = inputer.state.error;
                    t3.Foreground = getSCB(Color.FromRgb(255, 127, 0));
                    tb.Background = getSCB(Color.FromRgb(200, 0, 0));
                    tb.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    t3.Text = "输入不正确！若退出，数量将置为 1";
                }

            }
        }

        double2 g2_create_tick_count = 0;
        private void g2_create_tick()
        {
            g2_create_tick_count += time_tick_actually;
            if (g2_create_tick_count > 0.2)
            {
                g2_create_tick_count = 0;
            }
            else
            {
                return;
            }







            g2_area a = g2_current;
            Grid gcc = (Grid)vm_elems["g2_game_create_container"];
            Grid gac = (Grid)vm_elems["g2_game_amount_container"];




            int n = a.edit.right_selected_index;
            if (n != -1)
            {
                int i = n % 4;
                int j = n / 4;
                Rectangle r = (Rectangle)vm_elems["g2_game_option_create_creater_" + i + "_" + j];
                if (gcc.Visibility == Visibility.Visible)
                {
                    r.Fill = getSCB(Color.FromArgb(100, 255, 255, 0));
                }
                else
                {
                    r.Fill = getSCB(Color.FromArgb(100, 180, 180, 180));
                    a.edit.right_selected_index = -1;
                }
            }

            n = a.edit.item_left_selected_index;
            if (n != -1)
            {
                int i = n % 2;
                int j = n / 2;
                Rectangle r = (Rectangle)vm_elems["g2_game_item_edit_" + i + "_" + j];
                if (gac.Visibility == Visibility.Visible)
                {
                    r.Fill = getSCB(Color.FromArgb(100, 200, 200, 120));
                }
                else
                {
                    r.Fill = getSCB(Color.FromArgb(75, 200, 120, 120));
                    a.edit.item_left_selected_index = -1;
                }
            }

            n = a.edit.item_right_selected_index;
            if (n != -1)
            {
                int i = n % 2;
                int j = n / 2;
                Rectangle r = (Rectangle)vm_elems["g2_game_item_edit_" + i + "_" + j];
                if (gcc.Visibility == Visibility.Visible)
                {
                    r.Fill = getSCB(Color.FromArgb(100, 200, 200, 120));
                }
                else
                {
                    r.Fill = getSCB(Color.FromArgb(75, 200, 120, 120));
                    a.edit.item_right_selected_index = -1;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int x = i + j * 4;
                    if (a.edit.creater[x] != null)
                    {
                        g2_draw_cell(a.edit.creater[x], "g2_game_option_create_creater_" + i + "_" + j + "_item", 50, 1, false);
                    }
                    vm_elems["g2_game_option_create_creater_" + i + "_" + j + "_select"].Visibility = Visibility.Hidden;
                }
            }
            n = a.edit.left_selected_index;
            if (n != -1)
            {
                int i = n % 4;
                int j = n / 4;
                vm_elems["g2_game_option_create_creater_" + i + "_" + j + "_select"].Visibility = Visibility.Visible;
            }




            //g2_game_create_attrs_grid.Name = "g2_game_create_attrs_grid";
            if (vm_elems["g2_game_create_attrs_grid"].Visibility == Visibility.Hidden)
            {
                g2_t.selected = -1;
            }

            Grid gcasg = (Grid)vm_elems["g2_game_create_attrs_show_grid"];
            foreach (FrameworkElement f in gcasg.Children)
            {
                f.Visibility = Visibility.Hidden;
            }
            n = 0;
            string design_select = "";
            foreach (Tuple<int, int, int, int> t in a.edit.result.edit_chain)
            {
                int i = n % 5;
                int j = n / 5;
                Grid show_grid = (Grid)gcasg.Children[i * 3 + j];
                show_grid.Visibility = Visibility.Visible;

                g2_edit_cell_inf eci = g2_ci[t.Item1, t.Item2, t.Item3, t.Item4];

                //g2_game_create_attr_show_item.Name = "g2_game_create_attr_show_" + i + "_" + j + "_item";
                g2_draw_cell(eci.c, "g2_game_create_attr_show_" + i + "_" + j + "_item", 24, 1, false);

                //g2_game_creater_text.Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_text";
                //g2_game_create_attr_show_text.Name = "g2_game_create_attr_show_" + i + "_" + j + "_text";
                TextBlock t1 = (TextBlock)vm_elems["g2_game_creater_" + t.Item1 + "_" + t.Item2 + "_" + t.Item3 + "_" + t.Item4 + "_text"];
                TextBlock t2 = (TextBlock)vm_elems["g2_game_create_attr_show_" + i + "_" + j + "_text"];
                t2.Text = t1.Text;

                Rectangle bg = (Rectangle)vm_elems["g2_game_create_attr_show_" + i + "_" + j + "_bg"];
                Rectangle cover = (Rectangle)vm_elems["g2_game_create_attr_show_" + i + "_" + j];
                
                int tag = (int)cover.Tag;
                if (g2_t.is_designable(eci.c))
                {
                    bg.Fill = getSCB(Color.FromRgb(60, 90, 120));
                    t2.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    flag_remove(ref tag, 10);
                }
                else
                {
                    bg.Fill = getSCB(Color.FromRgb(200, 0, 0));
                    t2.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    flag_add(ref tag, 10);
                }
                cover.Tag = tag;
                if (g2_t.selected == n)
                {
                    bg.Fill = getSCB(Color.FromRgb(255, 255, 0));
                    t2.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    design_select = t2.Text;
                }
                if (tag / 2 == 1)
                {
                    bg.Fill = color_mul(bg.Fill, 0.5);
                }
                if (tag % 2 == 1)
                {
                    bg.Fill = color_mul(bg.Fill, 0.8);
                }

                n++;
            }
            TextBlock g2_game_create_attrs_show_text = (TextBlock)vm_elems["g2_game_create_attrs_show_text"];
            if (n == 0)
            {
                g2_game_create_attrs_show_text.Visibility = Visibility.Visible;
            }
            else
            {
                g2_game_create_attrs_show_text.Visibility = Visibility.Hidden;
            }

            bool correct = false;
            //g2_game_create_attrs_design_grid.Name = "g2_game_create_attrs_design_grid";
            Grid gcadg = (Grid)vm_elems["g2_game_create_attrs_design_grid"];
            foreach (FrameworkElement f in gcadg.Children)
            {
                if (f.Tag != null && 
                    f.Tag is string && 
                    ((string)f.Tag) == design_select)
                {
                    f.Visibility = Visibility.Visible;
                }
                else
                {
                    f.Visibility = Visibility.Hidden;
                }
            }
            g2_design_tick();

            

            if (gcc.Visibility == Visibility.Visible)
            {
                g2_cell c1 = a.edit.result;

                Grid g2_game_create_objects_grid = (Grid)vm_elems["g2_game_create_objects_grid"];

                for (int i = 0; i < 2; i++)
                {
                    for(int j = 0; j < 2; j++)
                    {
                        int row = (int)g2_game_create_objects_grid.RowDefinitions[2 * j].Height.Value;
                        int col = (int)g2_game_create_objects_grid.ColumnDefinitions[2 * i].Width.Value;
                        for(int p = 0; p < col; p++)
                        {
                            for(int q = 0; q < row; q++)
                            {
                                string sbase()
                                {
                                    return "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q;
                                }

                                Grid g = (Grid)vm_elems[sbase() + "_grid"];
                                if (g.Visibility != Visibility.Visible)
                                {
                                    continue;
                                }
                                g2_edit_cell_inf eci = g2_ci[i, j, p, q];

                                g2_cell c2 = eci.c;
                                if (eci.cur != g2_edit_cell_inf.type.select)
                                {
                                    if (g2_t.is_selectable(c1, c2))
                                    {
                                        eci.cur = g2_edit_cell_inf.type.normal;
                                        if (g2_t.is_must(c1, c2))
                                        {
                                            eci.cur = g2_edit_cell_inf.type.must;
                                        }
                                    }
                                    else
                                    {
                                        eci.cur = g2_edit_cell_inf.type.conflict;
                                    }
                                }

                                Rectangle r = (Rectangle)vm_elems[sbase() + "_bg"];
                                TextBlock t = (TextBlock)vm_elems[sbase() + "_text"];
                                Rectangle cover = (Rectangle)vm_elems[sbase()];
                                if (eci.cur != eci.old)
                                {
                                    eci.old = eci.cur;
                                    t.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                                    switch (eci.cur)
                                    {
                                        case g2_edit_cell_inf.type.normal:
                                            r.Fill = getSCB(Color.FromRgb(60, 90, 120));
                                            break;
                                        case g2_edit_cell_inf.type.must:
                                            r.Fill = getSCB(Color.FromRgb(0, 255, 150));
                                            t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                                            break;
                                        case g2_edit_cell_inf.type.conflict:
                                            r.Fill = getSCB(Color.FromRgb(200, 0, 0));
                                            break;
                                        case g2_edit_cell_inf.type.select:
                                            r.Fill = getSCB(Color.FromRgb(255, 255, 0));
                                            t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                                            break;
                                    }
                                    int ta = (int)cover.Tag;
                                    if (ta % 2 == 1)
                                    {
                                        ta -= 1;
                                    }
                                    cover.Tag = ta;
                                }


                                if (eci.locked)
                                {
                                    r.Fill = getSCB(Color.FromRgb(127, 127, 0));
                                    t.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                                }

                                int tag = (int)cover.Tag;
                                if (eci.cur == g2_edit_cell_inf.type.conflict || eci.locked)
                                {
                                    tag |= 1024;
                                }
                                else
                                {
                                    tag &= ~1024;
                                }
                                cover.Tag = tag;
                            }
                        }
                    }
                }
                
                if (c1.cell_type.Count > 0 &&
                    c1.have_but_not_only_type(g2_cell.type.item))
                {
                    g2_draw_cell(c1, "g2_game_create_preview_item", 150, 1, false);
                    vm_elems["g2_game_create_preview_item_bg"].Visibility = Visibility.Visible;
                    vm_elems["g2_game_create_preview_item"].Visibility = Visibility.Visible;
                    correct = true;
                }
                else
                {
                    vm_elems["g2_game_create_preview_item_bg"].Visibility = Visibility.Hidden;
                    vm_elems["g2_game_create_preview_item"].Visibility = Visibility.Hidden;
                    correct = false;
                }
            }

            Rectangle gcb0bg = (Rectangle)vm_elems["g2_game_create_btn_0_bg"];
            TextBlock gcb0txt = (TextBlock)vm_elems["g2_game_create_btn_0_text"];
            Rectangle gcb0 = (Rectangle)vm_elems["g2_game_create_btn_0"];
            //设计器0号按钮（确认键）变色
            {
                //g2_game_create_btn_grid.Name = "g2_game_create_btn_" + i + "_grid";
                if (correct && g2_t.is_correct(a.edit.result))
                {
                    flag_remove(gcb0, 10);
                    gcb0bg.Fill = getSCB(Color.FromRgb(0, 255, 0));
                    gcb0txt.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                }
                else
                {
                    flag_add(gcb0, 10);
                    gcb0bg.Fill = getSCB(Color.FromRgb(255, 0, 0));
                    gcb0txt.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                }
                if (flag_have(gcb0, 1))
                {
                    gcb0bg.Fill = color_mul(gcb0bg.Fill, 0.5);
                }
                if (flag_have(gcb0, 0))
                {
                    gcb0bg.Fill = color_mul(gcb0bg.Fill, 0.8);
                }
            }
        }

        //实时反应cell属性
        private void g2_design_tick()
        {
            g2_area a = g2_current;
            g2_cell c = a.edit.result;

            //起点
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    RadioButton g2_game_create_attr_start_radio = (RadioButton)vm_elems["g2_game_create_attr_start_radio_" + i + "_" + j];
                    switch (i + 4 * j)
                    {
                        case 0:
                            // con = "←"; 0
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(-1, 0);
                            break;
                        case 1:
                            //con = "→"; 1
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(1, 0);
                            break;
                        case 2:
                            //con = "↑"; 2
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(0, -1);
                            break;
                        case 3:
                            //con = "↓"; 3
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(0, 1);
                            break;
                        case 4:
                            //con = "↖"; 2*2+0
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(-1, -1);
                            break;
                        case 5:
                            //con = "↗"; 2*2+1
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(1, -1);
                            break;
                        case 6:
                            //con = "↙"; 2*3+0
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(-1, 1);
                            break;
                        case 7:
                            //con = "↘"; 2*3+1
                            g2_game_create_attr_start_radio.IsChecked = c.dir == new Point(1, 1);
                            break;
                    }
                }
            }
            
            //反射板
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    RadioButton g2_game_create_attr_reflecter_radio = (RadioButton)vm_elems["g2_game_create_attr_reflecter_radio_" + i + "_" + j];
                    switch (i + 2 * j)
                    {
                        case 0:
                            //con = "←→";
                            g2_game_create_attr_reflecter_radio.IsChecked = c.dir == new Point(1, 0);
                            break;
                        case 1:
                            //con = "↑  ↓";
                            g2_game_create_attr_reflecter_radio.IsChecked = c.dir == new Point(0, 1);
                            break;
                        case 2:
                            //con = "↙↗";
                            g2_game_create_attr_reflecter_radio.IsChecked = c.dir == new Point(-1, 1);
                            break;
                        case 3:
                            //con = "↖↘";
                            g2_game_create_attr_reflecter_radio.IsChecked = c.dir == new Point(1, 1);
                            break;
                    }
                }
            }
            //属性更新 TODO

        }

        private void g2_edit_init()
        {
            //g2_game_creater_grid.Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_grid";
            g2_t = new g2_item_relation_table();
            g2_ci = new g2_edit_cell_inf[2, 2, 5, 5];

            g2_cell c;
            List<g2_cell.type> types;
            int i, j, p, q;
            string sbase()
            {
                return "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_";
            }
            void gcg_show()
            {
                Grid g = (Grid)vm_elems[sbase() + "grid"];
                g.Visibility = Visibility.Visible;
                return;
            }
            string sgci()
            {
                return sbase() + "item";
            }
            TextBlock gct()
            {
                return (TextBlock)vm_elems[sbase() + "text"];
            }
            void gct_c_set(string s)
            {
                gct().Text = s;
                gct().FontSize = 21 / (1 + 0.3 * s.Length) * (1 + 0.4 * (s.Length / 5));
                g2_ci[i, j, p, q] = new g2_edit_cell_inf(c);
                if (j == 0)
                {
                    /*
                    foreach(g2_cell.type t in types)
                    {
                        List<g2_cell.type> ts = new List<g2_cell.type>();
                        ts.Add(g2_cell.type.item);
                        ts.Add(t);
                        g2_t.add(g2_t.conflict, ts);
                    }*/
                }
                else
                {
                    foreach (g2_cell.type t in types)
                    {
                        List<g2_cell.type> ts = new List<g2_cell.type>();
                        ts.Add(g2_cell.type.item);
                        ts.Add(t);
                        g2_t.add(g2_t.combine, ts);
                    }
                }
            }


            Grid gci0000 = (Grid)vm_elems["g2_game_creater_0_0_0_0_item_grid"];
            double gci_size = gci0000.Width;
            void draw()
            {
                g2_draw_cell(c, sgci(), gci_size, 1, false);
            }
            
            i = 0;
            j = 0;
            p = 0;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.empty);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("空白");

            i = 0;
            j = 0;
            p = 0;
            q = 1;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.special);
            types.Add(g2_cell.type.light);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("发光");

            i = 0;
            j = 0;
            p = 1;
            q = 1;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.special);
            types.Add(g2_cell.type.unlight);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("取消发光");








            i = 1;
            j = 0;
            p = 0;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.wall);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("墙");

            i = 1;
            j = 0;
            p = 1;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.super_wall);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("超级墙");

            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.wall);
            types.Add(g2_cell.type.super_wall);
            g2_t.add(g2_t.conflict, types);



            i = 1;
            j = 0;
            p = 2;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.door);
            c = new g2_cell(types)
            {
                door = "A"
            };
            gcg_show();
            draw();
            gct_c_set("门");
            g2_t.can_design.Add(g2_cell.type.door, true);
            g2_t.add_must(g2_cell.type.door, g2_cell.type.wall);
            g2_t.add_must(g2_cell.type.door, g2_cell.type.super_wall);

            i = 1;
            j = 0;
            p = 2;
            q = 1;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.x);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("占位器");


            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.x);
            types.Add(g2_cell.type.door);
            types.Add(g2_cell.type.key);
            g2_t.add(g2_t.combine, types);







            i = 0;
            j = 1;
            p = 0;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.reflecter);
            c = new g2_cell(types)
            {
                dir = new Point(1, 1)
            };
            gcg_show();
            draw();
            gct_c_set("反射板");
            g2_t.can_design.Add(g2_cell.type.reflecter, true);

            i = 0;
            j = 1;
            p = 1;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.end);
            c = new g2_cell(types);
            gcg_show();
            draw();
            gct_c_set("终点");






            i = 1;
            j = 1;
            p = 0;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.start);
            c = new g2_cell(types)
            {
                dir = new Point(1, 0)
            };
            gcg_show();
            draw();
            gct_c_set("起点");
            g2_t.can_design.Add(g2_cell.type.start, true);
            g2_t.add_must(g2_cell.type.start, g2_cell.type.wall);
            g2_t.add_must(g2_cell.type.start, g2_cell.type.super_wall);


            i = 1;
            j = 1;
            p = 1;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.in_out);
            c = new g2_cell(types);
            c.in_dirs.Add(new Point(-1, 0));
            c.out_dirs.Add(new Point(1, 0));
            gcg_show();
            draw();
            gct_c_set("转换器");
            g2_t.can_design.Add(g2_cell.type.in_out, true);
            g2_t.add_must(g2_cell.type.in_out, g2_cell.type.wall);
            g2_t.add_must(g2_cell.type.in_out, g2_cell.type.super_wall);



            i = 1;
            j = 1;
            p = 2;
            q = 0;
            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.key);
            c = new g2_cell(types)
            {
                key = "a"
            };
            gcg_show();
            draw();
            gct_c_set("钥匙");
            g2_t.can_design.Add(g2_cell.type.key, true);



            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.start);
            types.Add(g2_cell.type.in_out);
            types.Add(g2_cell.type.door);
            g2_t.add(g2_t.conflict, types);

            types = new List<g2_cell.type>();
            types.Add(g2_cell.type.start);
            types.Add(g2_cell.type.in_out);
            types.Add(g2_cell.type.reflecter);
            types.Add(g2_cell.type.key);
            types.Add(g2_cell.type.end);
            g2_t.add(g2_t.conflict, types);
        }
    }
}
