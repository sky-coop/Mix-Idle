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
    [Serializable]
    public partial class gamestats
    {
        [NonSerialized]
        public MainWindow m;
        
        public static double2 infinity = new double2(1, 1);

        public gamestats(MainWindow mainWindow)
        {
            m = mainWindow;
        }

        [Serializable]
        public class inputer
        {
            public enum state
            {
                normal = 2,
                warning = 1,
                error = 0
            }
            public state curr_state = state.error;
            public string text = "";
        }
        Dictionary<string, inputer> inputers = new Dictionary<string, inputer>();

        [Serializable]
        public class multiplier
        {
            public bool resetable = false;
            public double2 value = new double2(1, 0);
            public multiplier(bool reset, double2 val)
            {
                resetable = reset;
                value = val.copy();
            }
            public void reset()
            {
                if (resetable)
                {
                    value = new double2(1, 0);
                }
            }
        }

        [Serializable]
        public class muls
        {
            Dictionary<string, multiplier> elems = new Dictionary<string, multiplier>();

            public void apply(string name, double2 val, bool reset)
            {
                if (!elems.ContainsKey(name))
                {
                    elems[name] = new multiplier(reset, val);
                }
                elems[name].value = val;
            }
            public double2 get_add()
            {
                double2 eff = 0;
                foreach(KeyValuePair<string, multiplier> kp in elems)
                {
                    eff += kp.Value.value;
                }
                return eff;
            }
            public double2 get_mul()
            {
                double2 eff = 1;
                foreach (KeyValuePair<string, multiplier> kp in elems)
                {
                    eff *= kp.Value.value;
                }
                return eff;
            }
            public void reset()
            {
                foreach (KeyValuePair<string, multiplier> kp in elems)
                {
                    kp.Value.reset();
                }
            }
        }

        [Serializable]
        public class resource
        {
            public int show_loc;
            protected double2 value;
            public double2 reset_base = 0;
            public double2 best = 0;
            public muls addition = new muls();
            public string name;
            public bool unlocked = false;

            public bool rev = false;

            public byte r;
            public byte g;
            public byte b;

            public double2 luck_req = new double2(0, 0);
            public Dictionary<string, multiplier> luck_req_mul = new Dictionary<string, multiplier>();
            public double2 get_luck_req()
            {
                double2 x = new double2(1, 0);
                foreach (KeyValuePair<string, multiplier> kp in luck_req_mul)
                {
                    x *= kp.Value.value;
                }
                return x * luck_req;
            }

            public Dictionary<string, multiplier> multipliers = new Dictionary<string, multiplier>();
            public muls adders = new muls();
            public Dictionary<string, double2> pools = new Dictionary<string, double2>();

            public double2 all_get = new double2(0, 0);
            public double2 prestige_get = new double2(0, 0);

            public resource(int LOC, double2 VAL, string NAME, SolidColorBrush COLOR)
            {
                show_loc = LOC;
                value = VAL;
                name = NAME;
                if (COLOR != null)
                {
                    r = COLOR.Color.R;
                    g = COLOR.Color.G;
                    b = COLOR.Color.B;
                }
                best = double2.Max(best, get_value());
            }

            public SolidColorBrush text_color()
            {
                return new SolidColorBrush(Color.FromRgb(r, g, b));
            }

            public double2 get_mul()
            {
                double2 x = new double2(1, 0);
                foreach (KeyValuePair<string, multiplier> kp in multipliers)
                {
                    if(kp.Value.value != 1)
                    {
                        x *= kp.Value.value;
                    }
                }
                return x;
            }

            public void store_to_pool(string name, double2 amount)
            {
                double2 will = double2.Min(get_value(), amount);
                if (pools.ContainsKey(name))
                {
                    pools[name] += will;
                }
                else
                {
                    pools[name] = will;
                }
                value -= will;
            }

            public void restore_from_pool(string name, double2 amount)
            {
                if (pools.ContainsKey(name))
                {
                    double2 will = double2.Min(amount, pools[name]);
                    pools[name] -= will;
                    value += will;
                }
            }

            public double2 see_pool(string name)
            {
                if (pools.ContainsKey(name))
                {
                    return pools[name];
                }
                return 0;
            }

            public double2 get_from_pool(string name, double2 amount)
            {
                if (pools.ContainsKey(name))
                {
                    double2 will = double2.Min(amount, pools[name]);
                    pools[name] -= will;
                    return will;
                }
                return 0;
            }
            public double2 restore_from_all_pool()
            {
                foreach (KeyValuePair<string, double2> kp in pools)
                {
                    restore_from_pool(kp.Key, kp.Value);
                }
                return 0;
            }

            public double2 all_in_pool()
            {
                double2 x = 0;
                foreach (KeyValuePair<string, double2> kp in pools)
                {
                    x += kp.Value;
                }
                return x;
            }

            public double2 all_with_pool()
            {
                double2 x = get_value() + all_in_pool();
                return x;
            }

            public void add_value(double2 dv, bool no_mul = false)
            {
                if (no_mul)
                {
                    value += dv;
                    all_get += dv;
                    prestige_get += dv;
                    return;
                }

                if (dv >= 0)
                {
                    value += dv * get_mul();
                    all_get += dv * get_mul();
                    prestige_get += dv * get_mul();
                }
                else // dv < 0
                {
                    double2 abs = double2.Abs(value + dv);
                    if (abs == 0)
                    {

                    }
                    // 1 - 2 < - 2 * 0.000001
                    else if (get_value() + dv < dv * 1e-6)
                    {
                        throw new Exception();
                    }
                    value += dv;
                    if (get_value() < new double2(0, 0))
                    {
                        value = -addition.get_add();
                    }
                }
                best = double2.Max(best, value);
            }

            public void set_value(double2 v)
            {
                value = v;
                best = double2.Max(best, get_value());
            }
            public double2 get_value()
            {
                return value + addition.get_add();
            }
            public void reset()
            {
                set_value(reset_base);
            }
        }

        [Serializable]
        public class energy
        {
            public double2 time_boost = new double2(0, 0);
            public double2 time_boost_max = new double2(100, 0);
            public double2 time_boost_eff = new double2(1, 0);
            public double2 cost_mul = new double2(1, 0);
            public energy reseter;

            public energy()
            {
            }

            public void reset()
            {
                time_boost_max = reseter.time_boost_max;
                time_boost_eff = reseter.time_boost_eff;
                cost_mul = reseter.cost_mul;
                if (time_boost > time_boost_max)
                {
                    time_boost = time_boost_max;
                }
            }

        }energy ex = new energy();

        [Serializable]
        public class block_producter
        {
            public string name;
            public decimal level = 1;
            public string cost_res_type;
            public double2 cost;
            public double2 cost_exponent;

            public decimal best = 1;

            public double2 level_up_time_factor;
            public double2 level_up_production_factor;

            public bool unlocked = false;
            public bool can_reset = false;

            public double2 current_value;
            public double2 current_time;
            public double2 max_value;
            public double2 max_time;

            //p = M * (t / T) ^ p_e
            public double2 production_exponent;

            public decimal milestone = 0;
            public double2 milestone_effect_time_factor = new double2(1, 0);
            public double2 milestone_effect_production_factor = new double2(1, 0);

            public decimal milestone2 = 0;
            public double2 milestone_effect_time_factor2 = new double2(1, 0);
            public double2 milestone_effect_production_factor2 = new double2(1, 0);

            public block_producter reseter;

            public block_producter(string NAME)
            {
                name = NAME;
            }

            public void set_init_cost(string cost_type, double2 COST, decimal LEVEL, double2 ltf, double2 lpf, double2 c_e)
            {
                cost_res_type = cost_type;
                cost = COST;
                level = LEVEL;
                level_up_time_factor = ltf;
                level_up_production_factor = lpf;
                cost_exponent = c_e;
                if (can_reset)
                {
                    if (reseter == null)
                    {
                        reseter = new block_producter(name);
                        reseter.set_init_cost(cost_type, COST, LEVEL, ltf, lpf, c_e);
                    }
                }
            }

            public void set_init_value(double2 cur_value, double2 cur_time, double2 max_val, double2 max_tim)
            {
                current_value = cur_value;
                current_time = cur_time;
                max_value = max_val;
                max_time = max_tim;
                if (can_reset)
                {
                    reseter.set_init_value(cur_value, cur_time, max_val, max_tim);
                }
            }

            public void set_init_ms(decimal ms1, double2 ms1_time_f, double2 ms1_p_f)
            {
                milestone = ms1;
                milestone_effect_time_factor = ms1_time_f;
                milestone_effect_production_factor = ms1_p_f;
                if (can_reset)
                {
                    reseter.set_init_ms(ms1, ms1_time_f, ms1_p_f);
                }
            }

            public void set_init_ms2(decimal ms2, double2 ms2_time_f, double2 ms2_p_f)
            {
                milestone2 = ms2;
                milestone_effect_time_factor2 = ms2_time_f;
                milestone_effect_production_factor2 = ms2_p_f;
                if (can_reset)
                {
                    reseter.set_init_ms2(ms2, ms2_time_f, ms2_p_f);
                }
            }

            public void set_init_special(double2 exp)
            {
                production_exponent = exp;
                if (can_reset)
                {
                    reseter.set_init_special(exp);
                }
            }

            public void reset()
            {
                block_producter p = reseter;
                set_init_cost(p.cost_res_type, p.cost, p.level, p.level_up_time_factor, p.level_up_production_factor, p.cost_exponent);
                set_init_value(p.current_value, p.current_time, p.max_value, p.max_time);
                set_init_ms(p.milestone, p.milestone_effect_time_factor, p.milestone_effect_production_factor);
                set_init_ms2(p.milestone2, p.milestone_effect_time_factor2, p.milestone_effect_production_factor2);
            }

            public void multiply(double2 dt, double2 dp)
            {
                max_time *= dt;
                max_value *= dp;
            }

            public void multiply_not_reset(double2 dt, double2 dp)
            {
                multiply(dt, dp);
                reseter.multiply(dt, dp);
            }
        }

        [Serializable]
        public class prestige_upgrade
        {
            public string name;
            public int level = 0;
            public int max_level;
            public string cost_res_type;
            public List<double2> cost;
            public bool unlocked = false;
            public bool can_reset = false;

            public double2 exponent;

            public prestige_upgrade reseter;

            public prestige_upgrade(string NAME)
            {
                name = NAME;
            }

            public void set_init_cost(string cost_type, List<double2> COST, int LEVEL, int MAX_LEVEL)
            {
                cost_res_type = cost_type;
                cost = COST;
                cost.Add(new double2(0, 0));
                level = LEVEL;
                max_level = MAX_LEVEL;
                if (can_reset)
                {
                    if (reseter == null)
                    {
                        reseter = new prestige_upgrade(name);
                        reseter.set_init_cost(cost_type, COST, LEVEL, MAX_LEVEL);
                    }
                }
            }

            public void set_init_special(double2 sp1)
            {
                exponent = sp1;
                if (can_reset)
                {
                    reseter.set_init_special(sp1);
                }
            }

            public void reset()
            {
                prestige_upgrade p = reseter;
                set_init_cost(p.cost_res_type, p.cost, p.level, p.max_level);
                set_init_special(p.exponent);
            }
        }

        [Serializable]
        public class link
        {
            //a -> b
            [NonSerialized]
            public Grid s;
            [NonSerialized]
            public Grid a;
            [NonSerialized]
            public Grid b;
            decimal need;
            decimal current;
            public bool unlocked = false;

            public bool pt = false;
            prestige_upgrade pa;
            prestige_upgrade pb;

            [NonSerialized]
            public Line bg = new Line();
            [NonSerialized]
            public Line pg = new Line();
            public bool complete = false;
            public double get_progress()
            {
                if (current >= need)
                {
                    return 1;
                }
                return (double)current / (double)need;
            }

            public void produce_line(Line x, bool pro = false)
            {
                Point p1 = a.TranslatePoint(new Point(0, 0), s);
                x.X1 = p1.X + a.Width / 2;
                x.Y1 = p1.Y + a.Height / 2;

                Point p2 = b.TranslatePoint(new Point(0, 0), s);
                x.X2 = p2.X + b.Width / 2;
                x.Y2 = p2.Y + b.Height / 2;

                Grid.SetRowSpan(x, 10);
                Grid.SetColumnSpan(x, 10);

                x.StrokeThickness = 8;
                if (pro)
                {
                    x.Stroke = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                }
                else
                {
                    x.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }

            public link(Grid space, Grid start, Grid end, decimal require, bool show)
            {
                s = space;
                a = start;
                b = end;
                need = require;
                bg = new Line();
                pg = new Line();
                produce_line(pg, true);
                produce_line(bg);
                if (show)
                {
                    unlock();
                }
            }

            public void prestige_type(prestige_upgrade a, prestige_upgrade b)
            {
                pt = true;
                pa = a;
                pb = b;
            }

            public void unlock()
            {
                unlocked = true;
                SHOW(pg);
                SHOW(bg);
            }

            public void SHOW(Line x)
            {
                //置底
                List<UIElement> temp = new List<UIElement>();
                foreach (UIElement u in s.Children)
                {
                    temp.Add(u);
                }
                foreach (UIElement u in temp)
                {
                    s.Children.Remove(u);
                }
                s.Children.Add(x);
                if (temp.Contains(x))
                {
                    temp.Remove(x);
                }
                foreach (UIElement u in temp)
                {
                    s.Children.Add(u);
                }
            }

            public void update_progress(decimal CURRENT)
            {
                if(bg == null)
                {
                    bg = new Line();
                    pg = new Line();
                    produce_line(pg, true);
                    produce_line(bg);
                }
                
                current = CURRENT;
                if (!complete)
                {
                    double p = get_progress();

                    double radius = a.Width;
                    double length = Math.Sqrt((bg.X2 - bg.X1) * (bg.X2 - bg.X1) + (bg.Y2 - bg.Y1) * (bg.Y2 - bg.Y1));
                    double effective = (length - radius) / length;
                    double covered_half = (radius / 2) / length;
                    

                    //例如radius = 50  length = 130  need = 3 
                    //x1 = 100, x2 = 150, y1 = 80, y2 = 200
                    //effective = 80 / 130   covered_half = 25 / 130
                    //25 - 80 - 25
                    //current = 1; progress = 25 + 26.67 = 51.67 
                    //                             new x = 100 + (25 / 130 + (1 / 3) * 80 / 130) * (150 - 100)
                    //                                   = 100 + 51.67 / 130 * 50;
                    //                                   = 119.9      dx = 19.9
                    //                             new y = 80 + (51.67 / 130) * 120
                    //                                   = 127.7      dy = 47.7      51.67^2 = 2670 = 396 + 2274
                    //current = 2; progress = 25 + 53.33 = 88.33;
                    //current = 3; progress = 140;
                    if (current >= need)
                    {
                        pg.X2 = bg.X2;
                        pg.Y2 = bg.Y2;
                        complete = true;
                    }
                    else
                    {
                        pg.X2 = bg.X1 + (covered_half + p * effective) * (bg.X2 - bg.X1);
                        pg.Y2 = bg.Y1 + (covered_half + p * effective) * (bg.Y2 - bg.Y1);
                    }
                }
                else
                {
                    pg.X2 = bg.X2;
                    pg.Y2 = bg.Y2;
                }
                if (unlocked)
                {
                    SHOW(pg);
                    SHOW(bg);
                }
            }
        }

        [Serializable]
        public class upgrade
        {
            public string name;
            public bool material;
            public bool auto_cost;
            public string type;

            public int level = 0;
            public int max_level; //-1为无限
            public List<List<Tuple<string, double2>>> cost_table;
            public List<string> description = new List<string>();
            public List<string> description2 = new List<string>();

            public Dictionary<string, double2> save = new Dictionary<string, double2>();


            public bool unlocked = false;
            public bool can_reset = false;

            public int best = 1;

            public double2 exponent;
            public double2 factor;

            public upgrade reseter;

            public position pos;

            public upgrade(string NAME, string TYPE, bool MATERIAL = false, bool AUTO_COST = false)
            {
                name = NAME;
                type = TYPE;
                material = MATERIAL;
                auto_cost = AUTO_COST;
            }

            public void set_init_cost(List<List<Tuple<string, double2>>> COST, int LEVEL, int MAX_LEVEL)
            {
                cost_table = COST;
                cost_table.Add(null);
                level = LEVEL;
                max_level = MAX_LEVEL;
                if (can_reset)
                {
                    if (reseter == null)
                    {
                        reseter = new upgrade(name, type, material, auto_cost);
                        reseter.set_init_cost(COST, LEVEL, MAX_LEVEL);
                        
                    }
                }
            }

            public void set_init_special(double2 exp, double2 fact)
            {
                exponent = exp;
                factor = fact;
                if (can_reset)
                {
                    reseter.set_init_special(exp, fact);
                }
            }

            public string get_auto_res()
            {
                return cost_table[0][0].Item1;
            }

            public double2 get_auto_value()
            {
                return cost_table[0][0].Item2;
            }

            public void first_expensive(double2 mul)
            {
                List<Tuple<string, double2>> x = new List<Tuple<string, double2>>();
                foreach(Tuple<string, double2> t in cost_table[0])
                {
                    x.Add(new Tuple<string, double2>(t.Item1,
                        t.Item2 * mul));
                }
                cost_table[0] = x;
            }

            public void reset()
            {
                if (can_reset)
                {
                    upgrade p = reseter;
                    set_init_cost(p.cost_table, p.level, p.max_level);
                    set_init_special(p.exponent, p.factor);
                }
            }
        }

        [Serializable]
        public class spell : upgrade
        {
            public bool study_active = false;
            public bool studying = false;

            public bool entering = false;

            public double2 current_time = 0;
            public Dictionary<string, multiplier> cost_downs = new Dictionary<string, multiplier>();
            public Dictionary<string, multiplier> speed_ups = new Dictionary<string, multiplier>();

            public bool normal = false;
            public bool cast_active = false;
            public bool casting = false;

            public bool entering2 = false;

            public int current_active_lv = 0;
            public List<string> passive_des = new List<string>();
            public List<string> active_des = new List<string>();
            public List<List<Tuple<string, double2>>> cost_table_active = new List<List<Tuple<string, double2>>>();
            

            public List<double2> time = new List<double2>();

            public spell(string NAME, string TYPE, bool MATERIAL = false, bool AUTO_COST = false) : base(NAME, TYPE)
            {
            }
            public spell add_time(double2 t)
            {
                time.Add(t);
                return this;
            }


            public double2 get_cost_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in cost_downs)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public void spell_reset()
            {
                if (can_reset)
                {
                    reset();

                    current_time = 0;
                    current_active_lv = 0;

                    study_active = false;
                    cast_active = false;
                    studying = false;
                    casting = false;
                }
            }

            public void add_time_mul(string name, double2 val, bool res)
            {
                if (!speed_ups.ContainsKey(name))
                {
                    speed_ups.Add(name, new multiplier(res, 1));
                }
                double2 old = speed_ups[name].value;
                speed_ups[name].value = val;
                current_time /= (val / old);
            }

            public double2 get_time()
            {
                double2 speed_factor = 1;
                foreach (KeyValuePair<string, multiplier> kp in speed_ups)
                {
                    speed_factor *= kp.Value.value;
                }

                if (level == max_level)
                {
                    return new double2(1, -1);
                }
                return time[(int)level] / speed_factor;
            }

            public void add_passive(string s)
            {
                passive_des.Add(s);
            }
            public void add_active(string s)
            {
                active_des.Add(s);
            }

            public void change_level(int dl)
            {
                current_active_lv += dl;
                if (current_active_lv < 1)
                {
                    current_active_lv = 1;
                }
                if (current_active_lv > level)
                {
                    current_active_lv = level;
                }
            }
        }

        [Serializable]
        public class enchant
        {
            public string name;

            public decimal level = 1;
            public decimal max_level = 100;
            //cost_table[0:2] 名称 首级价格 增长速度
            public List<Tuple<string, double2, double2>> cost_table;

            public double2 time_base;
            public double2 time_exponment;
            public double2 current_time = 0;

            public string produce_res;
            public double2 produce_base;
            public double2 produce_exponent;

            public bool is_potion = false;
            public Dictionary<string, multiplier> effect_ups = new Dictionary<string, multiplier>();
            public Dictionary<string, multiplier> speed_ups = new Dictionary<string, multiplier>();
            public Dictionary<string, multiplier> cost_downs = new Dictionary<string, multiplier>();

            public double2 cost_level_down = 0;

            public bool changing_time = false;
            public decimal craft_amount = 0;
            public double2 ca_time_increment = 0;
            public double2 ca_time_factor = 0;

            public bool unlocked = false;
            public bool active = false;
            public bool can_reset = false;
            public enchant reseter;

            [NonSerialized]
            public LinearGradientBrush LinearGradientBrush;
            public bool can_buy = true;

            public position p;

            public double2 get_effect_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in effect_ups)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public double2 get_time_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in speed_ups)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public double2 get_cost_divide()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in cost_downs)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public enchant(string NAME, LinearGradientBrush LGB)
            {
                name = NAME;
                LinearGradientBrush = LGB;
            }

            public void set_init_cost(List<Tuple<string, double2, double2>> COST, decimal LEVEL, decimal MAX_LEVEL, double2 t, double2 t_e)
            {
                cost_table = COST;
                level = LEVEL;
                max_level = MAX_LEVEL;
                time_base = t;
                time_exponment = t_e;
                if (can_reset)
                {
                    if (reseter == null)
                    {
                        reseter = new enchant(name, null);
                        reseter.set_init_cost(COST, LEVEL, MAX_LEVEL, t, t_e);
                    }
                }
            }

            public void set_produce(string type_res, double2 p_b, double2 p_e)
            {
                produce_res = type_res;
                produce_base = p_b;
                produce_exponent = p_e;
                if (can_reset)
                {
                    reseter.set_produce(type_res, p_b, p_e);
                }
            }

            public void set_changing_time(double2 increment, double2 factor)
            {
                changing_time = true;
                ca_time_increment = increment;
                ca_time_factor = factor;
                if (can_reset)
                {
                    reseter.set_changing_time(increment, factor);
                }
            }

            public void change_level(decimal dl)
            {
                level += dl;
                if (level < 1)
                {
                    level = 1;
                }
                if (level > max_level)
                {
                    level = max_level;
                }
                if (current_time > get_time())
                {
                    current_time = get_time();
                }
            }

            public double2 get_time()
            {
                return (time_base + (craft_amount * ca_time_increment))
                    * double2.Pow(time_exponment, level - 1)
                    * (1 + craft_amount * ca_time_factor)
                    / get_time_mul();
            }

            public double2 get_produce()
            {
                return produce_base * double2.Pow(produce_exponent, level - 1);
            }

            public Tuple<string, double2> get_cost(int index)
            {
                if (cost_table.Count <= index)
                {
                    return null;
                }
                string res_name = cost_table[index].Item1;
                double2 cost = cost_table[index].Item2 * double2.Pow(cost_table[index].Item3, level - 1 - cost_level_down) / get_cost_divide();
                return new Tuple<string, double2>(res_name, cost);
            }

            public void reset()
            {
                if (can_reset)
                {
                    enchant p = reseter;
                    set_init_cost(p.cost_table, level, p.max_level, p.time_base, p.time_exponment);
                    set_produce(p.produce_res, p.produce_base, p.produce_exponent);
                    cost_level_down = reseter.cost_level_down;
                    if (changing_time)
                    {
                        set_changing_time(p.ca_time_increment, p.ca_time_factor);
                    }
                    craft_amount = 0;
                    current_time = 0;
                    active = false;
                }
            }
        }

        [Serializable]
        public class altar
        {
            public string mode = "魔力祭坛";

            public bool can_reset = false;

            public Dictionary<string, double2> ate = new Dictionary<string, double2>();
            public Dictionary<string, double2> power_table = new Dictionary<string, double2>();
            public Dictionary<string, double2> power_table_base = new Dictionary<string, double2>();
            public double2 power = 0;
            public Dictionary<string, multiplier> power_ups = new Dictionary<string, multiplier>();
            public Dictionary<string, multiplier> speed_ups = new Dictionary<string, multiplier>();

            public double2 mana_exponent = 0.5;
            public double2 mana_factor = 1;

            public Dictionary<string, position> positions = new Dictionary<string, position>();

            public double2 get_power_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in power_ups)
                {
                    x *= kp.Value.value;
                }
                return x;
            }
            public double2 get_speed_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in speed_ups)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public void add(string res_name, double2 effect, position p)
            {
                power_table_base.Add(res_name, effect);
                power_table.Add(res_name, effect);
                positions.Add(res_name, p);
            }

            public void eat(string res_name, double2 n)
            {
                if (power_table.ContainsKey(res_name))
                {
                    double2 effect = power_table[res_name];
                    if (ate.ContainsKey(res_name))
                    {
                        ate[res_name] += n;
                        power += n * effect * get_power_mul();
                    }
                    else
                    {
                        ate.Add(res_name, n);
                        power += n * effect * get_power_mul();
                    }
                }
            }

            public void multiply(string res_name, double2 factor)
            {
                if (power_table.ContainsKey(res_name))
                {
                    power_table[res_name] *= factor;
                }
            }
            public void multiply_not_reset(string res_name, double2 factor)
            {
                if (power_table.ContainsKey(res_name))
                {
                    power_table[res_name] *= factor;
                    power_table_base[res_name] *= factor;
                }
            }

            public void reset()
            {
                power = 0;
                ate.Clear();
                power_table.Clear();
                foreach (KeyValuePair<string, double2> kp in power_table_base)
                {
                    power_table.Add(kp.Key, kp.Value);
                }
            }
        }
        altar magic_altar = new altar();

        Dictionary<string, attack_form> attack_forms = new Dictionary<string, attack_form>();
        double2 连斩_count = 0;
        double2 连斩_damage_base = 0.5;
        double2 连斩_damage_boost = 0.5;
        double2 连斩_max_damage = 1.5;
        double2 连斩_damage_base_reseter = 0.5;
        double2 连斩_damage_boost_reseter = 0.5;
        double2 连斩_max_damage_reseter = 1.5;
        [Serializable]
        public class attack_form
        {
            public string name;
            public bool unlocked = false;
            public bool lock__ = false;
            public bool can_reset = false;

            public bool manual = false;
            public double2 manual_anti_fast_click_cd = 1;
            public double2 mafcc_cur_time = infinity;

            public bool skill = false;
            public bool skilling = false;
            public double2 skill_time = 0;
            public double2 skill_time_current = 0;
            public double2 attack_progress = 0;
            public double2 attack_time = 0;
            public double2 shine_progress = 0;
            public double2 shine_time = 0.1;

            public double2 attack_factor;
            public double2 at_factor;
            public double2 sr_factor;

            public double2 def_down_percent = 0;     //降低百分比防御
            public double2 def_down = 0;             //降低防御
            public double2 def_pierce_percent = 0;   //穿透百分比防御（真实伤害）
            public double2 def_pierce = 0;           //穿透防御
            public double2 def_ignore_percent = 0;   //无视百分比防御
            public double2 def_ignore = 0;           //无视防御

            public double2 overkill = 0;

            public attack_form reseter;

            public attack_form(string NAME, bool lock_, bool MANUAL, double2 mafcc)
            {
                name = NAME;
                lock__ = lock_;
                manual = MANUAL;
                manual_anti_fast_click_cd = mafcc;
            }

            public void set_factor(double2 af, double2 atf, double2 srf)
            {
                attack_factor = af;
                at_factor = atf;
                sr_factor = srf;
                if (can_reset)
                {
                    if (reseter == null)
                    {
                        reseter = new attack_form(name, true, manual, manual_anti_fast_click_cd);
                        reseter.set_factor(af, atf, srf);
                    }
                }
            }

            public void reset()
            {
                if (can_reset)
                {
                    attack_factor = reseter.attack_factor;
                    at_factor = reseter.at_factor;
                    sr_factor = reseter.sr_factor;
                    manual_anti_fast_click_cd = reseter.manual_anti_fast_click_cd;

                    def_down_percent = reseter.def_down_percent;
                    def_down = reseter.def_down;
                    def_pierce_percent = reseter.def_pierce_percent;
                    def_pierce = reseter.def_pierce;
                    def_ignore_percent = reseter.def_ignore_percent;
                    def_ignore = reseter.def_ignore;

                    overkill = reseter.overkill;

                    skill_time_current = 0;
                    attack_progress = 0;
                    shine_progress = 0;
                    attack_time = reseter.attack_time;
                 }
                if (!lock__)
                {
                    unlocked = false;
                }
            }
        }

        [Serializable]
        public class player
        {
            public double2 攻击次数 = 0;
            public double2 pierce_percent = 0;
            public Dictionary<string, double2> save = new Dictionary<string, double2>();

            public double2 base_attack = 1;
            public double2 item_attack = 0;
            public double2 level_attack = 0;

            public double2 level_attack_factor = 1;
            public muls other_attack_factor = new muls();

            public double2 level_attack_increment = 0.5;
            public double2 level_attack_exponent = 1.1;

            public double2 attack_progress = 0;
            public double2 attack_time_base = 2;
            public double2 item_at_factor = 1;
            public double2 level_at_factor = 1;
            public double2 other_at_factor = 1;
            public double2 slow_value = 0;

            public double2 attack_repeat = 1;

            public double2 slow_cap = 1;

            public double2 level_at_exponent = 1.01;

            public double2 sr_base = 0.05;
            public double2 item_sr = 0;
            public double2 level_sr = 0;
            public double2 sr_factor = 1;

            public double2 level_sr_increment = 0.01;

            public attack_form auto_attack_form;

            public double2 level = 0;
            public double2 exp = 0;
            public double2 exp_need_base = 10;
            public double2 exp_exponent = 1.25;
            public double2 exp_exponent_increment = 0.0001;
            public Dictionary<string, multiplier> exp_gain_multipliers = new Dictionary<string, multiplier>();

            public player reseter;

            public double2 get_exp_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in exp_gain_multipliers)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public double2 get_exp_to_level(double2 n)
            {
                return exp_need_base * double2.Pow(exp_exponent + (level + n - 1) * exp_exponent_increment, (level + n - 1)) * n;
            }

            public void level_up(double2 n)
            {
                exp -= get_exp_to_level(n);
                level += n;
                update();
            }

            public void update()
            {
                level_attack = level * level_attack_increment;
                level_sr = level * level_sr_increment;
                level_attack_factor = double2.Pow(level_attack_exponent, level);
                level_at_factor = double2.Pow(level_at_exponent, level);
            }

            public void gain_exp(double2 e)
            {
                exp += e * get_exp_mul();
                while (exp >= get_exp_to_level(1))
                {
                    try_level();
                }
            }

            public void try_level()
            {
                //目标count 425
                double2 count = 1;
                double2 times = exp / get_exp_to_level(count);
                while (times > 1000000)
                {
                    count *= 2;
                    times = exp / get_exp_to_level(count);
                }
                //count = 512
                while (times > 1000000 || times < 1)
                {
                    double2 count2 = 1;
                    //count = 512 -> 511 -> 509 -> 505 -> 497 -> 481 -> 449 -> 385
                    //count = 448 -> 447 -> 445 -> 441 -> 433 -> 417
                    //count = 432 -> 431 -> 429 -> 425[]
                    while (times < 1)
                    {
                        count -= count2;
                        times = exp / get_exp_to_level(count);
                        count2 *= 2;
                    }

                    count2 = 1;
                    //count = 385 -> 386 -> 388 -> 392 -> 400 -> 416 -> 448
                    //count = 417 -> 418 -> 420 -> 424 -> 432
                    while (times > 1000000)
                    {
                        count += count2;
                        times = exp / get_exp_to_level(count);
                        count2 *= 2;
                    }
                }
                level_up(count);
            }

            public void to_level(double2 n)
            {
                if (level < n)
                {
                    level = n;
                    exp = 0;
                    update();
                }
            }

            public player(bool first)
            {
                if (!first)
                {
                    return;
                }
                reseter = new player(false);
            }

            public double2 get_attack()
            {
                double2 attack_base = base_attack + item_attack + level_attack;
                return attack_base * level_attack_factor * other_attack_factor.get_mul();
            }

            public double2 get_attack_time()
            {
                return attack_time_base / item_at_factor / level_at_factor / other_at_factor * slow_value * auto_attack_form.at_factor;
            }

            public double2 get_sr()
            {
                return (sr_base + item_sr + level_sr) * sr_factor * auto_attack_form.sr_factor;
            }

            public void reset()
            {
                pierce_percent = reseter.pierce_percent;

                base_attack = reseter.base_attack;
                item_attack = reseter.item_attack;
                level_attack = reseter.level_attack;
                level_attack_factor = reseter.level_attack_factor;
                other_attack_factor.reset();
                level_attack_increment = reseter.level_attack_increment;
                level_attack_exponent = reseter.level_attack_exponent;

                attack_progress = 0;
                attack_time_base = reseter.attack_time_base;
                item_at_factor = reseter.item_at_factor;
                level_at_factor = reseter.level_at_factor;
                other_at_factor = reseter.other_at_factor;
                slow_value = reseter.slow_value;
                attack_repeat = reseter.attack_repeat;
                slow_cap = reseter.slow_cap;
                level_at_exponent = reseter.level_at_exponent;

                sr_base = reseter.sr_base;
                item_sr = reseter.item_sr;
                level_sr = reseter.level_sr;
                sr_factor = reseter.sr_factor;

                level = reseter.level;
                exp = reseter.exp;
                exp_need_base = reseter.exp_need_base;
                exp_exponent = reseter.exp_exponent;
                exp_exponent_increment = reseter.exp_exponent_increment;
            }
        }
        player you = new player(true);

        [Serializable]
        public class enemy
        {
            public string field;
            public string name;
            static public string curr_field;
            static public enemy current;

            public string des;

            public double2 health_base;
            public double2 exp_base;

            public double2 curr_health;

            public double2 slow_base = 0;
            public double2 defense_base = 0;
            public double2 regen_base = 0;

            public double2 level = 1;
            public double2 max_level;
            public double2 max_level_base;

            public double2 health_exponent;
            public double2 exp_exponent;

            public double2 slow_increment = 0;
            public double2 defense_exponent = 1;
            public double2 regen_exponent = 1;

            public double2 health;
            public double2 exp;
            public double2 slow;
            public double2 defense;
            public double2 regen;

            public position p;

            public byte r;
            public byte g;
            public byte b;

            //类型 基础数量 增长率
            public Dictionary<string, Tuple<double2, double2>> drop;
            public Dictionary<string, double2> curr_level_drop = new Dictionary<string, double2>();

            public enemy(string f, string n, double2 h, double2 h_e, double2 e, double2 e_e, double2 max, 
                Dictionary<string, Tuple<double2, double2>> loot, SolidColorBrush color)
            {
                field = f;
                name = n;
                health_base = h;
                health_exponent = h_e;
                exp_base = e;
                exp_exponent = e_e;
                max_level = max_level_base = max;
                drop = loot;
                r = color.Color.R;
                g = color.Color.G;
                b = color.Color.B;
                set_level(1);
            }

            public SolidColorBrush text_color()
            {
                return new SolidColorBrush(Color.FromRgb(r, g, b));
            }

            public void set_special(double2 SLOW, double2 DEFENSE, double2 REGEN)
            {
                slow_base = SLOW;
                defense_base = DEFENSE;
                regen_base = REGEN;
                set_level(1);
            }

            public void set_special_exponent(double2 s_i, double2 d_e, double2 r_e)
            {
                slow_increment = s_i;
                defense_exponent = d_e;
                regen_exponent = r_e;
                set_level(1);
            }

            public void set_level(double2 n)
            {
                if (n < 1)
                {
                    n = 1;
                }
                if (n > max_level)
                {
                    n = max_level;
                }
                level = n;
                health = health_base * double2.Pow(health_exponent, n - 1);
                exp = exp_base * double2.Pow(exp_exponent, n - 1);
                slow = slow_base + slow_increment * (n - 1);
                defense = defense_base * double2.Pow(defense_exponent, n - 1);
                regen = regen_base * double2.Pow(regen_exponent, n - 1);

                curr_health = health;

                curr_level_drop.Clear();
                foreach (KeyValuePair<string, Tuple<double2, double2>> kp in drop)
                {
                    curr_level_drop.Add(kp.Key, kp.Value.Item1 * double2.Pow(kp.Value.Item2, n - 1));
                }
            }

            public void reset()
            {
                max_level = max_level_base;
                if (level > max_level)
                {
                    set_level(max_level);
                }
            }
        }

        [Serializable]
        public class mine_cell
        {
            public int depth = 1;
            public int max_depth;
            public double2 luck;
            public List<double2> cost;
            public List<double2> exp;
            public List<Dictionary<string, double2>> loot = new List<Dictionary<string, double2>>();

            public List<byte> r;
            public List<byte> g;
            public List<byte> b;

            public bool entering = false;
            public bool enter_not_change = false;

            public mine_cell(int DEPTH, int MAX_DEPTH, double2 LUCK, List<double2> COST, List<double2> EXP, 
                List<Dictionary<string, double2>> LOOT, List<byte> R, List<byte> G, List<byte> B)
            {
                depth = DEPTH;
                max_depth = MAX_DEPTH;
                luck = LUCK;
                cost = COST;
                exp = EXP;
                loot = LOOT;
                r = R;
                g = G;
                b = B;
            }

            SolidColorBrush get_color(int depth)
            {
                return new SolidColorBrush(Color.FromRgb(r[depth - 1], g[depth - 1], b[depth - 1]));
            }

        }

        [Serializable]
        public class mine
        {
            public gamestats gs = null;

            public double2 cell_size = 1000;
            public int max_depth = 10;
            public double2 depth_luck_exponent = 1.8;
            public double2 point_cost_down = 1;
            public double2 exp = 5;
            public double2 luck = 1;
            public mine_cell[,] graph = null;

            public bool entering = false;
            public bool entering2 = false;
            public bool holding = false;
            public bool holding2 = false;

            public mine reseter;

            public mine()
            {
            }

            public void init(gamestats g, double2 size, int maxdepth, double2 exp_,
                double2 luck_, double2 depthluck_exponent, double2 point_cost_d)
            {
                reseter = new mine();

                gs = g;
                cell_size = size;
                max_depth = maxdepth;
                exp = exp_;
                luck = luck_;
                depth_luck_exponent = depthluck_exponent;
                point_cost_down = point_cost_d;
            }

            public void reset()
            {
                cell_size = reseter.cell_size;
                max_depth = reseter.max_depth;
                exp = reseter.exp;
                luck = reseter.luck;
                depth_luck_exponent = reseter.depth_luck_exponent;

                generate();
            }

            public int get_depth_min()
            {
                int depth_min = int.MaxValue;
                foreach (mine_cell m in graph)
                {
                    depth_min = Math.Min(depth_min, m.depth);
                }
                return depth_min;
            }

            public List<List<int>> get_all_top_index(int layer)
            {
                List<List<int>> ret = new List<List<int>>();
                int depth_min = get_depth_min();

                // layer = 3   min = 2   mine = 2～4
                // 2 3 4 2     value2:  1 0 0 1    value3: 1 1 0 1
                // 2 2 3 2     ( <= 2)  1 1 0 1    ( <= 3) 1 1 1 1
                // 2 5 6 2              1 0 0 1            1 0 0 1
                // 2 2 4 5              1 1 0 0            1 1 0 0

                if (depth_min == int.MaxValue)
                {
                    return ret;
                }

                int target_max = Math.Min(depth_min + layer - 1, max_depth);

                for(int p = 0; p <= target_max; p++)
                {
                    ret.Add(new List<int>());
                }

                for(int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        mine_cell m = graph[i, j];
                        for(int k = depth_min; k <= target_max; k++)
                        {
                            if(m.depth <= k)
                            {
                                ret[k].Add(10 * i + j);
                            }
                        }
                    }
                }
                return ret;
            }

            public double2 get_all_top_cost(int layer)
            {
                List<List<int>> index = get_all_top_index(layer);
                
                double2 cost = 0;

                int cur_depth = 0;
                foreach(List<int> k in index)
                {
                    cost += k.Count * double2.Pow(cur_depth, 2) / point_cost_down;
                    cur_depth++;
                }
                return cost;
            }

            public double2 get_all_top_exp(int layer)
            {
                List<List<int>> index = get_all_top_index(layer);

                double2 exp = 0;

                int cur_depth = 0;
                foreach (List<int> k in index)
                {
                    foreach(int n in k)
                    {
                        int i = n / 10;
                        int j = n % 10;
                        mine_cell m = graph[i, j];
                        exp += m.exp[cur_depth - 1];
                    }
                    cur_depth++;
                }
                return exp;
            }

            public void get_all_top_depth_up(int layer)
            {
                List<List<int>> index = get_all_top_index(layer);

                int cur_depth = 0;
                foreach (List<int> k in index)
                {
                    foreach (int n in k)
                    {
                        int i = n / 10;
                        int j = n % 10;
                        mine_cell m = graph[i, j];
                        m.depth++;
                        if (m.depth > m.max_depth)
                        {
                            m.depth = int.MaxValue;
                        }
                    }
                    cur_depth++;
                }
                gs.get_field();
            }

            public Dictionary<string, double2> get_all_top_loot(int layer)
            {
                List<List<int>> index = get_all_top_index(layer);
                Dictionary<string, double2> d = new Dictionary<string, double2>();
                d.Add("泥土方块", 0);
                d.Add("石头方块", 0);
                foreach (string s in gs.矿物)
                {
                    d.Add(s, 0);
                }
                foreach (KeyValuePair<string, treasure> kp in gs.treasures)
                {
                    d.Add(kp.Value.name, 0);
                }

                int cur_depth = 0;
                foreach (List<int> k in index)
                {
                    foreach(int n in k)
                    {
                        int i = n / 10;
                        int j = n % 10;
                        mine_cell m = graph[i, j];
                        foreach(KeyValuePair<string, double2> kp in m.loot[cur_depth - 1])
                        {
                            d[kp.Key] += kp.Value;
                        }
                    }
                    cur_depth++;
                }

                List<string> temp = new List<string>();
                foreach (KeyValuePair<string, double2> kp in d)
                {
                    if(kp.Value <= 0)
                    {
                        temp.Add(kp.Key);
                    }
                }
                foreach (string s in temp)
                {
                    d.Remove(s);
                }

                return d;
            }

            public void generate()
            {
                graph = new mine_cell[8, 8];
                Random a = new Random();
                for (int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        List<double2> c = new List<double2>();
                        List<double2> e = new List<double2>();
                        List<Dictionary<string, double2>> l = new List<Dictionary<string, double2>>();
                        List<byte> rr = new List<byte>();
                        List<byte> gg = new List<byte>();
                        List<byte> bb = new List<byte>();
;                       double2 total = double2.Pow(cell_size, 3);
                        double2 y_down = cell_size / 1000;

                        for (int k = 0; k < max_depth; k++)
                        {
                            int depth = k + 1;
                            Dictionary<string, double2> res = new Dictionary<string, double2>();
                            double2 luck_value = double2.Pow(depth, depth_luck_exponent) * luck;
                            double2 cost = double2.Pow(depth, 2) / point_cost_down;
                            double2 _exp = exp * double2.Pow(cost, 0.9) * (0.5 + a.NextDouble());
                            c.Add(cost);
                            e.Add(_exp);

                            double2 stone_up_chance = 0.02 + (double2.Pow(luck_value, 0.1) - 0.5) * a.NextDouble();
                            if (stone_up_chance > 1)
                            {
                                stone_up_chance = 1;
                            }
                            double2 dirt_chance = 1 - stone_up_chance;

                            // luck_value 不能超过 100M
                            double2 coal_chance = 0;
                            double2 coal_random = 0;
                            if (luck_value >= gs.res_table["采矿"]["煤"].get_luck_req())
                            {
                                coal_random = 0.01 + 0.99 * double2.Pow(a.NextDouble(), 1.5);
                                coal_chance = 5e-9 * double2.Pow(luck_value, 2.0 / 3.0) * coal_random;
                            }

                            double2 copper_chance = 0;
                            double2 copper_random = 0;
                            if (luck_value >= gs.res_table["采矿"]["铜矿"].get_luck_req())
                            {
                                copper_random = 0.005 + 0.995 * double2.Pow(a.NextDouble(), 1.65);
                                copper_chance = 3e-9 * luck_value * copper_random;
                            }

                            double2 iron_chance = 0;
                            double2 iron_random = 0;
                            if (luck_value >= gs.res_table["采矿"]["铁矿"].get_luck_req())
                            {
                                iron_random = 0.0025 + 0.9975 * double2.Pow(a.NextDouble(), 1.75);
                                iron_chance = 1e-9 * luck_value * iron_random;
                            }

                            double2 magic_chance = 0;
                            double2 magic_random = 0;
                            if (luck_value >= gs.res_table["采矿"]["魔石"].get_luck_req())
                            {
                                magic_random = -0.2 + 1.2 * double2.Pow(a.NextDouble(), 1.85);
                                if (magic_random < 0)
                                {
                                    magic_random = 0;
                                }
                                else if (magic_random < 0.005)
                                {
                                    magic_random = 0.005;
                                } 
                                magic_chance = 50e-12 * luck_value * magic_random;
                            }

                            double2 oil_chance = 0;
                            double2 oil_random = 0;
                            if (luck_value >= gs.res_table["采矿"]["石油"].get_luck_req())
                            {
                                oil_random = -0.3 + 1.3 * double2.Pow(a.NextDouble(), 2);
                                if (oil_random < 0)
                                {
                                    oil_random = 0;
                                }
                                else if (oil_random < 0.005)
                                {
                                    oil_random = 0.005;
                                }
                                oil_chance = 1e-12 * double2.Pow(luck_value, 2.0 / 3.0) * oil_random;
                            }


                            List<double2> resources = new List<double2>();
                            List<double2> randoms = new List<double2>();
                            List<string> names = new List<string>();
                            foreach(KeyValuePair<string, treasure> kp in gs.treasures)
                            {
                                double2 am = 0;
                                double2 random = 0;
                                treasure tr = kp.Value;
                                if(luck_value >= tr.get_luck_req())
                                {
                                    double2 rev_chance = 5 * double2.Pow(tr.get_luck_req(), 0.25);
                                    random = (1 - rev_chance) + rev_chance * a.NextDouble();
                                    if (random < 0)
                                    {
                                        random = 0;
                                    }
                                    am = random * tr.div * cell_size * double2.Pow(luck_value, 1.0 / 3.0);
                                    am = new double2(double_floor(am.d), am.i);
                                }
                                if (am > 0)
                                {
                                    resources.Add(am);
                                    randoms.Add(random);
                                    names.Add(tr.name);
                                }
                            }

                            res["泥土方块"] = total * dirt_chance;
                            res["石头方块"] = total * (stone_up_chance - coal_chance - copper_chance - iron_chance); 
                            res["煤"] = total / y_down * coal_chance;
                            res["铜矿"] = total * copper_chance;
                            res["铁矿"] = total * iron_chance;
                            res["魔石"] = total * magic_chance;
                            res["石油"] = total / y_down * oil_chance;
                            
                            for(int p = names.Count - gs.tr_show; p < names.Count; p++)
                            {
                                if (p < 0)
                                {
                                    continue;
                                }
                                res[names[p]] = resources[p];
                                res["石头方块"] -= resources[p];
                            }

                            List<string> temp = new List<string>();
                            foreach(KeyValuePair<string, double2> kp in res)
                            {
                                if (kp.Value <= 0)
                                {
                                    temp.Add(kp.Key);
                                }
                            }
                            foreach(string s in temp)
                            {
                                res.Remove(s);
                            }
                            l.Add(res);

                            // 泥土 255 127 0
                            // 石头 175 175 175
                            // 煤   150 150 150
                            // 铜矿 255 255 127
                            // 铁矿 200 66  40
                            // 魔石 55  255  255
                            // 石油 150 150 150
                            double r = (255 * dirt_chance.d + 175 * stone_up_chance.d) / 2;
                            double g = (127 * dirt_chance.d + 175 * stone_up_chance.d) / 2;
                            double b = 175 * stone_up_chance.d / 2;

                            //例如 coal 0.5      copper 0.4       iron 0.25
                            // all = 1.15     dirt_stone_percent = -0.15
                            // coal = 0.5 / 1.15
                            // copper = 0.4 / 1.15
                            // iron = 0.25 / 1.15
                            double all = coal_random.d + copper_random.d + iron_random.d + oil_random.d;
                            double coal_percent = coal_random.d;
                            double copper_percent = copper_random.d;
                            double iron_percent = iron_random.d;
                            double magic_percent = magic_random.d;
                            double oil_percent = oil_random.d;

                            double dirt_stone_percent = 1 - all;
                            if (dirt_stone_percent < 0)
                            {
                                coal_percent = coal_random.d / all;
                                copper_percent = copper_random.d / all;
                                iron_percent = iron_random.d / all;
                                magic_percent = magic_random.d / all;
                                oil_percent = oil_random.d / all;
                                dirt_stone_percent = 0;
                                all = 1;
                            }

                            r = (dirt_stone_percent * r) + (coal_percent * 150) + (copper_percent * 255) + (iron_percent * 200) + (magic_percent * 55) + (oil_percent * 100);
                            g = (dirt_stone_percent * g) + (coal_percent * 150) + (copper_percent * 255) + (iron_percent * 66) + (magic_percent * 255) + (oil_percent * 100);
                            b = (dirt_stone_percent * b) + (coal_percent * 150) + (copper_percent * 127) + (iron_percent * 40) + (magic_percent * 255) + (oil_percent * 100);


                            rr.Add((byte)r);
                            gg.Add((byte)g);
                            bb.Add((byte)b);
                        }
                        graph[i, j] = new mine_cell(1, max_depth, luck, c, e, l, rr, gg, bb);
                    }
                }
            }
        }mine minef = new mine();

        [Serializable]
        public class mineP
        {
            public double2 mine_amount = 0;

            public double2 old_level = 0;

            public double2 level = 0;
            public double2 exp = 0;
            public double2 exp_need_base = 5;
            public double2 exp_increment = 1;
            public double2 exp_exponment = 2; //(5 + n) ^ 2
            public Dictionary<string, double2> save = new Dictionary<string, double2>();

            public double2 exp_boost = 0;
            public List<Dictionary<string, multiplier>> exp_multi = new List<Dictionary<string, multiplier>>();

            public double2 luck_boost = 0;
            public double2 luck_addition = 0.1;
            public double2 luck_exponent = 1.02;
            public List<Dictionary<string, multiplier>> luck_multi = new List<Dictionary<string, multiplier>>();

            public double2 size_boost = 0;
            public double2 size_exponent = 1.1;
            public List<Dictionary<string, multiplier>> size_multi = new List<Dictionary<string, multiplier>>();

            public double2 point_boost = 0;
            public List<Dictionary<string, multiplier>> point_multi = new List<Dictionary<string, multiplier>>();

            public double2 point_cost_down = 1;
            public double2 minep_time_down = 1;

            public double2 mine_point_time_base = 5;
            public double2 mine_point_time_exponment = 0.5;
            public double2 mine_point_progress = 0;
            public double2 next_mine_point = 5;
            public double2 mine_point_get = 1;

            public double2 mine_point_got = 0;

            public double2 depth_luck_exponent = 1.8;
            public double2 reset_cost = 5;

            public bool unlocked = false;
            public mineP reseter;

            public mineP()
            {
                
            }

            public void init()
            {
                reseter = new mineP();
                
                for(int i = 0; i < 3; i++)
                {
                    exp_multi.Add(new Dictionary<string, multiplier>());
                    luck_multi.Add(new Dictionary<string, multiplier>());
                    size_multi.Add(new Dictionary<string, multiplier>());
                    point_multi.Add(new Dictionary<string, multiplier>());
                }
            }

            public void update()
            {
                next_mine_point = mine_point_time_base + double2.Pow(mine_point_got, mine_point_time_exponment);
                next_mine_point /= minep_time_down;
                level_update();
            }

            public double2 get_exp_mul()
            {
                double2 x = 1;
                foreach (Dictionary<string, multiplier> d in exp_multi)
                {
                    foreach (KeyValuePair<string, multiplier> kp in d)
                    {
                        x *= kp.Value.value;
                    }
                }
                return x;
            }
            public double2 get_luck_mul()
            {
                double2 x = 1;
                foreach (Dictionary<string, multiplier> d in luck_multi)
                {
                    foreach (KeyValuePair<string, multiplier> kp in d)
                    {
                        x *= kp.Value.value;
                    }
                }
                return x;
            }
            public double2 get_size_mul()
            {
                double2 x = 1;
                foreach (Dictionary<string, multiplier> d in size_multi)
                {
                    foreach (KeyValuePair<string, multiplier> kp in d)
                    {
                        x *= kp.Value.value;
                    }
                }
                return x;
            }
            public double2 get_point_mul()
            {
                double2 x = 1;
                foreach (Dictionary<string, multiplier> d in point_multi)
                {
                    foreach (KeyValuePair<string, multiplier> kp in d)
                    {
                        x *= kp.Value.value;
                    }
                }
                return x;
            }

            public double2 get_exp_to_level(double2 n)
            {
                return double2.Pow(exp_need_base + (level + n - 1) * exp_increment,
                    exp_exponment + 0.15 * ((level + n - 1) / 10));
            }

            public void level_up(double2 n)
            {
                exp -= get_exp_to_level(n);
                level++;
                level_update();
            }

            public void level_update()
            {
                luck_boost += (level - old_level) * luck_addition;
                old_level = level;

                if (!luck_multi[0].ContainsKey("等级"))
                {
                    luck_multi[0].Add("等级", new multiplier(true, 1));
                }
                luck_multi[0]["等级"].value = double2.Pow(luck_exponent, level);

                if (!size_multi[0].ContainsKey("等级"))
                {
                    size_multi[0].Add("等级", new multiplier(true, 1));
                }
                size_multi[0]["等级"].value = double2.Pow(size_exponent, level);
                
            }

            public void gain_exp(double2 e)
            {
                exp += e * get_exp_mul();
                while (exp >= get_exp_to_level(1))
                {
                    try_level();
                }
            }

            public void try_level()
            {
                //目标count 425
                double2 count = 1;
                double2 times = exp / get_exp_to_level(count);
                if(times <= 1000000)
                {
                    level_up(1);
                    return;
                }
                while (times > 1000000)
                {
                    count *= 2;
                    times = exp / get_exp_to_level(count);
                }
                //count = 512
                while (times > 1000000 || times < 100)
                {
                    double2 count2 = 1;
                    //count = 512 -> 511 -> 509 -> 505 -> 497 -> 481 -> 449 -> 385
                    //count = 448 -> 447 -> 445 -> 441 -> 433 -> 417
                    //count = 432 -> 431 -> 429 -> 425[]
                    while (times < 100)
                    {
                        count -= count2;
                        times = exp / get_exp_to_level(count);
                        count2 *= 2;
                    }

                    count2 = 1;
                    //count = 385 -> 386 -> 388 -> 392 -> 400 -> 416 -> 448
                    //count = 417 -> 418 -> 420 -> 424 -> 432
                    while (times > 1000000)
                    {
                        count += count2;
                        times = exp / get_exp_to_level(count);
                        count2 *= 2;
                    }
                }
                level_up(count);
            }


            public void to_level(double2 n)
            {
                if (level < n)
                {
                    level = n;
                    exp = 0;
                    level_update();
                }
            }

            public void reset()
            {
                mine_point_progress = 0;
                next_mine_point = reseter.next_mine_point;
                mine_point_get = reseter.mine_point_get;
                mine_point_got = 0;
                minep_time_down = reseter.minep_time_down;

                old_level = 0;

                level = reseter.level;
                exp = reseter.exp;
                exp_need_base = reseter.exp_need_base;
                exp_increment = reseter.exp_increment;
                exp_exponment = reseter.exp_exponment;

                mine_point_time_base = reseter.mine_point_time_base;
                mine_point_time_exponment = reseter.mine_point_time_exponment;
                depth_luck_exponent = reseter.depth_luck_exponent;
                reset_cost = reseter.reset_cost;

                exp_boost = reseter.exp_boost;

                luck_boost = reseter.luck_boost;
                luck_addition = reseter.luck_addition;
                luck_exponent = reseter.luck_exponent;

                size_boost = reseter.size_boost;
                size_exponent = reseter.size_exponent;

                point_boost = reseter.point_boost;
            }
        }mineP minep = new mineP();


        [Serializable]
        public class heater_x_recipe
        {
            public string a_name;

            public string b1_name;
            public double2 b1_product;

            public string b2_name;
            public double2 b2_product;

            public double2 fire_req;
            public double2 base_speed_req;

            public heater_x_recipe(string A_NAME, string B1_NAME, string B2_NAME,
                double2 B1_PRODUCT, double2 B2_PRODUCT, double2 FIRE_REQ, double2 BASE_SPEED_REQ)
            {
                a_name = A_NAME;
                b1_name = B1_NAME;
                b2_name = B2_NAME;
                b1_product = B1_PRODUCT;
                b2_product = B2_PRODUCT;
                fire_req = FIRE_REQ;
                base_speed_req = BASE_SPEED_REQ;
            }
        }
        Dictionary<string, heater_x_recipe> x_recipes = new Dictionary<string, heater_x_recipe>();

        [Serializable]
        public class heater_y_recipe
        {
            public string a_name;

            public double2 fire_product;

            public double2 fire_req;
            public decimal level_req;
            public double2 base_speed_req;

            public heater_y_recipe(string A_NAME, double2 FIRE_PRODUCT, double2 FIRE_REQ,
                decimal LEVEL_REQ, double2 BASE_SPEED_REQ)
            {
                a_name = A_NAME;
                fire_product = FIRE_PRODUCT;
                fire_req = FIRE_REQ;
                level_req = LEVEL_REQ;
                base_speed_req = BASE_SPEED_REQ;
            }
        }
        Dictionary<string, heater_y_recipe> y_recipes = new Dictionary<string, heater_y_recipe>();

        [Serializable]
        public class heater_x
        {
            public heater_x_recipe recipe;
            public double2 amount;
            public double2 max_amount;
            public bool open = true;

            public heater_x(heater_x_recipe x, double2 MAX_AMOUNT)
            {
                recipe = x;
                amount = MAX_AMOUNT;
                max_amount = MAX_AMOUNT;
            }
        }

        [Serializable]
        public class heater_y
        {
            public heater_y_recipe recipe;
            public double2 amount;
            public double2 max_amount;
            public bool open = true;

            public heater_y(heater_y_recipe y, double2 MAX_AMOUNT)
            {
                recipe = y;
                amount = MAX_AMOUNT;
                max_amount = MAX_AMOUNT;
            }
        } 

        [Serializable]
        public class heater
        {
            public double2 fire = 100;
            public double2 fire_min = 100;
            public double2 fire_drop = 0.1;
            public decimal level = 0;

            public double2 x_speed_factor = 1;
            public double2 y_speed_factor = 1;

            public List<heater_x> xs = new List<heater_x>();
            public List<heater_y> ys = new List<heater_y>();

            public heater reseter;

            public void init()
            {
                reseter = new heater();
            }

            public bool add_x(heater_x x)
            {
                if(xs.Count < 4)
                {
                    xs.Add(x);
                    return true;
                }
                return false;
            }

            public bool add_y(heater_y y)
            {
                if (ys.Count < 4)
                {
                    ys.Add(y);
                    return true;
                }
                return false;
            }
            //删除在按键处

            public int get_current_x_index()
            {
                int r = -1;
                for(int i = xs.Count - 1; i >= 0; i--)
                {
                    if (xs[i].open && xs[i].amount > 0 && fire >= xs[i].recipe.fire_req)
                    {
                        r = i;
                    }
                }
                return r;
            }

            public heater_x get_current_x()
            {
                int index = get_current_x_index();
                if(index == -1)
                {
                    return null;
                }
                return xs[index];
            }

            public int get_current_y_index()
            {
                int r = -1;
                for (int i = ys.Count - 1; i >= 0; i--)
                {
                    if (ys[i].open && ys[i].amount > 0 && fire >= ys[i].recipe.fire_req && level >= ys[i].recipe.level_req)
                    {
                        r = i;
                    }
                }
                return r;
            }

            public heater_y get_current_y()
            {
                int index = get_current_y_index();
                if (index == -1)
                {
                    return null;
                }
                return ys[index];
            }

            public double2 get_x_speed()
            {
                return fire * x_speed_factor;
            }

            public double2 get_y_speed()
            {
                return double2.Pow(fire, 0.5) * y_speed_factor;
            }

            public double2 get_x_loss()
            {
                if (get_current_x_index() == -1)
                {
                    return 0;
                }
                return get_x_speed() / xs[get_current_x_index()].recipe.base_speed_req;
            }

            public double2 get_y_loss()
            {
                if (get_current_y_index() == -1)
                {
                    return 0;
                }
                return get_y_speed() / ys[get_current_y_index()].recipe.base_speed_req;
            }

            public string index_convert_to_string(int i)
            {
                if (i == -1)
                {
                    return "无";
                }
                else return Convert.ToString(i + 1);
            }

            public void reset()
            {
                xs.Clear();
                ys.Clear();

                fire = 0;
                level = reseter.level;
                fire_drop = reseter.fire_drop;
                fire_min = reseter.fire_min;
                x_speed_factor = reseter.x_speed_factor;
                y_speed_factor = reseter.y_speed_factor;

            }
        }heater furance = new heater();

        [Serializable]
        public class treasure : resource
        {
            public double2 div;
            public bool is_treasure = true;
            public treasure(string name, SolidColorBrush brush, double2 luckreq, 
                double2 div_, int LOC = 1, double val = 0): base(LOC, val, name, brush)
            {
                luck_req = luckreq;
                div = div_;
            }
        }
        Dictionary<string, treasure> treasures = new Dictionary<string, treasure>();

        [Serializable]
        public class achievement_level
        {
            public string des;
            public double2 reward;

            public achievement_level(string description, double2 award)
            {
                des = description;
                reward = award;
            }
        }

        [Serializable]
        public class achievement
        {
            public string name;
            public string des;
            public int curr_level = 0;
            public int up_level = 0;
            public int max_level = 0;
            public List<achievement_level> levels = new List<achievement_level>();

            public achievement(string nam, string des_ = "")
            {
                name = nam;
                des = des_;
            }

            public void add_level(achievement_level x)
            {
                levels.Add(x);
                max_level++;
            }
        }
        int total_up_levels = 0;
        Dictionary<string, achievement> achievements_name = new Dictionary<string, achievement>();
        Dictionary<int, achievement> achievements_id = new Dictionary<int, achievement>();

        [Serializable]
        class configs
        {

        }


        [NonSerialized]
        public DispatcherTimer ticker = new DispatcherTimer();

        public Dictionary<string, Dictionary<string, resource>> res_table =
            new Dictionary<string, Dictionary<string, resource>>();

        Dictionary<string, block_producter> block_producters =
            new Dictionary<string, block_producter>();

        Dictionary<string, upgrade> upgrades = new Dictionary<string, upgrade>();
        Dictionary<string, spell> spells = new Dictionary<string, spell>();
        Dictionary<string, link> links = new Dictionary<string, link>();

        Dictionary<string, Dictionary<string, enemy>> enemies = new Dictionary<string, Dictionary<string, enemy>>();
        Dictionary<string, enchant> enchants = new Dictionary<string, enchant>();

        [NonSerialized]
        Grid[,] minefield;
        [NonSerialized]
        Rectangle[,] minefield_bg;
        [NonSerialized]
        TextBlock[,] minefield_texts;
        [NonSerialized]
        Rectangle[,,] minefield_tr;
        [NonSerialized]
        Rectangle[,] minefield_cover;

        [NonSerialized]
        Grid[,] achievefield;
        [NonSerialized]
        Rectangle[,] achievefield_bg;
        [NonSerialized]
        TextBlock[,] achievefield_texts;
        [NonSerialized]
        Rectangle[,] achievefield_cover;

        [NonSerialized]
        Grid[,] achievefield_hint;
        [NonSerialized]
        Rectangle[,] achievefield_hint_bg;
        [NonSerialized]
        TextBlock[,] achievefield_hint_texts;

        [NonSerialized]
        List<Rectangle> 制造_options = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 战斗_options = new List<Rectangle>();

        [NonSerialized]
        List<List<Rectangle>> 战斗_enemies = new List<List<Rectangle>>();
        [NonSerialized]
        List<Rectangle> 战斗_洁白世界_enemies = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 战斗_草原_enemies = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 战斗_死火山_enemies = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 战斗_机关屋_enemies = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 战斗_魔境_enemies = new List<Rectangle>();

        [NonSerialized]
        List<Rectangle> 战斗_自动攻击风格 = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 魔法_options = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 采矿_options = new List<Rectangle>();
        [NonSerialized]
        List<Rectangle> 娱乐_options = new List<Rectangle>();

        List<string> 矿物 = new List<string>();
        List<treasure> 宝物 = new List<treasure>(); 

        /*调整游戏速度*/
        int tick_time = 5;
        double2 game_speed_base = new double2(1, 0);
        double2 energy_game_speed = new double2(0, 0);
        private double2 gamespeed()
        {
            return game_speed_base + energy_game_speed;
        }

        bool buy_int = true;
        int buy_number = 1;
        double buy_percent = 0;
        private void change_buy_config(Rectangle r)
        {
            if (r.Name.Substring(4, 1) == "x")
            {
                buy_int = true;
                buy_number = Convert.ToInt32(r.Name.Split('x')[1]);
            }
            else
            {
                buy_int = false;
                string s = r.Name.Split('_')[1];
                if (s == "01")
                {
                    buy_percent = 0.1;
                }
                else if (s == "max")
                {
                    buy_percent = 100.0;
                }
                else
                {
                    buy_percent = Convert.ToInt32(s);
                }
            }
        }

        private double2 get_can_use(double2 x)
        {
            if (buy_int)
            {
                return x;
            }
            return x * buy_percent / 100.0;
        }




        //no.1 方块
        #region
        private void 方块生产器升级(block_producter bp, decimal add_level_no_cost = -100)
        {
            decimal up_level = add_level_no_cost;
            if (up_level == 0)
            {
                return;
            }
            if (add_level_no_cost == -100)
            {
                resource r = find_resource(bp.cost_res_type);

                double2 use = r.get_value();
                double2 need = bp.cost;
                int buy_level = 1;
                if (buy_int && buy_number > 1)
                {
                    need = need * (double2.Pow(bp.cost_exponent, buy_number) - 1) / (bp.cost_exponent - 1);
                    buy_level = buy_number;
                }
                else if (!buy_int)
                {
                    double2 next_cost = bp.cost * bp.cost_exponent;
                    use *= (buy_percent / 100.0);
                    while (use > need + next_cost)
                    {
                        buy_level++;
                        need += next_cost;
                        next_cost *= bp.cost_exponent;
                    }
                }

                if (use < need)
                {
                    return;
                }

                r.add_value(-need);
                up_level = buy_level;
            }

            decimal old_level = bp.level;
            bp.level += up_level;
            bp.cost *= double2.Pow(bp.cost_exponent, up_level);
            bp.max_time *= double2.Pow(bp.level_up_time_factor, up_level);
            bp.max_value *= double2.Pow(bp.level_up_production_factor, up_level);

            if (bp.best < bp.level)
            {
                bp.best = bp.level;
            }

            if (bp.milestone != 0)
            {
                decimal reach_ms = decimal.Floor(bp.level / bp.milestone) - decimal.Floor(old_level / bp.milestone);
                bp.max_time *= double2.Pow(bp.milestone_effect_time_factor, reach_ms);
                bp.max_value *= double2.Pow(bp.milestone_effect_production_factor, reach_ms);
            }
            if (bp.milestone2 != 0)
            {
                decimal reach_ms = decimal.Floor(bp.level / bp.milestone2) - decimal.Floor(old_level / bp.milestone2);
                bp.max_time *= double2.Pow(bp.milestone_effect_time_factor2, reach_ms);
                bp.max_value *= double2.Pow(bp.milestone_effect_production_factor2, reach_ms);
            }
        }
        private void 方块生产器收集(block_producter bp)
        {
            resource r = res_table["方块"][bp.name];
            
            if (bp.current_value < bp.max_value)
            {
                r.add_value(bp.current_value * time_power());
                bp.current_value = 0;
                bp.current_time = 0;
            }
            else
            {
                double2 times = bp.current_time / bp.max_time;
                double2 count = new double2(double_floor(times.d), times.i);
                r.add_value(bp.max_value * time_power() * count);
                bp.current_time -= bp.max_time * count;
                if (bp.current_time < 0)
                {
                    bp.current_time = 0;
                }
                bp.current_value = bp.max_value * double2.Pow((bp.current_time / bp.max_time), bp.production_exponent);
            }
        }
        #endregion


        //no.2 制造
        #region
        private double2 can_buy_material_num(upgrade u)
        {
            resource r = find_resource(u.get_auto_res());
            double2 can_use = get_can_use(r.get_value());

            double buy = 1;
            if (buy_int)
            {
                buy = buy_number;
                if (can_use < u.factor * double2.Pow(buy, u.exponent))
                {
                    return 0;
                }
                return buy;
            }
            else
            {
                if (can_use < u.factor)
                {
                    return 0;
                }
                else
                {
                    return double2.Pow((can_use / u.factor), 1 / u.exponent);
                }
            }
        }

        //double2 n = 0
        private double2 buy_material_cost(upgrade u, double2 n)
        {
            if (n == 0)
            {
                n = can_buy_material_num(u);
            }
            if (n == 0)
            {
                return 0;
            }
            else
            {
                return u.factor * double2.Pow(n, u.exponent);
            }
        }

        private void buy_material(upgrade u)
        {
            double2 n = can_buy_material_num(u);
            if (u.material)
            {
                double2 cost = u.factor * double2.Pow(n, u.exponent);
                resource r = find_resource(u.name);
                r.add_value(n, true);
                resource co = find_resource(u.get_auto_res());
                co.add_value(-cost);
            }
        }

        //制作升级时填表
        private List<List<Tuple<string, double2>>> get_auto_cost_table(string res_name)
        {
            Tuple<string, double2> t = new Tuple<string, double2>(res_name, 0);
            List<Tuple<string, double2>> l = new List<Tuple<string, double2>>();
            l.Add(t);
            List<List<Tuple<string, double2>>> ll = new List<List<Tuple<string, double2>>>();
            ll.Add(l);
            return ll;
        }

        private List<Tuple<string, double2>> upgrade_cost_adder(List<Tuple<string, double2>> l, Tuple<string, double2> t)
        {
            if (l == null)
            {
                return new List<Tuple<string, double2>>();
            }
            l.Add(t);
            return l;
        }

        //能买的最大升级数
        private int can_buy_upgrade_num(upgrade u)
        {
            int buy = 0;
            Dictionary<string, double2> list_res = new Dictionary<string, double2>();
            while (true)
            {
                if (u.level + buy == u.max_level || (buy_int && buy >= buy_number))
                {
                    return buy;
                }
                foreach (Tuple<string, double2> l in u.cost_table[(int)(u.level + buy)])
                {
                    resource need = find_resource(l.Item1);
                    if (!list_res.ContainsKey(need.name))
                    {
                        double2 can_use = get_can_use(need.get_value());
                        list_res.Add(need.name, can_use);
                    }
                    list_res[need.name] -= l.Item2;
                    if (list_res[need.name] < 0)
                    {
                        return buy;
                    }
                }
                buy++;
            }
        }

        //当前选项是否能买
        private bool can_buy_upgrade(upgrade u)
        {
            return can_buy_upgrade_num(u) == 0 || (buy_int && (can_buy_upgrade_num(u) < buy_number));
        }

        //购买n个升级需要的资源  double2 n = 0 购买最大可能值
        private Dictionary<string, double2> buy_upgrade_cost(upgrade u, double2 n)
        {
            if (n == 0)
            {
                n = can_buy_upgrade_num(u);
            }
            if (n == 0)
            {
                return null;
            }
            else
            {
                Dictionary<string, double2> list_res = new Dictionary<string, double2>();
                for (int i = 0; i < n; i++)
                {
                    if (u.level + i == u.max_level)
                    {
                        break;
                    }
                    foreach (Tuple<string, double2> l in u.cost_table[(int)u.level + i])
                    {
                        resource need = find_resource(l.Item1);
                        if (!list_res.ContainsKey(need.name))
                        {
                            list_res.Add(need.name, 0);
                        }
                        list_res[need.name] += l.Item2;
                    }
                }
                return list_res;
            }
        }

        //将需要的资源转为字符串
        private string upgrade_string_show(Dictionary<string, double2> d)
        {
            string ret = "";
            if (d == null)
            {
                return ret;
            }
            foreach (KeyValuePair<string, double2> kp in d)
            {
                ret = ret + "\n   " + number_format(kp.Value) + " " + kp.Key;
            }
            return ret;
        }

        //购买升级
        private void buy_upgrade(upgrade u, int n = -1)
        {
            int old_level = u.level;
            int buy_level = 0;
            if (n == -1)
            {
                buy_level = can_buy_upgrade_num(u);
            }
            else
            {
                buy_level = n;
            }
            Dictionary<string, double2> list_res = buy_upgrade_cost(u, 0);
            if (list_res == null)
            {
                return;
            }
            foreach (KeyValuePair<string, double2> kp in list_res)
            {
                resource r = find_resource(kp.Key);
                r.add_value(-kp.Value);
            }
            u.level += buy_level;

            if (u.best < u.level)
            {
                u.best = u.level;
            }

            for (decimal i = old_level + 1; i <= u.level; i++)
            {
                upgrade_effect(u, i);
            }
        }

        //白嫖升级
        #region
        private void buy_upgrade_no_cost(upgrade u, int to_level)
        {
            int old_level = u.level;
            u.level = Math.Max(old_level, to_level);

            if (u.best < u.level)
            {
                u.best = u.level;
            }

            for (decimal i = old_level + 1; i <= u.level; i++)
            {
                upgrade_effect(u, i);
            }
        }
        #endregion
        #endregion


        //no.3 战斗
        #region
        private bool fighting = false;
        private void change_enemy(enemy e)
        {
            fighting = false;
            m.战斗_场景_information_fight_背景.Fill = getSCB(Color.FromRgb(0, 255, 195));
            m.战斗_场景_information_fight_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));

            enemy.curr_field = e.field;
            enemy.current = e;
            e.curr_health = e.health;
            连斩_count = 0;
        }

        private void enemy_show()
        {
            enemy e = enemy.current;
            m.战斗_场景_information_pos.Text = e.field;
            m.战斗_场景_information_pos.Foreground = e.text_color();

            m.战斗_场景_information_enemyname.Text = e.name;
            m.战斗_场景_information_enemyname.FontSize = 18;
            m.战斗_场景_information_enemyname.HorizontalAlignment = HorizontalAlignment.Center;
            m.战斗_场景_information_enemyname.Margin = new Thickness(0, 15, 0, 0);
            if (you.attack_repeat > 1)
            {
                m.战斗_场景_information_enemyname.Text += " (×" + number_format(you.attack_repeat) + ")";
                m.战斗_场景_information_enemyname.FontSize = 14;
                m.战斗_场景_information_enemyname.HorizontalAlignment = HorizontalAlignment.Left;
                m.战斗_场景_information_enemyname.Margin = new Thickness(100, 15, 0, 0);
            }

            m.战斗_场景_information_enemyname.Foreground = e.text_color();

            m.战斗_场景_information_level.Text = "等级 " + number_format(e.level) + " / " + number_format(e.max_level);
            m.战斗_场景_information_level.Foreground = e.text_color();

            m.战斗_场景_information_health.Text = "生命值: " + number_format(e.curr_health) + " / " + number_format(e.health);
            m.战斗_场景_information_health.Foreground = e.text_color();

            m.战斗_场景_information_进度条_顶.Width = 200 * Math.Min((enemy.current.curr_health / enemy.current.health).d, 1);

            m.战斗_场景_information_进度条_顶.Fill = e.text_color();

            double2 exp_drop_mul = 1;
            if (prestige_ups["战斗增幅"].level >= 6)
            {
                exp_drop_mul = 3;
            }
            if (prestige_ups["战斗增幅"].level >= 9)
            {
                exp_drop_mul = 6;
            }
            if (prestige_ups["战斗增幅"].level >= 11)
            {
                exp_drop_mul = 60;
            }
            exp_drop_mul *= global_xp_boost();

            m.战斗_场景_information_exp.Text = "经验值: " + number_format(e.exp * you.get_exp_mul() * exp_drop_mul * you.attack_repeat);
            m.战斗_场景_information_exp.Foreground = e.text_color();

            m.战斗_场景_information_loot1.Text = "";
            m.战斗_场景_information_loot2.Text = "";
            m.战斗_场景_information_loot3.Text = "";

            int count = 0;
            foreach (KeyValuePair<string, double2> kp in e.curr_level_drop)
            {
                count++;
                string res = kp.Key;

                double2 level_mul = 1;
                if (prestige_ups["强化等级"].level >= 5)
                {
                    if (res == "白色方块" || res == "白色粉末")
                    {
                        level_mul = double2.Pow(1.015, you.level);
                    }
                    else
                    {
                        level_mul = double2.Pow(1.003, you.level);
                    }
                }

                bool uncast_boost = prestige_ups["转化"].level >= 3;
                double2 uncast_effect = get_spell_uncast_boost();
                if (((spell)upgrades["白色魔法"]).current_active_lv >= 7)
                {
                    double2 boost = 0.5;
                    if (!((spell)upgrades["白色魔法"]).cast_active)
                    {
                        if (uncast_boost)
                        {
                            boost *= uncast_effect;
                        }
                        else
                        {
                            boost = 0;
                        }
                    }
                    level_mul *= (1 + boost);
                }

                string amount = number_format(kp.Value * find_resource(res).get_mul() * level_mul * you.attack_repeat);
                if (count == 1)
                {
                    m.战斗_场景_information_loot1.Text = "掉落物: " + amount + " " + res;
                }
                if (count == 2)
                {
                    m.战斗_场景_information_loot2.Text = amount + " " + res;
                }
                if (count == 3)
                {
                    m.战斗_场景_information_loot3.Text = amount + " " + res;
                }
            }

            m.战斗_场景_information_loot1.Foreground = e.text_color();
            m.战斗_场景_information_loot2.Foreground = e.text_color();
            m.战斗_场景_information_loot3.Foreground = e.text_color();



            m.战斗_场景_information_slow.Text = "减速: " + number_format(e.slow);
            if (prestige_ups["冷静"].level == 1)
            {
                m.战斗_场景_information_slow.Text = "减速: " + number_format(e.slow / 300);
            }
            if (prestige_ups["冷静"].level >= 2)
            {
                m.战斗_场景_information_slow.Text = "减速: " + number_format(e.slow / 60);
            }
            m.战斗_场景_information_slow.Foreground = e.text_color();

            m.战斗_场景_information_defense.Text = "防御: " + number_format(e.defense);
            m.战斗_场景_information_defense.Foreground = e.text_color();

            m.战斗_场景_information_regen.Text = "回复: " + number_format(e.regen);
            m.战斗_场景_information_regen.Foreground = e.text_color();

            m.战斗_场景_information_介绍.Text = "       " + e.des;
            m.战斗_场景_information_介绍.Foreground = e.text_color();

            m.战斗_场景_information_grid.Visibility = 0;

            if (e.level == 1)
            {
                m.战斗_场景_information_leveldown_grid.Visibility = (Visibility)1;
            }
            else
            {
                m.战斗_场景_information_leveldown_grid.Visibility = 0;
            }

            if (e.level == e.max_level)
            {
                m.战斗_场景_information_levelup_grid.Visibility = (Visibility)1;
            }
            else
            {
                m.战斗_场景_information_levelup_grid.Visibility = 0;
            }

        }

        private void player_show()
        {
            double2 reg = 0;
            double2 damage_multi = you.auto_attack_form.attack_factor;
            if (you.auto_attack_form.name == "连斩")
            {
                damage_multi = 连斩_count * 连斩_damage_boost + 连斩_damage_base;
                if(damage_multi > 连斩_max_damage)
                {
                    damage_multi = 连斩_max_damage;
                }
            }
            else
            {
                连斩_count = 0;
            }

            double2 attack = you.get_attack() * damage_multi;
            double2 true_dmg = double2.Min(attack, attack * (you.auto_attack_form.def_pierce_percent + you.pierce_percent) + you.auto_attack_form.def_pierce);
            double2 remain_dmg = attack - true_dmg;
            double2 all_dmg = true_dmg;

            if (enemy.current != null)
            {
                if (remain_dmg > 0)
                {
                    double2 true_def = (enemy.current.defense * (1 - you.auto_attack_form.def_ignore_percent) - you.auto_attack_form.def_ignore);
                    if (true_def < 0)
                    {
                        true_def = 0;
                    }
                    if (remain_dmg - true_def > 0)
                    {
                        all_dmg += remain_dmg - true_def;
                    }
                    reg = enemy.current.regen;
                }
            }
            else
            {
                all_dmg = attack;
            }

            m.战斗_玩家_等级属性.Text = "战斗等级 " + number_format(you.level);

            m.战斗_玩家_经验_text.Text = "经验值: " + number_format(you.exp) + " / " + number_format(you.get_exp_to_level(1));
            m.战斗_玩家_经验_进度条_顶.Width = m.战斗_玩家_经验_进度条_底.Width * Math.Min(((you.exp / you.get_exp_to_level(1))).d, 1);

            m.战斗_玩家_攻击属性_0.Text = "攻击力: " + number_format(attack);
            m.战斗_玩家_攻击属性_0x.Text = "伤害: " + number_format(all_dmg);
            m.战斗_玩家_攻击属性_1.Text = "基础攻击: " + number_format(you.base_attack);
            m.战斗_玩家_攻击属性_2.Text = "物品增益攻击: +" + number_format(you.item_attack);
            m.战斗_玩家_攻击属性_3.Text = "等级增益攻击: +" + number_format(you.level_attack);
            m.战斗_玩家_攻击属性_4.Text = "等级强化: ×" + number_format(you.level_attack_factor);
            m.战斗_玩家_攻击属性_5.Text = "其他强化: ×" + number_format(you.other_attack_factor.get_mul());
            m.战斗_玩家_攻击属性_6.Text = "攻击风格: ×" + number_format(damage_multi);

            m.战斗_玩家_攻击间隔属性_0.Text = "攻击时间: " + number_format(you.get_attack_time()) + "s";
            m.战斗_玩家_攻击间隔属性_1.Text = "基础攻击时间: " + number_format(you.attack_time_base) + "s";
            m.战斗_玩家_攻击间隔属性_2.Text = "物品强化: /" + number_format(you.item_at_factor);
            m.战斗_玩家_攻击间隔属性_3.Text = "等级强化: /" + number_format(you.level_at_factor);
            m.战斗_玩家_攻击间隔属性_4.Text = "其他强化: /" + number_format(you.other_at_factor);
            m.战斗_玩家_攻击间隔属性_5.Text = "减速效果: ×" + number_format(you.slow_value);
            m.战斗_玩家_攻击间隔属性_6.Text = "攻击风格: ×" + number_format(you.auto_attack_form.at_factor);

            m.战斗_玩家_减速属性_0.Text = "减速恢复: " + number_format(you.get_sr()) + "/s";
            m.战斗_玩家_减速属性_0x.Text = "减速值: " + number_format(you.slow_value);
            m.战斗_玩家_减速属性_1.Text = "基础速度: " + number_format(you.sr_base);
            m.战斗_玩家_减速属性_2.Text = "物品增益速度: " + number_format(you.item_sr);
            m.战斗_玩家_减速属性_3.Text = "等级增益速度: " + number_format(you.level_sr);
            m.战斗_玩家_减速属性_4.Text = "强化: ×" + number_format(you.sr_factor);
            m.战斗_玩家_减速属性_5.Text = "攻击风格: ×" + number_format(you.auto_attack_form.sr_factor);

            m.战斗_玩家_每秒伤害.Text = "伤害: " + number_format(all_dmg / you.get_attack_time()) + "/s";
            m.战斗_玩家_每秒敌人受损.Text = "敌人受损: " + number_format(all_dmg / you.get_attack_time() - reg) + "/s";

            if (you.attack_repeat > 1)
            {
                m.战斗_玩家_攻击重数.Text = "攻击重数: " + number_format(you.attack_repeat);
            }
            else
            {
                m.战斗_玩家_攻击重数.Text = "";
            }

            m.战斗_玩家_前一击敌人死亡数.Text = "前一击敌人死亡数: " + number_format(last_enemy_die);

            double2 death_count = 0;
            foreach (Tuple<double2, double2> t in die_time_spot)
            {
                death_count += t.Item2;
            }
            m.战斗_玩家_前10秒敌人死亡数.Text = "前 10 秒敌人死亡数: " + number_format(death_count / 10.0) + "/s";

        }

        private void change_enemy_level(double2 i)
        {
            enemy e = enemy.current;
            e.set_level(e.level + i);
        }

        //double2 count = 1
        private void enemy_die(double2 count)
        {
            enemy.current.set_level(enemy.current.level);

            连斩_count = 0;
            last_enemy_die += count;
            die_time_spot.Add(new Tuple<double2, double2>(time_all_acutally, count));

            health_change(-1, 0);
            if (enemy.current.name == "陨石" && upgrades["镐"].level == 0)
            {
                return;
            }

            foreach (KeyValuePair<string, double2> kp in enemy.current.curr_level_drop)
            {
                string res = kp.Key;

                double2 level_mul = 1;
                if (prestige_ups["强化等级"].level >= 5)
                {
                    if (res == "白色方块" || res == "白色粉末")
                    {
                        level_mul = double2.Pow(1.015, you.level);
                    }
                    else
                    {
                        level_mul = double2.Pow(1.003, you.level);
                    }
                }

                bool uncast_boost = prestige_ups["转化"].level >= 3;
                double2 uncast_effect = get_spell_uncast_boost();
                if (((spell)upgrades["白色魔法"]).current_active_lv >= 7)
                {
                    double2 boost = 0.5;
                    if (!((spell)upgrades["白色魔法"]).cast_active)
                    {
                        if (uncast_boost)
                        {
                            boost *= uncast_effect;
                        }
                        else
                        {
                            boost = 0;
                        }
                    }
                    level_mul *= (1 + boost);
                }

                double2 amount = kp.Value * level_mul * you.attack_repeat * count;

                resource r = find_resource(res);
                r.add_value(amount);
            }

            double2 exp_drop_mul = 1;
            if(prestige_ups["战斗增幅"].level >= 6)
            {
                exp_drop_mul = 3;
            }
            if (prestige_ups["战斗增幅"].level >= 9)
            {
                exp_drop_mul = 6;
            }
            if (prestige_ups["战斗增幅"].level >= 11)
            {
                exp_drop_mul = 60;
            }
            exp_drop_mul *= global_xp_boost();

            you.gain_exp(enemy.current.exp * exp_drop_mul * you.attack_repeat * count);

            if (enemy.current.name == "白色粒子")
            {
                m.魔法.Visibility = 0;
            }
            if (enemy.current.name == "土丘")
            {
                upgrades["祭坛升级"].unlocked = true;
                m.魔法_祭坛_升级0_grid.Visibility = 0;
            }
        }

        private double2 health_change(double2 damage, double2 times)
        {
            if (damage == -1)
            {
                enemy.current.curr_health = enemy.current.health;
                return 0;
            }
            else
            {
                damage *= times;
                if (hit_start)
                {
                    hit_count += times;
                }
                double2 remain = damage - enemy.current.curr_health;

                enemy.current.curr_health -= damage;
                if (enemy.current.curr_health <= 0)   //攻击高
                {
                    enemy_die(1);
                    return remain;
                }
                //血量高
                return 0;
            }
        }

        double2 last_enemy_die = 0;
        List<Tuple<double2, double2>> die_time_spot = new List<Tuple<double2, double2>>();
        double2 overkill_count = 0;
        double2 hit_count = 0;
        bool hit_start = false;

        //double2 count = 1
        private void attack(attack_form af, double2 count)
        {
            if (count >= 1)
            {
                last_enemy_die = 0;
            }
            if (enemy.current != null)
            {
                double2 old_level = you.level;
                overkill_count = 0;
                hit_count = 0;
                hit_start = false;
                double2 next_exec = 1;
                double2 curr_exec = 1;
                for (double2 i = 0; i < count; i += next_exec)
                {
                    curr_exec = next_exec;
                    if (enemy.current.health == enemy.current.curr_health)
                    {
                        hit_start = true;
                    }


                    if (af.manual && af.mafcc_cur_time < af.manual_anti_fast_click_cd)
                    {
                        return;
                    }
                    else
                    {
                        af.mafcc_cur_time = 0;
                    }

                    double2 damage_multi = af.attack_factor;
                    if (af.name == "连斩")
                    {
                        damage_multi = 连斩_count * 连斩_damage_boost + 连斩_damage_base;
                        if (damage_multi > 连斩_max_damage)
                        {
                            damage_multi = 连斩_max_damage;
                        }
                        if (count > 10000000)
                        {
                            连斩_count += 10000000;
                        }
                        if (连斩_count < 10000000)
                        {
                            连斩_count += curr_exec;
                        }
                    }
                    else if (!af.manual)
                    {
                        连斩_count = 0;
                    }

                    double2 attack = you.get_attack() * damage_multi;

                    //100攻击 200防御 30%+10穿甲   =   30+10穿甲    40伤害
                    double2 true_dmg = double2.Min(attack, attack * (af.def_pierce_percent + you.pierce_percent) + af.def_pierce);
                    double2 remain_dmg = attack - true_dmg;
                    double2 all_dmg = true_dmg;
                    if (remain_dmg > 0)
                    {
                        //100攻击 200防御 60%+15无视护甲    -135防御  -> 65防御   35伤害
                        double2 true_def = (enemy.current.defense * (1 - af.def_ignore_percent) - af.def_ignore);
                        if (true_def < 0)
                        {
                            true_def = 0;
                        }
                        if (remain_dmg - true_def > 0)
                        {
                            all_dmg += remain_dmg - true_def;
                        }
                    }


                    double2 remain = health_change(all_dmg, curr_exec);

                    if (remain > 0)
                    {
                        overkill_count++;

                        if (af.overkill > 0)
                        {
                            overkill_count--;
                            while (remain > 0)
                            {
                                overkill_count++;
                                remain = health_change(remain * af.overkill, 1);
                            }
                        }

                    }
                    else
                    {
                        enemy.current.defense *= 1 - af.def_down_percent;
                        enemy.current.defense -= af.def_down;
                    }



                    prestige_upgrade p = prestige_ups["冷静"];
                    double2 manual_factor = 1;
                    if (af.manual && af.sr_factor != 0)
                    {
                        manual_factor = af.sr_factor;
                    }


                    if (hit_count > 10 * curr_exec && overkill_count == 0)
                    {
                        next_exec *= 2;
                    }

                    if (hit_start && overkill_count > 0)
                    {
                        double2 curr_count = 1;
                        double2 remain_count = count - i - 1;     //3 - 0 - 1 = 2;
                        if (remain_count * overkill_count / hit_count > 10)
                        {
                            double2 real_count = remain_count;
                            double2 death_ratio = overkill_count / hit_count;
                            enemy_die(real_count * death_ratio);
                            curr_exec += real_count;
                            i += real_count;
                            remain_count = 0;
                        }
                        else
                        {
                            while (remain_count >= double2.Abs(double2.Min(curr_count, count * 1e-10)))
                            {
                                double2 real_count = double2.Min(curr_count, remain_count);
                                double2 death_ratio = overkill_count / hit_count;
                                enemy_die(real_count * death_ratio);

                                curr_exec += real_count;
                                i += real_count;
                                remain_count = count - i - 1;     //3 - 0 - 1 = 2;

                                if (old_level < you.level)
                                {
                                    old_level = you.level;
                                    break;
                                }
                                curr_count *= 2;
                            }
                        }


                        hit_start = false;
                        hit_count = 0;
                        overkill_count = 0;
                    }
                    if (p.level == 0)
                    {
                        you.slow_value += enemy.current.slow * manual_factor * curr_exec;
                    }
                    if (p.level == 1)
                    {
                        you.slow_value += enemy.current.slow * manual_factor / 300 * curr_exec;
                    }
                    if (p.level >= 2)
                    {
                        you.slow_value += enemy.current.slow * manual_factor / 60 * curr_exec;
                    }

                    if (enemy.curr_field == "草原")
                    {
                        if (upgrades["铲子"].level == 1)
                        {
                            res_table["方块"]["泥土方块"].add_value(10 * you.attack_repeat * curr_exec);
                        }
                        if (upgrades["铲子"].level == 2)
                        {
                            res_table["方块"]["泥土方块"].add_value(10e3 * you.attack_repeat * curr_exec);
                        }
                    }
                    if (upgrades["铲子"].level == 3)
                    {
                        res_table["方块"]["泥土方块"].add_value(100e6 * you.attack_repeat * curr_exec);
                    }
                    if (upgrades["铲子"].level == 4)
                    {
                        res_table["方块"]["泥土方块"].add_value(1e12 * you.attack_repeat * curr_exec);
                    }
                    you.攻击次数 += you.attack_repeat * curr_exec;
                }
            }
        }
        #endregion

        //no.4 魔法
        #region
        private double2 alter_effect()
        {
            double2 ret = 0;
            if (magic_altar.mode == "魔力祭坛")
            {
                ret = double2.Pow(magic_altar.power, magic_altar.mana_exponent) * magic_altar.mana_factor * magic_altar.get_speed_mul();
            }
            return ret;
        }

        //p.i 行数   p.j 页数
        public void spell_add(spell s, position p)
        {
            upgrades.Add(s.name, s);
            spells.Add(s.name, s);
            if (p != null)
            {
                s.pos = p;
                spell_all_page_update(p.j);
            }
        }

        int spell_page = 1;
        int spell_max_page = 1;
        int spell_all_page = 1;
        private void spell_page_update(int x)
        {
            spell_max_page = Math.Max(spell_max_page, x);
        }
        private void spell_unlock(string name)
        {
            spell s = spells[name];
            s.unlocked = true;
            visual_unlock("魔法_法术_" + name + "_grid");
            spell_page_update(s.pos.j);
        }
        private void spell_all_page_update(int x)
        {
            spell_all_page = Math.Max(spell_all_page, x);
        }
        private void spell_page_show()
        {
            for(int i = 1; i <= spell_all_page; i++)
            {
                Grid g = (Grid)m.FindName("魔法_法术_第" + Convert.ToString(i) + "页_grid");
                if(i == spell_page)
                {
                    g.Visibility = Visibility.Visible;
                }
                else
                {
                    g.Visibility = Visibility.Hidden;
                }
            }
            ((TextBlock)m.FindName("魔法_法术_页号_text")).Text = "第 " + Convert.ToString(spell_page) +
                " / " + Convert.ToString(spell_max_page) + " 页";

            Grid r = ((Grid)m.FindName("魔法_法术_翻页前_grid"));
            if(spell_page == 1)
            {
                r.Visibility = Visibility.Hidden;
            }
            else
            {
                r.Visibility = Visibility.Visible;
            }

            r = ((Grid)m.FindName("魔法_法术_翻页后_grid"));
            if (spell_page == spell_max_page)
            {
                r.Visibility = Visibility.Hidden;
            }
            else
            {
                r.Visibility = Visibility.Visible;
            }
        }
        #endregion

        //no.5
        #region
        int tr_page = 1;
        int tr_max_page = 1;
        int tr_all_page = 2;
        private void tr_page_update(int x)
        {
            tr_max_page = Math.Max(tr_max_page, x);
        }
        private void tr_page_show()
        {
            for (int i = 1; i <= tr_all_page; i++)
            {
                Grid g = (Grid)m.FindName("采矿_宝物_第" + Convert.ToString(i) + "页_grid");
                if (i == tr_page)
                {
                    g.Visibility = Visibility.Visible;
                }
                else
                {
                    g.Visibility = Visibility.Hidden;
                }
            }
            ((TextBlock)m.FindName("采矿_宝物_页号_text")).Text = "第 " + Convert.ToString(tr_page) +
                " / " + Convert.ToString(tr_max_page) + " 页";

            Grid r = ((Grid)m.FindName("采矿_宝物_翻页前_grid"));
            if (tr_page == 1)
            {
                r.Visibility = Visibility.Hidden;
            }
            else
            {
                r.Visibility = Visibility.Visible;
            }

            r = ((Grid)m.FindName("采矿_宝物_翻页后_grid"));
            if (tr_page == tr_max_page)
            {
                r.Visibility = Visibility.Hidden;
            }
            else
            {
                r.Visibility = Visibility.Visible;
            }
        }
        #endregion

        //no.6
        #region
        #endregion

        //no.7
        #region
        #endregion

        //no.8
        #region
        #endregion





        //no.9 转生
        #region
        Dictionary<string, prestige_upgrade> prestige_ups = new Dictionary<string, prestige_upgrade>();
        //计算转生点数
        private double2 cal_pp_gain()
        {
            resource wb = res_table["方块"]["白色方块"];
            pp_gain = double2.Pow(wb.get_value(), 1 / 3.0);
            if (prestige_ups["对数增益"].level == 1)
            {
                pp_gain += (wb.get_value() + 1).Log10() / Math.Log10(2);
            }
            if (prestige_ups["对数增益"].level == 2)
            {
                pp_gain += double2.Pow((wb.get_value() + 1).Log10() / Math.Log10(2), 2);
            }
            if (prestige_ups["对数增益"].level == 3)
            {
                pp_gain += double2.Pow((wb.get_value() + 1).Log10() / Math.Log10(2), 3);
            }
            if (prestige_ups["对数增益"].level == 4)
            {
                pp_gain += double2.Pow((wb.get_value() + 1).Log10() / Math.Log10(2), 4);
            }
            return pp_gain;
        }

        private void reset_res(resource r)
        #region 资源保留
        {
            prestige_upgrade p = prestige_ups["资源保留"];
            if (p.level == 0)
            {
                r.set_value(0);
                r.prestige_get = 0;
                return;
            }

            double2 keep_pow = 0.7;
            double2 keep_value = 200;
            if (p.level == 2)
            {
                keep_pow = 0.75;
                keep_value = 1000;
            }
            if (p.level == 3)
            {
                keep_pow = 0.8;
                keep_value = 5000;
            }
            if (p.level == 4)
            {
                keep_pow = 0.825;
                keep_value = 30000;
            }
            if (p.level >= 5)
            {
                keep_pow = 0.85;
                keep_value = 100000;
            }
            if (r.get_value() <= keep_value)
            {
                r.prestige_get = r.get_value();
            }
            else
            {
                r.set_value(keep_value + double2.Pow(r.get_value() - keep_value, keep_pow));
                r.prestige_get = keep_value + double2.Pow(r.get_value() - keep_value, keep_pow);
            }
        }
        #endregion

        private void reset_tr(treasure r)
        {
            r.set_value(0);
            r.prestige_get = 0;
        }

        private void prestige_base()
        {
            resource pp = res_table["转生"]["转生点数"];
            resource wb = res_table["方块"]["白色方块"];
            pp.add_value(cal_pp_gain());

            time_this_prestige_game = 0;

            foreach (KeyValuePair<string, Dictionary<string, resource>> kp1 in res_table)
            {
                foreach (KeyValuePair<string, resource> kp2 in kp1.Value)
                {
                    resource r = kp2.Value;
                    foreach (KeyValuePair<string, multiplier> kp3 in r.multipliers)
                    {
                        kp3.Value.reset();
                    }
                }
            }


            prestige_upgrade p = prestige_ups["资源保留"];
            //方块 重置
            #region
            foreach (KeyValuePair<string, resource> r in res_table["方块"])
            {
                if (p.level == 0)
                {
                    r.Value.set_value(0);
                    r.Value.prestige_get = 0;
                    if (r.Key == "白色方块")
                    {
                        r.Value.set_value(1);
                        r.Value.prestige_get = 1;
                    }
                }
                else
                {
                    reset_res(r.Value);
                }
            }
            foreach (KeyValuePair<string, block_producter> kp in block_producters)
            {
                block_producter bp = kp.Value;
                bp.reseter.level = 0;
                bp.reset();
                decimal target = 0;
                if (prestige_ups["升级保留"].level >= 1)
                {
                    target = decimal.Floor(bp.best * (decimal)0.75);
                }
                if (prestige_ups["升级保留"].level >= 3)
                {
                    target = decimal.Floor(bp.best * (decimal)0.9);
                }
                target = Math.Max(target, bp.level);
                方块生产器升级(bp, target);
            }
            foreach (Grid g in m.方块_grid.Children)
            {
                string[] strs = g.Name.Split('_');
            }
            #endregion

            //制造 重置
            #region
            foreach (KeyValuePair<string, resource> r in res_table["制造"])
            {
                reset_res(r.Value);
            }
            #endregion

            //战斗 重置
            #region
            foreach (KeyValuePair<string, resource> r in res_table["战斗"])
            {
                reset_res(r.Value);
            }

            foreach(KeyValuePair<string, attack_form> kp in attack_forms)
            {
                kp.Value.reset();
            }


            if (fighting)
            {
                m.战斗_场景_information_fight_背景.Fill = getSCB(Color.FromRgb(0, 255, 195));
                m.战斗_场景_information_fight_文字.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                fighting = false;
            }
            连斩_count = 0;
            连斩_damage_base = 连斩_damage_base_reseter;
            连斩_damage_boost = 连斩_damage_boost_reseter;
            连斩_max_damage = 连斩_max_damage_reseter;
            if (enemy.current != null)
            {
                health_change(-1, 0);
            }


            you.reset();
            #endregion

            //魔法 重置
            #region
            foreach (KeyValuePair<string, resource> r in res_table["魔法"])
            {
                reset_res(r.Value);
            }
            foreach (KeyValuePair<string, enchant> r in enchants)
            {
                r.Value.reset();
            }
            foreach (Grid g in m.魔法_菜单_附魔_target_grid.Children)
            {
                string[] strs = g.Name.Split('_');
                string cover_name = "魔法_附魔_" + strs[3];

                ((Rectangle)m.FindName(cover_name + "_背景")).Fill = getSCB(Color.FromRgb(0, 255, 195));
                ((TextBlock)m.FindName(cover_name + "_文字")).Foreground = getSCB(Color.FromRgb(0, 0, 0));
                ((TextBlock)m.FindName(cover_name + "_文字")).Text = "开始附魔";
            }
            foreach (Grid g in m.魔法_菜单_药水_target_grid.Children)
            {
                string[] strs = g.Name.Split('_');
                string cover_name = "魔法_药水_" + strs[3];

                ((Rectangle)m.FindName(cover_name + "_背景")).Fill = getSCB(Color.FromRgb(0, 255, 195));
                ((TextBlock)m.FindName(cover_name + "_文字")).Foreground = getSCB(Color.FromRgb(0, 0, 0));
            }
            foreach (KeyValuePair<string, spell> kp1 in spells)
            {
                foreach (KeyValuePair<string, multiplier> kp2 in ((spell)kp1.Value).cost_downs)
                {
                    kp2.Value.reset();
                }
                foreach (KeyValuePair<string, multiplier> kp2 in ((spell)kp1.Value).speed_ups)
                {
                    kp2.Value.reset();
                }
            }

            foreach (KeyValuePair<string, enchant> kp1 in enchants)
            {
                foreach (KeyValuePair<string, multiplier> kp2 in (kp1.Value).effect_ups)
                {
                    kp2.Value.reset();
                }
                foreach (KeyValuePair<string, multiplier> kp2 in (kp1.Value).speed_ups)
                {
                    kp2.Value.reset();
                }
                foreach (KeyValuePair<string, multiplier> kp2 in (kp1.Value).cost_downs)
                {
                    kp2.Value.reset();
                }
            }

            foreach (KeyValuePair<string, multiplier> kp2 in magic_altar.power_ups)
            {
                kp2.Value.reset();
            }
            foreach (KeyValuePair<string, multiplier> kp2 in magic_altar.speed_ups)
            {
                kp2.Value.reset();
            }

            foreach (KeyValuePair<string, multiplier> kp2 in you.exp_gain_multipliers)
            {
                kp2.Value.reset();
            }

            magic_altar.reset();
            #endregion

            //采矿 重置
            #region
            foreach (KeyValuePair<string, resource> r in res_table["采矿"])
            {
                reset_res(r.Value);
            }
            foreach (Dictionary<string, multiplier> d in minep.exp_multi)
            {
                foreach (KeyValuePair<string, multiplier> kp in d)
                {
                    kp.Value.reset();
                }
            }
            foreach (Dictionary<string, multiplier> d in minep.luck_multi)
            {
                foreach (KeyValuePair<string, multiplier> kp in d)
                {
                    kp.Value.reset();
                }
            }
            foreach (Dictionary<string, multiplier> d in minep.size_multi)
            {
                foreach (KeyValuePair<string, multiplier> kp in d)
                {
                    kp.Value.reset();
                }
            }
            foreach (Dictionary<string, multiplier> d in minep.point_multi)
            {
                foreach (KeyValuePair<string, multiplier> kp in d)
                {
                    kp.Value.reset();
                }
            }
            minep.reset();
            mine_regenerate();
            furance.reset();
            old_minep_level = 0;

            foreach (KeyValuePair<string, treasure> kp in treasures)
            {
                reset_tr(kp.Value);
            }
            #endregion

            //升级 重置
            foreach (KeyValuePair<string, upgrade> kp in upgrades)
            #region
            {
                upgrade u = kp.Value;
                if (prestige_ups["升级保留"].level >= 2 && !(u is spell))
                {
                    u.reseter.level = u.best - 1;
                }
                if (u.can_reset)
                {
                    u.reset();
                    for(int i = 1; i <= u.reseter.level; i++)
                    {
                        upgrade_effect(u, i);
                    }
                }

                if (u is spell)
                {
                    if (prestige_ups["升级保留"].level >= 3)
                    {
                        u.reseter.level = u.best;
                        for (int i = 1; i <= u.reseter.level; i++)
                        {
                            upgrade_effect(u, i);
                        }
                    }
                    if (u.can_reset)
                    {
                        ((spell)u).spell_reset();
                    }
                }
            }
            #endregion

            //特殊 重置
            if (prestige_ups["资源保留"].level >= 6)
            {
                reset_res(res_table["特殊"]["能量"]);
            }
            else
            {
                res_table["特殊"]["能量"].set_value(0);
            }
            //res_table["特殊"]["高阶能量"].set_value(0);
            //res_table["特殊"]["终极能量"].set_value(0);
            ex.reset();
        }

        #endregion


        private void level_potion_time_reduce()
        {
            if (prestige_ups["强化等级"].level <= 6)
            {
                return;
            }
            foreach (KeyValuePair<string, enchant> kp in enchants)
            {
                enchant ec = kp.Value;
                if (ec.is_potion)
                {
                    if (!ec.speed_ups.ContainsKey("等级"))
                    {
                        ec.speed_ups.Add("等级", new multiplier(true, 1));
                    }
                    ec.speed_ups["等级"].value = double2.Pow(1.005, you.level + minep.level);
                }
            }
        }

        private void fight_update()
        {

            int min = 10;
            int max = 0;
            int i = 0;
            foreach (Grid g in m.战斗_玩家_攻击风格_自动_grid.Children)
            {
                g.Visibility = (Visibility)1;

                //战斗_玩家_攻击风格_自动_普通_grid
                string name = g.Name.Split('_')[4];
                if (attack_forms[name].unlocked)
                {
                    g.Visibility = 0;
                    min = Math.Min(min, i);
                    max = Math.Max(max, i);
                }
                i++;
            }

            m.战斗_玩家_攻击风格_线.X1 = 40 + 80 * min;
            m.战斗_玩家_攻击风格_线.X2 = 40 + 80 * max;


            foreach (Grid g in m.战斗_玩家_攻击风格_手动_grid.Children)
            {
                g.Visibility = (Visibility)1;

                //战斗_玩家_攻击风格_手动_喷雾_grid
                string name = g.Name.Split('_')[4];
                if (attack_forms[name].unlocked)
                {
                    g.Visibility = 0;
                }
            }
        }

        private void mine_regenerate()
        {
            minef.cell_size = (1000 + minep.size_boost) * minep.get_size_mul();
            minef.luck = (1 + minep.luck_boost) * minep.get_luck_mul();
            minef.exp = (5 + minep.exp_boost) * minep.get_exp_mul();
            minef.point_cost_down = minep.point_cost_down;
            minef.generate();
            get_field();

            minep.mine_amount++;
        }
        
    }
 }
