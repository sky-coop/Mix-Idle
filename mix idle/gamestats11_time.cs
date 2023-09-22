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
using System.Net;

namespace mix_idle
{
    public partial class gamestats
    {
        public bool offline = true;
        public double2 save_time = 0;
        public double2 time_active_actually = 0;

        public double2 offline_time_current = 0;
        public double2 offline_time_remain = 0;
        public double2 offline_time_used = 0;
        public double2 offline_time_valid = 0;
        public double2 offline_time_all()
        {
            return offline_time_remain + offline_time_used;
        }
        public double2 time_all_real()
        {
            return offline_time_all() + time_active_actually;
        }

        public int pulse_max = 0;
        public int pulse_count = 0;
        double2 pulse_t = 0;
        bool pulsing = false;
        private void pulse(double a)
        {
            pulse_t += a;
        }

        public double2 offline_effect = 100;


        public double2 get_now()
        {
            return (double2)(decimal)DateTime.Now.Ticks / 10000000;
        }
        public string get_online_now()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                {
                    if (h == "Date")
                    {
                        datetime = headerCollection[h];
                    }
                }
                return datetime;
            }
            catch (Exception) { return datetime; }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }

        public void offline_exit(object sender, MouseButtonEventArgs e)
        {
            m.offline_grid.Visibility = Visibility.Hidden;
        }

        public void offline_update()
        {
            m.offline_t2.Text = "与上次保存时相比，你离线了：" + number_format(offline_time_current) + "s";
            
            m.offline_t3.Text = "                                           （相当于：" + time_transfer(offline_time_current) + "）";

            offline_time_valid = offline_time_remain;
            if(offline_time_valid + offline_time_used > 2 * time_active_actually)
            {
                offline_time_valid = 2 * time_active_actually - offline_time_used;
            }

            m.offline_t4.Text = "你有 " + number_format(offline_time_used) + "s 已使用的" +
                "离线时间，" + number_format(offline_time_remain) + "s 未使用的离线时间，" +
                 number_format(time_active_actually) + "s 在线游戏时间，请注意所有有效的离线时间" +
                 "不会超过在线时间的2倍，因此本次 " + number_format(offline_time_valid) + "s " +
                 "的离线时间会帮助你更进一步。";
            
            pulse_max = (int)(double2.Pow(offline_time_valid, 0.5)).d;
            if (m.能量_grid.Visibility == 0)
            {
                m.offline_t5.Text = "这些离线时间会转化为 " + number_format(offline_time_valid * offline_effect) + 
                    " 的终极能量（每秒" + number_format(offline_effect) + "），" +
                    "可用于在线游玩时加速游戏，并且永远不会被重置。";
            }
            else
            {
                m.offline_t5.Text = "很遗憾！你并没有解锁[能量]，这些离线时间" +
                    "将被转化为相应时长的瞬间产出。（原始游戏速度，" +
                    "在" + number_format(pulse_max) + "帧中消耗完毕）";
            }
        }

        public void offline_produce()
        {
            if (m.能量_grid.Visibility == 0)
            {
                find_resource("终极能量").add_value(offline_time_valid * offline_effect, true);
            }
            else
            {
                for (pulse_count = 0; pulse_count < pulse_max; pulse_count++)
                {
                    pulse_t = offline_time_valid / pulse_max * (game_speed_base / gamespeed());
                    time_active_actually -= pulse_t;
                    game_tick(new object(), null);
                }
            }
            offline_time_remain -= offline_time_valid;
            offline_time_used += offline_time_valid;
            offline_time_current = 0;
            offline_time_valid = 0;
        }

        [Serializable]
        public class slow_ticker
        {
            public double2 acc = 0;
            public double2 last = 0;

            public slow_ticker(double2 now)
            {
                last = now;
            }
        }
        Dictionary<string, slow_ticker> s_tickers = new Dictionary<string, slow_ticker>();

        public bool s_ticker(string name, double t)
        {
            set_s_ticker(name, t);
            slow_ticker s = s_tickers[name];
            double2 now = time_all_acutally;
            s.acc += now - s.last;
            if(s.acc >= t)
            {
                s.acc = 0;
                s.last = now;
                return true;
            }
            s.last = now;
            return false;
        }

        public double2 s_pulser(string name)
        {
            set_s_ticker(name, 0);
            slow_ticker s = s_tickers[name];
            double2 now = time_all_acutally;
            s.acc += now - s.last;
            double2 temp = s.acc;
            s.acc = 0;
            s.last = now;
            return temp;
        }

        public void set_s_ticker(string name, double t)
        {
            if (!s_tickers.ContainsKey(name))
            {
                s_tickers.Add(name, new slow_ticker(time_all_acutally));
                slow_ticker s = s_tickers[name];
                s.acc += t;
            }
        }

        public string time_transfer(double2 time)
        #region
        {
            double i = time.i;
            double d = time.d;
            string s = "";
            if (i > 0)
            {
                s += "(" + number_format(i) + " 无限)";
            }
            if (d >= 86400)
            {
                double fd = double_floor(d / 86400);
                s += number_format(fd) + " 天 ";
                d -= fd * 86400;
            }
            if (d >= 3600)
            {
                double fh = double_floor(d / 3600);
                s += number_format(fh) + " 时 ";
                d -= fh * 3600;
            }
            if (d >= 60)
            {
                double fm = double_floor(d / 60);
                s += number_format(fm) + " 分 ";
                d -= fm * 60;
            }
            double fs = double_floor(d);
            s += number_format(fs) + " 秒";

            return s;
        }
        #endregion

        /*
        public double2 online_time_convert(string str)
        {

        }*/
    }
}
