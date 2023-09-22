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
        public class float_message : drawable
        {
            public textblock text;
            public double remain_time;
            public double time;
            public bool showing = false;

            public ARGB background = null;

            public float_message(string name, string target, 
                HorizontalAlignment ha, VerticalAlignment va,
                double w, double h, thickness t,
                textblock text, double time)
                : base(name, target, ha, va, w, h, t)
            {
                this.name = name;
                this.target = target;
                this.text = text;
                this.time = time;
                remain_time = time;
            }
        }

        public List<float_message> float_messages =
            new List<float_message>();

        public decimal float_message_id = 0;

        public Dictionary<string, Tuple<string, Point>> temp_texts = 
            new Dictionary<string, Tuple<string, Point>>();

        public void float_message_tick()
        {
            List<float_message> del_temp = new List<float_message>();
            foreach (float_message f in float_messages)
            {
                double ratio = f.remain_time / f.time;
                byte a = (byte)(ratio * 255);
                f.remain_time -= time_tick_actually.d;

                if (f.showing == false)
                {
                    Panel p = find_elem<Panel>(f.target);
                    TextBlock x = new TextBlock
                    {
                        Name = p.Name + "_float_" + f.name,
                        HorizontalAlignment = f.ha,
                        VerticalAlignment = f.va,
                        Foreground = f.text.color.toBrush(),
                        Text = f.text.content,
                        FontSize = f.text.size,
                        Margin = f.t.GetThickness(),
                    };
                    if (f.background != null)
                    {
                        x.Background = f.background.toBrush();
                    }
                    x.IsHitTestVisible = false;
                    p.Children.Add(x);
                    vm_assign(x);
                    f.showing = true;
                }
                if (ratio > 0)
                {
                    TextBlock x = find_elem<TextBlock>
                        (f.target + "_float_" + f.name);

                    f.text.color.a = a;
                    f.text.color.update();
                    x.Foreground = f.text.color.toBrush();

                    if (f.background != null)
                    {
                        f.background.a = a;
                        f.background.update();
                        x.Background = f.background.toBrush();
                    }
                }
                else
                {
                    Panel p = find_elem<Panel>(f.target);
                    TextBlock x = find_elem<TextBlock>
                        (f.target + "_float_" + f.name);
                    p.Children.Remove(x);
                    del_temp.Add(f);
                }
            }
            foreach(float_message f in del_temp)
            {
                float_messages.Remove(f);
            }
        }
    }
}
