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
using System.Windows.Media.Media3D;
using System.Xml.Linq;

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
        public Grid find_grid(string name)
        {
            return (Grid)m.FindName(name);
        }

        public void visibility_transfer(FrameworkElement f, bool b)
        {
            if (b)
            {
                f.Visibility = Visibility.Visible;
            }
            else
            {
                f.Visibility = Visibility.Hidden;
            }
        }

        public void visual_unlock(string name, bool visible = true)
        {
            FrameworkElement f = (FrameworkElement)m.FindName(name);
            visibility_transfer(f, visible);
        }

        public Grid custom_button(string namebase, Panel parent,
            double width, double height, string text, double fontsize = 11)
        {
            double radius = Math.Min(width / 2, height / 2);

            Grid grid = new Grid()
            { 
                Name = namebase + "_grid",
            };
            reg_name(parent, grid);

            Rectangle bg = new Rectangle()
            {
                Name = namebase + "_背景",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stroke = getSCB(Colors.Black),
                Fill = getSCB(Colors.White),
                Width = width,
                Height = height,
                RadiusX = radius,
                RadiusY = radius,
            };
            reg_name(grid, bg);

            TextBlock textBlock = new TextBlock()
            {
                Name = namebase + "_文字",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Text = text,
                FontSize = fontsize,
            };
            reg_name(grid, textBlock);

            Rectangle cover = new Rectangle()
            {
                Name = namebase,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stroke = getSCB(Colors.Black),
                Fill = C(0, 0, 0, 0),
                Width = width,
                Height = height,
                RadiusX = radius,
                RadiusY = radius,
            };
            set_lbtn(cover);
            reg_name(grid, cover);

            return grid;
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

                custom_button(name_base + "_详细信息", grid2, 80, 24, "详细信息");
                grid_btn = custom_button(name_base + "_收集", grid2, 80, 24, "手动收集");
                Grid.SetColumn(grid_btn, 1);
                grid_btn = custom_button(name_base + "_升级", grid2, 80, 24, "升级！");
                Grid.SetColumn(grid_btn, 2);
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
                for (int i = 0; i < 4; i++)
                {
                    tab.RowDefinitions.Add(new RowDefinition());
                    tab.ColumnDefinitions.Add(new ColumnDefinition());
                }
                foreach (upgrade u in upgrades)
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

                    visibility_transfer(grid, u.unlocked);

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

                        Grid btn = custom_button("制造_升级_" + type + "_" + u.name,
                            grid, 80, 24, "制造"); 
                        btn.Height = 25;
                        btn.VerticalAlignment = VerticalAlignment.Bottom;
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

                        Grid btn = custom_button("制造_升级_" + type + "_" + u.name,
                            grid, 50, 20, "制造");
                        btn.HorizontalAlignment = HorizontalAlignment.Right;
                        btn.VerticalAlignment = VerticalAlignment.Bottom;
                    }
                }
            }
        }

        public Dictionary<string, position> enemies_group_positions = new Dictionary<string, position>();
        public void visual_create_enemies()
        {
            Grid option = m.战斗_option_grid;
            Grid main = m.战斗_场景_grid;
            TextBlock t;
            foreach (KeyValuePair<string, Dictionary<string, enemy>> pairs in enemies)
            {
                string group = pairs.Key;
                position p = enemies_group_positions[group];

                string name_base = "战斗_场景_" + group;
                #region option
                Grid option_grid = custom_button(name_base, option, 120, 36, group, 20);
                Grid.SetRow(option_grid, p.i);
                Grid.SetColumn(option_grid, p.j);

                int n = p.i * option.ColumnDefinitions.Count + p.j;
                visibility_transfer(option_grid, unlocks.fight_unlock[n]);
                #endregion option

                #region main

                Grid target = new Grid()
                {
                    Name = "战斗_场景_" + group + "_target_grid",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 150,
                    Height = 350,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                reg_name(main, target);
                target.Visibility = Visibility.Hidden;

                Grid enemies = new Grid()
                {
                    Name = "战斗_场景_" + group + "_enemy_grid",
                };
                reg_name(target, enemies);
                for(int i = 0; i < 7; i++)
                {
                    enemies.RowDefinitions.Add(new RowDefinition());
                }

                t = new TextBlock()
                {
                    Name = "战斗_场景_" + group + "_text",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = C(255, 255, 255),
                    TextWrapping = TextWrapping.Wrap,
                    Text = "选择敌人：",
                    FontSize = 24,
                    Margin = new Thickness(0, 0, 0, 0),
                };
                reg_name(enemies, t);
                
                foreach (KeyValuePair<string, enemy> pair in pairs.Value)
                {
                    string name = pair.Key;
                    int row = pair.Value.p.i;
                    string enemy_base = "战斗_场景_" + group + "_enemy_" + name + "_" + row;
                    Grid e = custom_button(enemy_base, enemies, 120, 36, name, 20);
                    Grid.SetRow(e, row);
                }
                #endregion main
            }
        }

        Dictionary<string, ARGB> potion_colors = new Dictionary<string, ARGB>();
        public void visual_create_magic()
        {
            Grid main;
            TextBlock t;
            Rectangle rect;

            #region 祭坛
            main = m.魔法_祭坛_祭品_grid;
            foreach (KeyValuePair<string, position> pair in magic_altar.positions)
            {
                string name = pair.Key;
                position p = pair.Value;

                Grid grid = new Grid()
                {
                    Name = "魔法_祭坛_祭品_" + name + "_grid",
                    Background = C(63, 0, 0, 0),
                };
                reg_name(main, grid);
                Grid.SetRow(grid, p.i);
                Grid.SetColumn(grid, p.j);

                t = new TextBlock()
                {
                    Name = "魔法_祭坛_祭品_" + name + "_text",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    FontSize = 10,
                    Margin = new Thickness(0, 5, 0, 0),
                };
                reg_name(grid, t);

                Grid btn = custom_button("魔法_祭坛_祭品_" + name + "_献祭", grid, 80, 26, "献祭！", 14);
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.VerticalAlignment = VerticalAlignment.Bottom;
                btn.Margin = new Thickness(0, 0, 0, 3);
            }
            #endregion 祭坛

            #region 附魔/药水
            Grid ec1 = m.魔法_菜单_附魔_target_grid;
            Grid ec2 = m.魔法_菜单_药水_target_grid;
            foreach (KeyValuePair<string, enchant> pair in enchants)
            {
                enchant ec = pair.Value;
                string sub = "附魔";
                if (ec.is_potion)
                {
                    sub = "药水";
                }

                LinearGradientBrush lgb = get_lgb();
                Grid grid = new Grid()
                {
                    Name = "魔法_次_" + sub + "_" + ec.name + "_grid",
                    Background = lgb,
                };
                if (ec.is_potion)
                {
                    reg_name(ec2, grid);
                }
                else
                {
                    reg_name(ec1, grid);
                }
                Grid.SetRow(grid, ec.p.i);
                Grid.SetColumn(grid, ec.p.j);
                visibility_transfer(grid, ec.unlocked);

                string namebase = "魔法_" + sub + "_" + ec.name;
                string diff = "";
                int i_diff = 0;

                t = new TextBlock()
                {
                    Name = namebase + "_text",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Text = ec.name,
                    FontSize = 16,
                    Margin = new Thickness(10, 5, 0, 0),
                };
                if (ec.is_potion)
                {
                    t.Foreground = potion_colors[ec.name].toBrush();
                }
                else
                {
                    t.Foreground = find_resource(ec.name).text_color();
                }
                reg_name(grid, t);

                if (!ec.is_potion)
                {
                    diff = "速度";
                    i_diff = 20;
                }
                t = new TextBlock()
                {
                    Name = namebase + "_" + diff + "等级",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    FontSize = 14,
                    Margin = new Thickness(120 - i_diff, 7, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_数量t",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    Text = "获得：",
                    Margin = new Thickness(10, 61, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_数量",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(40, 61, 0, 0),
                };
                reg_name(grid, t);

                if (!ec.is_potion)
                {
                    t = new TextBlock()
                    {
                        Name = namebase + "_数量s",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(160, 61, 0, 0),
                    };
                    reg_name(grid, t);
                }

                t = new TextBlock()
                {
                    Name = namebase + "_价格t",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    Text = "需求：",
                    Margin = new Thickness(10, 25, 0, 0),
                };
                reg_name(grid, t);

                for (int i = 1; i <= 3; i++)
                {
                    t = new TextBlock()
                    {
                        Name = namebase + "_价格" + i,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = getSCB(Colors.White),
                        Margin = new Thickness(40, 13 + 12 * i, 0, 0),
                    };
                    reg_name(grid, t);

                    t = new TextBlock()
                    {
                        Name = namebase + "_价格" + i + "s",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = getSCB(Colors.White),
                        Margin = new Thickness(160, 13 + 12 * i, 0, 0),
                    };
                    reg_name(grid, t);
                }

                t = new TextBlock()
                {
                    Name = namebase + "_时间",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = C(0, 235, 9),
                    Margin = new Thickness(0, 75, 0, 0),
                };
                reg_name(grid, t);

                Grid btn;
                diff = "开始附魔";
                if (ec.is_potion)
                {
                    diff = "开始配制";
                } 
                btn = custom_button(namebase, grid, 80, 24, diff);
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.VerticalAlignment = VerticalAlignment.Bottom;

                diff = "减速";
                if (ec.is_potion)
                {
                    diff = "降级";
                }
                btn = custom_button(namebase + "_" + diff, grid, 50, 18, diff);
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.VerticalAlignment = VerticalAlignment.Bottom;
                btn.Margin = new Thickness(25, 0, 0, 3);

                diff = "加速";
                if (ec.is_potion)
                {
                    diff = "升级";
                }
                btn = custom_button(namebase + "_" + diff, grid, 50, 18, diff);
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Bottom;
                btn.Margin = new Thickness(0, 0, 25, 3);

                LinearGradientBrush linearGradientBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0),
                };
                linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0));
                linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Lime, 1));
                rect = new Rectangle()
                {
                    Name = namebase + "_进度条_底",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Stroke = new SolidColorBrush(Colors.Black),
                    Width = 236.7,
                    Height = 10,
                    RadiusX = 5,
                    RadiusY = 5,
                    Margin = new Thickness(0, 90, 0, 0),
                    Fill = linearGradientBrush,
                };
                reg_name(grid, rect);

                rect = new Rectangle()
                {
                    Name = namebase + "_进度条_顶",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 100,
                    Height = 10,
                    RadiusX = 5,
                    RadiusY = 5,
                    Margin = new Thickness(0, 90, 15, 0),
                    Fill = C(207, 0, 0, 0),
                };
                reg_name(grid, rect);
            }
            #endregion 附魔/药水

            #region 法术
            main = m.魔法_菜单_法术_target_grid;
            for (int i = 1; i <= spell_all_page; i++)
            {
                Grid page = new Grid()
                {
                    Name = "魔法_法术_第" + i + "页_grid",
                };
                Grid.SetColumnSpan(page, 2);
                Grid.SetRow(page, 1);
                for(int k = 0; k < 3; k++)
                {
                    page.RowDefinitions.Add(new RowDefinition());
                }
                reg_name(main, page);
            }

            foreach(KeyValuePair<string, spell> pair in spells)
            {
                string name = pair.Key;
                spell s = pair.Value;

                if (s.pos == null)
                {
                    continue;
                }

                string namebase = "魔法_法术_" + name;
                Grid grid = new Grid()
                {
                    Name = namebase + "_grid",
                };
                Grid.SetRow(grid, s.pos.i);
                Grid parent = find_grid("魔法_法术_第" + s.pos.j + "页_grid");
                reg_name(parent, grid);
                visibility_transfer(grid, s.unlocked);

                LinearGradientBrush linearGradientBrush = new LinearGradientBrush() 
                { 
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0)
                };
                linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(127, 0, 100, 100), 0));
                linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(127, 255, 255, 0), 0.5));
                linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(127, 100, 100, 255), 1));
                rect = new Rectangle()
                {
                    Name = namebase + "_进度条_底",
                    Stroke = getSCB(Colors.Black),
                    StrokeThickness = 2,
                    Fill = linearGradientBrush,
                };
                reg_name(grid, rect);

                rect = new Rectangle()
                {
                    Name = namebase + "_进度条_顶",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Stroke = C(100, 150, 200),
                    StrokeThickness = 2,
                    Fill = C(63, 0, 0),
                };
                reg_name(grid, rect);

                t = new TextBlock()
                {
                    Name = namebase + "_t1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    FontSize = 16,
                    Margin = new Thickness(10, 5, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_t2",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = C(200, 200, 255),
                    FontSize = 14,
                    Margin = new Thickness(90, 7, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_t3",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = C(0, 200, 255),
                    FontSize = 13,
                    Margin = new Thickness(195, 7, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_t4",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = C(255, 255, 100),
                    FontSize = 14,
                    Margin = new Thickness(340, 7, 0, 0),
                };
                reg_name(grid, t);

                #region 消耗
                t = new TextBlock()
                {
                    Name = namebase + "_消耗",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    FontSize = 14,
                    Text = "消耗:",
                    Margin = new Thickness(20, 25, 0, 0),
                };
                reg_name(grid, t);

                for(int i = 1; i <= 4; i++)
                {
                    t = new TextBlock()
                    {
                        Name = namebase + "_消耗" + i,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = getSCB(Colors.White),
                        FontSize = 14,
                        Margin = new Thickness(55, 9 + 17 * i, 0, 0),
                    };
                    reg_name(grid, t);
                }
                #endregion 消耗

                #region 效果
                t = new TextBlock()
                {
                    Name = namebase + "_效果",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.Yellow),
                    Margin = new Thickness(180, 25, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_主动效果",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.Lime),
                    Margin = new Thickness(410, 4, 0, 0),
                };
                reg_name(grid, t);

                t = new TextBlock()
                {
                    Name = namebase + "_被动效果",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.Cyan),
                    Margin = new Thickness(410, 57, 0, 0),
                };
                reg_name(grid, t);
                #endregion 效果

                #region 主动消耗
                t = new TextBlock()
                {
                    Name = namebase + "_主动消耗",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = getSCB(Colors.White),
                    Text = "消耗:",
                    Margin = new Thickness(650, 25, 0, 0),
                };
                reg_name(grid, t);

                for(int i = 1; i <= 4; i++)
                {
                    t = new TextBlock()
                    {
                        Name = namebase + "_主动消耗" + i,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = getSCB(Colors.White),
                        Margin = new Thickness(680, 9 + 16 * i, 0, 0),
                    };
                    reg_name(grid, t);
                }
                #endregion 主动消耗

                Grid btn;
                btn = custom_button(namebase + "_学习", grid, 80, 20, "学习", 14);
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Bottom;
                btn.Margin = new Thickness(0, 0, 400, 0);

                btn = custom_button(namebase + "_施法", grid, 80, 20, "施法", 14);
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Bottom;

                btn = custom_button(namebase + "_施法_升级", grid, 60, 20, "升级");
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Top;
                btn.Margin = new Thickness(0, 6, 10, 0);

                btn = custom_button(namebase + "_施法_降级", grid, 60, 20, "降级");
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Top;
                btn.Margin = new Thickness(0, 6, 85, 0);
            }
            #endregion 法术
        }
    }
}
