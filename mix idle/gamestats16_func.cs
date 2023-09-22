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
        resource none = null;
        resource one = null;
        
        public List<resource> res_max(resource[] r)
        {
            List<resource> ret = new List<resource>();
            double2 max = r[0].get_value();
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i].get_value() == max)
                {
                    ret.Add(r[i]);
                }
                if (r[i].get_value() > max)
                {
                    ret.Clear();
                    ret.Add(r[i]);
                    max = ret[0].get_value();
                }
            }
            return ret;
        }
        

        public List<resource> res_min(resource[] r)
        {
            List<resource> ret = new List<resource>();
            double2 min = r[0].get_value();
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i].get_value() == min)
                {
                    ret.Add(r[i]);
                }
                if (r[i].get_value() < min)
                {
                    ret.Clear();
                    ret.Add(r[i]);
                    min = ret[0].get_value();
                }
            }
            return ret;
        }


        public double2 res_L_AAVG(resource[] r)
        {
            double2 logs = 0;
            for (int i = 0; i < r.Length; i++)
            {
                logs += double2.Max(r[i].get_value(), 1).Log10();
            }
            logs /= r.Length;
            return r.Length * double2.Pow(10, logs);
        }

        public double2 res_DX(resource[] r)
        {
            double2 logs = 0;
            for (int i = 0; i < r.Length; i++)
            {
                logs += double2.L(r[i].get_value());
            }
            double2 avg = logs / r.Length;
            double2 DX = 0;
            for (int i = 0; i < r.Length; i++)
            {
                double2 x = double2.L(r[i].get_value());
                DX = (x - avg) * (x - avg);
            }
            DX /= r.Length;
            return DX;
        }

        public static Tuple<bool, double2> parse_double2(string s)
        {
            int index = 0;
            if (s == "" || s.Length > 30)
            {
                return new Tuple<bool, double2>(false, 0);
            }
            string remain()
            {
                return s.Substring(index);
            };

            double2 ret = 0;

            int stage = 0;
            //-6.5-Mi30
            while (index < s.Length)
            {
                double2 current = 1;
                double t = 0;
                string before_e = "";
                string after_e = "";
                for (int i = remain().Length; i >= 1; i--)
                {
                    string cur = remain().Substring(0, i);
                    if (double.TryParse(cur, out t))
                    {
                        if (cur.Contains("e"))
                        {
                            before_e = cur.Split('e')[0];
                            after_e = cur.Split('e')[1];
                            index += before_e.Length + 1;
                        }
                        else if (cur.Contains("E"))
                        {
                            before_e = cur.Split('E')[0];
                            after_e = cur.Split('E')[1];
                            index += before_e.Length + 1;
                        }
                        else
                        {
                            current *= t;
                            index += i;
                        }
                        break;
                    }
                }
                if (before_e != "")
                {
                    current *= double.Parse(before_e);
                }
                if (after_e != "")
                {
                    int u = 0;
                    for (int i = remain().Length; i >= 1; i--)
                    {
                        if (int.TryParse(remain().Substring(0, i), out u))
                        {
                            current *= double2.Pow(10, u);
                            index += i;
                            break;
                        }
                    }
                }

                int neg = 1;
                if (remain().Length > 0 && remain()[0] == '-')
                {
                    neg = -1;
                    index++;
                }
                for (int i = number_level.Length - 1; i >= 0; i--)
                {
                    string check = number_level[i].ToLower();
                    if (remain().Length >= check.Length &&
                        remain().Substring(0, check.Length).ToLower() == check)
                    {
                        index += check.Length;
                        int OOM = (i + 1) * 3 * neg;
                        current *= double2.Pow(10, OOM);
                        break;
                    }
                }
                if (stage == 0)
                {
                    ret = current;
                }
                else
                {
                    ret.i += current.d;
                    break;
                }
                if (remain().Length > 0 && 
                    (remain()[0] == 'i' || remain()[0] == 'I'))
                {
                    stage = 1;
                    index++;
                }
                else
                {
                    break;
                }
            }
            if (remain() == "")
            {
                return new Tuple<bool, double2>(true, ret);
            }
            else
            {
                return new Tuple<bool, double2>(false, 0);
            }
        }
        
        /*
        public Dictionary<int, List<List<Tuple<int, int>>>> match3(game_grid g)
        {
            Dictionary<int, List<List<Tuple<int, int>>>> ret =
                new Dictionary<int, List<List<Tuple<int, int>>>>();
            List<int> types = new List<int>();
            foreach (game_grid_element e in g.elems)
            {
                if (e.type != 0)
                {
                    if (!types.Contains(e.type))
                    {
                        types.Add(e.type);
                        ret[e.type] = new List<List<Tuple<int, int>>>();
                    }
                }
            }
            for (int i = 0; i < g.row(); i++)
            {
                for(int j = 0; j < g.col(); j++)
                {
                    int type = g.elems[i, j].type;
                    
                }
            }
            foreach (int type in types)
            {
                for (int i = 0; i < g.row(); i++)
                {

                }
            }
        }*/
    }
}
