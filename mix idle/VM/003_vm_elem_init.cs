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
        public void vm_vos_elem_init()
        {

        }

        public void vm_sos_elem_init()
        {

        }

        //vm_vconfig_elem_init
        #region
        [NonSerialized]
        List<Rectangle> vm_vconfig_menu_group;
        public void vm_vconfig_elem_init()
        {
            //通用代码段
            Grid app_grid = vm_get_app_grid();
            Grid root = new Grid
            {
                Name = "设置",
                Visibility = Visibility.Hidden,
                Width = app_grid.Width,
                Height = app_grid.Height
            };
            app_grid.Children.Add(root);
            vm_assign(root);

            //背景
            Rectangle bg = new Rectangle
            {
                Name = "vm_vconfig_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(160, 0, 0, 0)),
            };
            root.Children.Add(bg);
            vm_assign(bg);

            int max = 8;
            // 1 系统交互
            // 2 应用信息
            // 3 个性化
            int n = 3;
            //菜单栏
            #region
            Grid menu = new Grid
            {
                Name = "vm_vconfig_menu",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = root.Width * 0.25,
                Height = root.Height * 0.9,
                Margin = new Thickness(root.Width * 0.025, 0, 0, 0),
                Background = getSCB(Color.FromRgb(200, 255, 200))
            };
            for (int i = 0; i < max; i++)
            {
                menu.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < n; i++)
            {
                Grid Smenu = new Grid
                {
                    Name = "vm_vconfig_menu_" + i + "_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
                Grid.SetRow(Smenu, i);
                menu.Children.Add(Smenu);
                vm_assign(Smenu);

                Rectangle Smenu_bg = new Rectangle
                {
                    Name = "vm_vconfig_menu_" + i + "_背景",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Fill = getSCB(Color.FromRgb(170, 140, 200)),
                    Stroke = getSCB(Color.FromRgb(50, 50, 50)),
                    StrokeThickness = 1.5,
                };
                Smenu.Children.Add(Smenu_bg);
                vm_assign(Smenu_bg);

                TextBlock Smenu_txt = new TextBlock
                {
                    Name = "vm_vconfig_menu_" + i + "_文字",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 20,
                };
                switch (i)
                {
                    case 0:
                        Smenu_txt.Text = "系统交互";
                        break;
                    case 1:
                        Smenu_txt.Text = "应用信息";
                        break;
                    case 2:
                        Smenu_txt.Text = "个性化";
                        break;
                }
                Smenu.Children.Add(Smenu_txt);
                vm_assign(Smenu_txt);

                Rectangle Smenu_button = new Rectangle
                {
                    Name = "vm_vconfig_menu_" + i,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                };
                vm_set_lbtn(Smenu_button);
                Smenu_button.Tag = enable;
                Smenu.Children.Add(Smenu_button);
                vm_assign(Smenu_button);
            }
            root.Children.Add(menu);
            vm_assign(menu);

            vm_vconfig_menu_group = make_group(menu);
            #endregion //菜单栏

            //设置界面
            Grid main = new Grid
            {
                Name = "vm_vconfig_menu",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = root.Width * 0.675,
                Height = root.Height * 0.9,
                Margin = new Thickness(root.Width * 0.3, 0, 0, 0),
            };
            root.Children.Add(main);
            vm_assign(main);

            Grid[] settings = new Grid[n];
            for (int i = 0; i < n; i++)
            {
                Grid target = new Grid
                {
                    Name = "vm_vconfig_menu_" + i + "_target_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Visibility = Visibility.Hidden,
                    Background = getSCB(Color.FromRgb(100, 180, 135))
                };
                main.Children.Add(target);
                vm_assign(target);
                settings[i] = target;
            }

            // 1 settings[0] 系统交互
            #region
            // C 关机确认 定时
            // C 锁屏定时
            // 事件形式（中心弹窗/下方弹窗/上方弹窗）  发送3个事件以测试
            // 通知形式（渐入渐出/滑入滑出/放大缩小/关闭） 停留时间 （未读通知会保留到）
            // 事件和通知记录到文件中  设置：保留时间
            Grid s0_close_grid = new Grid
            #region
            {
                Name = "s0_close_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = main.Height * 0.1,
                Margin = new Thickness(0, 0, 0, 0),
            };
            settings[0].Children.Add(s0_close_grid);
            vm_assign(s0_close_grid);

            CheckBox s0_close_c1 = new CheckBox
            {
                Name = "s0_close_c1",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 0, 0, 0),
                Content = "关机确认",
            };
            scale(s0_close_c1, 1.5, 1.5);
            s0_close_grid.Children.Add(s0_close_c1);
            vm_assign(s0_close_c1);

            TextBlock s0_close_time_text = new TextBlock
            {
                Name = "s0_close_time_text",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, main.Width * 0.35, 0),
                Text = "确认时间：",
                FontSize = 16,
                Foreground = getSCB(Color.FromRgb(0, 0, 0)),  
            };
            s0_close_grid.Children.Add(s0_close_time_text);
            vm_assign(s0_close_time_text);

            ComboBox s0_close_time = new ComboBox
            {
                Name = "s0_close_time",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = main.Width * 0.2,
                Margin = new Thickness(0, main.Height * 0.01, main.Width * 0.15, 0),
            };
            scale(s0_close_time, 1.5, 1.5);
            s0_close_time.Items.Add("3秒");
            s0_close_time.Items.Add("5秒");
            s0_close_time.Items.Add("10秒");
            s0_close_time.Items.Add("15秒");
            s0_close_time.SelectedIndex = 0;
            s0_close_grid.Children.Add(s0_close_time);
            vm_assign(s0_close_time);

            Rectangle s0_close_time_mask = new Rectangle
            {
                Name = "s0_close_time_mask",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = main.Width * 0.7,
                Margin = new Thickness(main.Width * 0.3, 0, 0, 0),
                Fill = getSCB(Color.FromArgb(127, 0, 0, 0)),
            };
            s0_close_grid.Children.Add(s0_close_time_mask);
            vm_assign(s0_close_time_mask);
            #endregion

            Grid s0_lock_grid = new Grid
            #region
            {
                Name = "s0_lock_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = main.Height * 0.1,
                Margin = new Thickness(0, main.Height * 0.1, 0, 0),
            };
            settings[0].Children.Add(s0_lock_grid);
            vm_assign(s0_lock_grid);

            CheckBox s0_lock_c1 = new CheckBox
            {
                Name = "s0_lock_c1",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 0, 0, 0),
                Content = "自动锁屏",
            };
            scale(s0_lock_c1, 1.5, 1.5);
            s0_lock_grid.Children.Add(s0_lock_c1);
            vm_assign(s0_lock_c1);

            TextBlock s0_lock_time_text = new TextBlock
            {
                Name = "s0_lock_time_text",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, main.Width * 0.35, 0),
                Text = "自动锁屏时间：",
                FontSize = 16,
                Foreground = getSCB(Color.FromRgb(0, 0, 0)),
            };
            s0_lock_grid.Children.Add(s0_lock_time_text);
            vm_assign(s0_lock_time_text);

            ComboBox s0_lock_time = new ComboBox
            {
                Name = "s0_lock_time",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = main.Width * 0.2,
                Margin = new Thickness(0, main.Height * 0.01, main.Width * 0.15, 0),
            };
            scale(s0_lock_time, 1.5, 1.5);
            s0_lock_time.Items.Add("30秒");
            s0_lock_time.Items.Add("60秒");
            s0_lock_time.Items.Add("120秒");
            s0_lock_time.Items.Add("240秒");
            s0_lock_time.Items.Add("480秒");
            s0_lock_time.Items.Add("1000秒");
            s0_lock_time.Items.Add("3600秒");
            s0_lock_time.Items.Add("99999999秒");
            s0_lock_time.SelectedIndex = 0;
            s0_lock_grid.Children.Add(s0_lock_time);
            vm_assign(s0_lock_time);

            Rectangle s0_lock_time_mask = new Rectangle
            {
                Name = "s0_lock_time_mask",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = main.Width * 0.7,
                Margin = new Thickness(main.Width * 0.3, 0, 0, 0),
                Fill = getSCB(Color.FromArgb(127, 0, 0, 0)),
            };
            s0_lock_grid.Children.Add(s0_lock_time_mask);
            vm_assign(s0_lock_time_mask);
            #endregion

            #endregion

            // 2 settings[1] 应用信息
            #region

            #endregion

            // 3 settings[2] 个性化
            // 切换边框颜色
            // 切换工具栏颜色
            // 切换底色/背景
            #region
            Grid s2_pre_grid = new Grid
            {
                Name = "s2_pre_grid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 20, 0, 0),
                Width = 400,
                Height = 225,
            };
            settings[2].Children.Add(s2_pre_grid);
            vm_assign(s2_pre_grid);

            Rectangle s2_pre_stroke = new Rectangle
            {
                Name = "s2_pre_stroke",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                StrokeThickness = 1.25,
            };
            s2_pre_grid.Children.Add(s2_pre_stroke);
            vm_assign(s2_pre_stroke);

            Rectangle s2_pre_bg = new Rectangle
            {
                Name = "s2_pre_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            s2_pre_grid.Children.Add(s2_pre_bg);
            vm_assign(s2_pre_bg);

            Image s2_pre_img = new Image {
                Name = "s2_pre_img",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            s2_pre_grid.Children.Add(s2_pre_img);
            vm_assign(s2_pre_img);

            //实现文件后切换图片
            #endregion

            config_load();
        }
        #endregion

        public void vm_vstore_elem_init()
        #region
        {
            //通用代码段
            Grid app_grid = vm_get_app_grid();
            Grid root = new Grid
            {
                Name = "应用商店",
                Visibility = Visibility.Hidden,
                Width = app_grid.Width,
                Height = app_grid.Height
            };
            app_grid.Children.Add(root);
            vm_assign(root);

            double w = app_grid.Width;
            double h = app_grid.Height;

            Grid title = new Grid
            #region
            {
                Name = "vm_vstore_title_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = h * 0.1,
                Background = getSCB(Color.FromRgb(225, 225, 225)),
            };
            root.Children.Add(title);
            vm_assign(title);

            TextBlock title_text = new TextBlock
            {
                Name = "vm_vstore_title_text",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "应用商店",
                FontSize = 20,
            };
            title.Children.Add(title_text);
            vm_assign(title_text);
            #endregion

            int x = 4;
            int y = 5;
            Grid main = new Grid
            #region
            {
                Name = "vm_vstore_main_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = h * 0.9,
                Margin = new Thickness(0, h * 0.1, 0, 0),
                Background = getSCB(Color.FromRgb(100, 160, 100)),
            };
            for (int k = 0; k < x; k++)
            {
                main.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int k = 0; k < y; k++)
            {
                main.RowDefinitions.Add(new RowDefinition());
            }
            root.Children.Add(main);
            vm_assign(main);

            Grid details = new Grid
            {
                Name = "vm_vstore_details_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Height = h * 0.9,
                Margin = new Thickness(0, h * 0.1, 0, 0),
            };
            root.Children.Add(details);
            vm_assign(details);


            for (int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    int n = i * x + j;
                    if (n >= vstore_list.Count)
                    {
                        continue;
                    }

                    Grid item = new Grid
                    {
                        Name = "vm_vstore_item_grid_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };
                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);
                    main.Children.Add(item);
                    vm_assign(item);

                    Rectangle bg = new Rectangle
                    {
                        Name = "vm_vstore_item_" + i + "_" + j + "_bg",
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromRgb(125, 150, 150)),
                    };
                    item.Children.Add(bg);
                    vm_assign(bg);

                    Rectangle stroke = new Rectangle
                    {
                        Name = "vm_vstore_item_stroke_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Stroke = getSCB(Color.FromRgb(50, 50, 125)),
                        StrokeThickness = 1,
                    };
                    item.Children.Add(stroke);
                    vm_assign(stroke);

                    Image image = new Image
                    {
                        Name = "vm_vstore_item_image_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = h * 0.15,
                        Height = h * 0.15,
                        Margin = new Thickness(h * 0.015, 0, 0, 0),
                    };
                    image.Source = vm_icon(vstore_list[n]);
                    item.Children.Add(image);
                    vm_assign(image);

                    RowDefinition definition;
                    Grid information = new Grid
                    #region
                    {
                        Name = "vm_vstore_item_information_grid_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Width = h * 0.25,
                        Height = h * 0.15,
                        Margin = new Thickness(0, 0, h * 0.015, 0),
                    };
                    definition = new RowDefinition();
                    definition.Height = new GridLength(5, GridUnitType.Star);
                    information.RowDefinitions.Add(definition);
                    definition = new RowDefinition();
                    definition.Height = new GridLength(10, GridUnitType.Star);
                    information.RowDefinitions.Add(definition);
                    definition = new RowDefinition();
                    definition.Height = new GridLength(4, GridUnitType.Star);
                    information.RowDefinitions.Add(definition);

                    item.Children.Add(information);
                    vm_assign(information);

                    TextBlock it1 = new TextBlock
                    {
                        Name = "vm_vstore_item_information_t1_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 18,
                        Foreground = getSCB(Color.FromRgb(255, 255, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(600)
                    };
                    Grid.SetRow(it1, 0);
                    information.Children.Add(it1);
                    vm_assign(it1);

                    TextBlock it2 = new TextBlock
                    {
                        Name = "vm_vstore_item_information_t2_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 14,
                        Foreground = getSCB(Color.FromRgb(0, 255, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(500)
                    };
                    Grid.SetRow(it2, 1);
                    information.Children.Add(it2);
                    vm_assign(it2);

                    TextBlock it3 = new TextBlock
                    {
                        Name = "vm_vstore_item_information_grid_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 14,
                        Foreground = getSCB(Color.FromRgb(255, 0, 0)),
                        FontWeight = FontWeight.FromOpenTypeWeight(600)
                    };
                    Grid.SetRow(it3, 2);
                    information.Children.Add(it3);
                    vm_assign(it3);

                    Rectangle cover = new Rectangle
                    {
                        Name = "vm_vstore_item_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Fill = getSCB(Color.FromArgb(0, 0, 0, 0)),
                    };
                    item.Children.Add(cover);
                    vm_assign(cover);

                    //后续添加的可能需要重启应用
                    //if (n < vstore_list.Count)
                    {
                        VM_APP a = vm_search_store_app(vstore_list[n]);
                        it1.Text = a.name;
                        it2.Text = a.type;
                        it3.Text = vstore_apps[a] + " 娱乐币";
                        vm_set_lbtn(cover); 

                        Grid detail = new Grid
                        #region
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail",
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            Visibility = Visibility.Hidden,
                            Background = getSCB(Color.FromRgb(249, 204, 226)),
                        };
                        Grid.SetRowSpan(detail, 100);
                        Grid.SetColumnSpan(detail, 100);
                        details.Children.Add(detail);
                        vm_assign(detail);

                        Grid up = new Grid
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_up",
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Top,
                            Height = h * 0.3,
                        };
                        detail.Children.Add(up);
                        vm_assign(up);

                        Grid down = new Grid
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_down",
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Height = h * 0.6,
                        };
                        detail.Children.Add(down);
                        vm_assign(down);

                        Line middle = new Line
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_middle",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            X1 = 10,
                            X2 = w - 10,
                            Y1 = 0,
                            Y2 = 0,
                            Stroke = getSCB(Color.FromRgb(127, 127, 127)),
                            StrokeThickness = 1.5,
                        };
                        down.Children.Add(middle);
                        vm_assign(middle);

                        //up
                        #region
                        Image detail_img = new Image
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_img",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Width = h * 0.25,
                            Height = h * 0.25,
                            Margin = new Thickness(h * 0.025, 0, 0, 0),
                        };
                        detail_img.Source = vm_icon(a.name);
                        up.Children.Add(detail_img);
                        vm_assign(detail_img);

                        Grid detail_text = new Grid
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_text_grid",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Width = w - h * 0.325,
                            Height = h * 0.25,
                            Margin = new Thickness(h * 0.3, 0, 0, 0),
                        };
                        up.Children.Add(detail_text);
                        vm_assign(detail_text);
                        definition = new RowDefinition();
                        definition.Height = new GridLength(3, GridUnitType.Star);
                        detail_text.RowDefinitions.Add(definition);
                        definition = new RowDefinition();
                        definition.Height = new GridLength(2, GridUnitType.Star);
                        detail_text.RowDefinitions.Add(definition);
                        definition = new RowDefinition();
                        definition.Height = new GridLength(2, GridUnitType.Star);
                        detail_text.RowDefinitions.Add(definition);
                        definition = new RowDefinition();
                        definition.Height = new GridLength(2, GridUnitType.Star);
                        detail_text.RowDefinitions.Add(definition);
                        definition = new RowDefinition();
                        definition.Height = new GridLength(2, GridUnitType.Star);
                        detail_text.RowDefinitions.Add(definition);


                        TextBlock dt1 = new TextBlock
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_t1",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 20,
                            Foreground = getSCB(Color.FromRgb(0, 127, 0)),
                            FontWeight = FontWeight.FromOpenTypeWeight(600),
                            Text = a.name,
                        };
                        Grid.SetRow(dt1, 0);
                        detail_text.Children.Add(dt1);
                        vm_assign(dt1);

                        TextBlock dt2 = new TextBlock
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_t2",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 16,
                            Foreground = getSCB(Color.FromRgb(0, 0, 0)),
                            Text = a.type,
                        };
                        Grid.SetRow(dt2, 1);
                        detail_text.Children.Add(dt2);
                        vm_assign(dt2);

                        TextBlock dt3 = new TextBlock
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_t3",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 16,
                            Foreground = getSCB(Color.FromRgb(0, 0, 0)),
                            Text = "开发者：" + a.author,
                        };
                        Grid.SetRow(dt3, 2);
                        detail_text.Children.Add(dt3);
                        vm_assign(dt3);

                        TextBlock dt4 = new TextBlock
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_t4",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 16,
                            Foreground = getSCB(Color.FromRgb(150, 0, 0)),
                            Text = "价格：" + vstore_apps[a] + " 娱乐币",
                        };
                        Grid.SetRow(dt4, 3);
                        detail_text.Children.Add(dt4);
                        vm_assign(dt4);

                        TextBlock dt5 = new TextBlock
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_t5",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 16,
                            Foreground = getSCB(Color.FromRgb(0, 0, 200)),
                            Text = "简介：" + a.des,
                        };
                        Grid.SetRow(dt5, 4);
                        detail_text.Children.Add(dt5);
                        vm_assign(dt5);

                        Button exit = new Button
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_exit",
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 50,
                            Height = 20,
                            Content = "返回",
                            Tag = detail.Name,
                            Margin = new Thickness(0, 0, 28, 0),
                        };
                        scale(exit, 1.5, 1.5);
                        exit.Click += button_click;
                        up.Children.Add(exit);
                        vm_assign(exit);

                        Button install = new Button
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_detail_install",
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 50,
                            Height = 20,
                            Content = "安装",
                            Tag = a,
                            Margin = new Thickness(0, 100, 28, 0),
                        };
                        scale(install, 1.5, 1.5);
                        install.Click += button_click;
                        up.Children.Add(install);
                        vm_assign(install);

                        if (a.installed)
                        {
                            install.Content = "已安装";
                            install.IsEnabled = false;
                        }
                        #endregion //up

                        //down
                        #region
                        Image des_img = new Image
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_des_img",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = h * 0.8,
                            Height = h * 0.45,
                            Margin = new Thickness(h * 0.03, h * 0.03, 0, 0),
                        };
                        //des_img.Source = vm_bg("Rainbow Hair/8_White");
                        down.Children.Add(des_img);
                        vm_assign(des_img);

                        Grid des_img_change = new Grid
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_des_img_change",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = h * 0.8,
                            Height = h * 0.06,
                            Margin = new Thickness(h * 0.03, h * 0.51, 0, 0),
                            Background = getSCB(Color.FromRgb(0, 0, 200)),
                        };
                        down.Children.Add(des_img_change);
                        vm_assign(des_img_change);

                        Grid des_main = new Grid
                        #region
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_des_main",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = w - h * 0.89,
                            Height = h * 0.54,
                            Margin = new Thickness(h * 0.86, h * 0.03, 0, 0),
                        };
                        down.Children.Add(des_main);
                        vm_assign(des_main);

                        TextBlock des_t1 = new TextBlock
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_des_t1",
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Top,
                            Text = "应用介绍：",
                            FontSize = 18,
                        };
                        des_main.Children.Add(des_t1);
                        vm_assign(des_t1);

                        Grid des_grid = new Grid
                        {
                            Name = "vm_vstore_item_" + i + "_" + j + "_des_grid",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = w - h * 0.89,
                            Height = h * 0.48,
                            Margin = new Thickness(0, h * 0.06, 0, 0),
                        };
                        des_main.Children.Add(des_grid);
                        vm_assign(des_grid);

                        rainbow_text rt = a.des2;
                        if (rt != null)
                        {
                            rt.prepare(des_grid.Name, HorizontalAlignment.Left,
                                VerticalAlignment.Top, new Thickness(0),
                                des_grid.Width, des_grid.Height, 14);
                            draw_r_text(rt);
                        }

                        #endregion

                        #endregion //down

                        #endregion
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        public void vm_vfs_elem_init()
        {

        }

        public void vm_valarm_elem_init()
        {

        }

        public void vm_vtask_elem_init()
        {

        }

        public void vm_vupdate_elem_init()
        {

        }

        public void vm_g1_elem_init()
        {
            g1_elem_init();
        }

        
        public void vm_g2_elem_init()
        {
            g2_elem_init();
        }

        public void vm_se_elem_init()
        {

        }

        public void vm_lock_elem_init()
        {

        }

        public void vm_mail_elem_init()
        {

        }

        public void vm_g3_elem_init()
        {

        }
    }
}
