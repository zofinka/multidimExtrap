using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approx
{
    public interface IAnalyzer
    {
        void do_random_forest_analyse(IMLAlgorithm rg, Func<double[], IFunction, Grid, double[], double[][], int, double[]> build_features);
        void do_random_forest_analyse(IMLAlgorithm cls, double allowErr, Func<double[], double> meFunc, Func<double[], double[]> calcDerivative);
        double[][] Result { get; }
    }

    public class Analyzer : IAnalyzer
    {
        IFunction func;
        double[][] xf;
        double[][] xfcandidates;
        Grid grid;
        int[] domain;
        double[] dist;
        int[][] graph;
        double[] borderdist;
        int[] bordernear;
        double[] error;
        int[] candidates;
        static int counter = 0;

        const int NGRID = 100;

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

        public Analyzer(IFunction func, double[][] xf)
        {
            this.func = func;
            this.xf = xf;

            int[] count = new int[N];
            for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
        }

        public void do_default_analyse()
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
            analyse_error();
        }

        public void do_some_analyse()
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
        }

        public int getGoodPr(Func<double[], double[]> meFunc, float allowErr)
        {
            int prGoodRes = 0;
            int n = grid.Node.Length;
            for (int i = 0; i < n; i++)
            {
                double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);

                double[] approxFunctionVal = new double[this.M];
                for (int k = 0; k < this.M; ++k)
                {
                    approxFunctionVal[k] = cuurentNode[this.N + k];
                }

                var realFunctionVal = meFunc(grid.Node[i]);

                double[] diffs = realFunctionVal.Zip(approxFunctionVal, (d1, d2) => Math.Abs(d1 - d2)).ToArray();

                double err = (diffs.Sum() / diffs.Length);

                if (err < allowErr)
                {
                    prGoodRes += 1;
                }
            }
            return prGoodRes * 100 / n;
        }

        public void do_default_analyse(int[] count)
        {
            create_grid(count);
            analyse_voronoi();
            analyse_error();
        }

        public void do_random_forest_analyse(IMLAlgorithm rg, Func<double[], IFunction, Grid, double[], double[][], int, double[]> build_features)
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();

            //int n = candidates.Length;
            int n = grid.Node.Length;
            LabeledData[] ldata = new LabeledData[n];
            
            for (int i = 0; i < grid.Node.Length; i++)
            {
                ldata[i] = new LabeledData(build_features(grid.Node[i], this.func, grid, this.dist, xf, i), 0);
            }

            candidates = new int[grid.Node.Length];

            List<Tuple<double, int>> improveDiffs = new List<Tuple<double, int>>();

            for (int i = 0; i < grid.Node.Length; ++i)
            {
                Object dist;
                rg.infer(ldata[i].data, out dist);
                improveDiffs.Add(new Tuple<double, int>(Convert.ToDouble(dist), i));
            }

            var sortedImproveDiffs = improveDiffs.OrderByDescending((t) => t.Item1).ToList();

            for (int i = 0; i < grid.Node.Length; i++)
            {
                candidates[i] = sortedImproveDiffs[i].Item2;
            }

            xfcandidates = Tools.Sub(grid.Node, candidates);
        }

        public void do_random_forest_analyse(IMLAlgorithm cls, double allowErr, Func<double[], double> meFunc, Func<double[], double[]> calcDerivative)
        {
            int[] count = new int[N]; for (int i = 0; i < N; i++) count[i] = (Min[i] == Max[i]) ? 1 : NGRID;
            create_grid(count);
            analyse_voronoi();
            analyse_all_error();

            int n = grid.Node.Length;
            LabeledData[] ldata = new LabeledData[n];
            LabeledData[] ldata1 = new LabeledData[n];
            int featureCount = 0;
            for (int i = 0; i < n; i++)
            {
                // min, max in locality
                double maxNeighbours = double.MinValue;
                double minNeighbours = double.MaxValue;
                foreach (var neighbour in grid.Neighbours(i))
                //foreach (var neighbour in grid.Neighbours(i))
                {
                    double[] calcNeighbour = (double[])grid.Node[neighbour].Clone();
                    this.func.Calculate(calcNeighbour);
                    if (calcNeighbour[calcNeighbour.Length - 1] < minNeighbours)
                    {
                        minNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                    }
                    if (calcNeighbour[calcNeighbour.Length - 1] > maxNeighbours)
                    {
                        maxNeighbours = calcNeighbour[calcNeighbour.Length - 1];
                    }
                }
                // current val
                double[] cuurentNode = (double[])grid.Node[i].Clone();
                this.func.Calculate(cuurentNode);
                double cuurentNodeVal = cuurentNode[cuurentNode.Length - 1];
                if (cuurentNodeVal < minNeighbours)
                {
                    minNeighbours = cuurentNodeVal;
                }
                if (cuurentNodeVal > maxNeighbours)
                {
                    maxNeighbours = cuurentNodeVal;
                }

                // is real function and approximation are equal, class for point
                int pointClass = 0;
                if (Math.Abs(meFunc(grid.Node[i]) - cuurentNodeVal) > allowErr)
                {
                    pointClass = 1;
                }

                //derivative 
                double[] derivative = calcDerivative(grid.Node[i]);

                // build features vector
                double[] features = new double[2];
                features[0] = Math.Abs(cuurentNodeVal - maxNeighbours);
                features[1] = Math.Abs(minNeighbours - cuurentNodeVal);

                ldata[i] = new LabeledData(features, 0);
                ldata1[i] = new LabeledData(features, pointClass);
                featureCount = features.Length;
            }
            List<int> newCandidates = new List<int>();
            Object[] y = new Object[ldata.Length];
            for (int i = 0; i < ldata.Length; i++)
            {
                cls.infer(ldata[i].data, out y[i]);
                if ((int)y[i] == 1)
                {
                    newCandidates.Add(i);
                }
            }
            candidates = newCandidates.ToArray();

            xfcandidates = Tools.Sub(grid.Node, candidates);
        }
        
        public Analyzer(IFunction func, double[][] xf, int[] count)
        {
            this.func = func;
            this.xf = xf;
        }

        public void create_grid(int[] count)
        {
            grid = new Grid(N, M, Min, Max, count);
        }

        private void analyse_voronoi()
        {
            //вычисляю принадлежность узлов сетки доменам (графовый алгоритм на базе структуры уровней смежности)
            SortedSet<int>[] adjncy = new SortedSet<int>[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                adjncy[i] = new SortedSet<int>();

            //Console.WriteLine("Partition of the space into domains");
            Queue<int> queue = new Queue<int>();
            domain = new int[grid.Node.Length];
            dist = new double[grid.Node.Length];
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
                //Console.WriteLine("index " + index + "domain " + domain[index] + " dist " + dist[index] );
                queue.Enqueue(index);
            }
            Analyzer.counter++;
            while (queue.Count > 0)
            {
                int index = queue.Dequeue();
                int i = domain[index];
                //Console.WriteLine("index " + index + "domain " + domain[index]);
                foreach (var adj in grid.Neighbours(index))
                {
                    double d = distanceX(grid.Node[adj], xf[i]);
                    // Console.WriteLine("adj " + adj + "distanceX(grid.Node[adj], xf[i]) " + d);
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
                    //Console.WriteLine("adj " + adj + "domain " + domain[adj] + " dist " + dist[adj]);
                    //setvalue(adj, xf[i]);
                    queue.Enqueue(adj);
                }
            }

            //Console.WriteLine("dist " + String.Join(", ", dist));
            //Console.WriteLine("domen " + String.Join(", ", domain));

            //Console.WriteLine("Building a domain graph");
            //строю граф соседства доменов
            graph = new int[xf.Length][];
            for (int i = 0; i < xf.Length; i++)
            {
                adjncy[i].Add(i);
                graph[i] = adjncy[i].ToArray();
                // Console.WriteLine("dist " + String.Join(", ", adjncy[i]));
            }

            //Console.WriteLine("Building a Voronoi diagram on a grid");
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

            //Console.WriteLine("Domain Border Calculation");
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

            //Console.WriteLine("Construction of the function of distances to domain boundaries");
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

            //Console.WriteLine("Normalization of the function of distances to domain boundaries");
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

        private double square_area(double[] x)
        {
            double res = 1;
            for (int i = 0; i < x.Length - 1; i++)
            {
                res = res * x[i];
            }
            return res;
        }

        private void analyse_error()
        {
            Console.WriteLine("Calculation of local extrapolants");
            //вычисляю экстраполянты
            //Shepard[] sh = new Shepard[xf.Length];
            //MeasuredPoint[] xfMeasured = MeasuredPoint.getArrayFromDouble(xf, N);
            ShepardApprox[] shepardApprox = new ShepardApprox[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                shepardApprox[i] = new ShepardApprox(N, xf, graph[i]);

            Console.WriteLine("Approximation of the values of the initial function at the domain boundaries");
            //пересчитываю значения в узлах решетки только на границах доменов
            for (int i = 0; i < grid.Node.Length; i++)
            {
                if (borderdist[i] > 0) continue;
                shepardApprox[domain[i]].Calculate(grid.Node[i]);
            }

            Console.WriteLine("Calculation of the approximation error of a function at domain boundaries");
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

            Console.WriteLine("Interpolation of approximation error function on domain domains");
            //интерполирую ошибку
            double max = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int brd = bordernear[i];
                double err = error[brd];
                error[i] = err * (1 - borderdist[i]);
                if (max < error[i]) max = error[i];
            }

            Console.WriteLine("Error Function Normalization");
            //нормирую ошибку
            if (max > 0)
                for (int i = 0; i < grid.Node.Length; i++)
                    error[i] = error[i] / max;

            int maxcandidates = Math.Min(candidates.Length, 1000);
            if (candidates.Length > maxcandidates)
                Console.WriteLine("Preliminary selection of {1} of {0} candidates", candidates.Length, maxcandidates);

            Console.WriteLine("Ordering {0} potential candidates", maxcandidates);
            //сортировка потенциальных кандидатов
            for (int i = 0; i < maxcandidates - 1; i++)
                for (int j = i + 1; j < candidates.Length; j++)
                    if (error[candidates[i]] < error[candidates[j]])
                    {
                        int temp = candidates[i];
                        candidates[i] = candidates[j];
                        candidates[j] = temp;
                    }

            Console.WriteLine("Creating a list of candidates", maxcandidates);
            candidates = Tools.Sub(candidates, 0, maxcandidates);
            xfcandidates = Tools.Sub(grid.Node, candidates);
            for (int i = 0; i < xfcandidates.Length; i++)
            {
                for (int j = N; j < N + M; j++) xfcandidates[i][j] = 0;
            }
            Console.WriteLine("Calculation process completed successfully");
        }

        private void analyse_all_error()
        {
            //вычисляю экстраполянты
            //Shepard[] sh = new Shepard[xf.Length];
            //MeasuredPoint[] xfMeasured = MeasuredPoint.getArrayFromDouble(xf, N);
            ShepardApprox[] shepardApprox = new ShepardApprox[xf.Length];
            for (int i = 0; i < xf.Length; i++)
                shepardApprox[i] = new ShepardApprox(N, xf, graph[i]);

            //пересчитываю значения в узлах решетки только на границах доменов
            for (int i = 0; i < grid.Node.Length; i++)
            {
                shepardApprox[domain[i]].Calculate(grid.Node[i]);
            }

            //вычисляю ошибку на границах доменов
            error = new double[grid.Node.Length];
            for (int i = 0; i < grid.Node.Length; i++)
            {
                error[i] = 0;
                foreach (var adj in grid.Neighbours(i))
                {
                    double d = distanceF(grid.Node[i], grid.Node[adj]);
                    if (error[i] < d) error[i] = d;
                }
            }

            //интерполирую ошибку
            double max = 0;
            for (int i = 0; i < grid.Node.Length; i++)
            {
                int brd = bordernear[i];
                double err = error[brd];
                error[i] = err * (1 - borderdist[i]);
                if (max < error[i]) max = error[i];
            }

            //нормирую ошибку
            if (max > 0)
                for (int i = 0; i < grid.Node.Length; i++)
                    error[i] = error[i] / max;

            int maxcandidates = Math.Min(candidates.Length, 1000);

            //сортировка потенциальных кандидатов
            for (int i = 0; i < maxcandidates - 1; i++)
                for (int j = i + 1; j < candidates.Length; j++)
                    if (error[candidates[i]] < error[candidates[j]])
                    {
                        int temp = candidates[i];
                        candidates[i] = candidates[j];
                        candidates[j] = temp;
                    }

            candidates = Tools.Sub(candidates, 0, maxcandidates);
            xfcandidates = Tools.Sub(grid.Node, candidates);
            for (int i = 0; i < xfcandidates.Length; i++)
            {
                for (int j = N; j < N + M; j++) xfcandidates[i][j] = 0;
            }
            Console.WriteLine("Calculation process completed successfully");
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
            for (int i = N; i < N + M; i++) dist += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(dist);
        }


        private void setvalue(int index, double[] value)
        {
            for (int i = N; i < value.Length; i++)
                grid.Node[index][i] = value[i];
        }

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
