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
        public class grid
        {
            public string name;
            public string target;

            public double width;
            public double height;
            public Visibility v = Visibility.Visible;

            public thickness t;
            public HorizontalAlignment ha = HorizontalAlignment.Left;
            public VerticalAlignment va = VerticalAlignment.Top;

            public drawable.type type;

            public grid(string NAME, string TARGET, 
                double WIDTH, double HEIGHT, thickness T, Visibility V,
                HorizontalAlignment HA = HorizontalAlignment.Left,
                VerticalAlignment VA = VerticalAlignment.Top,
                drawable.type TYPE = drawable.type.solid_crit)
            {
                name = NAME;
                target = TARGET;
                width = WIDTH;
                height = HEIGHT;
                t = T;
                v = V;
                ha = HA;
                va = VA;
                type = TYPE;
            }

        }

        [Serializable]
        public class button
        {
            public string name;
            public string target;
            public bool ext_target;

            public string content;

            public double width;
            public double height;
            public double scale = 1;
            public Visibility v = Visibility.Visible;

            public thickness t;
            public HorizontalAlignment ha = HorizontalAlignment.Left;
            public VerticalAlignment va = VerticalAlignment.Top;

            public drawable.type type;

            public button(string NAME, bool EXT_TARGET, string TARGET, 
                string CONTENT,
                double WIDTH, double HEIGHT, thickness T, 
                HorizontalAlignment HA = HorizontalAlignment.Left,
                VerticalAlignment VA = VerticalAlignment.Top,
                drawable.type TYPE = drawable.type.solid_crit)
            {
                name = NAME;
                ext_target = EXT_TARGET;
                target = TARGET;
                content = CONTENT;
                scale = HEIGHT / 30;
                width = WIDTH / scale;
                height = HEIGHT / scale;
                t = T;
                ha = HA;
                va = VA;
                type = TYPE;
            }
        }

        [Serializable]
        public class rectangle
        {
            public string name;
            public string target;
            public bool ext_target;

            public double width;
            public double height;
            public double radiusX;
            public double radiusY;
            public ARGB color;
            public double stroke_thickness;
            public ARGB stroke_color;
            public Visibility v = Visibility.Visible;

            public thickness t;
            public HorizontalAlignment ha = HorizontalAlignment.Left;
            public VerticalAlignment va = VerticalAlignment.Top;

            public drawable.type type;

            public rectangle(string NAME, bool EXT_TARGET, string TARGET,
                ARGB COLOR, ARGB STROKE_COLOR, double STROKE_THICKNESS,
                double WIDTH, double HEIGHT,
                double rx, double ry, thickness T,
                HorizontalAlignment HA = HorizontalAlignment.Left,
                VerticalAlignment VA = VerticalAlignment.Top,
                drawable.type TYPE = drawable.type.solid_crit)
            {
                name = NAME;
                ext_target = EXT_TARGET;
                target = TARGET;
                color = COLOR;
                stroke_color = STROKE_COLOR;
                stroke_thickness = STROKE_THICKNESS;
                width = WIDTH;
                height = HEIGHT;
                radiusX = rx;
                radiusY = ry;
                t = T;
                ha = HA;
                va = VA;
                type = TYPE;
            }
        }

        public delegate Tuple<resource, double2> calculator();

        [Serializable]
        public class textblock
        {
            public string name;
            public string target;
            public bool ext_target;

            public string content;

            public bool bind_resource;
            public string res;

            public bool bind_cal;
            public calculator cal;

            public double width;
            public double height;
            public double size;
            public ARGB color;
            public Visibility v = Visibility.Visible;

            public thickness t;
            public HorizontalAlignment ha = HorizontalAlignment.Left;
            public VerticalAlignment va = VerticalAlignment.Top;

            public drawable.type type;

            public textblock(string NAME, bool EXT_TARGET, string TARGET, string CONTENT,
                ARGB COLOR,
                double WIDTH, double HEIGHT, double SIZE, thickness T,
                HorizontalAlignment HA = HorizontalAlignment.Left,
                VerticalAlignment VA = VerticalAlignment.Top,
                drawable.type TYPE = drawable.type.solid_crit)
            {
                name = NAME;
                ext_target = EXT_TARGET;
                target = TARGET;
                content = CONTENT;
                color = COLOR;
                width = WIDTH;
                height = HEIGHT;
                size = SIZE;
                t = T;
                ha = HA;
                va = VA;
                type = TYPE;
            }

            public void add_resource(string r)
            {
                bind_resource = true;
                res = r;
            }

            public void add_cal(calculator c)
            {
                bind_cal = true;
                cal = c;
            }
        }

        [Serializable]
        public class drawable
        {
            public enum type
            {
                no = 0,
                solid = 1,
                rainbow = 2,
                solid_crit = 3,
                rainbow_crit = 4,
            }

            public string name;

            public string target;

            public enum click {
                no = 0,
                left = 1,
                lr = 2,
            }
            public click clickable = click.no;
            public bool grouping = false;
            public bool masking = false;

            public ARGB cover_color = null;

            public HorizontalAlignment ha;
            public VerticalAlignment va;
            public double width;
            public double height;
            public thickness t;

            public type t_fill;
            public solid_type s_fill;

            public type t_progress;
            public solid_type s_progress;
            public double p_progress = 0.5;

            public List<grid> grids = new List<grid>();
            public List<button> buttons = new List<button>();
            public List<rectangle> rectangles = new List<rectangle>();
            public List<textblock> textblocks = new List<textblock>();

            public void set_p_progress(double2 a, double2 b)
            {
                if (a < 0 || a.i < b.i)
                {
                    p_progress = 0;
                    return;
                }
                if (a > b)
                {
                    p_progress = 1;
                    return;
                }
                p_progress = a.d / b.d;
                return;
            }

            public type t_stroke;
            public solid_type s_stroke;
            public double thickness_stroke;

            public type t_title;
            public solid_text s_title;
            public rainbow_text r_title;

            public type t_content;
            public solid_text s_content;
            public rainbow_text r_content;

            public type t_mask;
            public solid_type s_mask;

            public string name_base()
            {
                return target + "__" + name;
            }

            public drawable(string NAME, string TARGET,
                HorizontalAlignment HA, VerticalAlignment VA,
                double W, double H, thickness T)
            {
                name = NAME;
                target = TARGET;
                ha = HA;
                va = VA;
                width = W;
                height = H;
                t = T;
            }

            public void setFill(solid_type s)
            {
                t_fill = type.solid;
                s_fill = s;
            }

            public void setProgress(solid_type s, double p = 0)
            {
                t_progress = type.solid;
                s_progress = s;
                p_progress = p;
            }

            public void setStroke(solid_type s, double t)
            {
                t_stroke = type.solid;
                s_stroke = s;
                thickness_stroke = t;
            }

            public void setTitle_s(solid_text s)
            {
                t_title = type.solid;
                s_title = s;
            }

            public void setTitle_r(rainbow_text r)
            {
                t_title = type.rainbow;
                r_title = r;
            }

            public void setContent_s(solid_text s)
            {
                t_content = type.solid;
                s_content = s;
            }

            public void setContent_r(rainbow_text r)
            {
                t_content = type.rainbow;
                r_content = r;
            }

            public void setMask(solid_type s)
            {
                t_mask = type.solid;
                s_mask = s;
            }

            public void addGrid(grid g)
            {
                grids.Add(g);
            }

            public void addButton(button b)
            {
                buttons.Add(b);
            }

            public void addRectangle(rectangle r)
            {
                rectangles.Add(r);
            }

            public void addTextblock(textblock t)
            {
                textblocks.Add(t);
            }

            public static void setCRIT(ref type t)
            {
                t += 2;
            }

            public static bool isCRIT(type t)
            {
                return Convert.ToBoolean((int)t & 2);
            }
        }

        public Grid draw_drawable(drawable d)
        {
            Panel p = find_elem<Panel>(d.target);
            string name_base = d.name_base();
            bool e = exist_elem(name_base + "_grid", p);

            string bg_str = "_bg";
            if (d.grouping)
            {
                bg_str = "_背景";
            }
            string text_str = "_content";
            if (d.grouping)
            {
                text_str = "_文字";
            }


            if (!e)
            {
                Grid main = new Grid
                {
                    Name = name_base + "_grid",
                    HorizontalAlignment = d.ha,
                    VerticalAlignment = d.va,
                    Margin = d.t.GetThickness(),
                };
                p.Children.Add(main);
                vm_assign(main);

                if (d.t_fill != drawable.type.no)
                {
                    Rectangle x = new Rectangle
                    {
                        Name = name_base + bg_str,
                        HorizontalAlignment = d.s_fill.ha,
                        VerticalAlignment = d.s_fill.va,
                        Width = d.width,
                        Height = d.height,
                        Fill = d.s_fill.color.toBrush(),
                        Margin = d.s_fill.t.GetThickness(),
                    };
                    Grid.SetRowSpan(x, 1000);
                    Grid.SetColumnSpan(x, 1000);
                    main.Children.Add(x);
                    vm_assign(x);
                }

                if (d.t_progress != drawable.type.no)
                {
                    Rectangle x = new Rectangle
                    {
                        Name = name_base + "_progress",
                        HorizontalAlignment = d.s_progress.ha,
                        VerticalAlignment = d.s_progress.va,
                        Width = d.width,
                        Height = d.height,
                        Fill = d.s_progress.color.toBrush(),
                        Margin = d.s_progress.t.GetThickness(),
                    };
                    Grid.SetRowSpan(x, 1000);
                    Grid.SetColumnSpan(x, 1000);
                    main.Children.Add(x);
                    vm_assign(x);
                }

                if (d.t_stroke != drawable.type.no)
                {
                    Rectangle x = new Rectangle
                    {
                        Name = name_base + "_stroke",
                        HorizontalAlignment = d.s_stroke.ha,
                        VerticalAlignment = d.s_stroke.va,
                        Stroke = d.s_stroke.color.toBrush(),
                        StrokeThickness = d.thickness_stroke,
                        Width = d.width,
                        Height = d.height,
                        Margin = d.s_stroke.t.GetThickness(),
                    };
                    Grid.SetRowSpan(x, 1000);
                    Grid.SetColumnSpan(x, 1000);
                    main.Children.Add(x);
                    vm_assign(x);
                }

                if (d.t_title != drawable.type.no)
                {
                    if (d.t_title == drawable.type.rainbow ||
                        d.t_title == drawable.type.rainbow_crit)
                    {
                        d.r_title.target = main.Name;
                        draw_r_text(d.r_title);
                    }
                    else
                    {
                        TextBlock x = new TextBlock
                        {
                            Name = name_base + "_title",
                            HorizontalAlignment = d.s_title.ha,
                            VerticalAlignment = d.s_title.va,
                            Foreground = d.s_title.color.toBrush(),
                            Text = d.s_title.text,
                            FontSize = d.s_title.size,
                            Margin = d.s_title.t.GetThickness(),
                        };
                        Grid.SetRowSpan(x, 1000);
                        Grid.SetColumnSpan(x, 1000);
                        main.Children.Add(x);
                        vm_assign(x);
                    }
                }

                if (d.t_content != drawable.type.no)
                {
                    if (d.t_content == drawable.type.rainbow ||
                           d.t_content == drawable.type.rainbow_crit)
                    {
                        d.r_content.target = main.Name;
                        draw_r_text(d.r_content);
                    }
                    else
                    {
                        TextBlock x = new TextBlock
                        {
                            Name = name_base + text_str,
                            HorizontalAlignment = d.s_content.ha,
                            VerticalAlignment = d.s_content.va,
                            Foreground = d.s_content.color.toBrush(),
                            Text = d.s_content.text,
                            FontSize = d.s_content.size,
                            Margin = d.s_content.t.GetThickness(),
                        };
                        Grid.SetRowSpan(x, 1000);
                        Grid.SetColumnSpan(x, 1000);
                        main.Children.Add(x);
                        vm_assign(x);
                    }
                }

            }

            Rectangle bg = null;
            if (vm_elems.ContainsKey(name_base + bg_str))
            {
                bg = (Rectangle)vm_elems[name_base + bg_str];
                /*
                bg.Width = d.width;
                bg.Height = d.height;*/
            }
            Rectangle progress = null;
            if (vm_elems.ContainsKey(name_base + "_progress"))
            {
                progress = (Rectangle)vm_elems[name_base + "_progress"];
                progress.Width = d.width * d.p_progress;
                progress.Height = d.height;
            }
            Rectangle stroke = null;
            if (vm_elems.ContainsKey(name_base + "_stroke"))
            {
                stroke = (Rectangle)vm_elems[name_base + "_stroke"];
                /*
                stroke.Width = d.width;
                stroke.Height = d.height;*/
            }

            if (bg != null && drawable.isCRIT(d.t_fill))
            {
                bg.Fill = d.s_fill.color.toBrush();
            }
            if (progress != null && drawable.isCRIT(d.t_progress))
            {
                progress.Fill = d.s_progress.color.toBrush();
                progress.Width = bg.Width * d.p_progress;
            }
            if (stroke != null && drawable.isCRIT(d.t_stroke))
            {
                stroke.Stroke = d.s_stroke.color.toBrush();
                stroke.StrokeThickness = d.thickness_stroke;
            }
            if (drawable.isCRIT(d.t_title))
            {
                if (d.r_title != null)
                {
                    draw_r_text(d.r_title);
                }
                else
                {
                    TextBlock title = (TextBlock)vm_elems[name_base + "_title"];
                    title.Text = d.s_title.text;
                    title.FontSize = d.s_title.size;
                    title.Foreground = d.s_title.color.toBrush();
                }
            }
            if (drawable.isCRIT(d.t_content))
            {
                if (d.r_content != null)
                {
                    draw_r_text(d.r_content);
                }
                else
                {
                    TextBlock con = (TextBlock)vm_elems[name_base + text_str];
                    con.Text = d.s_content.text;
                    con.FontSize = d.s_content.size;
                    con.Foreground = d.s_content.color.toBrush();
                }
            }

            Grid ret = (Grid)vm_elems[name_base + "_grid"];
            ret.Width = d.width;
            ret.Height = d.height;
            
            foreach (grid g in d.grids)
            {
                Grid gd = null;
                if (exist_elem(name_base + "_" + g.name, ret))
                {
                    gd = vm_find_elem<Grid>(name_base + "_" + g.name);
                }
                if (gd == null)
                {
                    gd = new Grid
                    {
                        Name = name_base + "_" + g.name,
                        HorizontalAlignment = g.ha,
                        VerticalAlignment = g.va,
                        Margin = g.t.GetThickness(),
                        Tag = g,
                    };
                    Grid.SetRowSpan(gd, 1000);
                    Grid.SetColumnSpan(gd, 1000);
                    ret.Children.Add(gd);
                    vm_assign(gd);
                }

                gd.Width = g.width;
                gd.Height = g.height;
                gd.Visibility = g.v;
            }

            foreach (button b in d.buttons)
            {
                Button btn = null;
                Grid target = ret;
                if (b.ext_target)
                {
                    target = (Grid)vm_elems[name_base + "_" + b.target];
                }
                if (exist_elem(name_base + "_" + b.name, target))
                {
                    btn = vm_find_elem<Button>(name_base + "_" + b.name);
                }
                if (btn == null)
                {
                    btn = new Button
                    {
                        Name = name_base + "_" + b.name,
                        HorizontalAlignment = b.ha,
                        VerticalAlignment = b.va,
                        Content = b.content,
                        Margin = b.t.GetThickness(),
                        Tag = b,
                    };
                    Grid.SetRowSpan(btn, 1000);
                    Grid.SetColumnSpan(btn, 1000);
                    btn.Width = b.width;
                    btn.Height = b.height;
                    scale(btn, b.scale, b.scale, 0.5, 0.5);
                    btn.Click += button_click;
                    target.Children.Add(btn);
                    vm_assign(btn);
                }
                else
                {
                    btn.Content = b.content;
                }
                btn.Visibility = b.v;
            }

            foreach (rectangle r in d.rectangles)
            {
                Rectangle R = null;
                Grid target = ret;
                if (r.ext_target)
                {
                    target = (Grid)vm_elems[name_base + "_" + r.target];
                }
                if (exist_elem(name_base + "_" + r.name, ret))
                {
                    R = vm_find_elem<Rectangle>(name_base + "_" + r.name);
                }
                if (R == null)
                {
                    R = new Rectangle
                    {
                        Name = name_base + "_" + r.name,
                        HorizontalAlignment = r.ha,
                        VerticalAlignment = r.va,
                        Margin = r.t.GetThickness(),
                        Tag = r,
                    };
                    Grid.SetRowSpan(R, 1000);
                    Grid.SetColumnSpan(R, 1000);
                    ret.Children.Add(R);
                    vm_assign(R);
                }

                R.Width = r.width;
                R.Height = r.height;
                R.RadiusX = r.radiusX;
                R.RadiusY = r.radiusY;
                R.Fill = r.color.toBrush();
                R.Stroke = r.stroke_color.toBrush();
                R.StrokeThickness = r.stroke_thickness;
                R.Visibility = r.v;
            }

            foreach (textblock t in d.textblocks)
            {
                TextBlock text = null;
                Grid target = ret;
                if (t.ext_target)
                {
                    target = (Grid)vm_elems[name_base + "_" + t.target];
                }
                if (exist_elem(name_base + "_" + t.name, target))
                {
                    text = vm_find_elem<TextBlock>(name_base + "_" + t.name);
                }
                if (text == null)
                {
                    text = new TextBlock
                    {
                        Name = name_base + "_" + t.name,
                        HorizontalAlignment = t.ha,
                        VerticalAlignment = t.va,
                        Text = t.content,
                        Margin = t.t.GetThickness(),
                        FontSize = t.size,
                        Foreground = t.color.toBrush(),
                        TextWrapping = TextWrapping.Wrap,
                        Tag = t,
                    };
                    Grid.SetRowSpan(text, 1000);
                    Grid.SetColumnSpan(text, 1000);
                    text.Width = t.width;
                    text.Height = t.height;
                    target.Children.Add(text);
                    vm_assign(text);
                }
                else
                {
                    text.Text = t.content;
                    text.FontSize = t.size;
                    text.Foreground = t.color.toBrush();
                }
                if (t.bind_resource)
                {
                    resource r = find_resource(t.res);
                    if(r == null)
                    {
                        text.Text = "0 " + t.res;
                    }
                    else
                    {
                        text.Text = r.get_value() + " " + r.name;
                        text.Foreground = r.text_color();
                    }
                }
                if (t.bind_cal)
                {
                    Tuple<resource, double2> tuple = t.cal();
                    resource r = tuple.Item1;
                    text.Text = tuple.Item2 + " " + r.name;
                    text.Foreground = r.text_color();
                }
                text.Visibility = t.v;
            }

            if (!e)
            {
                if (d.clickable != drawable.click.no)
                {
                    Rectangle x = new Rectangle
                    {
                        Name = name_base,
                        HorizontalAlignment = d.s_fill.ha,
                        VerticalAlignment = d.s_fill.va,
                        Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                        Margin = d.s_fill.t.GetThickness(),
                    };
                    Grid.SetRowSpan(x, 1000);
                    Grid.SetColumnSpan(x, 1000);
                    x.Width = d.width;
                    x.Height = d.height;
                    if (d.clickable == drawable.click.left)
                    {
                        vm_set_lbtn(x);
                    }
                    if (d.clickable == drawable.click.lr)
                    {
                        vm_set_lrbtn(x);
                    }
                    ret.Children.Add(x);
                    vm_assign(x);
                }
                {
                    Rectangle x = new Rectangle
                    {
                        Name = name_base + "_mask",
                        HorizontalAlignment = d.s_fill.ha,
                        VerticalAlignment = d.s_fill.va,
                        Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                        Margin = d.s_fill.t.GetThickness(),
                    };
                    Grid.SetRowSpan(x, 1000);
                    Grid.SetColumnSpan(x, 1000);
                    x.Width = d.width;
                    x.Height = d.height;
                    ret.Children.Add(x);
                    vm_assign(x);
                }
            }
            if(d.cover_color != null)
            {
                Rectangle cover = (Rectangle)vm_elems[name_base];
                cover.Fill = d.cover_color.toBrush();
            }


            Rectangle mask = (Rectangle)vm_elems[name_base + "_mask"];
            if (d.masking)
            {
                mask.Fill = d.s_mask.color.toBrush();
                mask.Visibility = Visibility.Visible;
            }
            else
            {
                mask.Visibility = Visibility.Hidden;
            }
            return ret;
        }
    }
}
