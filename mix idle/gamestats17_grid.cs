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
        public class game_grid_element
        {
            public bool selectable = true;

            public double progress = 0;
            public int type = 0;
            public int special = 0;

            public double progress_delay = 0;
            public int type_delay = 0;
            public int special_delay = 0;

            public drawable draw;
            public int r;
            public int c;

            public game_grid_element(drawable d)
            {
                draw = d;
            }
        }

        [Serializable]
        public class game_grid : g1_drawable
        {
            public int row()
            {
                return elems.GetLength(0);
            }
            public int col()
            {
                return elems.GetLength(1);
            }

            public int type_max = 0;

            public List<game_grid_element> select_chain =
                new List<game_grid_element>();
            public List<game_grid_element> growing =
                new List<game_grid_element>();
            public List<game_grid_element> removing =
                new List<game_grid_element>();
            public game_grid_element[,] elems;
            public int count = 0;

            public game_grid(game_grid_element[,] elements,
                string name, g1_level LEVEL, g1_layer LAYER,
                g1_tab TAB, int PAGE, string target, HorizontalAlignment ha,
                VerticalAlignment va, double width, double height,
                thickness t)
                : base(name, LEVEL, LAYER, TAB, PAGE, target, 
                      ha, va, width, height, t)
            {
                LAYER.drawables.RemoveAt(LAYER.drawables.Count - 1);
                LAYER.drawables.Add(this);
                elems = elements;
            }

            public bool valid(int r, int c)
            {
                if(r >= 0 && r < row() && c >= 0 && c < col())
                {
                    return true;
                }
                return false;
            }

            public List<game_grid_element> surround(int r, int c, int distance,
                bool oblique = false, bool max_distance = false)
            {
                List<game_grid_element> ret = new List<game_grid_element>();
                for(int dr = -distance; dr <= distance; dr++)
                {
                    for (int dc = -distance; dc <= distance; dc++)
                    {
                        if (dr == 0 && dc == 0)
                        {
                            continue;
                        }
                        int dis = Math.Abs(dr) + Math.Abs(dc);
                        bool b = false;
                        if (max_distance)
                        {
                            b = oblique || dis == distance;
                            if (oblique)
                            {
                                if(Math.Abs(dr) < distance &&
                                   Math.Abs(dc) < distance)
                                {
                                    b = false;
                                }
                            }
                        }
                        else
                        {
                            b = oblique || dis <= distance;
                        }
                        if (b)
                        {
                            int nr = r + dr;
                            int nc = c + dc;
                            if (valid(nr, nc))
                            {
                                ret.Add(elems[nr, nc]);
                            }
                        }
                    }
                }
                return ret;
            }

            public bool[,] same_search(int r, int c, 
                bool oblique = false)
            {
                int type = elems[r, c].type;
                bool[,] b = new bool[row(), col()];
                for(int i = 0; i < row(); i++)
                {
                    for(int j = 0; j < col(); j++)
                    {
                        b[i, j] = false;
                    }
                }
                Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
                queue.Enqueue(new Tuple<int, int>(r, c));
                while(queue.Count != 0)
                {
                    Tuple<int, int> t = queue.Dequeue();
                    b[t.Item1, t.Item2] = true;
                    for (int i = -1; i <= 1; i++)
                    {
                        int nr = t.Item1 + i;
                        if (nr < 0 || nr >= row())
                        {
                            continue;
                        }
                        for (int j = -1; j <= 1; j++)
                        {
                            int nc = t.Item2 + j;
                            if (nc < 0 || nc >= col())
                            {
                                continue;
                            }
                            if (b[nr, nc] == true)
                            {
                                continue;
                            }
                            if (oblique || i == 0 || j == 0)
                            {
                                if(elems[nr, nc].progress == 1 &&
                                   elems[nr, nc].type == type)
                                {
                                    queue.Enqueue(new Tuple<int, int>(nr, nc));
                                }
                            }
                        }
                    }
                }
                return b;
            }

            public List<Tuple<Point, Point>> match_X_line(int r, int c, int n = 3,
                bool oblique = false)
            {
                bool[,] same = same_search(r, c, oblique);
                List<Tuple<Point, Point>> ret = new List<Tuple<Point, Point>>();
                bool[,] vb = new bool[row(), col()];
                bool[,] hb = new bool[row(), col()];
                for (int i = 0; i < row(); i++)
                {
                    for (int j = 0; j < col(); j++)
                    {
                        vb[i, j] = false;
                        hb[i, j] = false;
                    }
                }
                for (int i = 0; i < row(); i++)
                {
                    for (int j = 0; j < col(); j++)
                    {
                        if(same[i, j] == true)
                        {
                            if (vb[i, j] == false)
                            {
                                vb[i, j] = true;
                                int di = 1;
                                for (; i + di < row(); di++)
                                {
                                    vb[i + di, j] = true;
                                    if (!same[i + di, j])
                                    {
                                        break;
                                    }
                                }
                                if (di >= n)
                                {
                                    ret.Add(new Tuple<Point, Point>(
                                        new Point(i, j),
                                        new Point(i + di - 1, j)));
                                }
                            }
                            if (hb[i, j] == false)
                            {
                                hb[i, j] = true;
                                int dj = 1;
                                for (; j + dj < col(); dj++)
                                {
                                    hb[i, j + dj] = true;
                                    if (!same[i, j + dj])
                                    {
                                        break;
                                    }
                                }
                                if (dj >= n)
                                {
                                    ret.Add(new Tuple<Point, Point>(
                                        new Point(i, j),
                                        new Point(i, j + dj - 1)));
                                }
                            }
                        }
                    }
                }
                return ret;
            }

            public List<Point> match_X_list(List<Tuple<Point, Point>> t)
            {
                List<Point> ret = new List<Point>();
                bool[,] b = new bool[row(), col()];
                for (int i = 0; i < row(); i++)
                {
                    for (int j = 0; j < col(); j++)
                    {
                        b[i, j] = false;
                    }
                }
                foreach (Tuple<Point, Point> tuple in t)
                {
                    Point start = tuple.Item1;
                    Point end = tuple.Item2;
                    int dr = (int)(end.X - start.X);
                    int dc = (int)(end.Y - start.Y);
                    if (dr > 0)
                    {
                        for(int i = 0; i <= dr; i++)
                        {
                            if (b[(int)start.X + i, (int)start.Y] == false)
                            {
                                b[(int)start.X + i, (int)start.Y] = true;
                                ret.Add(new Point(start.X + i, start.Y));
                            }
                        }
                    }
                    if (dc > 0)
                    {
                        for (int i = 0; i <= dc; i++)
                        {
                            if (b[(int)start.X, (int)start.Y + i] == false)
                            {
                                b[(int)start.X, (int)start.Y + i] = true;
                                ret.Add(new Point(start.X, start.Y + i));
                            }
                        }
                    }
                }
                return ret;
            }
        }
        public Dictionary<string, game_grid> game_grids = 
            new Dictionary<string, game_grid>();

        public bool m3_check(int er, int ec)
        {
            game_grid m3 = game_grids["m3"];
            bool ret = false;

            if (m3.removing.Contains(m3.elems[er, ec]))
            {
                return false;
            }

            List<Tuple<Point, Point>> lines =
                m3.match_X_line(er, ec);

            Dictionary<int, int> scores = new Dictionary<int, int>();
            for(int i = 3; i <= Math.Max(m3.row(), m3.col()); i++)
            {
                scores[i] = 0;
            }
            foreach (Tuple<Point, Point> tuple in lines)
            {
                Point start = tuple.Item1;
                Point end = tuple.Item2;
                int dr = (int)(end.X - start.X);
                int dc = (int)(end.Y - start.Y);
                if (dr > 0)
                {
                    scores[dr + 1]++;
                }
                if (dc > 0)
                {
                    scores[dc + 1]++;
                }
            }

            List<Point> points = m3.match_X_list(lines);
            foreach (Point p in points)
            {
                int r = (int)p.X;
                int c = (int)p.Y;
                m3.elems[r, c].type_delay = 0;
                m3.elems[r, c].progress_delay = 0;
                m3.elems[r, c].special_delay = 0;
                m3_explode(r, c);
                m3.removing.Add(m3.elems[r, c]);
                ret = true;
            }

            if (ret)
            {
                int k = 1;
                for (int i = 3; i < Math.Max(m3.row(), m3.col()); i++)
                {
                    m3.elems[er, ec].special_delay += scores[i] * k;
                    k *= 10;
                }
                if (m3.elems[er, ec].special_delay == 1)
                {
                    m3.elems[er, ec].special_delay = 0;
                }
                if (m3.elems[er, ec].special_delay > 0)
                {
                    m3.elems[er, ec].type_delay = m3.elems[er, ec].type;
                    m3.elems[er, ec].progress_delay = 1;
                }

                
            }
            return ret;
        }


        public void m3_explode(int er, int ec)
        {
            game_grid m3 = game_grids["m3"];

            game_grid_element e = m3.elems[er, ec];
            int special = e.special;
            if (m3.select_chain.Contains(e))
            {
                game_key_name("fake_m3button__" + er + "_" + ec);
            }
            if (m3.removing.Contains(e))
            {
                return;
            }
            if (special < 10)
            {
                return;
            }
            m3.removing.Add(e);
            e.type_delay = 0;
            e.progress_delay = 0;
            e.special_delay = 0;

            Dictionary<int, int> scores = new Dictionary<int, int>();
            int k = 1;
            for (int i = 3; i <= Math.Max(m3.row(), m3.col()); i++)
            {
                scores[i] = special % (k * 10) / k;
                k *= 10;
            }
            List<game_grid_element> impact = new List<game_grid_element>();
            if (scores[5] == 0)
            {
                impact = m3.surround(er, ec, scores[4]);
                foreach(game_grid_element f in impact)
                {
                    f.type_delay = 0;
                    f.progress_delay = 0;
                    f.special_delay = 0;
                    m3_explode(f.r, f.c);
                    m3.removing.Add(f);
                }
            }
            else
            {
                impact = m3.surround(er, ec, scores[5], false, true);
                int old_type = e.type;
                e.type++;
                if (e.type > m3.type_max)
                {
                    e.type = 1;
                }
                foreach (game_grid_element f in impact)
                {
                    if(f.type != 0)
                    {
                        f.type = old_type;
                        f.special += scores[4] * 10;
                        f.progress = 0;
                        m3.growing.Add(f);
                        m3_check(f.r, f.c);
                    }
                }
                e.special -= scores[4] * 10;
                m3.removing.Remove(e);
            }
        }
    }
}
