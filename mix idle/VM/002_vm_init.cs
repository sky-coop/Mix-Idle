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
        public void vm_vos_init()
        {

        }

        public void vm_sos_init()
        {

        }

        public void vm_vconfig_init()
        {

        }

        Dictionary<VM_APP, double> vstore_apps;
        List<string> vstore_list;
        public void vm_vstore_init()
        #region
        {
            vstore_apps = new Dictionary<VM_APP, double>();
            vstore_list = new List<string>();

            string name = "世界树";
            rainbow_text rt = new rainbow_text(name);
            rt.add("世界树", 0, 225, 0);
            rt.add("是一款", 0, 0, 0);
            rt.add("放置类游戏", 0, 150, 150);
            rt.add("，它的界面与", 0, 0, 0);
            rt.add("声望树系列", 225, 0, 0);
            rt.add("的游戏相似，但体验有很大的不同。游戏开始时你有一棵小小的", 0, 0, 0);
            rt.add("世界树", 0, 200, 0);
            rt.add("，通过发展", 0, 0, 0);
            rt.add("其中的小世界", 127, 0, 200);
            rt.add("和", 0, 0, 0);
            rt.add("其外的森林和文明", 127, 0, 200);
            vstore_add(new VM_APP(name, new app_ticker(vm_g1_tick), 
                new app_initer(vm_g1_init), new app_elem_initer(vm_g1_elem_init)), 0,
                "游戏 / 放置 / 增量 / 声望树",
                "培育你的世界树，建设周围的文明和森林，赢取世界点数和娱乐币！",
                rt);      //game 1

            name = "聚能光珠";
            rt = new rainbow_text(name);
            vstore_add(new VM_APP(name, new app_ticker(vm_g2_tick), 
                new app_initer(vm_g2_init), new app_elem_initer(g2_elem_init)), 0,
                "游戏 / 解谜 / 放置 / 自制地图",
                "",
                null);      //game 2

            name = "安全卫士";
            rt = new rainbow_text(name);
            vstore_add(new VM_APP(name, new app_ticker(vm_se_tick), 
                new app_initer(vm_se_init), new app_elem_initer(vm_se_elem_init)), 0,
                "工具 / 安全",
                "",
                null);      //？

            name = "一键锁屏";
            rt = new rainbow_text(name);
            vstore_add(new VM_APP(name, new app_ticker(vm_lock_tick), 
                new app_initer(vm_lock_init), new app_elem_initer(vm_lock_elem_init)), 0,
                "工具",
                "",
                null);  //恶意应用

            name = "邮件";
            rt = new rainbow_text(name);
            vstore_add(new VM_APP(name, new app_ticker(vm_mail_tick), 
                new app_initer(vm_mail_init), new app_elem_initer(vm_mail_elem_init)), 0,
                "工具 / 推荐",
                "",
                null);  //通知

            name = "一维战士";
            rt = new rainbow_text(name);
            vstore_add(new VM_APP(name, new app_ticker(vm_g3_tick),
                new app_initer(vm_g3_init), new app_elem_initer(vm_g3_elem_init)), 1000,
                "游戏 / 战斗 / 卡牌 / 放置",
                "",
                null);  //game 3

            foreach(KeyValuePair<VM_APP, double> kp in vstore_apps)
            {
                kp.Key.initer();
            }
        }

        public void vstore_add(VM_APP a, double cost, string type, 
            string d, rainbow_text d2, string author = "skycoop")
        {
            vstore_apps.Add(a, cost);
            vstore_list.Add(a.name);
            a.type = type;
            a.des = d;
            a.des2 = d2;
            a.author = author;
        }
        #endregion

        public void vm_vfs_init()
        {

        }

        public void vm_valarm_init()
        {

        }

        public void vm_vtask_init()
        {

        }

        public void vm_vupdate_init()
        {

        }

        public void vm_g1_init()
        {
            g1_init();
        }

        public void vm_g2_init()
        {

        }

        public void vm_se_init()
        {

        }

        public void vm_lock_init()
        {

        }

        public void vm_mail_init()
        {

        }

        public void vm_g3_init()
        {

        }
    }
}
