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
        private void g2_elem_init()
        {
            g2_ball_id = 0;
            g2_balls.Clear();

            Grid app_grid = vm_get_app_grid();

            Grid root = new Grid
            {
                Name = "聚能光珠",
                Visibility = Visibility.Hidden,
                Width = app_grid.Width,
                Height = app_grid.Height
            };
            app_grid.Children.Add(root);
            vm_elems.Add(root.Name, root);

            //背景
            Rectangle bg = new Rectangle
            {
                Name = "g2_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(50, 50, 50)),
            };
            root.Children.Add(bg);
            vm_assign(bg);

            Grid menu = new Grid
            {
                Name = "g2_menu",
                Width = app_grid.Width,
                Height = app_grid.Height
            };
            root.Children.Add(menu);
            vm_elems.Add(menu.Name, menu);


            TextBlock title = new TextBlock
            {
                Name = "g2_title",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, app_grid.Height * 0.03, 0, 0),
                FontSize = 28,
                Foreground = getSCB(Color.FromRgb(0, 255, 255)),
                Text = "聚能光珠"
            };
            menu.Children.Add(title);
            vm_elems.Add(title.Name, title);

            Grid mode_grid = new Grid
            {
                Name = "g2_mode_grid",
                Width = app_grid.Width * 0.6,
                Height = app_grid.Height * 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, app_grid.Height * 0.15)
            };
            mode_grid.RowDefinitions.Add(new RowDefinition());
            mode_grid.RowDefinitions.Add(new RowDefinition());
            mode_grid.RowDefinitions.Add(new RowDefinition());
            mode_grid.ColumnDefinitions.Add(new ColumnDefinition());
            mode_grid.ColumnDefinitions.Add(new ColumnDefinition());
            menu.Children.Add(mode_grid);
            vm_elems.Add(mode_grid.Name, mode_grid);

            Grid[] mode_selecter_grid = new Grid[6];
            Rectangle[] mode_selecter_bg = new Rectangle[6];
            TextBlock[] mode_selecter_text = new TextBlock[6];
            Rectangle[] mode_selecter = new Rectangle[6];
            for (int i = 0; i < 6; i++)
            {
                mode_selecter_grid[i] = new Grid();
                Grid x = mode_selecter_grid[i];
                Grid.SetRow(x, i / 2);
                Grid.SetColumn(x, i % 2);
                x.HorizontalAlignment = HorizontalAlignment.Stretch;
                x.VerticalAlignment = VerticalAlignment.Stretch;
                x.Name = "g2_mode_selecter_" + Convert.ToString(i) + "_grid";
                mode_grid.Children.Add(x);
                vm_elems.Add(x.Name, x);

                mode_selecter_bg[i] = new Rectangle();
                Rectangle r = mode_selecter_bg[i];
                r.Fill = getSCB(Color.FromRgb(255, 255, 255));
                r.Stroke = getSCB(Color.FromRgb(50, 100, 50));
                r.StrokeThickness = 2;
                r.HorizontalAlignment = HorizontalAlignment.Center;
                r.VerticalAlignment = VerticalAlignment.Center;
                r.Width = app_grid.Width * 0.25;
                r.Height = app_grid.Height * 0.15;
                r.RadiusX = r.Height / 3;
                r.RadiusY = r.Height / 3;
                r.Name = "g2_mode_selecter_" + Convert.ToString(i) + "_bg";
                x.Children.Add(r);
                vm_elems.Add(r.Name, r);

                mode_selecter_text[i] = new TextBlock();
                TextBlock t = mode_selecter_text[i];
                t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                t.FontSize = 20;
                t.HorizontalAlignment = HorizontalAlignment.Center;
                t.VerticalAlignment = VerticalAlignment.Center;
                t.Text = "";
                t.Name = "g2_mode_selecter_" + Convert.ToString(i) + "_text";
                x.Children.Add(t);
                vm_elems.Add(t.Name, t);

                mode_selecter[i] = new Rectangle();
                r = mode_selecter[i];
                r.Fill = getSCB(Color.FromArgb(0, 0, 0, 0));
                r.HorizontalAlignment = HorizontalAlignment.Center;
                r.VerticalAlignment = VerticalAlignment.Center;
                r.Width = app_grid.Width * 0.25;
                r.Height = app_grid.Height * 0.15;
                r.RadiusX = r.Height / 3;
                r.RadiusY = r.Height / 3;
                r.Name = "g2_mode_selecter_" + Convert.ToString(i);
                x.Children.Add(r);
                vm_elems.Add(r.Name, r);
                r.Tag = 0;

                r.MouseEnter += rectangle_cover_enter;
                r.MouseLeave += rectangle_cover_leave;
                r.MouseLeftButtonDown += rectangle_cover_down;
                r.MouseLeftButtonUp += rectangle_cover_up;
                r.MouseMove += rectangle_cover_move;
            }

            mode_selecter_text[0].Text = "经典模式";
            mode_selecter_text[1].Text = "连发模式";

            mode_selecter_text[3].Text = "地图编辑器";



            Grid classic_menu = new Grid
            {
                Name = "g2_classic_menu",
                Visibility = Visibility.Hidden,
                Width = app_grid.Width,
                Height = app_grid.Height
            };
            root.Children.Add(classic_menu);
            vm_elems.Add(classic_menu.Name, classic_menu);

            Rectangle classic_menu_background = new Rectangle
            {
                Name = "g2_classic_menu_background",
                Width = app_grid.Width,
                Height = app_grid.Height,
                Fill = getSCB(Color.FromArgb(150, 0, 0, 0))
            };
            classic_menu_background.MouseLeftButtonUp += Classic_menu_background_MouseLeftButtonUp;
            classic_menu.Children.Add(classic_menu_background);
            vm_elems.Add(classic_menu_background.Name, classic_menu_background);

            TextBlock classic_menu_text = new TextBlock
            {
                Name = "g2_classic_menu_text",
                Margin = new Thickness(0, classic_menu.Height * 0.1, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(255, 255, 0)),
                Text = "经典模式：按方向键来移动地图，点击地图外可返回"
            };
            classic_menu.Children.Add(classic_menu_text);
            vm_elems.Add(classic_menu_text.Name, classic_menu_text);

            Grid classic_level_map_container = new Grid
            {
                Width = app_grid.Width * 0.8,
                Height = app_grid.Height * 0.8,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, classic_menu.Height * 0.03),
                ClipToBounds = true,
                Name = "g2_classic_level_map_container"
            };
            classic_menu.Children.Add(classic_level_map_container);
            vm_elems.Add(classic_level_map_container.Name, classic_level_map_container);

            Grid classic_level_map = new Grid
            {
                Width = classic_level_map_container.Width * 5,
                Height = classic_level_map_container.Height * 5,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            LinearGradientBrush linear = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };
            linear.GradientStops.Add(new GradientStop(Color.FromRgb(0, 150, 200), 0));
            linear.GradientStops.Add(new GradientStop(Color.FromRgb(200, 150, 0), 1));
            classic_level_map.Background = linear;
            classic_level_map.Name = "g2_classic_level_map";
            classic_level_map_container.Children.Add(classic_level_map);
            vm_elems.Add(classic_level_map.Name, classic_level_map);

            Grid classic_level_map_2 = new Grid
            {
                Width = classic_level_map_container.Width * 5,
                Height = classic_level_map_container.Height * 5,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Name = "g2_classic_level_map_2"
            };
            classic_level_map_container.Children.Add(classic_level_map_2);
            vm_elems.Add(classic_level_map_2.Name, classic_level_map_2);

            const int CLASSIC_LEVEL_AMOUNT = 20;
            Grid[] classic_level_grid = new Grid[CLASSIC_LEVEL_AMOUNT];
            Rectangle[] classic_level_bg = new Rectangle[CLASSIC_LEVEL_AMOUNT];
            TextBlock[] classic_level_text = new TextBlock[CLASSIC_LEVEL_AMOUNT];
            Rectangle[] classic_level = new Rectangle[CLASSIC_LEVEL_AMOUNT];
            for (int i = 0; i < CLASSIC_LEVEL_AMOUNT; i++)
            {
                string si = (i + 1).ToString();

                classic_level_grid[i] = new Grid();
                Grid cg = classic_level_grid[i];
                cg.HorizontalAlignment = HorizontalAlignment.Left;
                cg.VerticalAlignment = VerticalAlignment.Top;
                cg.Width = 50;
                cg.Height = 50;
                cg.Margin = new Thickness(10 + 100 * i, 10, 0, 0);
                cg.Name = "g2_classic_level_" + si + "_grid";
                classic_level_map_2.Children.Add(cg);
                vm_elems.Add(cg.Name, cg);

                classic_level_bg[i] = new Rectangle();
                Rectangle b = classic_level_bg[i];
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.RadiusX = 100;
                b.RadiusY = 100;
                b.Stroke = getSCB(Color.FromRgb(0, 0, 0));
                b.StrokeThickness = 2;
                b.Fill = getSCB(Color.FromRgb(100, 100, 100));
                b.Name = "g2_classic_level_" + si + "_bg";
                cg.Children.Add(b);
                vm_elems.Add(b.Name, b);

                classic_level_text[i] = new TextBlock();
                TextBlock t = classic_level_text[i];
                t.HorizontalAlignment = HorizontalAlignment.Center;
                t.VerticalAlignment = VerticalAlignment.Center;
                t.FontSize = 18;
                t.Text = si;
                t.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                t.Name = "g2_classic_level_" + si + "_text";
                cg.Children.Add(t);
                vm_elems.Add(t.Name, t);

                classic_level[i] = new Rectangle();
                Rectangle r = classic_level[i];
                r.HorizontalAlignment = HorizontalAlignment.Stretch;
                r.VerticalAlignment = VerticalAlignment.Stretch;
                r.RadiusX = 100;
                r.RadiusY = 100;
                r.Fill = getSCB(Color.FromArgb(0, 0, 0, 0));
                r.Name = "g2_classic_level_" + si;
                r.Tag = 0;

                /*
                r.MouseEnter += rectangle_cover_enter;
                r.MouseLeave += rectangle_cover_leave;
                r.MouseLeftButtonDown += rectangle_cover_down;
                r.MouseLeftButtonUp += rectangle_cover_up;*/
                r.MouseMove += rectangle_cover_move;

                cg.Children.Add(r);
                vm_elems.Add(r.Name, r);
            }
            classic_level_grid[1 - 1].Margin = new Thickness(20, 20, 0, 0);
            classic_level_grid[2 - 1].Margin = new Thickness(80, 100, 0, 0);
            classic_level_grid[3 - 1].Margin = new Thickness(180, 80, 0, 0);
            classic_level_grid[4 - 1].Margin = new Thickness(130, 30, 0, 0);
            classic_level_grid[5 - 1].Margin = new Thickness(150, 150, 0, 0);
            classic_level_grid[6 - 1].Margin = new Thickness(250, 110, 0, 0);
            classic_level_grid[7 - 1].Margin = new Thickness(225, 180, 0, 0);
            classic_level_grid[8 - 1].Margin = new Thickness(245, 20, 0, 0);
            classic_level_grid[9 - 1].Margin = new Thickness(320, 100, 0, 0);
            classic_level_grid[10 - 1].Margin = new Thickness(390, 110, 0, 0);
            classic_level_grid[11 - 1].Margin = new Thickness(420, 35, 0, 0);
            classic_level_grid[12 - 1].Margin = new Thickness(350, 200, 0, 0);
            classic_level_grid[13 - 1].Margin = new Thickness(470, 180, 0, 0);
            classic_level_grid[14 - 1].Margin = new Thickness(530, 50, 0, 0);
            classic_level_grid[15 - 1].Margin = new Thickness(500, 260, 0, 0);
            classic_level_grid[18 - 1].Margin = new Thickness(900, 200, 0, 0);

            Grid g2_game_container = new Grid
            {
                Name = "g2_game_container",
                Width = app_grid.Width,
                Height = app_grid.Height,
                Visibility = Visibility.Hidden
            };
            root.Children.Add(g2_game_container);
            vm_elems.Add(g2_game_container.Name, g2_game_container);

            Grid g2_game_option_container = new Grid
            {
                Name = "g2_game_option_container",
                Width = app_grid.Width * 0.3,
                Height = app_grid.Height,
                Background = getSCB(Color.FromRgb(100, 120, 140)),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            g2_game_container.Children.Add(g2_game_option_container);
            vm_elems.Add(g2_game_option_container.Name, g2_game_option_container);



            Grid g2_game_options_grid = new Grid
            {
                Name = "g2_game_options",
                Width = 210,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_options_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_option_container.Children.Add(g2_game_options_grid);
            vm_elems.Add(g2_game_options_grid.Name, g2_game_options_grid);

            //退出按键
            #region
            Grid g2_game_option_exit_grid = new Grid
            {
                Name = "g2_game_option_exit_grid",
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(g2_game_option_exit_grid, 6);
            g2_game_options_grid.Children.Add(g2_game_option_exit_grid);
            vm_assign(g2_game_option_exit_grid);

            Rectangle g2_game_option_exit_bg = new Rectangle
            {
                Name = "g2_game_option_exit_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(150, 150, 150)),
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_exit_grid.Children.Add(g2_game_option_exit_bg);
            vm_assign(g2_game_option_exit_bg);

            Line g2_game_option_exit_line1 = new Line
            {
                Name = "g2_game_option_exit_line1",
                X1 = 8,
                X2 = 22,
                Y1 = 8,
                Y2 = 22,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 2
            };
            g2_game_option_exit_grid.Children.Add(g2_game_option_exit_line1);
            vm_assign(g2_game_option_exit_line1);

            Line g2_game_option_exit_line2 = new Line
            {
                Name = "g2_game_option_exit_line2",
                X1 = 22,
                X2 = 8,
                Y1 = 8,
                Y2 = 22,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 2
            };
            g2_game_option_exit_grid.Children.Add(g2_game_option_exit_line2);
            vm_assign(g2_game_option_exit_line2);

            Rectangle g2_game_option_exit = new Rectangle
            {
                Name = "g2_game_option_exit",
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = 0
            };

            g2_game_option_exit.MouseEnter += rectangle_cover_enter;
            g2_game_option_exit.MouseLeave += rectangle_cover_leave;
            g2_game_option_exit.MouseLeftButtonDown += rectangle_cover_down;
            g2_game_option_exit.MouseLeftButtonUp += rectangle_cover_up;
            g2_game_option_exit.MouseMove += rectangle_cover_move;

            g2_game_option_exit_grid.Children.Add(g2_game_option_exit);
            vm_assign(g2_game_option_exit);
            #endregion
            //重开按键
            #region
            Grid g2_game_option_restart_grid = new Grid
            {
                Name = "g2_game_option_restart_grid",
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(g2_game_option_restart_grid, 5);
            g2_game_options_grid.Children.Add(g2_game_option_restart_grid);
            vm_assign(g2_game_option_restart_grid);

            Rectangle g2_game_option_restart_bg = new Rectangle
            {
                Name = "g2_game_option_restart_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(150, 150, 150)),
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_restart_grid.Children.Add(g2_game_option_restart_bg);
            vm_assign(g2_game_option_restart_bg);

            Rectangle g2_game_option_restart_circle = new Rectangle
            {
                Name = "g2_game_option_restart_circle",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 20,
                Height = 20,
                RadiusX = 10,
                RadiusY = 10,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 2
            };
            g2_game_option_restart_grid.Children.Add(g2_game_option_restart_circle);
            vm_assign(g2_game_option_restart_circle);

            Rectangle g2_game_option_restart_grey = new Rectangle
            {
                Name = "g2_game_option_restart_grey",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 10,
                Height = 6,
                Fill = getSCB(Color.FromRgb(150, 150, 150)),
                Margin = new Thickness(0, 0, 2, 0)
            };
            g2_game_option_restart_grid.Children.Add(g2_game_option_restart_grey);
            vm_assign(g2_game_option_restart_grey);

            Line g2_game_option_restart_line1 = new Line
            {
                Name = "g2_game_option_restart_line1",
                X1 = 24.5,
                X2 = 18,
                Y1 = 12,
                Y2 = 11.5,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 2
            };
            g2_game_option_restart_grid.Children.Add(g2_game_option_restart_line1);
            vm_assign(g2_game_option_restart_line1);

            Line g2_game_option_restart_line2 = new Line
            {
                Name = "g2_game_option_restart_line2",
                X1 = 24.5,
                X2 = 24,
                Y1 = 12,
                Y2 = 6,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 2
            };
            g2_game_option_restart_grid.Children.Add(g2_game_option_restart_line2);
            vm_assign(g2_game_option_restart_line2);

            Rectangle g2_game_option_restart = new Rectangle
            {
                Name = "g2_game_option_restart",
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = 0
            };

            g2_game_option_restart.MouseEnter += rectangle_cover_enter;
            g2_game_option_restart.MouseLeave += rectangle_cover_leave;
            g2_game_option_restart.MouseLeftButtonDown += rectangle_cover_down;
            g2_game_option_restart.MouseLeftButtonUp += rectangle_cover_up;
            g2_game_option_restart.MouseMove += rectangle_cover_move;

            g2_game_option_restart_grid.Children.Add(g2_game_option_restart);
            vm_assign(g2_game_option_restart);
            #endregion
            //撤销按键
            #region
            Grid g2_game_option_undo_grid = new Grid
            {
                Name = "g2_game_option_undo_grid",
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(g2_game_option_undo_grid, 4);
            g2_game_options_grid.Children.Add(g2_game_option_undo_grid);
            vm_assign(g2_game_option_undo_grid);

            Rectangle g2_game_option_undo_bg = new Rectangle
            {
                Name = "g2_game_option_undo_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(150, 150, 150)),
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_undo_grid.Children.Add(g2_game_option_undo_bg);
            vm_assign(g2_game_option_undo_bg);

            Line g2_game_option_undo_line1 = new Line
            {
                Name = "g2_game_option_undo_line1",
                X1 = 4,
                X2 = 26,
                Y1 = 20,
                Y2 = 20,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_undo_grid.Children.Add(g2_game_option_undo_line1);
            vm_assign(g2_game_option_undo_line1);

            Line g2_game_option_undo_line2 = new Line
            {
                Name = "g2_game_option_undo_line2",
                X1 = 4,
                X2 = 9,
                Y1 = 20,
                Y2 = 16,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_undo_grid.Children.Add(g2_game_option_undo_line2);
            vm_assign(g2_game_option_undo_line2);

            Line g2_game_option_undo_line3 = new Line
            {
                Name = "g2_game_option_undo_line3",
                X1 = 4,
                X2 = 9,
                Y1 = 20,
                Y2 = 24,
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_undo_grid.Children.Add(g2_game_option_undo_line3);
            vm_assign(g2_game_option_undo_line3);

            TextBlock g2_game_option_undo_text = new TextBlock
            {
                Name = "g2_game_option_undo_text",
                Text = "Undo",
                FontSize = 10,
                FontFamily = new FontFamily("Comic Sans MS"),
                FontStyle = FontStyles.Italic,
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Margin = new Thickness(3, 4, 0, 0),
                Foreground = getSCB(Color.FromRgb(0, 0, 0))
            };
            g2_game_option_undo_grid.Children.Add(g2_game_option_undo_text);
            vm_assign(g2_game_option_undo_text);


            Rectangle g2_game_option_undo = new Rectangle
            {
                Name = "g2_game_option_undo",
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            vm_set_lbtn(g2_game_option_undo);
            g2_game_option_undo_grid.Children.Add(g2_game_option_undo);
            vm_assign(g2_game_option_undo);
            #endregion
            //start-exit按键
            #region
            Grid g2_game_option_startexit_grid = new Grid
            {
                Name = "g2_game_option_startexit_grid",
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(g2_game_option_startexit_grid, 3);
            g2_game_options_grid.Children.Add(g2_game_option_startexit_grid);
            vm_assign(g2_game_option_startexit_grid);

            Rectangle g2_game_option_startexit_bg = new Rectangle
            {
                Name = "g2_game_option_startexit_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(120, 200, 120)),
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                StrokeThickness = 1.5
            };
            g2_game_option_startexit_grid.Children.Add(g2_game_option_startexit_bg);
            vm_assign(g2_game_option_startexit_bg);

            TextBlock g2_game_option_startexit_text = new TextBlock
            {
                Name = "g2_game_option_startexit_text",
                Text = "Start",
                FontSize = 10,
                FontFamily = new FontFamily("Comic Sans MS"),
                FontStyle = FontStyles.Italic,
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Margin = new Thickness(0, 8, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = getSCB(Color.FromRgb(0, 0, 0))
            };
            RotateTransform rotate = new RotateTransform(340)
            {
                CenterX = 10,
                CenterY = 10
            };
            spin(g2_game_option_startexit_text, rotate);
            g2_game_option_startexit_grid.Children.Add(g2_game_option_startexit_text);
            vm_assign(g2_game_option_startexit_text);


            Rectangle g2_game_option_startexit = new Rectangle
            {
                Name = "g2_game_option_startexit",
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = 0
            };

            g2_game_option_startexit.MouseEnter += rectangle_cover_enter;
            g2_game_option_startexit.MouseLeave += rectangle_cover_leave;
            g2_game_option_startexit.MouseLeftButtonDown += rectangle_cover_down;
            g2_game_option_startexit.MouseLeftButtonUp += rectangle_cover_up;
            g2_game_option_startexit.MouseMove += rectangle_cover_move;

            g2_game_option_startexit_grid.Children.Add(g2_game_option_startexit);
            vm_assign(g2_game_option_startexit);
            #endregion

            #region
            Grid g2_game_eoptions_grid = new Grid
            {
                Name = "g2_game_eoptions_grid",
                Width = 90,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(g2_game_eoptions_grid, 0);
            Grid.SetColumnSpan(g2_game_eoptions_grid, 3);
            g2_game_eoptions_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_eoptions_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_eoptions_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_option_container.Children.Add(g2_game_eoptions_grid);
            vm_assign(g2_game_eoptions_grid);

            //0 设置页    用于设置游戏模式，游戏目标，提醒还有其他页面需要浏览
            //1 放置页    增加或减少行列数，制作并放置基本道具
            //2 预览页    设置拥有的道具，展示游戏目标，提示以及安放一个游玩按键。
            #region
            for (int ei = 0; ei < 3; ei++)
            {
                string sei = ei.ToString();
                Grid g2_game_eoption_grid = new Grid
                {
                    Name = "g2_game_eoption_" + sei + "_grid",
                    Width = 30,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top
                };
                Grid.SetColumn(g2_game_eoption_grid, ei);
                g2_game_eoptions_grid.Children.Add(g2_game_eoption_grid);
                vm_assign(g2_game_eoption_grid);

                Rectangle g2_game_eoption_bg = new Rectangle
                {
                    Name = "g2_game_eoption_" + sei + "_bg",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Fill = getSCB(Color.FromRgb(150, 150, 150)),
                    Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                    StrokeThickness = 1.5
                };
                g2_game_eoption_grid.Children.Add(g2_game_eoption_bg);
                vm_assign(g2_game_eoption_bg);

                TextBlock g2_game_eoption_text = new TextBlock
                {
                    Name = "g2_game_eoption_" + sei + "_text"
                };
                string eot = "";
                switch (ei)
                {
                    case 0:
                        eot = "O";
                        break;
                    case 1:
                        eot = "C";
                        break;
                    case 2:
                        eot = "P";
                        break;
                }
                g2_game_eoption_text.Text = eot;
                g2_game_eoption_text.FontSize = 18;
                g2_game_eoption_text.FontFamily = new FontFamily("Comic Sans MS");
                g2_game_eoption_text.FontStyle = FontStyles.Italic;
                g2_game_eoption_text.FontWeight = FontWeight.FromOpenTypeWeight(600);
                g2_game_eoption_text.HorizontalAlignment = HorizontalAlignment.Center;
                g2_game_eoption_text.VerticalAlignment = VerticalAlignment.Center;
                g2_game_eoption_text.Foreground = getSCB(Color.FromRgb(0, 0, 0));
                g2_game_eoption_grid.Children.Add(g2_game_eoption_text);
                vm_assign(g2_game_option_undo_text);

                Rectangle g2_game_eoption = new Rectangle
                {
                    Name = "g2_game_eoption_" + sei,
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
                vm_set_lbtn(g2_game_eoption);
                g2_game_eoption_grid.Children.Add(g2_game_eoption);
                vm_assign(g2_game_eoption);
            }
            #endregion

            #endregion

            #region
            Grid g2_game_energy_grid = new Grid
            {
                Name = "g2_game_energy_grid",
                Width = app_grid.Width * 0.28,
                Height = app_grid.Height * 0.04,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, app_grid.Height * 0.12, 0, 0)
            };
            g2_game_option_container.Children.Add(g2_game_energy_grid);
            vm_assign(g2_game_energy_grid);

            Rectangle g2_game_energy_bg = new Rectangle
            {
                Name = "g2_game_energy_bg",
                Width = app_grid.Width * 0.28,
                Height = app_grid.Height * 0.04
            };
            g2_game_energy_bg.RadiusX = g2_game_energy_bg.Height / 2;
            g2_game_energy_bg.RadiusY = g2_game_energy_bg.Height / 2;
            g2_game_energy_bg.Fill = getSCB(Color.FromRgb(160, 160, 160));
            g2_game_energy_grid.Children.Add(g2_game_energy_bg);
            vm_assign(g2_game_energy_bg);

            Rectangle g2_game_energy_bg2 = new Rectangle
            {
                Name = "g2_game_energy_bg2",
                Width = app_grid.Width * 0.28,
                Height = app_grid.Height * 0.04,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                RadiusX = g2_game_energy_bg.Height / 2,
                RadiusY = g2_game_energy_bg.Height / 2,
                Fill = getSCB(Color.FromRgb(255, 255, 255))
            };
            g2_game_energy_grid.Children.Add(g2_game_energy_bg2);
            vm_assign(g2_game_energy_bg2);

            TextBlock g2_game_energy_text = new TextBlock
            {
                Name = "g2_game_energy_text",
                FontSize = 11,
                FontWeight = FontWeight.FromOpenTypeWeight(700),
                Foreground = getSCB(Color.FromRgb(0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0)
            };
            g2_game_energy_grid.Children.Add(g2_game_energy_text);
            vm_assign(g2_game_energy_text);

            Rectangle g2_game_energy_cov = new Rectangle
            {
                Name = "g2_game_energy_cov",
                Width = app_grid.Width * 0.28,
                Height = app_grid.Height * 0.04,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            g2_game_energy_cov.RadiusX = g2_game_energy_cov.Height / 2;
            g2_game_energy_cov.RadiusY = g2_game_energy_cov.Height / 2;
            g2_game_energy_cov.Fill = getSCB(Color.FromArgb(0, 0, 0, 0));
            g2_game_energy_cov.MouseLeftButtonUp += g2_shoot_key;
            g2_game_energy_grid.Children.Add(g2_game_energy_cov);
            vm_assign(g2_game_energy_cov);
            #endregion

            #region
            Grid g2_game_items_container = new Grid
            {
                Name = "g2_game_items_container",
                Width = app_grid.Width * 0.2,
                Height = app_grid.Height * 0.6,
                Margin = new Thickness(0, 0, 0, app_grid.Height * 0.12),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                ClipToBounds = true
            };
            g2_game_option_container.Children.Add(g2_game_items_container);
            vm_assign(g2_game_items_container);


            Rectangle g2_games_items_bg = new Rectangle
            {
                Name = "g2_games_items_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(0, 100, 150)),
                Stroke = getSCB(Color.FromRgb(150, 150, 200)),
                StrokeThickness = 1.5
            };
            g2_game_items_container.Children.Add(g2_games_items_bg);
            vm_assign(g2_games_items_bg);

            Grid g2_games_items_grid = new Grid
            {
                Name = "g2_games_items_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            g2_games_items_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_games_items_grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 8; i++)
            {
                g2_games_items_grid.RowDefinitions.Add(new RowDefinition());
            }
            g2_game_items_container.Children.Add(g2_games_items_grid);
            vm_assign(g2_games_items_grid);

            Grid g2_games_items_edit_grid = new Grid
            {
                Name = "g2_games_items_edit_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            g2_games_items_edit_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_games_items_edit_grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 8; i++)
            {
                g2_games_items_edit_grid.RowDefinitions.Add(new RowDefinition());
            }
            g2_game_items_container.Children.Add(g2_games_items_edit_grid);
            vm_assign(g2_games_items_edit_grid);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string si = i.ToString();
                    string sj = j.ToString();

                    Grid g2_game_item_container = new Grid
                    {
                        Name = "g2_game_item_container_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetRow(g2_game_item_container, j);
                    Grid.SetColumn(g2_game_item_container, i);
                    g2_games_items_grid.Children.Add(g2_game_item_container);
                    vm_assign(g2_game_item_container);

                    Rectangle g2_game_item_select_bg = new Rectangle
                    {
                        Name = "g2_game_item_select_" + si + "_" + sj + "_bg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(75, 100, 125)),
                        Stroke = getSCB(Color.FromRgb(150, 150, 200)),
                        StrokeThickness = 1.5
                    };
                    g2_game_item_container.Children.Add(g2_game_item_select_bg);
                    vm_assign(g2_game_item_select_bg);

                    Grid g2_game_item_grid = new Grid
                    {
                        Name = "g2_game_item_grid_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = app_grid.Height * 0.05,
                        Height = app_grid.Height * 0.05,
                        Margin = new Thickness(app_grid.Width * 0.01, 0, 0, 0)
                    };
                    g2_game_item_container.Children.Add(g2_game_item_grid);
                    vm_assign(g2_game_item_grid);


                    Rectangle g2_game_item_bg = new Rectangle
                    {
                        Name = "g2_game_item_" + si + "_" + sj + "_bg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(75, 75, 75)),
                        Stroke = getSCB(Color.FromRgb(180, 180, 180)),
                        StrokeThickness = 1
                    };
                    g2_game_item_grid.Children.Add(g2_game_item_bg);
                    vm_assign(g2_game_item_bg);

                    Grid g2_game_item_item = new Grid
                    {
                        Name = "g2_game_item_" + si + "_" + sj + "_item"
                    };
                    g2_game_item_grid.Children.Add(g2_game_item_item);
                    vm_assign(g2_game_item_item);

                    TextBlock g2_game_item_amount = new TextBlock
                    {
                        Name = "g2_game_item_amount_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(app_grid.Width * 0.045, 0, 0, 0),
                        Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                        FontSize = 16,
                        Text = "×100"
                    };
                    g2_game_item_container.Children.Add(g2_game_item_amount);
                    vm_assign(g2_game_item_amount);

                    Rectangle g2_game_item_select = new Rectangle
                    {
                        Name = "g2_game_item_select_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                        Tag = 0
                    };
                    g2_game_item_select.MouseEnter += rectangle_cover_enter;
                    g2_game_item_select.MouseLeave += rectangle_cover_leave;
                    g2_game_item_select.MouseLeftButtonDown += rectangle_cover_down;
                    g2_game_item_select.MouseLeftButtonUp += rectangle_cover_up;
                    g2_game_item_select.MouseMove += rectangle_cover_move;
                    g2_game_item_container.Children.Add(g2_game_item_select);
                    vm_assign(g2_game_item_select);

                    Rectangle g2_game_item_edit = new Rectangle
                    {
                        Name = "g2_game_item_edit_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromArgb(75, 200, 120, 120)),
                        Stroke = getSCB(Color.FromArgb(200, 50, 50, 50)),
                        StrokeThickness = 1.5
                    };
                    Grid.SetRow(g2_game_item_edit, j);
                    Grid.SetColumn(g2_game_item_edit, i);
                    vm_set_lrbtn(g2_game_item_edit);
                    g2_games_items_edit_grid.Children.Add(g2_game_item_edit);
                    vm_assign(g2_game_item_edit);
                }
            }
            #endregion

            // 关卡名
            #region
            Grid g2_game_text0_grid = new Grid
            {
                Name = "g2_game_text0_grid",
                Width = app_grid.Width * 0.28,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, app_grid.Height * 0.068, 0, 0)
            };
            g2_game_option_container.Children.Add(g2_game_text0_grid);
            vm_assign(g2_game_text0_grid);

            TextBlock g2_game_text0_text = new TextBlock
            {
                Name = "g2_game_text0_text",
                Width = app_grid.Width * 0.28,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Foreground = getSCB(Color.FromRgb(180, 180, 255)),
                Text = "关卡 1：反射",
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                //FontFamily = new FontFamily("Comic Sans MS"),
            };
            g2_game_text0_grid.Children.Add(g2_game_text0_text);
            vm_assign(g2_game_text0_text);
            #endregion

            // 介绍
            #region
            Grid g2_game_text1_grid = new Grid
            {
                Name = "g2_game_text1_grid",
                Width = app_grid.Width * 0.24,
                Height = app_grid.Height * 0.1,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, app_grid.Height * 0.01)
            };
            g2_game_option_container.Children.Add(g2_game_text1_grid);
            vm_assign(g2_game_text1_grid);

            TextBlock g2_game_text1_text = new TextBlock
            {
                Name = "g2_game_text1_text",
                Width = app_grid.Width * 0.24,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                Foreground = getSCB(Color.FromRgb(255, 255, 255))
            };
            g2_game_text1_grid.Children.Add(g2_game_text1_text);
            vm_assign(g2_game_text1_text);
            #endregion

            // 过关条件
            #region
            Grid g2_game_text2_grid = new Grid
            {
                Name = "g2_game_text2_grid",
                Width = app_grid.Width * 0.24,
                Height = app_grid.Height * 0.1,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, app_grid.Height * 0.17, 0, 0)
            };
            g2_game_text2_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_text2_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_text2_grid.RowDefinitions.Add(new RowDefinition());
            ColumnDefinition columnDefinition = new ColumnDefinition
            {
                Width = new GridLength(60, GridUnitType.Pixel)
            };
            g2_game_text2_grid.ColumnDefinitions.Add(columnDefinition);
            g2_game_text2_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_option_container.Children.Add(g2_game_text2_grid);
            vm_assign(g2_game_text2_grid);

            TextBlock g2_game_text2_text0 = new TextBlock
            {
                Name = "g2_game_text2_text0",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 11,
                Text = "过关条件：",
                Foreground = getSCB(Color.FromRgb(255, 255, 255))
            };
            Grid.SetRow(g2_game_text2_text0, 0);
            Grid.SetColumn(g2_game_text2_text0, 0);
            g2_game_text2_grid.Children.Add(g2_game_text2_text0);
            vm_assign(g2_game_text2_text0);

            for (int i = 0; i < 3; i++)
            {
                TextBlock g2_game_text2_text = new TextBlock
                {
                    Name = "g2_game_text2_text" + i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 12,
                    Foreground = getSCB(Color.FromRgb(255, 255, 255))
                };
                Grid.SetRow(g2_game_text2_text, i);
                Grid.SetColumn(g2_game_text2_text, 1);
                g2_game_text2_grid.Children.Add(g2_game_text2_text);
                vm_assign(g2_game_text2_text);
            }
            #endregion


            Grid g2_game_grid_container = new Grid
            {
                Name = "g2_game_grid_container",
                Width = app_grid.Width * 0.7,
                Height = app_grid.Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            g2_game_container.Children.Add(g2_game_grid_container);
            vm_elems.Add(g2_game_grid_container.Name, g2_game_grid_container);

            Rectangle g2_game_black_stroke = new Rectangle
            {
                Name = "g2_game_black_stroke",
                Width = g2_game_grid_container.Width,
                Height = g2_game_grid_container.Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0),
                Fill = getSCB(Color.FromRgb(0, 0, 0))
            };
            g2_game_grid_container.Children.Add(g2_game_black_stroke);
            vm_elems.Add(g2_game_black_stroke.Name, g2_game_black_stroke);

            Grid g2_game_grid = new Grid
            {
                Name = "g2_game_grid",
                Width = g2_game_grid_container.Width - 20,//550
                Height = g2_game_grid_container.Height - 20,//430
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ClipToBounds = true
            };
            g2_game_grid_container.Children.Add(g2_game_grid);
            vm_elems.Add(g2_game_grid.Name, g2_game_grid);

            Grid g2_game_object_grid = new Grid
            {
                Name = "g2_game_object_grid",
                Width = g2_game_grid.Width,
                Height = g2_game_grid.Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_grid.Children.Add(g2_game_object_grid);
            vm_elems.Add(g2_game_object_grid.Name, g2_game_object_grid);

            Grid g2_game_ball_grid = new Grid
            {
                Name = "g2_game_ball_grid",
                Width = g2_game_grid.Width,
                Height = g2_game_grid.Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_grid.Children.Add(g2_game_ball_grid);
            vm_elems.Add(g2_game_ball_grid.Name, g2_game_ball_grid);

            Grid g2_game_edit_grid = new Grid
            {
                Name = "g2_game_edit_grid",
                Width = g2_game_grid.Width,
                Height = g2_game_grid.Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_grid.Children.Add(g2_game_edit_grid);
            vm_assign(g2_game_edit_grid);



            // 1. 编辑器option
            #region
            Grid g2_game_option_option_container = new Grid
            {
                Name = "g2_game_option_option_container",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = getSCB(Color.FromRgb(150, 200, 100)),
                Height = app_grid.Height - 30
            };
            g2_game_option_container.Children.Add(g2_game_option_option_container);
            vm_assign(g2_game_option_option_container);




            #endregion

            // 2.编辑器creater
            #region
            Grid g2_game_option_create_container = new Grid
            {
                Name = "g2_game_option_create_container",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = getSCB(Color.FromRgb(180, 150, 80)),
                Height = app_grid.Height - 30
            };
            g2_game_option_container.Children.Add(g2_game_option_create_container);
            vm_assign(g2_game_option_create_container);

            // 2 * 4 大小调整格
            #region
            Grid g2_game_option_create_sizechanges_grid = new Grid
            {
                Name = "g2_game_option_create_sizechanges_grid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 200,
                Height = 120,
                Margin = new Thickness(0, 20, 0, 0)
            };
            for (int i = 0; i < 4; i++)
            {
                g2_game_option_create_sizechanges_grid.RowDefinitions.Add(new RowDefinition());
            }
            g2_game_option_create_container.Children.Add(g2_game_option_create_sizechanges_grid);
            vm_assign(g2_game_option_create_sizechanges_grid);

            Rectangle g2_game_option_create_sizechanges_bg = new Rectangle
            {
                Name = "g2_game_option_create_sizechanges_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(200, 150, 0)),
                Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                RadiusX = 20,
                RadiusY = 20,
                StrokeThickness = 2
            };
            Grid.SetRowSpan(g2_game_option_create_sizechanges_bg, 100);
            g2_game_option_create_sizechanges_grid.Children.Add(g2_game_option_create_sizechanges_bg);
            vm_assign(g2_game_option_create_sizechanges_bg);

            for (int i = 0; i < 4; i++)
            {
                string si = i.ToString();
                Grid g2_game_option_create_sizechange_grid = new Grid
                {
                    Name = "g2_game_option_create_sizechange_" + si + "_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                for (int k = 0; k < 3; k++)
                {
                    g2_game_option_create_sizechange_grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                Grid.SetRow(g2_game_option_create_sizechange_grid, i);
                g2_game_option_create_sizechanges_grid.Children.Add(g2_game_option_create_sizechange_grid);
                vm_assign(g2_game_option_create_sizechange_grid);


                TextBlock g2_game_option_create_sizechange_text = new TextBlock
                {
                    Name = "g2_game_option_create_sizechange_" + si + "_text",
                    FontSize = 14,
                    Foreground = getSCB(Color.FromRgb(0, 0, 0)),
                    FontWeight = FontWeight.FromOpenTypeWeight(600),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                switch (i)
                {
                    case 0:
                        g2_game_option_create_sizechange_text.Text = "←方向:";
                        break;
                    case 1:
                        g2_game_option_create_sizechange_text.Text = "→方向:";
                        break;
                    case 2:
                        g2_game_option_create_sizechange_text.Text = "↑ 方向:";
                        break;
                    case 3:
                        g2_game_option_create_sizechange_text.Text = "↓ 方向:";
                        break;
                }
                Grid.SetColumn(g2_game_option_create_sizechange_text, 0);
                g2_game_option_create_sizechange_grid.Children.Add(g2_game_option_create_sizechange_text);
                vm_assign(g2_game_option_create_sizechange_text);

                for (int j = 0; j < 2; j++)
                {
                    string sj = j.ToString();
                    Grid g2_game_option_create_sizechange_btn_grid = new Grid
                    {
                        Name = "g2_game_option_create_sizechange_btn_" + si + "_" + sj + "_grid",
                        Width = 56,
                        Height = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(g2_game_option_create_sizechange_btn_grid, 1 + j);
                    g2_game_option_create_sizechange_grid.Children.Add(g2_game_option_create_sizechange_btn_grid);
                    vm_assign(g2_game_option_create_sizechange_btn_grid);

                    Rectangle g2_game_option_create_sizechange_btn_bg = new Rectangle
                    {
                        Name = "g2_game_option_create_sizechange_btn_" + si + "_" + sj + "_bg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        RadiusX = 12,
                        RadiusY = 12,
                        Fill = getSCB(Color.FromRgb(250, 250, 250)),
                        Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                        StrokeThickness = 1.5
                    };
                    g2_game_option_create_sizechange_btn_grid.Children.Add(g2_game_option_create_sizechange_btn_bg);
                    vm_assign(g2_game_option_create_sizechange_btn_bg);

                    TextBlock g2_game_option_create_sizechange_btn_text = new TextBlock
                    {
                        Name = "g2_game_option_create_sizechange_btn_" + si + "_" + sj + "_text",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 11,
                        Foreground = getSCB(Color.FromRgb(0, 0, 0))
                    };
                    switch (j)
                    {
                        case 0:
                            g2_game_option_create_sizechange_btn_text.Text = "添加";
                            break;
                        case 1:
                            g2_game_option_create_sizechange_btn_text.Text = "去除";
                            break;
                    }
                    switch (i)
                    {
                        case 0:
                        case 1:
                            g2_game_option_create_sizechange_btn_text.Text += "一列";
                            break;
                        case 2:
                        case 3:
                            g2_game_option_create_sizechange_btn_text.Text += "一行";
                            break;
                    }
                    g2_game_option_create_sizechange_btn_grid.Children.Add(g2_game_option_create_sizechange_btn_text);
                    vm_assign(g2_game_option_create_sizechange_btn_text);

                    Rectangle g2_game_option_create_sizechange_btn = new Rectangle
                    {
                        Name = "g2_game_option_create_sizechange_btn_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        RadiusX = 12,
                        RadiusY = 12,
                        Fill = getSCB(Color.FromArgb(0, 0, 0, 0))
                    };
                    vm_set_lbtn(g2_game_option_create_sizechange_btn);
                    g2_game_option_create_sizechange_btn_grid.Children.Add(g2_game_option_create_sizechange_btn);
                    vm_assign(g2_game_option_create_sizechange_btn);
                }
            }
            #endregion

            // 4 * 4创造格
            #region
            Grid g2_game_option_create_creaters_grid = new Grid
            {
                Name = "g2_game_option_create_creaters_grid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Background = getSCB(Color.FromRgb(200, 130, 0)),
                Width = 200,
                Height = 200,
                Margin = new Thickness(0, 0, 0, 50)
            };
            for (int i = 0; i < 4; i++)
            {
                g2_game_option_create_creaters_grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < 4; i++)
            {
                g2_game_option_create_creaters_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            g2_game_option_create_container.Children.Add(g2_game_option_create_creaters_grid);
            vm_assign(g2_game_option_create_creaters_grid);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string si = i.ToString();
                    string sj = j.ToString();
                    Grid g2_game_option_create_creater_grid = new Grid
                    {
                        Name = "g2_game_option_create_creater_" + si + "_" + sj + "_grid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetColumn(g2_game_option_create_creater_grid, i);
                    Grid.SetRow(g2_game_option_create_creater_grid, j);
                    g2_game_option_create_creaters_grid.Children.Add(g2_game_option_create_creater_grid);
                    vm_assign(g2_game_option_create_creater_grid);

                    Rectangle g2_game_option_create_creater_bg = new Rectangle
                    {
                        Name = "g2_game_option_create_creater_" + si + "_" + sj + "_xbg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(200, 130, 0)),
                        Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                        StrokeThickness = 2
                    };
                    g2_game_option_create_creater_grid.Children.Add(g2_game_option_create_creater_bg);
                    vm_assign(g2_game_option_create_creater_bg);

                    Grid g2_game_option_create_creater_item = new Grid
                    {
                        Name = "g2_game_option_create_creater_" + si + "_" + sj + "_item",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    g2_game_option_create_creater_grid.Children.Add(g2_game_option_create_creater_item);
                    vm_assign(g2_game_option_create_creater_item);

                    Rectangle g2_game_option_create_creater_select = new Rectangle
                    {
                        Name = "g2_game_option_create_creater_" + si + "_" + sj + "_select",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Visibility = Visibility.Hidden,
                        Fill = getSCB(Color.FromArgb(120, 0, 255, 0)),
                        StrokeThickness = 1
                    };
                    g2_game_option_create_creater_grid.Children.Add(g2_game_option_create_creater_select);
                    vm_assign(g2_game_option_create_creater_select);

                    Rectangle g2_game_option_create_creater = new Rectangle
                    {
                        Name = "g2_game_option_create_creater_" + si + "_" + sj,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromArgb(100, 180, 180, 180))
                    };
                    vm_set_lrbtn(g2_game_option_create_creater);
                    g2_game_option_create_creater_grid.Children.Add(g2_game_option_create_creater);
                    vm_assign(g2_game_option_create_creater);
                }
            }
            #endregion

            Grid g2_game_option_create_size_grid = new Grid
            {
                Name = "g2_game_option_create_size_grid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 200,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10)
            };
            g2_game_option_create_container.Children.Add(g2_game_option_create_size_grid);
            vm_assign(g2_game_option_create_size_grid);

            TextBlock g2_game_option_create_size_text = new TextBlock
            {
                Name = "g2_game_option_create_size_text",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Text = "设置大小:",
                Foreground = getSCB(Color.FromRgb(0, 0, 0)),
                Margin = new Thickness(10, 0, 0, 0)
            };
            g2_game_option_create_size_grid.Children.Add(g2_game_option_create_size_text);
            vm_assign(g2_game_option_create_size_text);

            TextBox g2_game_option_create_size_textbox = new TextBox
            {
                Name = "g2_game_option_create_size_textbox",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 110,
                FontSize = 16,
                FontWeight = FontWeight.FromOpenTypeWeight(500),
                Text = "1",
                Background = getSCB(Color.FromRgb(0, 255, 0)),
                Foreground = getSCB(Color.FromRgb(0, 0, 0)),
                Margin = new Thickness(90, 0, 0, 0)
            };
            g2_game_option_create_size_grid.Children.Add(g2_game_option_create_size_textbox);
            vm_assign(g2_game_option_create_size_textbox);

            #endregion

            //数量设置器
            #region
            Grid g2_game_amount_container = new Grid
            {
                Name = "g2_game_amount_container",
                Width = app_grid.Width,
                Height = app_grid.Height,
                Visibility = Visibility.Hidden
            };
            root.Children.Add(g2_game_amount_container);
            vm_assign(g2_game_amount_container);

            Rectangle g2_game_amount_closer = new Rectangle
            {
                Name = "g2_game_amount_closer",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(50, 0, 0, 0))
            };
            g2_game_amount_closer.MouseLeftButtonUp += g2_amount_close;
            g2_game_amount_container.Children.Add(g2_game_amount_closer);
            vm_assign(g2_game_amount_closer);

            Grid g2_game_amount_grid = new Grid
            {
                Name = "g2_game_amount_grid",
                Width = app_grid.Width * 0.5,
                Height = app_grid.Height * 0.3,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_amount_container.Children.Add(g2_game_amount_grid);
            vm_assign(g2_game_amount_grid);

            Rectangle g2_game_amount_bg = new Rectangle
            {
                Name = "g2_game_amount_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                RadiusX = 25,
                RadiusY = 25,
                Fill = getSCB(Color.FromArgb(120, 50, 0, 100)),
                Stroke = getSCB(Color.FromArgb(180, 255, 255, 255)),
                StrokeThickness = 2
            };
            g2_game_amount_grid.Children.Add(g2_game_amount_bg);
            vm_assign(g2_game_amount_bg);

            TextBlock g2_game_amount_text1 = new TextBlock
            {
                Name = "g2_game_amount_text1",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 30, 0, 0),
                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                Text = "输入数量："
            };
            g2_game_amount_grid.Children.Add(g2_game_amount_text1);
            vm_assign(g2_game_amount_text1);

            inputers["g2_amount"] = new inputer();
            TextBox g2_game_amount_tb = new TextBox
            {
                Name = "g2_game_amount_tb",
                Text = "0",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(120, 30, 0, 0),
                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                Background = getSCB(Color.FromRgb(200, 0, 0)),
                Width = 150,
                FontSize = 18
            };
            g2_game_amount_grid.Children.Add(g2_game_amount_tb);
            vm_assign(g2_game_amount_tb);

            Button g2_game_amount_btn = new Button
            {
                Name = "g2_game_amount_btn",
                Content = "置为 ∞",
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(290, 30, 0, 0),
                Width = 90,
            };
            g2_game_amount_btn.Click += g2_amount_inf_btn;
            g2_game_amount_grid.Children.Add(g2_game_amount_btn);
            vm_assign(g2_game_amount_btn);

            TextBlock g2_game_amount_text2 = new TextBlock
            {
                Name = "g2_game_amount_text2",
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 70, 0, 0),
                Foreground = getSCB(Color.FromRgb(255, 255, 127)),
                Text = "输入1000或以上代表无限数量。（目前为无限）"
            };
            g2_game_amount_grid.Children.Add(g2_game_amount_text2);
            vm_assign(g2_game_amount_text2);

            TextBlock g2_game_amount_text3 = new TextBlock
            {
                Name = "g2_game_amount_text3",
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 100, 0, 0),
                Foreground = getSCB(Color.FromRgb(255, 127, 0)),
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Text = "输入不正确！若退出，数量将置为 1"
                //"输入正确，退出后将保存"
            };
            g2_game_amount_grid.Children.Add(g2_game_amount_text3);
            vm_assign(g2_game_amount_text3);
            #endregion


            //创造器
            #region
            Grid g2_game_create_container = new Grid
            {
                Name = "g2_game_create_container",
                Width = app_grid.Width,
                Height = app_grid.Height,
                Visibility = Visibility.Hidden
            };
            root.Children.Add(g2_game_create_container);
            vm_assign(g2_game_create_container);

            Rectangle g2_game_create_closer = new Rectangle
            {
                Name = "g2_game_create_closer",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(75, 0, 0, 0))
            };
            g2_game_create_closer.MouseLeftButtonUp += g2_create_close;
            g2_game_create_container.Children.Add(g2_game_create_closer);
            vm_assign(g2_game_create_closer);

            Rectangle g2_game_create_reshower = new Rectangle
            {
                Name = "g2_game_create_reshower",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                Visibility = Visibility.Hidden
            };
            g2_game_create_reshower.MouseLeftButtonUp += g2_create_reshow;
            g2_game_create_container.Children.Add(g2_game_create_reshower);
            vm_assign(g2_game_create_reshower);

            Grid g2_game_create_grid = new Grid
            {
                Name = "g2_game_create_grid",
                Width = app_grid.Width * 0.8,
                Height = app_grid.Height * 0.8,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_create_container.Children.Add(g2_game_create_grid);
            vm_assign(g2_game_create_grid);

            Rectangle g2_game_create_bg = new Rectangle
            {
                Name = "g2_game_create_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                RadiusX = 25,
                RadiusY = 25,
                Fill = getSCB(Color.FromArgb(180, 0, 0, 0)),
                Stroke = getSCB(Color.FromArgb(180, 255, 255, 255)),
                StrokeThickness = 2
            };
            g2_game_create_grid.Children.Add(g2_game_create_bg);
            vm_assign(g2_game_create_bg);

            TextBlock g2_game_create_text1 = new TextBlock
            {
                Name = "g2_game_create_text1",
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                Text = "物品创造器",
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 13, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_text1);
            vm_assign(g2_game_create_text1);

            CheckBox g2_game_create_cb = new CheckBox
            {
                Name = "g2_game_create_cb",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(13, 13, 0, 0)
            };
            g2_game_create_cb.Checked += g2_create_hide;
            scale(g2_game_create_cb, 2, 2);
            g2_game_create_grid.Children.Add(g2_game_create_cb);
            vm_assign(g2_game_create_cb);

            TextBlock g2_game_create_text2 = new TextBlock
            {
                Name = "g2_game_create_text2",
                FontSize = 12,
                Foreground = getSCB(Color.FromRgb(255, 255, 0)),
                Text = "勾选它可以隐藏创造器窗口，以便观察\n隐藏后，再次点击屏幕即可恢复",
                FontWeight = FontWeight.FromOpenTypeWeight(500),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(45, 13, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_text2);
            vm_assign(g2_game_create_text2);

            //创造器选择区
            #region
            Grid g2_game_create_objects_grid = new Grid
            {
                Name = "g2_game_create_objects_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 381,
                Height = 243,
                Margin = new Thickness(54, 87, 0, 0)
            };

            RowDefinition rd;
            rd = new RowDefinition
            {
                Height = new GridLength(3, GridUnitType.Star)
            };
            g2_game_create_objects_grid.RowDefinitions.Add(rd);
            rd = new RowDefinition
            {
                Height = new GridLength(3, GridUnitType.Pixel)
            };
            g2_game_create_objects_grid.RowDefinitions.Add(rd);
            rd = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            g2_game_create_objects_grid.RowDefinitions.Add(rd);

            ColumnDefinition cd;
            cd = new ColumnDefinition
            {
                Width = new GridLength(2, GridUnitType.Star)
            };
            g2_game_create_objects_grid.ColumnDefinitions.Add(cd);
            cd = new ColumnDefinition
            {
                Width = new GridLength(6, GridUnitType.Pixel)
            };
            g2_game_create_objects_grid.ColumnDefinitions.Add(cd);
            cd = new ColumnDefinition
            {
                Width = new GridLength(3, GridUnitType.Star)
            };
            g2_game_create_objects_grid.ColumnDefinitions.Add(cd);

            g2_game_create_grid.Children.Add(g2_game_create_objects_grid);
            vm_assign(g2_game_create_objects_grid);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int m = 2 * i;
                    int n = 2 * j;

                    int row = (int)g2_game_create_objects_grid.RowDefinitions[n].Height.Value;
                    int col = (int)g2_game_create_objects_grid.ColumnDefinitions[m].Width.Value;

                    Grid g2_game_create_object_grid = new Grid
                    {
                        Name = "g2_game_create_object_grid_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetRow(g2_game_create_object_grid, n);
                    Grid.SetColumn(g2_game_create_object_grid, m);
                    for (int k = 0; k < col; k++)
                    {
                        g2_game_create_object_grid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                    for (int k = 0; k < row; k++)
                    {
                        g2_game_create_object_grid.RowDefinitions.Add(new RowDefinition());
                    }
                    g2_game_create_objects_grid.Children.Add(g2_game_create_object_grid);
                    vm_assign(g2_game_create_object_grid);

                    for (int p = 0; p < col; p++)
                    {
                        for (int q = 0; q < row; q++)
                        {
                            Grid g2_game_creater_grid = new Grid
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_grid",
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Visibility = Visibility.Hidden
                            };
                            Grid.SetRow(g2_game_creater_grid, q);
                            Grid.SetColumn(g2_game_creater_grid, p);
                            g2_game_create_object_grid.Children.Add(g2_game_creater_grid);
                            vm_assign(g2_game_creater_grid);

                            Rectangle g2_game_creater_bg = new Rectangle
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_bg",
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Fill = getSCB(Color.FromRgb(60, 90, 120)),
                                Stroke = getSCB(Color.FromRgb(200, 200, 200)),
                                StrokeThickness = 1.5
                            };
                            g2_game_creater_grid.Children.Add(g2_game_creater_bg);
                            vm_assign(g2_game_creater_bg);

                            Grid g2_game_creater_item_grid = new Grid
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_item_grid",
                                Width = 24,
                                Height = 24,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(4, 0, 0, 0)
                            };
                            g2_game_creater_grid.Children.Add(g2_game_creater_item_grid);
                            vm_assign(g2_game_creater_item_grid);

                            Rectangle g2_game_creater_item_bg = new Rectangle
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_item_bg",
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Fill = getSCB(Color.FromRgb(75, 75, 75)),
                                Stroke = getSCB(Color.FromRgb(180, 180, 180)),
                                StrokeThickness = 0.48
                            };
                            g2_game_creater_item_grid.Children.Add(g2_game_creater_item_bg);
                            vm_assign(g2_game_creater_item_bg);

                            Grid g2_game_creater_item = new Grid
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_item"
                            };
                            g2_game_creater_item_grid.Children.Add(g2_game_creater_item);
                            vm_assign(g2_game_creater_item);

                            TextBlock g2_game_creater_text = new TextBlock
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_text",
                                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                                FontSize = 11,
                                TextWrapping = TextWrapping.Wrap,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(32, 0, 0, 0)
                            };
                            g2_game_creater_grid.Children.Add(g2_game_creater_text);
                            vm_assign(g2_game_creater_text);

                            Rectangle g2_game_creater = new Rectangle
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Fill = getSCB(Color.FromArgb(0, 0, 0, 0))
                            };
                            vm_set_lbtn(g2_game_creater);
                            g2_game_creater_grid.Children.Add(g2_game_creater);
                            vm_assign(g2_game_creater);

                            Grid g2_game_creater_mask_grid = new Grid
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_mask_grid",
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Visibility = Visibility.Hidden,
                                Background = getSCB(Color.FromRgb(150, 0, 0))
                            };
                            g2_game_creater_grid.Children.Add(g2_game_creater_mask_grid);
                            vm_assign(g2_game_creater_mask_grid);

                            TextBlock g2_game_creater_mask_text = new TextBlock
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_mask_text",
                                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                                FontSize = 15,
                                FontWeight = FontWeight.FromOpenTypeWeight(600),
                                Text = "未解锁",
                                TextWrapping = TextWrapping.Wrap,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };
                            g2_game_creater_mask_grid.Children.Add(g2_game_creater_mask_text);
                            vm_assign(g2_game_creater_mask_text);

                            Grid g2_game_creater_mask = new Grid
                            {
                                Name = "g2_game_creater_" + i + "_" + j + "_" + p + "_" + q + "_mask",
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Background = getSCB(Color.FromArgb(0, 0, 0, 0))
                            };
                            g2_game_creater_mask.MouseMove += rectangle_cover_move;
                            g2_game_creater_mask_grid.Children.Add(g2_game_creater_mask);
                            vm_assign(g2_game_creater_mask);
                        }
                    }
                }
            }
            #endregion

            //创造器三个长方形
            #region

            Rectangle g2_game_create_object_base_stroke = new Rectangle
            {
                Name = "g2_game_create_object_base_stroke",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 156,
                Height = 280,
                Stroke = getSCB(Color.FromRgb(255, 255, 0)),
                StrokeThickness = 3,
                RadiusX = 3,
                RadiusY = 3,
                Margin = new Thickness(51, 56, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_object_base_stroke);
            vm_assign(g2_game_create_object_base_stroke);

            TextBlock g2_game_create_object_base_text = new TextBlock
            {
                Name = "g2_game_create_object_base_text",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Text = "独立",
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(255, 255, 0)),
                Margin = new Thickness(111, 62, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_object_base_text);
            vm_assign(g2_game_create_object_base_text);

            Rectangle g2_game_create_object_combine_stroke = new Rectangle
            {
                Name = "g2_game_create_object_combine_stroke",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 231,
                Height = 280,
                Stroke = getSCB(Color.FromRgb(0, 255, 255)),
                StrokeThickness = 3,
                RadiusX = 3,
                RadiusY = 3,
                Margin = new Thickness(207, 56, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_object_combine_stroke);
            vm_assign(g2_game_create_object_combine_stroke);

            TextBlock g2_game_create_object_combine_text = new TextBlock
            {
                Name = "g2_game_create_object_combine_text",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Text = "可组合",
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(0, 255, 255)),
                Margin = new Thickness(297, 62, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_object_combine_text);
            vm_assign(g2_game_create_object_combine_text);


            Rectangle g2_game_create_object_item_stroke = new Rectangle
            {
                Name = "g2_game_create_object_item_stroke",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 421,
                Height = 156,
                Stroke = getSCB(Color.FromRgb(255, 0, 255)),
                StrokeThickness = 3,
                RadiusX = 3,
                RadiusY = 3,
                Margin = new Thickness(20, 177, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_object_item_stroke);
            vm_assign(g2_game_create_object_item_stroke);

            TextBlock g2_game_create_object_item_text = new TextBlock
            {
                Name = "g2_game_create_object_item_text",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Text = "道\n具",
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(255, 0, 255)),
                Margin = new Thickness(29, 228, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_object_item_text);
            vm_assign(g2_game_create_object_item_text);
            #endregion

            //创造器预览
            #region
            TextBlock g2_game_create_preview_text = new TextBlock
            {
                Name = "g2_game_create_preview_text",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Text = "预览：",
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                Margin = new Thickness(465, 31, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_preview_text);
            vm_assign(g2_game_create_preview_text);

            Grid g2_game_create_preview_grid = new Grid
            {
                Name = "g2_game_create_preview_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 150,
                Height = 150,
                Margin = new Thickness(465, 56, 0, 0)
            };
            g2_game_create_grid.Children.Add(g2_game_create_preview_grid);
            vm_assign(g2_game_create_preview_grid);

            Rectangle g2_game_create_preview_bg = new Rectangle
            {
                Name = "g2_game_create_preview_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stroke = getSCB(Color.FromRgb(180, 180, 180)),
                StrokeThickness = 3
            };
            g2_game_create_preview_grid.Children.Add(g2_game_create_preview_bg);
            vm_assign(g2_game_create_preview_bg);

            Grid g2_game_create_preview_hint_grid = new Grid
            {
                Name = "g2_game_create_preview_hint_grid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_create_preview_hint_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_preview_hint_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_preview_grid.Children.Add(g2_game_create_preview_hint_grid);
            vm_assign(g2_game_create_preview_hint_grid);

            for (int i = 0; i < 2; i++)
            {
                TextBlock g2_game_create_preview_hint_text = new TextBlock
                {
                    Name = "g2_game_create_preview_hint_text_" + i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = "请在左边选择"
                };
                if (i == 1)
                {
                    g2_game_create_preview_hint_text.Text = "至少一项";
                }
                g2_game_create_preview_hint_text.FontSize = 18;
                g2_game_create_preview_hint_text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                Grid.SetRow(g2_game_create_preview_hint_text, i);
                g2_game_create_preview_hint_grid.Children.Add(g2_game_create_preview_hint_text);
                vm_assign(g2_game_create_preview_hint_text);
            }

            Rectangle g2_game_create_preview_item_bg = new Rectangle
            {
                Name = "g2_game_create_preview_item_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(75, 75, 75)),
                Stroke = getSCB(Color.FromRgb(180, 180, 180)),
                StrokeThickness = 3,
                Visibility = Visibility.Hidden
            };
            g2_game_create_preview_grid.Children.Add(g2_game_create_preview_item_bg);
            vm_assign(g2_game_create_preview_item_bg);

            Grid g2_game_create_preview_item = new Grid
            {
                Name = "g2_game_create_preview_item",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden
            };
            g2_game_create_preview_grid.Children.Add(g2_game_create_preview_item);
            vm_assign(g2_game_create_preview_item);
            #endregion

            //创造器的四个按钮
            #region
            Grid g2_game_create_btns_grid = new Grid
            {
                Name = "g2_game_create_btns_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 150,
                Height = 120
            };
            g2_game_create_btns_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_btns_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_btns_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_btns_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_btns_grid.Margin = new Thickness(465, 216, 0, 0);
            g2_game_create_grid.Children.Add(g2_game_create_btns_grid);
            vm_assign(g2_game_create_btns_grid);

            for (int i = 0; i < 4; i++)
            {
                Grid g2_game_create_btn_grid = new Grid
                {
                    Name = "g2_game_create_btn_" + i + "_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                Grid.SetRow(g2_game_create_btn_grid, i);
                g2_game_create_btns_grid.Children.Add(g2_game_create_btn_grid);
                vm_assign(g2_game_create_btn_grid);

                Rectangle g2_game_create_btn_bg = new Rectangle
                {
                    Name = "g2_game_create_btn_" + i + "_bg",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 120,
                    Height = 24,
                    Fill = getSCB(Color.FromRgb(255, 255, 255)),
                    Stroke = getSCB(Color.FromRgb(0, 0, 0)),
                    StrokeThickness = 1,
                    RadiusX = 12,
                    RadiusY = 12
                };
                g2_game_create_btn_grid.Children.Add(g2_game_create_btn_bg);
                vm_assign(g2_game_create_btn_bg);

                TextBlock g2_game_create_btn_text = new TextBlock
                {
                    Name = "g2_game_create_btn_" + i + "_text",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    Text = "123456789",
                    FontWeight = FontWeight.FromOpenTypeWeight(550),
                    Foreground = getSCB(Color.FromRgb(0, 0, 0))
                };
                g2_game_create_btn_grid.Children.Add(g2_game_create_btn_text);
                vm_assign(g2_game_create_btn_text);

                switch (i)
                {
                    case 0:
                        g2_game_create_btn_bg.Fill = getSCB(Color.FromRgb(0, 255, 0));
                        g2_game_create_btn_text.Text = "确认";
                        break;
                    case 1:
                        g2_game_create_btn_bg.Fill = getSCB(Color.FromRgb(127, 0, 255));
                        g2_game_create_btn_text.Foreground = getSCB(Color.FromRgb(255, 255, 255));
                        g2_game_create_btn_text.Text = "重设";
                        break;
                    case 2:
                        g2_game_create_btn_bg.Fill = getSCB(Color.FromRgb(0, 255, 255));
                        g2_game_create_btn_text.Text = "属性设计器";
                        break;
                    case 3:
                        g2_game_create_btn_bg.Fill = getSCB(Color.FromRgb(255, 255, 255));
                        g2_game_create_btn_text.Text = "退出 & 删除";
                        break;
                }


                Rectangle g2_game_create_btn = new Rectangle
                {
                    Name = "g2_game_create_btn_" + i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 120,
                    Height = 24,
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                    RadiusX = 12,
                    RadiusY = 12
                };
                vm_set_lbtn(g2_game_create_btn);
                g2_game_create_btn_grid.Children.Add(g2_game_create_btn);
                vm_assign(g2_game_create_btn);
            }
            #endregion

            //创造器属性设计器
            #region
            Grid g2_game_create_attrs_grid = new Grid
            {
                Name = "g2_game_create_attrs_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(15, 50, 0, 0),
                Width = 430,
                Height = 300,
                Visibility = Visibility.Hidden
            };
            g2_game_create_grid.Children.Add(g2_game_create_attrs_grid);
            vm_assign(g2_game_create_attrs_grid);

            Rectangle g2_game_create_attrs_bg = new Rectangle
            {
                Name = "g2_game_create_attrs_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(0, 50, 50)),
                RadiusX = 15,
                RadiusY = 15,
                Stroke = getSCB(Color.FromRgb(150, 150, 150)),
                StrokeThickness = 1.5
            };
            g2_game_create_attrs_grid.Children.Add(g2_game_create_attrs_bg);
            vm_assign(g2_game_create_attrs_bg);

            TextBlock g2_game_create_attrs_text1 = new TextBlock
            {
                Name = "g2_game_create_attrs_text1",
                FontSize = 16,
                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                Text = "属性设计器",
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 13, 0, 0)
            };
            g2_game_create_attrs_grid.Children.Add(g2_game_create_attrs_text1);
            vm_assign(g2_game_create_attrs_text1);

            TextBlock g2_game_create_attrs_show_text = new TextBlock
            {
                Name = "g2_game_create_attrs_show_text",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 18,
                Text = "请先在创造器中选择类型",
                Foreground = getSCB(Color.FromRgb(255, 127, 0)),
                Margin = new Thickness(0, 80, 0, 0)
            };
            g2_game_create_attrs_grid.Children.Add(g2_game_create_attrs_show_text);
            vm_assign(g2_game_create_attrs_show_text);

            #region
            Grid g2_game_create_attrs_show_grid = new Grid
            {
                Name = "g2_game_create_attrs_show_grid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 375,
                Height = 90,
                Margin = new Thickness(0, 50, 0, 0)
            };
            for (int i = 0; i < 5; i++)
            {
                g2_game_create_attrs_show_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 3; i++)
            {
                g2_game_create_attrs_show_grid.RowDefinitions.Add(new RowDefinition());
            }
            g2_game_create_attrs_grid.Children.Add(g2_game_create_attrs_show_grid);
            vm_assign(g2_game_create_attrs_show_grid);

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Grid g2_game_create_attr_show_grid = new Grid
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j + "_grid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetColumn(g2_game_create_attr_show_grid, i);
                    Grid.SetRow(g2_game_create_attr_show_grid, j);
                    g2_game_create_attrs_show_grid.Children.Add(g2_game_create_attr_show_grid);
                    vm_assign(g2_game_create_attr_show_grid);

                    Rectangle g2_game_create_attr_show_bg = new Rectangle
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j + "_bg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(60, 90, 120)),
                        Stroke = getSCB(Color.FromRgb(200, 200, 200)),
                        StrokeThickness = 1.5
                    };
                    g2_game_create_attr_show_grid.Children.Add(g2_game_create_attr_show_bg);
                    vm_assign(g2_game_create_attr_show_bg);

                    Grid g2_game_create_attr_show_item_grid = new Grid
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j + "_item_grid",
                        Width = 24,
                        Height = 24,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(4, 0, 0, 0)
                    };
                    g2_game_create_attr_show_grid.Children.Add(g2_game_create_attr_show_item_grid);
                    vm_assign(g2_game_create_attr_show_item_grid);

                    Rectangle g2_game_create_attr_show_item_bg = new Rectangle
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j + "_item_bg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(75, 75, 75)),
                        Stroke = getSCB(Color.FromRgb(180, 180, 180)),
                        StrokeThickness = 0.48
                    };
                    g2_game_create_attr_show_item_grid.Children.Add(g2_game_create_attr_show_item_bg);
                    vm_assign(g2_game_create_attr_show_item_bg);

                    Grid g2_game_create_attr_show_item = new Grid
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j + "_item"
                    };
                    g2_game_create_attr_show_item_grid.Children.Add(g2_game_create_attr_show_item);
                    vm_assign(g2_game_create_attr_show_item);

                    TextBlock g2_game_create_attr_show_text = new TextBlock
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j + "_text",
                        Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                        FontSize = 11,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = "11",
                        Margin = new Thickness(32, 0, 0, 0)
                    };
                    g2_game_create_attr_show_grid.Children.Add(g2_game_create_attr_show_text);
                    vm_assign(g2_game_create_attr_show_text);

                    Rectangle g2_game_create_attr_show = new Rectangle
                    {
                        Name = "g2_game_create_attr_show_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromArgb(0, 0, 0, 0))
                    };
                    vm_set_lbtn(g2_game_create_attr_show);
                    g2_game_create_attr_show_grid.Children.Add(g2_game_create_attr_show);
                    vm_assign(g2_game_create_attr_show);

                }
            }
            #endregion

            //属性设计器主体
            #region
            Grid g2_game_create_attrs_design_main_grid = new Grid
            {
                Name = "g2_game_create_attrs_design_main_grid",
                Width = 400,
                Height = 140,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 10)
            };
            g2_game_create_attrs_grid.Children.Add(g2_game_create_attrs_design_main_grid);
            vm_assign(g2_game_create_attrs_design_main_grid);

            Rectangle g2_game_create_attrs_design_bg = new Rectangle
            {
                Name = "g2_game_create_attrs_design_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(0, 100, 100)),
                Stroke = getSCB(Color.FromRgb(255, 255, 255)),
                RadiusX = 10,
                RadiusY = 10
            };
            g2_game_create_attrs_design_main_grid.Children.Add(g2_game_create_attrs_design_bg);
            vm_assign(g2_game_create_attrs_design_bg);

            Grid g2_game_create_attrs_design_grid = new Grid
            {
                Name = "g2_game_create_attrs_design_grid",
                Width = 380,
                Height = 120,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            g2_game_create_attrs_design_main_grid.Children.Add(g2_game_create_attrs_design_grid);
            vm_assign(g2_game_create_attrs_design_grid);

            //起点
            #region
            Grid g2_game_create_attr_start_grid = new Grid
            {
                Name = "g2_game_create_attr_start_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            g2_game_create_attr_start_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_start_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_start_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_start_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_start_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_start_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_start_grid.Tag = "起点";
            g2_game_create_attr_start_grid.Visibility = Visibility.Hidden;
            g2_game_create_attrs_design_grid.Children.Add(g2_game_create_attr_start_grid);
            vm_assign(g2_game_create_attr_start_grid);

            TextBlock g2_game_create_attr_start_text = new TextBlock
            {
                Name = "g2_game_create_attr_start_text",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = getSCB(Color.FromRgb(255, 255, 180)),
                FontSize = 15,
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Text = "选择方向:"
            };
            g2_game_create_attr_start_grid.Children.Add(g2_game_create_attr_start_text);
            vm_assign(g2_game_create_attr_start_text);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Grid g2_game_create_attr_start_ngrid = new Grid
                    {
                        Name = "g2_game_create_attr_start_ngrid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetColumn(g2_game_create_attr_start_ngrid, i);
                    Grid.SetRow(g2_game_create_attr_start_ngrid, j);
                    g2_game_create_attr_start_grid.Children.Add(g2_game_create_attr_start_ngrid);
                    vm_assign(g2_game_create_attr_start_ngrid);

                    RadioButton g2_game_create_attr_start_radio = new RadioButton
                    {
                        Name = "g2_game_create_attr_start_radio_" + i + "_" + j,
                        Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    string con = "";
                    switch (i + 4 * j)
                    {
                        case 0:
                            con = "←";
                            break;
                        case 1:
                            con = "→";
                            break;
                        case 2:
                            con = "↑";
                            break;
                        case 3:
                            con = "↓";
                            break;
                        case 4:
                            con = "↖";
                            break;
                        case 5:
                            con = "↗";
                            break;
                        case 6:
                            con = "↙";
                            break;
                        case 7:
                            con = "↘";
                            break;
                    }
                    g2_game_create_attr_start_radio.Content = con;
                    g2_game_create_attr_start_radio.GroupName = "g2_start";
                    g2_game_create_attr_start_radio.Margin = new Thickness(14, 0, 0, 0);
                    g2_game_create_attr_start_radio.Checked += game_button_key;
                    //g2_game_create_attr_start_radio.Unchecked += game_button_key;
                    scale(g2_game_create_attr_start_radio, 2.25, 2.25);
                    g2_game_create_attr_start_ngrid.Children.Add(g2_game_create_attr_start_radio);
                    vm_assign(g2_game_create_attr_start_radio);
                }
            }
            #endregion

            //反射板
            #region
            Grid g2_game_create_attr_reflecter_grid = new Grid
            {
                Name = "g2_game_create_attr_reflecter_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            g2_game_create_attr_reflecter_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_reflecter_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_reflecter_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_reflecter_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_reflecter_grid.Tag = "反射板";
            g2_game_create_attr_reflecter_grid.Visibility = Visibility.Hidden;
            g2_game_create_attrs_design_grid.Children.Add(g2_game_create_attr_reflecter_grid);
            vm_assign(g2_game_create_attr_reflecter_grid);

            TextBlock g2_game_create_attr_reflecter_text = new TextBlock
            {
                Name = "g2_game_create_attr_reflecter_text",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = getSCB(Color.FromRgb(255, 255, 180)),
                FontSize = 15,
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Text = "选择方向:"
            };
            g2_game_create_attr_reflecter_grid.Children.Add(g2_game_create_attr_reflecter_text);
            vm_assign(g2_game_create_attr_reflecter_text);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Grid g2_game_create_attr_reflecter_ngrid = new Grid
                    {
                        Name = "g2_game_create_attr_reflecter_ngrid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetColumn(g2_game_create_attr_reflecter_ngrid, i);
                    Grid.SetRow(g2_game_create_attr_reflecter_ngrid, j);
                    g2_game_create_attr_reflecter_grid.Children.Add(g2_game_create_attr_reflecter_ngrid);
                    vm_assign(g2_game_create_attr_reflecter_ngrid);

                    RadioButton g2_game_create_attr_reflecter_radio = new RadioButton
                    {
                        Name = "g2_game_create_attr_reflecter_radio_" + i + "_" + j,
                        Foreground = getSCB(Color.FromRgb(255, 255, 255)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    string con = "";
                    switch (i + 2 * j)
                    {
                        case 0:
                            con = "←→";
                            break;
                        case 1:
                            con = "↑  ↓";
                            break;
                        case 2:
                            con = "↙↗";
                            break;
                        case 3:
                            con = "↖↘";
                            break;
                    }
                    g2_game_create_attr_reflecter_radio.Content = con;
                    g2_game_create_attr_reflecter_radio.GroupName = "g2_reflecter";
                    g2_game_create_attr_reflecter_radio.Margin = new Thickness(14, 0, 0, 0);
                    g2_game_create_attr_reflecter_radio.Checked += game_button_key;
                    //g2_game_create_attr_reflecter_radio.Unchecked += game_button_key;
                    scale(g2_game_create_attr_reflecter_radio, 2.25, 2.25);
                    g2_game_create_attr_reflecter_ngrid.Children.Add(g2_game_create_attr_reflecter_radio);
                    vm_assign(g2_game_create_attr_reflecter_radio);
                }
            }
            #endregion

            //属性：转换器
            #region
            Grid g2_game_create_attr_inout_grid = new Grid
            {
                Name = "g2_game_create_attr_inout_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            g2_game_create_attr_inout_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_inout_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_inout_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_inout_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_inout_grid.RowDefinitions.Add(new RowDefinition());
            g2_game_create_attr_inout_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_inout_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_inout_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_inout_grid.ColumnDefinitions.Add(new ColumnDefinition());
            g2_game_create_attr_inout_grid.Tag = "转换器";
            g2_game_create_attr_inout_grid.Visibility = Visibility.Hidden;
            g2_game_create_attrs_design_grid.Children.Add(g2_game_create_attr_inout_grid);
            vm_assign(g2_game_create_attr_inout_grid);

            TextBlock g2_game_create_attr_inout_text = new TextBlock
            {
                Name = "g2_game_create_attr_inout_text",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = getSCB(Color.FromRgb(255, 255, 180)),
                FontSize = 15,
                FontWeight = FontWeight.FromOpenTypeWeight(600),
                Text = "选择方向:（绿色为入口，红色为出口）"
            };
            Grid.SetColumnSpan(g2_game_create_attr_inout_text, 100);
            g2_game_create_attr_inout_grid.Children.Add(g2_game_create_attr_inout_text);
            vm_assign(g2_game_create_attr_inout_text);

            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    int j = k + 1;
                    Grid g2_game_create_attr_inout_ngrid = new Grid
                    {
                        Name = "g2_game_create_attr_inout_ngrid",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Grid.SetColumn(g2_game_create_attr_inout_ngrid, i);
                    Grid.SetRow(g2_game_create_attr_inout_ngrid, j);
                    g2_game_create_attr_inout_grid.Children.Add(g2_game_create_attr_inout_ngrid);
                    vm_assign(g2_game_create_attr_inout_ngrid);

                    CheckBox g2_game_create_attr_inout_cb = new CheckBox
                    {
                        Name = "g2_game_create_attr_inout_cb_" + i + "_" + k,
                        Foreground = getSCB(Color.FromRgb(0, 255, 0))
                    };
                    if (k >= 2)
                    {
                        g2_game_create_attr_inout_cb.Foreground = getSCB(Color.FromRgb(255, 0, 0));
                    }
                    g2_game_create_attr_inout_cb.HorizontalAlignment = HorizontalAlignment.Left;
                    g2_game_create_attr_inout_cb.VerticalAlignment = VerticalAlignment.Center;
                    string con = "";
                    switch ((i + 4 * k) % 8)
                    {
                        case 0:
                            con = "←";
                            break;
                        case 1:
                            con = "→";
                            break;
                        case 2:
                            con = "↑";
                            break;
                        case 3:
                            con = "↓";
                            break;
                        case 4:
                            con = "↖";
                            break;
                        case 5:
                            con = "↗";
                            break;
                        case 6:
                            con = "↙";
                            break;
                        case 7:
                            con = "↘";
                            break;
                    }
                    g2_game_create_attr_inout_cb.Content = con;
                    g2_game_create_attr_inout_cb.Margin = new Thickness(20, 0, 0, 0);
                    scale(g2_game_create_attr_inout_cb, 1.8, 1.8);
                    g2_game_create_attr_inout_cb.Checked += game_button_key;
                    g2_game_create_attr_inout_cb.Unchecked += game_button_key;
                    g2_game_create_attr_inout_ngrid.Children.Add(g2_game_create_attr_inout_cb);
                    vm_assign(g2_game_create_attr_inout_cb);
                }
            }
            #endregion

            //属性init TODO

            #endregion

            #endregion

            #endregion

            g2_edit_init();
            g2_level_init();
        }
    }
}
