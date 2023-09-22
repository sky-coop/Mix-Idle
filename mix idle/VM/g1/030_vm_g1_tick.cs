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
        public void g1_tick(double2 t)
        {
            double2 time_base = t * gamespeed() * g1_levels["世界"].speed;
            foreach (KeyValuePair<string, g1_level> l in g1_levels)
            {
                if (l.Value.started)
                {
                    l.Value.in_game_time += time_base;
                    l.Value.active_time += t;
                }
            }
            #region 世界
            #region layer 世界树 森林
            g1_resource world_point = (g1_resource)find_resource("世界点数");

            g1_resource c_level = (g1_resource)find_resource("文明水平");
            g1_resource rese_point = (g1_resource)find_resource("研究点数");
            g1_resource life = (g1_resource)find_resource("生命力");

            yggdrasill.produce_ex = 2 + g1_cal_0_wp_effect();

            apply_mul(yggdrasill.exp_gain_multipliers, "生命力",
                g1_cal_life_effect());
            apply_mul(life.multipliers, "自然力量",
                g1_cal_natural_effect());
            apply_mul(life.multipliers, "林业",
                g1_cal_research_wood_life_gain());

            yggdrasill.gain_exp(yggdrasill.exp_gain * yggdrasill.get_exp_mul() * 
                global_xp_boost() * time_base, 100);

            world_point.add_value(yggdrasill.produce() * time_base);

            #region 世界树层更新
            if ((g1_ups["世界_世界树_里程碑_2"] as g1_milestone).completed &&
                g1_levels["世界树"].started)
            {
                resource y_lv = find_resource("世界树等级");
                double2 stage_temp = double2.Log10(y_lv.best);
                if(stage_temp > 100)
                {
                    stage_temp = 100;
                }
                int stage = (int)stage_temp.d;
                g1_level level = g1_levels["世界树"];
                int old = level.stage;
                yggdrasill_change_stage(stage - old);
            }
            #endregion 世界树层更新

            #endregion layer 世界树 森林
            #region layer 自然树
            // +(^ 0.3)
            if (g1_ups["世界_生态循环"].level >= 1)
            {
                life.store_to_pool("自然树", 0);
                life.pools["自然树"] += double2.Pow(life.see_pool("自然树"),
                    g1_cal_npower_eco_effect()) * time_base;
            }
            if (g1_ups["世界_自然循环"].level >= 1)
            {
                apply_mul(find_resource("自然点数").multipliers, 
                    "自然循环", g1_cal_npower_npoint_effect());
            }
            #endregion layer 自然树
            #region layer 水晶树
            apply_mul(find_resource("生命效果").addition,
                "水晶球", g1_cal_crystalball_effect());
            #endregion layer 水晶树
            #region layer 文明
            apply_mul(c_level.adders, "基本", 1);

            apply_mul(c_level.adders, "食物学",
                g1_cal_research_food_cp_gain());

            apply_mul(you.other_attack_factor, "战斗学",
                g1_cal_research_battle_attack_gain());
            apply_mul(c_level.adders, "战斗学",
                g1_cal_research_battle_cp_gain());

            apply_mul(c_level.adders, "语言与文字",
                g1_cal_research_language_cp_gain());

            foreach (KeyValuePair<string, enchant> kp in enchants)
            {
                enchant ec = kp.Value;
                if (ec.is_potion)
                {
                    apply_mul(ec.speed_ups, "水源工程",
                        g1_cal_research_water_potion_gain());
                }
            }
            apply_mul(c_level.adders, "水源工程",
                g1_cal_research_water_cp_gain());

            apply_mul(find_resource("木头方块").multipliers, "林业",
                g1_cal_research_wood_wood_gain());
            apply_mul(c_level.adders, "林业",
                g1_cal_research_wood_cp_gain());

            if (g1_ups["世界_经验总结"].level >= 1)
            {
                double2 time = time_base;
                if (time_base > 1)
                {
                    time = double2.Pow(time, 6 / (6 + double2.Log10(time + 9)));
                }
                c_level.add_value(g1_cal_exp_summary_gain() * c_level.adders.get_add() *
                    time);
            }

            if (g1_ups["世界_房屋"].level >= 1)
            {
                apply_mul(c_level.multipliers, "房屋",
                    g1_cal_life_cp_syn());
                apply_mul(life.multipliers, "房屋",
                    g1_cal_life_cp_syn());
            }
            if (g1_ups["世界_桥"].level >= 1)
            {
                apply_mul(c_level.multipliers, "桥",
                    g1_cal_ylv_to_cp_syn());
                apply_mul(yggdrasill.exp_gain_multipliers, "桥",
                    g1_cal_cp_to_yxp_syn());
            }

            //research tick
            foreach (KeyValuePair<string, g1_level> kp in g1_levels)
            {
                g1_level l = kp.Value;
                if (l.started)
                {
                    foreach(g1_upgrade u in l.GetAllUpgrades())
                    {
                        if (u is g1_research)
                        {
                            g1_research r = u as g1_research;
                            r.tick(time_base);
                        }
                    }
                }
            }

            //研究点数+
            if (c_level.get_value() > rese_point.all_with_pool())
            {
                rese_point.add_value(c_level.get_value() - rese_point.all_with_pool());
            }
            #endregion layer 文明
            #endregion 世界

            #region 世界树
            if (g1_levels["世界树"].started)
            {
                double2 tick = g1_levels["世界树"].speed * time_base;
                resource[] p = new resource[101];
                p[0] = find_resource("0层点数");
                for (int i = 1; i <= 100; i++)
                {
                    p[i] = find_resource(i + "层点数");
                    string str = "打开" + i + "层";
                    g1_upgrade up = g1_ups["世界树_" + str];
                    int gain = 1;
                    #region 1
                    if (i == 1)
                    {
                        if (g1_ups["世界树_闪亮水晶"].level >= 1)
                        {
                            gain *= 10;
                            find_resource("水晶质量").add_value(g1_cal_1_crystal_effect()
                                * tick);
                        }
                    }
                    #endregion 1
                    if (up.level >= 1)
                    {
                        p[i].add_value(gain * tick);
                    }
                }

                apply_mul(p[0].multipliers, p[1].name, g1_cal_1_0_effect_mul());
                p[0].add_value(g1_cal_1_0_effect_add() * tick
                    / g1_cal_1_0_effect_mul());
                if (g1_ups["世界树_世界的基础"].level >= 1)
                {
                    double2 x = 0.01;
                    if (g1_ups["世界树_迷宫元素沉积"].level >= 1)
                    {
                        x *= g1_cal_maze_0_effect();
                    }
                    p[0].add_value(x * tick);
                }
            }
            #endregion 世界树

            #region 自然树
            g1_resource natural_point = (g1_resource)find_resource("自然点数");
            if (g1_levels["自然树"].started)
            {
                double2 tick = g1_levels["自然树"].speed * time_base;

                if (g1_ups["自然树_水源"].level >= 1)
                {
                    double2 mul = 1;
                    if (g1_ups["自然树_水分保持"].level >= 1)
                    {
                        mul *= g1_cal_water_np_mul();
                    }
                    if (g1_ups["自然树_汇聚"].level >= 1)
                    {
                        mul *= g1_cal_water_water_mul();
                    }
                    if (g1_ups["自然树_营养液"].level >= 1)
                    {
                        mul *= g1_cal_water_from_tree_mul();
                    }
                    if (g1_ups["自然树_非常水的升级"].level >= 1)
                    {
                        mul *= 2;
                    }
                    if (g1_ups["自然树_吸水"].level >= 1)
                    {
                        mul *= 3; 
                    }
                    mul *= g1_cal_research_water_water_gain();
                    find_resource("水").add_value(1 * tick * mul);

                    g1_cal_water_nu_gain = double2.Pow(mul, 0.5);
                    if (g1_ups["自然树_非常水的升级"].level >= 1)
                    {
                        find_resource("营养").add_value(g1_cal_water_nu_gain * tick);
                    }
                }

                if (g1_ups["自然树_矿物质"].level >= 1)
                {
                    double2 mul = 1;
                    if (g1_ups["自然树_光照"].level >= 1)
                    {
                        mul *= g1_cal_nu_light_effect();
                    }
                    if (g1_ups["自然树_肥料"].level >= 1)
                    {
                        mul *= g1_cal_nu_fat_effect();
                    }
                    if (g1_ups["自然树_气流"].level >= 1)
                    {
                        mul *= g1_cal_nu_air_effect();
                    }
                    mul *= g1_cal_s2_nu_effect();
                    find_resource("营养").add_value(0.1 * tick * mul);
                }

                resource r_seed = find_resource("种子");
                apply_mul(seed.exp_gain_multipliers, "生命力",
                    g1_cal_life_effect(true, "自然树"));
                apply_mul(seed.exp_gain_adders, "水",
                    g1_cal_water_effect());
                apply_mul(seed.exp_gain_multipliers, "营养",
                    g1_cal_nu_tree_effect());
                if (r_seed.get_value() > 0)
                {
                    seed.gain_exp(seed.get_exp_gain() * tick
                        * global_xp_boost(), double2.max);
                }
                if ((g1_ups["自然树_种子_里程碑_2"] as g1_milestone).completed)
                {
                    apply_mul(seed.exp_base_mul, "种子里程碑2",
                        double2.Max(1, double2.Pow(find_resource("种子成长度").get_value(), 0.5)));
                }
                natural_point.add_value(r_seed.get_value() * seed.produce() * tick);

                resource r_sapling = find_resource("树苗");
                apply_mul(sapling.exp_gain_multipliers, "生命力",
                    g1_cal_life_effect(true, "自然树"));
                apply_mul(sapling.exp_gain_adders, "水",
                    g1_cal_water_effect());
                apply_mul(sapling.exp_gain_multipliers, "营养",
                    g1_cal_nu_tree_effect());
                if (r_sapling.get_value() > 0)
                {
                    sapling.gain_exp(sapling.get_exp_gain() * tick * 
                        global_xp_boost(), double2.max);
                }
                natural_point.add_value(r_sapling.get_value() * sapling.produce() * tick);

                resource r_smalltree = find_resource("小树");
                apply_mul(smalltree.exp_gain_multipliers, "生命力",
                    g1_cal_life_effect(true, "自然树"));
                apply_mul(smalltree.exp_gain_adders, "水",
                    g1_cal_water_effect());
                apply_mul(smalltree.exp_gain_multipliers, "营养",
                    g1_cal_nu_tree_effect());
                if (r_smalltree.get_value() > 0)
                {
                    smalltree.gain_exp(smalltree.get_exp_gain() * tick *
                        global_xp_boost(), double2.max);
                }
                natural_point.add_value(r_smalltree.get_value() * smalltree.produce() * tick);
            }

            #endregion 自然树

            #region 水晶树
            if (g1_levels["水晶树"].started)
            {
                double2 tick = g1_levels["水晶树"].speed * time_base;

                bool easy = g1_levels["水晶树"].difficulty == g1_level.type.easy;
                bool Y_eff = false;
                bool M_eff = false;
                bool C_eff = false;
                bool W_eff = false;

                if (easy && g1_ups["水晶树_海浪宝石"].accing)
                {
                    g1_upgrade upgrade = g1_ups["水晶树_海浪宝石"];
                    if(upgrade.acc_time2 == 0)
                    {
                        upgrade.acc_time -= t * 0.02;
                        if (upgrade.acc_time < 0)
                        {
                            upgrade.acc_time = 0;
                            upgrade.acc_time2 = 1;
                        }
                    }
                    else
                    {
                        upgrade.acc_time += t * 0.02;
                        if (upgrade.acc_time > 1)
                        {
                            upgrade.acc_time = 1;
                            upgrade.acc_time2 = 0;
                        }
                    }
                    tick *= g1_cal_F_Bsea_effect();
                }


                resource r = null;

                double2 rgb_mul = 1;
                double2 all_mul = 1;
                rgb_mul *= g1_cal_G_art1_effect(false);
                if (g1_ups["世界_献祭水晶"].level >= 1)
                {
                    rgb_mul *= g1_cal_sacrifice_effect(3);
                }
                if (easy)
                {
                    rgb_mul *= g1_cal_F_Rpickaxe_effect();
                    rgb_mul *= g1_cal_F_Gscanner_effect();
                    rgb_mul *= g1_cal_F_Bspin_effect();
                }
                else
                {
                    Y_eff = g1_ups["水晶树_黄色水晶释放"].level >= 1 &&
                        g1_ups["水晶树_黄色水晶控制"].valid;
                    M_eff = g1_ups["水晶树_洋红色水晶释放"].level >= 1;
                    C_eff = g1_ups["水晶树_青色水晶释放"].level >= 1;
                    if (C_eff)
                    {
                        all_mul *= g1_cal_C_effect();
                    }
                    W_eff = g1_ups["水晶树_白色水晶释放"].level >= 1;
                    g1_cal_W_temp = g1_cal_W_effect();
                    if (W_eff)
                    {
                        all_mul *= g1_cal_W_effect();
                    }
                }

                #region 提前计算
                if (s_ticker("g1_crystal", 1))
                {
                    g1_crystal_rgb_min = g1_prepare_crystal_rgb_min();
                    g1_crystal_rgb_max = g1_prepare_crystal_rgb_max();
                    g1_crystal_curr_min = g1_crystal_rgb_min;
                    g1_crystal_curr_max = g1_crystal_rgb_max;

                    g1_cal_A_2_art1_condition =
                        double2.Log10(double2.Max(2, g1_crystal_rgb_max[0].get_value())) /
                        double2.Log10(double2.Max(2, g1_crystal_rgb_min[0].get_value()));
                    g1_cal_R_cconvert_temp = g1_cal_R_cconvert_effect();
                    if (!easy)
                    {
                        g1_crystal_all_min = g1_prepare_crystal_all_min();
                        g1_crystal_all_max = g1_prepare_crystal_all_max();
                        g1_crystal_curr_min = g1_crystal_all_min;
                        g1_crystal_curr_max = g1_crystal_all_max;
                    }
                }
                #endregion 提前计算

                double2 mul = 1;
                rgb_mul *= all_mul;
                #region Rp
                mul = rgb_mul;
                {
                    r = find_resource("红色水晶生成力");
                    mul *= g1_cal_lifec_red_effect();
                    if (g1_crystal_curr_min[0].name == "红色水晶")
                    {
                        mul *= g1_cal_A_2_art1_effect1(false);
                    }
                    if (g1_cal_A_2_art2_select.name == "红色水晶")
                    {
                        mul *= g1_cal_A_2_art2_effect(false);
                    }

                    if (g1_ups["水晶树_平静变化"].level >= 1 &&
                        g1_crystal_rgb_min[0].name == "红色水晶")
                    {
                        mul *= g1_cal_R_balance_1_effect();
                    }
                    if (g1_ups["水晶树_循环转化"].level >= 1 &&
                        g1_cal_R_cconvert_temp.Item1 == false)
                    {
                        mul *= g1_cal_R_cconvert_temp.Item2;
                    }
                    if (Y_eff && g1_crystal_all_max[0].name == "红色水晶")
                    {
                        mul *= g1_cal_Y_effect();
                    }
                    r.set_value(mul);
                }
                #endregion Rp
                #region Gp
                mul = rgb_mul;
                {
                    r = find_resource("绿色水晶生成力");
                    mul *= g1_cal_life_effect(true, "水晶树");
                    if (g1_crystal_curr_min[0].name == "绿色水晶")
                    {
                        mul *= g1_cal_A_2_art1_effect1(false);
                    }
                    if (g1_cal_A_2_art2_select.name == "绿色水晶")
                    {
                        mul *= g1_cal_A_2_art2_effect(false);
                    }

                    if (g1_ups["水晶树_平静变化"].level >= 1 && 
                        g1_crystal_rgb_min[0].name != "红色水晶")
                    {
                        mul *= g1_cal_R_balance_2_effect();
                    }
                    if (g1_ups["水晶树_吸收与利用"].level >= 1)
                    {
                        mul *= g1_cal_G_absorb_effect();
                    }
                    if (Y_eff && g1_crystal_all_max[0].name == "绿色水晶")
                    {
                        mul *= g1_cal_Y_effect();
                    }
                    r.set_value(mul);
                }
                #endregion Gp
                #region Bp
                mul = rgb_mul;
                {
                    r = find_resource("蓝色水晶生成力");
                    if (g1_crystal_curr_min[0].name == "蓝色水晶")
                    {
                        mul *= g1_cal_A_2_art1_effect1(false);
                    }
                    if (g1_cal_A_2_art2_select.name == "蓝色水晶")
                    {
                        mul *= g1_cal_A_2_art2_effect(false);
                    }

                    if (g1_ups["水晶树_平静变化"].level >= 1 &&
                        g1_crystal_rgb_min[0].name != "红色水晶")
                    {
                        mul *= g1_cal_R_balance_2_effect();
                    }
                    if (g1_ups["水晶树_环境加成"].level >= 1)
                    {
                        mul *= g1_cal_B_duplicate_effect();
                    }
                    if (g1_ups["水晶树_稳定结晶"].level >= 1)
                    {
                        mul *= 3;
                    }
                    if (Y_eff && g1_crystal_all_max[0].name == "蓝色水晶")
                    {
                        mul *= g1_cal_Y_effect();
                    }
                    r.set_value(mul);
                }
                #endregion Bp


                if (!easy)
                {
                    string[] mix_crystals = new string[4] {
                    "黄色水晶", "洋红色水晶", "青色水晶", "白色水晶",
                    };
                    double2 y_effect = g1_cal_Y_effect();
                    foreach (string s in mix_crystals)
                    {
                        resource mix = find_resource(s);
                        if (Y_eff && g1_crystal_all_max[0].name == s)
                        {
                            apply_mul(mix.multipliers, "黄色水晶效果",
                                y_effect);
                        }
                        else
                        {
                            if (g1_ups["水晶树_黄金比例"].level >= 1)
                            {
                                apply_mul(mix.multipliers, "黄色水晶效果",
                                    double2.Pow(y_effect, 0.618));
                            }
                            else
                            {
                                apply_mul(mix.multipliers, "黄色水晶效果",
                                    1);
                            }
                        }

                        if (g1_crystal_curr_min[0].name == s)
                        {
                            apply_mul(mix.multipliers, "透亮圆盘效果",
                                g1_cal_A_2_art1_effect1(false));
                        }
                        else
                        {
                            apply_mul(mix.multipliers, "透亮圆盘效果",
                                1);
                        }

                        if (g1_cal_A_2_art2_select.name == s)
                        {
                            apply_mul(mix.multipliers, "超大水晶效果",
                                g1_cal_A_2_art2_effect(false));
                        }
                        else
                        {
                            apply_mul(mix.multipliers, "超大水晶效果",
                                1);
                        }

                        if (C_eff)
                        {
                            apply_mul(mix.multipliers, "青色水晶效果",
                                all_mul);
                        }
                        else
                        {
                            apply_mul(mix.multipliers, "青色水晶效果",
                                all_mul);
                        }

                        if (g1_ups["世界_献祭水晶"].level >= 1)
                        {
                            if (s == "白色水晶")
                            {
                                apply_mul(mix.multipliers, "献祭水晶",
                                    g1_cal_sacrifice_effect(6));
                            }
                            else
                            {
                                apply_mul(mix.multipliers, "献祭水晶",
                                    g1_cal_sacrifice_effect(9));
                            }
                        }
                    }
                }


                #region R
                mul = 1;
                {
                    r = find_resource("红色水晶");
                    mul *= find_resource("红色水晶生成力").get_value();
                    r.add_value(mul * tick);
                }
                #endregion R
                #region G
                mul = 1;
                {
                    r = find_resource("绿色水晶");
                    mul *= find_resource("绿色水晶生成力").get_value();
                    r.add_value(mul * tick);
                }
                #endregion G
                #region B
                mul = 1;
                {
                    r = find_resource("蓝色水晶");
                    mul *= find_resource("蓝色水晶生成力").get_value();
                    r.add_value(mul * tick);
                }
                #endregion B


                #region F
                if (easy)
                {
                    if (g1_ups["水晶树_红水晶镐"].accing)
                    {
                        g1_ups["水晶树_红水晶镐"].acc_time += tick;
                    }
                    if (g1_ups["水晶树_红色斧头"].accing)
                    {
                        g1_ups["水晶树_红色斧头"].acc_time += tick;
                    }
                }
                #endregion F

                #region Y
                #endregion Y
                #region M
                #endregion M
                #region C
                #endregion C
                #region W
                #endregion W
                #region 水晶块
                r = find_resource("水晶块");
                g1_crystal_mul = 1;
                g1_crystal_mul *= g1_cal_1_crystal_crystalpiece_effect() *
                        g1_cal_B_art1_effect(false);

                if (g1_ups["水晶树_平淡升级"].level >= 1)
                {
                    g1_crystal_mul *= 2;
                }
                g1_crystal_mul /= g1_cal_A_2_art1_effect2(false);

                if (g1_ups["水晶树_循环转化"].level >= 1 &&
                    g1_cal_R_cconvert_temp.Item1 == true)
                {
                    g1_crystal_mul *= g1_cal_R_cconvert_temp.Item2;
                }
                if (g1_ups["水晶树_生命混合"].level >= 1)
                {
                    g1_crystal_mul *= g1_cal_G_lifemix_effect();
                }
                if (easy) //简单模式
                {
                    art_mul = 1;
                    art_mul *= g1_cal_F_Raxe_effect();
                    g1_crystal_mul *= g1_cal_F_Grecycler_effect();
                }
                else
                {
                    if (g1_ups["水晶树_黄金比例"].level >= 1)
                    {
                        resource res0 = find_resource("黄色水晶");
                        if (res0.get_value() < 1e15)
                        {
                            res0.set_value(1e15);
                        }
                    }
                    if (g1_ups["水晶树_记忆晶石"].level >= 1)
                    {
                        resource res0 = find_resource("洋红色水晶");
                        if (res0.get_value() < 1e15)
                        {
                            res0.set_value(1e15);
                        }
                    }
                    if (g1_ups["水晶树_超级转换"].level >= 1)
                    {
                        resource res0 = find_resource("青色水晶");
                        if (res0.get_value() < 1e15)
                        {
                            res0.set_value(1e15);
                        }
                    }
                }
                if (M_eff)
                {
                    double2 m_effect = g1_cal_M_effect();
                    if (g1_ups["水晶树_记忆晶石"].level >= 1)
                    {
                        m_effect = double2.Pow(m_effect, 1.8);
                    }
                    g1_crystal_mul *= m_effect;
                }
                if (C_eff)
                {
                    double2 c_effect2 = g1_cal_C_effect();
                    if(g1_ups["水晶树_超级转换"].level >= 1)
                    {
                        c_effect2 = double2.Pow(c_effect2, 0.5);
                    }
                    g1_crystal_mul /= c_effect2;
                }

                if (easy)
                {
                    r.add_value(g1_cal_crystal_rgb_log_avg() * g1_crystal_mul * tick);
                }
                else
                {
                    r.add_value(g1_cal_crystal_all_log_avg() * g1_crystal_mul * tick);
                }
                #endregion 水晶块
            }

            #endregion 水晶树

            #region 合成树
            if (g1_levels["合成树"].started)
            {
                double2 tick = g1_levels["合成树"].speed * time_base;

                int ball_type_count = 4 + (int)g1_levels["合成树"].difficulty;

                #region 提前计算
                game_grid m3 = game_grids["m3"];
                m3.type_max = ball_type_count;

                g1_ball_rate_min = g1_prepare_ball_rate_min();


                g1_layer main = g1_layers["合成资源"];
                double2 sum = 0;
                for(int i = 0; i < ball_type_count; i++)
                {
                    sum += main.resources[ball_type_count + i].get_value();
                }
                double2[] ratios = new double2[ball_type_count];
                for (int i = 0; i < ball_type_count; i++)
                {
                    ratios[i] = main.resources[ball_type_count + i].get_value() / sum;
                }
                #endregion 提前计算

                #region 合成资源
                double2 start = 1;
                resource r = null;

                #region 分数球
                start = 1;
                {
                    r = find_resource("分数球生成比率");
                    r.set_value(start);
                }
                #endregion 分数球
                #region 回归球
                start = 1;
                {
                    r = find_resource("回归球生成比率");
                    for(int i = 0; i < ball_type_count; i++)
                    {
                        start *= double2.Pow(double2.Max(1, main.resources[i].get_value()), 
                            0.4 / ball_type_count);
                    }
                    start /= double2.Pow(double2.Max(1, 
                        find_resource("回归球").get_value()), 0.2);
                    r.set_value(start);
                }
                #endregion 回归球
                #region 增量球
                start = 1;
                {
                    r = find_resource("增量球生成比率");
                    start *= double2.Pow(double2.Max(1,
                        find_resource("增量球").get_value()), 0.2);
                    r.set_value(start);
                }
                #endregion 增量球
                #region 生命球
                start = 1;
                {
                    r = find_resource("生命球生成比率");
                    start *= double2.Log10(g1_cal_life_effect(true, "合成树") + 9);
                    r.set_value(start);
                }
                #endregion 生命球

                #endregion 合成资源
                #region M3
                resource m3_time = g1_res["M3活动时间"];
                resource m3_time_max = g1_res["M3活动时间上限"];

                m3_time.add_value(tick);
                int auto_max = 1;
                int auto = 0;
                int produce_max = 1;
                int produce = 0;
                while (m3_time.get_value() > m3_time_max.get_value() &&
                       auto < auto_max &&
                       produce < produce_max)
                {
                    m3_time.add_value(-m3_time_max.get_value());

                    m3.count = 0;
                    foreach (game_grid_element e in m3.elems)
                    {
                        if (e.type != 0)
                        {
                            m3.count++;
                        }
                    }

                    if (m3.count == m3.elems.Length && auto < auto_max)// 自动交换
                    {
                        auto++;
                        continue; //需要重新计算count
                    }
                    Random a = new Random();
                    while (m3.count < m3.elems.Length && produce < produce_max)
                    {
                        m3.count = 0;
                        foreach (game_grid_element e in m3.elems)
                        {
                            if (e.type != 0)
                            {
                                m3.count++;
                            }
                        }

                        int temp = m3.elems.Length - m3.count;
                        foreach (game_grid_element e in m3.elems)
                        {
                            if (e.type == 0)
                            {
                                if (a.NextDouble() <= 1.0 / temp)
                                {
                                    game_grid_element cas = e;
                                    while (cas.r + 1 < m3.row())
                                    {
                                        if (m3.elems[cas.r + 1, cas.c].type == 0)
                                        {
                                            cas = m3.elems[cas.r + 1, cas.c];
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    if (cas.type_delay != 0)
                                    {
                                        temp--;
                                    }
                                    m3.removing.Remove(cas);
                                    cas.progress = 0;
                                    cas.special = 0;

                                    double ran = a.NextDouble();
                                    double2 acc = 0;
                                    for (int i = 0; i < ball_type_count; i++)
                                    {
                                        if (acc <= ran && acc + ratios[i] > ran)
                                        {
                                            cas.type = i + 1;
                                            break;
                                        }
                                        acc += ratios[i];
                                    }

                                    m3.growing.Add(cas);
                                    produce++;
                                    break;
                                }
                                temp--;
                            }
                        }
                    }
                }

                List<game_grid_element> del_temp = new List<game_grid_element>();
                int[] ball_get = new int[ball_type_count];

                bool first = true;
                Point first_loc = new Point();

                foreach (game_grid_element e in m3.removing)
                {
                    e.progress = e.progress - time_tick_actually.d * 8;
                    if (e.progress <= 0 && e.type != 0)
                    {
                        if (first)
                        {
                            first = false;
                            first_loc = new Point(e.r, e.c);
                        }
                        ball_get[e.type - 1]++;
                        e.type = e.type_delay;
                        e.special = e.special_delay;
                        e.progress = e.progress_delay;
                        del_temp.Add(e);
                    }
                }
                foreach (game_grid_element e in del_temp)
                {
                    m3.removing.Remove(e);
                }

                //TODO float message
                string text = "";
                for (int i = 0; i < ball_type_count; i++)
                {
                    if (ball_get[i] == 0)
                    {
                        continue;
                    }
                    double2 gain = ball_get[i] * g1_ball_rate_min[0].get_value();
                    text += "+" + number_format(gain) + 
                        " " + main.resources[i].name;

                    main.resources[i].add_value(gain);

                    text += "\n";
                }
                text = text.Trim('\n');
                temp_texts["m3_produce"] = new Tuple<string, Point>(text, first_loc);

                List<game_grid_element> check_temp = new List<game_grid_element>();
                foreach (game_grid_element e in m3.growing)
                {
                    e.progress = e.progress + time_tick_actually.d * 4;
                    if (e.progress >= 1)
                    {
                        e.progress = 1;
                        check_temp.Add(e);
                        del_temp.Add(e);
                    }
                }
                foreach (game_grid_element e in check_temp)
                {
                    m3_check(e.r, e.c);
                }
                foreach (game_grid_element e in del_temp)
                {
                    m3.growing.Remove(e);
                }

                #endregion M3
            }
            #endregion 合成树
            find_resource("娱乐币").add_value(g1_cal_wp_effect() * time_base);

            g1_resource_syn();

            if (vm.search_app("世界树").showing)
            {
                if (g1_current_level != null)
                {
                    g1_show();
                }
                g1_upgrade_check();
            }
        }
    }
}
