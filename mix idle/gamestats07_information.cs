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
        //方块信息
        #region
        private void 方块_介绍(block_producter bp)
        {
            resource r = find_resource(bp.name);
            string total = "";
            string[] strs = new string[12];
            for (int i = 0; i < 12; i++)
            {
                strs[i] = "";
            }


            if (bp.name == "白色方块")
            {
                strs[0] = "···生产你的第一种资源：白色方块，有许许多多的用途。\n";
            }

            if (bp.production_exponent > 1)
            {
                strs[3] = "···生产速度随着时间增加而增加，直到达到最大产量为止，\n";
            }
            else if (bp.production_exponent == 1)
            {
                strs[3] = "···其生产速度恒定不变，当达到最大产量时，\n";
            }
            else if (bp.production_exponent < 1)
            {
                strs[3] = "···其生产速度随着时间增加而减少，直到达到最大产量为止，\n";
            }

            strs[4] = "此时会自动收取。您也可以提前手动收取。\n";
            strs[6] = "···时间与产量的关系为: p = M × (t / T) ^ " + number_format(bp.production_exponent) + "\n";
            strs[7] = "（p:当前产量，M:最大产量，t:当前时间，T:最大时间）\n";
            strs[8] = "···目前的值为: " + number_format(bp.current_value * r.get_mul() * time_power()) + " = "
                                        + number_format(bp.max_value * r.get_mul() * time_power()) + " × ("
                                        + number_format(bp.current_time) + " / "
                                        + number_format(bp.max_time) + ") ^ "
                                        + number_format(bp.production_exponent) + "\n";
            strs[9] = "···目前的生产速度是 " + number_format(gamespeed() * bp.max_value * r.get_mul() * time_power() 
                / double2.Pow(bp.max_time, bp.production_exponent) * bp.production_exponent 
                * double2.Pow(bp.current_time, bp.production_exponent - 1)) + "/s。\n"; //求导
            if (time_a_tick_game > bp.max_time)
            {
                strs[9] = "···目前的生产速度是 " + number_format(gamespeed() * bp.max_value / bp.max_time) + "/s。\n"; //求导
            } 
            strs[10] = "···升级以调整最大时间及提升最大产量。";

            foreach (string s in strs)
            {
                total += s;
            }

            m.详细信息框.Text = total;
        }

        private void 方块_收集信息(block_producter bp)
        {
            resource r = find_resource(bp.name);

            string total = "";
            string[] strs = new string[8];
            for (int i = 0; i < 8; i++)
            {
                strs[i] = "";
            }

            if (bp.max_time > 30)
            {
                strs[0] = "···手动收集" + bp.name + "，而不需要等很长时间。\n";
            }
            else
            {
                strs[0] = "···手动收集" + bp.name + "。\n";
            }

            if (bp.production_exponent > 1)
            {
                strs[2] = "···要注意，你收集方块越早，平均收益就越低。\n";
            }
            else if (bp.production_exponent < 1)
            {
                strs[2] = "···要注意，你收集方块越晚，平均收益就越低。\n";
            }

            strs[3] = "···当进度条满时，游戏会自动收取方块。\n";
            strs[4] = "···现在手动收取可以获得：" + number_format(bp.current_value * r.get_mul()) + " " + r.name + "。\n";
            strs[5] = "···这将导致你拥有 " + number_format(bp.current_value * r.get_mul() + r.get_value()) + " " + r.name + "。";

            foreach (string s in strs)
            {
                total += s;
            }

            m.详细信息框.Text = total;
        }

        private void 方块_升级信息(block_producter bp)
        {
            resource r = find_resource(bp.cost_res_type);

            double2 can_use = get_can_use(r.get_value());
            double2 need = bp.cost;
            int buy_level = 1;
            if (buy_int)
            {
                buy_level = buy_number;
                need = need * (double2.Pow(bp.cost_exponent, buy_number) - 1) / (bp.cost_exponent - 1);
            }
            else
            {
                double2 next_cost = bp.cost * bp.cost_exponent;
                while (can_use > need + next_cost)
                {
                    buy_level++;
                    need += next_cost;
                    next_cost *= bp.cost_exponent;
                }
            }

            string s1 = "", s2 = "", s3 = "";
            if (can_use - need >= 0)
            {
                s1 = "购买后你会剩下 " + number_format(r.get_value() - need) + " " + bp.cost_res_type + "。";
            }
            else
            {
                s1 = "根据现在的购买设置，你还买不起。";
            }

            if (bp.level_up_time_factor != 1)
            {
                s2 += "最大耗时×" + number_format(bp.level_up_time_factor) + "，";
            }
            if (bp.level_up_production_factor != 1)
            {
                s2 += "最大产量×" + number_format(bp.level_up_production_factor) + "，";
            }

            string ss = "";
            if (bp.milestone != 0)
            {
                s3 += "\n";
                bool comma = false;
                if (bp.milestone_effect_time_factor != 1)
                {
                    ss += "最大耗时×" + number_format(bp.milestone_effect_time_factor);
                    comma = true;
                }
                if (bp.milestone_effect_production_factor != 1)
                {
                    if (comma)
                    {
                        ss += "，";
                    }
                    ss += "最大产量×" + number_format(bp.milestone_effect_production_factor);
                }
                s3 += "···每" + number_format(bp.milestone) + "个等级会使它的" + ss + "。";
            }

            ss = "";
            if (bp.milestone2 != 0)
            {
                s3 += "\n";
                bool comma = false;
                if (bp.milestone_effect_time_factor2 != 1)
                {
                    ss += "最大耗时×" + number_format(bp.milestone_effect_time_factor2);
                    comma = true;
                }
                if (bp.milestone_effect_production_factor2 != 1)
                {
                    if (comma)
                    {
                        ss += "，";
                    }
                    ss += "最大产量×" + number_format(bp.milestone_effect_production_factor2);
                }
                s3 += "···每" + number_format(bp.milestone2) + "个等级会使它的" + ss + "。";
            }

            m.详细信息框.Text = "···使用 " + number_format(need) + " " + bp.cost_res_type + "提升你的" + bp.name + "生产器 " + buy_level + " 级。\n"
                           + "···" + s1 + "\n"
                           + "···每升1级，" + s2 + "下一级价格×" + number_format(bp.cost_exponent) + "。"
                           + s3;
        }
        #endregion

        //制造信息
        #region

        #endregion

        //转生信息
        #region
        private void 转生_信息()
        {
            resource pp = res_table["转生"]["转生点数"];
            resource wb = res_table["方块"]["白色方块"];

            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            if (prestige_ups["对数增益"].level == 1)
            {
                s1 = " + log2(wb + 1)";
                s2 = " + log2(" + number_format(wb.get_value()) + " + 1)";
            }
            if (prestige_ups["对数增益"].level == 2)
            {
                s1 = " + log2(wb + 1) ^ 2";
                s2 = " + log2(" + number_format(wb.get_value()) + " + 1) ^ 2";
            }
            if (prestige_ups["对数增益"].level == 3)
            {
                s1 = " + log2(wb + 1) ^ 3";
                s2 = " + log2(" + number_format(wb.get_value()) + " + 1) ^ 3";
            }
            if (prestige_ups["对数增益"].level == 4)
            {
                s1 = " + log2(wb + 1) ^ 4";
                s2 = " + log2(" + number_format(wb.get_value()) + " + 1) ^ 4";
            }

            if (pp.get_mul() != 1)
            {
                s3 = " （另有转生点数增益 ×" + number_format(pp.get_mul()) + "）";
                s4 = " × " + number_format(pp.get_mul());
            }

            m.详细信息框.Text = "···进行一次转生！\n"
                           + "···根据你的现有白色方块量获得转生点数。但重置所有其他面板的内容。\n"
                           + "···转生点数可以用于购买强大的升级。\n"
                           + "···转生点数公式为：pp = wb ^ (1 / 3)" + s1 + s3 + "\n"
                           + "（pp:转生点数，wb:白色方块数）\n"
                           + "···现在转生可以获得：" + number_format(pp_gain * pp.get_mul()) + " 转生点数。(" + number_format(wb.get_value()) + " ^ (1 / 3)" + s2 + ")" + s4 + "\n"
                           + "···这将导致你拥有 " + number_format(pp_gain * pp.get_mul() + pp.get_value()) + " 转生点数。";
        }

        private void 对数增益_信息()
        {
            resource pp = res_table["转生"]["转生点数"];
            resource wb = res_table["方块"]["白色方块"];
            prestige_upgrade p = prestige_ups["对数增益"];

            double2 pp_gain_extra = 0;
            double2 pp_gain_extra_next = 0;

            string not_max = "";
            string ss = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[(int)p.level]) + " " + p.cost_res_type;
            }

            string s0 = "";
            string s1 = "";
            if (p.level >= 0)
            {
                pp_gain_extra = 0;
                pp_gain_extra_next = (wb.get_value() + 1).Log10() / Math.Log10(2);
                s0 = "";
                s1 = "log2(wb + 1)";
            }
            if (p.level >= 1)
            {
                pp_gain_extra = pp_gain_extra_next;
                pp_gain_extra_next = double2.Pow((wb.get_value() + 1).Log10() / Math.Log10(2), 2);
                s0 = "log2(" + number_format(wb.get_value()) + " + 1)";
                s1 = "log2(wb + 1) ^ 2";
            }
            if (p.level >= 2)
            {
                pp_gain_extra = pp_gain_extra_next;
                pp_gain_extra_next = double2.Pow((wb.get_value() + 1).Log10() / Math.Log10(2), 3);
                s0 = "log2(" + number_format(wb.get_value()) + " + 1) ^ 2";
                s1 = "log2(wb + 1) ^ 3";
            }
            if (p.level >= 3)
            {
                pp_gain_extra = pp_gain_extra_next;
                pp_gain_extra_next = double2.Pow((wb.get_value() + 1).Log10() / Math.Log10(2), 4);
                s0 = "log2(" + number_format(wb.get_value()) + " + 1) ^ 3";
                s1 = "log2(wb + 1) ^ 4";
            }
            if (p.level >= 4)
            {
                pp_gain_extra = pp_gain_extra_next;
                s0 = "log2(" + number_format(wb.get_value()) + " + 1) ^ 4";
            }

            if (p.level != p.max_level)
            {
                ss = "\n···升级后，可以获得额外转生点数：" + s1 + "\n"
                   + "（wb:白色方块数）\n"
                   + "···现在购买可以使转生时获得：" + number_format(pp_gain_extra_next * pp.get_mul()) + "额外转生点数。\n"
                   + "···这将导致你在转生时获得" + number_format(pp_gain_extra_next * pp.get_mul() + pp_gain * pp.get_mul()) + "转生点数。";
            }

            m.详细信息框.Text = "···对数增益升级。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                           + "···使转生时获得更多转生点数。效果随白色方块的数量增加而增加，但效果衰减得很快。\n"
                           + "···此升级已为你提供" + s0 + "[+" + number_format(pp_gain_extra * pp.get_mul()) + "]转生时的转生点数。" + ss;
        }

        private void 生成器_信息()
        {
            prestige_upgrade p = prestige_ups["生成器"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[(int)p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，每秒自动生产 100 白色方块。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，每秒自动生产 500 白色方块 和 5000 白色粉末。\n";
                s2 = "···此升级目前的效果是：\n       每秒生产 100 白色方块。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，每秒自动生产 3000 白色方块，" + number_format(500e3) + " 白色粉末 和 30 魔力。\n";
                s2 = "···此升级目前的效果是：\n       每秒生产 500 白色方块。\n" +
                                                 "       每秒生产 5000 白色粉末。";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，每秒自动生产 25 白色粒子 和 3 绿色粒子。\n";
                s2 = "···此升级目前的效果是：\n       每秒生产 3000 白色方块。\n" +
                                                 "       每秒生产 " + number_format(500e3) + " 白色粉末。\n" +
                                                 "       每秒生产 30 魔力。";
            }
            else if (p.level == 4)
            {
                s1 = "···购买本级后，每秒自动生产 " + number_format(2e6) + " 白色方块，" + number_format(3e12) + " 白色粉末， 7000 魔力 和 " + number_format(10e6) + " 糖方块。\n";
                s2 = "···此升级目前的效果是：\n       每秒生产 3000 白色方块。\n" +
                                                 "       每秒生产 " + number_format(500e3) + " 白色粉末。\n" +
                                                 "       每秒生产 25 白色粒子。\n" +
                                                 "       每秒生产 3 绿色粒子。\n" +
                                                 "       每秒生产 30 魔力。";
            }
            else if (p.level == 5)
            {
                s1 = "···购买本级后，每秒自动生产 " + number_format(10e9) + " 白色方块，" + number_format(4e3) + " 白色粒子，" + number_format(400) + " 绿色粒子，\n" +
                                                        number_format(100) + " 红色粒子，" + number_format(30) + " 橙色粒子，" + number_format(12) + " 蓝色粒子，" +
                                                        number_format(6) + " 无色粒子，" + number_format(100e3) + " 魔力\n";
                s2 = "···此升级目前的效果是：\n       每秒生产 " + number_format(2e6) + " 白色方块。\n" +
                                                 "       每秒生产 " + number_format(10e6) + " 糖方块。\n" +
                                                 "       每秒生产 " + number_format(3e12) + " 白色粉末。\n" +
                                                 "       每秒生产 25 白色粒子。\n" +
                                                 "       每秒生产 3 绿色粒子。\n" +
                                                 "       每秒生产 7000 魔力。";
            }
            else if (p.level == 6)
            {
                s1 = "···购买本级后，每秒自动生产 " + number_format(3e12) + " 白色方块，" + number_format(500e12) + " 糖方块，" + number_format(15e6) + " 铜，" + number_format(5e6) + " 铁\n";
                s2 = "···此升级目前的效果是：\n       每秒生产 " + number_format(10e9) + " 白色方块。\n" +
                                                 "       每秒生产 " + number_format(10e6) + " 糖方块。\n" +
                                                 "       每秒生产 " + number_format(3e12) + " 白色粉末。\n" +
                                                 "       每秒生产 " + number_format(4e3) + " 白色粒子。\n" +
                                                 "       每秒生产 " + number_format(400) + " 绿色粒子。\n" +
                                                 "       每秒生产 " + number_format(100) + " 红色粒子。\n" +
                                                 "       每秒生产 " + number_format(30) + " 橙色粒子。\n" +
                                                 "       每秒生产 " + number_format(12) + " 蓝色粒子。\n" +
                                                 "       每秒生产 " + number_format(6) + " 无色粒子。\n" +
                                                 "       每秒生产 " + number_format(100e3) + " 魔力。";
            }
            else if (p.level == 7)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       每秒生产 " + number_format(3e12) + " 白色方块。\n" +
                                                 "       每秒生产 " + number_format(500e12) + " 糖方块。\n" +
                                                 "       每秒生产 " + number_format(3e12) + " 白色粉末。\n" +
                                                 "       每秒生产 " + number_format(4e3) + " 白色粒子。\n" +
                                                 "       每秒生产 " + number_format(400) + " 绿色粒子。\n" +
                                                 "       每秒生产 " + number_format(100) + " 红色粒子。\n" +
                                                 "       每秒生产 " + number_format(30) + " 橙色粒子。\n" +
                                                 "       每秒生产 " + number_format(12) + " 蓝色粒子。\n" +
                                                 "       每秒生产 " + number_format(6) + " 无色粒子。\n" +
                                                 "       每秒生产 " + number_format(100e3) + " 魔力。\n" +
                                                 "       每秒生产 " + number_format(15e6) + " 铜。\n" +
                                                 "       每秒生产 " + number_format(5e6) + " 铁。";
            }


            m.详细信息框.Text = "···需要“对数增益”等级 1 以解锁。\n"
                           + "···自动生产一些资源。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                           + s1 + s2;

        }

        private void 资源保留_信息()
        {
            prestige_upgrade p = prestige_ups["资源保留"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，转生后保留 ^ 0.7 的资源，且少于 200 的资源全部保留。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，转生后保留 ^ 0.75 的资源，且少于 1000 的资源全部保留。\n";
                s2 = "···此升级目前的效果是：\n       转生后保留 ^ 0.7 的资源。" +
                                                 "       少于 200 的资源全部保留。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，转生后保留 ^ 0.8 的资源，且少于 5000 的资源全部保留。\n";
                s2 = "···此升级目前的效果是：\n       转生后保留 ^ 0.75 的资源。\n" +
                                                 "       少于 1000 的资源全部保留。";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，转生后保留 ^ 0.825 的资源，且少于 " + number_format(30e3) + " 的资源全部保留。\n";
                s2 = "···此升级目前的效果是：\n       转生后保留 ^ 0.8 的资源。\n" +
                                                 "       少于 5000 的资源全部保留。";
            }
            else if (p.level == 4)
            {
                s1 = "···购买本级后，转生后保留 ^ 0.85 的资源，且少于 " + number_format(100e3) + " 的资源全部保留。\n";
                s2 = "···此升级目前的效果是：\n       转生后保留 ^ 0.825 的资源。\n" +
                                                 "       少于 " + number_format(30e3) + " 的资源全部保留。";
            }
            else if (p.level == 5)
            {
                s1 = "···购买本级后，能量也会加入保留行列！\n";
                s2 = "···此升级目前的效果是：\n       转生后保留 ^ 0.85 的资源。\n" +
                                                 "       少于 " + number_format(100e3) + " 的资源全部保留。";
            }
            else if (p.level == 6)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       转生后保留 ^ 0.85 的资源（包括能量）。\n" +
                                                 "       少于 " + number_format(100e3) + " 的资源全部保留。";
            }


            m.详细信息框.Text = "···需要“生成器”等级 3 以解锁。\n"
                           + "···在转生时保留你的部分资源（右方展示的那些）。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                           + s1 + s2;

        }

        private void 升级保留_信息()
        {
            prestige_upgrade p = prestige_ups["升级保留"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，转生后使方块生产器的等级不降到历史最大等级的75%以下。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，转生后将所有升级（法术除外）等级变为（历史最大等级 -1）。\n";
                s2 = "···此升级目前的效果是：\n       转生后使方块生产器的等级不降到历史最大等级的75%以下。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，转生后保留所有法术等级，方块生产器的保留等级提高为90%。\n";
                s2 = "···此升级目前的效果是：\n       转生后使方块生产器的等级不降到最大等级的75%以下。\n" +
                                                 "       转生后将所有升级（法术除外）等级变为（历史最大等级 -1）。";
            }
            else if (p.level == 3)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       转生后使方块生产器的等级不降到最大等级的90%以下。\n" +
                                                 "       转生后将所有升级（法术除外）等级变为（历史最大等级 -1）。" +
                                                 "       转生后保留所有法术等级。";
            }

            m.详细信息框.Text = "···需要“资源保留”等级 3 以解锁。\n"
                           + "···在转生时保留你的部分升级。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                           + s1 + s2;

        }

        private void 制造_信息()
        {
            prestige_upgrade p = prestige_ups["制造"];

            string not_max = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }

            string s1 = "";
            string s2 = "";
            if (p.level == 0)
            {
                s1 = "···解锁制造面板，你可以在此面板中制造各种各样的东西。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，解锁糖浆和“药水效果降低”升级器。\n";
                s2 = "···此升级目前的效果是：\n       解锁制造面板。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，解锁斧头、植物祭品、动物祭品。\n";
                s2 = "···此升级目前的效果是：\n       解锁制造面板。\n" +
                                                 "       解锁糖浆和“药水效果降低”升级器。";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，解锁魔法粉末制造、魔杖、能量饮料、冰镇果汁。\n";
                s2 = "···此升级目前的效果是：\n       解锁制造面板。\n" +
                                                 "       解锁糖浆和“药水效果降低”升级器。\n" +
                                                 "       解锁斧头、植物祭品、动物祭品。";
            }
            else if (p.level == 4)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       解锁制造面板。\n" +
                                                 "       解锁糖浆和“药水效果降低”升级器。\n" +
                                                 "       解锁斧头、植物祭品、动物祭品。\n" +
                                                 "       解锁魔法粉末制造、魔杖、能量饮料、冰镇果汁。";
            }

            m.详细信息框.Text = "···解锁一些制造项。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 核心_信息()
        {
            prestige_upgrade p = prestige_ups["核心"];

            string not_max = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }

            string s1 = "";
            string s2 = "";
            if (p.level == 0)
            {
                s1 = "···解锁核心面板，你可以在此面板中进行进一步的游戏探索。";
            }
            else if (p.level == 1)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       解锁核心面板。";
            }

            m.详细信息框.Text = "···需要“制造”等级 4 以解锁。\n"
                            + "···解锁核心面板的游戏内容。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 方块增幅_信息()
        {
            prestige_upgrade p = prestige_ups["方块增幅"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，白色方块生产器产量 × 3。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，白色方块生产器最大时间 / 2。\n";
                s2 = "···此升级目前的效果是：\n       白色方块生产器产量 × 3。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，白色方块生产器产量 × 2，最大时间 / 1.5。\n";
                s2 = "···此升级目前的效果是：\n       白色方块生产器产量 × 3。" +
                                               "\n       白色方块生产器最大时间 / 2。";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，解锁泥土方块生产升级器，解锁糖方块生产器，白色方块生产器产量 × 1.5。\n";
                s2 = "···此升级目前的效果是：\n       白色方块生产器产量 × 6。" +
                                               "\n       白色方块生产器最大时间 / 3。";
            }
            else if (p.level == 4)
            {
                s1 = "···购买本级后，白色方块生产器最大时间 / 5。\n";
                s2 = "···此升级目前的效果是：\n       白色方块生产器产量 × 9。" +
                                               "\n       白色方块生产器最大时间 / 3。" +
                                               "\n       解锁泥土方块生产升级器。" +
                                               "\n       解锁糖方块生产器。";
            }
            else if (p.level == 5)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       白色方块产量 × 9。" +
                                               "\n       白色方块最大时间 / 15。" +
                                               "\n       解锁泥土方块生产升级器。" +
                                               "\n       解锁糖方块生产器。";
            }

            m.详细信息框.Text = "···需要“制造”等级 1 以解锁。\n"
                            + "···提升方块产量。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 时间力量_信息()
        {
            prestige_upgrade p = prestige_ups["时间力量"];
            double2 percent = (time_power() - 1) * 100;

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，生成器和方块生产器的产量 +t%\n" +
                     "        （t是距离本次转生的时间，单位为秒），在转生后重置。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，转生后首先获取 9900 秒的时间力量。\n";
                s2 = "···此升级目前的效果是：\n       生成器和方块生产器的产量 +t%。[ +" + number_format(percent) + "% ]";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，时间力量产量效果 ^ 1.1。\n";
                s2 = "···此升级目前的效果是：\n       生成器和方块生产器的产量 +(t + 9900)%。[ +" + number_format(percent) + "% ]";
            }
            else if (p.level == 3)
            {
                s1 = "";//倍增燃料速度值
                s2 = "···此升级目前的效果是：\n       生成器和方块生产器的产量 +((t + 9900) ^ 1.1)%。[ +" + number_format(percent) + "% ]";
            }

            m.详细信息框.Text = "···需要“方块增幅”等级 5 以解锁。\n"
                            + "···随时间增强你的一些属性。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 魔法增幅_信息()
        {
            prestige_upgrade p = prestige_ups["魔法增幅"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，法术学习消耗 / 1.5，法术学习速度 × 1.5。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，药水配制速度 × 3。\n";
                s2 = "···此升级目前的效果是：\n       法术学习消耗 / 1.5，法术学习速度 × 1.5。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，法术学习消耗 / 2，法术学习速度 × 2，药水效果 × 1.5。\n";
                s2 = "···此升级目前的效果是：\n       法术学习消耗 / 1.5，法术学习速度 × 1.5。" +
                                               "\n       药水配制速度 × 3。";
            }
            else if (p.level == 3)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       法术学习消耗 / 3，法术学习速度 × 3。" +
                                               "\n       药水配制速度 × 3。" +
                                               "\n       药水效果 × 1.5。";
            }

            m.详细信息框.Text = "···需要“方块增幅”等级 3 以解锁。\n"
                            + "···让魔法变得更强大。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private double2 get_magic_res_boost()
        {
            double2 ret = 1;
            foreach (KeyValuePair<string, resource> kp in res_table["魔法"]) 
            {
                ret *= 1 + ((1 + kp.Value.get_value()).Log10() / Math.Log10(2) / 100);
            }
            return ret;
        }

        private double2 get_potion_value_boost()
        {
            return double2.Pow(res_table["转生"]["药水值"].get_value() + 1, 0.05);
        }

        private double2 save_spell_uncast_boost;
        private double2 get_spell_uncast_boost()
        {
            decimal level = 0;
            foreach (KeyValuePair<string, enchant> kp in enchants)
            {
                level += kp.Value.level / 10;
            }
            foreach (KeyValuePair<string, upgrade> kp in upgrades)
            {
                if (kp.Value is spell)
                {
                    level += kp.Value.level;
                }
            }
            return double2.Min(1 - double2.Pow(0.99, level), 0.75);
        }

        private void 转化_信息()
        {
            prestige_upgrade p = prestige_ups["转化"];

            string not_max = "";
            string s1 = "";
            string s2 = "";

            string mrb = number_format(get_magic_res_boost());
            string pvb = number_format(get_potion_value_boost());
            string sub = number_format(get_spell_uncast_boost());

            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，“魔法”中的每种资源以 +log2(资源数 + 1)% 的效率累乘到祭坛魔力转换速度和药水配制速度。（目前 ×" + mrb + "）";
                //显示目前的效果
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，喝下一瓶药水获得 [药水等级 * (2 ^ 药水序号)] 的药水值，（药水序号指药水摆放的位置，从1开始）" +
                    "\n将转生点数和魔力的获取变为 [(药水值 + 1) ^ 0.05] 倍。（目前 ×" + pvb + "）\n";
                s2 = "···此升级目前的效果是：\n       “魔法”中的每种资源以 +log2(资源数 + 1)% 的效率累乘到祭坛的魔力转换速度和药水配制速度。（目前 ×" + mrb + "）";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，当法术的主动效果未激活时仍能获取 {1 - 0.99 ^ [总法术等级 + (总附魔速度等级 / 10)]} 倍（最大 0.75 倍）的效果。（目前 ×" + sub + "）\n";
                s2 = "···此升级目前的效果是：\n       “魔法”中的每种资源以 +log2(资源数 + 1)% 的效率累乘到祭坛的魔力转换速度和药水配制速度。（目前 ×" + mrb + "）" +
                                               "\n       喝下一瓶药水获得 [药水等级 * (2 ^ 药水序号)] 的药水值，将转生点数和魔力的获取变为 [(药水值 + 1) ^ 0.05]  倍。（目前 ×" + pvb + "）";
            }
            else if (p.level == 3)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       “魔法”中的每种资源以 +log2(资源数 + 1)% 的效率累乘到祭坛的魔力转换速度和药水配制速度。（目前 ×" + mrb + "）" +
                                               "\n       喝下一瓶药水获得 [药水等级 * (2 ^ 药水序号)] 的药水值，将转生点数和魔力的获取变为 [(药水值 + 1) ^ 0.05] 倍。（目前 ×" + pvb + "）" +
                                               "\n       当法术的主动效果未激活时仍能获取 {1 - 0.99 ^ [总法术等级 + (总附魔速度等级 / 10)]} 倍（最大 0.75 倍）的效果。（目前 ×" + sub + "）";
            }

            m.详细信息框.Text = "···需要“魔法增幅”等级 3 以解锁。\n"
                            + "···使魔法资源能够提供奖励。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 采矿增幅_信息()
        {
            prestige_upgrade p = prestige_ups["采矿增幅"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，采矿基础经验 +2，格子边长 +200。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，熔炉的燃料速度值 ×2.5。\n";
                s2 = "···此升级目前的效果是：\n       采矿基础经验 +2。" +
                                               "\n       格子边长 +200。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，采矿点获取 ×1.5， 格子边长 +500，转生后采矿等级从 20 开始。\n";
                s2 = "···此升级目前的效果是：\n       采矿基础经验 +2。" +
                                               "\n       格子边长 +200。" +
                                               "\n       熔炉的燃料速度值 ×2.5。";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，熔炉的原料速度值 ×3，燃料速度值 ×2。\n";
                s2 = "···此升级目前的效果是：\n       采矿基础经验 +2。" +
                                               "\n       格子边长 +700。" +
                                               "\n       采矿点获取 ×1.5。" +
                                               "\n       转生后采矿等级从 20 开始。" +
                                               "\n       熔炉的燃料速度值 ×2.5。";
            }
            else if (p.level == 4)
            {
                s1 = "···购买本级后，幸运值 ×2，采矿基础经验 +13，转生后采矿等级从 40 开始。\n";
                s2 = "···此升级目前的效果是：\n       采矿基础经验 +2。" +
                                               "\n       格子边长 +700。" +
                                               "\n       采矿点获取 ×1.5。" +
                                               "\n       转生后采矿等级从 20 开始。" +
                                               "\n       熔炉的燃料速度值 ×5。" +
                                               "\n       熔炉的原料速度值 ×3。";
            }
            else if (p.level == 5)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       采矿基础经验 +15。" +
                                               "\n       格子边长 +700。" +
                                               "\n       采矿点获取 ×1.5。" +
                                               "\n       幸运值 ×2。" +
                                               "\n       转生后采矿等级从 40 开始。" +
                                               "\n       熔炉的燃料速度值 ×5。" +
                                               "\n       熔炉的原料速度值 ×3。";
            }

            m.详细信息框.Text = "···需要“魔法增幅”等级 2 以解锁。\n"
                            + "···获得采矿中的数据增益。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }


        private void 战斗增幅_信息()
        {
            prestige_upgrade p = prestige_ups["战斗增幅"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，基础攻击变为 25，基础减速值下降速度变为 0.2。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，减速效果下降的速度 × 1.5。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 25。" +
                                               "\n       基础减速值下降速度变为 0.2。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，基础攻击变为 75，且攻击时间 / 1.5。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 25。" +
                                               "\n       基础减速值下降速度变为 0.2。" +
                                               "\n       减速效果下降的速度 × 1.5。";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，攻击 × 1.25，基础攻击变为 200，基础减速值下降速度变为 1。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 75。" +
                                               "\n       基础减速值下降速度变为 0.2。" +
                                               "\n       减速效果下降的速度 × 1.5。" +
                                               "\n       攻击时间 / 1.5。";
            }
            else if (p.level == 4)
            {
                s1 = "···购买本级后，攻击 × 1.6，减速效果下降的速度 × 2。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 200。" +
                                               "\n       攻击 × 1.25。" + 
                                               "\n       基础减速值下降速度变为 1。" +
                                               "\n       减速效果下降的速度 × 1.5。" +
                                               "\n       攻击时间 / 1.5。";
            }
            else if (p.level == 5)
            {
                s1 = "···购买本级后，基础攻击变为 500，基础减速值下降速度变为 4，\n" +
                     "击败敌人获取3倍经验。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 200。" +
                                               "\n       攻击 × 2。" +
                                               "\n       基础减速值下降速度变为 1。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 1.5。";
            }
            else if (p.level == 6)
            {
                s1 = "···购买本级后，升级所需的基础经验值 / 7，转生后战斗等级从150开始。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 500。" +
                                               "\n       攻击 × 2。" +
                                               "\n       基础减速值下降速度变为 4。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 1.5。" + 
                                               "\n       击败敌人获取经验 × 3。";
            }
            else if (p.level == 7)
            {
                s1 = "···购买本级后，基础攻击变为 1500，攻击时间 / 1.2，攻击 × 2。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 500。" +
                                               "\n       攻击 × 2。" +
                                               "\n       基础减速值下降速度变为 4。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 1.5。" +
                                               "\n       升级所需的基础经验值 / 7。" +
                                               "\n       击败敌人获取经验 × 3。" +
                                               "\n       转生后战斗等级从150开始。";
            }
            else if (p.level == 8)
            {
                s1 = "···购买本级后，基础攻击变为 5000，击败敌人获取经验 × 2。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 1500。" +
                                               "\n       攻击 × 4。" +
                                               "\n       基础减速值下降速度变为 4。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 1.8。" +
                                               "\n       升级所需的基础经验值 / 7。" +
                                               "\n       击败敌人获取经验 × 3。" +
                                               "\n       转生后战斗等级从150开始。";
            }
            else if (p.level == 9)
            {
                s1 = "···购买本级后，基础减速值下降速度变为 30，攻击 × 1.6。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 5000。" +
                                               "\n       攻击 × 6.4。" +
                                               "\n       基础减速值下降速度变为 4。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 1.8。" +
                                               "\n       升级所需的基础经验值 / 7。" +
                                               "\n       击败敌人获取经验 × 6。" +
                                               "\n       转生后战斗等级从150开始。";
            }
            else if (p.level == 10)
            {
                s1 = "···购买本级后，攻击时间 / 1.667，击败敌人获取经验 × 10，转生后战斗等级从300开始。\n";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 5000。" +
                                               "\n       攻击 × 6.4。" +
                                               "\n       基础减速值下降速度变为 30。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 1.8。" +
                                               "\n       升级所需的基础经验值 / 7。" +
                                               "\n       击败敌人获取经验 × 6。" +
                                               "\n       转生后战斗等级从150开始。";
            }
            else if (p.level == 11)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       基础攻击变为 5000。" +
                                               "\n       攻击 × 6.4。" +
                                               "\n       基础减速值下降速度变为 30。" +
                                               "\n       减速效果下降的速度 × 3。" +
                                               "\n       攻击时间 / 3。" +
                                               "\n       升级所需的基础经验值 / 7。" +
                                               "\n       击败敌人获取经验 × 60。" +
                                               "\n       转生后战斗等级从300开始。";
            }
            m.详细信息框.Text = "···需要“制造”等级 1 以解锁。\n"
                            + "···给予你各种各样的战斗属性增益。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 强化等级_信息()
        {
            prestige_upgrade p = prestige_ups["强化等级"];

            string potion_reduce = "（目前 / " + number_format(double2.Pow(1.005, you.level + minep.level)) + "）";

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，每一战斗等级的攻击力 +2，而不是 +0.5。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，每一战斗等级获取 +0.02 的减速值下降速度，而不是 +0.01。\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +2，而不是 +0.5";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，每一战斗等级的攻击力 +11%，而不是 +10%。\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +2，而不是 +0.5" +
                                               "\n       每一战斗等级获取 +0.02 的减速值下降速度，而不是 +0.01";
            }
            else if (p.level == 3)
            {
                s1 = "···购买本级后，每一战斗等级的攻击力 +5，而不是 +2。\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +2，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11%，而不是 +10%" +
                                               "\n       每一战斗等级获取 +0.02 的减速值下降速度，而不是 +0.01";
            }
            else if (p.level == 4)
            {
                s1 = "···购买本级后，每一战斗等级 +0.3% 战斗掉落物（累乘），\n" + 
                    "且对白色方块和白色粉末5倍效果\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +5，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11%，而不是 +10%" +
                                               "\n       每一战斗等级获取 +0.02 的减速值下降速度，而不是 +0.01";
            }
            else if (p.level == 5)
            {
                s1 = "···购买本级后，每一战斗等级获取 +0.05 的减速值下降速度，而不是 +0.02。\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +5，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11%，而不是 +10%" +
                                               "\n       每一战斗等级 +0.3% 战斗掉落物（累乘），对白色方块和白色粉末5倍效果" +
                                               "\n       每一战斗等级获取 +0.02 的减速值下降速度，而不是 +0.01";
            }
            else if (p.level == 6)
            {
                s1 = "···购买本级后，每级（战斗、采矿或娱乐）将药水配制时间 -0.5%。" + potion_reduce + "\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +5，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11%，而不是 +10%" +
                                               "\n       每一战斗等级 +0.3% 战斗掉落物（累乘），对白色方块和白色粉末5倍效果" +
                                               "\n       每一战斗等级获取 +0.05 的减速值下降速度，而不是 +0.01";
            }
            else if (p.level == 7)
            {
                s1 = "···购买本级后，每一战斗等级的攻击力 +30，而不是 +5。\n" +
                    "每一采矿等级的格子边长 +11%，而不是 +10%\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +5，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11%，而不是 +10%" +
                                               "\n       每一战斗等级 +0.3% 战斗掉落物（累乘），对白色方块和白色粉末5倍效果" +
                                               "\n       每一战斗等级获取 +0.05 的减速值下降速度，而不是 +0.01" +
                                               "\n       每级（战斗、采矿或娱乐）将药水配制时间 -0.5%。" + potion_reduce;
            }
            else if (p.level == 8)
            {
                s1 = "···购买本级后，每一战斗等级获取 +0.15 的减速值下降速度，而不是 +0.05。\n" +
                    "每一战斗等级的攻击力 +11.5%，而不是 +11%。\n" +
                    "每一采矿等级的幸运值 +0.25，而不是 +0.1。\n" +
                    "每 10 采矿等级使熔炉的燃料速度值 ×1.25。\n";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +30，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11%，而不是 +10%" +
                                               "\n       每一战斗等级 +0.3% 战斗掉落物（累乘），对白色方块和白色粉末5倍效果" +
                                               "\n       每一战斗等级获取 +0.05 的减速值下降速度，而不是 +0.01" +
                                               "\n       每级（战斗、采矿或娱乐）将药水配制时间 -0.5%。" + potion_reduce +
                                               "\n       每一采矿等级的格子边长 +11%，而不是 +10%";
            }
            else if (p.level == 9)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       每一战斗等级的攻击力 +30，而不是 +0.5" +
                                               "\n       每一战斗等级的攻击力 +11.5%，而不是 +10%" +
                                               "\n       每一战斗等级 +0.3% 战斗掉落物（累乘），对白色方块和白色粉末5倍效果" +
                                               "\n       每一战斗等级获取 +0.15 的减速值下降速度，而不是 +0.01" +
                                               "\n       每级（战斗、采矿或娱乐）将药水配制时间 -0.5%。" + potion_reduce +
                                               "\n       每一采矿等级的格子边长 +11%，而不是 +10%" +
                                               "\n       每一采矿等级的幸运值 +0.25，而不是 +0.01" +
                                               "\n       每 10 采矿等级使熔炉的燃料速度值 ×1.25";
            }

            m.详细信息框.Text = "···需要“战斗增幅”等级 2 以解锁。\n"
                            + "···让等级提升后的奖励更好。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        private void 战斗探索_信息()
        {
            prestige_upgrade p = prestige_ups["战斗探索"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，每 1 级探索魔法让洁白世界敌人的最大等级 +5。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，每 1 级探索魔法让草原和死火山敌人的最大等级 +3。\n";
                s2 = "···此升级目前的效果是：\n       每 1 级探索魔法让洁白世界敌人的最大等级 +5。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，获得 2 个虚拟的探索魔法等级用于增强本升级的效果。\n";
                s2 = "···此升级目前的效果是：\n       每 1 级探索魔法让洁白世界敌人的最大等级 +5。" +
                                               "\n       每 1 级探索魔法让草原敌人的最大等级 +3。" +
                                               "\n       每 1 级探索魔法让死火山敌人的最大等级 +3。";
            }
            else if (p.level == 3)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       每 1 级探索魔法让洁白世界敌人的最大等级 +5。" +
                                               "\n       每 1 级探索魔法让草原敌人的最大等级 +3。" +
                                               "\n       每 1 级探索魔法让死火山敌人的最大等级 +3。" +
                                               "\n       有 2 个虚拟的探索魔法等级用于增强本升级的效果。";
            }

            m.详细信息框.Text = "···需要“强化等级”等级 3 以解锁。\n"
                            + "···提升敌人的最大等级。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }
        private void 冷静_信息()
        {
            prestige_upgrade p = prestige_ups["冷静"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，将敌方减速值 / 300，你的减速值下降速度 / 200。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，将攻击时间 ×7，攻击重数 ×10，减速值获取 ×5。\n" +
                    "（攻击重数倍增所有攻击后获得的战利品，包括攻击次数）";
                s2 = "···此升级目前的效果是：\n       将敌方减速值 / 300，你的减速值下降速度 / 200。";
            }
            else if (p.level == 2)
            {
                s1 = "···购买本级后，附魔所需时间 ×10，材料需求 -10级，最大速度等级 +50\n";
                s2 = "···此升级目前的效果是：\n       将敌方减速值 / 300，你的减速值下降速度 / 200。" +
                                               "\n       攻击时间 ×7，攻击重数 ×10，减速值获取 ×5。" +
                                               "\n     （攻击重数倍增所有攻击后获得的战利品，包括攻击次数）";
            }
            else if (p.level == 3)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       将敌方减速值 / 300，你的减速值下降速度 / 200。" +
                                               "\n       攻击时间 ×7，攻击重数 ×10，减速值获取 ×5。" +
                                               "\n     （攻击重数倍增所有攻击后获得的战利品，包括攻击次数）" + 
                                               "\n       附魔所需时间 ×10，材料需求 -10级，最大速度等级 +50";
            }

            m.详细信息框.Text = "···需要“战斗探索”等级 1 以解锁。\n"
                            + "···减慢一些活动的进行，并因此获得增益。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }
        private void 成就加成_信息()
        {
            prestige_upgrade p = prestige_ups["成就加成"];

            string not_max = "";
            string s1 = "";
            string s2 = "";
            if (p.level != p.max_level)
            {
                not_max = "     价格: " + number_format(p.cost[p.level]) + " " + p.cost_res_type;
            }
            if (p.level == 0)
            {
                s1 = "···购买本级后，每 1 成就点数使白色方块和白色粉末获取 +1%（累加）。";
            }
            else if (p.level == 1)
            {
                s1 = "···购买本级后，每 1 成就点数使采矿经验 +0.5%（累加）。\n";
                s2 = "···此升级目前的效果是：\n       每 1 成就点数使白色方块和白色粉末获取 +1%（累加）。";
            }
            else if (p.level == 2)
            {
                s1 = "";
                s2 = "···此升级目前的效果是：\n       每 1 成就点数使白色方块和白色粉末获取 +1%（累加）。" +
                                               "\n       每 1 成就点数使采矿经验 +0.5%（累加）。";
            }

            m.详细信息框.Text = "···根据成就点数获得奖励。   等级 " + p.level + " / " + p.max_level + not_max + "\n"
                            + s1 + s2;
        }

        #endregion
    }
}
