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
        [Serializable]
        public class Yggdrasill : EXP
        {
            public double2 exp_need_now = 20;
            public EXP_BAR exp_bar;

            public double2 exp_gain = 0;
            public double2 produce_ex = 2;

            public Yggdrasill reseter;

            public Yggdrasill() : base("世界树", false)
            {
                level = 1;
                base_exp = 20;
                exp_bar = new EXP_BAR("世界树等级");
            }

            public void init_reseter()
            {
                reseter = new Yggdrasill();
                reseter.level = 1;
                reseter.base_exp = 20;
            }

            public double2 produce()
            {
                return double2.Pow(level, produce_ex);
            }


            public override double2 get_exp_to_level(double2 n)
            {
                if (sumable)
                {
                    //TODO
                    return 0;
                }
                else  // n = 1
                {
                    if (n != 1)
                    {
                        throw new Exception();
                    }
                    return exp_need_now;
                }
            }

            public override void update()
            {
                if (sumable)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    exp_need_now *= 1 + (3 / double2.Pow(level, 0.5));
                }
            }

            public void upgrade()
            {
                if (sumable)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    level++;
                    exp_need_now *= 1 + (3 / double2.Pow(level, 0.5));
                }
            }

            public void downgrade()
            {
                if (sumable)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    exp_need_now /= 1 + (3 / double2.Pow(level, 0.5));
                    level--;
                }
            }

            public void set_level(decimal target)
            {
                if (sumable)
                {
                    throw new NotImplementedException();
                }
                double2 temp = level;

                if (temp < target)
                {
                    for(int i = 0; i < target - temp; i++)
                    {
                        upgrade();
                    }
                }
                if (temp > target)
                {
                    for (int i = 0; i < temp - target; i++)
                    {
                        downgrade();
                    }
                }
            }

            public void reset()
            {
                level = reseter.level;
                exp_gain = reseter.exp_gain;
                current_exp = reseter.current_exp;
                base_exp = reseter.base_exp;
                exp_need_now = reseter.base_exp;
            }
        }
        Yggdrasill yggdrasill = new Yggdrasill();

        [Serializable]
        public abstract class g1_seed_base : EXP
        {
            public EXP_BAR exp_bar;
            public double2 exp_base_gain;
            public double2 exp_need_now;
            public double2 n_seed = 1;

            public g1_level.type d;
            public muls exp_base_mul = new muls();
            public muls produce_mul = new muls();

            public g1_seed_base reseter;

            public abstract void init_reseter();
            public abstract double2 produce();
            public double2 get_exp_gain()
            {
                if (n_seed <= 0)
                {
                    return 0;
                }
                double2 b = n_seed * exp_base_gain * exp_base_mul.get_mul() + get_exp_add();
                return b * get_exp_mul();
            }
            public override double2 get_exp_to_level(double2 n)
            {
                return (S(level + n) - S(level)) * n_seed;
            }
            public abstract double2 S(double2 n);
            public void set_level(double2 target)
            {
                level = target;
                update();
            }

            public void reset()
            {
                level = reseter.level;
                exp_base_gain = reseter.exp_base_gain;
                current_exp = reseter.current_exp;
                base_exp = reseter.base_exp;
                exp_need_now = reseter.base_exp;
            }

            public g1_seed_base(string name) : base(name, true)
            {

            }
        }

        [Serializable]
        public class g1_seed : g1_seed_base
        {
            public g1_seed(g1_level.type diff) : base("种子")
            {
                level = 0;
                exp_base_gain = 1;
                base_exp = 1 * Math.Pow(100, (int)diff);
                d = diff;
                exp_need_now = base_exp;
                exp_bar = new EXP_BAR("种子等级");
            }

            public override void init_reseter()
            {
                reseter = new g1_seed(d);
                reseter.level = 0;
                reseter.exp_base_gain = 1;
                reseter.base_exp = 1 * Math.Pow(100, (int)d);
                reseter.exp_need_now = reseter.base_exp;
            }

            public override double2 produce()
            {
                return ((3 - (int)d) + level * (0.05 * Math.Pow(5, (int)d))) * 
                    produce_mul.get_mul();
            }
            public override double2 S(double2 n)
            {
                return (n * (n + 1) * (2 * n + 1) / 6) * Math.Pow(100, (int)d);
            }
            public override void update()
            {
                exp_need_now = ((level + 1) * (level + 1)) * Math.Pow(100, (int)d);
                exp_need_now *= n_seed;
            }
        }
        g1_seed seed = new g1_seed(0);

        [Serializable]
        public class g1_sapling : g1_seed_base
        {
            public g1_sapling(g1_level.type diff) : base("树苗")
            {
                level = 0;
                exp_base_gain = 100;
                base_exp = 1000 * Math.Pow(400, (int)diff);
                exp_need_now = base_exp;
                d = diff;
                exp_bar = new EXP_BAR("树苗等级");
            }

            public override void init_reseter()
            {
                reseter = new g1_sapling(d);
                reseter.level = 0;
                reseter.exp_base_gain = 100;
                reseter.base_exp = 1000 * Math.Pow(400, (int)d);
                reseter.exp_need_now = reseter.base_exp;
            }
            public override double2 produce()
            {
                return ((10 * Math.Pow(4, (int)d)) +
                    double2.Pow(level, 2) 
                    * (0.1 * Math.Pow(12, (int)d))) * produce_mul.get_mul();
            }
            public override double2 S(double2 n)
            {
                double2 b = 1000 * double2.Pow(n, 4);
                b += (100.0 / 3.0) * (-n + 10 * double2.Pow(n, 3) -
                    15 * double2.Pow(n, 4) + 6 * double2.Pow(n, 5));
                return b * Math.Pow(400, (int)d);
            }
            public override void update()
            {
                exp_need_now = double2.Pow(10 * (level + 1), 4) / 10  * 
                    Math.Pow(400, (int)d) * n_seed;
            }
        }
        g1_sapling sapling = new g1_sapling(0);

        [Serializable]
        public class g1_smalltree : g1_seed_base
        {
            public g1_smalltree(g1_level.type diff) : base("小树")
            {
                level = 0;
                exp_base_gain = 200000;
                base_exp = 1e9 * Math.Pow(20000, (int)diff);
                exp_need_now = base_exp;
                d = diff;
                exp_bar = new EXP_BAR("小树等级");
            }

            public override void init_reseter()
            {
                reseter = new g1_smalltree(d);
                reseter.level = 0;
                reseter.exp_base_gain = 200000;
                reseter.base_exp = 1e9 * Math.Pow(20000, (int)d);
                reseter.exp_need_now = reseter.base_exp;
            }
            public override double2 produce()
            {
                return ((1200 * Math.Pow(12, (int)d)) +
                    double2.Pow(level, 4)
                    * (10 * Math.Pow(100, (int)d))) * produce_mul.get_mul();
            }
            public override double2 S(double2 n)
            {
                double2 b = 1e9 * double2.Pow(n, 8);
                b += 1e9 * (1.0 / 90.0) * 
                    (-3 * n + 
                    20 * double2.Pow(n, 3) -
                    42 * double2.Pow(n, 5) + 
                    60 * double2.Pow(n, 7) -
                    45 * double2.Pow(n, 8) +
                    10 * double2.Pow(n, 9));
                return b * Math.Pow(20000, (int)d);
            }
            public override void update()
            {
                exp_need_now = double2.Pow(level + 1, 8) * 1e9 *
                    Math.Pow(20000, (int)d) * n_seed;
            }
        }
        g1_smalltree smalltree = new g1_smalltree(0);

        // 大 sizable
        
    }
}
