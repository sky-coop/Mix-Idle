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
using System.Runtime.InteropServices;

namespace mix_idle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Allocates a new console for current process.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// Frees the console.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gs.save();
        }

        public gamestats gs;

        private void main_window_init(object sender, RoutedEventArgs e)
        {
            Top = 0;
            Left = 0;
            Image_bg_grid.Visibility = Visibility.Hidden;
            init();
        }

        private void init()
        {
            gs = new gamestats(this);
            gs.game_init(true);
        }

        public bool save_new(string filename)
        {
            gs = new gamestats(this);
            gs.game_init();
            return gs.save(filename);
        }

        public void change_savefile(gamestats g)
        {
            gs = g;
            gs.game_load();
        }

        private void option_selected(object sender, MouseButtonEventArgs e)
        {
            gs.option_selected(sender, e);
        }
        private void option_res_selected(object sender, MouseButtonEventArgs e)
        {
            gs.option_res_selected(sender, e);
        }

        private void rectangle_cover_move(object sender, MouseEventArgs e)
        {
            gs.rectangle_cover_move(sender, e);
        }
        private void rectangle_cover_enter(object sender, MouseEventArgs e)
        {
            gs.rectangle_cover_enter(sender, e);
        }
        private void rectangle_cover_leave(object sender, MouseEventArgs e)
        {
            gs.rectangle_cover_leave(sender, e);
        }
        private void rectangle_cover_down(object sender, MouseButtonEventArgs e)
        {
            gs.rectangle_cover_down(sender, e);
        }
        private void rectangle_cover_up(object sender, MouseButtonEventArgs e)
        {
            gs.rectangle_cover_up(sender, e);
        }

        private void debug(object sender, MouseButtonEventArgs e)
        {
            gs.debug(sender, e);
        }
        private void debug_gamespeed_down(object sender, MouseButtonEventArgs e)
        {
            gs.debug_gamespeed_down(sender, e);
        }
        private void debug_gamespeed_up(object sender, MouseButtonEventArgs e)
        {
            gs.debug_gamespeed_up(sender, e);
        }

        private void setting(object sender, MouseButtonEventArgs e)
        {
            gs.setting(sender, e);
        }

        private void setting_exit(object sender, MouseButtonEventArgs e)
        {
            gs.setting_exit(sender, e);
        }

        private void help(object sender, MouseButtonEventArgs e)
        {
            gs.help(sender, e);
        }

        private void help_exit(object sender, MouseButtonEventArgs e)
        {
            gs.help_exit(sender, e);
        }

        private void nmc_selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            gs.nmc_selectionchanged(sender, e);
        }

        private void achieve(object sender, MouseButtonEventArgs e)
        {
            gs.achieve(sender, e);
        }

        private void achieve_exit(object sender, MouseButtonEventArgs e)
        {
            gs.achieve_exit(sender, e);
        }

        private void resource_enter(object sender, RoutedEventArgs e)
        {
            gs.resource_enter(sender, e);
        }

        private void resource_exit(object sender, MouseButtonEventArgs e)
        {
            gs.resource_exit(sender, e);
        }

        private void time_down(object sender, RoutedEventArgs e)
        {
            gs.time_down(sender, e);
        }
        private void time_up(object sender, RoutedEventArgs e)
        {
            gs.time_up(sender, e);
        }


        double main_w = 0;
        double main_h = 0;
        double main_grid_w = 0;
        double main_grid_h = 0;
        private void sizechanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width == 0)
            {
                main_grid_w = main_grid.Width = main_grid.ActualWidth;
                main_grid_h = main_grid.Height = main_grid.ActualHeight;
                main_w = Width;
                main_h = Height;
                main_grid.HorizontalAlignment = HorizontalAlignment.Left;
                main_grid.VerticalAlignment = VerticalAlignment.Top;
                return;
            }
            double mulx = e.NewSize.Width / main_w;
            double muly = e.NewSize.Height / main_h;
            double mul = Math.Min(mulx, muly);
            if (mul < 1)
            {
                Width = main_w;
                Height = main_h;
                /*
                main_grid.Width = main_grid_w * mul;
                main_grid.Height = main_grid_h * mul;*/
            }
            else
            {
                gamestats.scale(main_grid, mul, mul);
            }
        }

        private void offline_exit(object sender, MouseButtonEventArgs e)
        {
            gs.offline_exit(sender, e);
        }

        private void debug_pulse(object sender, MouseButtonEventArgs e)
        {
            gs.debug_pulse(sender, e);
        }

        private void debug_recover(object sender, MouseButtonEventArgs e)
        {
            gs.debug_recover(sender, e);
        }
    }
}
