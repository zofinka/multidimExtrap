using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Analyzer : IFunction
    {
        IFunction func;
        double[][] xf;
        double[][] xfcandidates;
        Grid grid;
        int[] domain;
        int[][] graph;
        double[] borderdist;
        int[] bordernear;
        double[] error;
        int[] candidates;


        const int NGRID = 100;

        public Analyzer(IFunction func, double[][] xf)
        {
            this.func = func;
            this.xf = xf;

            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;

            create_grid(count);
            analyse_voronoi();
            analyse_error();
        }

        public Analyzer(IFunction func, double[][] xf, int[] count)
        {
            this.func = func;
            this.xf = xf;

            create_grid(count);
            analyse_voronoi();
            analyse_error();
        }

        private void create_grid(int[] count)
        {
            Console.WriteLine("Построение сетки ({0})", count.Str());
            grid = new Grid(N, M, Min, Max, count);
            Console.WriteLine("Сетка на {0} узлах построена успешно", grid.Node.Length);
        }

        private void analyse_voronoi()
        {
            //вычисляю принадлежность узлов сетки доменам (графовый алгоритм на базе структуры уровней смежности)
            SortedSet<int>[] adjncy = new SortedSet<int>[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                adjncy[i] = new SortedSet<int>();

            Console.WriteLine("Разбиение пространства на домены");
            Queue<int> queue = new Queue<int>();
            domain = new int[grid.Node.Length];
            double[] dist = new double[grid.Node.Length];
            for (int i = 0; i < domain.Length; i++)
            {
                domain[i] = -1;
                dist[i] = double.PositiveInfinity;
            }
            for (int i = 0; i < xf.Length; i++)
            {
                int index;
                grid.ToIndex(xf[i], out index);
                dist[index] = distanceX(grid.Node[index], xf[i]);
                domain[index] = i;
                //setvalue(index, xf[i]);
                queue.Enqueue(index);
            }
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                int i = domain[index];
                foreach (var adj in grid.Neighbours(index))
                {
                    double d = distanceX(grid.Node[adj], xf[i]);
                    if (domain[adj] >= 0)
                    {
                        adjncy[domain[adj]].Add(i);
                        adjncy[i].Add(domain[adj]);
                        if (d < dist[adj])
                        {
                            domain[adj] = i;
                            dist[adj] = d;
                        }
                        continue;
                    }
                    domain[adj] = i;
                    dist[adj] = d;
                    //setvalue(adj, xf[i]);
                    queue.Enqueue(adj);
                }
            }

            Console.WriteLine("Построение графа доменов");
            //строю граф соседства доменов
            graph = new int[xf.Length][];
            for (int i = 0; i < xf.Length; i++)
            {
                adjncy[i].Add(i);
                graph[i] = adjncy[i].ToArray();
            }

            Console.WriteLine("Построение диграммы Вороного на сетке");
            //уточняю домены (диаграмма вороного на сетке)
            for (int i = 0; i < grid.Node.Length; i++)
            {
                double[] xy = grid.Node[i];
                int[] adj = graph[domain[i]];
                double min = double.PositiveInfinity;
                for (int j = 0; j < adj.Length; j++)
                {
                    double d = distanceX(xy, xf[adj[j]]);
                    if (min > d) { min = d; domain[i] = adj[j]; }
                }
            }

            Console.WriteLine("Вычисление границ доменов");
            //вычисляю границы доменов
            borderdist = new double[grid.Node.Length];
            bordernear = new int[grid.Node.Length];
            for (int i = 0; i < grid.Node.Length; i++)
            {
                borderdist[i] = double.PositiveInfinity;
                bordernear[i] = -1;
                int dom = domain[i];
                foreach (var adj in grid.Neighbours(i))
                    if (domain[adj] != dom)
                    {
                        borderdist[i] = 0;
                        bordernear[i] = i;
                        queue.Enqueue(i);
                        break;
                    }
            }
            candidates = queue.ToArray();

            Console.WriteLine("Построение функции расстояний до границ доменов");
            //вычисляю расстояния от границ
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                int dom = domain[index];
                int brd = bordernear[index];
                foreach (var adj in grid.Neighbours(index))
                {
                    if (domain[adj] != dom) continue;
                    double d = distanceX(grid.Node[adj], grid.Node[brd]);
                    if (bordernear[adj] >= 0)
                    {
                        if (d < borderdist[adj])
                        {
                            bordernear[adj] = brd;
                            borderdist[adj] = d;
                        }
                        continue;
                    }
                    bordernear[adj] = brd;
                    borderdist[adj] = d;
                    queue.Enqueue(adj);
                }
            }

            Console.WriteLine("Нормировка функции расстояний до границ доменов");
            //нормирую расстояния от границ
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int dom = domain[i];
                int brd = bordernear[i];
                double a = distanceX(grid.Node[i], xf[dom]);
                double b = distanceX(grid.Node[i], grid.Node[brd]);
                double c = a + b;
                borderdist[i] = (c == 0) ? 0 : b / c;
            }
        }

        private void analyse_error()
        {
            Console.WriteLine("Вычисление локальных экстраполянтов");
            //вычисляю экстраполянты
            Shepard[] sh = new Shepard[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                sh[i] = new Shepard(N, xf, graph[i]);

            Console.WriteLine("Аппроксимация значений исходной функции на границах доменов");
            //пересчитываю значения в узлах решетки только на границах доменов
            for (int i = 0; i < grid.Node.Length; i++)
            {
                if (borderdist[i] > 0) continue;
                sh[domain[i]].Calculate(grid.Node[i]);
            }

            Console.WriteLine("Вычисление ошибки аппроксимации функции на границах доменов");
            //вычисляю ошибку на границах доменов
            error = new double[grid.Node.Length];
            for (int i = 0; i < grid.Node.Length; i++)
            {
                if (borderdist[i] > 0) continue;
                error[i] = 0;
                foreach (var adj in grid.Neighbours(i))
                {
                    double d = distanceF(grid.Node[i], grid.Node[adj]);
                    if (error[i] < d) error[i] = d;
                }
            }

            Console.WriteLine("Интерполяция ошибки аппроксимации функции на области доменов");
            //интерполирую ошибку
            double max = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int brd = bordernear[i];
                double err = error[brd];
                error[i] = err * (1 - borderdist[i]);
                if (max < error[i]) max = error[i];
            }

            Console.WriteLine("Нормировка функции ошибки");
            //нормирую ошибку
            if (max > 0) 
                for (int i = 0; i < grid.Node.Length; i++)
                    error[i] = error[i] / max;

            int maxcandidates = Math.Min(candidates.Length, 1000);
            if (candidates.Length > maxcandidates)
                Console.WriteLine("Предварительный отбор {1} из {0} кандидатов", candidates.Length, maxcandidates);

            Console.WriteLine("Упорядочение {0} потенциальных кандидатов", maxcandidates);
            //сортировка потенциальных кандидатов
            for (int i = 0; i < maxcandidates - 1; i++)
                for (int j = i + 1; j < candidates.Length; j++)
                    if (error[candidates[i]] < error[candidates[j]])
                    {
                        int temp = candidates[i];
                        candidates[i] = candidates[j];
                        candidates[j] = temp;
                    }

            Console.WriteLine("Формирование списка кандидатов", maxcandidates);
            candidates = Tools.Sub(candidates, 0, maxcandidates);
            xfcandidates = Tools.Sub(grid.Node, candidates);
            for (int i = 0; i < xfcandidates.Length; i++)
            {
                //func.Calculate(xfcandidates[i]);
                for (int j = N; j < N + M; j++) xfcandidates[i][j] = 0;
            }
            Console.WriteLine("Процесс расчета закончен успешно");
        }

        private double distanceX(double[] a, double[] b)
        {
            double dist = 0;
            for (int i = 0; i < N; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }

        private double distanceF(double[] a, double[] b)
        {
            double dist = 0;
            for (int i = N; i < N+M; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }


        private void setvalue(int index, double[] value)
        {
            for (int i = N; i < value.Length; i++)
                grid.Node[index][i] = value[i];
        }

        public int M { get { return func.M; } }

        public double[] Max { get { return func.Max; } }

        public double[] Min { get { return func.Min; } }

        public int N { get { return func.N; } }

        public void Calculate(double[] xy)
        {
            func.Calculate(xy);
        }

        public IFunction Function { get { return func; } }

        public double[][] Source { get { return xf; } }

        public double[][] Result { get { return xfcandidates; } }
   
        public int Error2Nodes(double err)
        {
            for (int i = 0; i < candidates.Length; i++)
                if (error[candidates[i]] < err) return i + 1;
            return candidates.Length;
        }

        public double Nodes2Error(int nodes)
        {
            if (nodes < 0) nodes = 0;
            if (nodes >= candidates.Length - 1) nodes = candidates.Length - 1;
            return error[candidates[nodes]];
        }

        public int Domain(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            id = domain[id];
            return id;
        }

        public double Border(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            return borderdist[id];
        }

        public int Voronoi(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            int dom = domain[id];
            //if (borderdist[id] > 0) return dom;  //это оптимизация для скорости вычисления (потеря точности)
            int[] adj = graph[dom];
            double min = double.PositiveInfinity;
            for (int i = 0; i < adj.Length; i++)
            {
                double d = distanceX(xy, xf[adj[i]]);
                if (min > d) { min = d; dom = adj[i]; }
            }
            return dom;
        }

        public double Error(double[] xy)
        {
            int id;
            grid.ToIndex(xy, out id);
            return error[id];
        }

        public double Error(double[] xy, int compX, int compY)
        {
            int id;
            grid.ToIndex(xy, out id);
            updateError(xy, compX, compY);
            if (errmax == errmin) return error[id];
            return (error[id] - errmin) / (errmax - errmin);
        }

        double errmin = 0, errmax = 0;
        int errX = -1, errY = -1;
        double[] errXY = null;

        private void updateError(double[] xy, int compX, int compY)
        {
            if (errX == compX && errY == compY && errXY != null)
            {
                bool equal = true;
                for (int i = 0; i < xy.Length; i++)
                {
                    if (i == compX || i == compY) continue;
                    if (xy[i] != errXY[i]) { equal = false; break; }
                }
                if (equal) return;
            }
            int id;
            grid.ToIndex(xy, out id);
            int[] counter = new int[N];
            grid.ToCounter(id, counter);
            counter[compX] = 0;
            counter[compY] = 0;
            int errID;
            grid.ToIndex(counter, out errID);
            errXY = (double[])xy.Clone();
            errX = compX;
            errY = compY;
            errmin = double.PositiveInfinity;
            errmax = 0;
            for (int i = 0; i < grid.count[errX]; i++)
                for (int j = 0; j < grid.count[errY]; j++)
                {
                    double err = error[errID + grid.rount[errX] * i + grid.rount[errY] * j];
                    if (errmin > err) errmin = err;
                    if (errmax < err) errmax = err;
                }
        }
    }
}
