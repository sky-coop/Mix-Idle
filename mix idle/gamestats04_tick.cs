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
        Stopwatch stopwatch = new Stopwatch();
        bool first_watch = true;

        double2 save_count = 0;
        double2 auto_save_time = 30;

        double2 achieve_count = 0;
        double2 achieve_time = 0.5;

        //
        double2 time_tick_actually = 0;
        double2 time_this_prestige_actually = 0;
        double2 time_all_acutally = 0;

        double2 time_a_tick_game = 0;
        double2 time_this_prestige_game = 0;
        double2 time_all_game = 0;

        List<double2> fps_container = new List<double2>();
        int fps_container_pointer = 0;

        double2 g2_tick_counter = 0;

        //tick
        private void game_tick(object sender, EventArgs e)
        {
            time_start();
            if (pulse_t != 0)
            {
                time_tick_actually = pulse_t;
                pulse_t = 0;
                pulsing = true;
            }
            else
            {
                if (first_watch)
                {
                    stopwatch.Start();
                    time_tick_actually = 0;
                    first_watch = false;
                }
                else
                {
                    stopwatch.Stop();
                    time_tick_actually = stopwatch.ElapsedMilliseconds / 1000.0;
                    stopwatch.Restart();
                }
                pulsing = false;
            }

            if(ex.cost_mul == 0)
            {
                throw new Exception();
            }

            energy_tick();

            time_a_tick_game = time_tick_actually * gamespeed();
            time_this_prestige_game += time_a_tick_game;
            time_all_game += time_a_tick_game;

            time_this_prestige_actually = time_tick_actually;
            time_all_acutally += time_tick_actually;
            time_active_actually += time_tick_actually;

            if (!pulsing)
            {
                if (fps_container.Count < 10)
                {
                    fps_container.Add(time_tick_actually);
                }
                else
                {
                    fps_container[fps_container_pointer] = time_tick_actually;
                    fps_container_pointer = (fps_container_pointer + 1) % 10;
                }
                double2 avg_time = 0;
                foreach (double2 time in fps_container)
                {
                    avg_time += time;
                }
                avg_time /= fps_container.Count;

                if (avg_time != 0 && fps_container_pointer % 5 == 0)
                {
                    m.FPS.Text = "FPS: " + number_format(double_floor(1 / avg_time.d));
                }
            }

            if (s_ticker("unlock", 0.5))
            {
                unlock();
            }
            check();
            
            auto_produce();
            
            block_produce();

            fight_tick();
            level_potion_time_reduce();

            transform_tick();
            altar_produce();
            spell_produce();
            enchant_produce();

            mine_tick();
            treasure_tick();
            heat();

            vm_tick();

            key_check();
            show();

            float_message_tick();

            if (!pulsing)
            {
                save_count += time_tick_actually;
                if (save_count >= auto_save_time)
                {
                    save_count = 0;
                    save();
                }

                achieve_count += time_tick_actually;
                if (achieve_count >= achieve_time)
                {
                    achieve_count = 0;
                    achieve_check();
                }
            }
        }

        private void unlock()
        {
            foreach (KeyValuePair<string, Dictionary<string, resource>> kp1 in res_table)
            {
                foreach (KeyValuePair<string, resource> kp2 in kp1.Value)
                {
                    if (kp2.Value.get_value() > 0)
                    {
                        kp2.Value.unlocked = true;
                    }
                    if (double.IsNaN(kp2.Value.get_value().d))
                    {
                        throw new Exception();
                    }
                }
            }
            foreach (KeyValuePair<string, treasure> kp in treasures)
            {
                if (kp.Value.get_value() > 0)
                {
                    kp.Value.unlocked = true;
                }
            }
        }

        private void debug_check()
        {
            
            IInputElement input = Mouse.DirectlyOver;
            if (input != null && input is FrameworkElement)
            {
                FrameworkElement f = (FrameworkElement)input;
                m.调试b.Text = f.Name;
            }
            else
            {
                m.调试b.Text = "null";
            }

            g2_area a = g2_current;
            if (a != null)
            {
                foreach (g2_ball b in a.balls)
                {
                    m.调试a.Text = number_format(b.ball_pos.X) + "\t  " + number_format(b.ball_pos.Y);
                }
            }
        }

        double2 old_minep_level = 0;
        private void check()
        {
            debug_check();
            if (prestige_ups["强化等级"].level >= 9)
            {
                double2 fy_gain = double_floor(minep.level.d / 10) - double_floor(old_minep_level.d / 10);
                furance.y_speed_factor *= double2.Pow(1.25, fy_gain);
                old_minep_level = minep.level;
            }

            if (bg_is_image)
            {
                m.Image_bg_grid.Visibility = Visibility.Visible;
            }
            else
            {
                m.Image_bg_grid.Visibility = Visibility.Hidden;
            }
        }

        //时间力量
        private double2 time_power()
        {
            prestige_upgrade p = prestige_ups["时间力量"];
            double2 tim_mul = 1;
            if (p.level == 1)
            {
                tim_mul += time_this_prestige_game / 100;
            }
            if (p.level == 2)
            {
                tim_mul += (time_this_prestige_game + 9900) / 100;
            }
            if (p.level == 3)
            {
                tim_mul += double2.Pow((time_this_prestige_game + 9900), 1.1) / 100;
            }
            return tim_mul;
        }

        //生成器的效果
        private void auto_produce()
        {
            prestige_upgrade p = prestige_ups["生成器"];

            if (p.level == 1)
            {
                res_table["方块"]["白色方块"].add_value(100 * time_a_tick_game * time_power());
            }
            else if (p.level == 2)
            {
                res_table["方块"]["白色方块"].add_value(500 * time_a_tick_game * time_power());
                res_table["制造"]["白色粉末"].add_value(5000 * time_a_tick_game * time_power());
            }
            else if (p.level == 3)
            {
                res_table["方块"]["白色方块"].add_value(3000 * time_a_tick_game * time_power());
                res_table["制造"]["白色粉末"].add_value(500000 * time_a_tick_game * time_power());
                res_table["魔法"]["魔力"].add_value(30 * time_a_tick_game * time_power());
            }
            else if (p.level == 4)
            {
                res_table["方块"]["白色方块"].add_value(3000 * time_a_tick_game * time_power());
                res_table["制造"]["白色粉末"].add_value(500000 * time_a_tick_game * time_power());
                res_table["战斗"]["白色粒子"].add_value(25 * time_a_tick_game * time_power());
                res_table["战斗"]["绿色粒子"].add_value(3 * time_a_tick_game * time_power());
                res_table["魔法"]["魔力"].add_value(30 * time_a_tick_game * time_power());
            }
            else if (p.level == 5)
            {
                res_table["方块"]["白色方块"].add_value(2e6 * time_a_tick_game * time_power());
                res_table["方块"]["糖方块"].add_value(10e6 * time_a_tick_game * time_power());
                res_table["制造"]["白色粉末"].add_value(3e12 * time_a_tick_game * time_power());
                res_table["战斗"]["白色粒子"].add_value(25 * time_a_tick_game * time_power());
                res_table["战斗"]["绿色粒子"].add_value(3 * time_a_tick_game * time_power());
                res_table["魔法"]["魔力"].add_value(7000 * time_a_tick_game * time_power());
            }
            else if (p.level == 6)
            {
                res_table["方块"]["白色方块"].add_value(10e9 * time_a_tick_game * time_power());
                res_table["方块"]["糖方块"].add_value(10e6 * time_a_tick_game * time_power());
                res_table["制造"]["白色粉末"].add_value(3e12 * time_a_tick_game * time_power());
                res_table["战斗"]["白色粒子"].add_value(4e3 * time_a_tick_game * time_power());
                res_table["战斗"]["绿色粒子"].add_value(400 * time_a_tick_game * time_power());
                res_table["战斗"]["红色粒子"].add_value(100 * time_a_tick_game * time_power());
                res_table["战斗"]["橙色粒子"].add_value(30 * time_a_tick_game * time_power());
                res_table["战斗"]["蓝色粒子"].add_value(12 * time_a_tick_game * time_power());
                res_table["战斗"]["无色粒子"].add_value(6 * time_a_tick_game * time_power());
                res_table["魔法"]["魔力"].add_value(100e3 * time_a_tick_game * time_power());
            }
            else if (p.level == 7)
            {
                res_table["方块"]["白色方块"].add_value(3e12 * time_a_tick_game * time_power());
                res_table["方块"]["糖方块"].add_value(500e12 * time_a_tick_game * time_power());
                res_table["制造"]["白色粉末"].add_value(3e12 * time_a_tick_game * time_power());
                res_table["战斗"]["白色粒子"].add_value(4e3 * time_a_tick_game * time_power());
                res_table["战斗"]["绿色粒子"].add_value(400 * time_a_tick_game * time_power());
                res_table["战斗"]["红色粒子"].add_value(100 * time_a_tick_game * time_power());
                res_table["战斗"]["橙色粒子"].add_value(30 * time_a_tick_game * time_power());
                res_table["战斗"]["蓝色粒子"].add_value(12 * time_a_tick_game * time_power());
                res_table["战斗"]["无色粒子"].add_value(6 * time_a_tick_game * time_power());
                res_table["魔法"]["魔力"].add_value(100e3 * time_a_tick_game * time_power());
                res_table["采矿"]["铜"].add_value(15e6 * time_a_tick_game * time_power());
                res_table["采矿"]["铁"].add_value(5e6 * time_a_tick_game * time_power());
            }
        }

        private void block_produce()
        {
            foreach (KeyValuePair<string, block_producter> kp in block_producters)
            {
                block_producter bp = kp.Value;
                resource r = find_resource(bp.name);
                if (bp.unlocked)
                {
                    bp.current_time += time_a_tick_game;
                    bp.current_value = bp.max_value * double2.Pow((bp.current_time / bp.max_time), bp.production_exponent);
                    if (current_group == "方块")
                    {
                        Grid main = (Grid)m.FindName("方块_" + bp.name + "_grid");
                        foreach (FrameworkElement f in main.Children)
                        {
                            if (f.Name.Contains("等级"))
                            {
                                TextBlock t = f as TextBlock;
                                t.Text = bp.name + " 生产器 等级" + number_format(bp.level);
                            }
                            if (f.Name.Contains("最大产量"))
                            {
                                TextBlock t = f as TextBlock;
                                t.Text = "最大产量：" + number_format(bp.max_value * r.get_mul() * time_power());
                            }
                            if (f.Name.Contains("最大耗时"))
                            {
                                TextBlock t = f as TextBlock;
                                t.Text = "最大耗时：" + number_format(bp.max_time) + "s";
                            }
                            if (f.Name.Contains("当前产量"))
                            {
                                TextBlock t = f as TextBlock;
                                if (time_a_tick_game > bp.max_time)
                                {
                                    t.Text = "当前产量：" + number_format(bp.current_time / bp.max_time
                                        * bp.max_value * r.get_mul() * time_power());
                                }
                                else
                                {
                                    t.Text = "当前产量：" + number_format(bp.current_value * r.get_mul() * time_power());

                                }
                            }
                            if (f.Name.Contains("当前耗时"))
                            {
                                TextBlock t = f as TextBlock;
                                t.Text = "当前耗时：" + number_format(bp.current_time) + "s";
                            }
                            if (f.Name.Contains("进度条_顶"))
                            {
                                Rectangle t = f as Rectangle;
                                t.Width = Math.Min((bp.current_time / bp.max_time).d, 1) * 250;
                            }
                        }
                    }
                    if (bp.current_time >= bp.max_time)
                    {
                        方块生产器收集(bp);
                    }
                    bp.current_value = bp.max_value * double2.Pow((bp.current_time / bp.max_time), bp.production_exponent);
                }
            }
        }

        private void fight_tick()
        {
            you.update();
            you.slow_value -= you.get_sr() * time_a_tick_game;
            if (you.slow_value < you.slow_cap)
            {
                you.slow_value = you.slow_cap;
            }

            if (enemy.current != null)
            {
                enemy.current.curr_health += enemy.current.regen * time_a_tick_game;
                if (enemy.current.curr_health > enemy.current.health)
                {
                    enemy.current.curr_health = enemy.current.health;
                }
            }

            bool show = current_group == "战斗";

            if (fighting)
            {
                you.attack_progress += time_a_tick_game;
                double2 cn = you.attack_progress / you.get_attack_time();
                double2 count = cn.floor();
                if (show)
                {
                    m.战斗_玩家_攻击_text.Text = "攻击时间: " + number_format(you.attack_progress) + "s / " + number_format(you.get_attack_time()) + "s";
                    m.战斗_玩家_攻击_进度条_顶.Width = m.战斗_玩家_攻击_进度条_底.Width * Math.Min(((you.attack_progress / you.get_attack_time())).d, 1);
                }
                you.attack_progress -= you.get_attack_time() * count;
                attack(you.auto_attack_form, count);
                if(you.attack_progress < 0)
                {
                    you.attack_progress = 0;
                }
            }
            else
            {
                if (you.attack_progress > you.get_attack_time())
                {
                    you.attack_progress = you.get_attack_time();
                }
            }

            while (die_time_spot.Count > 0 && die_time_spot[0].Item1 < (time_all_acutally - 10))
            {
                die_time_spot.RemoveAt(0);
            }

            
            Rectangle cover = null;
            Rectangle bg = null;
            Rectangle bg2 = null;
            TextBlock txt = null;
            foreach (Grid g in m.战斗_玩家_攻击风格_手动_grid.Children)
            {
                string[] strs = g.Name.Split('_');
                string form_name = strs[4];
                string cover_name = "战斗_玩家_攻击风格_手动_" + form_name;

                if (show)
                {
                    cover = (Rectangle)m.FindName(cover_name);
                    bg = (Rectangle)m.FindName(cover_name + "_背景");
                    bg2 = (Rectangle)m.FindName(cover_name + "_背景2");
                    txt = (TextBlock)m.FindName(cover_name + "_文字");
                }

                attack_form af = attack_forms[form_name];
                if (!af.skill)
                {
                    af.mafcc_cur_time += time_a_tick_game;
                    if (show)
                    {
                        if (af.mafcc_cur_time >= af.manual_anti_fast_click_cd)
                        {
                            if (((SolidColorBrush)(bg.Fill)).Color.G == 0)
                            {
                                bg.Fill = getSCB(Color.FromRgb(0, 0, 0));
                                txt.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                            }
                            else
                            {
                                bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                                txt.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                            }
                        }
                        else
                        {
                            if (((SolidColorBrush)(bg.Fill)).Color.G == 225)
                            {
                            }
                            else
                            {
                                bg.Fill = getSCB(Color.FromRgb(0, 0, 180));
                                txt.Foreground = getSCB(Color.FromRgb(200, 225, 180));
                            }
                        }
                    }
                }

                if (af.skilling)
                {
                    af.shine_time = af.attack_time / 7.0;

                    af.attack_progress += double2.Min(af.skill_time_current, time_a_tick_game);
                    double2 xn = af.attack_progress / af.attack_time;
                    double2 count = xn.floor();
                    if (fighting)
                    {
                        attack(af, count);
                    }
                    if (show)
                    {
                        cover.Tag = disable;
                        bg.Fill = getSCB(Color.FromRgb(0, 200, 200));
                        bg2.Fill = getSCB(Color.FromArgb(127, 0, 200, 0));
                        txt.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    }
                    af.attack_progress -= af.attack_time * count;

                    af.shine_progress += time_a_tick_game;
                    if (count > 0)
                    {
                        af.shine_progress = 0;
                    }
                    if (af.shine_progress <= af.shine_time)
                    {
                        if (show)
                        {
                            bg.Fill = getSCB(Color.FromRgb(0, 255, 255));
                            bg2.Fill = getSCB(Color.FromArgb(127, 0, 255, 0));
                            txt.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                        }
                    }

                    af.skill_time_current -= time_a_tick_game;
                    if (af.skill_time_current <= 0)
                    {
                        af.skill_time_current = 0;
                        af.skilling = false;

                        af.attack_progress = 0;

                        af.shine_progress = 0;

                        if (show)
                        {
                            cover.Tag = enable;
                            if (Mouse.DirectlyOver != null && Mouse.DirectlyOver.Equals(cover))
                            {
                                bg.Fill = getSCB(Color.FromRgb(0, 0, 0));
                                txt.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                            }
                            else
                            {
                                bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                                txt.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                            }
                        }
                    }

                    if (show)
                    {
                        bg2.Width = bg.Width * Math.Min(1, (af.skill_time_current / af.skill_time).d);
                    }
                }
            }
        }

        private void transform_tick()
        {
            prestige_upgrade p = prestige_ups["转化"];

            if (p.level >= 1)
            {
                magic_altar.speed_ups["转化"].value = get_magic_res_boost();
                foreach (KeyValuePair<string, enchant> kp in enchants)
                {
                    if (kp.Value.is_potion)
                    {
                        enchant e = kp.Value;
                        e.speed_ups["转化"].value = get_magic_res_boost();
                    }
                }
            }
            if (p.level >= 2)
            {
                res_table["转生"]["转生点数"].multipliers["转化"].value = get_potion_value_boost();
                res_table["魔法"]["魔力"].multipliers["转化"].value = get_potion_value_boost();
            }
        }

        private void enchant_produce()
        {
            foreach (KeyValuePair<string, enchant> kp in enchants)
            {
                enchant ec = kp.Value;
                
                ec.can_buy = true;
                double2 percent_max = new double2(1, double.MaxValue);
                Dictionary<resource, double2> temp = new Dictionary<resource, double2>();
                Tuple<string, double2> cost_temp = null;
                for (int i = 0; (cost_temp = ec.get_cost(i)) != null; i++)
                {
                    resource r = find_resource(cost_temp.Item1);
                    double2 cost = cost_temp.Item2;
                    if (r.get_value() < cost)
                    {
                        ec.can_buy = false;
                        break;
                    }
                    else
                    {
                        double2 ratio = r.get_value() / cost;
                        //percent_max取ratio最小值
                        if (percent_max > ratio)
                        {
                            percent_max = ratio;
                        }
                    }
                    temp.Add(r, cost);
                }

                if (ec.active)
                {
                    ec.current_time += time_a_tick_game;
                    if (ec.current_time >= ec.get_time())
                    {
                        double2 ratio = ec.current_time / ec.get_time();
                        //percent_max受限于ratio
                        if (ratio < percent_max)
                        {
                            percent_max = ratio;
                        }
                        double2 buy_am = percent_max;

                        if (ec.can_buy)
                        {
                            ec.current_time -= ec.get_time() * buy_am;
                            foreach (KeyValuePair<resource, double2> t in temp)
                            {
                                t.Key.add_value(-t.Value * buy_am);
                            }
                            if (ec.is_potion)
                            {
                                double2 potion_point = ec.level;
                                if (ec.name == "战斗经验药水")
                                {
                                    you.gain_exp(buy_am * ec.get_effect_mul() * 1.5e6 * double2.Pow(30, ec.level) *
                                        global_xp_boost());
                                    potion_point *= 2;
                                }
                                if (ec.name == "攻击药水")
                                {
                                    you.item_attack += buy_am * ec.get_effect_mul() * double2.Pow(1.5 + 0.1 * (double2)ec.level, 2);
                                    potion_point *= 4;
                                }
                                if (ec.name == "魔力药水")
                                {
                                    find_resource("魔力").add_value(buy_am * ec.get_effect_mul() * 1200 * double2.Pow(11, ec.level));
                                    potion_point *= 8;
                                }
                                if (ec.name == "烈焰药水")
                                {
                                    you.item_sr += (buy_am * ec.get_effect_mul() * double2.Pow(0.2 + 0.01 * (double2)ec.level, 2));
                                    potion_point *= 16;
                                }
                                if (ec.name == "幸运药水")
                                {
                                    minep.luck_boost += (buy_am * ec.get_effect_mul() * double2.Pow(0.03 + 0.0025 * (double2)ec.level, 1.5));
                                    potion_point *= 32;
                                }

                                if (prestige_ups["转化"].level >= 2)
                                {
                                    res_table["转生"]["药水值"].add_value(potion_point);
                                }
                                ec.craft_amount++;
                            }
                            else
                            {
                                find_resource(ec.produce_res).add_value(buy_am * ec.get_produce());
                            }
                        }
                        if (ec.current_time >= ec.get_time())
                        {
                            ec.current_time = ec.get_time();
                        }
                        if (ec.current_time < 0)
                        {
                            ec.current_time = 0;
                        }
                    }
                }
            }
        }

        private void altar_produce()
        {
            if (magic_altar.mode == "魔力祭坛")
            {
                double2 reduce = double2.Pow(magic_altar.power, magic_altar.mana_exponent) * time_a_tick_game * magic_altar.get_speed_mul();
                if (reduce > magic_altar.power)
                {
                    reduce = magic_altar.power;
                }
                magic_altar.power -= reduce;
                res_table["魔法"]["魔力"].add_value(reduce * magic_altar.mana_factor);
            }
        }

        Dictionary<string, double2> spell_cast_protection = new Dictionary<string, double2>();
        private void spell_produce()
        {
            spell_cast_protection = new Dictionary<string, double2>();
            foreach (KeyValuePair<string, upgrade> kp in upgrades)
            {
                if(kp.Value is spell)
                {
                    spell s = (spell)kp.Value;
                    if (s.cast_active && s.cost_table_active[s.current_active_lv] != null)
                    {
                        foreach (Tuple<string, double2> tuple in s.cost_table_active[s.current_active_lv])
                        {
                            if (!spell_cast_protection.ContainsKey(tuple.Item1))
                            {
                                spell_cast_protection.Add(tuple.Item1, 0);
                            }
                            spell_cast_protection[tuple.Item1] += tuple.Item2 * gamespeed();
                        }
                    }
                }
            }

            save_spell_uncast_boost = get_spell_uncast_boost();
            foreach (FrameworkElement f in m.魔法_菜单_法术_target_grid.Children)
            {
                Grid g = null;
                if(f is Grid)
                {
                    g = (Grid)f;
                }
                else
                {
                    continue;
                }
                string[] strs = g.Name.Split('_');
                spell s = null;
                if (strs[2].Substring(0, 1) == "第")
                {
                    foreach (Grid g2 in g.Children)
                    {
                        string[] strs2 = g2.Name.Split('_');
                        s = ((spell)find_upgrade(strs2[2]));
                        spell_show(s);
                    }
                }
                else
                {
                    s = ((spell)find_upgrade(strs[2]));
                    spell_show(s);
                }
                if (s == null)
                {
                    continue;
                }
            }
        }

        private void spell_show(spell s)
        {
            if (s == null)
            {
                return;
            }
            if (s.cast_active)
            {
                bool can_cast = true;
                List<Tuple<string, double2>> tuples = new List<Tuple<string, double2>>();
                if (s.cost_table_active[s.current_active_lv] != null)
                {
                    foreach (Tuple<string, double2> tuple in s.cost_table_active[s.current_active_lv])
                    {
                        tuples.Add(tuple);
                        resource r = find_resource(tuple.Item1);
                        double2 mul = 1;
                        if (r.name == "能量")
                        {
                            mul = ex.cost_mul;
                        }

                        if (r.get_value() < (time_a_tick_game * tuple.Item2 * mul))
                        {
                            can_cast = false;
                        }
                    }
                }
                else
                {
                    can_cast = false;
                }

                if (can_cast)
                {
                    foreach (Tuple<string, double2> tuple in tuples)
                    {
                        resource r = find_resource(tuple.Item1);

                        double2 mul = 1;
                        if (r.name == "能量")
                        {
                            mul = ex.cost_mul;
                        }

                        r.add_value(-time_a_tick_game * tuple.Item2 * mul);
                    }
                }
                else
                {
                    s.cast_active = false;
                    s.casting = false;
                }

            }
            if (s.normal)
            {
                if (s_ticker("spell_" + s.name, 0.2))
                {
                    active_spell_effect(s);
                }
            }

            if (s.study_active)
            {
                bool can_study = true;
                double2 percent = 0;
                double2 percent_min = double.MaxValue;

                double2 time = double2.Min(time_a_tick_game, s.get_time() - s.current_time);
                if (time == 0)
                {
                    if (s.current_time >= s.get_time())
                    {
                        s.current_time = 0;
                        s.level++;
                        upgrade_effect(s, s.level);

                        s.study_active = false;
                        s.studying = false;
                    }
                    return;
                }
                List<Tuple<string, double2>> tuples = new List<Tuple<string, double2>>();
                if (s.cost_table[s.level] != null)
                {
                    foreach (Tuple<string, double2> tuple in s.cost_table[s.level])
                    {
                        resource r = find_resource(tuple.Item1);
                        double2 mul = 1;
                        if (r.name == "能量")
                        {
                            mul = ex.cost_mul;
                        }

                        Tuple<string, double2> tuple_n = new Tuple<string, double2>(tuple.Item1, tuple.Item2 / s.get_cost_mul() * mul);
                        tuples.Add(tuple_n);

                        double2 protection = 0;
                        if (spell_cast_protection.ContainsKey(tuple_n.Item1))
                        {
                            protection = spell_cast_protection[tuple_n.Item1];
                        }

                        if (r.get_value() <= protection)
                        {
                            can_study = false;
                        }

                        //percent = 4魔力 / ((0.01s / 10s) * 5e3魔力) = 0.8; + 0.008
                        //percent = 4 / (0.002s / 10s) * 5000 = 4
                        //-> percent = 1

                        //percent = 100魔力 / ((0.05s / 10s) * 5e3魔力) = 4; + 0.05(25 mana)
                        //        = 100 / 5e-3 / 5e3 = 4
                        //-> percent = 1
                        percent = (r.get_value() - protection) / ((time / s.get_time()) * tuple_n.Item2);

                        if (percent < 0)
                        {
                            percent = 0;
                        }
                        if (percent > 1)
                        {
                            percent = 1;
                        }
                        if (percent < percent_min)
                        {
                            percent_min = percent;
                        }
                    }
                }
                else
                {
                    can_study = false;
                }
                if (can_study)
                {
                    foreach (Tuple<string, double2> tuple in tuples)
                    {
                        resource r = find_resource(tuple.Item1);
                        

                        r.add_value(-percent_min * ((time / s.get_time()) * tuple.Item2));

                    }
                    s.current_time += percent_min * time;
                    if (s.current_time >= s.get_time())
                    {
                        s.current_time = 0;
                        s.level++;
                        upgrade_effect(s, s.level);

                        s.study_active = false;
                        s.studying = false;
                        /*if (!s.normal)
                        {
                        }*/
                    }
                }
                else
                {
                    s.study_active = false;
                    s.studying = false;
                }

            }
        }

        private void active_spell_effect(spell s)
        {
            bool uncast_boost = prestige_ups["转化"].level >= 3;
            double2 uncast_effect = save_spell_uncast_boost;

            if (s.name == "白色魔法")
            {
                if (!res_table["方块"]["白色方块"].multipliers.ContainsKey("白色魔法"))
                {
                    res_table["方块"]["白色方块"].multipliers.Add("白色魔法", new multiplier(true, 1));
                }
                if (!res_table["制造"]["白色粉末"].multipliers.ContainsKey("白色魔法"))
                {
                    res_table["制造"]["白色粉末"].multipliers.Add("白色魔法", new multiplier(true, 1));
                }
                if (!res_table["方块"]["白色方块"].multipliers.ContainsKey("白色魔法被动"))
                {
                    res_table["方块"]["白色方块"].multipliers.Add("白色魔法被动", new multiplier(true, 1));
                }

                switch (s.current_active_lv)
                {
                    case 1:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 2;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 1;
                        break;
                    case 2:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 2.5;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 1.6;
                        break;
                    case 3:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 3;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 2;
                        break;
                    case 4:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 4;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 3;
                        break;
                    case 5:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 5.5;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 4;
                        break;
                    case 6:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 7;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 6;
                        break;
                    case 7:
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 10;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 8;
                        break;
                }

                if (!s.cast_active)
                {
                    if (uncast_boost)
                    {
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 
                            1 + (res_table["方块"]["白色方块"].multipliers["白色魔法"].value - 1) * uncast_effect;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value =
                            1 + (res_table["制造"]["白色粉末"].multipliers["白色魔法"].value - 1) * uncast_effect;
                    }
                    else
                    {
                        res_table["方块"]["白色方块"].multipliers["白色魔法"].value = 1;
                        res_table["制造"]["白色粉末"].multipliers["白色魔法"].value = 1;
                    }
                }

                switch (s.level)
                {
                    case 1:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 1.5;
                        break;
                    case 2:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 2;
                        break;
                    case 3:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 3;
                        break;
                    case 4:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 5;
                        break;
                    case 5:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 7;
                        break;
                    case 6:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 9;
                        break;
                    case 7:
                        res_table["方块"]["白色方块"].multipliers["白色魔法被动"].value = 12;
                        break;
                }
            }
            if (s.name == "绿色魔法")
            {
                if (!you.exp_gain_multipliers.ContainsKey("绿色魔法"))
                {
                    you.exp_gain_multipliers.Add("绿色魔法", new multiplier(true, 1));
                }
                if (!minep.exp_multi[2].ContainsKey("绿色魔法"))
                {
                    minep.exp_multi[2].Add("绿色魔法", new multiplier(true, 1));
                }
                foreach (KeyValuePair<string, upgrade> kp in upgrades)
                {
                    if (kp.Value is spell)
                    {
                        spell sp = (spell)(kp.Value);
                        sp.add_time_mul("绿色魔法", 1, true);
                    }
                }
                if (!res_table["转生"]["转生点数"].multipliers.ContainsKey("绿色魔法"))
                {
                    res_table["转生"]["转生点数"].multipliers.Add("绿色魔法", new multiplier(true, 1));
                }

                switch (s.current_active_lv)
                {
                    case 1:
                        you.exp_gain_multipliers["绿色魔法"].value = 2;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.add_time_mul("绿色魔法", 3, true);
                            }
                        }
                        break;
                    case 2:
                        you.exp_gain_multipliers["绿色魔法"].value = 4;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.add_time_mul("绿色魔法", 7.5, true);
                            }
                        }
                        break;
                    case 3:
                        you.exp_gain_multipliers["绿色魔法"].value = 9;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.add_time_mul("绿色魔法", 20, true);
                            }
                        }
                        break;
                    case 4:
                        you.exp_gain_multipliers["绿色魔法"].value = 30;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.add_time_mul("绿色魔法", 80, true);
                            }
                        }
                        break;
                    case 5:
                        you.exp_gain_multipliers["绿色魔法"].value = 75;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.add_time_mul("绿色魔法", 400, true);
                            }
                        }
                        minep.exp_multi[2]["绿色魔法"].value = 1.5;
                        break;
                }

                if (!s.cast_active)
                {
                    if (uncast_boost)
                    {
                        you.exp_gain_multipliers["绿色魔法"].value =
                            1 + (you.exp_gain_multipliers["绿色魔法"].value - 1) * uncast_effect;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.speed_ups["绿色魔法"].value =
                                    1 + (sp.speed_ups["绿色魔法"].value - 1) * uncast_effect;
                                sp.current_time /= uncast_effect;
                            }
                        }
                        minep.exp_multi[2]["绿色魔法"].value =
                            1 + (minep.exp_multi[2]["绿色魔法"].value - 1) * uncast_effect;
                    }
                    else
                    {
                        you.exp_gain_multipliers["绿色魔法"].value = 1;
                        foreach (KeyValuePair<string, upgrade> kp in upgrades)
                        {
                            if (kp.Value is spell)
                            {
                                spell sp = (spell)(kp.Value);
                                sp.add_time_mul("绿色魔法", 1, true);
                            }
                        }
                        minep.exp_multi[2]["绿色魔法"].value = 1;
                    }
                }

                switch (s.level)
                {
                    case 1:
                        res_table["转生"]["转生点数"].multipliers["绿色魔法"].value = 1.2;
                        break;
                    case 2:
                        res_table["转生"]["转生点数"].multipliers["绿色魔法"].value = 1.44;
                        break;
                    case 3:
                        res_table["转生"]["转生点数"].multipliers["绿色魔法"].value = 1.7;
                        break;
                    case 4:
                        res_table["转生"]["转生点数"].multipliers["绿色魔法"].value = 2;
                        break;
                    case 5:
                        res_table["转生"]["转生点数"].multipliers["绿色魔法"].value = 2.5;
                        break;
                }
            }

            if (s.name == "红色魔法")
            {
                if (!s.save.ContainsKey("减速值"))
                {
                    s.save.Add("减速值", 1);
                }
                if (!s.save.ContainsKey("燃料速度值"))
                {
                    s.save.Add("燃料速度值", 1);
                }
                if (!s.save.ContainsKey("方块生产器"))
                {
                    s.save.Add("方块生产器", 1);
                }
                

                double2 mod_attack = 1;
                double2 mod_sr = 1;
                double2 mod_y = 1;
                double2 mod_bp_time = 1;
                /*
                you.other_attack_factor /= old_attack;
                you.sr_factor /= old_sr;
                furance.y_speed_factor /= old_y;
                foreach (KeyValuePair<string, block_producter> kp in block_producters)
                {
                    block_producter bp = kp.Value;
                    bp.multiply(old_bp_time, 1);
                }*/

                switch (s.current_active_lv)
                {
                    case 1:
                        mod_attack = 2;
                        mod_sr = 1.25;
                        break;
                    case 2:
                        mod_attack = 4;
                        mod_sr = 1.5;
                        break;
                    case 3:
                        mod_attack = 8;
                        mod_sr = 2.1;
                        break;
                    case 4:
                        mod_attack = 32;
                        mod_sr = 2.7;
                        mod_y = 1.5;
                        break;
                }

                if (!s.cast_active)
                {
                    if (uncast_boost)
                    {
                        mod_attack = 1 + (mod_attack - 1) * uncast_effect;
                        mod_sr = 1 + (mod_sr - 1) * uncast_effect;
                        mod_y = 1 + (mod_y - 1) * uncast_effect;
                    }
                    else
                    {
                        mod_attack = 1;
                        mod_sr = 1;
                        mod_y = 1;
                    }
                }

                you.other_attack_factor.apply("红色魔法", mod_attack, true);
                you.sr_factor *= mod_sr / s.save["减速值"];
                furance.y_speed_factor *= mod_y / s.save["燃料速度值"];
                s.save["攻击"] = mod_attack;
                s.save["减速值"] = mod_sr;
                s.save["燃料速度值"] = mod_y;

                
                switch (s.level)
                {
                    case 1:
                        mod_bp_time = 2;
                        break;
                    case 2:
                        mod_bp_time = 4;
                        break;
                    case 3:
                        mod_bp_time = 10;
                        break;
                    case 4:
                        mod_bp_time = 25;
                        break;
                }

                foreach (KeyValuePair<string, block_producter> kp in block_producters)
                {
                    block_producter bp = kp.Value;
                    bp.multiply(s.save["方块生产器"] / mod_bp_time, 1);
                }
                s.save["方块生产器"] = mod_bp_time;

            }
            if (s.name == "橙色魔法")
            {
                if (!minep.point_multi[2].ContainsKey("橙色魔法"))
                {
                    minep.point_multi[2].Add("橙色魔法", new multiplier(true, 1));
                }
                if (!minep.size_multi[2].ContainsKey("橙色魔法"))
                {
                    minep.size_multi[2].Add("橙色魔法", new multiplier(true, 1));
                }
                if (!s.save.ContainsKey("方块生产器产量"))
                {
                    s.save.Add("方块生产器产量", 1);
                }
                
                double2 mod_bp_p = 1;

                switch (s.current_active_lv)
                {
                    case 1:
                        mod_bp_p = 3;
                        minep.point_multi[2]["橙色魔法"].value = 1.5;
                        you.pierce_percent = 0;
                        break;
                    case 2:
                        mod_bp_p = 8;
                        minep.point_multi[2]["橙色魔法"].value = 1.95;
                        you.pierce_percent = 0;
                        break;
                    case 3:
                        mod_bp_p = 20;
                        minep.point_multi[2]["橙色魔法"].value = 2.5;
                        you.pierce_percent = 0.1;
                        break;
                }

                if (!s.cast_active)
                {
                    if (uncast_boost)
                    {
                        mod_bp_p = 1 + (mod_bp_p - 1) * uncast_effect;
                        minep.point_multi[2]["橙色魔法"].value = 
                            1 + (minep.point_multi[2]["橙色魔法"].value - 1) * uncast_effect;
                        you.pierce_percent *= uncast_effect;
                    }
                    else
                    {
                        mod_bp_p = 1;
                        minep.point_multi[2]["橙色魔法"].value = 1;
                        minep.size_multi[2]["橙色魔法"].value = 1;
                        you.pierce_percent = 0;
                    }
                }

                foreach (KeyValuePair<string, block_producter> kp in block_producters)
                {
                    block_producter bp = kp.Value;
                    bp.multiply(1, mod_bp_p / s.save["方块生产器产量"]);
                }
                s.save["方块生产器产量"] = mod_bp_p;

                minep.size_multi[2]["橙色魔法"].value = 1;
                switch (s.level)
                {
                    case 1:
                        minep.size_multi[2]["橙色魔法"].value = 1.2;
                        break;
                    case 2:
                        minep.size_multi[2]["橙色魔法"].value = 1.44;
                        break;
                    case 3:
                        minep.size_multi[2]["橙色魔法"].value = 1.8;
                        break;
                }
            }
            if (s.name == "蓝色魔法")
            {
                foreach(KeyValuePair<string, enchant> kp in enchants)
                {
                    enchant e = kp.Value;
                    if(e.is_potion)
                    {
                        if (!e.effect_ups.ContainsKey("蓝色魔法"))
                        {
                            e.effect_ups.Add("蓝色魔法", new multiplier(true, 1));
                        }

                        switch (s.current_active_lv)
                        {
                            case 1:
                                e.effect_ups["蓝色魔法"].value = 1.15;
                                break;
                            case 2:
                                e.effect_ups["蓝色魔法"].value = 1.3;
                                break;
                            case 3:
                                e.effect_ups["蓝色魔法"].value = 1.5;
                                break;
                        }

                        if (!s.cast_active)
                        {
                            if (uncast_boost)
                            {
                                e.effect_ups["蓝色魔法"].value =
                                    1 + (e.effect_ups["蓝色魔法"].value - 1) * uncast_effect;
                            }
                            else
                            {
                                e.effect_ups["蓝色魔法"].value = 1;
                            }
                        }
                    }
                }

                if (!res_table["魔法"]["魔力"].multipliers.ContainsKey("蓝色魔法"))
                {
                    res_table["魔法"]["魔力"].multipliers.Add("蓝色魔法", new multiplier(true, 1));
                }

                switch (s.current_active_lv)
                {
                    case 1:
                        res_table["魔法"]["魔力"].multipliers["蓝色魔法"].value = 1.3;
                        break;
                    case 2:
                        res_table["魔法"]["魔力"].multipliers["蓝色魔法"].value = 1.7;
                        break;
                    case 3:
                        res_table["魔法"]["魔力"].multipliers["蓝色魔法"].value = 2.25;
                        break;
                }

                if (!s.cast_active)
                {
                    if (uncast_boost)
                    {
                        res_table["魔法"]["魔力"].multipliers["蓝色魔法"].value =
                            1 + (res_table["魔法"]["魔力"].multipliers["蓝色魔法"].value - 1) * uncast_effect;
                    }
                    else
                    {
                        res_table["魔法"]["魔力"].multipliers["蓝色魔法"].value = 1;
                    }
                }


                if (!s.save.ContainsKey("升级消耗"))
                {
                    s.save.Add("升级消耗", 1);
                }
                double2 old_cd = s.save["升级消耗"];
                double2 new_cd = 1;
                s.save["升级消耗"] = 1;

                switch (s.level)
                {
                    case 1:
                        new_cd = 0.9;
                        s.save["升级消耗"] = 0.9;
                        break;
                    case 2:
                        new_cd = 0.8;
                        s.save["升级消耗"] = 0.8;
                        break;
                    case 3:
                        new_cd = 0.7;
                        s.save["升级消耗"] = 0.7;
                        break;
                }


                foreach (KeyValuePair<string, upgrade> kp in upgrades)
                {
                    upgrade u = kp.Value;
                    if (u is spell)
                    {
                        spell sss = (spell)u;
                        if (!sss.cost_downs.ContainsKey("蓝色魔法"))
                        {
                            sss.cost_downs.Add("蓝色魔法", new multiplier(true, 1));
                        }
                        sss.cost_downs["蓝色魔法"].value = 1;

                        switch (s.current_active_lv)
                        {
                            case 3:
                                sss.cost_downs["蓝色魔法"].value = 1.5;
                                break;
                        }

                        if (!s.cast_active)
                        {
                            if (uncast_boost)
                            {
                                sss.cost_downs["蓝色魔法"].value =
                                    1 + (sss.cost_downs["蓝色魔法"].value - 1) * uncast_effect;
                            }
                            else
                            {
                                sss.cost_downs["蓝色魔法"].value = 1;
                            }
                        }
                    }

                    double2 mod = new_cd / old_cd;
                    if(mod != 1)
                    {
                        foreach (List<Tuple<string, double2>> k1 in u.cost_table)
                        {
                            List<Tuple<string, double2>> temp = new List<Tuple<string, double2>>();

                            if (k1 == null)
                            {
                                continue;
                            }
                            foreach (Tuple<string, double2> k2 in k1)
                            {
                                temp.Add(k2);
                            }
                            k1.Clear();
                            foreach (Tuple<string, double2> k2 in temp)
                            {
                                k1.Add(new Tuple<string, double2>(k2.Item1, k2.Item2 * mod));
                            }
                        }
                    }
                }
            }
            if (s.name == "无色魔法")
            {
                if (!s.save.ContainsKey("采矿点数获取"))
                {
                    s.save.Add("采矿点数获取", 1);
                }

                if (!s.save.ContainsKey("攻击时间降低"))
                {
                    s.save.Add("攻击时间降低", 1);
                }

                double2 mod_cd1 = 1;
                double2 mod_cd2 = 1;
                double2 pp_will_gain = 0;

                switch (s.current_active_lv)
                {
                    case 1:
                        mod_cd1 = 1.5;
                        mod_cd2 = 1.5;
                        pp_will_gain = double2.Pow(cal_pp_gain(), 0.6) * time_a_tick_game;
                        break;
                    case 2:
                        mod_cd1 = 2;
                        mod_cd2 = 2.25;
                        pp_will_gain = double2.Pow(cal_pp_gain(), 0.63) * time_a_tick_game;
                        break;
                    case 3:
                        mod_cd1 = 2.8;
                        mod_cd2 = 3.5;
                        pp_will_gain = double2.Pow(cal_pp_gain(), 0.66) * time_a_tick_game;
                        break;
                }

                if (!s.cast_active)
                {
                    if (uncast_boost)
                    {
                        mod_cd1 = 1 + (mod_cd1 - 1) * uncast_effect;
                        mod_cd2 = 1 + (mod_cd2 - 1) * uncast_effect;
                        pp_will_gain *= uncast_effect;
                    }
                    else
                    {
                        mod_cd1 = 1;
                        mod_cd2 = 1;
                        pp_will_gain = 0;
                    }
                }

                double2 mod1 = mod_cd1 / s.save["采矿点数获取"];
                double2 mod2 = mod_cd2 / s.save["攻击时间降低"];
                if (mod1 != 1)
                {
                    minep.minep_time_down *= mod1;
                }
                if (mod2 != 1)
                {
                    you.other_at_factor *= mod2;
                }
                res_table["转生"]["转生点数"].add_value(pp_will_gain);

                s.save["采矿点数获取"] = mod_cd1;
                s.save["攻击时间降低"] = mod_cd2;

                switch (s.level)
                {
                    case 1:
                        res_table["特殊"]["能量"].add_value(20 * time_a_tick_game);
                        break;
                    case 2:
                        res_table["特殊"]["能量"].add_value(50 * time_a_tick_game);
                        break;
                    case 3:
                        res_table["特殊"]["能量"].add_value(100 * time_a_tick_game);
                        break;
                }
            }
        }

        private void mine_tick()
        {
            if (minep.unlocked)
            {
                double2 time1 = minep.mine_point_progress;
                double2 time2 = minep.next_mine_point;
                double2 gain = (1 + minep.point_boost) * minep.get_point_mul();
                minep.mine_point_progress += time_a_tick_game;

                double2 curr_count = 1;
                while (minep.mine_point_progress >= minep.next_mine_point * curr_count)
                {
                    minep.mine_point_progress -= minep.next_mine_point * curr_count;
                    minep.mine_point_got += curr_count;
                    res_table["采矿"]["采矿点数"].add_value(gain * curr_count);
                    curr_count *= 2;
                }
                minep.update();

                m.采矿_数据_采矿点数_text.Text = "+" + number_format(gain) + " 采矿点数：" + number_format(time1) + " s / " + number_format(time2) + " s";
                m.采矿_数据_采矿点数_进度条_顶.Width = m.采矿_数据_采矿点数_进度条_底.Width * Math.Min((time1 / time2).d, 1);
            }
        }

        private void heat()
        {
            heater_x x = furance.get_current_x();
            heater_y y = furance.get_current_y();

            double2 lost_fire = furance.fire * furance.fire_drop * time_a_tick_game;


            if (x != null)
            {
                double2 loss = 0;
                loss = furance.get_x_speed() / x.recipe.base_speed_req * time_a_tick_game;
                loss = double2.Min(loss, x.amount);

                x.amount -= loss;
                find_resource(x.recipe.b1_name).add_value(loss * x.recipe.b1_product);

                if (x.recipe.b2_name != null)
                {
                    find_resource(x.recipe.b2_name).add_value(loss * x.recipe.b2_product);
                }
            }
            if (y != null)
            {
                double2 loss = 0;
                loss = furance.get_y_speed() / y.recipe.base_speed_req * time_a_tick_game;
                loss = double2.Min(loss, y.amount);

                y.amount -= loss;
                furance.fire += loss * y.recipe.fire_product;
            }


            furance.fire -= lost_fire;
            if (furance.fire < furance.fire_min)
            {
                furance.fire = furance.fire_min;
            }

        }

        double2 save_y_tr = 1;
        double2 save_wbp_time = 1;
        double2 save_wbp_p = 1;
        private void treasure_tick()
        {
            foreach(KeyValuePair<string, treasure> kp in treasures)
            {
                treasure tr = kp.Value;
                double2 value = tr.get_value();
                string name = tr.name;

                if (name == "荧光宝石")
                {
                    resource r0 = res_table["转生"]["转生点数"];
                    if (r0.all_get == 0)
                    {
                        minep.point_cost_down = double2.Max(1, double2.Pow(value, 0.1));
                    }
                    else
                    {
                        minep.point_cost_down = double2.Max(1, double2.Pow(3, r0.get_value() / r0.all_get) * double2.Pow(value, 0.1));
                    }
                }
                if (name == "熔岩球")
                {
                    double2 old_y = save_y_tr;
                    double2 new_y = 1 + double2.Pow(0.05 * (double2)furance.level, 3) * double2.Pow(value, 0.2);
                    double2 ratio = new_y / old_y;

                    if(ratio != 1)
                    {
                        furance.y_speed_factor *= ratio;
                    }
                    save_y_tr = new_y;
                }
                if (name == "魔方")
                {
                    double2 mana = res_table["魔法"]["魔力"].get_value();
                    double2 e1 = treasure_3_1(value, mana);
                    double2 e2 = treasure_3_2(value, mana);
                    foreach (KeyValuePair<string, resource> kp5 in res_table["方块"])
                    {
                        resource r = kp5.Value;
                        if (!r.multipliers.ContainsKey("魔方"))
                        {
                            r.multipliers.Add("魔方", new multiplier(true, 1));
                        }
                        r.multipliers["魔方"].value = e1;
                    }
                    if (!minep.size_multi[1].ContainsKey("魔方"))
                    {
                        minep.size_multi[1].Add("魔方", new multiplier(true, 1));
                    }
                    minep.size_multi[1]["魔方"].value = e2;

                    /*
                    double2.Pow(double2.Pow(value, 0.3) + 1,
                    0.01 * (mana + 1).Log10());*/
                }
                if (name == "脉冲符文")
                {
                    double2 level = 0;
                    if (enemy.current != null)
                    {
                        level = enemy.current.level;
                    }

                    resource r = null;
                    double2 mul = double2.Pow(1 + 0.002 * (value + 1).Log10(), level);

                    r = res_table["特殊"]["能量"];
                    if (!r.multipliers.ContainsKey("脉冲符文"))
                    {
                        r.multipliers.Add("脉冲符文", new multiplier(true, 1));
                    }
                    r.multipliers["脉冲符文"].value = mul;

                    r = res_table["特殊"]["高阶能量"];
                    if (!r.multipliers.ContainsKey("脉冲符文"))
                    {
                        r.multipliers.Add("脉冲符文", new multiplier(true, 1));
                    }
                    r.multipliers["脉冲符文"].value = mul;

                    r = res_table["特殊"]["终极能量"];
                    if (!r.multipliers.ContainsKey("脉冲符文"))
                    {
                        r.multipliers.Add("脉冲符文", new multiplier(true, 1));
                    }
                    r.multipliers["脉冲符文"].value = mul;
                }
                if (name == "宝箱")
                {
                    resource r0 = res_table["采矿"]["采矿点数"];
                    double2 effect = double2.Pow(value + 1, 0.04);
                    if (r0.prestige_get != 0)
                    {
                        effect = double2.Pow(effect, r0.get_value() / r0.prestige_get + 1);
                    }

                    foreach (KeyValuePair<string, treasure> kp1 in treasures)
                    {
                        treasure t = kp1.Value;
                        if (!t.multipliers.ContainsKey("宝箱"))
                        {
                            t.multipliers.Add("宝箱", new multiplier(true, 1));
                        }
                        t.multipliers["宝箱"].value = effect;
                    }
                }
                if (name == "光速徽章")
                {
                    double2 old1 = save_wbp_time;
                    double2 new1 = 1 + double2.Pow(value, 0.1) * double2.Pow(minep.mine_amount, 0.5);
                    double2 ratio1 = new1 / old1;
                    save_wbp_time = new1;

                    double2 old2 = save_wbp_p;
                    double2 new2 = 1 + double2.Min(value, 1) * double2.Pow(minep.mine_amount, 0.3);
                    double2 ratio2 = new2 / old2;
                    save_wbp_p = new2;

                    if (ratio1 != 1 || ratio2 != 1)
                    {
                        block_producters["白色方块"].multiply(1 / ratio1, 1 / ratio2);
                    }
                }
            }
        }
        
        

        bool[] ecb = new bool[3];
        double2 time_boost_max_old = -1;
        private void energy_tick()
        {
            // use = time_actually_go;

            //
            //[e ^ 2 / 100 = cost]
            //[k * cost = k(e ^ 2) / 100 = ((k ^ 0.5 * e) ^ 2) / 100]
            //[e = sqrt(100cost)]
            //[e = em ^ sqrt(percent)]
            //[em ^ 2sqrt(1 / 4) / 100 = cost]
            //[percent = (log(100cost) / log(em) / 2) ^ 2]


            if (s_ticker("energy", 0.1))
            {
                double2 cost_per_second = double2.Pow(ex.time_boost, 2) / 100;
                if (time_boost_max_old != ex.time_boost_max)
                {
                    for (int i = 2; i <= 4; i++)
                    {
                        TextBlock t = (TextBlock)m.FindName("游戏速度_指针" + Convert.ToString(i) + "_text");
                        string ts = "+" + number_format(double2.Pow(ex.time_boost_max, Math.Sqrt((i - 1.0) / 4))) + "%";
                        t.Text = ts;
                    }
                    time_boost_max_old = ex.time_boost_max;
                }
            ((TextBlock)m.FindName("游戏速度_指针1_text")).Text = "+0～1%";
                ((TextBlock)m.FindName("游戏速度_指针5_text")).Text = "+" + number_format(ex.time_boost_max) + "%";
                double2 p = get_energy_percent(ex.time_boost, ex.time_boost_max);

                ((Grid)m.FindName("游戏速度_指针0_Grid")).Margin = new Thickness((300 + 400 * p).d, 24, 0, 0);
                ((TextBlock)m.FindName("游戏速度_速度增加_text")).Text = "游戏速度：+" + number_format(ex.time_boost * ex.time_boost_eff) + "%";
                ((TextBlock)m.FindName("游戏速度_能量消耗_text")).Text = "能量消耗：" + number_format(cost_per_second * ex.cost_mul) + "/s";

                ((TextBlock)m.FindName("能量1_text")).Text = "能量：" + number_format(res_table["特殊"]["能量"].get_value());
                ((TextBlock)m.FindName("能量2_text")).Text = "高阶能量：" + number_format(res_table["特殊"]["高阶能量"].get_value());
                ((TextBlock)m.FindName("能量3_text")).Text = "终极能量：" + number_format(res_table["特殊"]["终极能量"].get_value());

                ((Rectangle)m.FindName("游戏速度_进度条_顶")).Width = 400 * Math.Min((1 - p).d, 1);

                Button b = (Button)m.FindName("减少输出_button");
                if (ex.time_boost == 0)
                {
                    b.Visibility = Visibility.Hidden;
                }
                else
                {
                    b.Visibility = Visibility.Visible;
                }
                b = (Button)m.FindName("增加输出_button");
                if (ex.time_boost >= ex.time_boost_max)
                {
                    b.Visibility = Visibility.Hidden;
                }
                else
                {
                    b.Visibility = Visibility.Visible;
                }

                //time_boost = 100   time_boost_cost = 2   time_boost_eff = 3
                //need = 50
                //spend = 25
                //(25/50)^0.5 * 100 * 3 = 150sq(2)=210

                double2 need = cost_per_second * ex.cost_mul * time_tick_actually;
                double2 spend = 0;
                ecb[0] = false;
                ecb[1] = false;
                ecb[2] = false;
                if (((CheckBox)m.FindName("能量1_checkbox")).IsChecked == true)
                {
                    ecb[0] = true;
                    if (res_table["特殊"]["能量"].get_value() < need - spend)
                    {
                        res_table["特殊"]["能量"].set_value(0);
                        spend += res_table["特殊"]["能量"].get_value();
                    }
                    else
                    {
                        res_table["特殊"]["能量"].add_value(-(need - spend));
                        spend = need;
                    }
                }
                if (((CheckBox)m.FindName("能量2_checkbox")).IsChecked == true)
                {
                    ecb[1] = true;
                    if (res_table["特殊"]["高阶能量"].get_value() < need - spend)
                    {
                        res_table["特殊"]["高阶能量"].set_value(0);
                        spend += res_table["特殊"]["高阶能量"].get_value();
                    }
                    else
                    {
                        res_table["特殊"]["高阶能量"].add_value(-(need - spend));
                        spend = need;
                    }
                }
                if (((CheckBox)m.FindName("能量3_checkbox")).IsChecked == true)
                {
                    ecb[2] = true;
                    if (res_table["特殊"]["终极能量"].get_value() < need - spend)
                    {
                        res_table["特殊"]["终极能量"].set_value(0);
                        spend += res_table["特殊"]["终极能量"].get_value();
                    }
                    else
                    {
                        res_table["特殊"]["终极能量"].add_value(-(need - spend));
                        spend = need;
                    }
                }

                if (need == 0)
                {
                    energy_game_speed = 0;
                }
                else
                {
                    energy_game_speed = Math.Pow((spend / need).d, 0.5) * ex.time_boost * ex.time_boost_eff / 100;
                }
                if (spend < need)
                {
                    ((TextBlock)m.FindName("游戏速度_速度增加_text")).Text = "游戏速度：+" + number_format(ex.time_boost * ex.time_boost_eff) + "%";
                    ((TextBlock)m.FindName("游戏速度_能量消耗_text")).Text = "能量不足！";
                }
                if (need == 0)
                {
                    energy_game_speed = 0;
                }
            }
        }
        private double2 get_energy_percent(double2 e, double2 em)
        {
            if (e == 0)
            {
                return 0;
            }
            return double2.Pow(e.Log10() / em.Log10(), 2);
        }
    }
}
