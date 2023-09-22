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
        public double2 g1_cal_wp_effect()
        {
            double2 effect = 0;
            resource r = find_resource("世界点数");
            effect += double2.Pow(double2.Log10(r.get_value() + 1), 3) * 0.0001;
            return effect;
        }


        #region layer 森林
        public double2 g1_cal_life_point()
        {
            double2 wp_v = find_resource("世界点数").get_value();
            resource life_p = find_resource("生命力");
            return double2.Pow(wp_v, 0.3) * double2.Pow(yggdrasill.level, yggdrasill.produce_ex) 
                * life_p.get_mul();
        }

        public double2 g1_cal_life_effect(bool pool = false, string s_pool = "")
        {
            double2 effect = 1;
            resource life_p = find_resource("生命力");
            double2 power = find_resource("生命效果").get_value();

            if (pool)
            {
                double2 real = life_p.see_pool(s_pool);
                effect = double2.Max(effect, double2.Pow(real, power));
            }
            else
            {
                effect = double2.Max(effect, double2.Pow(life_p.get_value(), power));
            }
            return effect;
        }
        #endregion layer 森林

        #region 关卡cal
        public Tuple<resource, double2> g1_cal_生命力_自然树()
        {
            resource r = find_resource("生命力");
            double2 inpool = r.see_pool("自然树");
            return new Tuple<resource, double2>(r, inpool);
        }
        public Tuple<resource, double2> g1_cal_生命力_水晶树()
        {
            resource r = find_resource("生命力");
            double2 inpool = r.see_pool("水晶树");
            return new Tuple<resource, double2>(r, inpool);
        }
        public Tuple<resource, double2> g1_cal_生命力_合成树()
        {
            resource r = find_resource("生命力");
            double2 inpool = r.see_pool("合成树");
            return new Tuple<resource, double2>(r, inpool);
        }
        #endregion 关卡cal

        #region layer 自然树
        public double2 g1_cal_natural_effect()
        {
            double2 effect = 1;
            resource r = find_resource("自然力量");
            effect = double2.Max(effect, double2.Pow(r.get_value(), 0.5));
            return effect;
        }



        //mul
        public double2 g1_cal_npower_eco_effect()
        {
            double2 effect = 0.4;
            
            resource r = find_resource("自然力量");
            effect += 0.6 * (1 - 40 / (40 + double2.Pow((r.get_value() + 1).Log10(), 1.6)));
            return effect;
        }
        //mul
        public double2 g1_cal_npower_npoint_effect()
        {
            double2 effect = 1.5;

            resource r = find_resource("自然力量");
            effect += double2.Log10(r.get_value() / 1e5 + 1) * 0.1;
            return effect;
        }
        #endregion layer 自然树

        #region layer 水晶树
        public double2 g1_cal_crystalball_effect()
        {
            double2 effect = 0;
            resource r = find_resource("水晶球");
            effect = double2.Pow((r.get_value() + 1).Log10(), 0.5) * 0.01;
            return effect;
        }
        public double2 g1_cal_refraction_effect()
        {
            double2 effect = 0;
            resource r = find_resource("水晶球");
            effect = double2.Pow(r.get_value(), 0.4);
            return effect;
        }
        public double2 g1_cal_sacrifice_effect(double2 val)
        {
            double2 effect = 0;
            resource r = find_resource("水晶球");
            effect = double2.Pow(r.get_value(), 1.0 / val);
            effect = double2.Max(effect, 1);
            return effect;
        }
        #endregion layer 水晶树

        #region layer 合成树
        public double2 g1_cal_craft_effect()
        {
            double2 effect = 1;
            resource r = find_resource("合成熟练度");
            if (r != null)
            {
                effect += double2.Pow(r.get_value(), 0.2);
            }
            return effect;
        }
        #endregion layer 合成树

        #region layer 文明
        //mul
        public double2 g1_cal_exp_summary_gain()
        {
            double2 effect = 0.1;

            resource r = find_resource("文明水平");
            effect *=  1 / (1 + double2.Pow(r.get_value(), 2 + 
                double2.Log10(r.get_value() + 1)));
            return effect;
        }
        //mul
        public double2 global_xp_boost()
        {
            double2 effect = 1;
            g1_upgrade r = g1_ups["世界_经验总结"];
            if (r.level >= 1)
            {
                effect += 0.2;
            }
            return effect;
        }
        //mul
        public double2 g1_cal_life_cp_syn()
        {
            double2 effect = 1;
            resource r = find_resource("生命力");
            resource r2 = find_resource("文明水平");
            effect += double2.Pow(double2.Log10(r.get_value() + 1), 0.6) *
                 double2.Pow(double2.Log10(r2.get_value() + 10), 3);
            return effect;
        }
        //mul
        public double2 g1_cal_cp_to_yxp_syn()
        {
            double2 effect = 1;
            resource r = find_resource("文明水平");
            effect = double2.Max(1, double2.Pow(r.get_value(), 0.5));
            return effect;
        }
        //mul
        public double2 g1_cal_ylv_to_cp_syn()
        {
            double2 effect = 1;
            resource r = find_resource("世界树等级");
            effect = double2.Max(1, r.get_value());
            return effect;
        }
        #endregion layer 文明












        #region 研究
        public double2 g1_cal_research_food_food_gain()
        {
            g1_research r = g1_ups["世界_食物学"] as g1_research;
            double2 effect = 1 + double2.Log10(r.xp.level + 1) * 0.05;
            return effect;
        }
        //add
        public double2 g1_cal_research_food_cp_gain()
        {
            g1_research r = g1_ups["世界_食物学"] as g1_research;
            double2 effect = double2.Pow(r.xp.level, 0.6) / 
                double2.Log10(r.xp.level + 9);
            return effect;
        }

        public double2 g1_cal_research_battle_attack_gain()
        {
            g1_research r = g1_ups["世界_战斗学"] as g1_research;
            double2 effect = 1 + double2.Pow(double2.Log10(r.xp.level + 1), 1.5) * 0.05;
            return effect;
        }
        //add
        public double2 g1_cal_research_battle_cp_gain()
        {
            g1_research r = g1_ups["世界_战斗学"] as g1_research;
            double2 effect = double2.Pow(r.xp.level, 0.6) /
                double2.Log10(r.xp.level + 9);
            return effect;
        }

        //add
        public double2 g1_cal_research_language_cp_gain()
        {
            g1_research r = g1_ups["世界_语言与文字"] as g1_research;
            double2 effect = double2.Pow(r.xp.level, 1) /
                double2.Log10(r.xp.level + 9);
            return effect;
        }


        public double2 g1_cal_research_water_potion_gain()
        {
            g1_research r = g1_ups["世界_水源工程"] as g1_research;
            double2 effect = 1 + double2.Pow(double2.Log10(r.xp.level + 1), 1.3) * 0.05;
            return effect;
        }
        public double2 g1_cal_research_water_water_gain()
        {
            g1_research r = g1_ups["世界_水源工程"] as g1_research;
            double2 effect = 1 + double2.Pow(double2.Log10(r.xp.level + 1), 3) * 0.1;
            return effect;
        }
        //add
        public double2 g1_cal_research_water_cp_gain()
        {
            g1_research r = g1_ups["世界_水源工程"] as g1_research;
            double2 effect = double2.Pow(r.xp.level, 0.6) /
                double2.Log10(r.xp.level + 99) +
                double2.Log10(r.xp.level + 1) * 5;
            return effect;
        }

        public double2 g1_cal_research_wood_wood_gain()
        {
            g1_research r = g1_ups["世界_林业"] as g1_research;
            double2 effect = 1 + double2.Pow(double2.Log10(r.xp.level + 1), 1.8) * 0.05;
            return effect;
        }
        public double2 g1_cal_research_wood_life_gain()
        {
            g1_research r = g1_ups["世界_林业"] as g1_research;
            double2 effect = 1 + double2.Pow(double2.Log10(r.xp.level + 1), 2.5) * 0.1;
            return effect;
        }
        //add
        public double2 g1_cal_research_wood_cp_gain()
        {
            g1_research r = g1_ups["世界_林业"] as g1_research;
            double2 effect = double2.Pow(r.xp.level, 0.6) /
                double2.Log10(r.xp.level + 99) +
                double2.Log10(r.xp.level + 1) * 10;
            return effect;
        }
        #endregion 研究

        #region level 世界树
        #region 0
        public double2 g1_cal_0_wp_effect()
        {
            double2 effect = 0;

            resource r = find_resource("0层点数");
            effect += double2.Max(0, (double2.Pow(r.get_value(), 0.01) - 1) * 10);
            return effect;
        }
        public double2 g1_cal_maze_0_effect()
        {
            double2 effect = 1;

            resource r = find_resource("0层点数");
            effect = 4 + double2.Pow(find_resource("迷宫元素").get_value(), 2) * 2;
            return effect;
        }
        #endregion 0
        #region 1
        public double2 g1_cal_1_0_effect_mul()
        {
            double2 effect = 1;

            resource r = find_resource("1层点数");
            effect += double2.Log10(r.get_value() + 1) * 0.4;
            return effect;
        }

        public double2 g1_cal_1_0_effect_add()
        {
            double2 effect = 0;

            resource r = find_resource("1层点数");
            effect += double2.Pow(double2.Log10(r.get_value() + 1), 3) * 0.001;
            return effect;
        }

        public double2 g1_cal_1_crystal_effect()
        {
            double2 effect = 0;

            resource r = find_resource("1层点数");
            if (g1_ups["世界树_闪亮水晶"].level >= 1)
            {
                effect += 3 + double2.Pow(r.get_value(), 0.2);
            }
            return effect;
        }

        public double2 g1_cal_1_crystal_crystalpiece_effect()
        {
            double2 effect = 0;
            resource r = find_resource("水晶质量");
            if (r == null)
            {
                return 1;
            }
            effect += double2.Pow(r.get_value() + 1, 0.1);
            return effect;
        }
        #endregion 1
        #endregion level 世界树

        #region level 自然树
        #region layer 树苗

        public double2 g1_cal_s2_nu_effect()
        {
            if (g1_ups["自然树_营养供给"].level >= 1)
            {
                double2 effect = 3;
                resource r = find_resource("树苗");
                effect += double2.Max(0, double2.Pow(r.get_value(), 0.1) - 1);
                return effect;
            }
            return 1;
        }
        #endregion layer 树苗

        #region layer 水
        //add
        public double2 g1_cal_water_effect()
        {
            double2 effect = 0;
            resource r = find_resource("水");

            double2 power = 0.5;
            if ((g1_ups["自然树_水_里程碑_1"] as g1_milestone).completed)
            {
                power = 0.6;
            }
            power += g1_cal_nu_water_effect();

            effect = double2.Pow(r.get_value(), power);
            return effect;
        }

        //mul
        public double2 g1_cal_water_np_mul()
        {
            double2 effect = 1;
            resource r = find_resource("自然点数");
            effect = 1 + (r.get_value() + 1).Log10();
            return effect;
        }
        //mul
        public double2 g1_cal_water_water_mul()
        {
            double2 effect = 1;
            resource r = find_resource("水");
            effect = 1 + double2.Pow(r.get_value(), 0.15);
            return effect;
        }
        //mul
        public double2 g1_cal_water_from_tree_mul()
        {
            double2 effect = 1;

            double2 score = 0;
            resource r = find_resource("种子成长度");
            resource r2 = find_resource("树苗成长度");
            score += r.get_value();
            score += 5 * double2.Pow(r2.get_value(), 1.5);

            effect = 1 + double2.Pow(score, 0.25);
            return effect;
        }

        public double2 g1_cal_water_nu_gain = 0;
        #endregion layer 水

        #region layer 营养
        //mul
        public double2 g1_cal_nu_tree_effect()
        {
            double2 effect = 1;
            resource r = find_resource("营养");

            double2 power = 0.3;
            if ((g1_ups["自然树_营养_里程碑_1"] as g1_milestone).completed)
            {
                power *= 1.1;
            }

            effect = double2.Max(effect, double2.Pow(r.get_value(), power));
            return effect;
        }


        //add
        public double2 g1_cal_nu_water_effect()
        {
            double2 effect = 0;
            resource r = find_resource("营养");

            double2 power = 0.05;
            if ((g1_ups["自然树_营养_里程碑_1"] as g1_milestone).completed)
            {
                power *= 1.1;
            }

            effect = (r.get_value() + 1).Log10() * power;
            return effect;
        }

        //mul
        public double2 g1_cal_nu_light_effect()
        {
            double2 effect = 1;
            resource r = find_resource("营养");
            effect = 1 + 10 / (1 + 0.1 * double2.Pow((r.get_value() + 1).Log10(), 2));
            return effect;
        }

        //mul
        public double2 g1_cal_nu_fat_effect()
        {
            double2 effect = 1;

            double2 score = 0;
            resource r = find_resource("种子");
            resource r2 = find_resource("树苗");
            resource r3 = find_resource("小树");
            resource r_ = find_resource("种子成长度");
            resource r2_ = find_resource("树苗成长度");
            resource r3_ = find_resource("小树成长度");

            if (r.get_value() > 0)
            {
                score += r_.get_value();
            }
            if (r2.get_value() > 0)
            {
                score += double2.Pow(r2_.get_value(), 2);
            }
            if (r3.get_value() > 0)
            {
                score += double2.Pow(r3_.get_value(), 4);
            }

            effect = 1 + 10 / (1 + 0.1 * double2.Pow((score + 1).Log10(), 2));
            return effect;
        }

        //mul
        public double2 g1_cal_nu_air_effect()
        {
            double2 effect = 1;
            resource r = find_resource("生命力");
            effect = 1 + 0.5 * (r.see_pool("自然树") + 1).Log10();
            return effect;
        }
        #endregion layer 营养
        #endregion level 自然树

        #region level 水晶树
        double2 g1_crystal_mul = 1;

        public double2 g1_cal_lifec_red_effect()
        {
            double2 effect = 1;
            resource r = find_resource("生命力");
            resource r2 = find_resource("生命转化倍增");
            resource r3 = find_resource("生命转化");
            effect = double2.Pow((r.best + 1) * r2.get_value() *
                g1_cal_R_art1_effect(false), r3.get_value());
            return effect;
        }

        #region crystal rgb
        public double2 g1_cal_crystal_rgb_log_avg()
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            resource r2 = find_resource("绿色水晶");
            resource r3 = find_resource("蓝色水晶");
            double2 logs =
                double2.Max(r.get_value(), 1).Log10() *
                double2.Max(r2.get_value(), 1).Log10() *
                double2.Max(r3.get_value(), 1).Log10();
            logs = double2.Pow(logs, 1 / 3.0);
            effect = 3 * double2.Pow(10, logs);
            return effect;
        }

        public double2 g1_cal_crystal_rgb_log_avg_virtual(
            double2 dr, double2 dg, double2 db)
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            resource r2 = find_resource("绿色水晶");
            resource r3 = find_resource("蓝色水晶");
            double2 logs =
                double2.Max(r.get_value() + dr, 1).Log10() *
                double2.Max(r2.get_value() + dg, 1).Log10() *
                double2.Max(r3.get_value() + db, 1).Log10();
            logs = double2.Pow(logs, 1 / 3.0);
            effect = 3 * double2.Pow(10, logs);
            return effect;
        }
        #endregion crystal rgb
        #region crystal all
        public double2 g1_cal_crystal_all_log_avg()
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            resource r2 = find_resource("绿色水晶");
            resource r3 = find_resource("蓝色水晶");
            resource r4 = find_resource("黄色水晶");
            resource r5 = find_resource("洋红色水晶");
            resource r6 = find_resource("青色水晶");
            resource r7 = find_resource("白色水晶");
            double2 logs =
                double2.Max(r.get_value(), 1).Log10() *
                double2.Max(r2.get_value(), 1).Log10() *
                double2.Max(r3.get_value(), 1).Log10() *
                double2.Max(r4.get_value(), 1).Log10() *
                double2.Max(r5.get_value(), 1).Log10() *
                double2.Max(r6.get_value(), 1).Log10() *
                double2.Max(r7.get_value(), 1).Log10();
            logs = double2.Pow(logs, 1 / 7.0);
            effect = 7 * double2.Pow(10, logs);
            return effect;
        }

        public double2 g1_cal_crystal_all_log_avg_virtual(
            double2 dr, double2 dg, double2 db,
            double2 dy, double2 dm, double2 dc,
            double2 dw)
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            resource r2 = find_resource("绿色水晶");
            resource r3 = find_resource("蓝色水晶");
            resource r4 = find_resource("黄色水晶");
            resource r5 = find_resource("洋红色水晶");
            resource r6 = find_resource("青色水晶");
            resource r7 = find_resource("白色水晶");
            double2 logs =
                double2.Max(r.get_value() + dr, 1).Log10() *
                double2.Max(r2.get_value() + dg, 1).Log10() *
                double2.Max(r3.get_value() + db, 1).Log10() * 
                double2.Max(r4.get_value() + dy, 1).Log10() *
                double2.Max(r5.get_value() + dm, 1).Log10() *
                double2.Max(r6.get_value() + dc, 1).Log10() *
                double2.Max(r7.get_value() + dw, 1).Log10();
            logs = double2.Pow(logs, 1 / 7.0);
            effect = 7 * double2.Pow(10, logs);
            return effect;
        }
        #endregion crystal all
        

        #region rgb_max
        public List<resource> g1_crystal_rgb_max = new List<resource>();
        public List<resource> g1_prepare_crystal_rgb_max()
        {
            resource[] r = new resource[3];
            r[0] = find_resource("红色水晶");
            r[1] = find_resource("绿色水晶");
            r[2] = find_resource("蓝色水晶");
            return res_max(r);
        }
        #endregion rgb_max

        #region rgb_min
        public List<resource> g1_crystal_rgb_min = new List<resource>();
        public List<resource> g1_prepare_crystal_rgb_min()
        {
            resource[] r = new resource[3];
            r[0] = find_resource("红色水晶");
            r[1] = find_resource("绿色水晶");
            r[2] = find_resource("蓝色水晶");
            return res_min(r);
        }
        #endregion rgb_min


        #region all_max
        public List<resource> g1_crystal_all_max = new List<resource>();
        public List<resource> g1_prepare_crystal_all_max()
        {
            resource[] r = new resource[7];
            r[0] = find_resource("红色水晶");
            r[1] = find_resource("绿色水晶");
            r[2] = find_resource("蓝色水晶");
            r[3] = find_resource("黄色水晶");
            r[4] = find_resource("洋红色水晶");
            r[5] = find_resource("青色水晶");
            r[6] = find_resource("白色水晶");
            return res_max(r);
        }
        #endregion all_max

        #region all_min
        public List<resource> g1_crystal_all_min = new List<resource>();
        public List<resource> g1_prepare_crystal_all_min()
        {
            resource[] r = new resource[7];
            r[0] = find_resource("红色水晶");
            r[1] = find_resource("绿色水晶");
            r[2] = find_resource("蓝色水晶");
            r[3] = find_resource("黄色水晶");
            r[4] = find_resource("洋红色水晶");
            r[5] = find_resource("青色水晶");
            r[6] = find_resource("白色水晶");
            return res_min(r);
        }
        #endregion all_min


        #region curr_max
        public List<resource> g1_crystal_curr_max = new List<resource>();
        #endregion curr_max

        #region curr_min
        public List<resource> g1_crystal_curr_min = new List<resource>();
        #endregion curr_min

        #region Y convert
        public double2 g1_cal_Y_convert()
        {
            resource x = find_resource("黄色水晶");
            resource[] r = new resource[3];
            r[0] = find_resource("红色水晶");
            r[1] = find_resource("绿色水晶");
            r[2] = one;
            return res_L_AAVG(r) * x.get_mul();
        }
        #endregion Y convert
        #region M convert
        public double2 g1_cal_M_convert()
        {
            resource x = find_resource("洋红色水晶");
            resource[] r = new resource[3];
            r[0] = find_resource("红色水晶");
            r[2] = one;
            r[1] = find_resource("蓝色水晶");
            return res_L_AAVG(r) * x.get_mul();
        }
        #endregion M convert
        #region C convert
        public double2 g1_cal_C_convert()
        {
            resource x = find_resource("青色水晶");
            resource[] r = new resource[3];
            r[2] = one;
            r[0] = find_resource("绿色水晶");
            r[1] = find_resource("蓝色水晶");
            return res_L_AAVG(r) * x.get_mul();
        }
        #endregion C convert
        #region W convert
        public double2 g1_cal_W_convert()
        {
            resource x = find_resource("白色水晶");
            resource[] r = new resource[6];
            r[3] = one;
            r[4] = one;
            r[5] = one;
            r[0] = find_resource("黄色水晶");
            r[1] = find_resource("洋红色水晶");
            r[2] = find_resource("青色水晶");
            return res_L_AAVG(r) * x.get_mul();
        }
        #endregion W convert

        #region A
        public double2 art_mul = 0;
        public double2 g1_cal_A_2_art1_condition;
        public double2 g1_cal_A_2_art1_effect1(bool newer)
        {
            double2 effect = 1;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("透亮圆盘");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            effect += double2.Pow(double2.L(value / 1e15), 5);
            return effect;
        }
        public double2 g1_cal_A_2_art1_effect2(bool newer)
        {
            double2 effect = 1;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("透亮圆盘");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            effect += double2.Pow(double2.L(value / 1e15), 4);
            return effect;
        }

        public resource g1_cal_A_2_art2_select;
        public double2 g1_cal_A_2_art2_effect(bool newer)
        {
            double2 effect = 1;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("超大水晶");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            effect += double2.Pow(double2.L(value), 0.666);
            return effect;
        }

        public double2 g1_cal_A_2_art3_condition;
        public double2 g1_cal_A_2_art3_effect(bool newer)
        {
            double2 effect = 0;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("冰糖果冻");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            effect += 1 - 5 / (5 + double2.L(value));
            return effect;
        }
        #endregion A
        #region R
        public double2 g1_cal_R_art1_effect(bool newer)
        {
            double2 effect = 1;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("爱心宝石");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            double2 suppress = 1;
            if (value > 1e100)
            {
                suppress = 1 + (double2.Pow(double2.Log10(value / 1e96), 0.5) - 2)
                    * 0.04;
            }
            effect = double2.Max(double2.Pow(value, 1.0 / suppress), 1);
            return effect;
        }

        public double2 g1_cal_R_balance_1_effect()
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            effect += double2.Pow(r.get_value(), 0.3);
            return effect;
        }
        public double2 g1_cal_R_balance_2_effect()
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            effect += double2.Pow(r.get_value(), 0.12);
            return effect;
        }
        public Tuple<bool, double2> g1_cal_R_cconvert_temp;
        public Tuple<bool, double2> g1_cal_R_cconvert_effect()
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            resource r2 = find_resource("水晶块");
            double2 ratio = 1;
            if(r2.get_value() != 0 && r.get_value() != 0)
            {
                ratio = r2.get_value() / (r.get_value() * 1000);
            }
            if (ratio < 1)
            {
                double2 power = 1 / ratio;
                effect = 2 + double2.Pow(double2.Log10(power + 9), 3);
                return new Tuple<bool, double2>(true, effect);
            }
            else
            {
                double2 power = ratio;
                effect = 2 + double2.Pow(double2.Log10(power + 9), 2);
                return new Tuple<bool, double2>(false, effect);
            }
        }
        #endregion R
        #region G
        public double2 g1_cal_G_art1_effect(bool newer)
        {
            double2 effect = 1;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("坚固叶片");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            effect += double2.Pow(value, 0.1);
            return effect;
        }

        public double2 g1_cal_G_absorb_effect()
        {
            double2 effect = 1;
            resource r = find_resource("红色水晶");
            resource g = find_resource("绿色水晶");
            resource b = find_resource("蓝色水晶");
            effect = double2.Max(1, 
                double2.Pow(double2.Pow(r.get_value() * b.get_value(), 0.75) / (g.get_value() + 1), 0.5));
            return effect;
        }

        public double2 g1_cal_G_lifemix_effect()
        {
            double2 effect = 1;
            effect = double2.Pow(g1_cal_life_effect(true, "水晶树"), 0.44);
            return effect;
        }
        public double2 g1_cal_G_lifemix_gain()
        {
            double2 effect = 1;
            double2 inpool = find_resource("生命力").see_pool("水晶树");
            effect = double2.Pow(inpool,
                g1_ups["水晶树_生命混合"].acc_time);
            return effect;
        }
        #endregion G
        #region B
        public double2 g1_cal_B_art1_effect(bool newer)
        {
            double2 effect = 1;
            resource r = find_resource("水晶块");
            double2 value = r.see_pool("精致时钟");
            if (newer)
            {
                value += r.get_value();
            }
            value *= art_mul;
            double2 time = g1_levels["水晶树"].in_game_time;
            if (time > 1000)
            {
                time = 1000 + double2.Pow(time - 1000, 0.6);
            }
            effect = double2.Pow(double2.Max(time, 1),
                double2.Log10(value + 1) * 0.035);
            return effect;
        }

        public double2 g1_cal_B_duplicate_effect()
        {
            double2 effect = 1;
            resource r = find_resource("蓝色水晶");
            effect += double2.Pow(r.get_value(), 0.5);
            if(effect > 10000)
            {
                effect = 10000 + double2.Pow(effect - 10000, 0.66);
            }
            return effect;
        }
        #endregion B
        #region F
        public double2 g1_cal_F_Rpickaxe_effect()
        {
            double2 effect = 1;
            g1_upgrade u = g1_ups["水晶树_红水晶镐"];
            if (u.accing)
            {
                effect = 1 + 99 / double2.Pow(double2.Log10(
                    u.acc_time + 10), 2);
                effect *= double2.Pow(u.count, 0.33);
            }
            return effect;
        }
        public double2 g1_cal_F_Gscanner_effect()
        {
            double2 effect = 1;
            g1_upgrade u = g1_ups["水晶树_绿水晶探测器"];
            effect = 1 + 99 / double2.Pow(double2.Log10(
                find_resource("水晶块").get_value() + 10), 0.6);
            effect *= double2.Pow(u.count, 0.5);
            effect = double2.Max(1, effect);
            return effect;
        }
        public double2 g1_cal_F_Bspin_effect()
        {
            double2 effect = 1;
            g1_upgrade u = g1_ups["水晶树_蓝水晶转盘"];
            effect = 1 + 99 * double2.Pow(u.acc_time, 1.5) * double2.Pow(u.acc_time2, 1.5);
            effect *= double2.Pow(u.count, 0.66);
            effect = double2.Max(1, effect);
            return effect;
        }
        public double2 g1_cal_F_Raxe_effect()
        {
            double2 effect = 1;
            g1_upgrade u = g1_ups["水晶树_红色斧头"];
            if (u.accing)
            {
                effect = 1 + 1204 / double2.Pow(double2.Log10(
                    u.acc_time + 10), 1.5);
                effect *= double2.Pow(u.count, 0.8);
            }
            return effect;
        }
        public double2 g1_cal_F_Grecycler_effect()
        {
            double2 effect = 1;
            g1_upgrade u = g1_ups["水晶树_水晶回收器"];
            if(u.count == 0)
            {
                return effect;
            }

            double2 c_sum = 0;
            c_sum += find_resource("红色水晶").get_value();
            c_sum += find_resource("绿色水晶").get_value();
            c_sum += find_resource("蓝色水晶").get_value();

            double2 cx_sum = 0;
            cx_sum += find_resource("红色水晶原料").get_value();
            cx_sum += find_resource("绿色水晶原料").get_value();
            cx_sum += find_resource("蓝色水晶原料").get_value();

            double2 ratio = 0;
            if (cx_sum == 0)
            {
                ratio = 9999;
            }
            else
            {
                ratio = c_sum / cx_sum;
            }


            effect = 1 + 665 / double2.Pow(double2.Log10(
                ratio + 10), 2);
            effect *= double2.Pow(u.count, 0.9);
            return effect;
        }
        public double2 g1_cal_F_Bsea_effect()
        {
            double2 effect = 1;
            g1_upgrade u = g1_ups["水晶树_海浪宝石"];
            if (u.accing)
            {
                effect = 1 + 99 * u.acc_time;
                effect *= u.count;
            }
            return effect;
        }
        #endregion F
        #region Y
        public double2 g1_cal_Y_effect()
        {
            double2 effect = 1;
            resource r = find_resource("黄色水晶");
            effect += double2.Pow(r.get_value(), 0.15) * 10;
            return effect;
        }
        #endregion Y
        #region M
        public double2 g1_cal_M_time = 0;
        public double2 g1_cal_M_effect()
        {
            double2 effect = 1;
            double2 time = g1_cal_M_remain_time();
            resource r = find_resource("洋红色水晶");
            effect += double2.Pow(r.get_value(), 0.075) *
                double2.Pow(time + double2.Pow(double2.Log10(r.get_value() + 1), 1.5) * 3, 0.5);
            return effect;
        }
        public double2 g1_cal_M_remain_time()
        {
            return g1_levels["水晶树"].in_game_time - g1_cal_M_time;
        }
        #endregion M
        #region C
        public double2 g1_cal_C_effect()
        {
            double2 effect = 1;
            resource r = find_resource("青色水晶");
            effect += double2.Pow(r.get_value(), 0.2) * double2.Pow(
                double2.Log10(r.get_value() + 1), 1.5);
            return effect;
        }
        #endregion C
        #region W
        public double2 g1_cal_W_temp;
        public double2 g1_cal_W_effect()
        {
            double2 effect = 1;
            resource r = find_resource("白色水晶");
            resource[] rx = new resource[3];
            rx[0] = find_resource("黄色水晶");
            rx[1] = find_resource("洋红色水晶");
            rx[2] = find_resource("青色水晶");
            effect += double2.Pow(r.get_value(), 0.1) / (1 + 
                double2.Pow(res_DX(rx), 0.5) * 6) * 100;
            return effect;
        }
        #endregion W

        //TODO 2 art2的效果：为水晶boost；
        public void g1_next_crystal(ref resource crystal)
        {
            if (crystal.name == "红色水晶")
            {
                crystal = find_resource("绿色水晶");
                return;
            }
            if (crystal.name == "绿色水晶")
            {
                crystal = find_resource("蓝色水晶");
                return;
            }
            if (crystal.name == "蓝色水晶")
            {
                if (g1_levels["水晶树"].difficulty == g1_level.type.easy)
                {
                    crystal = find_resource("红色水晶");
                    return;
                }
                else
                {
                    crystal = find_resource("黄色水晶");
                    return;
                }
            }
            if (crystal.name == "黄色水晶")
            {
                crystal = find_resource("洋红色水晶");
                return;
            }
            if (crystal.name == "洋红色水晶")
            {
                crystal = find_resource("青色水晶");
                return;
            }
            if (crystal.name == "青色水晶")
            {
                crystal = find_resource("白色水晶");
                return;
            }
            if (crystal.name == "白色水晶")
            {
                crystal = find_resource("红色水晶");
                return;
            }
        }
        #endregion level 水晶树

        #region level 合成树

        #region ball_rate_min
        public List<resource> g1_ball_rate_min = new List<resource>();
        public List<resource> g1_prepare_ball_rate_min()
        {
            int types = game_grids["m3"].type_max;
            resource[] r = new resource[types];
            g1_layer main = g1_layers["合成资源"];
            for(int i = 0; i < types; i++)
            {
                r[i] = main.resources[types + i];
            }
            return res_min(r);
        }
        #endregion ball_rate_min
        #endregion level 合成树
    }
}
