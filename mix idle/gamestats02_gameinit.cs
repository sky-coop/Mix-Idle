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
        public void game_init(bool load_auto = false)
        {
            ex.reseter = new energy
            {
                cost_mul = 1
            };
            time_boost_max_old = -1;

            m.settings_grid.Visibility = (Visibility)1;
            m.achieves_grid.Visibility = (Visibility)1;
            m.achieve_hint_grid.Visibility = (Visibility)1;
            m.helps_grid.Visibility = (Visibility)1;
            m.resource_grid.Visibility = (Visibility)1;
            m.能量_grid.Visibility = (Visibility)1;
            m.number_mode_combobox.SelectedIndex = 0;

            inputers = new Dictionary<string, inputer>();
            inputers.Add("resource_name_input", new inputer());
            inputers.Add("resource_amount_input", new inputer());

            m.详细信息框.Visibility = (Visibility)2;
            foreach (TextBlock tb in m.options_grid.Children)
            {
                tb.Tag = not_sub_res;
            }
            all_option_unselect();
            option_select(m.方块);
            init_num_select();

            fps_container = new List<double2>();

            none = new resource(0, 0, "无", getSCB(Color.FromRgb(0, 0, 0)));
            one = new resource(0, 1, "无", getSCB(Color.FromRgb(0, 0, 0)));

            foreach (TextBlock t in m.options_grid.Children)
            {
                t.Visibility = (Visibility)1;
            }
            m.方块.Visibility = 0;
            m.转生.Visibility = 0;


            foreach (Grid g in m.制造_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            m.制造_菜单_食物_grid.Visibility = (Visibility)1;

            foreach (Grid g in m.战斗_option_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            m.战斗_场景_洁白世界_grid.Visibility = 0;


            foreach (Grid g in m.战斗_场景_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            m.战斗_场景_information_grid.Visibility = (Visibility)1;
            m.战斗_玩家_grid.Visibility = 0;

            //选项之间有线
            foreach (Grid g in m.战斗_玩家_攻击风格_手动_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            foreach (Grid g in m.战斗_玩家_攻击风格_自动_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }

            m.战斗_玩家_攻击风格_自动_普通_grid.Visibility = 0;
            m.战斗_玩家_攻击风格_线.X1 = 40;
            m.战斗_玩家_攻击风格_线.X2 = 120;
            m.战斗_玩家_攻击风格_自动_重击_grid.Visibility = 0;



            foreach (Grid g in m.魔法_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            m.魔法_菜单_药水_grid.Visibility = (Visibility)1;
            foreach (Grid g in m.采矿_主_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }

            Dictionary<string, resource> res_group_特殊 = new Dictionary<string, resource>();
            res_table.Add("特殊", res_group_特殊);

            resource res_能量 = new resource(1, 0, "能量",
                getSCB(Color.FromRgb(100, 200, 255)))
            {
                unlocked = false
            };
            res_group_特殊.Add("能量", res_能量);

            resource res_高阶能量 = new resource(2, 0, "高阶能量",
                getSCB(Color.FromRgb(100, 200, 255)))
            {
                unlocked = false
            };
            res_group_特殊.Add("高阶能量", res_高阶能量);

            resource res_终极能量 = new resource(3, 0, "终极能量",
                getSCB(Color.FromRgb(100, 200, 255)))
            {
                unlocked = false
            };
            res_group_特殊.Add("终极能量", res_终极能量);

            //no.1 方块:
            #region
            Dictionary<string, resource> res_group_方块 = new Dictionary<string, resource>();
            res_table.Add("方块", res_group_方块);

            resource res_白色方块 = new resource(1, 1, "白色方块",
                getSCB(Color.FromRgb(255, 255, 255)))
            {
                unlocked = true
            };
            res_group_方块.Add("白色方块", res_白色方块);

            resource res_泥土方块 = new resource(2, 0, "泥土方块",
                getSCB(Color.FromRgb(255, 127, 0)))
            {
                unlocked = false
            };
            res_group_方块.Add("泥土方块", res_泥土方块);

            resource res_木头方块 = new resource(3, 0, "木头方块",
                 getSCB(Color.FromRgb(225, 150, 0)))
            {
                unlocked = false
            };
            res_group_方块.Add("木头方块", res_木头方块);

            resource res_糖方块 = new resource(4, 0, "糖方块",
                 getSCB(Color.FromRgb(0, 255, 255)))
            {
                unlocked = false
            };
            res_group_方块.Add("糖方块", res_糖方块);

            resource res_石头方块 = new resource(5, 0, "石头方块",
                 getSCB(Color.FromRgb(175, 175, 175)))
            {
                unlocked = false
            };
            res_group_方块.Add("石头方块", res_石头方块);

            resource res_水晶方块 = new resource(7, 0, "水晶方块",
                 getSCB(Color.FromRgb(160, 80, 255)))
            {
                unlocked = false,
                luck_req = 1600
            };
            res_group_方块.Add("水晶方块", res_水晶方块);

            //白色方块：p = M * (t / T) ^ 1.5      p' = M / (T ^ 1.5) * (1.5t ^ 0.5)
            //         cost = 100 白色方块
            //         level   1: T = 1000  M = 50000
            //         level  +1: T * 0.8   cost * 2
            //         Every   5: T * 5     M * 8
            //         Every  20: T * 2     M * 10
            block_producter bp_白色方块 = new block_producter("白色方块")
            {
                can_reset = true
            };
            bp_白色方块.set_init_cost("白色方块", 50, 1, 0.8, 1, 2);
            bp_白色方块.set_init_value(0, 0, 50000, 1000);
            bp_白色方块.set_init_ms(5, 5, 8);
            bp_白色方块.set_init_ms2(20, 2, 10);
            bp_白色方块.set_init_special(1.5);
            bp_白色方块.unlocked = true;
            m.方块_白色方块_grid.Visibility = 0;
            block_producters.Add("白色方块", bp_白色方块);

            //泥土方块：p = M * (t / T) ^ 1.35      p' = M / (T ^ 1.35) * (1.35t ^ 0.35)
            //         cost = 100 泥土方块
            //         level  1: T = 1250  M = 250000
            //         level +1: T * 0.9   cost * 1.3
            //         Every 10: T * 3     M * 4.5
            block_producter bp_泥土方块 = new block_producter("泥土方块")
            {
                can_reset = true
            };
            bp_泥土方块.set_init_cost("泥土方块", 100, 1, 0.9, 1, 1.3);
            bp_泥土方块.set_init_value(0, 0, 250000, 1250);
            bp_泥土方块.set_init_ms(10, 3, 4.5);
            bp_泥土方块.set_init_special(1.35);
            bp_泥土方块.unlocked = false;
            m.方块_泥土方块_grid.Visibility = (Visibility)1;
            block_producters.Add("泥土方块", bp_泥土方块);
            //60T LV100
            //T*1.7426 M*60.5M
            //P = 200000 / 2200 * 60.5m / 1.7426 * 5 * 3 = 47.1b


            //木头方块：p = M * (t / T) ^ 1.25      p' = M / (T ^ 1.25) * (1.25t ^ 0.25)
            //         cost = 1 木头方块
            //         level  1: T = 50  M = 20000
            //         level +1: M * 1.1   cost * 1.4
            //         Every 10: T * 1.2     M * 1.5
            //         Every 50: T * 0.5
            block_producter bp_木头方块 = new block_producter("木头方块")
            {
                can_reset = true
            };
            bp_木头方块.set_init_cost("木头方块", 5, 1, 1, 1.1, 1.4);
            bp_木头方块.set_init_value(0, 0, 10000, 50);
            bp_木头方块.set_init_ms(10, 1.2, 1.5);
            bp_木头方块.set_init_ms2(50, 1 / 2.0, 1);
            bp_木头方块.set_init_special(1.25);
            bp_木头方块.unlocked = false;
            m.方块_木头方块_grid.Visibility = (Visibility)1;
            block_producters.Add("木头方块", bp_木头方块);

            //糖方块：p = M * (t / T) ^ 1.4      p' = M / (T ^ 1.4) * (1.4t ^ 0.4)
            //         cost = 150 糖方块
            //         level  1: T = 5  M = 360
            //         level +1: T * 0.8     M * 1.2   cost * 2
            //         Every 10: T * 10      M * 25
            block_producter bp_糖方块 = new block_producter("糖方块")
            {
                can_reset = true
            };
            bp_糖方块.set_init_cost("糖方块", 150, 1, 0.8, 1.2, 2);
            bp_糖方块.set_init_value(0, 0, 360, 5);
            bp_糖方块.set_init_ms(10, 10, 25);
            bp_糖方块.set_init_special(1.4);
            bp_糖方块.unlocked = false;
            m.方块_糖方块_grid.Visibility = (Visibility)1;
            block_producters.Add("糖方块", bp_糖方块);

            //石头方块：p = M * (t / T) ^ 1.35      p' = M / (T ^ 1.35) * (1.35t ^ 0.35)
            //         cost = 120M 石头方块
            //         level  1: T = 50  M = 450M
            //         level +1: M * 1.1     cost * 1.3
            //         Every 10: T * 0.7
            //         Every 25: T * 2.5     M * 3.6
            block_producter bp_石头方块 = new block_producter("石头方块")
            {
                can_reset = true
            };
            bp_石头方块.set_init_cost("石头方块", 120e6, 1, 1, 1.1, 1.3);
            bp_石头方块.set_init_value(0, 0, 450e6, 50);
            bp_石头方块.set_init_ms(10, 0.7, 1);
            bp_石头方块.set_init_ms2(25, 2.5, 3.6);
            bp_石头方块.set_init_special(1.35);
            bp_石头方块.unlocked = false;
            m.方块_石头方块_grid.Visibility = (Visibility)1;
            block_producters.Add("石头方块", bp_石头方块);
            #endregion

            //60T LV51 
            //M*7513 T*1.24416
            //P = M/T = 600M / 80 * 7513 / 1.24416 = 45.29B

            //no.2 制造：
            #region

            制造_options = make_group(m.制造_option_grid);

            Dictionary<string, resource> res_group_制造 = new Dictionary<string, resource>();
            res_table.Add("制造", res_group_制造);


            // 材料：白色粉末
            #region
            resource res_白色粉末 = new resource(1, 0, "白色粉末",
                getSCB(Color.FromRgb(255, 255, 255)))
            {
                unlocked = true
            };
            res_group_制造.Add("白色粉末", res_白色粉末);
            // 白色粉末 c(n) = 4 * n ^ 1.5
            upgrade up_白色粉末 = new upgrade("白色粉末", "制造", true, true)
            {
                can_reset = true,
                unlocked = true
            };
            m.制造_次_材料_白色粉末_grid.Visibility = 0;
            up_白色粉末.set_init_cost(get_auto_cost_table("白色方块"), 0, int.MaxValue);
            up_白色粉末.set_init_special(1.5, 4);
            upgrades.Add("白色粉末", up_白色粉末);
            #endregion

            // 材料：糖浆
            #region
            resource res_糖浆 = new resource(2, 0, "糖浆",
                getSCB(Color.FromRgb(0, 255, 255)))
            {
                unlocked = false
            };
            res_group_制造.Add("糖浆", res_糖浆);
            // 糖浆 c(n) = 100K * n ^ 2
            upgrade up_糖浆 = new upgrade("糖浆", "制造", true, true)
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_材料_糖浆_grid.Visibility = (Visibility)1;
            up_糖浆.set_init_cost(get_auto_cost_table("糖方块"), 0, int.MaxValue);
            up_糖浆.set_init_special(2, 100e3);
            upgrades.Add("糖浆", up_糖浆);
            #endregion

            // 材料：植物祭品
            #region
            resource res_植物祭品 = new resource(3, 0, "植物祭品",
                getSCB(Color.FromRgb(0, 160, 0)))
            {
                unlocked = false
            };
            res_group_制造.Add("植物祭品", res_植物祭品);
            // 植物祭品 c(n) = 1 * n ^ 1.925        100 祭坛能量
            upgrade up_植物祭品 = new upgrade("植物祭品", "制造", true, true)
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_材料_植物祭品_grid.Visibility = (Visibility)1;
            up_植物祭品.set_init_cost(get_auto_cost_table("烤植物"), 0, int.MaxValue);
            up_植物祭品.set_init_special(1.925, 1);
            upgrades.Add("植物祭品", up_植物祭品);
            #endregion

            // 材料：动物祭品
            #region
            resource res_动物祭品 = new resource(4, 0, "动物祭品",
                getSCB(Color.FromRgb(200, 200, 0)))
            {
                unlocked = false
            };
            res_group_制造.Add("动物祭品", res_动物祭品);
            // 动物祭品 c(n) = 1 * n ^ 1.596       400 祭坛能量
            upgrade up_动物祭品 = new upgrade("动物祭品", "制造", true, true)
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_材料_动物祭品_grid.Visibility = (Visibility)1;
            up_动物祭品.set_init_cost(get_auto_cost_table("烤动物"), 0, int.MaxValue);
            up_动物祭品.set_init_special(1.596, 1);
            upgrades.Add("动物祭品", up_动物祭品);
            #endregion

            // 材料：魔法粉末
            #region
            // 魔法粉末 c(n) = n ^ 2.865 / 5M
            upgrade up_魔法粉末 = new upgrade("魔法粉末", "制造", true, true)
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_材料_魔法粉末_grid.Visibility = (Visibility)1;
            up_魔法粉末.set_init_cost(get_auto_cost_table("魔石"), 0, int.MaxValue);
            up_魔法粉末.set_init_special(2.865, 1.0 / 5e6);
            upgrades.Add("魔法粉末", up_魔法粉末);
            #endregion

            // 材料：钻石
            #region
            resource res_钻石 = new resource(9, 0, "钻石",
                 getSCB(Color.FromRgb(0, 255, 255)))
            {
                unlocked = false,
                luck_req = 250000
            };
            res_group_制造.Add("钻石", res_钻石);
            // 钻石 c(n) = 100B * n ^ 1.11111
            upgrade up_钻石 = new upgrade("钻石", "制造", true, true)
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_材料_钻石_grid.Visibility = (Visibility)1;
            up_钻石.set_init_cost(get_auto_cost_table("煤"), 0, int.MaxValue);
            up_钻石.set_init_special(1.11111, 100e9);
            upgrades.Add("钻石", up_钻石);
            #endregion

            // 工具：用于攻击的方块棒
            #region
            upgrade up_方块棒 = new upgrade("方块棒", "制造")
            {
                can_reset = true,
                unlocked = true
            };
            m.制造_次_工具_方块棒_grid.Visibility = 0;
            List<List<Tuple<string, double2>>> up_方块棒_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_方块棒_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 20));
            up_方块棒.description.Add("攻击力 +3\n并永久解锁“战斗”");
            up_方块棒_ct.Add(up_方块棒_lv1);
            List<Tuple<string, double2>> up_方块棒_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 540000));
            up_方块棒.description.Add("攻击力 +5\n并永久解锁一个新的工具\n（目前合计 攻击力 +3）");
            up_方块棒_ct.Add(up_方块棒_lv2);
            List<Tuple<string, double2>> up_方块棒_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 34560000));
            up_方块棒.description.Add("攻击力 +12\n（目前合计 攻击力 +8）");
            up_方块棒_ct.Add(up_方块棒_lv3);
            List<Tuple<string, double2>> up_方块棒_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 8e8));
            up_方块棒.description.Add("攻击力 +20\n（目前合计 攻击力 +20）");
            up_方块棒_ct.Add(up_方块棒_lv4);
            List<Tuple<string, double2>> up_方块棒_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 2.5e10));
            up_方块棒.description.Add("攻击力 +26\n（目前合计 攻击力 +40）");
            up_方块棒_ct.Add(up_方块棒_lv5);
            List<Tuple<string, double2>> up_方块棒_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 3e12));
            up_方块棒.description.Add("攻击力 +54\n（目前合计 攻击力 +66）");
            up_方块棒_ct.Add(up_方块棒_lv6);
            List<Tuple<string, double2>> up_方块棒_lv7 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 9e14)),
                new Tuple<string, double2>("泥土方块", 200e3));
            up_方块棒.description.Add("攻击力 +80\n（目前合计 攻击力 +120）");
            up_方块棒_ct.Add(up_方块棒_lv7);
            List<Tuple<string, double2>> up_方块棒_lv8 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 2e16)),
                new Tuple<string, double2>("泥土方块", 15e6));
            up_方块棒.description.Add("攻击力 +100\n（目前合计 攻击力 +200）");
            up_方块棒_ct.Add(up_方块棒_lv8);
            List<Tuple<string, double2>> up_方块棒_lv9 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 6e18)),
                new Tuple<string, double2>("泥土方块", 600e6));
            up_方块棒.description.Add("攻击力 +150\n（目前合计 攻击力 +300）");
            up_方块棒_ct.Add(up_方块棒_lv9);
            List<Tuple<string, double2>> up_方块棒_lv10 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 25e9));
            up_方块棒.description.Add("攻击力 +216\n（目前合计 攻击力 +450）");
            up_方块棒_ct.Add(up_方块棒_lv10);
            List<Tuple<string, double2>> up_方块棒_lv11 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 1e12)),
                new Tuple<string, double2>("木头方块", 50e6));
            up_方块棒.description.Add("攻击力 +556\n（目前合计 攻击力 +666）");
            up_方块棒_ct.Add(up_方块棒_lv11);
            List<Tuple<string, double2>> up_方块棒_lv12 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 10e24)),
                new Tuple<string, double2>("木头方块", 1.25e9)),
                new Tuple<string, double2>("糖方块", 500e9));
            up_方块棒.description.Add("攻击力 +778，使“普通”攻击模式能够无视敌人25%的防御（目前合计 攻击力 +1222）");
            up_方块棒_ct.Add(up_方块棒_lv12);
            List<Tuple<string, double2>> up_方块棒_lv13 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 200e27)),
                new Tuple<string, double2>("石头方块", 400e12)),
                new Tuple<string, double2>("糖方块", 750e12));
            up_方块棒.description.Add("攻击力 +2000，使“重击”攻击模式伤害 +25%（目前合计 攻击力 +2000）");
            up_方块棒_ct.Add(up_方块棒_lv13);
            List<Tuple<string, double2>> up_方块棒_lv14 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 10e33)),
                new Tuple<string, double2>("木头方块", 360e12)),
                new Tuple<string, double2>("糖方块", 480e18));
            up_方块棒.description.Add("攻击力 +3500，使“重击”攻击模式拥有15%的真实伤害\n（目前合计 攻击力 +4000）");
            up_方块棒_ct.Add(up_方块棒_lv14);

            up_方块棒.description.Add("已达到最大等级！\n（目前合计 攻击力 +7500）");
            up_方块棒.set_init_cost(up_方块棒_ct, 0, up_方块棒_ct.Count);
            upgrades.Add("方块棒", up_方块棒);

            #endregion

            // 工具：用于攻击的喷雾
            #region
            upgrade up_喷雾 = new upgrade("喷雾", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_工具_喷雾_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_喷雾_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_喷雾_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 1.2e6)),
                new Tuple<string, double2>("白色粉末", 4000));
            up_喷雾.description.Add("解锁一种手动攻击方式\n每次点击攻击一次，攻击力为 40%\n有 0.2s 的冷却时间");
            up_喷雾_ct.Add(up_喷雾_lv1);
            List<Tuple<string, double2>> up_喷雾_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 2.4e9)),
                new Tuple<string, double2>("白色粉末", 800000));
            up_喷雾.description.Add("将喷雾的攻击从 40% 提高到 50%");
            up_喷雾_ct.Add(up_喷雾_lv2);
            List<Tuple<string, double2>> up_喷雾_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 5e12)),
                new Tuple<string, double2>("白色粉末", 2e8)),
                new Tuple<string, double2>("魔法粉末", 50));
            up_喷雾.description.Add("将喷雾的冷却时间从0.2s减至0.1s");
            up_喷雾_ct.Add(up_喷雾_lv3);
            List<Tuple<string, double2>> up_喷雾_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 9e15)),
                new Tuple<string, double2>("白色粉末", 2.75e10));
            up_喷雾.description.Add("将喷雾的攻击从 50% 提高到 70%");
            up_喷雾_ct.Add(up_喷雾_lv4);
            List<Tuple<string, double2>> up_喷雾_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 2e18)),
                new Tuple<string, double2>("烈焰粉末", 125));
            up_喷雾.description.Add("让喷雾攻击受到的减速效果降到 30%");
            up_喷雾_ct.Add(up_喷雾_lv5);
            List<Tuple<string, double2>> up_喷雾_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("糖方块", 4e15)),
                new Tuple<string, double2>("白色粉末", 200e18));
            up_喷雾.description.Add("让喷雾攻击能够降低敌人 20% 的防御");
            up_喷雾_ct.Add(up_喷雾_lv6);

            up_喷雾.description.Add("已达到最大等级！\n（目前 攻击力 70%，冷却 0.1s）");
            up_喷雾.set_init_cost(up_喷雾_ct, 0, up_喷雾_ct.Count);
            upgrades.Add("喷雾", up_喷雾);

            #endregion

            // 铲子
            #region
            upgrade up_铲子 = new upgrade("铲子", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_工具_铲子_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_铲子_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_铲子_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 3e10)),
                new Tuple<string, double2>("泥土方块", 20));
            up_铲子.description.Add("当你在草原的时候，每次攻击获得 10 泥土方块");
            up_铲子_ct.Add(up_铲子_lv1);

            List<Tuple<string, double2>> up_铲子_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 210e12)),
                new Tuple<string, double2>("泥土方块", 100e3));
            up_铲子.description.Add("当你在草原的时候，每次攻击获得 " + number_format(10e3) + " 泥土方块\n（目前 10 泥土方块）");
            up_铲子_ct.Add(up_铲子_lv2);

            List<Tuple<string, double2>> up_铲子_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色方块", 15e15)),
                new Tuple<string, double2>("泥土方块", 50e9));
            up_铲子.description.Add("每次攻击获得 " + number_format(100e6) + " 泥土方块\n（目前 " + number_format(10e3) + " 泥土方块）");
            up_铲子_ct.Add(up_铲子_lv3);

            List<Tuple<string, double2>> up_铲子_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("木头方块", 4.5e9)),
                new Tuple<string, double2>("泥土方块", 200e12)),
                new Tuple<string, double2>("铁", 1));
            up_铲子.description.Add("每次攻击获得 " + number_format(1e12) + " 泥土方块\n（目前 " + number_format(100e6) + " 泥土方块）");
            up_铲子_ct.Add(up_铲子_lv4);

            up_铲子.description.Add("已达到最大等级！\n（目前 " + number_format(1e12) + " 泥土方块）");
            up_铲子.set_init_cost(up_铲子_ct, 0, up_铲子_ct.Count);
            upgrades.Add("铲子", up_铲子);
            #endregion

            // 斧
            #region
            upgrade up_斧 = new upgrade("斧", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_工具_斧_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_斧_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_斧_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 1500));
            up_斧.description.Add("木头方块的获取 ×2");
            up_斧_ct.Add(up_斧_lv1);

            List<Tuple<string, double2>> up_斧_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铁", 50e3));
            up_斧.description.Add("木头方块的获取 ×1.5\n（目前 ×2）");
            up_斧_ct.Add(up_斧_lv2);

            List<Tuple<string, double2>> up_斧_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("钢", 10e3));
            up_斧.description.Add("木头方块的获取 ×2\n（目前 ×3）");
            up_斧_ct.Add(up_斧_lv3);

            List<Tuple<string, double2>> up_斧_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 30e9)),
                new Tuple<string, double2>("钢", 3e6));
            up_斧.description.Add("木头方块的获取 ×2\n（目前 ×6）");
            up_斧_ct.Add(up_斧_lv4);

            up_斧.description.Add("已达到最大等级！\n（目前 木头方块的获取 ×12）");
            up_斧.set_init_cost(up_斧_ct, 0, up_斧_ct.Count);
            upgrades.Add("斧", up_斧);
            #endregion

            // 剑
            #region
            upgrade up_剑 = new upgrade("剑", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_工具_剑_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_剑_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_剑_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("木头方块", 5e6));
            up_剑.description.Add("攻击力 +150，并解锁自动攻击模式“连斩”");
            up_剑_ct.Add(up_剑_lv1);

            List<Tuple<string, double2>> up_剑_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 500e9));
            up_剑.description.Add("攻击力 +450，“连斩”的最大伤害+10%（目前 +150 攻击，连斩最大伤害150%）");
            up_剑_ct.Add(up_剑_lv2);

            List<Tuple<string, double2>> up_剑_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 120e3));
            up_剑.description.Add("攻击力 +800，“连斩”的最大伤害+10%（目前 +600 攻击，连斩最大伤害160%）");
            up_剑_ct.Add(up_剑_lv3);

            List<Tuple<string, double2>> up_剑_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铁", 800e3));
            up_剑.description.Add("攻击力 +1600，“连斩”的最大伤害+10%（目前 +1400 攻击，连斩最大伤害170%）");
            up_剑_ct.Add(up_剑_lv4);

            List<Tuple<string, double2>> up_剑_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("钢", 50e3));
            up_剑.description.Add("攻击力 +2500，“连斩”的最大伤害+10%（目前 +3000 攻击，连斩最大伤害180%）");
            up_剑_ct.Add(up_剑_lv5);

            List<Tuple<string, double2>> up_剑_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("钢", 2e6));
            up_剑.description.Add("攻击力 +4500，“连斩”的最大伤害+10%（目前 +5500 攻击，连斩最大伤害190%）");
            up_剑_ct.Add(up_剑_lv6);

            up_剑.description.Add("已达到最大等级！\n（目前 +" + number_format(10e3) + " 攻击，连斩最大伤害200%）");
            up_剑.set_init_cost(up_剑_ct, 0, up_剑_ct.Count);
            upgrades.Add("剑", up_剑);
            #endregion

            // 镐
            #region
            upgrade up_镐 = new upgrade("镐", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_工具_镐_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_镐_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_镐_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 3e9));
            up_镐.description.Add("每次获得 +1 采矿点数");
            up_镐_ct.Add(up_镐_lv1);

            List<Tuple<string, double2>> up_镐_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 1000));
            up_镐.description.Add(" +300 格子边长\n（目前 +1 采矿点数）");
            up_镐_ct.Add(up_镐_lv2);

            List<Tuple<string, double2>> up_镐_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 6e12)),
                new Tuple<string, double2>("铜", 75e3));
            up_镐.description.Add("每次获得 +1 采矿点数\n（目前 +1 采矿点数，+300 格子边长）");
            up_镐_ct.Add(up_镐_lv3);

            List<Tuple<string, double2>> up_镐_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 25e12)),
                new Tuple<string, double2>("铁", 50e3));
            up_镐.description.Add("采矿基础经验 +3\n（目前 +2 采矿点数，+300 格子边长）");
            up_镐_ct.Add(up_镐_lv4);

            List<Tuple<string, double2>> up_镐_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铁", 670e3));
            up_镐.description.Add("+500 格子边长\n（目前 +2 采矿点数，+300 格子边长，+3 经验）");
            up_镐_ct.Add(up_镐_lv5);

            List<Tuple<string, double2>> up_镐_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 30e6)),
                new Tuple<string, double2>("铁", 5e6));
            up_镐.description.Add("每次获得 +2 采矿点数\n（目前 +2 采矿点数，+800 格子边长，+3 经验）");
            up_镐_ct.Add(up_镐_lv6);

            List<Tuple<string, double2>> up_镐_lv7 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 600e6)),
                new Tuple<string, double2>("铁", 128e6)),
                new Tuple<string, double2>("钢", 100e3));
            up_镐.description.Add("幸运值×2，采矿经验 +7\n（目前 +4 采矿点数，+800 格子边长，+3 经验）");
            up_镐_ct.Add(up_镐_lv7);

            List<Tuple<string, double2>> up_镐_lv8 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 15e9)),
                new Tuple<string, double2>("铁", 2.5e9)),
                new Tuple<string, double2>("钢", 2.4e6));
            up_镐.description.Add("+1200 格子边长\n（目前 +4 采矿点数，+800 格子边长，幸运值×2，+10 经验）");
            up_镐_ct.Add(up_镐_lv8);

            up_镐.description.Add("已达到最大等级！\n（目前 +4 采矿点数，+2000 格子边长，幸运值×2，+10 经验）");
            up_镐.set_init_cost(up_镐_ct, 0, up_镐_ct.Count);
            upgrades.Add("镐", up_镐);
            #endregion


            // 魔杖
            #region
            upgrade up_魔杖 = new upgrade("魔杖", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_工具_魔杖_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_魔杖_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_魔杖_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 200e6)),
                new Tuple<string, double2>("木头方块", 1.2e9));
            up_魔杖.description.Add("攻击力 +500，魔力获取 ×1.2，解锁手动攻击模式“烈焰”");
            up_魔杖_ct.Add(up_魔杖_lv1);

            List<Tuple<string, double2>> up_魔杖_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 1e12)),
                new Tuple<string, double2>("白色粒子", 20e9));
            up_魔杖.description.Add("攻击力 +3500，魔力获取 ×1.25，解锁自动攻击模式“光球”（目前 +500 攻击，魔力获取 ×1.2）");
            up_魔杖_ct.Add(up_魔杖_lv2);

            up_魔杖.description.Add("已达到最大等级！\n（目前 +" + number_format(4000) + " 攻击，魔力获取 ×1.5）");
            up_魔杖.set_init_cost(up_魔杖_ct, 0, up_魔杖_ct.Count);
            upgrades.Add("魔杖", up_魔杖);
            #endregion



            // 升级器：提升白色方块生产量
            #region
            upgrade up_白色方块生产 = new upgrade("白色方块生产", "制造")
            {
                can_reset = true,
                unlocked = true
            };
            m.制造_次_升级器_白色方块生产_grid.Visibility = 0;
            List<List<Tuple<string, double2>>> up_白色方块生产_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_白色方块生产_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 1));
            up_白色方块生产.description.Add("使白色方块生产器 T / 2\n");
            up_白色方块生产_ct.Add(up_白色方块生产_lv1);
            List<Tuple<string, double2>> up_白色方块生产_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 300));
            up_白色方块生产.description.Add("使白色方块生产器 T / 2\n（目前合计 T / 2）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv2);
            List<Tuple<string, double2>> up_白色方块生产_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 5000));
            up_白色方块生产.description.Add("使白色方块生产器 M × 4\n（目前合计 T / 4）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv3);
            List<Tuple<string, double2>> up_白色方块生产_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 60000));
            up_白色方块生产.description.Add("使白色方块生产器 T / 2\n（目前合计 T / 4，M × 4）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv4);
            List<Tuple<string, double2>> up_白色方块生产_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 500e3));
            up_白色方块生产.description.Add("此后每升一级使 T / 2\n（目前 T / 8，M × 4）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv5);
            List<Tuple<string, double2>> up_白色方块生产_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 40e6));
            up_白色方块生产.description.Add("使白色方块生产器 M × 3\n（目前 T / 16，M × 4）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv6);
            List<Tuple<string, double2>> up_白色方块生产_lv7 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 6.666e9)),
                new Tuple<string, double2>("魔法粉末", 300));
            up_白色方块生产.description.Add("使白色方块生产器 M × 2.5\n（目前 T / 32，M × 12）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv7);
            List<Tuple<string, double2>> up_白色方块生产_lv8 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 5e12)),
                new Tuple<string, double2>("魔法粉末", 20000));
            up_白色方块生产.description.Add("使白色方块生产器 T / 2\n（目前 T / 64，M × 30）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv8);
            List<Tuple<string, double2>> up_白色方块生产_lv9 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 49e15)),
                new Tuple<string, double2>("魔法粉末", 3.4e6));
            up_白色方块生产.description.Add("使白色方块生产器 M × 3.3\n（目前 T / 256，M × 30）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv9);
            List<Tuple<string, double2>> up_白色方块生产_lv10 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 13.37e30)),
                new Tuple<string, double2>("魔法粉末", 1.5e12));
            up_白色方块生产.description.Add("此后每升一级使 M × 2\n（目前 T / 512，M × 99）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv10);
            List<Tuple<string, double2>> up_白色方块生产_lv11 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 7e51)),
                new Tuple<string, double2>("魔法粉末", 200e18));
            up_白色方块生产.description.Add("使白色方块生产器 T / 4\n（目前 T / 1024，M × 198）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv11);
            List<Tuple<string, double2>> up_白色方块生产_lv12 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 3.14e75)),
                new Tuple<string, double2>("魔法粉末", 4.5e30));
            up_白色方块生产.description.Add("使白色方块生产器 T / 4\n（目前 T / 8192，M × 396）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv12);
            List<Tuple<string, double2>> up_白色方块生产_lv13 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 20e120)),
                new Tuple<string, double2>("魔法粉末", 10e45));
            up_白色方块生产.description.Add("保证 T / 500K，M × 2000\n（目前 T / 65536，M × 792）");
            up_白色方块生产_ct.Add(up_白色方块生产_lv13);


            up_白色方块生产.description.Add("已达到最大等级！\n（目前 T / 500K，M × 2000）");
            up_白色方块生产.set_init_cost(up_白色方块生产_ct, 0, up_白色方块生产_ct.Count);
            upgrades.Add("白色方块生产", up_白色方块生产);
            #endregion

            #region
            upgrade up_泥土方块生产 = new upgrade("泥土方块生产", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_升级器_泥土方块生产_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_泥土方块生产_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_泥土方块生产_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 1e6));
            up_泥土方块生产.description.Add("使泥土方块生产器 M × 2\n");
            up_泥土方块生产_ct.Add(up_泥土方块生产_lv1);
            List<Tuple<string, double2>> up_泥土方块生产_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 90e6));
            up_泥土方块生产.description.Add("使泥土方块生产器 M × 2.5\n（目前合计 M × 2）");
            up_泥土方块生产_ct.Add(up_泥土方块生产_lv2);
            List<Tuple<string, double2>> up_泥土方块生产_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 8e9)),
                new Tuple<string, double2>("魔法粉末", 40000));
            up_泥土方块生产.description.Add("使泥土方块生产器 T / 3\n（目前合计 M × 5）");
            up_泥土方块生产_ct.Add(up_泥土方块生产_lv3);

            up_泥土方块生产.description.Add("已达到最大等级！\n（目前 T / 3，M × 5）");
            up_泥土方块生产.set_init_cost(up_泥土方块生产_ct, 0, up_泥土方块生产_ct.Count);
            upgrades.Add("泥土方块生产", up_泥土方块生产);
            #endregion

            #region
            upgrade up_药水消耗降低 = new upgrade("药水消耗降低", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_升级器_药水消耗降低_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_药水消耗降低_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_药水消耗降低_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("糖浆", 1000));
            up_药水消耗降低.description.Add("药水消耗 /  1.5\n");
            up_药水消耗降低_ct.Add(up_药水消耗降低_lv1);
            List<Tuple<string, double2>> up_药水消耗降低_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("糖浆", 400e3));
            up_药水消耗降低.description.Add("药水消耗 / 1.6\n（目前药水消耗 / 1.5）\n");
            up_药水消耗降低_ct.Add(up_药水消耗降低_lv2);
            List<Tuple<string, double2>> up_药水消耗降低_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("糖浆", 650e6));
            up_药水消耗降低.description.Add("药水消耗 / 1.5\n（目前药水消耗 / 2.4）\n");
            up_药水消耗降低_ct.Add(up_药水消耗降低_lv3);

            up_药水消耗降低.description.Add("已达到最大等级！\n（目前药水消耗 / 3.6）");
            up_药水消耗降低.set_init_cost(up_药水消耗降低_ct, 0, up_药水消耗降低_ct.Count);
            upgrades.Add("药水消耗降低", up_药水消耗降低);
            #endregion

            //食物

            //缤纷沙拉
            #region
            upgrade up_缤纷沙拉 = new upgrade("缤纷沙拉", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_食物_缤纷沙拉_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_缤纷沙拉_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_缤纷沙拉_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 10e6)),
                new Tuple<string, double2>("糖方块", 5e6));
            up_缤纷沙拉.description.Add("能量 +9000\n");
            up_缤纷沙拉_ct.Add(up_缤纷沙拉_lv1);
            List<Tuple<string, double2>> up_缤纷沙拉_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 500e6)),
                new Tuple<string, double2>("糖浆", 60));
            up_缤纷沙拉.description.Add("能量 +12000\n");
            up_缤纷沙拉_ct.Add(up_缤纷沙拉_lv2);
            List<Tuple<string, double2>> up_缤纷沙拉_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 100e9)),
                new Tuple<string, double2>("糖方块", 50e9));
            up_缤纷沙拉.description.Add("能量 +18000\n");
            up_缤纷沙拉_ct.Add(up_缤纷沙拉_lv3);
            List<Tuple<string, double2>> up_缤纷沙拉_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 255e12)),
                new Tuple<string, double2>("糖浆", 35e3));
            up_缤纷沙拉.description.Add("能量 +24000\n");
            up_缤纷沙拉_ct.Add(up_缤纷沙拉_lv4);
            List<Tuple<string, double2>> up_缤纷沙拉_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 17.5e15)),
                new Tuple<string, double2>("魔法糖浆", 260e3));
            up_缤纷沙拉.description.Add("能量 +30000\n");
            up_缤纷沙拉_ct.Add(up_缤纷沙拉_lv5);
            List<Tuple<string, double2>> up_缤纷沙拉_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 90e18)),
                new Tuple<string, double2>("魔法糖浆", 16e6)),
                new Tuple<string, double2>("绿色粒子", 500e6));
            up_缤纷沙拉.description.Add("能量 +35000\n");
            up_缤纷沙拉_ct.Add(up_缤纷沙拉_lv6);

            up_缤纷沙拉.description.Add("已达到最大等级！");
            up_缤纷沙拉.set_init_cost(up_缤纷沙拉_ct, 0, up_缤纷沙拉_ct.Count);
            upgrades.Add("缤纷沙拉", up_缤纷沙拉);
            #endregion

            //勇敢生肉套餐
            #region
            upgrade up_勇敢生肉套餐 = new upgrade("勇敢生肉套餐", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_食物_勇敢生肉套餐_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_勇敢生肉套餐_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_勇敢生肉套餐_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("动物原料", 3e6)),
                new Tuple<string, double2>("糖方块", 35e6));
            up_勇敢生肉套餐.description.Add("能量 +15000，攻击 -150");
            up_勇敢生肉套餐_ct.Add(up_勇敢生肉套餐_lv1);
            List<Tuple<string, double2>> up_勇敢生肉套餐_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("动物原料", 500e6)),
                new Tuple<string, double2>("糖浆", 6e3));
            up_勇敢生肉套餐.description.Add("能量 +24000，攻击 -325\n（目前攻击 -150）");
            up_勇敢生肉套餐_ct.Add(up_勇敢生肉套餐_lv2);
            List<Tuple<string, double2>> up_勇敢生肉套餐_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("动物原料", 40e9)),
                new Tuple<string, double2>("糖浆", 200e3));
            up_勇敢生肉套餐.description.Add("能量 +35000，攻击 -625\n（目前攻击 -475）");
            up_勇敢生肉套餐_ct.Add(up_勇敢生肉套餐_lv3);
            List<Tuple<string, double2>> up_勇敢生肉套餐_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("动物原料", 10e12)),
                new Tuple<string, double2>("糖浆", 8e6));
            up_勇敢生肉套餐.description.Add("能量 +50000，攻击 -900\n（目前攻击 -1100）");
            up_勇敢生肉套餐_ct.Add(up_勇敢生肉套餐_lv4);

            up_勇敢生肉套餐.description.Add("已达到最大等级！（目前攻击 -2000）");
            up_勇敢生肉套餐.set_init_cost(up_勇敢生肉套餐_ct, 0, up_勇敢生肉套餐_ct.Count);
            upgrades.Add("勇敢生肉套餐", up_勇敢生肉套餐);
            #endregion

            //火爆蔬菜烧烤
            #region
            upgrade up_火爆蔬菜烧烤 = new upgrade("火爆蔬菜烧烤", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_食物_火爆蔬菜烧烤_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_火爆蔬菜烧烤_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_火爆蔬菜烧烤_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤植物", 100e6)),
                new Tuple<string, double2>("糖方块", 5e9)),
                new Tuple<string, double2>("烈焰粉末", 400));
            up_火爆蔬菜烧烤.description.Add("能量 +11000，减速值下降速度 +2.5");
            up_火爆蔬菜烧烤_ct.Add(up_火爆蔬菜烧烤_lv1);
            List<Tuple<string, double2>> up_火爆蔬菜烧烤_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤植物", 20e9)),
                new Tuple<string, double2>("糖方块", 199e9)),
                new Tuple<string, double2>("烈焰粉末", 25e3));
            up_火爆蔬菜烧烤.description.Add("能量 +18000，减速值下降速度 +4\n（目前减速值下降速度 +2.5）");
            up_火爆蔬菜烧烤_ct.Add(up_火爆蔬菜烧烤_lv2);
            List<Tuple<string, double2>> up_火爆蔬菜烧烤_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤植物", 1.35e12)),
                new Tuple<string, double2>("魔法糖浆", 60e3)),
                new Tuple<string, double2>("烈焰粉末", 433e3));
            up_火爆蔬菜烧烤.description.Add("能量 +25500，减速值下降速度 +5.5\n（目前减速值下降速度 +6.5）");
            up_火爆蔬菜烧烤_ct.Add(up_火爆蔬菜烧烤_lv3);
            List<Tuple<string, double2>> up_火爆蔬菜烧烤_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤植物", 640e12)),
                new Tuple<string, double2>("魔法糖浆", 1.2e6)),
                new Tuple<string, double2>("烈焰粉末", 25e6));
            up_火爆蔬菜烧烤.description.Add("能量 +32500，减速值下降速度 +8\n（目前减速值下降速度 +12）");
            up_火爆蔬菜烧烤_ct.Add(up_火爆蔬菜烧烤_lv4);

            up_火爆蔬菜烧烤.description.Add("已达到最大等级！（目前减速值下降速度 +20）");
            up_火爆蔬菜烧烤.set_init_cost(up_火爆蔬菜烧烤_ct, 0, up_火爆蔬菜烧烤_ct.Count);
            upgrades.Add("火爆蔬菜烧烤", up_火爆蔬菜烧烤);
            #endregion

            //经典BBQ大餐
            #region
            upgrade up_经典BBQ大餐 = new upgrade("经典BBQ大餐", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_食物_经典BBQ大餐_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_经典BBQ大餐_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_经典BBQ大餐_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤动物", 100e6)),
                new Tuple<string, double2>("糖方块", 5e12)),
                new Tuple<string, double2>("魔法粉末", 1.2e6));
            up_经典BBQ大餐.description.Add("能量 +16000，能量可提供 +40%的最大游戏速度");
            up_经典BBQ大餐_ct.Add(up_经典BBQ大餐_lv1);
            List<Tuple<string, double2>> up_经典BBQ大餐_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤动物", 15e9)),
                new Tuple<string, double2>("糖方块", 15e15)),
                new Tuple<string, double2>("魔法粉末", 60e6));
            up_经典BBQ大餐.description.Add("能量 +25000，能量可提供 +60%的最大游戏速度（目前 +40%最大游戏速度）");
            up_经典BBQ大餐_ct.Add(up_经典BBQ大餐_lv2);
            List<Tuple<string, double2>> up_经典BBQ大餐_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("烤动物", 6.7e12)),
                new Tuple<string, double2>("魔法糖浆", 320e6)),
                new Tuple<string, double2>("魔法粉末", 1e9));
            up_经典BBQ大餐.description.Add("能量 +36000，能量可提供 +100%的最大游戏速度\n（目前 +100%最大游戏速度）");
            up_经典BBQ大餐_ct.Add(up_经典BBQ大餐_lv3);

            up_经典BBQ大餐.description.Add("已达到最大等级！（目前 +200%最大游戏速度）");
            up_经典BBQ大餐.set_init_cost(up_经典BBQ大餐_ct, 0, up_经典BBQ大餐_ct.Count);
            upgrades.Add("经典BBQ大餐", up_经典BBQ大餐);
            #endregion

            //能量饮料
            #region
            upgrade up_能量饮料 = new upgrade("能量饮料", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_食物_能量饮料_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_能量饮料_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_能量饮料_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔法粉末", 5e6)),
                new Tuple<string, double2>("魔法糖浆", 64e3)),
                new Tuple<string, double2>("能量", 4000));
            up_能量饮料.description.Add("能量 +28000");
            up_能量饮料_ct.Add(up_能量饮料_lv1);
            List<Tuple<string, double2>> up_能量饮料_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("橙色粒子", 25e6)),
                new Tuple<string, double2>("无色粒子", 5e6)),
                new Tuple<string, double2>("能量", 12000));
            up_能量饮料.description.Add("能量 +48000");
            up_能量饮料_ct.Add(up_能量饮料_lv2);
            List<Tuple<string, double2>> up_能量饮料_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粉末", 1.5e24)),
                new Tuple<string, double2>("魔法糖浆", 45e6)),
                new Tuple<string, double2>("能量", 36000));
            up_能量饮料.description.Add("能量 +90000");
            up_能量饮料_ct.Add(up_能量饮料_lv3);

            up_能量饮料.description.Add("已达到最大等级！");
            up_能量饮料.set_init_cost(up_能量饮料_ct, 0, up_能量饮料_ct.Count);
            upgrades.Add("能量饮料", up_能量饮料);
            #endregion

            //冰镇果汁
            #region
            upgrade up_冰镇果汁 = new upgrade("冰镇果汁", "制造")
            {
                can_reset = true,
                unlocked = false
            };
            m.制造_次_食物_冰镇果汁_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_冰镇果汁_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_冰镇果汁_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 1.8e15)),
                new Tuple<string, double2>("蓝色粒子", 150e3)),
                new Tuple<string, double2>("魔法粉末", 4e6));
            up_冰镇果汁.description.Add("能量 +20000，燃料速度值×0.9，能量消耗×0.8");
            up_冰镇果汁_ct.Add(up_冰镇果汁_lv1);
            List<Tuple<string, double2>> up_冰镇果汁_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 900e15)),
                new Tuple<string, double2>("蓝色粒子", 3e6)),
                new Tuple<string, double2>("魔法糖浆", 25e6));
            up_冰镇果汁.description.Add("能量 +25000，燃料速度值×0.9，能量消耗×0.75" +
                "（目前燃料速度值×0.9，能量消耗×0.8）");
            up_冰镇果汁_ct.Add(up_冰镇果汁_lv2);
            List<Tuple<string, double2>> up_冰镇果汁_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("植物原料", 36e21)),
                new Tuple<string, double2>("蓝色粒子", 666e6)),
                new Tuple<string, double2>("魔法粉末", 927e6));
            up_冰镇果汁.description.Add("能量 +30000，燃料速度值×0.864，能量消耗×0.667" +
                "（目前燃料速度值×0.81，能量消耗×0.6）");
            up_冰镇果汁_ct.Add(up_冰镇果汁_lv3);

            up_冰镇果汁.description.Add("已达到最大等级！（目前燃料速度值×0.7，能量消耗×0.4）");
            up_冰镇果汁.set_init_cost(up_冰镇果汁_ct, 0, up_冰镇果汁_ct.Count);
            upgrades.Add("冰镇果汁", up_冰镇果汁);
            #endregion

            #endregion

            //no.3 战斗:
            #region

            战斗_options = make_group(m.战斗_option_grid);

            战斗_洁白世界_enemies = make_group(m.战斗_场景_洁白世界_enemy_grid);
            战斗_enemies.Add(战斗_洁白世界_enemies);
            战斗_草原_enemies = make_group(m.战斗_场景_草原_enemy_grid);
            战斗_enemies.Add(战斗_草原_enemies);
            战斗_死火山_enemies = make_group(m.战斗_场景_死火山_enemy_grid);
            战斗_enemies.Add(战斗_死火山_enemies);
            战斗_机关屋_enemies = make_group(m.战斗_场景_机关屋_enemy_grid);
            战斗_enemies.Add(战斗_机关屋_enemies);
            战斗_魔境_enemies = make_group(m.战斗_场景_魔境_enemy_grid);
            战斗_enemies.Add(战斗_魔境_enemies);

            战斗_自动攻击风格 = make_group(m.战斗_玩家_攻击风格_自动_grid);
            //更多group……

            Dictionary<string, resource> res_group_战斗 = new Dictionary<string, resource>();
            res_table.Add("战斗", res_group_战斗);

            //自动
            attack_form af_普通 = new attack_form("普通", true, false, 0)
            {
                can_reset = true,
                unlocked = true
            };
            af_普通.set_factor(1, 1, 1);
            attack_forms.Add("普通", af_普通);

            you.auto_attack_form = af_普通;
            group_process(战斗_自动攻击风格, m.战斗_玩家_攻击风格_自动_普通, false, getSCB(Color.FromRgb(100, 255, 100)));

            attack_form af_重击 = new attack_form("重击", true, false, 0)
            {
                can_reset = true,
                unlocked = true
            };
            af_重击.set_factor(1.5, 2, 0.8);
            attack_forms.Add("重击", af_重击);

            attack_form af_连斩 = new attack_form("连斩", true, false, 0)
            {
                can_reset = true,
                unlocked = false
            };
            af_连斩.set_factor(0.5, 1, 1);
            attack_forms.Add("连斩", af_连斩);

            attack_form af_光球 = new attack_form("光球", true, false, 0)
            {
                can_reset = true,
                unlocked = false
            };
            af_光球.set_factor(0.4, 0.5, 3);
            af_光球.overkill = 0.2;
            af_光球.reseter.overkill = 0.2;
            attack_forms.Add("光球", af_光球);

            //手动
            attack_form af_喷雾 = new attack_form("喷雾", false, true, 0.2)
            {
                can_reset = true,
                unlocked = false
            };
            af_喷雾.set_factor(0.4, 0, 1);
            attack_forms.Add("喷雾", af_喷雾);

            attack_form af_烈焰 = new attack_form("烈焰", false, true, 0)
            {
                can_reset = true,
                unlocked = false
            };
            af_烈焰.set_factor(5, 0, 0);
            af_烈焰.skill = true;
            af_烈焰.skill_time = 100;
            af_烈焰.attack_time = 1;
            af_烈焰.reseter.attack_time = 1;
            attack_forms.Add("烈焰", af_烈焰);

            //敌人
            #region
            //洁白世界
            #region
            {
                //经验 = 生命 ^ 1.23506
                //洁白世界::白色粒子
                resource res_白色粒子 = new resource(1, 0, "白色粒子",
                    getSCB(Color.FromRgb(255, 255, 255)))
                {
                    unlocked = true
                };
                res_group_战斗.Add("白色粒子", res_白色粒子);

                Dictionary<string, enemy> enemy_洁白世界 = new Dictionary<string, enemy>();
                enemies.Add("洁白世界", enemy_洁白世界);

                #region
                //洁白世界::白花
                Dictionary<string, Tuple<double2, double2>> enemy_洁白世界_白花_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_洁白世界_白花_loot.Add("白色方块", new Tuple<double2, double2>(3500, 2.55));
                enemy enemy_洁白世界_白花 = new enemy("洁白世界", "白花", 6, 2, 7, 2.35, 15, enemy_洁白世界_白花_loot, getSCB(Color.FromRgb(255, 255, 255)))
                {
                    des = "一朵白花，由很多小小的白色方块组成。"
                };
                enemy_洁白世界.Add("白花", enemy_洁白世界_白花);
                #endregion

                #region
                //洁白世界::雪
                Dictionary<string, Tuple<double2, double2>> enemy_洁白世界_雪_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_洁白世界_雪_loot.Add("白色粉末", new Tuple<double2, double2>(300, 1.867));
                enemy enemy_洁白世界_雪 = new enemy("洁白世界", "雪", 15, 2, 35, 2.37, 15, enemy_洁白世界_雪_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_洁白世界_雪.set_special(0.4, 0, 0);
                enemy_洁白世界_雪.set_special_exponent(0.1, 0, 0);
                enemy_洁白世界_雪.des = "美丽的雪，但你从中感受到寒冷。因此攻击它之后，攻击时间会暂时延长。";
                enemy_洁白世界.Add("雪", enemy_洁白世界_雪);
                #endregion

                #region
                //洁白世界::白砖
                Dictionary<string, Tuple<double2, double2>> enemy_洁白世界_白砖_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_洁白世界_白砖_loot.Add("白色方块", new Tuple<double2, double2>(84000, 2.55));
                enemy enemy_洁白世界_白砖 = new enemy("洁白世界", "白砖", 50, 2, 130, 2.35, 15, enemy_洁白世界_白砖_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_洁白世界_白砖.set_special(0, 10, 0);
                enemy_洁白世界_白砖.set_special_exponent(0, 2.1, 0);
                enemy_洁白世界_白砖.des = "一个很大的白色方块，很坚硬。攻击它时的伤害会减少。";
                enemy_洁白世界.Add("白砖", enemy_洁白世界_白砖);
                #endregion

                #region
                //洁白世界::糖
                Dictionary<string, Tuple<double2, double2>> enemy_洁白世界_糖_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_洁白世界_糖_loot.Add("白色粉末", new Tuple<double2, double2>(1800, 2.364));
                enemy enemy_洁白世界_糖 = new enemy("洁白世界", "糖", 120, 2.6, 300, 3.255, 15, enemy_洁白世界_糖_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_洁白世界_糖.set_special(0, 0, 35);
                enemy_洁白世界_糖.set_special_exponent(0, 0, 2.75);
                enemy_洁白世界_糖.des = "很甜的白色颗粒，会随着时间恢复血量。";
                enemy_洁白世界.Add("糖", enemy_洁白世界_糖);
                #endregion

                #region
                //洁白世界::白墙
                Dictionary<string, Tuple<double2, double2>> enemy_洁白世界_白墙_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_洁白世界_白墙_loot.Add("白色方块", new Tuple<double2, double2>(1.6e6, 3.54));
                enemy enemy_洁白世界_白墙 = new enemy("洁白世界", "白墙", 500, 2.55, 2000, 3.178, 15, enemy_洁白世界_白墙_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_洁白世界_白墙.set_special(0, 60, 100);
                enemy_洁白世界_白墙.set_special_exponent(0, 2.5, 2.5);
                enemy_洁白世界_白墙.des = "一堵巨大的白色墙，在很坚硬的同时还有神奇的恢复功能。";
                enemy_洁白世界.Add("白墙", enemy_洁白世界_白墙);
                #endregion

                #region
                //洁白世界::白色粒子
                Dictionary<string, Tuple<double2, double2>> enemy_洁白世界_白色粒子_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_洁白世界_白色粒子_loot.Add("白色粒子", new Tuple<double2, double2>(1, 1.5));
                enemy enemy_洁白世界_白色粒子 = new enemy("洁白世界", "白色粒子", 24000, 2.5, 300e3, 3.10, 15, enemy_洁白世界_白色粒子_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_洁白世界_白色粒子.set_special(1.4, 0, 4000);
                enemy_洁白世界_白色粒子.set_special_exponent(0.3, 0, 2.4);
                enemy_洁白世界_白色粒子.des = "极其活跃的白色粒子，具有魔法的力量。击败它以永久解锁“魔法”面板。";
                enemy_洁白世界.Add("白色粒子", enemy_洁白世界_白色粒子);
                #endregion
            }
            #endregion
            //草原
            #region
            {
                //草原::植物原料
                resource res_植物原料 = new resource(7, 0, "植物原料",
                    getSCB(Color.FromRgb(0, 150, 0)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("植物原料", res_植物原料);

                //草原::动物原料
                resource res_动物原料 = new resource(8, 0, "动物原料",
                    getSCB(Color.FromRgb(255, 255, 0)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("动物原料", res_动物原料);

                //草原::绿色粒子
                resource res_绿色粒子 = new resource(2, 0, "绿色粒子",
                    getSCB(Color.FromRgb(0, 255, 0)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("绿色粒子", res_绿色粒子);

                //草原::无色粒子
                resource res_无色粒子 = new resource(6, 0, "无色粒子",
                    getSCB(Color.FromRgb(255, 255, 255)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("无色粒子", res_无色粒子);

                Dictionary<string, enemy> enemy_草原 = new Dictionary<string, enemy>();
                enemies.Add("草原", enemy_草原);

                //草原::草
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_草原_草_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_草原_草_loot.Add("植物原料", new Tuple<double2, double2>(15, 1.442));
                enemy enemy_草原_草 = new enemy("草原", "草", 100e3, 1.5, 1.25e6, 1.65, 30, enemy_草原_草_loot, getSCB(Color.FromRgb(0, 150, 0)));
                enemy_草原_草.set_special(0, 0, 1000);
                enemy_草原_草.set_special_exponent(0, 0, 1.5);
                enemy_草原_草.des = "随处可见的草，拥有一点点恢复能力。";
                enemy_草原.Add("草", enemy_草原_草);
                #endregion

                //草原::虫子
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_草原_虫子_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_草原_虫子_loot.Add("动物原料", new Tuple<double2, double2>(4, 1.3));
                enemy enemy_草原_虫子 = new enemy("草原", "虫子", 320e3, 1.45, 5.25e6, 1.585, 20, enemy_草原_虫子_loot, getSCB(Color.FromRgb(255, 127, 127)));
                enemy_草原_虫子.set_special(0, 4000, 1000);
                enemy_草原_虫子.set_special_exponent(0, 1.4, 1.5);
                enemy_草原_虫子.des = "某些人最讨厌的东西。";
                enemy_草原.Add("虫子", enemy_草原_虫子);
                #endregion

                //草原::微风
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_草原_微风_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_草原_微风_loot.Add("无色粒子", new Tuple<double2, double2>(0.007, 1.23));
                enemy enemy_草原_微风 = new enemy("草原", "微风", 1.5e6, 1.6, 70e6, 1.8, 20, enemy_草原_微风_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_草原_微风.set_special(2.5, 0, 0);
                enemy_草原_微风.set_special_exponent(0.5, 0, 0);
                enemy_草原_微风.des = "对不够强大的人来说它其实是有力的风。";
                enemy_草原.Add("微风", enemy_草原_微风);
                #endregion

                //草原::羊
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_草原_羊_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_草原_羊_loot.Add("动物原料", new Tuple<double2, double2>(45, 1.454));
                enemy enemy_草原_羊 = new enemy("草原", "羊", 10e6, 1.7, 400e6, 1.93, 20, enemy_草原_羊_loot, getSCB(Color.FromRgb(255, 255, 0)));
                enemy_草原_羊.set_special(0, 1e6, 5e5);
                enemy_草原_羊.set_special_exponent(0, 1.7, 1.7);
                enemy_草原_羊.des = "有时候你可以提前看一看接下来的敌人，它们不一定都比上一个强很多。";
                enemy_草原.Add("羊", enemy_草原_羊);
                #endregion

                //草原::绿色粒子
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_草原_绿色粒子_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_草原_绿色粒子_loot.Add("绿色粒子", new Tuple<double2, double2>(1, 1.389));
                enemy enemy_草原_绿色粒子 = new enemy("草原", "绿色粒子", 5e6, 2.1, 180e6, 2.52, 20, enemy_草原_绿色粒子_loot, getSCB(Color.FromRgb(0, 255, 0)));
                enemy_草原_绿色粒子.set_special(1.5, 0, 5e7);
                enemy_草原_绿色粒子.set_special_exponent(0.25, 0, 2.25);
                enemy_草原_绿色粒子.des = "这可能是生命的源头。";
                enemy_草原.Add("绿色粒子", enemy_草原_绿色粒子);
                #endregion

                //草原::土丘
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_草原_土丘_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_草原_土丘_loot.Add("泥土方块", new Tuple<double2, double2>(60e3, 1.917));
                enemy enemy_草原_土丘 = new enemy("草原", "土丘", 35e6, 1.9, 3e9, 2.21, 20, enemy_草原_土丘_loot, getSCB(Color.FromRgb(255, 127, 0)));
                enemy_草原_土丘.set_special(0, 20e6, 0);
                enemy_草原_土丘.set_special_exponent(0, 1.85, 0);
                enemy_草原_土丘.des = "这么多泥土方块有什么用？击败它以永久解锁魔法祭坛的强化选项。";
                enemy_草原.Add("土丘", enemy_草原_土丘);
                #endregion
            }
            #endregion

            //死火山
            #region
            {
                //死火山::红色粒子
                resource res_红色粒子 = new resource(3, 0, "红色粒子",
                    getSCB(Color.FromRgb(255, 0, 0)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("红色粒子", res_红色粒子);

                Dictionary<string, enemy> enemy_死火山 = new Dictionary<string, enemy>();
                enemies.Add("死火山", enemy_死火山);

                //死火山::大树     经验 ^ 0.7306 植物原料
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_死火山_大树_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_死火山_大树_loot.Add("植物原料", new Tuple<double2, double2>(18e3, 1.7));
                enemy_死火山_大树_loot.Add("木头方块", new Tuple<double2, double2>(400, 1.39));
                enemy enemy_死火山_大树 = new enemy("死火山", "大树", 500e6, 1.8, 50e9, 2.067, 40, enemy_死火山_大树_loot, getSCB(Color.FromRgb(0, 210, 0)));
                enemy_死火山_大树.set_special(0, 0, 15e6);
                enemy_死火山_大树.set_special_exponent(0, 0, 1.8);
                enemy_死火山_大树.des = "非常大的树。";
                enemy_死火山.Add("大树", enemy_死火山_大树);
                #endregion

                //死火山::魔法果
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_死火山_魔法果_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_死火山_魔法果_loot.Add("植物原料", new Tuple<double2, double2>(90e3, 1.528));
                enemy_死火山_魔法果_loot.Add("魔力", new Tuple<double2, double2>(1500, 1.25));
                enemy enemy_死火山_魔法果 = new enemy("死火山", "魔法果", 2.4e9, 1.6, 400e9, 1.787, 30, enemy_死火山_魔法果_loot, getSCB(Color.FromRgb(0, 200, 255)));
                enemy_死火山_魔法果.set_special(6.6, 0, 8e7);
                enemy_死火山_魔法果.set_special_exponent(0.6, 0, 1.6);
                enemy_死火山_魔法果.des = "魔法植物是指拥有魔力的植物，比如魔法果。吃掉魔法果可以得到一些魔力。";
                enemy_死火山.Add("魔法果", enemy_死火山_魔法果);
                #endregion


                //死火山::野猪
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_死火山_野猪_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_死火山_野猪_loot.Add("动物原料", new Tuple<double2, double2>(6300, 1.331));
                enemy enemy_死火山_野猪 = new enemy("死火山", "野猪", 10e9, 1.5, 2e12, 1.672, 20, enemy_死火山_野猪_loot, getSCB(Color.FromRgb(255, 175, 0)));
                enemy_死火山_野猪.set_special(3.6, 1e9, 2e8);
                enemy_死火山_野猪.set_special_exponent(0.1, 1.5, 1.6);
                enemy_死火山_野猪.des = "战斗力强的野猪。";
                enemy_死火山.Add("野猪", enemy_死火山_野猪);
                #endregion

                //死火山::云
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_死火山_云_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_死火山_云_loot.Add("白色方块", new Tuple<double2, double2>(30e15, 2.55));
                enemy_死火山_云_loot.Add("白色粉末", new Tuple<double2, double2>(35e9, 1.867));
                enemy_死火山_云_loot.Add("无色粒子", new Tuple<double2, double2>(0.228, 1.359));
                enemy enemy_死火山_云 = new enemy("死火山", "云", 4.5e9, 2, 9e12, 2.36, 20, enemy_死火山_云_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_死火山_云.set_special(55, 0, 0);
                enemy_死火山_云.set_special_exponent(5, 0, 0);
                enemy_死火山_云.des = "这座山的高度足以让你触碰到云。";
                enemy_死火山.Add("云", enemy_死火山_云);
                #endregion

                //死火山::红色粒子
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_死火山_红色粒子_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_死火山_红色粒子_loot.Add("红色粒子", new Tuple<double2, double2>(44, 1.281));
                enemy enemy_死火山_红色粒子 = new enemy("死火山", "红色粒子", 1e12, 1.75, 520e12, 2, 20, enemy_死火山_红色粒子_loot, getSCB(Color.FromRgb(255, 0, 0)));
                enemy_死火山_红色粒子.set_special(0, 0, 0);
                enemy_死火山_红色粒子.set_special_exponent(0, 0, 0);
                enemy_死火山_红色粒子.des = "从这座死火山的顶端冒出来的红色粒子，为什么会有这么多生命值？";
                enemy_死火山.Add("红色粒子", enemy_死火山_红色粒子);
                #endregion

                //死火山::陨石
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_死火山_陨石_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_死火山_陨石_loot.Add("石头方块", new Tuple<double2, double2>(135e6, 1.834));
                enemy_死火山_陨石_loot.Add("铜矿", new Tuple<double2, double2>(7, 1.8));
                enemy_死火山_陨石_loot.Add("铁矿", new Tuple<double2, double2>(2, 1.8));
                enemy enemy_死火山_陨石 = new enemy("死火山", "陨石", 1e12, 1.8, 770e12, 2.067, 20, enemy_死火山_陨石_loot, getSCB(Color.FromRgb(150, 200, 255)));
                enemy_死火山_陨石.set_special(0, 3e11, 0);
                enemy_死火山_陨石.set_special_exponent(0, 1.8, 0);
                enemy_死火山_陨石.des = "你发现了陨石！若要获得战利品，必须先做一把镐子。";
                enemy_死火山.Add("陨石", enemy_死火山_陨石);
                #endregion
            }
            #endregion

            //机关屋
            #region
            {
                //机关屋::橙色粒子
                resource res_橙色粒子 = new resource(4, 0, "橙色粒子",
                    getSCB(Color.FromRgb(255, 127, 0)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("橙色粒子", res_橙色粒子);

                Dictionary<string, enemy> enemy_机关屋 = new Dictionary<string, enemy>();
                enemies.Add("机关屋", enemy_机关屋);

                //   经验 ^ 0.7306 植物原料
                //机关屋::土锤 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_机关屋_土锤_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_机关屋_土锤_loot.Add("泥土方块", new Tuple<double2, double2>(600e6, 1.418));
                enemy enemy_机关屋_土锤 = new enemy("机关屋", "土锤", 2.5e12, 1.55, 3.75e15, 1.718, 40, enemy_机关屋_土锤_loot, getSCB(Color.FromRgb(255, 127, 0)));
                enemy_机关屋_土锤.set_special(34, 800e9, 0);
                enemy_机关屋_土锤.set_special_exponent(4, 1.5, 0);
                enemy_机关屋_土锤.des = "如果是比草原上的土丘还大的锤子，一锤敲下来大概谁都会死。（幸好您没有血条）";
                enemy_机关屋.Add("土锤", enemy_机关屋_土锤);
                #endregion

                //机关屋::煤球 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_机关屋_煤球_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_机关屋_煤球_loot.Add("煤", new Tuple<double2, double2>(90, 1.452));
                enemy enemy_机关屋_煤球 = new enemy("机关屋", "煤球", 20e12, 1.75, 32e15, 1.996,
                    40, enemy_机关屋_煤球_loot, getSCB(Color.FromRgb(150, 150, 150)));
                enemy_机关屋_煤球.set_special(10.5, 2e12, 0);
                enemy_机关屋_煤球.set_special_exponent(1.5, 1.73, 0);
                enemy_机关屋_煤球.des = "这个很小的煤球滚得飞快。";
                enemy_机关屋.Add("煤球", enemy_机关屋_煤球);
                #endregion

                //机关屋::橙色粒子 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_机关屋_橙色粒子_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_机关屋_橙色粒子_loot.Add("橙色粒子", new Tuple<double2, double2>(120, 1.357));
                enemy enemy_机关屋_橙色粒子 = new enemy("机关屋", "橙色粒子", 15e12, 1.95, 60e15, 2.28,
                    30, enemy_机关屋_橙色粒子_loot, getSCB(Color.FromRgb(255, 127, 0)));
                enemy_机关屋_橙色粒子.set_special(0, 215e12, 0);
                enemy_机关屋_橙色粒子.set_special_exponent(0, 2, 0);
                enemy_机关屋_橙色粒子.des = "听说这是唯一一种有防御力的粒子，请不要对它进行任何有降低防御力或无视防御力效果的攻击，否则你将获得大量经验。";
                enemy_机关屋.Add("橙色粒子", enemy_机关屋_橙色粒子);
                #endregion

                //机关屋::铁气球 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_机关屋_铁气球_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_机关屋_铁气球_loot.Add("铁", new Tuple<double2, double2>(280, 1.85));
                enemy_机关屋_铁气球_loot.Add("无色粒子", new Tuple<double2, double2>(20, 1.311));
                enemy enemy_机关屋_铁气球 = new enemy("机关屋", "铁气球", 300e12, 1.85, 1.33e18, 2.138,
                    30, enemy_机关屋_铁气球_loot, getSCB(Color.FromRgb(168, 169, 170)));
                enemy_机关屋_铁气球.set_special(0, 300e12, 0);
                enemy_机关屋_铁气球.set_special_exponent(0, 1.85, 0);
                enemy_机关屋_铁气球.des = "铁皮的气球居然能够飞起来。";
                enemy_机关屋.Add("铁气球", enemy_机关屋_铁气球);
                #endregion

                //机关屋::铜风扇 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_机关屋_铜风扇_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_机关屋_铜风扇_loot.Add("铜", new Tuple<double2, double2>(80e3, 1.7));
                enemy enemy_机关屋_铜风扇 = new enemy("机关屋", "铜风扇", 5e15, 1.7, 300e18, 1.926,
                    30, enemy_机关屋_铜风扇_loot, getSCB(Color.FromRgb(186, 110, 64)));
                enemy_机关屋_铜风扇.set_special(225, 1.2e15, 0);
                enemy_机关屋_铜风扇.set_special_exponent(25, 1.7, 0);
                enemy_机关屋_铜风扇.des = "超强风力的铜风扇，最好离它远一些。";
                enemy_机关屋.Add("铜风扇", enemy_机关屋_铜风扇);
                #endregion

                //机关屋::钢弹 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_机关屋_钢弹_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_机关屋_钢弹_loot.Add("钢", new Tuple<double2, double2>(15, 1.75));
                enemy enemy_机关屋_钢弹 = new enemy("机关屋", "钢弹", 18e15, 1.75, 147e18, 1.996,
                    25, enemy_机关屋_钢弹_loot, getSCB(Color.FromRgb(200, 200, 200)));
                enemy_机关屋_钢弹.set_special(30, 32e15, 0);
                enemy_机关屋_钢弹.set_special_exponent(5, 1.75, 0);
                enemy_机关屋_钢弹.des = "小小的一颗钢弹，蕴含超大的能量。";
                enemy_机关屋.Add("钢弹", enemy_机关屋_钢弹);
                #endregion
            }
            #endregion


            //魔境
            #region
            {
                //魔境::蓝色粒子
                resource res_蓝色粒子 = new resource(5, 0, "蓝色粒子",
                    getSCB(Color.FromRgb(0, 63, 255)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("蓝色粒子", res_蓝色粒子);

                resource res_空间精华 = new resource(9, 0, "空间精华",
                    getSCB(Color.FromRgb(127, 63, 255)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("空间精华", res_空间精华);

                resource res_时间精华 = new resource(10, 0, "时间精华",
                    getSCB(Color.FromRgb(63, 127, 255)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("时间精华", res_时间精华);

                resource res_核心碎片 = new resource(11, 0, "核心碎片",
                    getSCB(Color.FromRgb(100, 200, 150)))
                {
                    unlocked = false
                };
                res_group_战斗.Add("核心碎片", res_核心碎片);

                Dictionary<string, enemy> enemy_魔境 = new Dictionary<string, enemy>();
                enemies.Add("魔境", enemy_魔境);

                //   经验 ^ 0.7306 植物原料

                //魔境::飞行白鲸 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_魔境_飞行白鲸_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_魔境_飞行白鲸_loot.Add("动物原料", new Tuple<double2, double2>(650e3, 1.63));
                enemy_魔境_飞行白鲸_loot.Add("白色粒子", new Tuple<double2, double2>(4500, 1.359));
                enemy enemy_魔境_飞行白鲸 = new enemy("魔境", "飞行白鲸", 15e12, 2, 16.7e15, 2.354,
                    30, enemy_魔境_飞行白鲸_loot, getSCB(Color.FromRgb(255, 255, 255)));
                enemy_魔境_飞行白鲸.set_special(0, 800e9, 10e12);
                enemy_魔境_飞行白鲸.set_special_exponent(0, 2, 2);
                enemy_魔境_飞行白鲸.des = "可爱肥美还会飞的白鲸。";
                enemy_魔境.Add("飞行白鲸", enemy_魔境_飞行白鲸);
                #endregion

                //魔境::治疗场 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_魔境_治疗场_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_魔境_治疗场_loot.Add("空间精华", new Tuple<double2, double2>(1, 1.1));
                enemy_魔境_治疗场_loot.Add("绿色粒子", new Tuple<double2, double2>(1000, 1.297));
                enemy enemy_魔境_治疗场 = new enemy("魔境", "治疗场", 100e12, 1.8, 165e15, 2.0667,
                    30, enemy_魔境_治疗场_loot, getSCB(Color.FromRgb(0, 255, 0)));
                enemy_魔境_治疗场.set_special(0, 0, 300e12);
                enemy_魔境_治疗场.set_special_exponent(0, 0, 1.8);
                enemy_魔境_治疗场.des = "治疗场在严格意义上不是一种场，它只是一堆治疗法阵构建的一片特殊区域。攻击它而非法阵仅仅是为了可持续发展。";
                enemy_魔境.Add("治疗场", enemy_魔境_治疗场);
                #endregion


                //魔境::火球 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_魔境_火球_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_魔境_火球_loot.Add("石油", new Tuple<double2, double2>(0.05, 1.424));
                enemy_魔境_火球_loot.Add("红色粒子", new Tuple<double2, double2>(730, 1.265));
                enemy enemy_魔境_火球 = new enemy("魔境", "火球", 1.8e15, 1.7, 5.8e18, 1.926,
                    30, enemy_魔境_火球_loot, getSCB(Color.FromRgb(255, 0, 0)));
                enemy_魔境_火球.set_special(0, 0, 0);
                enemy_魔境_火球.set_special_exponent(0, 0, 0);
                enemy_魔境_火球.des = "火球……似乎是很常见的东西，但是掉落物是石油的火球常见吗？";
                enemy_魔境.Add("火球", enemy_魔境_火球);
                #endregion

                //魔境::石头树人 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_魔境_石头树人_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_魔境_石头树人_loot.Add("魔石", new Tuple<double2, double2>(800, 1.65));  //800魔石 = 2.4M魔力
                enemy_魔境_石头树人_loot.Add("植物原料", new Tuple<double2, double2>(290e9, 1.7));
                enemy_魔境_石头树人_loot.Add("橙色粒子", new Tuple<double2, double2>(640, 1.297));
                enemy enemy_魔境_石头树人 = new enemy("魔境", "石头树人", 50e15, 1.8, 440e18, 2.0667,
                    30, enemy_魔境_石头树人_loot, getSCB(Color.FromRgb(255, 127, 0)));
                enemy_魔境_石头树人.set_special(0, 10e15, 25e15);
                enemy_魔境_石头树人.set_special_exponent(0, 1.8, 1.8);
                enemy_魔境_石头树人.des = "石头人身上长了很多树。";
                enemy_魔境.Add("石头树人", enemy_魔境_石头树人);
                #endregion

                //魔境::蓝色粒子 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_魔境_蓝色粒子_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_魔境_蓝色粒子_loot.Add("蓝色粒子", new Tuple<double2, double2>(1800, 1.161));
                enemy enemy_魔境_蓝色粒子 = new enemy("魔境", "蓝色粒子", 20e15, 1.4, 10e21, 1.515,
                    45, enemy_魔境_蓝色粒子_loot, getSCB(Color.FromRgb(0, 100, 255)));
                enemy_魔境_蓝色粒子.set_special(1800, 0, 0);
                enemy_魔境_蓝色粒子.set_special_exponent(300, 0, 0);
                enemy_魔境_蓝色粒子.des = "与温度无关，变得寒冷的只有精神。";
                enemy_魔境.Add("蓝色粒子", enemy_魔境_蓝色粒子);
                #endregion


                //魔境::核心碎片 
                #region
                Dictionary<string, Tuple<double2, double2>> enemy_魔境_核心碎片_loot = new Dictionary<string, Tuple<double2, double2>>();
                enemy_魔境_核心碎片_loot.Add("核心碎片", new Tuple<double2, double2>(1, 3));
                enemy_魔境_核心碎片_loot.Add("能量", new Tuple<double2, double2>(0.1, 1.25));
                enemy enemy_魔境_核心碎片 = new enemy("魔境", "核心碎片", 3e18, 10, 440e21, 17.181,
                    50, enemy_魔境_核心碎片_loot, getSCB(Color.FromRgb(100, 200, 150)));
                enemy_魔境_核心碎片.set_special(650, 200e15, 600e15);
                enemy_魔境_核心碎片.set_special_exponent(150, 10, 10);
                enemy_魔境_核心碎片.des = "核心不是随处可见的东西，随处可见的是它的碎片（至少在这里）。";
                enemy_魔境.Add("核心碎片", enemy_魔境_核心碎片);
                #endregion
            }
            #endregion

            //敌人 6-1 掉落木头


            //敌人结束
            #endregion




            //战斗结束
            #endregion




            //no.4 魔法：
            #region
            魔法_options = make_group(m.魔法_option_grid);
            LinearGradientBrush lgb = get_lgb();

            Dictionary<string, resource> res_group_魔法 = new Dictionary<string, resource>();
            res_table.Add("魔法", res_group_魔法);

            resource res_魔力 = new resource(1, 0, "魔力",
                getSCB(Color.FromRgb(0, 200, 255)))
            {
                unlocked = true
            };
            res_group_魔法.Add("魔力", res_魔力);

            resource res_魔法粉末 = new resource(2, 0, "魔法粉末",
                getSCB(Color.FromRgb(0, 200, 255)))
            {
                unlocked = true
            };
            res_group_魔法.Add("魔法粉末", res_魔法粉末);

            resource res_烈焰粉末 = new resource(3, 0, "烈焰粉末",
                getSCB(Color.FromRgb(255, 0, 0)))
            {
                unlocked = false
            };
            res_group_魔法.Add("烈焰粉末", res_烈焰粉末);

            resource res_魔法糖浆 = new resource(4, 0, "魔法糖浆",
                getSCB(Color.FromRgb(0, 255, 255)))
            {
                unlocked = false
            };
            res_group_魔法.Add("魔法糖浆", res_魔法糖浆);

            foreach (Grid g in m.魔法_祭坛_祭品_grid.Children)
            {
                g.Visibility = (Visibility)1;
            }
            m.魔法_祭坛_祭品_白色粒子_grid.Visibility = 0;

            magic_altar.add("白色粒子", 10);
            magic_altar.add("绿色粒子", 100);
            magic_altar.add("红色粒子", 500);
            magic_altar.add("橙色粒子", 2000);
            magic_altar.add("蓝色粒子", 5000);
            magic_altar.add("无色粒子", 10000);

            magic_altar.add("植物祭品", 80);
            magic_altar.add("动物祭品", 400);
            magic_altar.add("魔法糖浆", 4000);


            #region
            upgrade up_祭坛升级 = new upgrade("祭坛升级", "魔法")
            {
                can_reset = true,
                unlocked = false
            };
            m.魔法_祭坛_升级0_grid.Visibility = (Visibility)1;
            List<List<Tuple<string, double2>>> up_祭坛升级_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_祭坛升级_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 3e6)),
                new Tuple<string, double2>("魔力", 2.5e3));
            up_祭坛升级.description.Add("无效果\n");
            up_祭坛升级.description2.Add("祭坛能量获取 × 1.2\n魔力转换速度 × 2\n");
            up_祭坛升级_ct.Add(up_祭坛升级_lv1);
            List<Tuple<string, double2>> up_祭坛升级_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 400e6)),
                new Tuple<string, double2>("魔力", 500e3)),
                new Tuple<string, double2>("无色粒子", 30));
            up_祭坛升级.description.Add("祭坛能量获取 × 1.2\n魔力转换速度 × 2\n");
            up_祭坛升级.description2.Add("祭坛能量获取 × 1.5\n魔力转换速度 × 5\n");
            up_祭坛升级_ct.Add(up_祭坛升级_lv2);
            List<Tuple<string, double2>> up_祭坛升级_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 40e9)),
                new Tuple<string, double2>("魔力", 6.5e6)),
                new Tuple<string, double2>("无色粒子", 300));
            up_祭坛升级.description.Add("祭坛能量获取 × 1.5\n魔力转换速度 × 5\n");
            up_祭坛升级.description2.Add("祭坛能量获取 × 2\n魔力转换速度 × 30\n");
            up_祭坛升级_ct.Add(up_祭坛升级_lv3);
            List<Tuple<string, double2>> up_祭坛升级_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 2e15)),
                new Tuple<string, double2>("魔力", 20e9)),
                new Tuple<string, double2>("无色粒子", 250e3));
            up_祭坛升级.description.Add("祭坛能量获取 × 2\n魔力转换速度 × 30\n");
            up_祭坛升级.description2.Add("祭坛能量获取 × 2.8\n魔力转换速度 × 350\n");
            up_祭坛升级_ct.Add(up_祭坛升级_lv4);
            List<Tuple<string, double2>> up_祭坛升级_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("泥土方块", 50e18)),
                new Tuple<string, double2>("魔力", 2.25e12)),
                new Tuple<string, double2>("无色粒子", 20e6)),
                new Tuple<string, double2>("铜", 100e9));
            up_祭坛升级.description.Add("祭坛能量获取 × 2.8\n魔力转换速度 × 350\n");
            up_祭坛升级.description2.Add("祭坛能量获取 × 4.5\n魔力转换速度 × 7500\n");
            up_祭坛升级_ct.Add(up_祭坛升级_lv5);

            up_祭坛升级.description.Add("祭坛能量获取 × 4.5\n魔力转换速度 × 7500\n");
            up_祭坛升级.description2.Add("已达到最大等级！\n");
            up_祭坛升级.set_init_cost(up_祭坛升级_ct, 0, up_祭坛升级_ct.Count);
            upgrades.Add("祭坛升级", up_祭坛升级);
            #endregion

            enchant ec_魔法粉末 = new enchant("魔法粉末", lgb)
            {
                can_reset = true,
                unlocked = true
            };
            m.魔法_次_附魔_魔法粉末_grid.Visibility = 0;
            List<Tuple<string, double2, double2>> ec_魔法粉末_cost = new List<Tuple<string, double2, double2>>();
            ec_魔法粉末_cost.Add(new Tuple<string, double2, double2>("白色粉末", 1e6, 1.45));   //2.9528
            ec_魔法粉末_cost.Add(new Tuple<string, double2, double2>("魔力", 1, 1.25));         //3.3626
            ec_魔法粉末.set_init_cost(ec_魔法粉末_cost, 1, 100, 50, 0.9);
            ec_魔法粉末.set_produce("魔法粉末", 5, 1.15);
            enchants.Add("魔法粉末", ec_魔法粉末);

            enchant ec_烈焰粉末 = new enchant("烈焰粉末", lgb)
            {
                can_reset = true,
                unlocked = false
            };
            m.魔法_次_附魔_烈焰粉末_grid.Visibility = (Visibility)1;
            List<Tuple<string, double2, double2>> ec_烈焰粉末_cost = new List<Tuple<string, double2, double2>>();
            ec_烈焰粉末_cost.Add(new Tuple<string, double2, double2>("白色粉末", 200e6, 1.5));  //1.7165
            ec_烈焰粉末_cost.Add(new Tuple<string, double2, double2>("魔力", 500, 1.25));       //1.7912
            ec_烈焰粉末_cost.Add(new Tuple<string, double2, double2>("红色粒子", 1, 1.23));     //1.7831
            ec_烈焰粉末.set_init_cost(ec_烈焰粉末_cost, 1, 100, 30, 0.9);
            ec_烈焰粉末.set_produce("烈焰粉末", 1, 1.306);
            enchants.Add("烈焰粉末", ec_烈焰粉末);

            enchant ec_魔法糖浆 = new enchant("魔法糖浆", lgb)
            {
                can_reset = true,
                unlocked = false
            };
            m.魔法_次_附魔_魔法糖浆_grid.Visibility = (Visibility)1;
            List<Tuple<string, double2, double2>> ec_魔法糖浆_cost = new List<Tuple<string, double2, double2>>();
            ec_魔法糖浆_cost.Add(new Tuple<string, double2, double2>("糖浆", 1, 1.32));        //2.2599
            ec_魔法糖浆_cost.Add(new Tuple<string, double2, double2>("魔力", 200, 1.3));       //2.25
            ec_魔法糖浆.set_init_cost(ec_魔法糖浆_cost, 1, 100, 100, 0.9);
            ec_魔法糖浆.set_produce("魔法糖浆", 1, 1.2);
            enchants.Add("魔法糖浆", ec_魔法糖浆);   //拿去喂祭坛得到双倍魔力


            // 探索魔法
            #region
            spell sp_探索魔法 = new spell("探索魔法", "魔法")
            {
                can_reset = false,
                unlocked = true
            };
            m.魔法_法术_探索魔法_grid.Visibility = (Visibility)0;
            sp_探索魔法.add_time(40);
            List<List<Tuple<string, double2>>> sp_探索魔法_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> sp_探索魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 200));
            sp_探索魔法.description.Add("解锁下一个战斗场景和一个工具。\n以及一种新的方块。");
            sp_探索魔法_ct.Add(sp_探索魔法_lv1);

            sp_探索魔法.add_time(270);
            List<Tuple<string, double2>> sp_探索魔法_lv2 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 720e3));
            sp_探索魔法.description.Add("解锁下一个战斗场景。\n以及一种新方块、一种新附魔和一种新药水。");
            sp_探索魔法_ct.Add(sp_探索魔法_lv2);

            sp_探索魔法.add_time(450);
            List<Tuple<string, double2>> sp_探索魔法_lv3 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 5.4e6));
            sp_探索魔法.description.Add("解锁采矿、“制造”中的食物、能量。\n以及一种新的方块、一种新药水和两种新的工具。");
            sp_探索魔法_ct.Add(sp_探索魔法_lv3);

            sp_探索魔法.add_time(2000);
            List<Tuple<string, double2>> sp_探索魔法_lv4 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 400e6));
            sp_探索魔法.description.Add("解锁下两个战斗场景！");
            sp_探索魔法_ct.Add(sp_探索魔法_lv4);

            sp_探索魔法.add_time(2500000);
            List<Tuple<string, double2>> sp_探索魔法_lv5 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 25e15));
            sp_探索魔法.description.Add("解锁下一个战斗场景和娱乐面板！");
            sp_探索魔法_ct.Add(sp_探索魔法_lv5);

            sp_探索魔法.description.Add("已达到最大等级！");
            sp_探索魔法.set_init_cost(sp_探索魔法_ct, 0, sp_探索魔法_ct.Count);
            upgrades.Add("探索魔法", sp_探索魔法);
            #endregion

            // 法术创作
            #region
            spell sp_法术创作 = new spell("法术创作", "魔法")
            {
                can_reset = false,
                unlocked = true
            };
            m.魔法_法术_法术创作_grid.Visibility = (Visibility)0;

            sp_法术创作.add_time(30);
            List<List<Tuple<string, double2>>> sp_法术创作_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> sp_法术创作_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 600));
            sp_法术创作.description.Add("解锁白色魔法。");
            sp_法术创作_ct.Add(sp_法术创作_lv1);

            sp_法术创作.add_time(50);
            List<Tuple<string, double2>> sp_法术创作_lv2 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 3.6e3));
            sp_法术创作.description.Add("解锁药水。");
            sp_法术创作_ct.Add(sp_法术创作_lv2);

            sp_法术创作.add_time(75);
            List<Tuple<string, double2>> sp_法术创作_lv3 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 150e3));
            sp_法术创作.description.Add("解锁绿色魔法。");
            sp_法术创作_ct.Add(sp_法术创作_lv3);

            sp_法术创作.add_time(750);
            List<Tuple<string, double2>> sp_法术创作_lv4 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 6e6));
            sp_法术创作.description.Add("解锁红色魔法。");
            sp_法术创作_ct.Add(sp_法术创作_lv4);

            sp_法术创作.add_time(1800);
            List<Tuple<string, double2>> sp_法术创作_lv5 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 80e6));
            sp_法术创作.description.Add("解锁新的附魔：魔法糖浆。");
            sp_法术创作_ct.Add(sp_法术创作_lv5);

            sp_法术创作.add_time(3600);
            List<Tuple<string, double2>> sp_法术创作_lv6 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 320e6));
            sp_法术创作.description.Add("解锁橙色魔法。");
            sp_法术创作_ct.Add(sp_法术创作_lv6);

            sp_法术创作.add_time(7500);
            List<Tuple<string, double2>> sp_法术创作_lv7 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 1.25e9));
            sp_法术创作.description.Add("解锁蓝色魔法。");
            sp_法术创作_ct.Add(sp_法术创作_lv7);

            sp_法术创作.add_time(12500);
            List<Tuple<string, double2>> sp_法术创作_lv8 =
                 upgrade_cost_adder(upgrade_cost_adder(null, null),
                 new Tuple<string, double2>("魔力", 4.8e9));
            sp_法术创作.description.Add("解锁无色魔法。");
            sp_法术创作_ct.Add(sp_法术创作_lv8);
            // 挖掘魔法

            sp_法术创作.description.Add("已达到最大等级！");
            sp_法术创作.set_init_cost(sp_法术创作_ct, 0, sp_法术创作_ct.Count);
            upgrades.Add("法术创作", sp_法术创作);
            #endregion

            // 白色魔法
            #region
            spell sp_白色魔法 = new spell("白色魔法", "魔法")
            {
                can_reset = true,
                normal = true,
                unlocked = false
            };
            m.魔法_法术_白色魔法_grid.Visibility = (Visibility)1;

            sp_白色魔法.add_passive("无效果");
            sp_白色魔法.add_active("不可用");


            List<List<Tuple<string, double2>>> sp_白色魔法_ct = new List<List<Tuple<string, double2>>>();
            List<List<Tuple<string, double2>>> sp_白色魔法_cta = new List<List<Tuple<string, double2>>>();
            sp_白色魔法_cta.Add(null);

            sp_白色魔法.add_time(15);
            List<Tuple<string, double2>> sp_白色魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 1000));
            List<Tuple<string, double2>> sp_白色魔法_lv1_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 1)),
                new Tuple<string, double2>("魔力", 25));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×2\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.5");
            sp_白色魔法.add_active("白色方块获取 ×2");
            sp_白色魔法.add_passive("白色方块获取 ×1.5");
            sp_白色魔法_ct.Add(sp_白色魔法_lv1);
            sp_白色魔法_cta.Add(sp_白色魔法_lv1_a);

            sp_白色魔法.add_time(120);
            List<Tuple<string, double2>> sp_白色魔法_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 120e3));
            List<Tuple<string, double2>> sp_白色魔法_lv2_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 10)),
                new Tuple<string, double2>("魔力", 200));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×1.25，白色粉末获取 ×1.6\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.33");
            sp_白色魔法.add_active("白色方块获取 ×2.5，白色粉末获取 ×1.6");
            sp_白色魔法.add_passive("白色方块获取 ×2");
            sp_白色魔法_ct.Add(sp_白色魔法_lv2);
            sp_白色魔法_cta.Add(sp_白色魔法_lv2_a);

            sp_白色魔法.add_time(200);
            List<Tuple<string, double2>> sp_白色魔法_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 4.4e6));
            List<Tuple<string, double2>> sp_白色魔法_lv3_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 100)),
                new Tuple<string, double2>("魔力", 1200));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×1.2，白色粉末获取 ×1.25\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.5");
            sp_白色魔法.add_active("白色方块获取 ×3，白色粉末获取 ×2");
            sp_白色魔法.add_passive("白色方块获取 ×3");
            sp_白色魔法_ct.Add(sp_白色魔法_lv3);
            sp_白色魔法_cta.Add(sp_白色魔法_lv3_a);

            sp_白色魔法.add_time(500);
            List<Tuple<string, double2>> sp_白色魔法_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 15e6));
            List<Tuple<string, double2>> sp_白色魔法_lv4_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 10e3)),
                new Tuple<string, double2>("魔力", 35e3));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×1.33，白色粉末获取 ×1.5\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.67");
            sp_白色魔法.add_active("白色方块获取 ×4，白色粉末获取 ×3");
            sp_白色魔法.add_passive("白色方块获取 ×5");
            sp_白色魔法_ct.Add(sp_白色魔法_lv4);
            sp_白色魔法_cta.Add(sp_白色魔法_lv4_a);

            sp_白色魔法.add_time(850);
            List<Tuple<string, double2>> sp_白色魔法_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 400e6));
            List<Tuple<string, double2>> sp_白色魔法_lv5_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 200e3)),
                new Tuple<string, double2>("魔力", 600e3));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×1.375，白色粉末获取 ×1.333\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.4");
            sp_白色魔法.add_active("白色方块获取 ×5.5，白色粉末获取 ×4");
            sp_白色魔法.add_passive("白色方块获取 ×7");
            sp_白色魔法_ct.Add(sp_白色魔法_lv5);
            sp_白色魔法_cta.Add(sp_白色魔法_lv5_a);

            sp_白色魔法.add_time(3200);
            List<Tuple<string, double2>> sp_白色魔法_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 5e9));
            List<Tuple<string, double2>> sp_白色魔法_lv6_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 2e6)),
                new Tuple<string, double2>("魔力", 15e6));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×1.273，白色粉末获取 ×1.5\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.286");
            sp_白色魔法.add_active("白色方块获取 ×7，白色粉末获取 ×6");
            sp_白色魔法.add_passive("白色方块获取 ×9");
            sp_白色魔法_ct.Add(sp_白色魔法_lv6);
            sp_白色魔法_cta.Add(sp_白色魔法_lv6_a);

            sp_白色魔法.add_time(8400);
            List<Tuple<string, double2>> sp_白色魔法_lv7 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 150e9)),
                new Tuple<string, double2>("白色粉末", 1e21));
            List<Tuple<string, double2>> sp_白色魔法_lv7_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("白色粒子", 80e6)),
                new Tuple<string, double2>("魔力", 600e6)),
                new Tuple<string, double2>("能量", 10));
            sp_白色魔法.description.Add("下一级主动效果变化：\n白色方块获取 ×1.429，白色粉末获取 ×1.333，\n战利品×1.5\n" +
                                        "下一级被动效果变化：\n白色方块获取 ×1.333");
            sp_白色魔法.add_active("白色方块获取 ×10，白色粉末获取 ×8，\n战利品×1.5");
            sp_白色魔法.add_passive("白色方块获取 ×12");
            sp_白色魔法_ct.Add(sp_白色魔法_lv7);
            sp_白色魔法_cta.Add(sp_白色魔法_lv7_a);

            sp_白色魔法.cost_table_active = sp_白色魔法_cta;
            sp_白色魔法.description.Add("已达到最大等级！");
            sp_白色魔法.set_init_cost(sp_白色魔法_ct, 0, sp_白色魔法_ct.Count);
            upgrades.Add("白色魔法", sp_白色魔法);
            #endregion

            // 绿色魔法
            #region
            spell sp_绿色魔法 = new spell("绿色魔法", "魔法")
            {
                can_reset = true,
                normal = true,
                unlocked = false
            };
            m.魔法_法术_绿色魔法_grid.Visibility = (Visibility)1;

            sp_绿色魔法.add_passive("无效果");
            sp_绿色魔法.add_active("不可用");

            List<List<Tuple<string, double2>>> sp_绿色魔法_ct = new List<List<Tuple<string, double2>>>();
            List<List<Tuple<string, double2>>> sp_绿色魔法_cta = new List<List<Tuple<string, double2>>>();
            sp_绿色魔法_cta.Add(null);

            sp_绿色魔法.add_time(30);
            List<Tuple<string, double2>> sp_绿色魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 80000));
            List<Tuple<string, double2>> sp_绿色魔法_lv1_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("绿色粒子", 1)),
                new Tuple<string, double2>("魔力", 150));
            sp_绿色魔法.description.Add("下一级主动效果变化：\n战斗经验获取 ×2，法术学习速度 ×3\n" +
                                        "下一级被动效果变化：\n转生点数获取 ×1.2");
            sp_绿色魔法.add_active("战斗经验获取 ×2，法术学习速度 ×3");
            sp_绿色魔法.add_passive("转生点数获取 ×1.2");
            sp_绿色魔法_ct.Add(sp_绿色魔法_lv1);
            sp_绿色魔法_cta.Add(sp_绿色魔法_lv1_a);

            sp_绿色魔法.add_time(150);
            List<Tuple<string, double2>> sp_绿色魔法_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 900e3));
            List<Tuple<string, double2>> sp_绿色魔法_lv2_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("绿色粒子", 15)),
                new Tuple<string, double2>("魔力", 2000));
            sp_绿色魔法.description.Add("下一级主动效果变化：\n战斗经验获取 ×2，法术学习速度 ×2.5\n" +
                                        "下一级被动效果变化：\n转生点数获取 ×1.2");
            sp_绿色魔法.add_active("战斗经验获取 ×4，法术学习速度 ×7.5");
            sp_绿色魔法.add_passive("转生点数获取 ×1.44");
            sp_绿色魔法_ct.Add(sp_绿色魔法_lv2);
            sp_绿色魔法_cta.Add(sp_绿色魔法_lv2_a);

            sp_绿色魔法.add_time(500);
            List<Tuple<string, double2>> sp_绿色魔法_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 40e6));
            List<Tuple<string, double2>> sp_绿色魔法_lv3_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("绿色粒子", 600)),
                new Tuple<string, double2>("魔力", 40e3));
            sp_绿色魔法.description.Add("下一级主动效果变化：\n战斗经验获取 ×2.25，法术学习速度 ×2.67\n" +
                                        "下一级被动效果变化：\n转生点数获取 ×1.18");
            sp_绿色魔法.add_active("战斗经验获取 ×9，法术学习速度 ×20");
            sp_绿色魔法.add_passive("转生点数获取 ×1.7");
            sp_绿色魔法_ct.Add(sp_绿色魔法_lv3);
            sp_绿色魔法_cta.Add(sp_绿色魔法_lv3_a);

            sp_绿色魔法.add_time(1500);
            List<Tuple<string, double2>> sp_绿色魔法_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 5e9));
            List<Tuple<string, double2>> sp_绿色魔法_lv4_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("绿色粒子", 150e3)),
                new Tuple<string, double2>("魔力", 10e6));
            sp_绿色魔法.description.Add("下一级主动效果变化：\n战斗经验获取 ×3.33，法术学习速度 ×4\n" +
                                        "下一级被动效果变化：\n转生点数获取 ×1.18");
            sp_绿色魔法.add_active("战斗经验获取 ×30，法术学习速度 ×80");
            sp_绿色魔法.add_passive("转生点数获取 ×2");
            sp_绿色魔法_ct.Add(sp_绿色魔法_lv4);
            sp_绿色魔法_cta.Add(sp_绿色魔法_lv4_a);

            sp_绿色魔法.add_time(4500);
            List<Tuple<string, double2>> sp_绿色魔法_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 225e9)),
                new Tuple<string, double2>("植物原料", 1.5e15));
            List<Tuple<string, double2>> sp_绿色魔法_lv5_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("绿色粒子", 1.5e6)),
                new Tuple<string, double2>("魔力", 200e6)),
                new Tuple<string, double2>("能量", 20));
            sp_绿色魔法.description.Add("下一级主动效果变化：\n战斗经验获取 ×2.5，法术学习速度 ×5，\n采矿经验获取×1.5\n" +
                                        "下一级被动效果变化：\n转生点数获取 ×1.25");
            sp_绿色魔法.add_active("战斗经验获取 ×75，法术学习速度 ×400，\n采矿经验获取×1.5");
            sp_绿色魔法.add_passive("转生点数获取 ×2.5");
            sp_绿色魔法_ct.Add(sp_绿色魔法_lv5);
            sp_绿色魔法_cta.Add(sp_绿色魔法_lv5_a);

            sp_绿色魔法.cost_table_active = sp_绿色魔法_cta;
            sp_绿色魔法.description.Add("已达到最大等级！");
            sp_绿色魔法.set_init_cost(sp_绿色魔法_ct, 0, sp_绿色魔法_ct.Count);
            upgrades.Add("绿色魔法", sp_绿色魔法);
            #endregion


            // 红色魔法
            #region
            spell sp_红色魔法 = new spell("红色魔法", "魔法")
            {
                can_reset = true,
                normal = true,
                unlocked = false
            };
            m.魔法_法术_红色魔法_grid.Visibility = (Visibility)1;

            sp_红色魔法.add_passive("无效果");
            sp_红色魔法.add_active("不可用");

            List<List<Tuple<string, double2>>> sp_红色魔法_ct = new List<List<Tuple<string, double2>>>();
            List<List<Tuple<string, double2>>> sp_红色魔法_cta = new List<List<Tuple<string, double2>>>();
            sp_红色魔法_cta.Add(null);

            sp_红色魔法.add_time(180);
            List<Tuple<string, double2>> sp_红色魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 3e6));
            List<Tuple<string, double2>> sp_红色魔法_lv1_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("红色粒子", 30)),
                new Tuple<string, double2>("魔力", 15e3));
            sp_红色魔法.description.Add("下一级主动效果变化：\n攻击 ×2，减速值下降速度 ×1.25\n" +
                                        "下一级被动效果变化：\n方块生产器的时间 /2");
            sp_红色魔法.add_active("攻击 ×2，减速值下降速度 ×1.25");
            sp_红色魔法.add_passive("方块生产器的时间 /2");
            sp_红色魔法_ct.Add(sp_红色魔法_lv1);
            sp_红色魔法_cta.Add(sp_红色魔法_lv1_a);

            sp_红色魔法.add_time(1200);
            List<Tuple<string, double2>> sp_红色魔法_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 360e6));
            List<Tuple<string, double2>> sp_红色魔法_lv2_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("红色粒子", 500)),
                new Tuple<string, double2>("魔力", 250e3));
            sp_红色魔法.description.Add("下一级主动效果变化：\n攻击 ×2，减速值下降速度 ×1.2\n" +
                                        "下一级被动效果变化：\n方块生产器的时间 /2");
            sp_红色魔法.add_active("攻击 ×4，减速值下降速度 ×1.5");
            sp_红色魔法.add_passive("方块生产器的时间 /4");
            sp_红色魔法_ct.Add(sp_红色魔法_lv2);
            sp_红色魔法_cta.Add(sp_红色魔法_lv2_a);

            sp_红色魔法.add_time(3500);
            List<Tuple<string, double2>> sp_红色魔法_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 45e9));
            List<Tuple<string, double2>> sp_红色魔法_lv3_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("红色粒子", 6000)),
                new Tuple<string, double2>("魔力", 3e6));
            sp_红色魔法.description.Add("下一级主动效果变化：\n攻击 ×2，减速值下降速度 ×1.4\n" +
                                        "下一级被动效果变化：\n方块生产器的时间 /2.5");
            sp_红色魔法.add_active("攻击 ×8，减速值下降速度 ×2.1");
            sp_红色魔法.add_passive("方块生产器的时间 /10");
            sp_红色魔法_ct.Add(sp_红色魔法_lv3);
            sp_红色魔法_cta.Add(sp_红色魔法_lv3_a);

            sp_红色魔法.add_time(12500);
            List<Tuple<string, double2>> sp_红色魔法_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 800e9));
            List<Tuple<string, double2>> sp_红色魔法_lv4_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("红色粒子", 800000)),
                new Tuple<string, double2>("魔力", 650e6)),
                new Tuple<string, double2>("能量", 30));
            sp_红色魔法.description.Add("下一级主动效果变化：\n攻击 ×4，减速值下降速度 ×1.286，\n燃料速度值 ×1.5\n" +
                                        "下一级被动效果变化：\n方块生产器的时间 /2.5");
            sp_红色魔法.add_active("攻击 ×32，减速值下降速度 ×2.7，\n燃料速度值 ×1.5");
            sp_红色魔法.add_passive("方块生产器的时间 /25");
            sp_红色魔法_ct.Add(sp_红色魔法_lv4);
            sp_红色魔法_cta.Add(sp_红色魔法_lv4_a);

            sp_红色魔法.cost_table_active = sp_红色魔法_cta;
            sp_红色魔法.description.Add("已达到最大等级！");
            sp_红色魔法.set_init_cost(sp_红色魔法_ct, 0, sp_红色魔法_ct.Count);
            upgrades.Add("红色魔法", sp_红色魔法);
            #endregion

            // 橙色魔法
            #region
            spell sp_橙色魔法 = new spell("橙色魔法", "魔法")
            {
                can_reset = true,
                normal = true,
                unlocked = false
            };
            m.魔法_法术_橙色魔法_grid.Visibility = (Visibility)1;

            sp_橙色魔法.add_passive("无效果");
            sp_橙色魔法.add_active("不可用");

            List<List<Tuple<string, double2>>> sp_橙色魔法_ct = new List<List<Tuple<string, double2>>>();
            List<List<Tuple<string, double2>>> sp_橙色魔法_cta = new List<List<Tuple<string, double2>>>();
            sp_橙色魔法_cta.Add(null);

            sp_橙色魔法.add_time(2000);
            List<Tuple<string, double2>> sp_橙色魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 150e6));
            List<Tuple<string, double2>> sp_橙色魔法_lv1_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("橙色粒子", 1.2e3)),
                new Tuple<string, double2>("魔力", 2e6));
            sp_橙色魔法.description.Add("下一级主动效果变化：\n采矿点数获取×1.5，方块生产器产量×3\n" +
                                        "下一级被动效果变化：\n格子边长 ×1.2");
            sp_橙色魔法.add_active("采矿点数获取×1.5，方块生产器产量×3");
            sp_橙色魔法.add_passive("格子边长 ×1.2");
            sp_橙色魔法_ct.Add(sp_橙色魔法_lv1);
            sp_橙色魔法_cta.Add(sp_橙色魔法_lv1_a);

            sp_橙色魔法.add_time(5000);
            List<Tuple<string, double2>> sp_橙色魔法_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 8e9));
            List<Tuple<string, double2>> sp_橙色魔法_lv2_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("橙色粒子", 50e3)),
                new Tuple<string, double2>("魔力", 100e6));
            sp_橙色魔法.description.Add("下一级主动效果变化：\n采矿点数获取×1.3，方块生产器产量×2.67\n" +
                                        "下一级被动效果变化：\n格子边长 ×1.2");
            sp_橙色魔法.add_active("采矿点数获取×1.95，方块生产器产量×8");
            sp_橙色魔法.add_passive("格子边长 ×1.44");
            sp_橙色魔法_ct.Add(sp_橙色魔法_lv2);
            sp_橙色魔法_cta.Add(sp_橙色魔法_lv2_a);

            sp_橙色魔法.add_time(20000);
            List<Tuple<string, double2>> sp_橙色魔法_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 3e12)),
                new Tuple<string, double2>("铜", 3e9));
            List<Tuple<string, double2>> sp_橙色魔法_lv3_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("橙色粒子", 50e3)),
                new Tuple<string, double2>("魔力", 100e6)),
                new Tuple<string, double2>("能量", 40));
            sp_橙色魔法.description.Add("下一级主动效果变化：\n采矿点数获取×1.282，方块生产器产量\n×2.5，攻击力的 10% 穿透防御\n" +
                                        "下一级被动效果变化：\n格子边长 ×1.25");
            sp_橙色魔法.add_active("采矿点数获取×2.5，方块生产器产量×20，\n攻击力的 10% 穿透防御");
            sp_橙色魔法.add_passive("格子边长 ×1.8");
            sp_橙色魔法_ct.Add(sp_橙色魔法_lv3);
            sp_橙色魔法_cta.Add(sp_橙色魔法_lv3_a);

            sp_橙色魔法.cost_table_active = sp_橙色魔法_cta;
            sp_橙色魔法.description.Add("已达到最大等级！");
            sp_橙色魔法.set_init_cost(sp_橙色魔法_ct, 0, sp_橙色魔法_ct.Count);
            upgrades.Add("橙色魔法", sp_橙色魔法);
            #endregion

            // 蓝色魔法
            #region
            spell sp_蓝色魔法 = new spell("蓝色魔法", "魔法")
            {
                can_reset = true,
                normal = true,
                unlocked = false
            };
            m.魔法_法术_蓝色魔法_grid.Visibility = (Visibility)1;

            sp_蓝色魔法.add_passive("无效果");
            sp_蓝色魔法.add_active("不可用");

            List<List<Tuple<string, double2>>> sp_蓝色魔法_ct = new List<List<Tuple<string, double2>>>();
            List<List<Tuple<string, double2>>> sp_蓝色魔法_cta = new List<List<Tuple<string, double2>>>();
            sp_蓝色魔法_cta.Add(null);

            sp_蓝色魔法.add_time(4500);
            List<Tuple<string, double2>> sp_蓝色魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 750e6));
            List<Tuple<string, double2>> sp_蓝色魔法_lv1_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("蓝色粒子", 2e3)),
                new Tuple<string, double2>("魔力", 10e6));
            sp_蓝色魔法.description.Add("下一级主动效果变化：\n药水效果 ×1.15，魔力获取 ×1.3\n" +
                                        "下一级被动效果变化：\n所有升级的价格×0.9");
            sp_蓝色魔法.add_active("药水效果 ×1.15，魔力获取 ×1.3");
            sp_蓝色魔法.add_passive("所有升级的价格×0.9");
            sp_蓝色魔法_ct.Add(sp_蓝色魔法_lv1);
            sp_蓝色魔法_cta.Add(sp_蓝色魔法_lv1_a);

            sp_蓝色魔法.add_time(16000);
            List<Tuple<string, double2>> sp_蓝色魔法_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 50e9)),
                new Tuple<string, double2>("魔法糖浆", 5e6));
            List<Tuple<string, double2>> sp_蓝色魔法_lv2_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("蓝色粒子", 100e3)),
                new Tuple<string, double2>("魔力", 720e6)),
                new Tuple<string, double2>("能量", 9));
            sp_蓝色魔法.description.Add("下一级主动效果变化：\n药水效果 ×1.130，魔力获取 ×1.308\n" +
                                        "下一级被动效果变化：\n所有升级的价格×0.889");
            sp_蓝色魔法.add_active("药水效果 ×1.3，魔力获取 ×1.7");
            sp_蓝色魔法.add_passive("所有升级的价格×0.8");
            sp_蓝色魔法_ct.Add(sp_蓝色魔法_lv2);
            sp_蓝色魔法_cta.Add(sp_蓝色魔法_lv2_a);

            sp_蓝色魔法.add_time(40000);
            List<Tuple<string, double2>> sp_蓝色魔法_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 10e12)),
                new Tuple<string, double2>("魔法糖浆", 1e9));
            List<Tuple<string, double2>> sp_蓝色魔法_lv3_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("蓝色粒子", 1.6e6)),
                new Tuple<string, double2>("魔力", 12e9)),
                new Tuple<string, double2>("能量", 50));
            sp_蓝色魔法.description.Add("下一级主动效果变化：\n药水效果 ×1.154，魔力获取 ×1.324，\n施法消耗 / 1.5\n" +
                                        "下一级被动效果变化：\n所有升级的价格×0.875");
            sp_蓝色魔法.add_active("药水效果 ×1.5，魔力获取 ×2.25，\n施法消耗 / 1.5");
            sp_蓝色魔法.add_passive("所有升级的价格×0.7");
            sp_蓝色魔法_ct.Add(sp_蓝色魔法_lv3);
            sp_蓝色魔法_cta.Add(sp_蓝色魔法_lv3_a);

            sp_蓝色魔法.cost_table_active = sp_蓝色魔法_cta;
            sp_蓝色魔法.description.Add("已达到最大等级！");
            sp_蓝色魔法.set_init_cost(sp_蓝色魔法_ct, 0, sp_蓝色魔法_ct.Count);
            upgrades.Add("蓝色魔法", sp_蓝色魔法);
            #endregion

            // 无色魔法
            #region
            spell sp_无色魔法 = new spell("无色魔法", "魔法")
            {
                can_reset = true,
                normal = true,
                unlocked = false
            };
            m.魔法_法术_无色魔法_grid.Visibility = (Visibility)1;

            sp_无色魔法.add_passive("无效果");
            sp_无色魔法.add_active("不可用");

            List<List<Tuple<string, double2>>> sp_无色魔法_ct = new List<List<Tuple<string, double2>>>();
            List<List<Tuple<string, double2>>> sp_无色魔法_cta = new List<List<Tuple<string, double2>>>();
            sp_无色魔法_cta.Add(null);

            sp_无色魔法.add_time(7000);
            List<Tuple<string, double2>> sp_无色魔法_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 3e9));
            List<Tuple<string, double2>> sp_无色魔法_lv1_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("无色粒子", 5e3)),
                new Tuple<string, double2>("魔力", 50e6));
            sp_无色魔法.description.Add("下一级主动效果变化：每秒得到\n“即将获得的转生点数”的 ^ 0.6，攻击时间 / 1.5，\n采矿点数获取时间 / 1.5。\n" +
                                        "下一级被动效果变化：每秒获得 20 能量");
            sp_无色魔法.add_active("每秒得到“即将获得的转生点数”的 ^ 0.6，\n攻击时间 / 1.5，采矿点数获取时间 / 1.5。");
            sp_无色魔法.add_passive("每秒获得 20 能量");
            sp_无色魔法_ct.Add(sp_无色魔法_lv1);
            sp_无色魔法_cta.Add(sp_无色魔法_lv1_a);

            sp_无色魔法.add_time(22500);
            List<Tuple<string, double2>> sp_无色魔法_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 900e9)),
                new Tuple<string, double2>("转生点数", 900e9));
            List<Tuple<string, double2>> sp_无色魔法_lv2_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("无色粒子", 500)),
                new Tuple<string, double2>("魔力", 1.8e9)),
                new Tuple<string, double2>("魔力", 250e3));
            sp_无色魔法.description.Add("下一级主动效果变化：每秒得到\n“即将获得的转生点数”的 ^ 0.63，\n攻击时间 / 1.5，采矿点数获取时间 / 1.333。\n" +
                                        "下一级被动效果变化：每秒获得 50 能量");
            sp_无色魔法.add_active("每秒得到“即将获得的转生点数”的 ^ 0.63，\n攻击时间 / 2.25，采矿点数获取时间 / 2");
            sp_无色魔法.add_passive("每秒获得 50 能量");
            sp_无色魔法_ct.Add(sp_无色魔法_lv2);
            sp_无色魔法_cta.Add(sp_无色魔法_lv2_a);

            sp_无色魔法.add_time(80000);
            List<Tuple<string, double2>> sp_无色魔法_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("魔力", 750e12)),
                new Tuple<string, double2>("转生点数", 750e12)),
                new Tuple<string, double2>("钻石", 1000));
            List<Tuple<string, double2>> sp_无色魔法_lv3_a =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("红色粒子", 500)),
                new Tuple<string, double2>("魔力", 250e3));
            sp_无色魔法.description.Add("下一级主动效果变化：每秒得到\n“即将获得的转生点数”的 ^ 0.66，\n攻击时间 / 1.556，采矿点数获取时间 / 1.4。\n" +
                                        "下一级被动效果变化：每秒获得 100 能量");
            sp_无色魔法.add_active("每秒得到“即将获得的转生点数”的 ^ 0.66，\n攻击时间 / 3.5，采矿点数获取时间 / 2.8");
            sp_无色魔法.add_passive("每秒获得 100 能量");
            sp_无色魔法_ct.Add(sp_无色魔法_lv3);
            sp_无色魔法_cta.Add(sp_无色魔法_lv3_a);

            sp_无色魔法.cost_table_active = sp_无色魔法_cta;
            sp_无色魔法.description.Add("已达到最大等级！");
            sp_无色魔法.set_init_cost(sp_无色魔法_ct, 0, sp_无色魔法_ct.Count);
            upgrades.Add("无色魔法", sp_无色魔法);
            #endregion

            //战斗经验药水
            #region
            enchant ec_战斗经验药水 = new enchant("战斗经验药水", lgb)
            {
                can_reset = true,
                is_potion = true,
                unlocked = true
            };
            m.魔法_次_药水_战斗经验药水_grid.Visibility = 0;
            List<Tuple<string, double2, double2>> ec_战斗经验药水_cost = new List<Tuple<string, double2, double2>>();
            ec_战斗经验药水_cost.Add(new Tuple<string, double2, double2>("植物原料", 400, 12));
            ec_战斗经验药水.set_init_cost(ec_战斗经验药水_cost, 1, 100, 10, 1.4);
            enchants.Add("战斗经验药水", ec_战斗经验药水);
            #endregion

            //攻击药水
            #region
            enchant ec_攻击药水 = new enchant("攻击药水", lgb)
            {
                can_reset = true,
                is_potion = true,
                unlocked = true
            };
            m.魔法_次_药水_攻击药水_grid.Visibility = 0;
            List<Tuple<string, double2, double2>> ec_攻击药水_cost = new List<Tuple<string, double2, double2>>();
            ec_攻击药水_cost.Add(new Tuple<string, double2, double2>("动物原料", 100, 4));
            ec_攻击药水.set_init_cost(ec_攻击药水_cost, 1, 100, 5, 1);
            //(5 + 0.2n) * (1 + 0.005n) 
            ec_攻击药水.set_changing_time(0.2, 0.005);
            enchants.Add("攻击药水", ec_攻击药水);
            #endregion

            //魔力药水
            #region
            enchant ec_魔力药水 = new enchant("魔力药水", lgb)
            {
                can_reset = true,
                is_potion = true,
                unlocked = true
            };
            m.魔法_次_药水_魔力药水_grid.Visibility = 0;
            List<Tuple<string, double2, double2>> ec_魔力药水_cost = new List<Tuple<string, double2, double2>>();
            ec_魔力药水_cost.Add(new Tuple<string, double2, double2>("魔法粉末", 1500, 10));
            ec_魔力药水.set_init_cost(ec_魔力药水_cost, 1, 100, 20, 1.8);
            enchants.Add("魔力药水", ec_魔力药水);
            #endregion

            //烈焰药水
            #region
            enchant ec_烈焰药水 = new enchant("烈焰药水", lgb)
            {
                can_reset = true,
                is_potion = true,
                unlocked = false
            };
            m.魔法_次_药水_烈焰药水_grid.Visibility = (Visibility)1;
            List<Tuple<string, double2, double2>> ec_烈焰药水_cost = new List<Tuple<string, double2, double2>>();
            ec_烈焰药水_cost.Add(new Tuple<string, double2, double2>("烈焰粉末", 4, 5));
            ec_烈焰药水.set_init_cost(ec_烈焰药水_cost, 1, 100, 20, 1);
            ec_烈焰药水.set_changing_time(1, 0.01);
            enchants.Add("烈焰药水", ec_烈焰药水);
            #endregion

            //幸运药水
            #region
            enchant ec_幸运药水 = new enchant("幸运药水", lgb)
            {
                can_reset = true,
                is_potion = true,
                unlocked = false
            };
            m.魔法_次_药水_幸运药水_grid.Visibility = (Visibility)1;
            List<Tuple<string, double2, double2>> ec_幸运药水_cost = new List<Tuple<string, double2, double2>>();
            ec_幸运药水_cost.Add(new Tuple<string, double2, double2>("魔法糖浆", 100, 3));
            ec_幸运药水_cost.Add(new Tuple<string, double2, double2>("铁", 150, 8.25));
            ec_幸运药水.set_init_cost(ec_幸运药水_cost, 1, 100, 40, 1);
            ec_幸运药水.set_changing_time(2, 0.015);
            enchants.Add("幸运药水", ec_幸运药水);
            #endregion

            #endregion

            //no.5 采矿：
            #region
            Dictionary<string, resource> res_group_采矿 = new Dictionary<string, resource>();
            res_table.Add("采矿", res_group_采矿);

            采矿_options = make_group(m.采矿_option_grid);

            resource res_采矿点数 = new resource(1, 0, "采矿点数",
                getSCB(Color.FromRgb(255, 255, 0)))
            {
                unlocked = false
            };
            res_group_采矿.Add("采矿点数", res_采矿点数);

            resource res_煤 = new resource(2, 0, "煤",
                 getSCB(Color.FromRgb(150, 150, 150)))
            {
                unlocked = false,
                luck_req = 2
            };
            res_group_采矿.Add("煤", res_煤);


            resource res_铜矿 = new resource(3, 0, "铜矿",
                 getSCB(Color.FromRgb(255, 255, 127)))
            {
                unlocked = false,
                luck_req = 10
            };
            res_group_采矿.Add("铜矿", res_铜矿);

            resource res_铁矿 = new resource(4, 0, "铁矿",
                 getSCB(Color.FromRgb(200, 66, 40)))
            {
                unlocked = false,
                luck_req = 25
            };
            res_group_采矿.Add("铁矿", res_铁矿);

            resource res_魔石 = new resource(5, 0, "魔石",
                 getSCB(Color.FromRgb(55, 255, 255)))
            {
                unlocked = false,
                luck_req = 75
            };
            res_group_采矿.Add("魔石", res_魔石);

            resource res_石油 = new resource(6, 0, "石油",
                 getSCB(Color.FromRgb(150, 150, 150)))
            {
                unlocked = false,
                luck_req = 200
            };
            res_group_采矿.Add("石油", res_石油);

            resource res_银 = new resource(7, 0, "银",
                 getSCB(Color.FromRgb(225, 225, 225)))
            {
                unlocked = false,
                luck_req = 4000
            };
            res_group_采矿.Add("银", res_银);

            resource res_化石 = new resource(8, 0, "化石",
                 getSCB(Color.FromRgb(135, 135, 135)))
            {
                unlocked = false,
                luck_req = 10000
            };
            res_group_采矿.Add("化石", res_化石);

            resource res_金 = new resource(9, 0, "金",
                 getSCB(Color.FromRgb(255, 255, 0)))
            {
                unlocked = false,
                luck_req = 50000
            };
            res_group_采矿.Add("金", res_金);

            string[] ores = { "煤", "铜矿", "铁矿", "魔石", "石油", "水晶方块", "银", "化石", "金", "钻石" };
            矿物 = new List<string>(ores);

            //5 魔石    6 石油  水晶（方块）  7 银   8 化石   9 金   钻石（制造）
            //   ↓
            //魔力粉末    燃料           金属    

            //11 烤植物   12 烤动物    13 铜    14 铁   15 钢   16 
            //

            resource res_烤植物 = new resource(11, 0, "烤植物",
                 getSCB(Color.FromRgb(0, 135, 0)))
            {
                unlocked = false
            };
            res_group_采矿.Add("烤植物", res_烤植物);

            resource res_烤动物 = new resource(12, 0, "烤动物",
                 getSCB(Color.FromRgb(180, 127, 0)))
            {
                unlocked = false
            };
            res_group_采矿.Add("烤动物", res_烤动物);

            resource res_铜 = new resource(13, 0, "铜",
                 getSCB(Color.FromRgb(186, 110, 64)))
            {
                unlocked = false
            };
            res_group_采矿.Add("铜", res_铜);

            resource res_铁 = new resource(14, 0, "铁",
                 getSCB(Color.FromRgb(168, 169, 170)))
            {
                unlocked = false
            };
            res_group_采矿.Add("铁", res_铁);

            resource res_钢 = new resource(15, 0, "钢",
                 getSCB(Color.FromRgb(200, 200, 200)))
            {
                unlocked = false
            };
            res_group_采矿.Add("钢", res_钢);

            //根据 未花费的转生点数比例 获得奖励
            //a%未花费   Max(1, 3 ^ (a / 100) * n ^ 0.1) 降低挖掘所需的采矿点数（经验随之下降，但降幅低于采矿点数需求的降幅）
            treasures.Add("荧光宝石", new treasure("荧光宝石", getSCB(Color.FromRgb(200, 200, 255)), 50, 1e-4));

            //根据 熔炉等级 获得奖励
            //f熔炉等级  1 + [(0.05 * k) ^ 3 * (n ^ 0.2)] 倍增燃料速度值
            treasures.Add("熔岩球", new treasure("熔岩球", getSCB(Color.FromRgb(255, 160, 50)), 100, 5e-5));

            //根据 现有魔力 获得奖励
            //m魔力      (n + 1) ^ [0.01 * log10(m + 1)]  倍增所有方块获取（不包括采矿中的挖掘）
            //           (n ^ 0.3 + 1) ^ [0.01 * log10(m + 1)]  倍增格子边长
            treasures.Add("魔方", new treasure("魔方", getSCB(Color.FromRgb(150, 200, 150)), 200, 3e-5));

            //根据 目前选中的敌人等级 获得奖励
            //k等级      [1 + 0.002 * log10(n + 1)] ^ k   倍增所有能量获取
            treasures.Add("脉冲符文", new treasure("脉冲符文", getSCB(Color.FromRgb(225, 225, 25)), 400, 1.5e-5));

            //根据 未花费的采矿点数比例 获得奖励
            //a%未花费   (((n + 1) ^ 0.04) ^ (a / 100 + 1))   倍增所有宝物获取
            treasures.Add("宝箱", new treasure("宝箱", getSCB(Color.FromRgb(255, 180, 255)), 1200, 2e-6));

            //根据 历史生成的采矿区域数 获得奖励
            //h区域数    1 + [(n ^ 0.1) * (h ^ 0.5)] 降低白色方块生产器时间
            //           1 + [Min(n, 1) * (h ^ 0.3)] 降低白色方块生产器产量
            treasures.Add("光速徽章", new treasure("光速徽章", getSCB(Color.FromRgb(255, 240, 225)), 4000, 1e-7));



            #region
            upgrade up_熔炉升级 = new upgrade("熔炉升级", "采矿")
            {
                can_reset = true,
                unlocked = true
            };
            List<List<Tuple<string, double2>>> up_熔炉升级_ct = new List<List<Tuple<string, double2>>>();
            List<Tuple<string, double2>> up_熔炉升级_lv1 =
                upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 10e9));
            up_熔炉升级.description.Add("可以使用煤作为燃料");
            up_熔炉升级_ct.Add(up_熔炉升级_lv1);

            List<Tuple<string, double2>> up_熔炉升级_lv2 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 500e9)),
                new Tuple<string, double2>("铜", 1));
            up_熔炉升级.description.Add("燃料速度值 ×2");
            up_熔炉升级_ct.Add(up_熔炉升级_lv2);

            List<Tuple<string, double2>> up_熔炉升级_lv3 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 2e12)),
                new Tuple<string, double2>("铜", 15000));
            up_熔炉升级.description.Add("可以使用烈焰粉末作为燃料，火力将不会下降到" + number_format(10000) + "以下");
            up_熔炉升级_ct.Add(up_熔炉升级_lv3);

            List<Tuple<string, double2>> up_熔炉升级_lv4 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 11.67e12)),
                new Tuple<string, double2>("铁", 20000));
            up_熔炉升级.description.Add("燃料速度值 ×2");
            up_熔炉升级_ct.Add(up_熔炉升级_lv4);

            List<Tuple<string, double2>> up_熔炉升级_lv5 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 500e3)),
                new Tuple<string, double2>("铁", 150e3));
            up_熔炉升级.description.Add("燃料速度值 ×2.5");
            up_熔炉升级_ct.Add(up_熔炉升级_lv5);

            List<Tuple<string, double2>> up_熔炉升级_lv6 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 4e6)),
                new Tuple<string, double2>("铁", 1e6));
            up_熔炉升级.description.Add("原料速度值 ×4");
            up_熔炉升级_ct.Add(up_熔炉升级_lv6);

            List<Tuple<string, double2>> up_熔炉升级_lv7 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 6e15)),
                new Tuple<string, double2>("铁", 8e6));
            up_熔炉升级.description.Add("燃料速度值 ×2.5");
            up_熔炉升级_ct.Add(up_熔炉升级_lv7);

            List<Tuple<string, double2>> up_熔炉升级_lv8 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 150e6)),
                new Tuple<string, double2>("钢", 50e3));
            up_熔炉升级.description.Add("火力流失 / 2，可以使用石油作为燃料");
            up_熔炉升级_ct.Add(up_熔炉升级_lv8);

            List<Tuple<string, double2>> up_熔炉升级_lv9 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 880e15)),
                new Tuple<string, double2>("钢", 1.2e6));
            up_熔炉升级.description.Add("燃料速度值 ×2");
            up_熔炉升级_ct.Add(up_熔炉升级_lv9);

            List<Tuple<string, double2>> up_熔炉升级_lv10 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 150e9)),
                new Tuple<string, double2>("钢", 36e6));
            up_熔炉升级.description.Add("燃料速度值 ×2，原料速度值 ×2");
            up_熔炉升级_ct.Add(up_熔炉升级_lv10);

            List<Tuple<string, double2>> up_熔炉升级_lv11 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铁", 700e9)),
                new Tuple<string, double2>("魔石", 30e9));
            up_熔炉升级.description.Add("火力流失 / 5，火力将不会下降到" + number_format(100e9) + "以下");
            up_熔炉升级_ct.Add(up_熔炉升级_lv11);

            List<Tuple<string, double2>> up_熔炉升级_lv12 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("石头方块", 100e21)),
                new Tuple<string, double2>("熔岩球", 25e3));
            up_熔炉升级.description.Add("燃料速度值 ×3");
            up_熔炉升级_ct.Add(up_熔炉升级_lv12);

            List<Tuple<string, double2>> up_熔炉升级_lv13 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("铜", 10e15)),
                new Tuple<string, double2>("魔石", 120e12));
            up_熔炉升级.description.Add("燃料速度值 ×1.5，原料速度值 ×2.5");
            up_熔炉升级_ct.Add(up_熔炉升级_lv13);

            List<Tuple<string, double2>> up_熔炉升级_lv14 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("钢", 500e12)),
                new Tuple<string, double2>("水晶方块", 1000));
            up_熔炉升级.description.Add("燃料速度值 ×2.25");
            up_熔炉升级_ct.Add(up_熔炉升级_lv14);

            List<Tuple<string, double2>> up_熔炉升级_lv15 =
                upgrade_cost_adder(upgrade_cost_adder(upgrade_cost_adder(null, null),
                new Tuple<string, double2>("钢", 300e15)),
                new Tuple<string, double2>("水晶方块", 1e6));
            up_熔炉升级.description.Add("燃料速度值 ×3");
            up_熔炉升级_ct.Add(up_熔炉升级_lv15);

            up_熔炉升级.description.Add("已达到最大等级！");
            up_熔炉升级.set_init_cost(up_熔炉升级_ct, 0, up_熔炉升级_ct.Count);
            upgrades.Add("熔炉升级", up_熔炉升级);
            #endregion

            minep.init();
            mine_generate();
            minef.init(this, 1000, 10, 5, 1, 1.8, 1);
            minef.generate();

            get_field();

            furance.init();
            heater_generate();

            x_recipes.Add("植物原料", new heater_x_recipe("植物原料", "烤植物", null, 1, 0, 20000, 1));    //at f 100K: 100K 烤植物/s
            x_recipes.Add("动物原料", new heater_x_recipe("动物原料", "烤动物", null, 1, 0, 60000, 100));  //at f 100K: 1K 烤动物/s
            x_recipes.Add("铜矿", new heater_x_recipe("铜矿", "铜", null, 1, 0, 300e3, 3000));             //at f 300K: 100 铜/s     
            x_recipes.Add("铁矿", new heater_x_recipe("铁矿", "铁", null, 1, 0, 1000e3, 10000));           //at f 1M: 100 铁/s     
            x_recipes.Add("铁", new heater_x_recipe("铁", "钢", null, 0.001, 0, 200e6, 20e3));             //at f 200M: 10000 * 0.001 = 10 钢/s   

            //增加等于损耗
            //f^0.5 * gain = loss * f
            //loss * fmfm - gain * fm = 0
            //fm = gain / loss  f = (gain / loss) ^ 2    fdrop = loss * f = gain ^ 2 / loss    消耗 = fdrop / fproduct = gain ^ 2 / loss / fproduct
            y_recipes.Add("木头方块", new heater_y_recipe("木头方块", 1, 100, 0, 0.025));            //f = (40 / loss) ^ 2 = 160K       消耗 = 160K / 0.1 / 1 = 1.6M
            y_recipes.Add("煤", new heater_y_recipe("煤", 3000, 8000, 1, 50));                       //f = (60 / loss) ^ 2 = 360K       消耗 = 360K / 0.1 / 3K = 1200
            y_recipes.Add("烈焰粉末", new heater_y_recipe("烈焰粉末", 4000, 200e3, 3, 50));          //f = (80 / loss) ^ 2 = 640K       消耗 = 640K / 0.1 / 4K = 1600
            y_recipes.Add("石油", new heater_y_recipe("石油", 1e6, 5e9, 8, 10e3));                //f = (100 / loss) ^ 2 = 1M        消耗 = 1M / 0.1 / 1M = 10

            #endregion

            //no.6 核心：
            #region
            Dictionary<string, resource> res_group_核心 = new Dictionary<string, resource>();
            res_table.Add("核心", res_group_核心);
            #endregion

            //no.7 娱乐：
            #region
            Dictionary<string, resource> res_group_娱乐 = new Dictionary<string, resource>();
            res_table.Add("娱乐", res_group_娱乐);
            m.娱乐_全屏_text.Visibility = Visibility.Hidden;

            resource coin = new resource(1, 0, "娱乐币",
                getSCB(Color.FromRgb(0, 255, 255)))
            {
                unlocked = true
            };
            res_group_娱乐.Add("娱乐币", coin);

            vm_init();
            vm_elem_init();
            vm.close();
            #endregion

            //no.8 混沌：
            #region
            Dictionary<string, resource> res_group_混沌 = new Dictionary<string, resource>();
            res_table.Add("混沌", res_group_混沌);
            #endregion


            //no.9 转生：
            #region
            Dictionary<string, resource> res_group_转生 = new Dictionary<string, resource>();
            res_table.Add("转生", res_group_转生);

            resource prestige_point = new resource(1, 0, "转生点数",
                getSCB(Color.FromRgb(255, 255, 63)))
            {
                unlocked = true
            };
            res_group_转生.Add("转生点数", prestige_point);

            resource 药水值 = new resource(2, 0, "药水值",
                getSCB(Color.FromRgb(150, 200, 175)))
            {
                unlocked = false
            };
            res_group_转生.Add("药水值", 药水值);




            //转生：升级
            //0-0 对数增益
            #region
            prestige_upgrade pu_对数增益 = new prestige_upgrade("对数增益")
            {
                can_reset = true
            };
            double2[] data_对数增益 = new double2[] { 5, 12000, 400e3, 240e6 };
            List<double2> pu_对数增益_cost_table = new List<double2>(data_对数增益);
            pu_对数增益.set_init_cost("转生点数", pu_对数增益_cost_table, 0, pu_对数增益_cost_table.Count);
            pu_对数增益.unlocked = true;
            m.转生_升级_对数增益_grid.Visibility = 0;
            prestige_ups.Add("对数增益", pu_对数增益);
            #endregion

            //1-0 生成器
            #region
            prestige_upgrade pu_生成器 = new prestige_upgrade("生成器")
            {
                can_reset = true
            };
            double2[] data_生成器 = new double2[] { 400, 4000, 65000, 600e3, 54e6, 16e9, 200e9 };
            List<double2> pu_生成器_cost_table = new List<double2>(data_生成器);
            pu_生成器.set_init_cost("转生点数", pu_生成器_cost_table, 0, pu_生成器_cost_table.Count);
            pu_生成器.unlocked = false;
            m.转生_升级_生成器_grid.Visibility = 0;
            prestige_ups.Add("生成器", pu_生成器);
            #endregion

            //对数增益——生成器
            #region
            link lk_对数增益_生成器 = new link(m.转生_main_grid, m.转生_升级_对数增益_grid, m.转生_升级_生成器_grid, 1, true);
            lk_对数增益_生成器.prestige_type(pu_对数增益, pu_生成器);
            lk_对数增益_生成器.update_progress(0);
            links.Add("对数增益_生成器", lk_对数增益_生成器);
            #endregion

            //2-0 资源保留
            #region
            prestige_upgrade pu_资源保留 = new prestige_upgrade("资源保留")
            {
                can_reset = true
            };
            double2[] data_资源保留 = new double2[] { 32000, 2e5, 1.2e6, 9e6, 33e6, 1.35e9 };
            List<double2> pu_资源保留_cost_table = new List<double2>(data_资源保留);
            pu_资源保留.set_init_cost("转生点数", pu_资源保留_cost_table, 0, pu_资源保留_cost_table.Count);
            pu_资源保留.unlocked = false;
            m.转生_升级_资源保留_grid.Visibility = (Visibility)1;
            prestige_ups.Add("资源保留", pu_资源保留);
            #endregion

            //生成器——资源保留
            #region
            link lk_生成器_资源保留 = new link(m.转生_main_grid, m.转生_升级_生成器_grid, m.转生_升级_资源保留_grid, 3, false);
            lk_生成器_资源保留.prestige_type(pu_生成器, pu_资源保留);
            lk_生成器_资源保留.update_progress(0);
            links.Add("生成器_资源保留", lk_生成器_资源保留);
            #endregion

            //2-1 升级保留
            #region
            prestige_upgrade pu_升级保留 = new prestige_upgrade("升级保留")
            {
                can_reset = true
            };
            double2[] data_升级保留 = new double2[] { 20e6, 180e6, 150e9 };
            List<double2> pu_升级保留_cost_table = new List<double2>(data_升级保留);
            pu_升级保留.set_init_cost("转生点数", pu_升级保留_cost_table, 0, pu_升级保留_cost_table.Count);
            pu_升级保留.unlocked = false;
            m.转生_升级_升级保留_grid.Visibility = (Visibility)1;
            prestige_ups.Add("升级保留", pu_升级保留);
            #endregion

            //资源保留——升级保留
            #region
            link lk_资源保留_升级保留 = new link(m.转生_main_grid, m.转生_升级_资源保留_grid, m.转生_升级_升级保留_grid, 3, false);
            lk_资源保留_升级保留.prestige_type(pu_资源保留, pu_升级保留);
            lk_资源保留_升级保留.update_progress(0);
            links.Add("资源保留_升级保留", lk_资源保留_升级保留);
            #endregion

            //0-7 制造
            #region
            prestige_upgrade pu_制造 = new prestige_upgrade("制造")
            {
                can_reset = true
            };
            double2[] data_制造 = new double2[] { 12, 21e6, 25e6, 90e6 };
            List<double2> pu_制造_cost_table = new List<double2>(data_制造);
            pu_制造.set_init_cost("转生点数", pu_制造_cost_table, 0, pu_制造_cost_table.Count);
            pu_制造.unlocked = true;
            m.转生_升级_制造_grid.Visibility = 0;
            prestige_ups.Add("制造", pu_制造);
            #endregion

            //1-6 方块增幅
            #region
            prestige_upgrade pu_方块增幅 = new prestige_upgrade("方块增幅")
            {
                can_reset = true
            };
            double2[] data_方块增幅 = new double2[] { 300, 8000, 375e3, 2.25e6, 15e6 };
            List<double2> pu_方块增幅_cost_table = new List<double2>(data_方块增幅);
            pu_方块增幅.set_init_cost("转生点数", pu_方块增幅_cost_table, 0, pu_方块增幅_cost_table.Count);
            pu_方块增幅.unlocked = false;
            m.转生_升级_方块增幅_grid.Visibility = 0;
            prestige_ups.Add("方块增幅", pu_方块增幅);
            #endregion

            //制造——方块增幅
            #region
            link lk_制造_方块增幅 = new link(m.转生_main_grid, m.转生_升级_制造_grid, m.转生_升级_方块增幅_grid, 1, true);
            lk_制造_方块增幅.prestige_type(pu_制造, pu_方块增幅);
            lk_制造_方块增幅.update_progress(0);
            links.Add("制造_方块增幅", lk_制造_方块增幅);
            #endregion

            //1-7 核心
            #region
            prestige_upgrade pu_核心 = new prestige_upgrade("核心")
            {
                can_reset = true
            };
            double2[] data_核心 = new double2[] { 1e15 };
            List<double2> pu_核心_cost_table = new List<double2>(data_核心);
            pu_核心.set_init_cost("转生点数", pu_核心_cost_table, 0, pu_核心_cost_table.Count);
            pu_核心.unlocked = false;
            m.转生_升级_核心_grid.Visibility = 0;
            prestige_ups.Add("核心", pu_核心);
            #endregion

            //制造——核心
            #region
            link lk_制造_核心 = new link(m.转生_main_grid, m.转生_升级_制造_grid, m.转生_升级_核心_grid, 4, true);
            lk_制造_核心.prestige_type(pu_制造, pu_核心);
            lk_制造_核心.update_progress(0);
            links.Add("制造_核心", lk_制造_核心);
            #endregion 


            //1-5 时间力量
            #region
            prestige_upgrade pu_时间力量 = new prestige_upgrade("时间力量")
            {
                can_reset = true
            };
            double2[] data_时间力量 = new double2[] { 5e9, 225e9, 33e12 };
            List<double2> pu_时间力量_cost_table = new List<double2>(data_时间力量);
            pu_时间力量.set_init_cost("转生点数", pu_时间力量_cost_table, 0, pu_时间力量_cost_table.Count);
            pu_时间力量.unlocked = false;
            m.转生_升级_时间力量_grid.Visibility = (Visibility)1;
            prestige_ups.Add("时间力量", pu_时间力量);
            #endregion

            //方块增幅——时间力量
            #region
            link lk_方块增幅_时间力量 = new link(m.转生_main_grid, m.转生_升级_方块增幅_grid, m.转生_升级_时间力量_grid, 5, false);
            lk_方块增幅_时间力量.prestige_type(pu_方块增幅, pu_时间力量);
            lk_方块增幅_时间力量.update_progress(0);
            links.Add("方块增幅_时间力量", lk_方块增幅_时间力量);
            #endregion

            //2-6 魔法增幅
            #region
            prestige_upgrade pu_魔法增幅 = new prestige_upgrade("魔法增幅")
            {
                can_reset = true
            };
            double2[] data_魔法增幅 = new double2[] { 200e3, 6e6, 350e6 };
            List<double2> pu_魔法增幅_cost_table = new List<double2>(data_魔法增幅);
            pu_魔法增幅.set_init_cost("转生点数", pu_魔法增幅_cost_table, 0, pu_魔法增幅_cost_table.Count);
            pu_魔法增幅.unlocked = false;
            m.转生_升级_魔法增幅_grid.Visibility = (Visibility)1;
            prestige_ups.Add("魔法增幅", pu_魔法增幅);
            #endregion

            //方块增幅——魔法增幅
            #region
            link lk_方块增幅_魔法增幅 = new link(m.转生_main_grid, m.转生_升级_方块增幅_grid, m.转生_升级_魔法增幅_grid, 3, false);
            lk_方块增幅_魔法增幅.prestige_type(pu_方块增幅, pu_魔法增幅);
            lk_方块增幅_魔法增幅.update_progress(0);
            links.Add("方块增幅_魔法增幅", lk_方块增幅_魔法增幅);
            #endregion


            //2-5 转化
            #region
            prestige_upgrade pu_转化 = new prestige_upgrade("转化")
            {
                can_reset = true
            };
            double2[] data_转化 = new double2[] { 20e9, 45e12, 125e15 };
            List<double2> pu_转化_cost_table = new List<double2>(data_转化);
            pu_转化.set_init_cost("转生点数", pu_转化_cost_table, 0, pu_转化_cost_table.Count);
            pu_转化.unlocked = false;
            m.转生_升级_转化_grid.Visibility = (Visibility)1;
            prestige_ups.Add("转化", pu_转化);
            #endregion

            //魔法增幅——转化
            #region
            link lk_魔法增幅_转化 = new link(m.转生_main_grid, m.转生_升级_魔法增幅_grid, m.转生_升级_转化_grid, 3, false);
            lk_魔法增幅_转化.prestige_type(pu_魔法增幅, pu_转化);
            lk_魔法增幅_转化.update_progress(0);
            links.Add("魔法增幅_转化", lk_魔法增幅_转化);
            #endregion

            //3-6 采矿增幅
            #region
            prestige_upgrade pu_采矿增幅 = new prestige_upgrade("采矿增幅")
            {
                can_reset = true
            };
            double2[] data_采矿增幅 = new double2[] { 30e6, 200e6, 3.5e9, 12e9, 1.35e12 };
            List<double2> pu_采矿增幅_cost_table = new List<double2>(data_采矿增幅);
            pu_采矿增幅.set_init_cost("转生点数", pu_采矿增幅_cost_table, 0, pu_采矿增幅_cost_table.Count);
            pu_采矿增幅.unlocked = false;
            m.转生_升级_采矿增幅_grid.Visibility = (Visibility)1;
            prestige_ups.Add("采矿增幅", pu_采矿增幅);
            #endregion

            //魔法增幅——采矿增幅
            #region
            link lk_魔法增幅_采矿增幅 = new link(m.转生_main_grid, m.转生_升级_魔法增幅_grid, m.转生_升级_采矿增幅_grid, 2, false);
            lk_魔法增幅_采矿增幅.prestige_type(pu_魔法增幅, pu_采矿增幅);
            lk_魔法增幅_采矿增幅.update_progress(0);
            links.Add("魔法增幅_采矿增幅", lk_魔法增幅_采矿增幅);
            #endregion

            //0-6 战斗增幅
            #region
            prestige_upgrade pu_战斗增幅 = new prestige_upgrade("战斗增幅")
            {
                can_reset = true
            };
            double2[] data_战斗增幅 = new double2[] { 800, 2500, 16000, 55000, 150e3,
                                                    900e3, 10e6, 50e6, 1.337e9, 30e9,
                                                    20e12};
            List<double2> pu_战斗增幅_cost_table = new List<double2>(data_战斗增幅);
            pu_战斗增幅.set_init_cost("转生点数", pu_战斗增幅_cost_table, 0, pu_战斗增幅_cost_table.Count);
            pu_战斗增幅.unlocked = false;
            m.转生_升级_战斗增幅_grid.Visibility = 0;
            prestige_ups.Add("战斗增幅", pu_战斗增幅);
            #endregion

            //制造——战斗增幅
            #region
            link lk_制造_战斗增幅 = new link(m.转生_main_grid, m.转生_升级_制造_grid, m.转生_升级_战斗增幅_grid, 1, true);
            lk_制造_战斗增幅.prestige_type(pu_制造, pu_战斗增幅);
            lk_制造_战斗增幅.update_progress(0);
            links.Add("制造_战斗增幅", lk_制造_战斗增幅);
            #endregion




            //0-5 强化等级
            #region
            prestige_upgrade pu_强化等级 = new prestige_upgrade("强化等级")
            {
                can_reset = true
            };
            double2[] data_强化等级 = new double2[] { 4000, 45000, 215e3, 680e3, 4e6, 25e6, 200e6, 3.2e9, 100e12 };
            List<double2> pu_强化等级_cost_table = new List<double2>(data_强化等级);
            pu_强化等级.set_init_cost("转生点数", pu_强化等级_cost_table, 0, pu_强化等级_cost_table.Count);
            pu_强化等级.unlocked = false;
            m.转生_升级_强化等级_grid.Visibility = (Visibility)1;
            prestige_ups.Add("强化等级", pu_强化等级);
            #endregion

            //战斗增幅——强化等级
            #region
            link lk_战斗增幅_强化等级 = new link(m.转生_main_grid, m.转生_升级_战斗增幅_grid, m.转生_升级_强化等级_grid, 2, false);
            lk_战斗增幅_强化等级.prestige_type(pu_战斗增幅, pu_强化等级);
            lk_战斗增幅_强化等级.update_progress(0);
            links.Add("战斗增幅_强化等级", lk_战斗增幅_强化等级);
            #endregion

            //0-4 战斗探索
            #region
            prestige_upgrade pu_战斗探索 = new prestige_upgrade("战斗探索")
            {
                can_reset = true
            };
            double2[] data_战斗探索 = new double2[] { 100e3, 17.5e6, 12.5e9 };//获得两个虚拟的探索魔法等级用于本升级
            List<double2> pu_战斗探索_cost_table = new List<double2>(data_战斗探索);
            pu_战斗探索.set_init_cost("转生点数", pu_战斗探索_cost_table, 0, pu_战斗探索_cost_table.Count);
            pu_战斗探索.unlocked = false;
            m.转生_升级_战斗探索_grid.Visibility = (Visibility)1;
            prestige_ups.Add("战斗探索", pu_战斗探索);
            #endregion

            //强化等级——战斗探索
            #region
            link lk_强化等级_战斗探索 = new link(m.转生_main_grid, m.转生_升级_强化等级_grid, m.转生_升级_战斗探索_grid, 3, false);
            lk_强化等级_战斗探索.prestige_type(pu_强化等级, pu_战斗探索);
            lk_强化等级_战斗探索.update_progress(0);
            links.Add("强化等级_战斗探索", lk_强化等级_战斗探索);
            #endregion

            //0-3 冷静
            #region
            prestige_upgrade pu_冷静 = new prestige_upgrade("冷静")
            {
                can_reset = true
            };
            double2[] data_冷静 = new double2[] { 135e6, 4e9, 75e12 };
            List<double2> pu_冷静_cost_table = new List<double2>(data_冷静);
            pu_冷静.set_init_cost("转生点数", pu_冷静_cost_table, 0, pu_冷静_cost_table.Count);
            pu_冷静.unlocked = false;
            m.转生_升级_冷静_grid.Visibility = (Visibility)1;
            prestige_ups.Add("冷静", pu_冷静);
            #endregion

            //战斗探索——冷静
            #region
            link lk_战斗探索_冷静 = new link(m.转生_main_grid, m.转生_升级_战斗探索_grid, m.转生_升级_冷静_grid, 1, false);
            lk_战斗探索_冷静.prestige_type(pu_战斗探索, pu_冷静);
            lk_战斗探索_冷静.update_progress(0);
            links.Add("战斗探索_冷静", lk_战斗探索_冷静);
            #endregion



            //5-0 成就加成
            #region
            prestige_upgrade pu_成就加成 = new prestige_upgrade("成就加成")
            {
                can_reset = true
            };
            double2[] data_成就加成 = new double2[] { 1000, 500e6 };
            List<double2> pu_成就加成_cost_table = new List<double2>(data_成就加成);
            pu_成就加成.set_init_cost("转生点数", pu_成就加成_cost_table, 0, pu_成就加成_cost_table.Count);
            pu_成就加成.unlocked = true;
            m.转生_升级_成就加成_grid.Visibility = 0;
            prestige_ups.Add("成就加成", pu_成就加成);
            #endregion


            //成就
            #region
            init_achieve();
            achieve_generate();
            #endregion

            #endregion

            m.offline_grid.Visibility = Visibility.Hidden;
            
            if (load_auto && File.Exists("./存档/auto/auto.a.mixidle"))
            {
                load();
            }
            else
            {

            }
            time_start();
        }

        public void time_start()
        {
            if (m.gs.Equals(this))
            {
                if (!ticker.IsEnabled)
                {
                    ticker.Interval = new TimeSpan(0, 0, 0, 0, tick_time);
                    ticker.Tick += game_tick;
                    ticker.Start();
                    show();
                }
            }
            else
            {
                if (ticker.IsEnabled)
                {
                    ticker.Stop();
                }
            }
        }
    }
}
