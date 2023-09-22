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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace mix_idle
{
    public class savefile
    {

        public bool save(gamestats g, string filepath)
        {
            string[] strs = filepath.Split('/');
            string dir = "./存档/" + strs[2];
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    return false;
                }
            }

            using (FileStream fswrite = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //开始序列化对象
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fswrite, g);
            }
            return true;
        }

        public bool load(MainWindow m, string filepath)
        {
            gamestats g = new gamestats(m);

            string[] strs = filepath.Split('/');
            string dir = "./存档/" + strs[2];
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                    g.m.save_new(strs[2]);
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                using (FileStream fsread = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    //确定可以创建，然后做未做完的工作
                    List<UIElement> temp = new List<UIElement>();
                    foreach (UIElement u in m.转生_main_grid.Children)
                    {
                        if (u is Line)
                        {
                            temp.Add(u);
                        }
                    }
                    foreach (UIElement u in temp)
                    {
                        m.转生_main_grid.Children.Remove(u);
                    }


                    //开始反序列
                    BinaryFormatter bf = new BinaryFormatter();
                    g = (gamestats)bf.Deserialize(fsread);
                }
            }
            catch
            {
                return false;
            }
            g.m = m;
            m.change_savefile(g);
            return true;
        }
    }
}
