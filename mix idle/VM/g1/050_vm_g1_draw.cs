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
        public Grid g1_getMap()
        {
            return (Grid)vm_elems["vm_g1_map_grid"];
        }

        public Grid g1_getCTRL()
        {
            return (Grid)vm_elems["vm_g1_ctrl_grid"];
        }

        public void g1_draw_layer_icon(g1_layer layer, double mulX = 1, double mulY = 1)
        {
            if (layer.target == null)
            {
                return;
            }
            Grid target = (Grid)vm_elems[layer.target];
            string name_base = target.Name + "_layer__" + layer.name;
            bool e = exist_elem(name_base + "_grid", target);

            double mulMIN = Math.Min(mulX, mulY);

            Grid content = null;
            if (!e)
            {
                //TODO 若层不显示 线也不显示
                foreach (g1_layer next in layer.nexts)
                {
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.MappingMode = BrushMappingMode.Absolute;
                    lgb.StartPoint = layer.c_position;
                    lgb.EndPoint = next.c_position;
                    lgb.GradientStops.Add(new GradientStop(layer.line_color.toColor(), 0));
                    lgb.GradientStops.Add(new GradientStop(next.line_color.toColor(), 1));
                    Line l = new Line
                    {
                        Name = name_base + "_" + next.name + "_line",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        X1 = layer.c_position.X * mulX,
                        Y1 = layer.c_position.Y * mulY,
                        X2 = next.c_position.X * mulX,
                        Y2 = next.c_position.Y * mulY,
                        Stroke = lgb,
                        StrokeThickness = (layer.line_thickness + next.line_thickness) / 2 * (mulX + mulY) / 2,
                    };
                    target.Children.Add(l);
                    vm_assign(l);

                    layer.lines[name_base + "_" + next.name + "_line"] = next;
                    next.lines[name_base + "_" + next.name + "_line"] = layer;
                }

                content = new Grid
                {
                    Name = name_base + "_grid",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = layer.size * mulMIN,
                    Height = layer.size * mulMIN,
                    Margin = new Thickness(layer.c_position.X * mulX - layer.size * mulMIN * 0.5,
                        layer.c_position.Y * mulY - layer.size * mulMIN * 0.5, 0, 0),
                };
                target.Children.Add(content);
                vm_assign(content);

                Rectangle bg = new Rectangle
                {
                    Name = name_base + "_bg",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = layer.size * mulMIN,
                    Height = layer.size * mulMIN,
                    RadiusX = 100000,
                    RadiusY = 100000,
                    Fill = layer.cicrle_color.toBrush(),
                    Stroke = layer.stroke_color.toBrush(),
                    StrokeThickness = layer.stroke_t * (mulX + mulY) / 2,
                };
                content.Children.Add(bg);
                vm_assign(bg);

                TextBlock t = new TextBlock
                {
                    Name = name_base + "_text",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    MaxWidth = layer.size * 0.9 * mulMIN,
                    MaxHeight = layer.size * 0.9 * mulMIN,
                    FontSize = layer.size * layer.text_size_base * mulMIN,
                    Foreground = layer.text_color.toBrush(),
                    FontFamily = new FontFamily(layer.font_family),
                    Text = layer.text,
                };
                content.Children.Add(t);
                vm_assign(t);

                Rectangle cover = new Rectangle
                {
                    Name = name_base,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = layer.size * mulMIN,
                    Height = layer.size * mulMIN,
                    RadiusX = 100000,
                    RadiusY = 100000,
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                };
                vm_set_lbtn(cover);
                content.Children.Add(cover);
                vm_assign(cover);
            }

            content = (Grid)vm_elems[name_base + "_grid"];
            Rectangle bg_ = (Rectangle)vm_elems[name_base + "_bg"];
            TextBlock t_ = (TextBlock)vm_elems[name_base + "_text"];
            Rectangle cover_ = (Rectangle)vm_elems[name_base];
            if (layer.unlocked)
            {
                content.Visibility = Visibility.Visible;
                content.Width = layer.size * mulMIN;
                content.Height = layer.size * mulMIN;
                content.Margin = new Thickness(layer.c_position.X * mulX - layer.size * mulMIN * 0.5,
                        layer.c_position.Y * mulY - layer.size * mulMIN * 0.5, 0, 0);
                foreach (KeyValuePair<string, g1_layer> n in layer.lines)
                {
                    string base_name1 = Regex.Split(n.Key, "_layer__")[0];
                    string base_name2 = Regex.Split(name_base, "_layer__")[0];
                    if (base_name1 != base_name2)
                    {
                        continue;
                    }

                    Line line = (Line)vm_elems[n.Key];
                    if (n.Value.unlocked)
                    {
                        line.Visibility = Visibility.Visible;
                        line.StrokeThickness =
                            (layer.line_thickness + n.Value.line_thickness) / 2 * (mulX + mulY) / 2;

                        if (g1_glow)
                        {
                            if (layer.glowing)
                            {
                                if (g1_second)
                                {
                                    layer.current_line_color = layer.second_color;
                                }
                                else
                                {
                                    layer.current_line_color = layer.line_color;
                                }
                            }
                            else
                            {
                                layer.current_line_color = layer.line_color;
                            }

                            LinearGradientBrush lgb = new LinearGradientBrush();
                            lgb.MappingMode = BrushMappingMode.Absolute;
                            lgb.StartPoint = new Point(layer.c_position.X * mulX, layer.c_position.Y * mulY);
                            lgb.EndPoint = new Point(n.Value.c_position.X * mulX, n.Value.c_position.Y * mulY);
                            lgb.GradientStops.Add(new GradientStop(layer.current_line_color.toColor(), 0));
                            lgb.GradientStops.Add(new GradientStop(n.Value.current_line_color.toColor(), 1));
                            line.Stroke = lgb;
                        }
                    }
                    else
                    {
                        line.Visibility = Visibility.Hidden;
                    }
                }
                bg_.Width = layer.size * mulMIN;
                bg_.Height = layer.size * mulMIN;
                if (g1_glow)
                {
                    if (layer.glowing)
                    {
                        if (g1_second)
                        {
                            bg_.Stroke = layer.second_color.toBrush();
                        }
                        else
                        {
                            bg_.Stroke = layer.stroke_color.toBrush();
                        }
                    }
                    else
                    {
                        bg_.Stroke = layer.stroke_color.toBrush();
                    }
                }
                t_.MaxWidth = layer.size * 0.9 * mulMIN;
                t_.MaxHeight = layer.size * 0.9 * mulMIN;
                t_.FontSize = layer.size * layer.text_size_base * mulMIN;
                cover_.Width = layer.size * mulMIN;
                cover_.Height = layer.size * mulMIN;
            }
            else
            {
                content.Visibility = Visibility.Hidden;
                foreach (KeyValuePair<string, g1_layer> n in layer.lines)
                {
                    if (vm_elems.ContainsKey(n.Key))
                    {
                        vm_elems[n.Key].Visibility = Visibility.Hidden;
                    }
                }
            }

            Grid map = g1_getMap();
            Grid radar = (Grid)vm_elems["vm_g1_radar_grid"];
            if (map.Equals(target))
            {
                string temp = layer.target;
                layer.target = "vm_g1_radar_main";
                g1_draw_layer_icon(layer, radar.Width / map.Width, radar.Height / map.Height);
                layer.target = temp;
            }
        }

        public void g1_draw_layer_grid(g1_layer layer)
        {
            if (layer.target == null)
            {
                return;
            }
            Grid layer_grid = (Grid)vm_elems["vm_g1_layer_grid"];
            string name_base = "vm_g1_layer_" + layer.name;
            bool e = exist_elem(name_base + "_grid", layer_grid);

            Grid main = null;
            Grid tabs_grid = null;
            if (!e)
            {
                main = new Grid
                {
                    Name = name_base + "_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Visibility = Visibility.Hidden,
                };
                layer_grid.Children.Add(main);
                vm_assign(main);

                Grid app = vm_get_app_grid();
                StackPanel right_top = new StackPanel
                {
                    Name = name_base + "_right_top_panel",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, app.Height * 0.02, 0, 0),
                };
                main.Children.Add(right_top);
                vm_assign(right_top);

                tabs_grid = new Grid
                {
                    Name = name_base + "_tabs_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
                main.Children.Add(tabs_grid);
                vm_assign(tabs_grid);
            }
            else
            {
                main = (Grid)vm_elems[name_base + "_grid"];
                tabs_grid = (Grid)vm_elems[name_base + "_tabs_grid"];
            }


            foreach (KeyValuePair<string, g1_tab> kp in layer.tabs)
            {
                g1_tab t = kp.Value;
                t.tab.target = tabs_grid.Name;
                if (g1_glow)
                {
                    if (t.glowing)
                    {
                        if (g1_second)
                        {
                            t.tab.s_stroke.color = t.second_color;
                        }
                        else
                        {
                            t.tab.s_stroke.color = t.save_color;
                        }
                    }
                    else
                    {
                        t.tab.s_stroke.color = t.save_color;
                    }
                }
                Grid tab_button = draw_drawable(t.tab);
                if (t.unlocked)
                {
                    tab_button.Visibility = Visibility.Visible;
                }
                else
                {
                    tab_button.Visibility = Visibility.Hidden;
                }

                Grid tab_grid = null;
                if (exist_elem(name_base + "_" + kp.Key + "_grid", main))
                {
                    tab_grid = (Grid)vm_elems[name_base + "_" + kp.Key + "_grid"];
                }
                if (tab_grid == null)
                {
                    tab_grid = new Grid
                    {
                        Name = name_base + "_" + kp.Key + "_grid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };
                    main.Children.Add(tab_grid);
                    vm_assign(tab_grid);
                }
                if (layer.curr_tab.Equals(t))
                {
                    tab_grid.Visibility = Visibility.Visible;
                }
                else
                {
                    tab_grid.Visibility = Visibility.Hidden;
                }

                Grid attach_grid = null;
                if (exist_elem(name_base + "_" + kp.Key + "_attach_grid", tab_grid))
                {
                    attach_grid = (Grid)vm_elems[name_base + "_" + kp.Key + "_attach_grid"];
                }
                if (attach_grid == null)
                {
                    attach_grid = new Grid
                    {
                        Name = name_base + "_" + kp.Key + "_attach_grid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };
                    tab_grid.Children.Add(attach_grid);
                    vm_assign(attach_grid);
                }

                for (int i = 1; i <= kp.Value.page_max; i++)
                {
                    Grid u_grid = null;
                    if (exist_elem(name_base + "_" + kp.Key + "_" + i + "_grid", tab_grid))
                    {
                        u_grid = (Grid)vm_elems
                            [name_base + "_" + kp.Key + "_" + i + "_grid"];
                    }
                    if (u_grid == null)
                    {
                        u_grid = new Grid
                        {
                            Name = name_base + "_" + kp.Key + "_" + i + "_grid",
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                        };
                        tab_grid.Children.Add(u_grid);
                        vm_assign(u_grid);
                    }
                    if (i == kp.Value.page_now)
                    {
                        u_grid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        u_grid.Visibility = Visibility.Hidden;
                    }
                }

            }

            layer.group = make_group(tabs_grid);
            if (!e && layer.curr_tab != null)
            {
                g1_tab t = layer.curr_tab;
                g1_layer temp = g1_current_layer;
                g1_current_layer = layer;
                game_key_name(tabs_grid.Name + "__" + t.name);
                g1_current_layer = temp;
            }



            return;
        }

        public void g1_draw_scenery(g1_scenery scenery, double mulX = 1, double mulY = 1)
        {
            Grid target = (Grid)vm_elems[scenery.target];
            string name_base = "vm_g1_" + target.Name + "_scenery__" + scenery.name;
            bool e = exist_elem(name_base, target);

            double mulMIN = Math.Min(mulX, mulY);

            bool changed = false;
            if (scenery is curve)
            {
                curve curr = scenery as curve;
                List<Point> ps = new List<Point>();

                Path path = null;
                changed = curr.changed;
                if (changed)
                {
                    foreach (Point p in curr.points)
                    {
                        ps.Add(new Point(p.X * mulX, p.Y * mulY));
                    }
                    path = MakeCurve(ps, curr.tension, curr.closed);
                    path.Stroke = curr.s_color.toBrush();
                    path.StrokeThickness = curr.thickness * mulMIN;
                    path.Fill = curr.f_color.toBrush();
                    curr.changed = false;
                }
                if (!e)
                {
                    path.Name = name_base;
                    path.HorizontalAlignment = HorizontalAlignment.Left;
                    path.VerticalAlignment = VerticalAlignment.Top;
                    target.Children.Add(path);
                    vm_assign(path);
                }


                if (changed && curr.draw_point)
                {
                    int i = 0;
                    foreach (Point p in ps)
                    {
                        Rectangle point = new Rectangle
                        {
                            Name = name_base + "_p" + i,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 10 * mulMIN,
                            Height = 10 * mulMIN,
                            RadiusX = 10 * mulMIN,
                            RadiusY = 10 * mulMIN,
                            Fill = getSCB(Color.FromRgb(255, 255, 0)),
                            Margin = new Thickness(p.X - 5 * mulX,
                            p.Y - 5 * mulY, 0, 0),
                        };
                        if (!e)
                        {
                            target.Children.Add(point);
                            vm_assign(point);
                        }
                        i++;
                    }
                }
            }
            if (scenery is blocks)
            {
                blocks curr = scenery as blocks;
                int k = 0;
                foreach(Tuple<ARGB, ARGB, Point, double, double, double> t in curr.b)
                {
                    string n = name_base + "_" + curr.name + "_" + k;
                    Rectangle r = null;
                    if (!exist_elem(n, target))
                    {
                        r = new Rectangle
                        {
                            Name = n,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Fill = t.Item1.toBrush(),
                            Stroke = t.Item2.toBrush(),
                            StrokeThickness = t.Item4 / 40 * mulMIN,
                            Margin = new Thickness(t.Item3.X * mulMIN, t.Item3.Y * mulMIN, 0, 0),
                            Width = t.Item4 * mulMIN,
                            Height = t.Item5 * mulMIN,
                        };
                        spin(r, new RotateTransform(t.Item6, t.Item4 / 2 * mulMIN, t.Item5 / 2 * mulMIN));
                        target.Children.Add(r);
                        vm_assign(r);
                    }
                    k++;
                }
            }
            //TODO : more scenery



            Grid map = g1_getMap();
            Grid scene = (Grid)vm_elems["vm_g1_scene_grid"];
            Grid radar = (Grid)vm_elems["vm_g1_radar_grid"];
            foreach (object o in scene.Children)
            {
                if (o.Equals(target))
                {
                    string temp = scenery.target;
                    scenery.target = "vm_g1_radar_scenery";
                    scenery.changed = changed;
                    g1_draw_scenery(scenery, radar.Width / map.Width, radar.Height / map.Height);
                    scenery.target = temp;
                }
            }
        }
    }
}
