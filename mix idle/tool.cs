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
    //用于读取
    public partial class gamestats
    {
        private List<T> Swap<T>(List<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        private SolidColorBrush color_mula(Brush b, double m, bool positive = true)
        {
            if (!positive)
            {
                m = 1 / m;
            }
            SolidColorBrush c = (SolidColorBrush)b;
            return getSCB(Color.FromArgb(
                (byte)(c.Color.A * m),
                (byte)(c.Color.R * m),
                (byte)(c.Color.G * m),
                (byte)(c.Color.B * m)
                ));
        }

        private SolidColorBrush color_mul(Brush b, double m, bool positive = true)
        {
            if (!positive)
            {
                m = 1 / m;
            }
            SolidColorBrush c = (SolidColorBrush)b;
            return getSCB(Color.FromArgb(
                c.Color.A,
                (byte)(c.Color.R * m),
                (byte)(c.Color.G * m),
                (byte)(c.Color.B * m)
                ));
        }

        private SolidColorBrush color_mul_r(Brush b, double m, bool positive = true)
        {
            if (!positive)
            {
                m = 1 / m;
            }
            SolidColorBrush c = (SolidColorBrush)b;
            return getSCB(Color.FromArgb(
                c.Color.A,
                (byte)(c.Color.R * m),
                c.Color.G,
                c.Color.B
                ));
        }

        public static Point point_spin(int x)
        {
            int n = x % 8;
            if (n == 0)
            {
                return new Point(1, 0);
            }
            if (n == 1)
            {
                return new Point(1, 1);
            }
            if (n == 2)
            {
                return new Point(0, 1);
            }
            if (n == 3)
            {
                return new Point(-1, 1);
            }
            if (n == 4)
            {
                return new Point(-1, 0);
            }
            if (n == 5)
            {
                return new Point(-1, -1);
            }
            if (n == 6)
            {
                return new Point(0, -1);
            }
            if (n == 7)
            {
                return new Point(1, -1);
            }
            return new Point(0, 0);
        }

        public Color HslToRgb(double Hue, double Saturation, double Lightness)
        {
            if (Hue < 0) Hue = 0.0;
            if (Saturation < 0) Saturation = 0.0;
            if (Lightness < 0) Lightness = 0.0;
            if (Hue >= 360) Hue = 359.0;
            if (Saturation > 255) Saturation = 255;
            if (Lightness > 255) Lightness = 255;
            Saturation = Saturation / 255.0;
            Lightness = Lightness / 255.0;
            
            double C = (1 - Math.Abs(2 * Lightness - 1)) * Saturation;
            double hh = Hue / 60.0;
            double X = C * (1 - Math.Abs(hh % 2 - 1));
            double r = 0, g = 0, b = 0;
            if (hh >= 0 && hh < 1)
            {
                r = C;
                g = X;
            }
            else if (hh >= 1 && hh < 2)
            {
                r = X;
                g = C;
            }
            else if (hh >= 2 && hh < 3)
            {
                g = C;
                b = X;
            }
            else if (hh >= 3 && hh < 4)
            {
                g = X;
                b = C;
            }
            else if (hh >= 4 && hh < 5)
            {
                r = X;
                b = C;
            }
            else
            {
                r = C;
                b = X;
            }
            double m = Lightness - C / 2;
            r += m;
            g += m;
            b += m;
            r = r * 255.0;
            g = g * 255.0;
            b = b * 255.0;
            r = Math.Round(r);
            g = Math.Round(g);
            b = Math.Round(b);
            return Color.FromRgb((byte)r, (byte)g, (byte)b);
        }


        public static void add_transform(FrameworkElement f, Transform t)
        {
            Transform old_transform = null;
            if (f.RenderTransform == null)
            {
                f.RenderTransform = new TransformGroup();
            }
            if (!(f.RenderTransform is TransformGroup))
            {
                old_transform = f.RenderTransform;
                f.RenderTransform = new TransformGroup();
            }
            TransformGroup g = (TransformGroup)f.RenderTransform;
            if (old_transform != null)
            {
                g.Children.Add(old_transform);
            }
            foreach (Transform a in g.Children)
            {
                if (t is ScaleTransform && a is ScaleTransform)
                {
                    ScaleTransform st = (ScaleTransform)t;
                    ScaleTransform sa = (ScaleTransform)a;
                    sa.ScaleX *= st.ScaleX;
                    sa.ScaleY *= st.ScaleY;
                    return;
                }
            }
            g.Children.Add(t);
        }

        public static void spin(FrameworkElement f, Transform x)
        {
            Transform old_transform = null;
            if (f.RenderTransform == null)
            {
                f.RenderTransform = new TransformGroup();
            }
            if (!(f.RenderTransform is TransformGroup))
            {
                old_transform = f.RenderTransform;
                f.RenderTransform = new TransformGroup();
            }
            TransformGroup t = (TransformGroup)f.RenderTransform;
            if (old_transform != null)
            {
                t.Children.Add(old_transform);
            }
            foreach (Transform a in t.Children)
            {
                if (a is RotateTransform)
                {
                    RotateTransform r = (RotateTransform)x;
                    RotateTransform s = (RotateTransform)a;
                    s.Angle = r.Angle;
                    s.CenterX = r.CenterX;
                    s.CenterY = r.CenterY;
                    return;
                }
            }
            t.Children.Add(x);
        }

        public static void scale(FrameworkElement f, double mulx, double muly,
            double c_x = 0, double c_y = 0)
        {
            Transform old_transform = null;
            if (f.RenderTransform == null)
            {
                f.RenderTransform = new TransformGroup();
            }
            if (!(f.RenderTransform is TransformGroup))
            {
                old_transform = f.RenderTransform;
                f.RenderTransform = new TransformGroup();
            }
            TransformGroup t = (TransformGroup)f.RenderTransform;
            if (old_transform != null)
            {
                t.Children.Add(old_transform);
            }
            foreach (Transform a in t.Children)
            {
                if (a is ScaleTransform)
                {
                    ScaleTransform s = (ScaleTransform)a;
                    s.ScaleX = mulx;
                    s.ScaleY = muly;
                    if(f.ActualWidth != 0)
                    {
                        s.CenterX = c_x * f.ActualWidth;
                    }
                    if (f.ActualHeight != 0)
                    {
                        s.CenterY = c_y * f.ActualHeight;
                    }
                    return;
                }
            }
            if (f.Width.Equals(double.NaN) || f.Height.Equals(double.NaN))
            {
                t.Children.Add(new ScaleTransform(mulx, muly));
            }
            else
            {
                t.Children.Add(new ScaleTransform(mulx, muly, c_x * f.Width, c_y * f.Height));
            }
        }

        public static void scale_mul(FrameworkElement f, double mulx, double muly)
        {
            Transform old_transform = null;
            if (f.RenderTransform == null)
            {
                f.RenderTransform = new TransformGroup();
            }
            if (!(f.RenderTransform is TransformGroup))
            {
                old_transform = f.RenderTransform;
                f.RenderTransform = new TransformGroup();
            }
            TransformGroup t = (TransformGroup)f.RenderTransform;
            if (old_transform != null)
            {
                t.Children.Add(old_transform);
            }
            foreach(Transform a in t.Children)
            {
                if (a is ScaleTransform)
                {
                    ScaleTransform s = (ScaleTransform)a;
                    s.ScaleX *= mulx;
                    s.ScaleY *= muly;
                    return;
                }
            }
            t.Children.Add(new ScaleTransform(mulx, muly));
        }

        public FrameworkElement get_mouse()
        {
            IInputElement input = Mouse.DirectlyOver;
            if (input != null)
            {
                if (input is FrameworkElement)
                {
                    return input as FrameworkElement;
                }
            }
            return null;
        }

        public void flag_add(ref int x, int n)
        {
            x |= (1 << n);
        }
        public void flag_remove(ref int x, int n)
        {
            x &= ~(1 << n);
        }
        public bool flag_have(int x, int n)
        {
            int nx = (1 << n);
            return (x & nx) == nx;
        }

        public void flag_add(FrameworkElement f, int n)
        {
            if (!(f.Tag is int))
            {
                return;
            }
            int x = (int)f.Tag;
            flag_add(ref x, n);
            f.Tag = x;
        }
        public void flag_remove(FrameworkElement f, int n)
        {
            if (!(f.Tag is int))
            {
                return;
            }
            int x = (int)f.Tag;
            flag_remove(ref x, n);
            f.Tag = x;
        }
        public bool flag_have(FrameworkElement f, int n)
        {
            if(!(f.Tag is int))
            {
                return false;
            }
            return flag_have((int)f.Tag, n);
        }

        [Serializable]
        public struct double2
        {
            public static readonly double2 max = 
                new double2(double.MaxValue, double.MaxValue);

            public static class d_cal
            {
                public static double ls = 0;
                public static double c = 0;
                public static double lm = Math.Log10(double.MaxValue);
                public enum type
                {
                    add = 1,
                    sub = 2,
                    mul = 3,
                    div = 4,
                    pow = 5
                }

                public static void cal(type t, double a, double b)
                {
                    c = 0;
                    switch (t)
                    {
                        case type.add:
                        case type.sub:
                            if (t == type.sub)
                            {
                                b = -b;
                            }
                            if (double.IsInfinity(a + b))
                            {
                                c = 1;
                                a /= double.MaxValue;
                                b /= double.MaxValue;
                            }
                            else
                            {
                                c = 0;
                            }
                            ls = a + b;
                            break;
                        case type.mul:
                        case type.div:
                            int sign = 1;
                            if (a < 0)
                            {
                                a = -a;
                                sign = -sign;
                            }
                            if (b < 0)
                            {
                                b = -b;
                                sign = -sign;
                            }
                            if (a == 0 || b == 0)
                            {
                                if (t == type.div && b == 0)
                                {
                                    throw new Exception();
                                }
                                ls = 0;
                                break;
                            }
                            double la = Math.Log10(a);
                            double lb = Math.Log10(b);
                            if (t == type.div)
                            {
                                lb = -lb;
                            }
                            ls = la + lb;
                            if (ls >= lm)
                            {
                                ls -= lm;
                                c = 1;
                            }
                            if (ls < -lm)
                            {
                                ls += lm;
                                c = -1;
                            }
                            ls = Math.Pow(10, ls) * sign;
                            break;
                        case type.pow:
                            sign = 1;
                            if (a < 0)
                            {
                                throw new Exception();
                            }
                            if (b < 0)
                            {
                                b = -b;
                                sign = -sign;
                            }
                            la = Math.Log10(a);
                            ls = la * b / lm;
                            if (double.IsPositiveInfinity(ls))
                            {
                                ls = la / lm * b;
                            }
                            ls *= sign;
                            c = double_floor(ls);
                            ls -= c;
                            ls = Math.Pow(double.MaxValue, ls);
                            break;
                    }
                }
            }

            public double d;
            public double i;

            public double2(double du, double iu = 0)
            {
                d = du;
                i = iu;
            }

            public static void check(ref double2 n)
            {
                if (n.d == 0)
                {
                    n.i = 0;
                }
                if (double.IsPositiveInfinity(n.d))
                {
                    n.d = 1;
                    n.i++;
                }
                if (double.IsNegativeInfinity(n.d))
                {
                    n.d = -1;
                    n.i++;
                }
                double nd = Math.Abs(n.d);
                if (n.i > 0 && nd > 0 && nd < 1)
                {
                    n.d *= double.MaxValue;
                    n.i--;
                }
                if (n.i < 0 && nd > 1)
                {
                    n.d /= double.MaxValue;
                    n.i++;
                }
            }

            public static double2 copy(double2 n)
            {
                double2 temp = new double2(n.d, n.i);
                return temp;
            }

            public double2 copy()
            {
                double2 temp = new double2(d, i);
                return temp;
            }

            public static double2 operator +(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                double2 t = 0;
                if (a == 0)
                {
                    return b;
                }
                if (b == 0)
                {
                    return a;
                }
                if (a.i > b.i + 1)
                {
                    return copy(a);
                }
                if (a.i < b.i - 1)
                {
                    return copy(b);
                }
                if (a.i == b.i + 1)
                {
                    if (Math.Abs(b.d) < 1)
                    {
                        return copy(a);
                    }
                    else
                    {
                        d_cal.cal(d_cal.type.add, a.d, b.d / double.MaxValue);
                    }
                }
                else if (a.i == b.i - 1)
                {
                    if (Math.Abs(a.d) < 1)
                    {
                        return copy(b);
                    }
                    else
                    {
                        d_cal.cal(d_cal.type.add, a.d / double.MaxValue, b.d);
                    }
                    t.i++;
                }
                else
                {
                    d_cal.cal(d_cal.type.add, a.d, b.d);
                }
                double2 temp = new double2(d_cal.ls, a.i + d_cal.c + t.i);
                check(ref temp);
                return temp;
            }

            public static double2 operator ++(double2 a)
            {
                a += new double2(1, 0);
                return a;
            }

            public static double2 operator -(double2 a, double2 b)
            {
                return a + -b;
            }

            public static double2 operator --(double2 a)
            {
                a -= new double2(1, 0);
                return a;
            }

            public static double2 operator -(double2 a)
            {
                return new double2(-a.d, a.i);
            }

            public static double2 operator *(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                d_cal.cal(d_cal.type.mul, a.d, b.d);
                double2 temp = new double2(d_cal.ls, a.i + b.i + d_cal.c);
                check(ref temp);
                return temp;
            }

            public static double2 operator /(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                d_cal.cal(d_cal.type.div, a.d, b.d);
                double2 temp = new double2(d_cal.ls, a.i - b.i + d_cal.c);
                check(ref temp);
                return temp;
            }

            public static bool operator <(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                if (a.d < 0 && b.d >= 0)
                {
                    return true;
                }
                if (a.d == 0 && b.d > 0)
                {
                    return true;
                }
                if (a.d >= 0 && b.d <= 0)
                {
                    return false;
                }
                if (a.d < 0 && b.d < 0)
                {
                    if (a.i > b.i)
                    {
                        return true;
                    }
                    else if (a.i == b.i)
                    {
                        if (a == b)
                        {
                            return false;
                        }
                        return a.d < b.d;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (a.d > 0 && b.d > 0)
                {
                    if (a.i > b.i)
                    {
                        return false;
                    }
                    else if (a.i == b.i)
                    {
                        if (a == b)
                        {
                            return false;
                        }
                        return a.d < b.d;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;

            }

            public static bool operator <=(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                return a < b || a == b;
            }

            public static bool operator ==(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                /*
                bool eq = false;
                if (a.d == 0 || b.d == 0)
                {
                    eq = a.d == b.d;
                }
                if (Math.Abs(a.d - b.d) < 1e-6 * Math.Abs(a.d))
                {
                    eq = true;
                }*/
                return a.d == b.d && (a.i == b.i);
            }

            public static bool operator >=(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                return !(a < b);
            }
            public static bool operator >(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                return !(a <= b);
            }

            public static bool operator !=(double2 a, double2 b)
            {
                return !(a == b);
            }

            public static double2 Abs(double2 a)
            {
                check(ref a);
                return new double2(Math.Abs(a.d), a.i);
            }

            public static double2 Min(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                if(a < b)
                {
                    return a;
                }
                return b;
                //return (a + b - (Abs(a - b))) / new double2(2, 0);
            }
            public static double2 Max(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                if (a > b)
                {
                    return a;
                }
                return b;
                //return (a + b + (Abs(a - b))) / new double2(2, 0);
            }

            public static double2 Pow(double2 a, double2 b)
            {
                check(ref a);
                check(ref b);
                if (a.d == 0)
                {
                    return 0;
                }
                if (a.i >= 1 && b.i >= 1)
                {
                    throw new Exception();
                }

                double super = 0;
                super = Math.Log10(a.d) / d_cal.lm;
                super *= b.i * double.MaxValue;
                super += a.i * b.d;
                double isuper = double_floor(super);
                super -= isuper;
                super = Math.Pow(double.MaxValue, super);
                if (double.IsInfinity(super))
                {
                    super = double.MaxValue;
                }

                d_cal.cal(d_cal.type.pow, a.d, b.d);
                double c = d_cal.c;
                d_cal.cal(d_cal.type.mul, d_cal.ls, super);

                double2 temp = new double2(d_cal.ls, c + d_cal.c + isuper);
                check(ref temp);
                return temp;
            }

            public static double2 Log10(double2 x)
            {
                return Math.Log10(x.d) + (double2)x.i * d_cal.lm;
            }

            public static double2 L(double2 x)
            {
                return Max(1, x).Log10();
            }

            //1.566e10
            //12.34Sxd
            //-e155i23
            //e123i123
            //-i123
            //e308i150
            //i1000.12
            //i12999.1
            //-i315666
            //i50.15B
            //i12.3Sxd
            //-i1e100
            //i18e100
            //-i2e100
            //i1.5e300
            public override string ToString()
            {
                return number_format(this);
            }
            public override bool Equals(object a)
            {
                return this == (double2)a;
            }

            public double2 floor()
            {
                return new double2(double_floor(d), i);
            }

            public double2 Log10()
            {
                return Math.Log10(d) + (double2)i * d_cal.lm;
            }

            /*
            public static implicit operator double2(double x)
            {
                return new double2(x, 0);
            }
            */
            public static implicit operator double2(double x)
            {
                return new double2(x, 0);
            }
            public static implicit operator double2(int x)
            {
                return new double2(x, 0);
            }
            public static implicit operator double2(decimal x)
            {
                return new double2((double)x, 0);
            }
        }
        public static double double_floor(double input)
        {
            if (double.IsNaN(input))
            {
                throw new Exception();
            }
            if (double.IsInfinity(input))
            {
                throw new Exception();
            }
            decimal a = 0;
            bool success = false;
            double mul = 1;
            double val = 1;
            while (!success)
            {
                double cur = (input / mul);
                if (Math.Abs(cur) <= (double)decimal.MaxValue)
                {
                    a = (decimal)(input / mul);
                    success = true;
                    break;
                }
                else
                {
                    mul = Math.Pow(1e20, val);
                    if (val > 0)
                    {
                        val = -val;
                    }
                    else
                    {
                        val = -val;
                        val++;
                    }
                }
            }
            a = decimal.Floor(a);
            double ret = (double)a * mul;
            val = 0;
            while (ret > input)
            {
                ret -= mul;
                mul = Math.Pow(2, val);
                val++;
            }
            return ret;
        }


        public static int number_mode = 0;
        public static string[] number_level = new string[]{
          // 3    6      9      12     15     18     21     24     27     30 
  /*   0 */ "K", "M",   "B",   "T",   "Qa",  "Qi",  "Sx",  "Sp",  "Oc",  "No",
  /*  30 */"Dc", "Ud",  "Dd",  "Td",  "Qad", "Qid", "Sxd", "Spd", "Ocd", "Nod",
  /*  60 */"Vg", "Uv",  "Dv",  "Tv",  "Qav", "Qiv", "Sxv", "Spv", "Ocv", "Nov",
  /*  90 */"Tg", "Utg", "Dtg", "Ttg", "Qat", "Qit", "Sxt", "Spt", "Oct", "Not",
  /* 120 */"Ag", "Uag", "Dag", "Tag", "Qaa", "Qia", "Sxa", "Spa", "Oca", "Noa",
  /* 150 */"Ig", "Uig", "Dig", "Tig", "Qai", "Qii", "Sxi", "Spi", "Oci", "Noi",
  /* 180 */"Xg", "Uxg", "Dxg", "Txg", "Qax", "Qix", "Sxx", "Spx", "Ocx", "Nox",
  /* 210 */"Pg", "Upg", "Dpg", "Tpg", "Qap", "Qip", "Sxp", "Spp", "Ocp", "Nop",
  /* 240 */"Og", "Uog", "Dog", "Tog", "Qao", "Qio", "Sxo", "Spo", "Oco", "Noo",
  /* 270 */"Ng", "Ung", "Dng", "Tng", "Qan", "Qin", "Sxn", "Spn", "Ocn", "Non",
  /* 300 */"Ce", "Uce", "Dce"};
        public static string number_format(double2 input2, bool allow_small = false,
            bool def = true, int x_mode = 0)
        {
            int mode = number_mode;
            if (!def)
            {
                mode = x_mode;
            }

            double input = input2.d;
            double ii = input2.i;
            string ifirst = "";  //i1 i10000
            string isecond = ""; //
            int ifirst_s = 0;
            int P1_length = 8;
            int P2_length = 8;
            int iOOM = 0;
            int iLevel = 0;

            int sign = 1;
            if (input < 0)
            {
                sign = -1;
                input = -input;
                P2_length--;
            }

            if (ii < 0)
            {
                if (!allow_small)
                {
                    return "0";
                }
                else
                {
                    double rii = -ii;
                    iOOM = (int)double_floor(Math.Log10(rii));
                    iLevel = (int)double_floor(iOOM / 3.0);
                    if (Math.Abs(iOOM) < 6)
                    {
                        ifirst = "-i" + rii.ToString();
                    }
                    else
                    {
                        if (mode == 0)
                        {
                            ifirst = "-i" + (rii / Math.Pow(1000, iLevel)).ToString();
                            string sl = number_level[iLevel - 1];
                            isecond = sl;
                        }
                        else if (mode == 1)
                        {
                            isecond = "e" + make_text(iOOM);
                            ifirst = "-i" + (rii / Math.Pow(10, iOOM)).ToString();
                        }
                    }
                    ifirst_s = P2_length - isecond.Length;

                    if (ifirst.Length > ifirst_s)
                    {
                        ifirst = ifirst.Substring(0, ifirst_s);
                    }
                    if (ifirst.Last() == '.')
                    {
                        ifirst = ifirst.Substring(0, ifirst.Length - 1);
                    }

                    if (iOOM >= 2 || (iOOM >= 1 && sign == -1))
                    {
                        P1_length = 0;
                    }
                    else
                    {
                        P1_length = 8 - ifirst.Length - isecond.Length;
                    }
                }
            }
            if (ii > 0)
            {
                iOOM = (int)double_floor(Math.Log10(ii));
                iLevel = (int)double_floor(iOOM / 3.0);
                if (Math.Abs(iOOM) < 6)
                {
                    ifirst = "i" + ii.ToString();
                }
                else
                {
                    if (mode == 0)
                    {
                        ifirst = "i" + (ii / Math.Pow(1000, iLevel)).ToString();
                        string sl = number_level[iLevel - 1];
                        isecond = sl;
                    }
                    else if (mode == 1)
                    {
                        isecond = "e" + make_text(iOOM);
                        ifirst = "i" + (ii / Math.Pow(10, iOOM)).ToString();
                    }
                }
                ifirst_s = P2_length - isecond.Length;

                if (ifirst.Length > ifirst_s)
                {
                    ifirst = ifirst.Substring(0, ifirst_s);
                }
                if (ifirst.Last() == '.')
                {
                    ifirst = ifirst.Substring(0, ifirst.Length - 1);
                }

                if (iOOM >= 3 || (iOOM >= 2 && sign == -1))
                {
                    P1_length = 0;
                }
                else
                {
                    P1_length = 8 - ifirst.Length - isecond.Length;
                }
            }



            int OOM = 0;
            int Level = 0;
            if (input != 0)
            {
                OOM = (int)double_floor(Math.Log10(input));
                Level = (int)double_floor(OOM / 3.0);
            }
            else
            {
                return "0";
            }
            input *= sign;

            string final = "";
            string first = "";
            string second = "";  //K M B e99
            int first_s = 8;
            int second_s = 0;

            if (Math.Abs(OOM) < 4)
            {
                first_s = 5 + Math.Abs(OOM);
                first = input.ToString();
            }
            else
            {
                if (mode == 0)
                {
                    first = (input / Math.Pow(1000, Level)).ToString();

                    string ssign = "";
                    if (Level < 0)
                    {
                        ssign = "-";
                        Level = -Level;
                    }
                    string sl = number_level[Level - 1];
                    second = ssign + sl;
                }
                else if (mode == 1)
                {
                    second = "e" + make_text(OOM);
                    first = (input / Math.Pow(10, OOM)).ToString();
                }
            }
            first_s = Math.Min(first_s, P1_length - second.Length);

            if (first.Contains('.'))
            {
                string[] fs = first.Split('.');
                first = fs[0] + "." + fs[1].Substring(0, Math.Min(fs[1].Length, 4));
            }

            if (first_s > 0)
            {
                if (first.Length > first_s)
                {
                    first = first.Substring(0, first_s);
                }
                if (first.Last() == '.')
                {
                    first = first.Substring(0, first.Length - 1);
                }
            }
            else
            {
                first = "";
                if (sign == -1)
                {
                    first = "-";
                }
            }

            second_s = P1_length - first.Length;
            if (second_s > 0)
            {
                if (second.Length > second_s)
                {
                    second = second.Substring(0, second_s);
                }
            }
            else
            {
                second = "";
            }

            final = first + second + ifirst + isecond;
            if (final.Length > 8)
            {
                throw new Exception();
            }
            return final;
        }

        public void apply_mul(Dictionary<string, multiplier> m, string name, 
            double2 effect, bool reset = true)
        {
            if (!m.ContainsKey(name))
            {
                m.Add(name, new multiplier(reset, effect));
            }
            m[name].value = effect;
        }
        public void apply_mul(muls m, string name,
            double2 effect, bool reset = true)
        {
            m.apply(name, effect, reset);
        }

        [Serializable]
        public struct loc
        {
            public int first;
            public int second;

            public loc(int FIRST, int SECOND)
            {
                first = FIRST;
                second = SECOND;
            }
        }

        public void color_copy(ref ARGB x, byte a, byte r, byte g, byte b)
        {
            x.a = a;
            x.r = r;
            x.g = g;
            x.b = b;

            x.update();
        }

        public ARGB A(byte r, byte g, byte b)
        {
            return new ARGB(255, r, g, b);
        }

        public ARGB A(byte a, byte r, byte g, byte b)
        {
            return new ARGB(a, r, g, b);
        }

        public SolidColorBrush C(byte r, byte g, byte b)
        {
            return getSCB(Color.FromRgb(r, g, b));
        }

        public SolidColorBrush C(byte a, byte r, byte g, byte b)
        {
            return getSCB(Color.FromArgb(a, r, g, b));
        }

        public Point[] MakeCurvePoints(Point[] points, double tension)
        {
            if (points.Length < 2) return null;
            double control_scale = tension / 0.5 * 0.175;

            List<Point> result_points = new List<Point>();
            result_points.Add(points[0]);

            for (int i = 0; i < points.Length - 1; i++)
            {
                // Get the point and its neighbors.
                Point pt_before = points[Math.Max(i - 1, 0)];
                Point pt = points[i];
                Point pt_after = points[i + 1];
                Point pt_after2 = points[Math.Min(i + 2, points.Length - 1)];

                double dx1 = pt_after.X - pt_before.X;
                double dy1 = pt_after.Y - pt_before.Y;

                Point p1 = points[i];
                Point p4 = pt_after;

                double dx = pt_after.X - pt_before.X;
                double dy = pt_after.Y - pt_before.Y;
                Point p2 = new Point(
                    pt.X + control_scale * dx,
                    pt.Y + control_scale * dy);

                dx = pt_after2.X - pt.X;
                dy = pt_after2.Y - pt.Y;
                Point p3 = new Point(
                    pt_after.X - control_scale * dx,
                    pt_after.Y - control_scale * dy);

                // Save points p2, p3, and p4.
                result_points.Add(p2);
                result_points.Add(p3);
                result_points.Add(p4);
            }

            // Return the points.
            return result_points.ToArray();
        }

        public Path MakeBezierPath(Point[] points, bool close)
        {
            // Create a Path to hold the geometry.
            Path path = new Path();

            // Add a PathGeometry.
            PathGeometry path_geometry = new PathGeometry();
            path.Data = path_geometry;

            // Create a PathFigure.
            PathFigure path_figure = new PathFigure();
            path_figure.IsClosed = true;
            path_geometry.Figures.Add(path_figure);

            // Start at the first point.
            path_figure.StartPoint = points[0];

            // Create a PathSegmentCollection.
            PathSegmentCollection path_segment_collection =
                new PathSegmentCollection();
            path_figure.Segments = path_segment_collection;

            // Add the rest of the points to a PointCollection.
            PointCollection point_collection =
                new PointCollection(points.Length - 1);
            for (int i = 1; i < points.Length; i++)
                point_collection.Add(points[i]);

            // Make a PolyBezierSegment from the points.
            PolyBezierSegment bezier_segment = new PolyBezierSegment();
            bezier_segment.Points = point_collection;

            // Add the PolyBezierSegment to the segment collection.
            path_segment_collection.Add(bezier_segment);

            return path;
        }

        public Path MakeCurve(List<Point> ps, double tension, bool close)
        {
            Point[] points = new Point[ps.Count];
            for(int i = 0; i < ps.Count; i++)
            {
                points[i] = ps[i];
            }

            if (points.Length < 2) return null;
            Point[] result_points = MakeCurvePoints(points, tension);

            // Use the points to create the path.
            return MakeBezierPath(result_points.ToArray(), close);
        }
    }
}
