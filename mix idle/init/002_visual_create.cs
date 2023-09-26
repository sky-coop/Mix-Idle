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
using System.Windows.Ink;

namespace mix_idle
{
    public partial class gamestats
    {
        [Serializable]
        public class position
        {
            public string parent_name;
            public int i = 0;
            public int j = 0;

            public position(string parent_name, int i, int j)
            {
                this.parent_name = parent_name;
                this.i = i;
                this.j = j;
            }
        }

        Dictionary<string, List<upgrade>> position_groups = new Dictionary<string, List<upgrade>>();
        public void position_save(string group, upgrade u, position p)
        {
            u.pos = p;
            if (!position_groups.ContainsKey(group))
            {
                position_groups[group] = new List<upgrade>();
            }
            position_groups[group].Add(u);
        }
        private void set_lbtn(FrameworkElement r)
        {
            r.MouseEnter += rectangle_cover_enter;
            r.MouseLeave += rectangle_cover_leave;
            r.MouseLeftButtonDown += rectangle_cover_down;
            r.MouseLeftButtonUp += rectangle_cover_up;
            r.MouseMove += rectangle_cover_move;
        }

        private void set_lrbtn(FrameworkElement r)
        {
            r.MouseEnter += rectangle_cover_enter;
            r.MouseLeave += rectangle_cover_leave;
            r.MouseLeftButtonDown += rectangle_cover_down;
            r.MouseLeftButtonUp += rectangle_cover_up;
            r.MouseRightButtonDown += rectangle_cover_rdown;
            r.MouseRightButtonUp += rectangle_cover_rup;
            r.MouseMove += rectangle_cover_move;
        }

        public void reg_name(Panel parent, FrameworkElement elem)
        {
            FrameworkElement temp = null;
            foreach(FrameworkElement f in parent.Children)
            {
                if (f.Name == elem.Name)
                {
                    temp = f;
                }
            }
            if(temp != null)
            {
                parent.Children.Remove(temp);
            }
            parent.Children.Add(elem);
            if (m.FindName(elem.Name) != null)
            {
                m.UnregisterName(elem.Name);
                m.RegisterName(elem.Name, elem);
            }
            else
            {
                m.RegisterName(elem.Name, elem);
            }
        }

        public FrameworkElement find_name(string name)
        {
            return (FrameworkElement)m.FindName(name);
        }

        public void visual_create_bp()
        {
            Grid main = m.方块_grid;
            foreach (KeyValuePair<string, resource> pair in res_table["方块"])
            {
                resource r = pair.Value;
                int i = (r.show_loc - 1) / main.ColumnDefinitions.Count;
                int j = (r.show_loc - 1) % main.ColumnDefinitions.Count;
                string name_base = "方块_" + pair.Key;
                Grid grid = new Grid() 
                {
                    Name = name_base + "_grid",
                };
                Grid.SetRow(grid, i);
                Grid.SetColumn(grid, j);
                reg_name(main, grid);

                if (block_producters.ContainsKey(r.name) && block_producters[r.name].unlocked)
                {
                    grid.Visibility = Visibility.Visible;
                }
                else
                {
                    grid.Visibility = Visibility.Hidden;
                }

                RowDefinition rd = new RowDefinition
                {
                    Height = new GridLength(0.7, GridUnitType.Star)
                };
                grid.RowDefinitions.Add(rd);

                rd = new RowDefinition
                {
                    Height = new GridLength(1.3, GridUnitType.Star)
                };
                grid.RowDefinitions.Add(rd);

                grid.RowDefinitions.Add(new RowDefinition());

                #region row 1
                TextBlock t;
                t = new TextBlock()
                {
                    Name = name_base + "_等级",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = r.text_color(),
                    FontSize = 18,
                    Margin = new Thickness(0, 3, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = name_base + "_当前产量",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = r.text_color(),
                    Margin = new Thickness(15, 0, 0, 15),
                };
                Grid.SetRow(t, 1);
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = name_base + "_当前耗时",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = r.text_color(),
                    Margin = new Thickness(140, 0, 0, 15),
                };
                Grid.SetRow(t, 1);
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = name_base + "_最大产量",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = r.text_color(),
                    Margin = new Thickness(15, 0, 0, 0),
                };
                Grid.SetRow(t, 1);
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = name_base + "_最大耗时",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = r.text_color(),
                    Margin = new Thickness(140, 0, 0, 0),
                };
                Grid.SetRow(t, 1);
                reg_name(grid, t);

                Rectangle rect;
                rect = new Rectangle()
                {
                    Name = name_base + "_进度条_底",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Stroke = getSCB(Colors.Black),
                    Fill = new SolidColorBrush(Color.FromRgb(10, 4, 57)),
                    Height = 12,
                    Width = 250,
                    RadiusX = 6,
                    RadiusY = 6,
                    Margin = new Thickness(0, 4, 0, 0),
                };
                Grid.SetRow(rect, 1);
                reg_name(grid, rect);

