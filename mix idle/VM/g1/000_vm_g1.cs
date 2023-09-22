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
        [Serializable]
        public class g1_layer
        {
            public string name;
            public List<resource> resources = new List<resource>();
            public bool unlocked = false;
            public List<g1_layer> prevs = new List<g1_layer>();
            public List<g1_layer> nexts = new List<g1_layer>();

            public Dictionary<string, g1_layer> lines = new Dictionary<string, g1_layer>();

            public bool glowing = false;
            public ARGB second_color;
            public ARGB current_line_color;

            public string text;
            public string font_family;
            public double text_size_base;
            public ARGB text_color;

            public ARGB stroke_color;
            public double stroke_t;

            public string target;
            public ARGB cicrle_color;
            public double size;

            public bool dock;
            public dock_information d_information = new dock_information();
            public Point c_position;

            public ARGB line_color;
            public double line_thickness;

            public List<g1_drawable> drawables = new List<g1_drawable>();
            public Dictionary<g1_upgrade, g1_tab> upgrades
                = new Dictionary<g1_upgrade, g1_tab>();
            public Dictionary<string, g1_tab> tabs = new Dictionary<string, g1_tab>();
            public g1_tab curr_tab = null;
            [NonSerialized]
            public List<Rectangle> group;

            public g1_layer(string name, g1_resource resource)
            {
                this.name = name;
                resources.Add(resource);
            }

            public void add_upgrade(g1_upgrade u, string tab, int page)
            {
                g1_tab t = tabs[tab];
                upgrades.Add(u, t);
                u.page = page;
                u.tab = t;
            }

            public void unlock_upgrade(string name)
            {
                g1_upgrade u = find_upgrade(name);
                g1_tab t = u.tab;
                t.page_max = Math.Max(u.page, t.page_max);
                u.visitable = true;
            }

            public g1_upgrade find_upgrade(string name)
            {
                foreach(KeyValuePair<g1_upgrade, g1_tab> u in upgrades)
                {
                    if(u.Key.name == name)
                    {
                        return u.Key;
                    }
                }
                return null;
            }

            public g1_drawable find_drawable(string name)
            {
                foreach (g1_drawable d in drawables)
                {
                    if (d.name == name)
                    {
                        return d;
                    }
                }
                return null;
            }

            public void prev(g1_layer l)
            {
                l.nexts.Add(this);
                prevs.Add(l);
            }

            public void next(g1_layer l)
            {
                nexts.Add(l);
                l.prevs.Add(this);
            }

            public void prepare(string TARGET, Point pos,
                ARGB text_c, string TEXT, string family, double t_size_base,
                ARGB cicrle_c, double SIZE,
                ARGB line_c, double line_thick,
                ARGB stroke_c, double stroke_thick,
                bool doc = false, dock_information d = null)
            {
                target = TARGET;
                c_position = pos;

                text_color = text_c;
                text = TEXT;
                font_family = family;
                text_size_base = t_size_base;

                cicrle_color = cicrle_c;
                size = SIZE;

                line_color = line_c;
                line_thickness = line_thick;
                current_line_color = line_c;

                stroke_color = stroke_c;
                second_color = new ARGB(stroke_c.a,
                    (byte)(stroke_c.r),
                    (byte)(stroke_c.g + 128),
                    (byte)(stroke_c.b + 128));
                stroke_t = stroke_thick;

                dock = doc;
                d_information = d;
            }

            public void reset()
            {
                foreach (KeyValuePair<g1_upgrade, g1_tab> kp in
                    upgrades)
                {
                    kp.Key.reset();
                }
                foreach (g1_resource r in resources)
                {
                    r.reset();
                }
            }
        }
        Dictionary<string, g1_layer> g1_layers = new Dictionary<string, g1_layer>();
        g1_layer g1_current_layer;

        [Serializable]
        public class dock_information
        {
            public int direction;
            public double distance;
        }

        [Serializable]
        public class g1_resource : resource
        {

            public g1_resource(double2 VAL, string NAME, SolidColorBrush COLOR)
                : base(0, VAL, NAME, COLOR)
            {
            }
        }
        Dictionary<string, g1_resource> g1_res = new Dictionary<string, g1_resource>();

        [Serializable]
        public class g1_achievement : achievement
        {
            public g1_achievement(string nam, string des_ = "")
                : base(nam, des_)
            {
            }
        }
        Dictionary<string, g1_achievement> g1_achieves = new Dictionary<string, g1_achievement>();

        [Serializable]
        public class g1_level
        {
            public enum type
            {
                easy = 0, normal = 1, hard = 2
            }

            public double2 speed = 1;

            public string name;
            //public g1_resource main;
            public List<resource> resources = new List<resource>();
            public List<g1_layer> roots = new List<g1_layer>();
            public List<g1_scenery> sceneries = new List<g1_scenery>();

            public ARGB color;

            public List<g1_level> prevs = new List<g1_level>();
            public List<g1_level> nexts = new List<g1_level>();
            public g1_layer watching_layer = null;

            public double2 in_game_time = 0;
            public double2 active_time = 0;
            public double2 real_time = 0;
            public bool started = false;
            public type difficulty = type.normal;

            public bool size_change = false;
            public int stage = 1;
            public int stage_max = 1;

            public Point view_point = new Point(0, 0);

            public g1_level(string NAME, g1_resource MAIN, ARGB bg_color)
            {
                name = NAME;
                resources.Add(MAIN);
                color = bg_color;
            }

            public void start(double2 active, double2 real)
            {
                in_game_time = 0;
                active_time = 0;
                real_time = 0;
                started = true;
            }

            public void end()
            {
                started = false;
            }

            public void prev(g1_level l)
            {
                l.nexts.Add(this);
                prevs.Add(l);
            }

            public g1_layer find_layer(string name)
            {
                foreach(g1_layer l in getAllLayers())
                {
                    if(l.name == name)
                    {
                        return l;
                    }
                }
                return null;
            }

            public Queue<g1_layer> getAllLayers()
            {
                Dictionary<string, int> set = new Dictionary<string, int>();
                Queue<g1_layer> ret = new Queue<g1_layer>();
                Queue<g1_layer> q = new Queue<g1_layer>();
                foreach (g1_layer l in roots)
                {
                    q.Enqueue(l);
                }
                while (q.Count > 0)
                {
                    g1_layer layer = q.Dequeue();
                    if (!set.ContainsKey(layer.name))
                    {
                        set[layer.name] = 1;
                    }
                    else
                    {
                        continue;
                    }
                    foreach (g1_layer n in layer.nexts)
                    {
                        q.Enqueue(n);
                    }
                    ret.Enqueue(layer);
                }
                return ret;
            }

            public List<g1_upgrade> GetAllUpgrades()
            {
                List<g1_upgrade> ret = new List<g1_upgrade>();
                Queue<g1_layer> q = getAllLayers();
                while (q.Count > 0)
                {
                    g1_layer layer = q.Dequeue();
                    foreach(KeyValuePair<g1_upgrade, g1_tab> u in layer.upgrades)
                    {
                        ret.Add(u.Key);
                    }
                    foreach(g1_layer n in layer.nexts)
                    {
                        q.Enqueue(n);
                    }
                }
                return ret;
            }

            public g1_scenery find_scenery(string name)
            {
                foreach(g1_scenery s in sceneries)
                {
                    if(s.name == name)
                    {
                        return s;
                    }
                }
                return null;
            }

            public void clear()
            {
                resources.Clear();
                roots.Clear();
                sceneries.Clear();
            }
        }
        Dictionary<string, g1_level> g1_levels = new Dictionary<string, g1_level>();

        public void g1_init()
        {
            yggdrasill.reseter = new Yggdrasill();
            g1_levels_init();
        }

        [Serializable]
        public enum g1_show_mode
        {
            normal = 0,
            right = 1,
        }
        g1_show_mode g1_mode = g1_show_mode.normal;

        [Serializable]
        public class g1_scenery
        {
            public string name;
            public string target;
            public bool unlocked = false;
            public bool changed = true;
        }




        [Serializable]
        public class curve : g1_scenery
        {
            public List<Point> points = new List<Point>();
            public double tension;
            public bool draw_point;
            public bool closed = false;

            public ARGB f_color;
            public ARGB s_color;
            public double thickness;

            public curve(List<Point> Points, double Tension,
                ARGB S_Color, double Thickness, bool Closed = false,
                ARGB F_Color = null)
            {
                points = Points;
                tension = Tension;
                s_color = S_Color;
                thickness = Thickness;
                closed = Closed;
                f_color = F_Color;
            }
        }

        [Serializable]
        public class blocks : g1_scenery
        {
                     //fill stroke        width   height  angle
            public List<Tuple<ARGB, ARGB, Point, double, double, double>> b = 
                new List<Tuple<ARGB, ARGB, Point, double, double, double>>();
        }

        [Serializable]
        public class g1_tab
        {
            public string name;
            public bool unlocked = false;
            public drawable tab;
            public int page_now = 1;
            public int page_max = 1;

            public bool glowing = false;
            public ARGB second_color;
            public ARGB save_color;

            public List<g1_upgrade> attach = new List<g1_upgrade>();

            public solid_type c_active;

            public g1_tab(string NAME, string TARGET, 
                double W, double H, thickness T,
                solid_type s_fill, solid_type s_active,
                solid_type s_stroke, double t_stroke,
                solid_text s_content, bool UNLOCK = false)
            {
                name = NAME;
                tab = new drawable(NAME, TARGET, HorizontalAlignment.Left,
                    VerticalAlignment.Top, W, H, T);
                tab.setFill(s_fill);
                c_active = s_active;
                tab.setStroke(s_stroke, t_stroke);
                drawable.setCRIT(ref tab.t_stroke);

                save_color = s_stroke.color;
                second_color = new ARGB(save_color.a,
                    (byte)(save_color.r),
                    (byte)(save_color.g + 128),
                    (byte)(save_color.b + 128));

                tab.setContent_s(s_content);
                tab.clickable = drawable.click.left;
                tab.grouping = true;

                unlocked = UNLOCK;
            }
        }

        [Serializable]
        public class g1_milestone : g1_upgrade
        {
            public drawable draw;
            public bool completed = false;
            public bool log_show = false;

            public g1_milestone(string NAME, g1_level level, g1_layer layer)
                : base(NAME, level, layer)
            {
            }

            public void prepare(string NAME, string TARGET,
                HorizontalAlignment HA, VerticalAlignment VA,
                double W, double H, thickness T,
                solid_type S_FILL, solid_type S_PROGRESS,
                solid_type S_STORKE, double stroke_thickness,
                solid_text S_TITLE, rainbow_text R_CONTENT,
                solid_type S_MASK)
            {
                draw = new drawable(NAME, TARGET, HA, VA, W, H, T);
                draw.setFill(S_FILL);
                draw.setProgress(S_PROGRESS);
                draw.setStroke(S_STORKE, stroke_thickness);
                draw.setTitle_s(S_TITLE);
                draw.setContent_r(R_CONTENT);
                draw.setMask(S_MASK);
                drawable.setCRIT(ref draw.t_fill);
                drawable.setCRIT(ref draw.t_progress);
                drawable.setCRIT(ref draw.t_title);
                drawable.setCRIT(ref draw.t_content);
            }
        }

        [Serializable]
        public class g1_drawable : drawable
        {
            public object container;

            public g1_level level;
            public g1_layer layer;
            public g1_tab tab;
            public int page;

            public bool attaching = false;

            public Visibility v = Visibility.Visible;

            public g1_drawable(string NAME, g1_level LEVEL, g1_layer LAYER,
                g1_tab TAB, int PAGE, string TARGET,
                HorizontalAlignment HA, VerticalAlignment VA,
                double W, double H, thickness T)
                : base(NAME, TARGET, HA, VA, W, H, T)
            {
                LAYER.drawables.Add(this);
                level = LEVEL;
                layer = LAYER;
                tab = TAB;
                page = PAGE;
                if(tab.page_max < page)
                {
                    tab.page_max = page;
                }
            }
        }

        [Serializable]
        public class g1_save_slot
        {
            public g1_drawable draw;
            public bool have = false;

            public g1_save_slot(g1_drawable g)
            {
                draw = g;
                g.container = this;
            }
        }

        

        [Serializable]
        public class g1_research : g1_upgrade
        {
            public EXP_BAR bar;
            public add_exp xp;
            public bool is_reseter = false;

            public resource r;
            public double2 xp_gain_expon;
            public double2 xp_gain_exp_factor;

            public gamestats gs;

            public g1_research(string name, g1_level Level, g1_layer Layer,
                EXP_BAR b, gamestats gs, resource res,
                double2 xp_base, double2 xp_a, double2 xp_gain_exponment)
                : base(name, Level, Layer)
            {
                check = false;
                bar = b;
                auto_cost = true;
                this.gs = gs;
                r = res;
                xp = new add_exp(name, xp_base, xp_a);
                xp_gain_expon = xp_gain_exponment;
                if (!is_reseter)
                {
                    can_reset = true;
                    set_init_cost(gs.get_auto_cost_table(res.name), 0, -1);
                }
            }

            public double2 will()
            {
                double2 w = double2.Max(see(), 1);
                if (r.get_value() < w)
                {
                    w = r.get_value();
                }
                return w;
            }

            public void put(double2 amount)
            {
                r.store_to_pool(name, amount);
            }

            public void putALL()
            {
                r.store_to_pool(name, double2.max);
            }

            public double2 see()
            {
                return r.see_pool(name);
            }

            public void tick(double2 t)
            {
                double2 sum = double2.Pow(see(), xp_gain_expon);
                xp.gain_exp(sum * xp.get_exp_mul() * t * gs.global_xp_boost(), 
                    double2.max);
            }

            public new void reset()
            {
                xp.reset();
                r.restore_from_pool(name, double2.max);
            }
        }
    }
}
