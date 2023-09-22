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
        resource 成就点数 = new resource(0, 0, "成就点数", null);
        private void init_achieve()
        {
            achievement a;
            string name;

            //0，0 新世界
            name = "新世界";
            a = new achievement(name);

            a.add_level(new achievement_level("解锁“制造”", 4));
            a.add_level(new achievement_level("解锁“战斗”", 4));
            a.add_level(new achievement_level("解锁“魔法”", 6));
            a.add_level(new achievement_level("解锁“采矿”", 16));
            a.add_level(new achievement_level("解锁“核心”", 30));
            a.add_level(new achievement_level("解锁“娱乐”", 50));
            a.add_level(new achievement_level("解锁“混沌”", 90));

            achievements_name.Add(name, a);
            achievements_id.Add(0, a);

            //0，1 方块学家
            name = "方块学家";
            a = new achievement(name);
            
            a.add_level(new achievement_level("提升“白色方块生产器”到20级。", 2));
            a.add_level(new achievement_level("提升“白色方块生产器”到50级。", 3));
            a.add_level(new achievement_level("提升“泥土方块生产器”到100级。", 4));
            a.add_level(new achievement_level("拥有 " + number_format(5e12) + "木头方块。", 5));
            a.add_level(new achievement_level("提升“白色方块生产器”到120级。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(1, a);

            //0，2 
            name = "多产线";
            a = new achievement(name);

            a.add_level(new achievement_level("解锁泥土方块。", 2));
            a.add_level(new achievement_level("解锁木头方块。", 3));
            a.add_level(new achievement_level("解锁石头方块。", 4));

            achievements_name.Add(name, a);
            achievements_id.Add(2, a);

            //0，4
            name = "升级大师";
            a = new achievement(name);

            a.add_level(new achievement_level("提升战斗等级到50级。", 2));
            a.add_level(new achievement_level("提升战斗等级到100级。", 3));
            a.add_level(new achievement_level("提升战斗等级到150级。", 3));
            a.add_level(new achievement_level("提升采矿等级到10级。", 4));
            a.add_level(new achievement_level("提升战斗等级到200级。", 4));
            a.add_level(new achievement_level("提升采矿等级到50级。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(4, a);


            //0，5 
            name = "攻击性";
            a = new achievement(name);

            a.add_level(new achievement_level("攻击1000次。", 2));
            a.add_level(new achievement_level("攻击5000次。", 3));
            a.add_level(new achievement_level("攻击" + number_format(20e3) + "次。", 3));
            a.add_level(new achievement_level("攻击" + number_format(50e3) + "次。", 4));
            a.add_level(new achievement_level("攻击" + number_format(300e3) + "次。", 4));


            achievements_name.Add(name, a);
            achievements_id.Add(5, a);

            //0，6
            name = "攻击能力";
            a = new achievement(name);

            a.add_level(new achievement_level("攻击力达到" + number_format(1e6) + "。", 3));
            a.add_level(new achievement_level("攻击力达到" + number_format(1e9) + "。", 4));
            a.add_level(new achievement_level("攻击力达到" + number_format(10e12) + "。", 5));
            a.add_level(new achievement_level("攻击力达到" + number_format(100e18) + "。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(6, a);


            //0，7
            name = "防寒措施";
            a = new achievement(name);

            a.add_level(new achievement_level("减速值下降速度达到" + number_format(4) + "。", 3));
            a.add_level(new achievement_level("减速值下降速度达到" + number_format(15) + "。", 4));
            a.add_level(new achievement_level("减速值下降速度达到" + number_format(50) + "。", 5));
            a.add_level(new achievement_level("减速值下降速度达到" + number_format(200) + "。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(7, a);

            //1，0
            name = "魔法师";
            a = new achievement(name);

            a.add_level(new achievement_level("拥有" + number_format(100e6) + "魔力。", 3));
            a.add_level(new achievement_level("拥有" + number_format(25e9) + "魔力。", 4));
            a.add_level(new achievement_level("拥有" + number_format(10e12) + "魔力。", 5));
            a.add_level(new achievement_level("拥有" + number_format(5e15) + "魔力。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(10, a);

            //1，1
            name = "多彩魔法";
            a = new achievement(name);

            a.add_level(new achievement_level("解锁白色魔法。", 2));
            a.add_level(new achievement_level("解锁绿色魔法。", 3));
            a.add_level(new achievement_level("解锁红色魔法。", 4));
            a.add_level(new achievement_level("解锁橙色魔法。", 4));
            a.add_level(new achievement_level("解锁蓝色魔法。", 4));
            a.add_level(new achievement_level("解锁无色魔法。", 4));

            achievements_name.Add(name, a);
            achievements_id.Add(11, a);

            //1，4
            name = "大矿场";
            a = new achievement(name);

            a.add_level(new achievement_level("格子边长达到 " + number_format(3000) + "。", 2));
            a.add_level(new achievement_level("格子边长达到 " + number_format(8000) + "。", 3));
            a.add_level(new achievement_level("格子边长达到 " + number_format(20e3) + "。", 4));
            a.add_level(new achievement_level("格子边长达到 " + number_format(50e3) + "。", 5));
            a.add_level(new achievement_level("格子边长达到 " + number_format(150e3) + "。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(14, a);



            //1，5
            name = "幸运点";
            a = new achievement(name);

            a.add_level(new achievement_level("基础幸运值达到 " + number_format(5) + "。", 3));
            a.add_level(new achievement_level("基础幸运值达到 " + number_format(10) + "。", 3));
            a.add_level(new achievement_level("基础幸运值达到 " + number_format(20) + "。", 4));
            a.add_level(new achievement_level("基础幸运值达到 " + number_format(40) + "。", 5));
            a.add_level(new achievement_level("基础幸运值达到 " + number_format(80) + "。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(15, a);


            //1，6
            name = "勤劳矿工";
            a = new achievement(name);

            a.add_level(new achievement_level("共获取采矿点数 " + number_format(1000) + "。", 2));
            a.add_level(new achievement_level("共获取采矿点数 " + number_format(5000) + "。", 3));
            a.add_level(new achievement_level("共获取采矿点数 " + number_format(25000) + "。", 4));
            a.add_level(new achievement_level("共获取采矿点数 " + number_format(100000) + "。", 5));
            a.add_level(new achievement_level("共获取采矿点数 " + number_format(300e3) + "。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(16, a);

            //1，7
            name = "燃起来了";
            a = new achievement(name);

            a.add_level(new achievement_level("火力达到 " + number_format(157.5e3) + "。", 1));
            a.add_level(new achievement_level("火力达到 " + number_format(5e6) + "。", 2));
            a.add_level(new achievement_level("火力达到 " + number_format(100e6) + "。", 4));
            a.add_level(new achievement_level("火力达到 " + number_format(50e9) + "。", 6));
            a.add_level(new achievement_level("火力达到 " + number_format(1e12) + "。", 6));

            achievements_name.Add(name, a);
            achievements_id.Add(17, a);

            //2，0
            name = "强悍熔炉";
            a = new achievement(name);

            a.add_level(new achievement_level("熔炉等级达到 " + number_format(2) + "。", 2));
            a.add_level(new achievement_level("熔炉等级达到 " + number_format(5) + "。", 4));
            a.add_level(new achievement_level("熔炉等级达到 " + number_format(10) + "。", 6));
            a.add_level(new achievement_level("熔炉等级达到 " + number_format(15) + "。", 8));
            a.add_level(new achievement_level("熔炉等级达到 " + number_format(20) + "。", 10));

            achievements_name.Add(name, a);
            achievements_id.Add(20, a);

            //

        }

        private void achieve_check()
        {
            while (true)
            {
                int id = 0;
                int lev = 0;

                //新世界
                id = 0;
                lev = 0;
                #region
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.制造.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.战斗.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.魔法.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.采矿.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.核心.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.娱乐.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (m.混沌.Visibility == 0)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                #endregion

                //方块学家
                id = 1;
                lev = 0;
                #region
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["白色方块"].level >= 20)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["白色方块"].level >= 50)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["泥土方块"].level >= 100)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["方块"]["木头方块"].get_value() >= 5e12)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["白色方块"].level >= 120)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                #endregion

                //多产线
                id = 2;
                lev = 0;
                #region
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["泥土方块"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["木头方块"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (block_producters["石头方块"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                #endregion

                //升级大师
                id = 4;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.level >= 50)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.level >= 100)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (you.level >= 150)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (minep.level >= 10)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.level >= 200)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (minep.level >= 50)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion


                //攻击性
                id = 5;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.攻击次数 >= 1000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.攻击次数 >= 5000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (you.攻击次数 >= 20e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.攻击次数 >= 50e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.攻击次数 >= 300e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion


                //攻击能力
                id = 6;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.get_attack() >= 1e6)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.get_attack() >= 1e9)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (you.get_attack() >= 10e12)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.get_attack() >= 100e18)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //防寒措施
                id = 7;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.get_sr() >= 4)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.get_sr() >= 15)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (you.get_sr() >= 50)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (you.get_sr() >= 200)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //魔法师
                id = 10;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["魔法"]["魔力"].get_value() >= 100e6)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["魔法"]["魔力"].get_value() >= 25e9)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (res_table["魔法"]["魔力"].get_value() >= 10e12)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["魔法"]["魔力"].get_value() >= 5e15)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //魔法师
                id = 11;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (upgrades["白色魔法"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (upgrades["绿色魔法"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (upgrades["红色魔法"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (upgrades["橙色魔法"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (upgrades["蓝色魔法"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (upgrades["无色魔法"].unlocked)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //大矿场
                id = 14;
                #region
                lev = 0;
                double2 size = (1000 + minep.size_boost) * minep.get_size_mul();
                if (achievements_id[id].curr_level == lev)
                {
                    if (size >= 3000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (size >= 8000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (size >= 20e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (size >= 50e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (size >= 150e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //幸运点
                id = 15;
                #region
                lev = 0;
                double2 luck = (1 + minep.luck_boost) * minep.get_luck_mul();
                if (achievements_id[id].curr_level == lev)
                {
                    if (luck >= 5)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (luck >= 10)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (luck >= 20)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (luck >= 40)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (luck >= 80)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //勤劳矿工
                id = 16;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["采矿"]["采矿点数"].all_get >= 1000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["采矿"]["采矿点数"].all_get >= 5000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (res_table["采矿"]["采矿点数"].all_get >= 25000)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["采矿"]["采矿点数"].all_get >= 100e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (res_table["采矿"]["采矿点数"].all_get >= 300e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //燃起来了
                id = 17;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.fire >= 157.5e3)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.fire >= 5e6)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (furance.fire >= 100e6)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.fire >= 50e9)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.fire >= 1e12)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                //强悍熔炉
                id = 20;
                #region
                lev = 0;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.level >= 2)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.level >= 5)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev) //3
                {
                    if (furance.level >= 10)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.level >= 15)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                if (achievements_id[id].curr_level == lev)
                {
                    if (furance.level >= 20)
                    {
                        achieve_complete(id);
                        continue;
                    }
                }
                lev++;
                #endregion

                break;
            }
        }

        private void achieve_complete(int id)
        {
            achievement a = achievements_id[id];
            double2 r = a.levels[a.curr_level].reward;
            成就点数.add_value(r);
            r *= 成就点数.get_mul();
            if (prestige_ups["成就加成"].level >= 1)
            {
                (find_resource("白色方块")).multipliers["成就加成"].value += 0.01 * r;
                (find_resource("白色粉末")).multipliers["成就加成"].value += 0.01 * r;
            }
            if (prestige_ups["成就加成"].level >= 2)
            {
                minep.exp_multi[2]["成就加成"].value += 0.005 * r;
            }

            a.curr_level++;
            a.up_level++;

            int i = id / 10;
            int j = id % 10;
            achievefield_hint[i, j].Visibility = 0;
            achievefield_hint_texts[i, j].Text = Convert.ToString(a.up_level);

            total_up_levels++;
            m.achieve_hint_grid.Visibility = 0;
            m.achieve_hint_total_text.Text = Convert.ToString(total_up_levels);

        }
    }
}
