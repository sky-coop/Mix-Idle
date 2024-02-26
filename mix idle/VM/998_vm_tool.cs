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
        public List<string> vm_line_split(string s)
        {
            return new List<string>(s.Split('\n'));
        }

        public VM_APP vm_search_store_app(string name)
        {
            foreach(KeyValuePair<VM_APP, double> a in vstore_apps)
            {
                if(a.Key.name == name)
                {
                    return a.Key;
                }
            }
            return null;
        }

        public void remove_elem(string name, Panel target)
        {
            FrameworkElement rm = null;
            foreach (FrameworkElement f in target.Children)
            {
                if (f.Name == name)
                {
                    rm = f;
                }
            }
            if (rm != null)
            {
                target.Children.Remove(rm);
            }
        }

        public bool exist_elem(string name, Panel target)
        {
            FrameworkElement p = null;
            if (vm_elems.ContainsKey(name))
            {
                p = vm_elems[name];
            }
            if (p != null && p.Parent != null && p.Parent.Equals(target))
            {
                return true;
            }
            return false;
        }   


        [Serializable]
        public class thickness
        {
            public double left;
            public double top;
            public double right;
            public double bottom;

            public thickness(double l, double t, double r, double b)
            {
                left = l;
                top = t;
                right = r;
                bottom = b;
            }

            public Thickness GetThickness()
            {
                return new Thickness(left, top, right, bottom);
            }
        }
    }
}
