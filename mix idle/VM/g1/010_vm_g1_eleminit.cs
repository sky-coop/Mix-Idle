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
        public Point g1_view_point_1 = new Point(0, 0);
        public Point g1_view_point_2 = new Point(0, 0);
        double g1_mw = 4;
        double g1_mh = 4;
        public void g1_elem_init()
        {
            //debug
            g1_levels_init();//TO DELETE

            g1_mw = 2;
            g1_mh = 7;

            //通用代码段
            Grid app_grid = vm_get_app_grid();
            Grid root = new Grid
            {
                Name = "世界树",
                Visibility = Visibility.Hidden,
                Width = app_grid.Width,
                Height = app_grid.Height
            };
            app_grid.Children.Add(root);
            vm_assign(root);

            double w = app_grid.Width;
            double h = app_grid.Height;

            Grid main = new Grid
            {
                Name = "vm_g1_main_grid",
                Width = app_grid.Width,
                Height = app_grid.Height,
                Background = getSCB(Color.FromRgb(50, 50, 50)),
                ClipToBounds = true,
            };
            root.Children.Add(main);
            vm_assign(main);

            Grid map = new Grid
            #region
            {
                Name = "vm_g1_map_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = app_grid.Width * g1_mw,
                Height = app_grid.Height * g1_mh,
                Background = getSCB(Color.FromRgb(50, 50, 50)),
            };
            main.Children.Add(map);
            vm_assign(map);

            /*
            for(int i = 0; i < 20; i++)
            {
                double px = map.Width * i / 20;
                double py = map.Height * i / 20;
                TextBlock test = new TextBlock
                {
                    Name = "vm_g1_map_text_" + i,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(px, py, 0, 0),
                    FontSize = 20,
                    Text = i + "",
                    Foreground = getSCB(Color.FromRgb(0, 255, 255)),
                };
                map.Children.Add(test);
                vm_assign(test);
            }*/

            Grid scene = new Grid
            {
                Name = "vm_g1_scene_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            map.Children.Add(scene);
            vm_assign(scene);

            #endregion

            Grid ctrl = new Grid
            #region
            {
                Name = "vm_g1_ctrl_grid",
                Width = app_grid.Width,
                Height = app_grid.Height,
            };
            main.Children.Add(ctrl);
            vm_assign(ctrl);

            Grid radar = new Grid
            #region
            {
                Name = "vm_g1_radar_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = w * 0.25,
                Height = h * 0.25,
                Margin = new Thickness(h * 0.02, 0, 0, h * 0.02),
            };
            ctrl.Children.Add(radar);
            vm_assign(radar);

            Rectangle radar_bg = new Rectangle
            {
                Name = "vm_g1_radar_bg",
                Width = radar.Width,
                Height = radar.Height,
                Fill = getSCB(Color.FromArgb(150, 127, 127, 127)),
                Stroke = getSCB(Color.FromArgb(150, 150, 150, 150)),
                StrokeThickness = 1.5,
            };
            radar.Children.Add(radar_bg);
            vm_assign(radar_bg);

            Grid radar_scenery = new Grid
            {
                Name = "vm_g1_radar_scenery",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            radar.Children.Add(radar_scenery);
            vm_assign(radar_scenery);

            Rectangle radar_view = new Rectangle
            {
                Name = "vm_g1_radar_view",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = radar.Width / g1_mw,
                Height = radar.Height / g1_mh,
                Fill = getSCB(Color.FromArgb(100, 255, 255, 255)),
                Stroke = getSCB(Color.FromArgb(100, 255, 255, 150)),
                StrokeThickness = 1,
            };
            g1_view_point_2 = new Point(radar_view.Width, radar_view.Height);
            radar.Children.Add(radar_view);
            vm_assign(radar_view);

            Grid radar_main = new Grid
            {
                Name = "vm_g1_radar_main",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            radar.Children.Add(radar_main);
            vm_assign(radar_main);
            #endregion

            Grid center = new Grid
            {
                Name = "vm_g1_center_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = app_grid.Width,
                Height = app_grid.Height,
            };
            ctrl.Children.Add(center);
            vm_assign(center);

            Grid center_ret_grid = new Grid
            #region
            {
                Name = "vm_g1_center_ret_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 50,
                Height = 30,
                Margin = new Thickness(2.5, 2.5, 0, 0),
            };
            center.Children.Add(center_ret_grid);
            vm_assign(center_ret_grid);

            Rectangle center_ret_bg = new Rectangle
            {
                Name = "vm_g1_center_ret_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(255, 181, 197)),
                Stroke = getSCB(Color.FromRgb(139, 58, 98)),
                StrokeThickness = 2,
            };
            center_ret_grid.Children.Add(center_ret_bg);
            vm_assign(center_ret_bg);

            Line center_ret_l1 = new Line
            {
                Name = "vm_g1_center_ret_l1",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stroke = getSCB(Color.FromRgb(25, 25, 25)),
                StrokeThickness = 2,
                X1 = 12,
                Y1 = 15,
                X2 = 38,
                Y2 = 15,
            };
            center_ret_grid.Children.Add(center_ret_l1);
            vm_assign(center_ret_l1);

            Line center_ret_l2 = new Line
            {
                Name = "vm_g1_center_ret_l2",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stroke = getSCB(Color.FromRgb(25, 25, 25)),
                StrokeThickness = 2,
                X1 = 12,
                Y1 = 15,
                X2 = 22,
                Y2 = 9,
            };
            center_ret_grid.Children.Add(center_ret_l2);
            vm_assign(center_ret_l2);

            Line center_ret_l3 = new Line
            {
                Name = "vm_g1_center_ret_l3",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stroke = getSCB(Color.FromRgb(25, 25, 25)),
                StrokeThickness = 2,
                X1 = 12,
                Y1 = 15,
                X2 = 22,
                Y2 = 21,
            };
            center_ret_grid.Children.Add(center_ret_l3);
            vm_assign(center_ret_l3);

            Rectangle center_ret = new Rectangle
            {
                Name = "vm_g1_center_ret",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
            };
            vm_set_lbtn(center_ret);
            center_ret_grid.Children.Add(center_ret);
            vm_assign(center_ret);
            #endregion

            StackPanel top = new StackPanel
            #region
            {
                Name = "vm_g1_top_panel",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, h * 0.02, 0, 0),
            };
            center.Children.Add(top);
            vm_assign(top);
            #endregion

            Grid right = new Grid
            #region
            {
                Name = "vm_g1_right_grid",
                Width = app_grid.Width * 0.5,
                Height = app_grid.Height,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = getSCB(Color.FromArgb(150, 75, 75, 75)),
                Visibility = Visibility.Hidden,
            };
            ctrl.Children.Add(right);
            vm_assign(right);

            Line bound = new Line
            {
                Name = "vm_g1_bound",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                X1 = 0,
                X2 = 0,
                Y1 = 0,
                Y2 = app_grid.Height,
                Stroke = getSCB(Color.FromRgb(255, 235, 205)),
                StrokeThickness = 5,
            };
            right.Children.Add(bound);
            vm_assign(bound);

            Grid ret_grid = new Grid
            #region
            {
                Name = "vm_g1_right_ret_grid",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(0, 2.5, 0, 0),
            };
            right.Children.Add(ret_grid);
            vm_assign(ret_grid);

            Rectangle ret_bg = new Rectangle
            {
                Name = "vm_g1_right_ret_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromRgb(0, 201, 87)),
                Stroke = getSCB(Color.FromRgb(127, 255, 212)),
                StrokeThickness = 2,
            };
            ret_grid.Children.Add(ret_bg);
            vm_assign(ret_bg);

            Line ret_l1 = new Line
            {
                Name = "vm_g1_right_ret_l1",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stroke = getSCB(Color.FromRgb(25, 25, 25)),
                StrokeThickness = 2,
                X1 = 8,
                Y1 = 8,
                X2 = 22,
                Y2 = 22,
            };
            ret_grid.Children.Add(ret_l1);
            vm_assign(ret_l1);

            Line ret_l2 = new Line
            {
                Name = "vm_g1_right_ret_l2",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stroke = getSCB(Color.FromRgb(25, 25, 25)),
                StrokeThickness = 2,
                X1 = 22,
                Y1 = 8,
                X2 = 8,
                Y2 = 22,
            };
            ret_grid.Children.Add(ret_l2);
            vm_assign(ret_l2);

            Rectangle ret = new Rectangle
            {
                Name = "vm_g1_right_ret",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
            };
            vm_set_lbtn(ret);
            ret_grid.Children.Add(ret);
            vm_assign(ret);
            #endregion

            Grid layer_grid = new Grid
            {
                Name = "vm_g1_layer_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            right.Children.Add(layer_grid);
            vm_assign(layer_grid);

            Grid page_grid = new Grid
            #region
            {
                Name = "vm_g1_page_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 30,
                Margin = new Thickness(0, 0, 0, 50),
            };
            right.Children.Add(page_grid);
            vm_assign(page_grid);

            TextBlock page_text = new TextBlock
            {
                Name = "vm_g1_page_text",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "第 1 / 1 页",
                FontSize = 18,
                Foreground = getSCB(Color.FromRgb(0, 255, 255)),
            };
            page_grid.Children.Add(page_text);
            vm_assign(page_text);

            Button page_back = new Button
            {
                Name = "vm_g1_page_back",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 30,
                Height = 20,
                Margin = new Thickness(0, 0, 180, 0),
                Content = "←",
            };
            scale(page_back, 1.5, 1.5, 0.5, 0.5);
            page_back.Click += button_click;
            page_grid.Children.Add(page_back);
            vm_assign(page_back);

            Button page_forward = new Button
            {
                Name = "vm_g1_page_forward",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 30,
                Height = 20,
                Margin = new Thickness(180, 0, 0, 0),
                Content = "→",
            };
            scale(page_forward, 1.5, 1.5, 0.5, 0.5);
            page_forward.Click += button_click;
            page_grid.Children.Add(page_forward);
            vm_assign(page_forward);
            #endregion

            #endregion // grid right

            #endregion // grid ctrl


            Grid information = new Grid
            #region
            {
                Name = "vm_g1_information_grid",
                Width = app_grid.Width,
                Height = app_grid.Height,
            };
            main.Children.Add(information);
            vm_assign(information);

            Grid window_container = new Grid
            {
                Name = "vm_g1_window_container",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            main.Children.Add(window_container);
            vm_assign(window_container);

            Rectangle window_bg = new Rectangle
            {
                Name = "vm_g1_window_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                RadiusX = 15,
                RadiusY = 15,
                Fill = getSCB(Color.FromArgb(175, 50, 50, 50))
            };
            window_container.Children.Add(window_bg);
            vm_assign(window_bg);

            StackPanel window = new StackPanel
            {
                Name = "vm_g1_window",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            window_container.Children.Add(window);
            vm_assign(window);

            for (int i = 0; i <= 10; i++)
            {
                TextBlock window_text = new TextBlock
                {
                    Name = "vm_g1_window_t" + i,
                    Visibility = Visibility.Collapsed,
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Padding = new Thickness(10, 3, 10, 3),
                };
                window.Children.Add(window_text);
                vm_assign(window_text);
                if (i == 0)
                {
                    window_text.FontSize = 18;
                    //window_text.FontWeight = FontWeight.FromOpenTypeWeight(600);
                    window_text.Visibility = Visibility.Visible;
                    window_text.Foreground = getSCB(Color.FromRgb(0, 255, 255));
                    window_text.FontFamily = new FontFamily("SimHei");
                }
            }
            #endregion // grid information

            g1_level_draw("世界");
        }

        public void g1_view_syn()
        {
            Grid radar = (Grid)vm_elems["vm_g1_radar_grid"];
            Rectangle view = (Rectangle)vm_elems["vm_g1_radar_view"];
            Grid map = (Grid)vm_elems["vm_g1_map_grid"];
            double sw = map.Width / radar.Width;
            double sh = map.Height / radar.Height;
            double x1 = view.Margin.Left;
            double y1 = view.Margin.Top;
            double x2 = view.Margin.Left + view.Width;
            double y2 = view.Margin.Top + view.Height;
            map.Margin = new Thickness(-x1 * sw, -y1 * sh, 0, 0);
        }

        public void g1_map_redraw(double mw, double mh, bool points = false)
        {
            g1_mw = mw;
            g1_mh = mh;

            Grid map = g1_getMap();
            Grid main = (Grid)map.Parent;
            map.Width = main.Width * g1_mw;
            map.Height = main.Height * g1_mh;

            Rectangle radar_view = (Rectangle)vm_elems["vm_g1_radar_view"];
            Grid radar = (Grid)radar_view.Parent;
            radar_view.Width = radar.Width / g1_mw;
            radar_view.Height = radar.Height / g1_mh;

            if (points)
            {
                for (int x = 0; x <= map.Width; x += 100)
                {
                    for (int y = 0; y <= map.Height; y += 100)
                    {
                        Rectangle point = new Rectangle
                        {
                            Name = "vm_g1_map_p" + x + "_" + y,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 10,
                            Height = 10,
                            RadiusX = 10,
                            RadiusY = 10,
                            Fill = getSCB(Color.FromRgb(0, 255, 0)),
                            Margin = new Thickness(x - 5, y - 5, 0, 0),
                        };
                        map.Children.Add(point);
                        vm_assign(point);
                    }
                }
            }
        }
    }
}
