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
        private bool bg_is_image = false;

        private void vm_set_lbtn(FrameworkElement r)
        {
            r.Tag = 0;

            r.MouseEnter += rectangle_cover_enter;
            r.MouseLeave += rectangle_cover_leave;
            r.MouseLeftButtonDown += rectangle_cover_down;
            r.MouseLeftButtonUp += rectangle_cover_up;
            r.MouseMove += rectangle_cover_move;
        }

        private void vm_set_lrbtn(FrameworkElement r)
        {
            r.Tag = 0;

            r.MouseEnter += rectangle_cover_enter;
            r.MouseLeave += rectangle_cover_leave;
            r.MouseLeftButtonDown += rectangle_cover_down;
            r.MouseLeftButtonUp += rectangle_cover_up;
            r.MouseRightButtonDown += rectangle_cover_rdown;
            r.MouseRightButtonUp += rectangle_cover_rup;
            r.MouseMove += rectangle_cover_move;
        }

        [NonSerialized]
        Dictionary<string, FrameworkElement> vm_elems = new Dictionary<string, FrameworkElement>();

        [Serializable]
        public class VM
        {
            public gamestats gs;

            public bool opened = false;
            public bool auto_lock = false;
            public bool locking = false;
            public double2 locking_time = -1;
            public bool closing = false;
            public double2 closing_time = -1;

            public VM_OS os;
            public List<VM_APP> show_list = new List<VM_APP>();
            public List<VM_APP> apps = new List<VM_APP>();
            public List<string> events = new List<string>();

            public List<VM_FILE> files = new List<VM_FILE>();
            public VM_FILE root;

            public bool dt_changed = true;
            public bool dt_top = false;

            public void open()
            {
                if (!opened)
                {
                    opened = true;
                    locking = false;
                    gs.m.vm_main_grid.Visibility = Visibility.Visible;
                    gs.vm_elem_init();
                    gs.vm_boot_frame_progress = 0;
                    os.loading = true;
                    dt_changed = true;
                }
            }

            public void close(bool must = false)
            {
                if (must || gs.vm_close_check())
                {
                    show_list.Clear();
                    foreach (VM_APP a in apps)
                    {
                        kill(a);
                    }
                    gs.vm_elems.Clear();
                    gs.m.vm_main_grid.Visibility = Visibility.Hidden;
                    opened = false;
                    os.loading = false;
                }
            }

            public void reboot()
            {
                close();
                open();
            }

            public void install(VM_APP a)
            {
                if(a.installed == false)
                {
                    a.installed = true;
                    if(a is VM_OS)
                    {
                        change_os((VM_OS)a);
                    }
                    else
                    {
                        apps.Add(a);
                    }
                }
                else
                {
                    events.Add("应用“" + a.name + "”" + "已经安装");
                }
            }

            public void uninstall(VM_APP a)
            {
                if (a.system)
                {
                    events.Add("“" + a.name + "”" + "是系统应用，无法卸载");
                    return;
                }
                if (a.installed == true)
                {
                    a.installed = false;
                    apps.Remove(a);
                }
                else
                {
                    events.Add("应用“" + a.name + "”" + "已经安装");
                }
            }


            public void run(VM_APP a)
            {
                if (a.installed)
                {
                    if (dt_top)
                    {
                        dt_top = false;
                    }
                    if (a.running)
                    {
                        show_list.Remove(a);
                        show_list.Add(a);
                    }
                    else
                    {
                        a.elem_Initer();
                        a.running = true;
                        show_list.Add(a);
                    }
                }
            }

            public void run(string name)
            {
                VM_APP a = search_app(name);
                run(a);
            }

            public void kill(VM_APP a)
            { 
                if (a.installed && a.running)
                {
                    a.running = false;
                    a.showing = false;
                    show_list.Remove(a);
                }
            }

            public void kill(string name)
            {
                VM_APP a = search_app(name);
                kill(a);
            }

            public void change_os(VM_OS a)
            {
                if (os != null && !a.Equals(os))
                {
                    foreach (VM_APP v in os.sys_apps)
                    {
                        v.installed = false;
                        apps.Remove(v);
                    }
                    os.installed = false;
                    reboot();
                }

                foreach (VM_APP v in a.sys_apps)
                {
                    install(v);
                }
                os = a;
            }

            public VM_APP search_app(string name)
            {
                foreach(VM_APP a in apps)
                {
                    if(a.name == name)
                    {
                        return a;
                    }
                }
                return null;
            }

            public Dictionary<string, VM_FILE> search_file(string name, string ext = "")
            {
                Dictionary<string, VM_FILE> found = new Dictionary<string, VM_FILE>();
                Queue<VM_FILE> dirs = new Queue<VM_FILE>();
                dirs.Enqueue(root);
                while(dirs.Count > 0)
                {
                    VM_FILE curr = dirs.Dequeue();
                    foreach (VM_FILE f in curr.children)
                    {
                        if (f.dic)
                        {
                            dirs.Enqueue(f);
                        }
                        if (f.name == name && f.ext == ext)
                        {
                            found.Add(file_pathname(f), f);
                        }
                    }
                }
                return found;
            }

            public string file_pathname(VM_FILE f)
            {
                string ret = "";
                Stack<string> s = new Stack<string>();
                VM_FILE curr = f;
                while(curr.parent != null)
                {
                    s.Push(f.parent.name);
                    curr = curr.parent;
                }
                while (s.Count > 0)
                {
                    ret += s.Pop();
                    ret += "/";
                }
                ret += f.name;
                if (f.ext != "")
                {
                    ret += "." + f.ext;
                }
                return ret;
            }
            
            public VM_FILE search_file_by_path(string path)
            {
                if(path == root.name)
                {
                    return root;
                }

                string[] parts = path.Split('/');
                VM_FILE curr = root;
                for(int p = 1; p < parts.Length; p++)
                {
                    foreach(VM_FILE f in curr.children)
                    {
                        if(p == parts.Length - 1)
                        {
                            if(f.ext == "" && parts[p] == f.name)
                            {
                                return f;
                            }
                            if(parts[p] == (f.name + "." + f.ext))
                            {
                                return f;
                            }
                        }
                        else if (f.dic && f.name == parts[p])
                        {
                            curr = f;
                            goto success;
                        }
                        else
                        {
                        }
                    }
                    return null;
                success:;
                }
                return null;
            }
        }
        VM vm = new VM();

        public delegate void app_ticker(double2 t);
        public delegate void app_initer();
        public delegate void app_elem_initer();
        [Serializable]
        public class VM_APP
        {
            public string name;
            public string author = "skycoop";
            public string type = "";
            public string des = "";  //简介
            public rainbow_text des2 = null; //介绍
            public bool system = false;
            public bool installed = false;
            public bool running = false;
            public bool showing = false;
            public app_ticker ticker;
            public app_initer initer;
            public app_elem_initer elem_Initer;
            public int version = 1;
            public int sub_version = 0;

            public double2 get_offline_time()
            {
                return 0;
            }

            public VM_APP(string n, app_ticker t, app_initer i, app_elem_initer ei, bool s = false)
            {
                name = n;
                ticker = t;
                initer = i;
                elem_Initer = ei;
                system = s;
            }
        }

        public delegate void vm_os_bootloader(int p);

        [Serializable]
        public class VM_OS : VM_APP
        {
            public List<VM_APP> sys_apps = new List<VM_APP>();

            public bool loading = false;
            public vm_os_bootloader loader;
            public int boot_frame_max = 300;

            public VM_OS(string n, app_ticker t, app_initer i, app_elem_initer ei) : base(n, t, i, ei, true)
            {

            }
        }

        [Serializable]
        public class VM_FILE
        #region
        {
            public bool valid = true;

            public VM_FILE parent;
            public bool dic = false;
            public List<VM_FILE> children = new List<VM_FILE>();

            public string name;
            public string ext;
            public string content;

            public bool r_only = false;

            public VM_FILE(string n, string ex = "")
            {
                if(check(n) && check(ex))
                {
                    name = n;
                    ext = ex;
                    dic = false;
                }
                else
                {
                    valid = false;
                }
            }

            public VM_FILE(string n, bool d = false)
            {
                if (check(n))
                {
                    name = n;
                    ext = "";
                    dic = d;
                }
                else
                {
                    valid = false;
                }
            }

            public bool check(string s)
            {
                foreach(char c in s)
                {
                    if(c == '/')
                    {
                        return false;
                    }
                }
                return true;
            }

            public void add(VM_FILE f)
            {
                children.Add(f);
                f.parent = this;
                if (r_only)
                {
                    f.r_only = true;
                }
            }

            public string read()
            {
                if (!dic)
                {
                    return content;
                }
                return "";
            }

            public void write(string s)
            {
                if (!dic)
                {
                    content = s;
                }
            }

            public void append(string s)
            {
                if (!dic)
                {
                    content += s;
                }
            }
        }
        #endregion

        public string bin = Environment.CurrentDirectory + "/";
        public const string vm_res_dir = "IMAGE/VM/";
        public const string vm_res_icon_dir = vm_res_dir + "ICON/";
        public const string vm_res_bg_dir = vm_res_dir + "Background/";
        public BitmapImage vm_icon(string name)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri(bin + vm_res_icon_dir + name + ".bmp", UriKind.Absolute));
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        
        public BitmapFrame vm_jpg(string abs_path)
        {
            return BitmapFrame.Create(new Uri(abs_path, UriKind.Absolute));
        }

        public BitmapFrame vm_bg(string name)
        {
            return vm_jpg(bin + vm_res_bg_dir + name + ".jpg");
        }

        public BitmapImage vm_icon_app(VM_APP a)
        {
            return vm_icon(a.name);
        }

        public void vm_tick()
        {
            if (vm.opened)
            {
                vm_boot();
                foreach (VM_APP a in vm.apps)
                {
                    if (a.running)
                    {
                        a.ticker(s_pulser(a.name));
                    }
                }
                vm_desktop_tick(s_pulser("vm_desktop"));
                vm_update();

                Grid close_grid = (Grid)vm_elems["vm_close_grid"];
                close_grid.Visibility = Visibility.Hidden;
                if (vm.closing)
                {
                    close_grid.Visibility = Visibility.Visible;
                    vm.closing_time -= time_tick_actually;
                    TextBlock t2 = (TextBlock)vm_elems["vm_close_t2"];
                    t2.Text = "您确定要关机吗？将在 " + time_transfer(vm.closing_time) +"后自动关机。";
                    if (vm.closing_time < 0)
                    {
                        vm.closing = false;
                        vm.close(true);
                    }
                }
            }
            else
            {
                m.vm_process_text.Text = "";
            }

            if (vm_fullscreen)
            {
                vm_fullscreen_time += time_tick_actually;
                m.娱乐_全屏_text.Visibility = Visibility.Visible;
                m.娱乐_全屏_text.Text = "您已进入全屏模式: " + vm_fullscreen_time + "s";
            }
            else
            {
                vm_fullscreen_time = 0;
                m.娱乐_全屏_text.Visibility = Visibility.Hidden;
            }
        }

        //show
        public void vm_update()
        {
            CheckBox c = (CheckBox)vm_elems["s0_lock_c1"];
            if ((bool)c.IsChecked && !vm.locking)
            {
                vm.auto_lock = true;
                vm.locking_time += time_tick_actually;
                double2 time = 30;
                string s = (string)((ComboBox)vm_elems["s0_lock_time"]).SelectedItem;
                s = s.Replace('秒', ' ');
                time = int.Parse(s);
                if (vm.locking_time >= time)
                {
                    vm.locking = true;
                }
            }
            else
            {
                vm.locking_time = 0;
            }

            if (vm.locking)
            {
                foreach (VM_APP a in vm.show_list)
                {
                    if(vm_elems[a.name].Visibility != Visibility.Hidden)
                    {
                        vm_elems[a.name].Visibility = Visibility.Hidden;
                    }
                    a.showing = false;
                }
                vm_elems["vm_lock_grid"].Visibility = Visibility.Visible;
                m.vm_process_text.Text = "已锁屏";
            }
            else
            {
                vm_elems["vm_lock_grid"].Visibility = Visibility.Hidden;
                vm.os.showing = false;

                VM_APP x = null;
                if (vm.show_list.Count == 0 || vm.dt_top)
                {
                    //显示操作系统，即什么应用都不显示
                    m.vm_process_text.Text = "当前显示：桌面";
                    vm.os.showing = true;
                }
                else
                {
                    x = vm.show_list.Last();
                    if (vm_elems[x.name].Visibility != Visibility.Visible)
                    {
                        vm_elems[x.name].Visibility = Visibility.Visible;
                    }
                    x.showing = true;
                    m.vm_process_text.Text = "当前显示进程：" + x.name;
                }

                foreach (VM_APP a in vm.show_list)
                {
                    if (x != null && x.Equals(a))
                    {
                        continue;
                    }
                    if (vm_elems[a.name].Visibility != Visibility.Hidden)
                    {
                        vm_elems[a.name].Visibility = Visibility.Hidden;
                    }
                    a.showing = false;
                }
            }
        }

        VM_OS VOS;
        VM_OS SOS;
        public void vm_init()
        {
            vm.gs = this;

            VOS = new VM_OS("VOS", new app_ticker(vm_vos_tick), 
                new app_initer(vm_vos_init), new app_elem_initer(vm_vos_elem_init));
            SOS = new VM_OS("SOS", new app_ticker(vm_sos_tick), 
                new app_initer(vm_sos_init), new app_elem_initer(vm_sos_elem_init));

            VM_APP vos_config = new VM_APP("设置", new app_ticker(vm_vconfig_tick), 
                new app_initer(vm_vconfig_init), new app_elem_initer(vm_vconfig_elem_init), true);
            VM_APP vos_store = new VM_APP("应用商店", new app_ticker(vm_vstore_tick), 
                new app_initer(vm_vstore_init), new app_elem_initer(vm_vstore_elem_init), true);
            VM_APP vos_fs = new VM_APP("文件", new app_ticker(vm_vfs_tick), 
                new app_initer(vm_vfs_init), new app_elem_initer(vm_vfs_elem_init), true);
            VM_APP vos_alarm = new VM_APP("闹钟", new app_ticker(vm_valarm_tick), 
                new app_initer(vm_valarm_init), new app_elem_initer(vm_valarm_elem_init), true);
            VM_APP vos_task = new VM_APP("进程管理", new app_ticker(vm_vtask_tick),
                new app_initer(vm_vtask_init), new app_elem_initer(vm_vtask_elem_init), true);
            VM_APP vos_update = new VM_APP("系统更新", new app_ticker(vm_vupdate_tick),
                new app_initer(vm_vupdate_init), new app_elem_initer(vm_vupdate_elem_init), true);

            VOS.sys_apps.Add(vos_config);
            VOS.sys_apps.Add(vos_store);
            VOS.sys_apps.Add(vos_fs);
            VOS.sys_apps.Add(vos_alarm);
            VOS.sys_apps.Add(vos_task);
            VOS.sys_apps.Add(vos_update);

            VOS.loader = vm_vos_boot_1;
            vm.install(VOS);

            vm.os.initer();
            foreach (VM_APP a in vm.apps)
            {
                a.initer();
                VM_APP_VER vv = new VM_APP_VER(a.name);
                vv.app_ver.Add(new Tuple<int, int>(1, 0));
                vm_ver_ctrl.apps_ver[a.name] = vv;
            }

            VM_FILE root = new VM_FILE("Root", true);
            vm.root = root;

            VM_FILE dir_sys = new VM_FILE("System", true);
            dir_sys.r_only = true;
            root.add(dir_sys);

            VM_FILE dir_sys_app = new VM_FILE("APP", true);
            dir_sys.add(dir_sys_app);
            VM_FILE dir_dt = new VM_FILE("Desktop", true);
            dir_sys.add(dir_dt);

            VM_FILE dt_def = new VM_FILE("Definition", "sys");
            dt_def.write("4 8");
            dir_dt.add(dt_def);
            VM_FILE dt_arr = new VM_FILE("Arrangement", "sys");
            dt_arr.append("设置 0 0\n");
            dt_arr.append("应用商店 0 1\n");
            dir_dt.add(dt_arr);
        }

        public void vm_elem_init()
        {
            Grid b_grid = m.vm_button_grid;
            //返回 桌面 进程管理 锁屏 电源
            for (int i = 0; i < 5; i++)
            {
                Grid x = new Grid
                {
                    Name = "vm_button_" + i + "_grid",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                Grid.SetColumn(x, i);
                b_grid.Children.Add(x);

                Image image = new Image
                {
                    Name = "vm_button_" + i,
                    Source = vm_icon(i.ToString()),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                x.Children.Add(image);
                
                vm_set_lbtn(image);
            }

            Grid vm_screen = new Grid
            {
                Name = "vm_screen",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            m.vm_grid.Children.Add(vm_screen);
            vm_assign(vm_screen);

            Rectangle border = new Rectangle
            {
                Name = "vm_border",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stroke = getSCB(Color.FromRgb(0, 255, 255)),
                StrokeThickness = 2.5
            };
            m.vm_grid.Children.Add(border);
            vm_assign(border);

            //给操作系统和应用显示的层
            Grid vm_app_grid = new Grid
            {
                Name = "vm_app_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = m.vm_grid.Width,
                Height = m.vm_grid.Height
            };
            vm_screen.Children.Add(vm_app_grid);
            vm_assign(vm_app_grid);

            //通知层
            Grid vm_event_grid = new Grid
            {
                Name = "vm_event_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            vm_screen.Children.Add(vm_event_grid);
            vm_assign(vm_event_grid);

            //关机提示
            #region
            Grid vm_close_grid = new Grid
            {
                Name = "vm_close_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = getSCB(Color.FromArgb(127, 0, 0, 0)),
            };
            vm_event_grid.Children.Add(vm_close_grid);
            vm_assign(vm_close_grid);

            Grid vm_close_cgrid = new Grid
            {
                Name = "vm_close_cgrid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = vm_app_grid.Width * 0.5,
                Height = vm_app_grid.Height * 0.5,
                Background = getSCB(Color.FromRgb(200, 200, 200)),
            };
            vm_close_grid.Children.Add(vm_close_cgrid);
            vm_assign(vm_close_cgrid);

            TextBlock vm_close_t1 = new TextBlock
            {
                Name = "vm_close_t1",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, vm_app_grid.Height * 0.075, 0, 0),
                FontSize = 20,
                Text = "提示",
            };
            vm_close_cgrid.Children.Add(vm_close_t1);
            vm_assign(vm_close_t1);

            TextBlock vm_close_t2 = new TextBlock
            {
                Name = "vm_close_t2",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, vm_app_grid.Height * 0.225, 0, 0),
                FontSize = 16,
                Text = "您确定要关机吗？将在3秒后自动关机。",
            };
            vm_close_cgrid.Children.Add(vm_close_t2);
            vm_assign(vm_close_t2);

            Button vm_close_confirm = new Button
            {
                Name = "vm_close_confirm",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = vm_app_grid.Width * 0.1,
                Height = vm_app_grid.Height * 0.05,
                Margin = new Thickness(0, 0, vm_app_grid.Width * 0.03, vm_app_grid.Height * 0.03),
                Content = "确定",
            };
            vm_close_confirm.Click += button_click;
            vm_close_cgrid.Children.Add(vm_close_confirm);
            vm_assign(vm_close_confirm);

            Button vm_close_cancel = new Button
            {
                Name = "vm_close_cancel",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = vm_app_grid.Width * 0.1,
                Height = vm_app_grid.Height * 0.05,
                Margin = new Thickness(vm_app_grid.Width * 0.03, 0, 0, vm_app_grid.Height * 0.03),
                Content = "取消",
            };
            vm_close_cancel.Click += button_click;
            vm_close_cgrid.Children.Add(vm_close_cancel);
            vm_assign(vm_close_cancel);
            #endregion

            //锁屏层
            Grid vm_lock_grid = new Grid
            {
                Name = "vm_lock_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = getSCB(Color.FromRgb(0, 0, 0)),
            };
            vm_screen.Children.Add(vm_lock_grid);
            vm_assign(vm_lock_grid);

            //启动画面的元素


            //操作系统界面
            #region
            Grid vm_os_grid = new Grid
            {
                Name = "vm_os_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            vm_app_grid.Children.Add(vm_os_grid);
            vm_assign(vm_os_grid);

            //背景
            #region
            Grid vm_os_bg_grid = new Grid
            {
                Name = "vm_os_bg_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            vm_os_grid.Children.Add(vm_os_bg_grid);
            vm_assign(vm_os_bg_grid);

            Image vm_os_bg_img = new Image
            {
                Name = "vm_os_bg_img",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            vm_os_bg_grid.Children.Add(vm_os_bg_img);
            vm_assign(vm_os_bg_img);

            //Argb mask
            Rectangle vm_os_bg_rect = new Rectangle
            {
                Name = "vm_os_bg_rect",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(255, 0, 180, 180)),
                //Fill = getSCB(Color.FromArgb(100, 0, 0, 0)),
            };
            vm_os_bg_grid.Children.Add(vm_os_bg_rect);
            vm_assign(vm_os_bg_rect);
            #endregion //背景

            //主界面
            #region
            Grid vm_os_main_grid = new Grid
            {
                Name = "vm_os_main_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(50, GridUnitType.Pixel);
            vm_os_main_grid.RowDefinitions.Add(rowDefinition);

            rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(1, GridUnitType.Star);
            vm_os_main_grid.RowDefinitions.Add(rowDefinition);


            vm_os_grid.Children.Add(vm_os_main_grid);
            vm_assign(vm_os_main_grid);


            //工具栏
            #region
            Grid vm_os_tool_grid = new Grid
            {
                Name = "vm_os_tool_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetRow(vm_os_tool_grid, 0);
            vm_os_main_grid.Children.Add(vm_os_tool_grid);
            vm_assign(vm_os_tool_grid);

            Rectangle vm_os_tool_bg = new Rectangle
            {
                Name = "vm_os_tool_bg",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Fill = getSCB(Color.FromArgb(100, 127, 127, 0)),
            };
            vm_os_tool_grid.Children.Add(vm_os_tool_bg);
            vm_assign(vm_os_tool_bg);

            TextBlock vm_os_tool_time_text = new TextBlock
            {
                Name = "vm_os_tool_time_text",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Margin = new Thickness(0, 0, 20, 0),
                Foreground = getSCB(Color.FromRgb(255, 255, 255)),
            };
            vm_os_tool_grid.Children.Add(vm_os_tool_time_text);
            vm_assign(vm_os_tool_time_text);
            #endregion //工具栏

            //桌面
            #region
            Grid vm_os_dt_grid = new Grid
            {
                Name = "vm_os_dt_grid",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetRow(vm_os_dt_grid, 1);
            VM_FILE dt_def = vm.search_file_by_path("Root/System/Desktop/Definition.sys");
            string[] parts = dt_def.read().Split(' ');
            int dt_h = Convert.ToInt32(parts[0]);
            int dt_w = Convert.ToInt32(parts[1]);
            for (int i = 0; i < dt_h; i++)
            {
                vm_os_dt_grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < dt_w; i++)
            {
                vm_os_dt_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            vm_os_main_grid.Children.Add(vm_os_dt_grid);
            vm_assign(vm_os_dt_grid);

            for(int i = 0; i < dt_h; i++)
            {
                for(int j = 0; j < dt_w; j++)
                {
                    Grid vm_os_app_grid = new Grid
                    {
                        Name = "vm_os_app_grid_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };
                    Grid.SetRow(vm_os_app_grid, i);
                    Grid.SetColumn(vm_os_app_grid, j);
                    vm_os_dt_grid.Children.Add(vm_os_app_grid);
                    vm_assign(vm_os_app_grid);

                    rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(4, GridUnitType.Star);
                    vm_os_app_grid.RowDefinitions.Add(rowDefinition);

                    rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                    vm_os_app_grid.RowDefinitions.Add(rowDefinition);

                    Image vm_os_app_icon = new Image
                    {
                        Name = "vm_os_app_icon_" + i + "_" + j,
                        Width = 70,
                        Height = 70,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    vm_set_lbtn(vm_os_app_icon);
                    Grid.SetRow(vm_os_app_icon, 0);
                    vm_os_app_grid.Children.Add(vm_os_app_icon);
                    vm_assign(vm_os_app_icon);

                    TextBlock vm_os_app_name = new TextBlock
                    {
                        Name = "vm_os_app_name_" + i + "_" + j,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = i + " " + j,
                        FontSize = 14,
                    };
                    Grid.SetRow(vm_os_app_name, 1);
                    vm_os_app_grid.Children.Add(vm_os_app_name);
                    vm_assign(vm_os_app_name);
                }
            }

            #endregion //桌面


            #endregion //主界面

            #endregion //操作系统界面

            //图片测试
            //vm_os_bg_img.Source = vm_bg("Rainbow Hair/8_White");
            m.image.Source = vm_bg("Rainbow Hair/8_White");

            foreach(VM_APP a in vm.apps)
            {
                if (a.running)
                {
                    a.elem_Initer();
                }
            }
            vm_vconfig_elem_init();   //可能被替换
        }

        public int vm_boot_frame_progress;
        public const int vm_boot_frame_max = 300;
        //启动动画 只是播放动画而已 不同操作系统可能不同
        public void vm_boot()
        {
            if (vm.os.loading)
            {
                vm.os.loader(vm_boot_frame_progress);
                vm_boot_frame_progress++;
            }
        }

        public bool vm_close_check()
        {
            CheckBox c = null;
            if (vm_elems.ContainsKey("s0_close_c1"))
            {
                c = (CheckBox)vm_elems["s0_close_c1"];
            }
            if (c != null && (bool)c.IsChecked)
            {
                vm.closing = true;
                double2 time = 3;
                string s = (string)((ComboBox)vm_elems["s0_close_time"]).SelectedItem;
                s = s.Replace('秒', ' ');
                time = int.Parse(s);
                vm.closing_time = time;
                return false;
            }
            else
            {
                return true;
            }
        }

        public Grid vm_get_app_grid()
        {
            return (Grid)vm_elems["vm_app_grid"];
        }

        private void vm_assign(FrameworkElement f)
        {
            if (!vm_elems.ContainsKey(f.Name))
            {
                vm_elems.Add(f.Name, f);
            }
            else
            {
                vm_elems[f.Name] = f;
            }
        }

        private T find_elem<T>(string name) where T : FrameworkElement
        {
            T r = vm_find_elem<T>(name);

            if(r == null)
            {
                r = m.FindName(name) as T;
            }
            return r;
        }

        private T vm_find_elem<T>(string name) where T : FrameworkElement
        {
            if(vm_elems == null)
            {
                return null;
            }
            T r = null;
            if (vm_elems.ContainsKey(name))
            {
                r = (T)vm_elems[name];
            }
            return r;
        }
    }
}
