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
        //升级效果
        private void upgrade_effect(upgrade u, decimal level)
        {
            if (u is g1_upgrade)
            {
                g1_upgrade_effect(u as g1_upgrade, level);
                return;
            }

            //工具
            if (u.name == "方块棒")
            #region
            {
                switch (level)
                {
                    case 1:
                        you.item_attack += 3;
                        m.战斗.Visibility = 0;
                        res_table["战斗"]["白色粒子"].unlocked = true;
                        break;
                    case 2:
                        you.item_attack += 5;
                        upgrades["喷雾"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_工具_喷雾_grid"))).Visibility = 0;
                        break;
                    case 3:
                        you.item_attack += 12;
                        break;
                    case 4:
                        you.item_attack += 20;
                        break;
                    case 5:
                        you.item_attack += 26;
                        break;
                    case 6:
                        you.item_attack += 54;
                        break;
                    case 7:
                        you.item_attack += 80;
                        break;
                    case 8:
                        you.item_attack += 100;
                        break;
                    case 9:
                        you.item_attack += 150;
                        break;
                    case 10:
                        you.item_attack += 216;
                        break;
                    case 11:
                        you.item_attack += 556;
                        break;
                    case 12:
                        you.item_attack += 778;
                        attack_forms["普通"].def_ignore_percent += 0.25;
                        break;
                    case 13:
                        you.item_attack += 2000;
                        attack_forms["重击"].attack_factor += 0.25;
                        break;
                    case 14:
                        you.item_attack += 3500;
                        attack_forms["重击"].def_pierce_percent += 0.1;
                        break;
                }
            }
            #endregion
            if (u.name == "喷雾")
            #region
            {
                switch (level)
                {
                    case 1:
                        attack_forms["喷雾"].attack_factor = 0.4;
                        attack_forms["喷雾"].unlocked = true;
                        m.战斗_玩家_攻击风格_手动_喷雾_grid.Visibility = 0;
                        break;
                    case 2:
                        attack_forms["喷雾"].attack_factor = 0.5;
                        break;
                    case 3:
                        attack_forms["喷雾"].manual_anti_fast_click_cd = 0.1;
                        break;
                    case 4:
                        attack_forms["喷雾"].attack_factor = 0.7;
                        break;
                    case 5:
                        attack_forms["喷雾"].sr_factor = 0.3;
                        break;
                    case 6:
                        attack_forms["喷雾"].def_down_percent = 0.2;
                        break;
                }
            }
            #endregion
            if (u.name == "剑")
            #region
            {
                switch (level)
                {
                    case 1:
                        you.item_attack += 150;
                        attack_forms["连斩"].unlocked = true;
                        break;
                    case 2:
                        you.item_attack += 450;
                        连斩_max_damage += 0.1;
                        break;
                    case 3:
                        you.item_attack += 800;
                        连斩_max_damage += 0.1;
                        break;
                    case 4:
                        you.item_attack += 1600;
                        连斩_max_damage += 0.1;
                        break;
                    case 5:
                        you.item_attack += 2500;
                        连斩_max_damage += 0.1;
                        break;
                    case 6:
                        you.item_attack += 4500;
                        连斩_max_damage += 0.1;
                        break;
                }
            }
            #endregion
            if (u.name == "镐")
            #region
            {
                switch (level)
                {
                    case 1:
                        minep.point_boost += 1;
                        break;
                    case 2:
                        minep.size_boost += 300;
                        break;
                    case 3:
                        minep.point_boost += 1;
                        break;
                    case 4:
                        minep.exp_boost += 3;
                        break;
                    case 5:
                        minep.size_boost += 500;
                        break;
                    case 6:
                        minep.point_boost += 2;
                        break;
                    case 7:
                        if (!minep.luck_multi[1].ContainsKey("镐"))
                        {
                            minep.luck_multi[1].Add("镐", new multiplier(true, 1));
                        }
                        minep.luck_multi[1]["镐"].value = 2;

                        minep.exp_boost += 7;
                        break;
                    case 8:
                        minep.size_boost += 1200;
                        break;
                }
            }
            #endregion
            if (u.name == "斧")
            #region
            {
                Dictionary<string, multiplier> d = res_table["方块"]["木头方块"].multipliers;
                if (!d.ContainsKey("斧"))
                {
                    d.Add("斧", new multiplier(true, 1));
                }
                switch (level)
                {
                    case 1:
                        d["斧"].value = 2;
                        break;
                    case 2:
                        d["斧"].value = 3;
                        break;
                    case 3:
                        d["斧"].value = 6;
                        break;
                    case 4:
                        d["斧"].value = 12;
                        break;
                }
            }
            #endregion
            if (u.name == "魔杖")
            #region
            {
                Dictionary<string, multiplier> d = res_table["魔法"]["魔力"].multipliers;
                if (!d.ContainsKey("魔杖"))
                {
                    d.Add("魔杖", new multiplier(true, 1));
                }
                switch (level)
                {
                    case 1:
                        you.item_attack += 500;
                        attack_forms["烈焰"].unlocked = true;
                        d["魔杖"].value = 1.2;
                        break;
                    case 2:
                        you.item_attack += 3500;
                        attack_forms["光球"].unlocked = true;
                        d["魔杖"].value = 1.5;
                        break;
                }
            }
            #endregion

            //升级器
            if (u.name == "白色方块生产")
            #region
            {
                switch (level)
                {
                    case 1:
                        block_producters["白色方块"].multiply(0.5, 1);
                        break;
                    case 2:
                        block_producters["白色方块"].multiply(0.5, 1);
                        break;
                    case 3:
                        block_producters["白色方块"].multiply(1, 4);
                        break;
                    case 4:
                        block_producters["白色方块"].multiply(0.5, 1);
                        break;
                    case 5:
                        block_producters["白色方块"].multiply(1 / 2.0, 1);
                        break;
                    case 6:
                        block_producters["白色方块"].multiply(1 / 2.0, 3);
                        break;
                    case 7:
                        block_producters["白色方块"].multiply(1 / 2.0, 2.5);
                        break;
                    case 8:
                        block_producters["白色方块"].multiply(1 / 2.0 / 2.0, 1);
                        break;
                    case 9:
                        block_producters["白色方块"].multiply(1 / 2.0, 3.3);
                        break;
                    case 10:
                        block_producters["白色方块"].multiply(1 / 2.0, 2);
                        break;
                    case 11:
                        block_producters["白色方块"].multiply(1 / 8.0, 2);
                        break;
                    case 12:
                        block_producters["白色方块"].multiply(1 / 8.0, 2);
                        break;
                    case 13:
                        block_producters["白色方块"].multiply(500000 / 65536, 2000 / 792);
                        break;
                }
            }
            #endregion
            if (u.name == "泥土方块生产")
            #region
            {
                switch (level)
                {
                    case 1:
                        block_producters["泥土方块"].multiply(1, 2);
                        break;
                    case 2:
                        block_producters["泥土方块"].multiply(1, 2.5);
                        break;
                    case 3:
                        block_producters["泥土方块"].multiply(1 / 3.0, 1);
                        break;
                }
            }
            #endregion
            if (u.name == "药水消耗降低")
            #region
            {
                switch (level)
                {
                    case 1:
                        foreach (KeyValuePair<string, enchant> kp in enchants)
                        {
                            enchant ec = kp.Value;
                            string key = "药水消耗降低";
                            if (ec.is_potion)
                            {
                                if (!ec.cost_downs.ContainsKey(key))
                                {
                                    ec.cost_downs.Add(key, new multiplier(true, 1));
                                }
                                ec.cost_downs[key].value = 1.5;
                            }
                        }
                        break;
                    case 2:
                        foreach (KeyValuePair<string, enchant> kp in enchants)
                        {
                            enchant ec = kp.Value;
                            string key = "药水消耗降低";
                            if (ec.is_potion)
                            {
                                ec.cost_downs[key].value = 2.4;
                            }
                        }
                        break;
                    case 3:
                        foreach (KeyValuePair<string, enchant> kp in enchants)
                        {
                            enchant ec = kp.Value;
                            string key = "药水消耗降低";
                            if (ec.is_potion)
                            {
                                ec.cost_downs[key].value = 3.6;
                            }
                        }
                        break;
                }
            }
            #endregion

            double2 food_mul = g1_cal_research_food_food_gain();
            //食物
            if (u.name == "缤纷沙拉")
            #region
            {
                switch (level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(9000 * food_mul);
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(12000 * food_mul);
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(18000 * food_mul);
                        break;
                    case 4:
                        res_table["特殊"]["能量"].add_value(24000 * food_mul);
                        break;
                    case 5:
                        res_table["特殊"]["能量"].add_value(30000 * food_mul);
                        break;
                    case 6:
                        res_table["特殊"]["能量"].add_value(35000 * food_mul);
                        break;
                }
            }
            #endregion
            if (u.name == "勇敢生肉套餐")
            #region
            {
                switch (level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(15000 * food_mul);
                        you.item_attack -= 150;
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(24000 * food_mul);
                        you.item_attack -= 325;
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(35000 * food_mul);
                        you.item_attack -= 625;
                        break;
                    case 4:
                        res_table["特殊"]["能量"].add_value(50000 * food_mul);
                        you.item_attack -= 900;
                        break;
                }
            }
            #endregion
            if (u.name == "火爆蔬菜烧烤")
            #region
            {
                switch (level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(11000 * food_mul);
                        you.item_sr += 2.5;
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(18000 * food_mul);
                        you.item_sr += 4;
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(25500 * food_mul);
                        you.item_sr += 5.5;
                        break;
                    case 4:
                        res_table["特殊"]["能量"].add_value(32500 * food_mul);
                        you.item_sr += 8;
                        break;
                }
            }
            #endregion
            if (u.name == "经典BBQ大餐")
            #region
            {
                switch (level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(16000 * food_mul);
                        ex.time_boost_max += 40;
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(25000 * food_mul);
                        ex.time_boost_max += 60;
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(36000 * food_mul);
                        ex.time_boost_max += 100;
                        break;
                }
            }
            #endregion
            if (u.name == "能量饮料")
            #region
            {
                switch (level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(28000 * food_mul);
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(48000 * food_mul);
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(90000 * food_mul);
                        break;
                }
            }
            #endregion
            if (u.name == "冰镇果汁")
            #region
            {
                switch (level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(20000 * food_mul);
                        furance.y_speed_factor *= 0.9;
                        ex.cost_mul *= 0.8;
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(25000 * food_mul);
                        furance.y_speed_factor *= 0.9;
                        ex.cost_mul *= 0.75;
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(30000 * food_mul);
                        furance.y_speed_factor *= 0.7 / 0.81;
                        ex.cost_mul *= 0.4 / 0.6;
                        break;
                }
            }
            #endregion

            //魔法
            if (u.name == "祭坛升级")
            #region
            {
                if (!magic_altar.power_ups.ContainsKey("祭坛升级"))
                {
                    magic_altar.power_ups.Add("祭坛升级", new multiplier(true, 1));
                }
                if (!magic_altar.speed_ups.ContainsKey("祭坛升级"))
                {
                    magic_altar.speed_ups.Add("祭坛升级", new multiplier(true, 1));
                }
                switch (level)
                {
                    case 1:
                        magic_altar.power_ups["祭坛升级"].value = 1.2;
                        magic_altar.speed_ups["祭坛升级"].value = 2;
                        break;
                    case 2:
                        magic_altar.power_ups["祭坛升级"].value = 1.5;
                        magic_altar.speed_ups["祭坛升级"].value = 5;
                        break;
                    case 3:
                        magic_altar.power_ups["祭坛升级"].value = 2;
                        magic_altar.speed_ups["祭坛升级"].value = 30;
                        break;
                    case 4:
                        magic_altar.power_ups["祭坛升级"].value = 2.8;
                        magic_altar.speed_ups["祭坛升级"].value = 350;
                        break;
                    case 5:
                        magic_altar.power_ups["祭坛升级"].value = 4.5;
                        magic_altar.speed_ups["祭坛升级"].value = 7500;
                        break;
                }
            }
            #endregion
            if (u.name == "探索魔法")
            #region
            {
                switch (level)
                {
                    case 1:
                        block_producters["泥土方块"].unlocked = true;
                        res_table["方块"]["泥土方块"].unlocked = true;
                        ((Grid)(m.FindName("方块_泥土方块_grid"))).Visibility = 0;

                        upgrades["铲子"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_工具_铲子_grid"))).Visibility = 0;

                        unlocks.fight_unlock[1] = true;
                        find_name("战斗_场景_草原_grid").Visibility = 0;
                        break;
                    case 2:
                        block_producters["木头方块"].unlocked = true;
                        res_table["方块"]["木头方块"].unlocked = true;
                        ((Grid)(m.FindName("方块_木头方块_grid"))).Visibility = 0;

                        unlocks.fight_unlock[2] = true;
                        find_name("战斗_场景_死火山_grid").Visibility = 0;

                        visual_unlock("魔法_次_附魔_烈焰粉末_grid");
                        res_table["魔法"]["烈焰粉末"].unlocked = true;
                        enchants["烈焰粉末"].unlocked = true;

                        visual_unlock("魔法_次_药水_烈焰药水_grid");
                        enchants["烈焰药水"].unlocked = true;
                        break;
                    case 3:
                        block_producters["石头方块"].unlocked = true;
                        res_table["方块"]["石头方块"].unlocked = true;
                        ((Grid)(m.FindName("方块_石头方块_grid"))).Visibility = 0;

                        m.采矿.Visibility = 0;
                        minep.unlocked = true;

                        mine_regenerate();

                        upgrades["剑"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_工具_剑_grid"))).Visibility = 0;
                        upgrades["镐"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_工具_镐_grid"))).Visibility = 0;

                        res_table["采矿"]["采矿点数"].unlocked = true;
                        res_table["采矿"]["煤"].unlocked = true;
                        res_table["采矿"]["铜矿"].unlocked = true;

                        res_table["采矿"]["烤植物"].unlocked = true;
                        res_table["采矿"]["烤动物"].unlocked = true;
                        res_table["采矿"]["铜"].unlocked = true;

                        m.能量_grid.Visibility = 0;
                        unlocks.food = true;
                        m.制造_菜单_食物_grid.Visibility = 0;

                        upgrades["缤纷沙拉"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_食物_缤纷沙拉_grid"))).Visibility = 0;
                        upgrades["勇敢生肉套餐"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_食物_勇敢生肉套餐_grid"))).Visibility = 0;
                        upgrades["火爆蔬菜烧烤"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_食物_火爆蔬菜烧烤_grid"))).Visibility = 0;
                        upgrades["经典BBQ大餐"].unlocked = true;
                        ((Grid)(m.FindName("制造_次_食物_经典BBQ大餐_grid"))).Visibility = 0;

                        visual_unlock("魔法_次_药水_幸运药水_grid");
                        enchants["幸运药水"].unlocked = true;
                        break;
                    case 4:
                        unlocks.fight_unlock[3] = true;
                        find_name("战斗_场景_机关屋_grid").Visibility = 0;
                        unlocks.fight_unlock[4] = true;
                        find_name("战斗_场景_魔境_grid").Visibility = 0;
                        break;
                }
                if (prestige_ups["战斗探索"].level >= 1)
                {
                    foreach (KeyValuePair<string, enemy> kp in enemies["洁白世界"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 5;
                    }
                }
                if (prestige_ups["战斗探索"].level >= 2)
                {
                    foreach (KeyValuePair<string, enemy> kp in enemies["草原"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 3;
                    }
                    foreach (KeyValuePair<string, enemy> kp in enemies["死火山"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 3;
                    }
                }
            }
            #endregion
            if (u.name == "法术创作")
            #region
            {
                switch (level)
                {
                    case 1:
                        spell_unlock("白色魔法");
                        break;
                    case 2:
                        unlocks.potion = true;
                        m.魔法_菜单_药水_grid.Visibility = 0;
                        break;
                    case 3:
                        spell_unlock("绿色魔法");
                        break;
                    case 4:
                        spell_unlock("红色魔法");
                        break;
                    case 5:
                        visual_unlock("魔法_次_附魔_魔法糖浆_grid");
                        res_table["魔法"]["魔法糖浆"].unlocked = true;
                        enchants["魔法糖浆"].unlocked = true;
                        break;
                    case 6:
                        spell_unlock("橙色魔法");
                        break;
                    case 7:
                        spell_unlock("蓝色魔法");
                        break;
                    case 8:
                        spell_unlock("无色魔法");
                        //挖掘魔法
                        break;
                }
            }
            #endregion

            //挖矿
            if (u.name == "熔炉升级")
            #region
            {
                switch (level)
                {
                    case 1:
                        break;
                    case 2:
                        furance.y_speed_factor *= 2;
                        break;
                    case 3:
                        furance.fire_min *= 100;  //100 -> 10000
                        break;
                    case 4:
                        furance.y_speed_factor *= 2;
                        break;
                    case 5:
                        furance.y_speed_factor *= 2.5;
                        break;
                    case 6:
                        furance.x_speed_factor *= 4;
                        break;
                    case 7:
                        furance.y_speed_factor *= 2.5;
                        break;
                    case 8:
                        furance.fire_drop /= 2;
                        break;
                    case 9:
                        furance.y_speed_factor *= 2;
                        break;
                    case 10:
                        furance.y_speed_factor *= 2;
                        furance.x_speed_factor *= 2;
                        break;
                    case 11:
                        furance.fire_drop /= 5;
                        furance.fire_min *= 1e7;  //1e4 -> 1e11
                        break;
                    case 12:
                        furance.y_speed_factor *= 3;
                        break;
                    case 13:
                        furance.y_speed_factor *= 1.5;
                        furance.x_speed_factor *= 2.5;
                        break;
                    case 14:
                        furance.y_speed_factor *= 2.25;
                        break;
                    case 15:
                        furance.y_speed_factor *= 3;
                        break;
                }
                furance.level = level;
            }

            #endregion
        }

        private void buy_prestige_upgrade(string name)
        {
            prestige_upgrade p = prestige_ups[name];
            string res_name = p.cost_res_type;
            resource res = find_resource(res_name);

            double2 have = res.get_value();
            double2 need = p.cost[(int)p.level];
            if (have < need)
            {
                return;
            }
            res.add_value(-need);
            p.level++;

            link lk = null;
            if (p.name == "对数增益")
            #region 
            {
                lk = links["对数增益_生成器"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["生成器"].unlocked = true;
                    //引出接下来的升级（及链接）;
                    m.转生_升级_资源保留_grid.Visibility = 0;
                    links["生成器_资源保留"].unlock();

                }
            }
            #endregion
            else if (p.name == "生成器")
            #region
            {
                lk = links["生成器_资源保留"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["资源保留"].unlocked = true;
                    //引出接下来的升级（及链接）;
                    m.转生_升级_升级保留_grid.Visibility = 0;
                    links["资源保留_升级保留"].unlock();
                }
            }
            #endregion
            else if (p.name == "资源保留")
            #region
            {
                lk = links["资源保留_升级保留"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["升级保留"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                }
            }
            #endregion
            else if (p.name == "升级保留")
            #region
            {
                if (p.level == 1)
                {
                    foreach (KeyValuePair<string, block_producter> kp in block_producters)
                    {
                        block_producter bp = kp.Value;
                        decimal target = 0;
                        target = decimal.Floor(bp.best * (decimal)0.75);
                        target = Math.Max(target, bp.level);
                        方块生产器升级(bp, target - bp.level);
                    }
                }
                if (p.level == 2)
                {
                    foreach (KeyValuePair<string, upgrade> kp in upgrades)
                    {
                        upgrade u = kp.Value;
                        if (!(u is spell))
                        {
                            u.reseter.level = u.best - 1;
                            buy_upgrade_no_cost(u, u.reseter.level);
                        }
                    }
                }
                if (p.level == 3)
                {
                    foreach (KeyValuePair<string, block_producter> kp in block_producters)
                    {
                        block_producter bp = kp.Value;
                        bp.reseter.level = 0;
                        decimal target = 0;
                        target = decimal.Floor(bp.best * (decimal)0.75);
                        target = Math.Max(target, bp.level);
                        方块生产器升级(bp, target - bp.level);
                    }

                    foreach (KeyValuePair<string, spell> kp in spells)
                    {
                        spell u = kp.Value;
                        if (u.can_reset)
                        {
                            u.reseter.level = u.best;
                            buy_upgrade_no_cost(u, u.reseter.level);
                        }
                    }
                }

            }
            #endregion
            else if (p.name == "制造")
            #region
            {
                if (p.level == 1)
                {
                    m.制造.Visibility = 0;
                    res_table["制造"]["白色粉末"].unlocked = true;
                    upgrades["白色粉末"].unlocked = true;
                }
                else if(p.level == 2)
                {
                    ((Grid)(m.FindName("制造_次_材料_糖浆_grid"))).Visibility = 0;
                    res_table["制造"]["糖浆"].unlocked = true;
                    upgrades["糖浆"].unlocked = true;

                    upgrades["药水消耗降低"].unlocked = true;
                    find_name("制造_次_升级器_药水消耗降低_grid").Visibility = 0;

                    if (!enemies["洁白世界"]["糖"].drop.ContainsKey("糖方块"))
                    {
                        enemies["洁白世界"]["糖"].drop.Add("糖方块", new Tuple<double2, double2>(0.01, 2.6));
                        enemies["洁白世界"]["糖"].set_level(enemies["洁白世界"]["糖"].level);
                    }
                }
                else if (p.level == 3)
                {
                    upgrades["斧"].unlocked = true;
                    ((Grid)(m.FindName("制造_次_工具_斧_grid"))).Visibility = 0;

                    ((Grid)(m.FindName("制造_次_材料_植物祭品_grid"))).Visibility = 0;
                    res_table["制造"]["植物祭品"].unlocked = true;
                    upgrades["植物祭品"].unlocked = true;
                    ((Grid)(m.FindName("制造_次_材料_动物祭品_grid"))).Visibility = 0;
                    res_table["制造"]["动物祭品"].unlocked = true;
                    upgrades["动物祭品"].unlocked = true;
                }
                else if (p.level == 4)
                {
                    upgrades["魔杖"].unlocked = true;
                    ((Grid)(m.FindName("制造_次_工具_魔杖_grid"))).Visibility = 0;

                    upgrades["能量饮料"].unlocked = true;
                    find_name("制造_次_食物_能量饮料_grid").Visibility = 0;
                    upgrades["冰镇果汁"].unlocked = true;
                    find_name("制造_次_食物_冰镇果汁_grid").Visibility = 0;

                    ((Grid)(m.FindName("制造_次_材料_魔法粉末_grid"))).Visibility = 0;
                    upgrades["魔法粉末"].unlocked = true;
                }


                lk = links["制造_方块增幅"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["方块增幅"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                    m.转生_升级_魔法增幅_grid.Visibility = 0;
                    links["方块增幅_魔法增幅"].unlock();
                    m.转生_升级_时间力量_grid.Visibility = 0;
                    links["方块增幅_时间力量"].unlock();
                }

                lk = links["制造_战斗增幅"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["战斗增幅"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                    m.转生_升级_强化等级_grid.Visibility = 0;
                    links["战斗增幅_强化等级"].unlock();
                }

                lk = links["制造_核心"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["核心"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                }
            }
            #endregion
            else if (p.name == "方块增幅")
            #region
            {
                if (p.level == 1)
                {
                    block_producters["白色方块"].multiply_not_reset(1, 3);
                }
                else if (p.level == 2)
                {
                    block_producters["白色方块"].multiply_not_reset(1 / 2.0, 1);
                }
                else if (p.level == 3)
                {
                    block_producters["白色方块"].multiply_not_reset(1 / 1.5, 2);
                }
                else if (p.level == 4)
                {
                    upgrades["泥土方块生产"].unlocked = true;
                    find_name("制造_次_升级器_泥土方块生产_grid").Visibility = 0;

                    block_producters["糖方块"].unlocked = true;
                    res_table["方块"]["糖方块"].unlocked = true;
                    ((Grid)(m.FindName("方块_糖方块_grid"))).Visibility = 0;
                    if (!enemies["洁白世界"]["糖"].drop.ContainsKey("糖方块"))
                    {
                        enemies["洁白世界"]["糖"].drop.Add("糖方块", new Tuple<double2, double2>(0.01, 2.6));
                        enemies["洁白世界"]["糖"].set_level(enemies["洁白世界"]["糖"].level);
                    }

                    block_producters["白色方块"].multiply_not_reset(1, 1.5);
                }
                else if (p.level == 5)
                {
                    block_producters["白色方块"].multiply_not_reset(1 / 5.0, 1);
                }

                lk = links["方块增幅_魔法增幅"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["魔法增幅"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                    m.转生_升级_采矿增幅_grid.Visibility = 0;
                    links["魔法增幅_采矿增幅"].unlock();

                    m.转生_升级_转化_grid.Visibility = 0;
                    links["魔法增幅_转化"].unlock();
                }

                lk = links["方块增幅_时间力量"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["时间力量"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                }
            }
            #endregion
            else if (p.name == "时间力量")
            #region
            {
            }
            #endregion
            else if (p.name == "魔法增幅")
            #region
            {
                if (p.level == 1)
                {
                    foreach (KeyValuePair<string, spell> kp in spells)
                    {
                        spell s = kp.Value;
                        if (!s.cost_downs.ContainsKey("魔法增幅"))
                        {
                            s.cost_downs.Add("魔法增幅", new multiplier(false, 1));
                        }
                        s.cost_downs["魔法增幅"].value = 1.5;
                        s.add_time_mul("魔法增幅", 1.5, false);
                    }
                }
                else if (p.level == 2)
                {
                    foreach (KeyValuePair<string, enchant> kp in enchants)
                    {
                        enchant ec = kp.Value;
                        if (ec.is_potion)
                        {
                            if (!ec.speed_ups.ContainsKey("魔法增幅"))
                            {
                                ec.speed_ups.Add("魔法增幅", new multiplier(false, 1));
                            }
                            ec.speed_ups["魔法增幅"].value = 3.0;
                        }
                    }
                }
                else if (p.level == 3)
                {
                    foreach (KeyValuePair<string, spell> kp in spells)
                    {
                        spell s = kp.Value;
                        if (!s.cost_downs.ContainsKey("魔法增幅"))
                        {
                            s.cost_downs.Add("魔法增幅", new multiplier(false, 1));
                        }
                        s.cost_downs["魔法增幅"].value = 3;
                        s.add_time_mul("魔法增幅", 3, false);
                    }
                    foreach (KeyValuePair<string, enchant> kp in enchants)
                    {
                        enchant ec = kp.Value;
                        if (ec.is_potion)
                        {
                            if (!ec.effect_ups.ContainsKey("魔法增幅"))
                            {
                                ec.effect_ups.Add("魔法增幅", new multiplier(false, 1));
                            }
                            ec.effect_ups["魔法增幅"].value = 1.5;
                        }
                    }
                }

                lk = links["魔法增幅_采矿增幅"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["采矿增幅"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                }

                lk = links["魔法增幅_转化"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["转化"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                }
            }
            #endregion
            else if (p.name == "转化")
            #region
            {
                if (p.level == 1)
                {
                    magic_altar.speed_ups.Add("转化", new multiplier(true, 1));
                    foreach(KeyValuePair<string, enchant> kp in enchants)
                    {
                        if (kp.Value.is_potion)
                        {
                            enchant e = kp.Value;
                            e.speed_ups.Add("转化", new multiplier(true, 1));
                        }
                    }
                }
                else if (p.level == 2)
                {
                    res_table["转生"]["药水值"].unlocked = true;

                    res_table["转生"]["转生点数"].multipliers.Add("转化", new multiplier(true, 1));
                    res_table["魔法"]["魔力"].multipliers.Add("转化", new multiplier(true, 1));
                }
                else if (p.level == 3)
                {

                }
            }
            #endregion
            else if (p.name == "采矿增幅")
            #region
            {
                if (p.level == 1)
                {
                    minep.exp_boost += 2;
                    minep.reseter.exp_boost += 2;
                    minep.size_boost += 200;
                    minep.reseter.size_boost += 200;
                }
                else if (p.level == 2)
                {
                    furance.y_speed_factor *= 2.5;
                    furance.reseter.y_speed_factor *= 2.5;
                }
                else if (p.level == 3)
                {
                    if (!minep.point_multi[2].ContainsKey("采矿增幅"))
                    {
                        minep.point_multi[2].Add("采矿增幅", new multiplier(false, 1));
                    }
                    minep.point_multi[2]["采矿增幅"].value = 1.5;
                    minep.size_boost += 500;
                    minep.reseter.size_boost += 500;

                    minep.reseter.level = 20;
                    minep.to_level(20);
                }
                else if (p.level == 4)
                {
                    furance.x_speed_factor *= 3;
                    furance.reseter.x_speed_factor *= 3;

                    furance.y_speed_factor *= 2;
                    furance.reseter.y_speed_factor *= 2;
                }
                else if (p.level == 5)
                {
                    if (!minep.luck_multi[2].ContainsKey("采矿增幅"))
                    {
                        minep.luck_multi[2].Add("采矿增幅", new multiplier(false, 1));
                    }
                    minep.luck_multi[2]["采矿增幅"].value = 2;
                    minep.exp_boost += 13;
                    minep.reseter.exp_boost += 13;

                    minep.reseter.level = 40;
                    minep.to_level(40);
                }
            }
            #endregion
            else if (p.name == "战斗增幅")
            #region
            {
                if (p.level == 1)
                {
                    you.base_attack = 25;
                    you.reseter.base_attack = 25;
                    you.sr_base = 0.2;
                    you.reseter.sr_base = 0.2;
                }
                else if (p.level == 2)
                {
                    you.sr_factor *= 1.5;
                    you.reseter.sr_factor *= 1.5;
                }
                else if (p.level == 3)
                {
                    you.base_attack = 75;
                    you.reseter.base_attack = 75;
                    you.other_at_factor *= 1.5;
                    you.reseter.other_at_factor *= 1.5;
                }
                else if (p.level == 4)
                {
                    you.other_attack_factor.apply("战斗增幅", 1.25, false);
                    you.base_attack = 200;
                    you.reseter.base_attack = 200;
                    you.sr_base = 1;
                    you.reseter.sr_base = 1;
                }
                else if (p.level == 5)
                {
                    you.other_attack_factor.apply("战斗增幅", 2, false);
                    you.sr_factor *= 2;
                    you.reseter.sr_factor *= 2;
                }
                else if (p.level == 6)
                {
                    you.base_attack = 500;
                    you.reseter.base_attack = 500;
                    you.sr_base = 4;
                    you.reseter.sr_base = 4;
                }
                else if (p.level == 7)
                {
                    you.exp_need_base /= 7;
                    you.reseter.exp_need_base /= 7;
                    you.reseter.level = 150;
                    you.to_level(150);
                }
                else if (p.level == 8)
                {
                    you.base_attack = 1500;
                    you.reseter.base_attack = 1500;
                    you.other_attack_factor.apply("战斗增幅", 4, false);
                    you.other_at_factor *= 1.2;
                    you.reseter.other_at_factor *= 1.2;
                }
                else if (p.level == 9)
                {
                    you.base_attack = 5000;
                    you.reseter.base_attack = 5000;
                }
                else if (p.level == 10)
                {
                    you.sr_base = 30;
                    you.reseter.sr_base = 30;
                    you.other_attack_factor.apply("战斗增幅", 6.4, false);
                }
                else if (p.level == 11)
                {
                    you.other_at_factor *= 1.66666666;
                    you.reseter.other_at_factor *= 1.66666666;
                    you.reseter.level = 300;
                    you.to_level(300);
                }

                lk = links["战斗增幅_强化等级"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["强化等级"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                    m.转生_升级_战斗探索_grid.Visibility = 0;
                    links["强化等级_战斗探索"].unlock();
                }
            }
            #endregion
            else if (p.name == "强化等级")
            #region
            {
                if (p.level == 1)
                {
                    you.level_attack_increment = 2;
                    you.reseter.level_attack_increment = 2;
                }
                else if (p.level == 2)
                {
                    you.level_sr_increment = 0.02;
                    you.reseter.level_sr_increment = 0.02;
                }
                else if (p.level == 3)
                {
                    you.level_attack_exponent = 1.11;
                    you.reseter.level_attack_exponent = 1.11;
                }
                else if (p.level == 4)
                {
                    you.level_attack_increment = 5;
                    you.reseter.level_attack_increment = 5;
                }
                else if (p.level == 5)
                {
                    //每级增加战利品 0.3%
                }
                else if (p.level == 6)
                {
                    you.level_sr_increment = 0.05;
                    you.reseter.level_sr_increment = 0.05;
                }
                else if (p.level == 7)
                {
                    //每级减少药水配置时间1%
                }
                else if (p.level == 8)
                {
                    you.level_attack_increment = 30;
                    you.reseter.level_attack_increment = 30;
                    minep.size_exponent = 1.11;
                    minep.reseter.size_exponent = 1.11;
                }
                else if (p.level == 9)
                {
                    you.level_sr_increment = 0.15;
                    you.reseter.level_sr_increment = 0.15;
                    minep.luck_addition = 0.15;
                    minep.reseter.luck_addition = 0.15;
                    you.level_attack_exponent = 1.115;
                    you.reseter.level_attack_exponent = 1.115;

                    double2 fyg = minep.level / 10;
                    double2 fy_gain = new double2(double_floor(fyg.d), fyg.i);
                    furance.y_speed_factor *= double2.Pow(1.25, fy_gain);
                }

                lk = links["强化等级_战斗探索"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["战斗探索"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                    m.转生_升级_冷静_grid.Visibility = 0;
                    links["战斗探索_冷静"].unlock();
                }
            }
            #endregion
            else if (p.name == "战斗探索")
            #region
            {
                if (p.level == 1)
                {
                    foreach (KeyValuePair<string, enemy> kp in enemies["洁白世界"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 5 * find_upgrade("探索魔法").level;
                    }
                }
                if (p.level == 2)
                {
                    foreach (KeyValuePair<string, enemy> kp in enemies["草原"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 3 * find_upgrade("探索魔法").level;
                    }
                    foreach (KeyValuePair<string, enemy> kp in enemies["死火山"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 3 * find_upgrade("探索魔法").level;
                    }
                }
                if (p.level == 3)
                {
                    foreach (KeyValuePair<string, enemy> kp in enemies["洁白世界"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 10;
                    }
                    foreach (KeyValuePair<string, enemy> kp in enemies["草原"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 6;
                    }
                    foreach (KeyValuePair<string, enemy> kp in enemies["死火山"])
                    {
                        enemy e = kp.Value;
                        e.max_level += 6;
                    }
                }
                lk = links["战斗探索_冷静"];
                lk.update_progress(p.level);
                if (lk.complete)
                {
                    prestige_ups["冷静"].unlocked = true;
                    //引出接下来的升级（及链接） 设定vis;
                }
            }
            #endregion
            else if (p.name == "冷静")
            #region
            {
                if (p.level == 1)
                {
                    you.sr_factor /= 200;
                    you.reseter.sr_factor /= 200;
                }
                if (p.level == 2)
                {
                    you.other_at_factor /= 7.0;
                    you.reseter.other_at_factor /= 7.0;
                    you.attack_repeat *= 10;
                    you.reseter.attack_repeat *= 10;
                }
                if (p.level == 3)
                {
                    foreach(KeyValuePair<string, enchant> kp in enchants)
                    {
                        if (kp.Value.is_potion)
                        {
                            continue;
                        }
                        kp.Value.speed_ups.Add("冷静", new multiplier(false, 0.1));

                        kp.Value.cost_level_down += 10;
                        kp.Value.reseter.cost_level_down += 10;

                        kp.Value.max_level += 50;
                        kp.Value.reseter.max_level += 50;
                    }
                }
            }
            #endregion
            else if (p.name == "成就加成")
            #region
            {
                if (p.level == 1)
                {
                    resource a = find_resource("白色方块");
                    if (!a.multipliers.ContainsKey("成就加成"))
                    {
                        a.multipliers.Add("成就加成", new multiplier(false, 1));
                    }
                    a.multipliers["成就加成"].value = 1 + 0.01 * 成就点数.get_value();


                    a = find_resource("白色粉末");
                    if (!a.multipliers.ContainsKey("成就加成"))
                    {
                        a.multipliers.Add("成就加成", new multiplier(false, 1));
                    }
                    a.multipliers["成就加成"].value = 1 + 0.01 * 成就点数.get_value();
                }
                if (p.level == 2)
                {
                    if (!minep.exp_multi[2].ContainsKey("成就加成"))
                    {
                        minep.exp_multi[2].Add("成就加成", new multiplier(false, 1));
                    }
                    minep.exp_multi[2]["成就加成"].value = 1 + 0.005 * 成就点数.get_value();
                }
            }
            #endregion
        }
    }
}
