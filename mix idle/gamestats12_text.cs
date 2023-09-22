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
        public class ARGB
        {
            [NonSerialized]
            public SolidColorBrush color;

            public byte a;
            public byte r;
            public byte g;
            public byte b;

            public ARGB(byte A, byte R, byte G, byte B)
            {
                change(A, R, G, B);
            }

            public ARGB(SolidColorBrush s)
            {
                fromColor(s.Color);
            }

            public void fromColor(Color x)
            {
                a = x.A;
                r = x.R;
                g = x.G;
                b = x.B;
                update();
            }

            public SolidColorBrush toBrush()
            {
                if(color == null)
                {
                    update();
                }
                return color;
            }
            
            public Color toColor()
            {
                return Color.FromArgb(a, r, g, b);
            }

            public void change(byte A, byte R, byte G, byte B)
            {
                a = A;
                r = R;
                g = G;
                b = B;
                update();
            }

            public void update()
            {
                color = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }
        };

        [Serializable]
        public class solid_type
        {
            public HorizontalAlignment ha = HorizontalAlignment.Left;
            public VerticalAlignment va = VerticalAlignment.Top;
            public thickness t;
            public ARGB color;

            public solid_type(ARGB c,
                HorizontalAlignment h = HorizontalAlignment.Left,
                VerticalAlignment v = VerticalAlignment.Top,
                thickness th = null)
            {
                color = c;
                ha = h;
                va = v;
                t = th;
                if (t == null)
                {
                    t = new thickness(0, 0, 0, 0);
                }
            }
        }

        [Serializable]
        public class solid_text : solid_type
        {
            public string text;
            public double size;

            public solid_text(string t, double s, ARGB c,
                HorizontalAlignment h = HorizontalAlignment.Left,
                VerticalAlignment v = VerticalAlignment.Top,
                thickness th = null)
                : base(c, h, v, th)
            {
                text = t;
                size = s;
            }
        }

        [Serializable]
        public class rainbow_text
        {
            public string name;
            public List<Tuple<string, ARGB>> list;
            public string target;
            public HorizontalAlignment ha;
            public VerticalAlignment va;
            //Thickness
            public double left;
            public double top;
            public double right;
            public double bottom;

            public double w;
            public double h;
            public double size;

            public rainbow_text(string NAME)
            {
                list = new List<Tuple<string, ARGB>>();
                name = NAME;
            }

            public void prepare(string TARGET, HorizontalAlignment HA,
                VerticalAlignment VA, Thickness T,
                double W, double H, double SIZE)
            {
                target = TARGET;
                ha = HA;
                va = VA;

                left = T.Left;
                top = T.Top;
                right = T.Right;
                bottom = T.Bottom;

                w = W;
                h = H;
                size = SIZE;
            }

            public Thickness GetThickness()
            {
                return new Thickness(left, top, right, bottom);
            }

            public void add(string s, byte r, byte g, byte b, byte a = 255)
            {
                list.Add(new Tuple<string, ARGB>(s, new ARGB(a, r, g, b)));
            }
            
            public void add(resource r)
            {
                list.Add(new Tuple<string, ARGB>(r.name, new ARGB(255, r.r, r.g, r.b)));
            }
        }

        //必须rainbow_text::prepare()
        public WrapPanel draw_r_text(rainbow_text rt, int len = 2)
        {
            if (rt == null)
            {
                return null;
            }
            Panel g = find_elem<Panel>(rt.target);
            string name_base = g.Name + "__" + rt.name;
            bool e = exist_elem(name_base, g);

            WrapPanel con = null;
            if (!e)
            {
                con = new WrapPanel
                {
                    Name = name_base,
                    HorizontalAlignment = rt.ha,
                    VerticalAlignment = rt.va,
                    Width = rt.w,
                    Height = rt.h,
                    Margin = rt.GetThickness(),
                };
                g.Children.Add(con);
                vm_assign(con);
                /*
                int k = 0;
                foreach (Tuple<string, ARGB> t in rt.list)
                {
                    int index = 0;
                    int remain = t.Item1.Length;
                    while (remain > 0)
                    {
                        int sublen = Math.Min(remain, len);
                        string sub = t.Item1.Substring(index, sublen);
                        remain -= sublen;
                        index += sublen;
                        TextBlock tb = new TextBlock
                        {
                            Name = name_base + "_rt" + k,
                            FontSize = rt.size,
                            Foreground = t.Item2.toBrush(),
                            Text = sub,
                        };
                        k++;
                        con.Children.Add(tb);
                        vm_assign(tb);
                    }
                }*/
            }
            con = (WrapPanel)vm_elems[name_base];

            int k0 = 0;
            foreach (Tuple<string, ARGB> t in rt.list)
            {
                int index = 0;
                int remain = t.Item1.Length;
                while (remain > 0 || t.Item1 == "")
                {
                    int sublen = Math.Min(remain, len);
                    string sub = t.Item1.Substring(index, sublen);
                    remain -= sublen;
                    index += sublen;

                    TextBlock tb = null;
                    if (exist_elem(name_base + "_rt" + k0, con))
                    {
                        tb = vm_find_elem<TextBlock>(name_base + "_rt" + k0);
                    }
                    if (tb != null)
                    {
                        tb.FontSize = rt.size;
                        tb.Foreground = t.Item2.toBrush();
                        tb.Text = sub;
                        if (tb.Visibility == Visibility.Collapsed)
                        {
                            tb.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        tb = new TextBlock
                        {
                            Name = name_base + "_rt" + k0,
                            FontSize = rt.size,
                            Foreground = t.Item2.toBrush(),
                            Text = sub,
                        };
                        con.Children.Add(tb);
                        vm_assign(tb);
                    }
                    if (t.Item1 == "") //换行
                    {
                        if (k0 == 0)
                        {
                            tb.Width = g.ActualWidth;
                        }
                        else
                        {
                            TextBlock last = vm_find_elem<TextBlock>(name_base + "_rt" + (k0 - 1));
                            Point p = (Point)VisualTreeHelper.GetOffset(last);
                            tb.Width = Math.Max(0, g.ActualWidth - p.X - last.ActualWidth
                                - con.Margin.Left - con.Margin.Right);
                        }
                        k0++;
                        break;
                    }
                    k0++;
                }
            }

            while (true)
            {
                TextBlock tb = vm_find_elem<TextBlock>(name_base + "_rt" + k0);
                if (tb != null)
                {
                    tb.Visibility = Visibility.Collapsed;
                }
                else
                {
                    break;
                }
                k0++;
            }


            return con;
        }

    }
}
