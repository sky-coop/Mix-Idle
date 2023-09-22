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
        [Serializable]
        public abstract class EXP
        {
            public string name;

            public double2 level;

            public double2 current_exp = 0;
            public double2 base_exp;

            public bool sumable;
            public Dictionary<string, multiplier> exp_gain_adders = new Dictionary<string, multiplier>();
            public Dictionary<string, multiplier> exp_gain_multipliers = new Dictionary<string, multiplier>();
            public Dictionary<string, multiplier> produce_muls = new Dictionary<string, multiplier>();

            public double2 get_exp_add()
            {
                double2 x = 0;
                foreach (KeyValuePair<string, multiplier> kp in exp_gain_adders)
                {
                    x += kp.Value.value;
                }
                return x;
            }

            public double2 get_exp_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in exp_gain_multipliers)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public double2 get_produce_mul()
            {
                double2 x = 1;
                foreach (KeyValuePair<string, multiplier> kp in produce_muls)
                {
                    x *= kp.Value.value;
                }
                return x;
            }

            public EXP(string n, bool a)
            {
                name = n;
                sumable = a;
            }

            public abstract double2 get_exp_to_level(double2 n);

            public abstract void update();

            public void gain_exp(double2 e, double2 max)
            {
                double2 old = level;
                current_exp += e;
                double2 pb = double2.Max(1, level * 1e-15);
                double2 p = double2.Max(1, level * 1e-15);
                bool direction = false;
                double2 con = 1;
                while (current_exp >= get_exp_to_level(double2.Max(1, level * 1e-15))
                    && old + max > level)
                {
                    bool b = try_level_up(p);
                    if (b)
                    {
                        if (direction)
                        {
                            con++;
                        }
                        else
                        {
                            con = 1;
                        }
                        direction = true;
                        p *= double2.Pow(2, con);
                    }
                    else
                    {
                        if (!direction)
                        {
                            con++;
                        }
                        else
                        {
                            con = 1;
                        }
                        direction = false;
                        p /= double2.Pow(2, con);
                        p = double2.Max(pb, p);
                    }
                }
            }

            public bool try_level_up(double2 n)
            {
                if (sumable)
                {
                    double2 exp_need = get_exp_to_level(n);
                    if (current_exp >= exp_need)
                    {
                        current_exp -= exp_need;
                        level += n;
                        update();
                        return true;
                    }
                    return false;
                }
                else
                {
                    bool b = true;
                    for (double2 i = 0; b && i < n; i++)
                    {
                        b = try_level_up_1();
                    }
                    return b;
                }
            }

            public bool try_level_up_1()
            {
                double2 exp_need = get_exp_to_level(1);
                if (current_exp >= exp_need)
                {
                    current_exp -= exp_need;
                    level += 1;
                    update();
                    return true;
                }
                return false;
            }
        }

        [Serializable]
        public class EXP_BAR
        {
            public string name;

            public drawable draw;
            
            public EXP_BAR(string s)
            {
                name = s;
            }

            public void prepare(string d_name, string d_target,
                HorizontalAlignment d_ha, VerticalAlignment d_va,
                double d_w, double d_h, thickness d_t,
                solid_type d_fill, solid_type d_progress, double d_percent,
                solid_type d_stroke, double d_st, solid_text d_content)
            {
                draw = new drawable(d_name, d_target, d_ha, d_va, d_w, d_h, d_t);
                draw.setFill(d_fill);
                draw.setProgress(d_progress, d_percent);
                drawable.setCRIT(ref draw.t_progress);
                draw.setStroke(d_stroke, d_st);
                draw.setContent_s(d_content);
                drawable.setCRIT(ref draw.t_content);
            }
        }

        [Serializable]
        public class mul_exp : EXP
        {
            public double2 exponment;
            public double2 exp_need_base;
            public double2 exp_need_now;
            public mul_exp(string name, double2 xp_base, double2 exponment)
                : base(name, true)
            {
                this.exponment = exponment;
                level = 0;
                exp_need_base = xp_base;
                exp_need_now = xp_base;
            }

            public override void update()
            {
                exp_need_now = exp_need_base * double2.Pow(exponment, level);
            }
            public double2 S(double2 n)
            {
                return exp_need_base * ((double2.Pow(exponment, n) - 1) /
                    (exponment - 1));
            }

            public override double2 get_exp_to_level(double2 n)
            {
                return S(n + level) - S(level);
            }

            public void reset()
            {
                exp_need_now = exp_need_base;
                level = 0;
            }
        }

        [Serializable]
        public class add_exp : EXP
        {
            public double2 add;
            public double2 exp_need_base;
            public double2 exp_need_now;
            public add_exp(string name, double2 xp_base, double2 add)
                : base(name, true)
            {
                this.add = add;
                level = 0;
                exp_need_base = xp_base;
                exp_need_now = xp_base;
            }

            public override void update()
            {
                exp_need_now = exp_need_base + add * level;
            }
            public new void gain_exp(double2 e, double2 max)
            {
                double2 exp_all = current_exp + e + S(level);
                current_exp += e;
                double2 a = exp_need_base;
                double2 d = add;
                double2 n = (-2 * a + d + 2 *
                    double2.Pow((-a + d / 2) * (-a + d / 2) + 2 * d * exp_all, 0.5))
                    / (2 * d);
                n.d = double_floor(n.d);
                if(n.d < 0)
                {
                    n = 0;
                }
                try_level_up(n - level);
            }

            public double2 S(double2 n)
            {
                return exp_need_base * n + n * (n - 1) * add / 2;
            }

            public override double2 get_exp_to_level(double2 n)
            {
                return S(n + level) - S(level);
            }

            public void reset()
            {
                current_exp = 0;
                exp_need_now = exp_need_base;
                level = 0;
            }
        }

    }
}