                rect = new Rectangle()
                {
                    Name = name_base + "_进度条_顶",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Stroke = getSCB(Colors.Black),
                    Fill = r.text_color(),
                    Height = 12,
                    Width = 0,
                    RadiusX = 6,
                    RadiusY = 6,
                    Margin = new Thickness(0, 4, 0, 0),
                };
                Grid.SetRow(rect, 1);
                reg_name(grid, rect);
                #endregion row 1

                #region row 2
                Grid grid2 = new Grid()
                {
                    Name = name_base + "_按键_grid",
                };
                Grid.SetRow(grid2, 2);
                reg_name(grid, grid2);

                for (int k = 0; k < 3; k++)
                {
                    grid2.ColumnDefinitions.Add(new ColumnDefinition());
                }

                Grid grid_btn;
                
                #region 详细信息
                grid_btn = new Grid()
                {
                    Name = name_base + "_详细信息_grid",
                };
                reg_name(grid2, grid_btn);

                rect = new Rectangle()
                {
                    Name = name_base + "_详细信息_背景",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = getSCB(Colors.Black),
                    Fill = getSCB(Colors.White),
                    Width = 80,
                    Height = 24,
                    RadiusX = 12,
                    RadiusY = 12,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                reg_name(grid_btn, rect);

                t = new TextBlock()
                {
                    Name = name_base + "_详细信息_文字",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Text = "详细信息",
                };
                reg_name(grid_btn, t);

                rect = new Rectangle()
                {
                    Name = name_base + "_详细信息",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = getSCB(Colors.Black),
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                    Width = 80,
                    Height = 24,
                    RadiusX = 12,
                    RadiusY = 12,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                rect.MouseEnter += rectangle_cover_enter;
                rect.MouseLeave += rectangle_cover_leave;
                rect.MouseMove += rectangle_cover_move;
                reg_name(grid_btn, rect);
                #endregion 详细信息


                #region 收集
                grid_btn = new Grid()
                {
                    Name = name_base + "_收集_grid",
                };
                Grid.SetColumn(grid_btn, 1);
                reg_name(grid2, grid_btn);

                rect = new Rectangle()
                {
                    Name = name_base + "_收集_背景",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = getSCB(Colors.Black),
                    Fill = getSCB(Colors.White),
                    Width = 80,
                    Height = 24,
                    RadiusX = 12,
                    RadiusY = 12,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                reg_name(grid_btn, rect);

                t = new TextBlock()
                {
                    Name = name_base + "_收集_文字",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Text = "手动收集",
                };
                reg_name(grid_btn, t);

                rect = new Rectangle()
                {
                    Name = name_base + "_收集",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = getSCB(Colors.Black),
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                    Width = 80,
                    Height = 24,
                    RadiusX = 12,
                    RadiusY = 12,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                rect.MouseLeftButtonDown += rectangle_cover_down;
                rect.MouseLeftButtonUp += rectangle_cover_up;
                rect.MouseEnter += rectangle_cover_enter;
                rect.MouseLeave += rectangle_cover_leave;
                rect.MouseMove += rectangle_cover_move;
                reg_name(grid_btn, rect);
                #endregion 收集

                #region 升级
                grid_btn = new Grid()
                {
                    Name = name_base + "_升级_grid",
                };
                Grid.SetColumn(grid_btn, 2);
                reg_name(grid2, grid_btn);

                rect = new Rectangle()
                {
                    Name = name_base + "_升级_背景",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = getSCB(Colors.Black),
                    Fill = getSCB(Colors.White),
                    Width = 80,
                    Height = 24,
                    RadiusX = 12,
                    RadiusY = 12,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                reg_name(grid_btn, rect);

                t = new TextBlock()
                {
                    Name = name_base + "_升级_文字",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Text = "升级！",
                };
                reg_name(grid_btn, t);

                rect = new Rectangle()
                {
                    Name = name_base + "_升级",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = getSCB(Colors.Black),
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                    Width = 80,
                    Height = 24,
                    RadiusX = 12,
                    RadiusY = 12,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                rect.MouseLeftButtonDown += rectangle_cover_down;
                rect.MouseLeftButtonUp += rectangle_cover_up;
                rect.MouseEnter += rectangle_cover_enter;
                rect.MouseLeave += rectangle_cover_leave;
                rect.MouseMove += rectangle_cover_move;
                reg_name(grid_btn, rect);
                #endregion 升级
                #endregion row 2
            }
        }

        public void visual_create_craft()
        {
            Grid main = (Grid)m.FindName("制造_主_grid");

            foreach(Grid tab in main.Children)
            {
                string type = tab.Name.Split('_')[2];
                if (!position_groups.ContainsKey(type))
                {
                    continue;
                }
                List<upgrade> upgrades = position_groups[type];
                foreach(upgrade u in upgrades)
                {
                    Grid grid = new Grid() 
                    { 
                        Name = "制造_次_" + type + "_" + u.name + "_grid",
                        Background = getSCB(Color.FromArgb(63, 0, 0, 0)),
                    };
                    if((u.pos.i + u.pos.j) % 2 == 1)
                    {
                        grid.Background = getSCB(Color.FromArgb(127, 0, 0, 0));
                    }
                    Grid.SetRow(grid, u.pos.i);
                    Grid.SetColumn(grid, u.pos.j);
                    reg_name(tab, grid);

                    if (u.unlocked)
                    {
                        grid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grid.Visibility = Visibility.Hidden;
                    }

                    TextBlock t;
                    Rectangle rect;
                    if (type == "材料")
                    {
                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_text",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            FontSize = 20,
                            Margin = new Thickness(0, 10, 0, 0),
                        };
                        reg_name(grid, t);

                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_价格",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            FontSize = 14,
                            Margin = new Thickness(0, 40, 0, 0),
                        };
                        reg_name(grid, t);

                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_数量",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            FontSize = 14,
                            Margin = new Thickness(0, 60, 0, 0),
                        };
                        reg_name(grid, t);

                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_公式",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            FontSize = 14,
                            Margin = new Thickness(0, 80, 0, 0),
                        };
                        reg_name(grid, t);

                        Grid btn = new Grid()
                        #region
                        {
                            Name = "制造_升级_" + type + "_" + u.name + "_grid",
                            Height = 25,
                            VerticalAlignment = VerticalAlignment.Bottom,
                        };
                        reg_name(grid, btn);

                        rect = new Rectangle()
                        {
                            Name = "制造_升级_" + type + "_" + u.name + "_背景",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Stroke = new SolidColorBrush(Colors.Black),
                            Fill = new SolidColorBrush(Colors.White),
                            Width = 80,
                            Height = 24,
                            RadiusX = 12,
                            RadiusY = 12,
                            Margin = new Thickness(0, 0, 0, 0),
                        };
                        reg_name(btn, rect);

                        t = new TextBlock()
                        {
                            Name = "制造_升级_" + type + "_" + u.name + "_文字",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap,
                            Text = "制造",
                        };
                        reg_name(btn, t);

                        rect = new Rectangle()
                        {
                            Name = "制造_升级_" + type + "_" + u.name,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Stroke = new SolidColorBrush(Colors.Black),
                            Fill = C(0, 0, 0, 0),
                            Width = 80,
                            Height = 24,
                            RadiusX = 12,
                            RadiusY = 12,
                            Margin = new Thickness(0, 0, 0, 0),
                        };
                        set_lbtn(rect);
                        reg_name(btn, rect);
                        #endregion
                    }
                    else
                    {
                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_text",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            FontSize = 14,
                            Margin = new Thickness(10, 6, 0, 0),
                        };
                        reg_name(grid, t);

                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_等级",
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            Margin = new Thickness(0, 8, 15, 0),
                        };
                        reg_name(grid, t);

                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_描述",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = getSCB(Colors.White),
                            Margin = new Thickness(10, 25, 0, 0),
                        };
                        reg_name(grid, t);

                        t = new TextBlock()
                        {
                            Name = "制造_次_" + type + "_" + u.name + "_价格_title",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            Text = "价格：",
                            Foreground = getSCB(Colors.White),
                            Margin = new Thickness(10, 80, 0, 0),
                        };
                        reg_name(grid, t);

                        for(int i = 1; i <= 3; i++)
                        {
                            t = new TextBlock()
                            {
                                Name = "制造_次_" + type + "_" + u.name + "_价格_" + i,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = getSCB(Colors.White),
                                Margin = new Thickness(40, 65 + 15 * i, 0, 0),
                            };
                            reg_name(grid, t);
                        }

                        Grid btn = new Grid()
                        #region
                        {
                            Name = "制造_升级_" + type + "_" + u.name + "_grid",
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Bottom,
                        };
                        reg_name(grid, btn);

                        rect = new Rectangle()
                        {
                            Name = "制造_升级_" + type + "_" + u.name + "_背景",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Stroke = new SolidColorBrush(Colors.Black),
                            Fill = new SolidColorBrush(Colors.White),
                            Width = 50,
                            Height = 20,
                            RadiusX = 10,
                            RadiusY = 10,
                            Margin = new Thickness(0, 0, 0, 0),
                        };
                        reg_name(btn, rect);

                        t = new TextBlock()
                        {
                            Name = "制造_升级_" + type + "_" + u.name + "_文字",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap,
                            Text = "制造",
                        };
                        reg_name(btn, t);

                        rect = new Rectangle()
                        {
                            Name = "制造_升级_" + type + "_" + u.name,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Stroke = new SolidColorBrush(Colors.Black),
                            Fill = C(0, 0, 0, 0),
                            Width = 50,
                            Height = 20,
                            RadiusX = 10,
                            RadiusY = 10,
                            Margin = new Thickness(0, 0, 0, 0),
                        };
                        set_lbtn(rect);
                        reg_name(btn, rect);
                        #endregion
                    }
                }
            }
        }
    }
}
