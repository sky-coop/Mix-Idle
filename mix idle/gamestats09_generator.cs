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
        int tr_show = 3;
        private void mine_generate()
        {
            m.采矿_挖掘_格子_grid.Children.Clear();


            minefield = new Grid[8, 8];
            minefield_bg = new Rectangle[8, 8];
            minefield_texts = new TextBlock[8, 8];
            minefield_tr = new Rectangle[8, 8, tr_show];
            minefield_cover = new Rectangle[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    minefield[i, j] = new Grid();
                    Grid x = minefield[i, j];
                    x.Width = 50;
                    x.Height = 50;
                    x.HorizontalAlignment = (HorizontalAlignment)1;
                    x.VerticalAlignment = (VerticalAlignment)1;
                    Grid.SetRow(x, i);
                    Grid.SetColumn(x, j);
                    m.采矿_挖掘_格子_grid.Children.Add(x);

                    minefield_bg[i, j] = new Rectangle();
                    Rectangle a = minefield_bg[i, j];
                    a.Width = 50;
                    a.Height = 50;
                    a.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    a.StrokeThickness = 1;
                    a.RadiusX = 5;
                    a.RadiusY = 5;
                    a.HorizontalAlignment = (HorizontalAlignment)1;
                    a.VerticalAlignment = (VerticalAlignment)1;
                    x.Children.Add(a);

                    minefield_texts[i, j] = new TextBlock();
                    TextBlock b = minefield_texts[i, j];
                    b.HorizontalAlignment = (HorizontalAlignment)1;
                    b.VerticalAlignment = (VerticalAlignment)1;
                    b.FontSize = 16;
                    x.Children.Add(b);

                    for(int p = 0; p < tr_show; p++)
                    {
                        minefield_tr[i, j, p] = new Rectangle();
                        a = minefield_tr[i, j, p];
                        a.Width = 30 + 5 * p;
                        a.Height = 30 + 5 * p;
                        a.Stroke = new SolidColorBrush(Color.FromRgb(0, (byte)(70 + 60 * p), (byte)(50 + 60 * p)));
                        a.StrokeThickness = 2.5;
                        a.RadiusX = 100;
                        a.RadiusY = 100;
                        a.HorizontalAlignment = (HorizontalAlignment)1;
                        a.VerticalAlignment = (VerticalAlignment)1;
                        x.Children.Add(a);
                    }

                    minefield_cover[i, j] = new Rectangle();
                    a = minefield_cover[i, j];
                    a.Width = 50;
                    a.Height = 50;
                    a.RadiusX = 5;
                    a.RadiusY = 5;
                    a.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    a.HorizontalAlignment = (HorizontalAlignment)1;
                    a.VerticalAlignment = (VerticalAlignment)1;
                    a.Tag = 10 * i + j;
                    a.Name = "minefield_" + Convert.ToString(i) + "_" + Convert.ToString(j);

                    a.MouseEnter += mine_enter;
                    a.MouseLeave += mine_leave;
                    a.MouseLeftButtonDown += mine_down;
                    a.MouseLeftButtonUp += mine_up;
                    a.MouseMove += mine_move;

                    x.Children.Add(a);
                }
            }
        }

        private void get_field()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    get_cell(minef.graph[i, j], i, j);
                }
            }
        }

        private void get_cell(mine_cell x, int i, int j)
        {
            if (x.depth == int.MaxValue)
            {
                minefield_bg[i, j].Fill = getSCB(Color.FromRgb(255, 255, 255));
                minefield_texts[i, j].Foreground = getSCB(Color.FromRgb(0, 0, 0));
                minefield_texts[i, j].Text = "×";
                for (int k = 0; k < tr_show; k++)
                {
                    minefield_tr[i, j, k].Stroke = getSCB(Color.FromArgb(0, 0, 0, 0));
                }
                return;
            }
            byte r = x.r[x.depth - 1];
            byte g = x.g[x.depth - 1];
            byte b = x.b[x.depth - 1];
            minefield_bg[i, j].Fill = getSCB(Color.FromRgb(r, g, b));
            //shift_color_byte(ref r);
            shift_color_byte(ref g);
            //shift_color_byte(ref b);
            minefield_texts[i, j].Foreground = getSCB(Color.FromRgb(r, g, b));
            minefield_texts[i, j].Text = number_format(x.depth);

            int p = 0;
            foreach (KeyValuePair<string, double2> kp in x.loot[x.depth - 1])
            {
                if (treasures.ContainsKey(kp.Key))
                {
                    minefield_tr[i, j, p].Stroke = treasures[kp.Key].text_color();
                    p++;
                }
            }
            for (; p < tr_show; p++)
            {
                minefield_tr[i, j, p].Stroke = getSCB(Color.FromArgb(0, 0, 0, 0));
            }
        }

        private ComboBoxItem get_xy(resource res)
        {
            ComboBoxItem t = new ComboBoxItem
            {
                Content = res.name,
                Foreground = getSCB(Color.FromRgb(255, 0, 0)),
                FontSize = 12
            };
            return t;
        }



        [NonSerialized]
        Dictionary<string, FrameworkElement> framework_elements = new Dictionary<string, FrameworkElement>();
        [NonSerialized]
        Dictionary<string, ComboBoxItem> xs_tb = new Dictionary<string, ComboBoxItem>();
        string[] xlist;
        string[] ylist;
        [NonSerialized]
        Dictionary<string, ComboBoxItem> ys_tb = new Dictionary<string, ComboBoxItem>();

        private void heater_generate()
        {
            Grid g0 = m.采矿_炼制_熔炉内部_grid;
            g0.Children.Clear();

            xs_tb = new Dictionary<string, ComboBoxItem>();
            ys_tb = new Dictionary<string, ComboBoxItem>();
            xlist = new string[] { "植物原料", "动物原料", "铜矿", "铁矿", "铁" };
            ylist = new string[] { "木头方块", "煤", "烈焰粉末", "石油" };
            m.采矿_炼制_熔炉_原料_combobox.Items.Clear();
            m.采矿_炼制_熔炉_燃料_combobox.Items.Clear();
            foreach (string a in xlist)
            {
                xs_tb.Add(a, get_xy(find_resource(a)));
                m.采矿_炼制_熔炉_原料_combobox.Items.Add(xs_tb[a]);
            }
            foreach (string a in ylist)
            {
                ys_tb.Add(a, get_xy(find_resource(a)));
                m.采矿_炼制_熔炉_燃料_combobox.Items.Add(ys_tb[a]);
            }

            for (int i = 0; i < 8; i++)
            {
                string s = Convert.ToString(i / 4);
                string s2 = Convert.ToString(i % 4);

                Rectangle r = new Rectangle
                {
                    Name = "heater_" + s + "_" + s2 + "_frame",
                    HorizontalAlignment = (HorizontalAlignment)3,
                    VerticalAlignment = (VerticalAlignment)3,
                    Fill = getSCB(Color.FromArgb(50, 127, 127, 127)),
                    Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                    StrokeThickness = 2
                };
                Grid.SetRow(r, i / 4);
                Grid.SetColumn(r, i % 4);
                g0.Children.Add(r);

                Grid g = new Grid
                {
                    Name = "heater_" + s + "_" + s2 + "_grid",
                    HorizontalAlignment = (HorizontalAlignment)3,
                    VerticalAlignment = (VerticalAlignment)3
                };
                Grid.SetRow(g, i / 4);
                Grid.SetColumn(g, i % 4);
                g0.Children.Add(g);
                framework_elements.Add(g.Name, g);


                r = new Rectangle
                {
                    Name = "heater_" + s + "_" + s2 + "_bg_1",
                    HorizontalAlignment = (HorizontalAlignment)3,
                    VerticalAlignment = (VerticalAlignment)3
                };
                if (i < 4)
                {
                    r.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * i), 0, 255, 255));
                }
                else
                {
                    r.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * (i - 4)), 255, 0, 0));
                }
                r.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                r.StrokeThickness = 2;
                g.Children.Add(r);
                framework_elements.Add(r.Name, r);

                r = new Rectangle
                {
                    Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_bg_2",
                    HorizontalAlignment = (HorizontalAlignment)0,
                    VerticalAlignment = (VerticalAlignment)3,
                    Width = 100
                };
                if (i < 4)
                {
                    r.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * i), 0, 127, 255));
                }
                else
                {
                    r.Fill = getSCB(Color.FromArgb((byte)(150 - 20 * (i - 4)), 255, 127, 255));
                }
                r.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                r.StrokeThickness = 2;
                g.Children.Add(r);
                framework_elements.Add(r.Name, r);

                string m = "";
                if (i < 4)
                {
                    m = "原料" + Convert.ToString(i + 1);
                }
                else
                {
                    m = "燃料" + Convert.ToString(i - 4 + 1);
                }
                TextBlock t = new TextBlock
                {
                    Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_text_1",
                    HorizontalAlignment = (HorizontalAlignment)0,
                    VerticalAlignment = (VerticalAlignment)0,
                    Text = m,
                    Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    FontWeight = FontWeight.FromOpenTypeWeight(700),
                    FontSize = 16,
                    Margin = new Thickness(10, 5, 0, 0)
                };
                g.Children.Add(t);
                framework_elements.Add(t.Name, t);

                t = new TextBlock
                {
                    Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_text_2",
                    HorizontalAlignment = (HorizontalAlignment)2,
                    VerticalAlignment = (VerticalAlignment)0,
                    Text = "开启",
                    Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                    FontSize = 16,
                    Margin = new Thickness(0, 5, 30, 0),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    FontWeight = FontWeight.FromOpenTypeWeight(700)
                };
                g.Children.Add(t);
                framework_elements.Add(t.Name, t);

                bool b = true;
                if (i < 4)
                {
                    if(furance.xs.Count <= i)
                    {
                        b = true;
                    }
                    else
                    {
                        b = furance.xs[i].open;
                    }
                }
                else
                {
                    if (furance.ys.Count <= i - 4)
                    {
                        b = true;
                    }
                    else
                    {
                        b = furance.ys[i - 4].open;
                    }
                }

                CheckBox c = new CheckBox
                {
                    Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_checkbox",
                    HorizontalAlignment = (HorizontalAlignment)2,
                    VerticalAlignment = (VerticalAlignment)0,
                    Margin = new Thickness(0, 9, 10, 0),
                    IsChecked = b
                };
                g.Children.Add(c);
                framework_elements.Add(c.Name, c);

                for (int k = 0; k < 5; k++)
                {
                    t = new TextBlock
                    {
                        Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_text_" + Convert.ToString(3 + k),
                        HorizontalAlignment = (HorizontalAlignment)3,
                        VerticalAlignment = (VerticalAlignment)0,
                        Text = "test",
                        FontSize = 11
                    };
                    if (k == 0)
                    {
                        t.Foreground = getSCB(Color.FromRgb(0, 255, 0));
                    }
                    else if (k == 1)
                    {
                        t.Foreground = getSCB(Color.FromRgb(255, 255, 0));
                    }
                    else if (k == 2)
                    {
                        t.Foreground = getSCB(Color.FromRgb(255, 255, 0));
                        if (i >= 4)
                        {
                            t.Foreground = getSCB(Color.FromRgb(0, 255, 255));
                        }
                    }
                    else if (k == 3)
                    {
                        t.Foreground = getSCB(Color.FromRgb(0, 255, 255));
                    }
                    else if (k == 4)
                    {
                        t.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                    }
                    t.Margin = new Thickness(10, 25 + 15 * k, 0, 0);
                    t.FontFamily = new FontFamily("Comic Sans MS");
                    t.FontWeight = FontWeight.FromOpenTypeWeight(700);
                    g.Children.Add(t);
                    framework_elements.Add(t.Name, t);
                }

                Grid grid = new Grid
                {
                    Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_grid",
                    HorizontalAlignment = (HorizontalAlignment)0,
                    VerticalAlignment = (VerticalAlignment)2,
                    Height = 20
                };
                for (int k = 0; k < 4; k++)
                {
                    ColumnDefinition definition = new ColumnDefinition
                    {
                        Width = new GridLength(50, GridUnitType.Pixel)
                    };
                    grid.ColumnDefinitions.Add(definition);
                }
                g.Children.Add(grid);

                for (int k = 0; k < 4; k++)
                {
                    Grid grid1 = new Grid
                    {
                        Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_button_" + Convert.ToString(k + 1) + "_grid",
                        HorizontalAlignment = (HorizontalAlignment)3,
                        VerticalAlignment = (VerticalAlignment)3
                    };
                    Grid.SetColumn(grid1, k);
                    grid.Children.Add(grid1);
                    framework_elements.Add(grid1.Name, grid1);

                    r = new Rectangle
                    {
                        Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_button_" + Convert.ToString(k + 1) + "_bg",
                        HorizontalAlignment = (HorizontalAlignment)3,
                        VerticalAlignment = (VerticalAlignment)3,
                        Fill = getSCB(Color.FromRgb(255, 255, 255)),
                        Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                        StrokeThickness = 1
                    };
                    grid1.Children.Add(r);
                    framework_elements.Add(r.Name, r);

                    t = new TextBlock
                    {
                        Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_button_" + Convert.ToString(k + 1) + "_text",
                        HorizontalAlignment = (HorizontalAlignment)1,
                        VerticalAlignment = (VerticalAlignment)1,
                        FontSize = 14
                    };
                    if (k == 0)
                    {
                        t.Text = "补充";
                        t.FontSize = 12;
                    }
                    else if (k == 1)
                    {
                        t.Text = "删除";
                        t.FontSize = 12;
                    }
                    else if (k == 2)
                    {
                        t.Text = "←";
                        t.FontWeight = FontWeight.FromOpenTypeWeight(700);
                    }
                    else if (k == 3)
                    {
                        t.Text = "→";
                        t.FontWeight = FontWeight.FromOpenTypeWeight(700);
                    }
                    t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                    grid1.Children.Add(t);
                    framework_elements.Add(t.Name, t);

                    r = new Rectangle
                    {
                        Name = "heater_" + s + "_" + Convert.ToString(i % 4) + "_button_" + Convert.ToString(k + 1),
                        HorizontalAlignment = (HorizontalAlignment)3,
                        VerticalAlignment = (VerticalAlignment)3,
                        Fill = getSCB(Color.FromArgb(0, 255, 255, 255))
                    };

                    r.MouseEnter += rectangle_cover_enter;
                    r.MouseLeave += rectangle_cover_leave;
                    r.MouseMove += rectangle_cover_move;
                    r.MouseLeftButtonDown += rectangle_cover_down;
                    r.MouseLeftButtonUp += rectangle_cover_up;
                    
                    grid1.Children.Add(r);
                    framework_elements.Add(r.Name, r);
                }
            }
        }

        private void achieve_generate()
        {
            m.achieves_center_grid.Children.Clear();

            achievefield = new Grid[8, 10];
            achievefield_bg = new Rectangle[8, 10];
            achievefield_texts = new TextBlock[8, 10];
            achievefield_cover = new Rectangle[8, 10];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    achievefield[i, j] = new Grid();
                    Grid x = achievefield[i, j];
                    x.Width = 90;
                    x.Height = 40;
                    x.HorizontalAlignment = (HorizontalAlignment)1;
                    x.VerticalAlignment = (VerticalAlignment)1;
                    Grid.SetRow(x, i);
                    Grid.SetColumn(x, j);
                    m.achieves_center_grid.Children.Add(x);

                    if (!achievements_id.ContainsKey(10 * i + j))
                    {
                        x.Visibility = (Visibility)1;
                    }

                    achievefield_bg[i, j] = new Rectangle();
                    Rectangle a = achievefield_bg[i, j];
                    a.Width = 90;
                    a.Height = 40;
                    a.Fill = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                    a.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    a.StrokeThickness = 1;
                    a.RadiusX = 6;
                    a.RadiusY = 6;
                    a.HorizontalAlignment = (HorizontalAlignment)1;
                    a.VerticalAlignment = (VerticalAlignment)1;
                    x.Children.Add(a);




                    achievefield_texts[i, j] = new TextBlock();
                    TextBlock b = achievefield_texts[i, j];
                    b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    b.HorizontalAlignment = (HorizontalAlignment)1;
                    b.VerticalAlignment = (VerticalAlignment)1;
                    b.FontSize = 14;
                    x.Children.Add(b);




                    achievefield_cover[i, j] = new Rectangle();
                    a = achievefield_cover[i, j];
                    a.Width = 90;
                    a.Height = 40;
                    a.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    a.RadiusX = 6;
                    a.RadiusY = 6;
                    a.HorizontalAlignment = (HorizontalAlignment)1;
                    a.VerticalAlignment = (VerticalAlignment)1;
                    a.Tag = 10 * i + j;

                    a.MouseMove += achieve_move;
                    a.MouseLeave += leave;

                    x.Children.Add(a);
                }
            }

            achievefield_hint = new Grid[8, 10];
            achievefield_hint_bg = new Rectangle[8, 10];
            achievefield_hint_texts = new TextBlock[8, 10];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    achievefield_hint[i, j] = new Grid();
                    Grid x = achievefield_hint[i, j];
                    x.Width = 16;
                    x.Height = 16;
                    x.HorizontalAlignment = (HorizontalAlignment)2;
                    x.VerticalAlignment = (VerticalAlignment)0;
                    Grid.SetRow(x, i);
                    Grid.SetColumn(x, j);

                    m.achieves_center_grid.Children.Add(x);
                    x.Visibility = (Visibility)1;


                    achievefield_hint_bg[i, j] = new Rectangle();
                    Rectangle a = achievefield_hint_bg[i, j];
                    a.Width = 16;
                    a.Height = 16;
                    a.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    a.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    a.StrokeThickness = 0.5;
                    a.RadiusX = 8;
                    a.RadiusY = 8;
                    a.HorizontalAlignment = (HorizontalAlignment)1;
                    a.VerticalAlignment = (VerticalAlignment)1;

                    x.Children.Add(a);




                    achievefield_hint_texts[i, j] = new TextBlock();
                    TextBlock b = achievefield_hint_texts[i, j];
                    b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    b.HorizontalAlignment = (HorizontalAlignment)1;
                    b.VerticalAlignment = (VerticalAlignment)1;
                    b.FontSize = 14;
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);

                    x.Children.Add(b);
                }
            }
        }

    }
}
